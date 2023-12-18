using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using JachtZ.src.db;
using JachtZ.src.events;

namespace JachtZ.src.pages
{
	/// <summary>
	/// Interaction logic for Boats.xaml
	/// </summary>
	public partial class Boats : Page
	{
		private MainWindow mainWindow = MainWindow.GetInstance();
		private JachtZEntities db = MainWindow.GetJachtZEntities();
		private List<Boat> boats;

		private double scale = 1.0;
		public Boats()
		{
			InitializeComponent();

			boats = db.Boats.ToList();
			loadBoats(boats);
		}

		public Grid createCatergory(String c, String v)
		{
			Grid grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto)});

			Label category = new Label();
			category.Content = c;

			Label value = new Label();
			value.Content = v;
			value.HorizontalAlignment = HorizontalAlignment.Right;

			grid.Margin = new Thickness(10, 0, 10, 0);

			grid.Children.Add(category);
			Grid.SetColumn(category, 0);
			grid.Children.Add(value);
			Grid.SetColumn(value, 1);

			return grid;
		}

		public void loadBoats(List<Boat> list)
		{
			panel.Children.Clear();
			
			for (int i = 0; i < list.Count; i++) 
			{
				Grid grid = new Grid();
				grid.RowDefinitions.Add(new RowDefinition());
				grid.RowDefinitions.Add(new RowDefinition());
				grid.Margin = new Thickness(10, 0, 10, 0);
				grid.Width = 200;
				grid.Height = 300;

				int boatIndex = i;

				grid.MouseDown += (o, e) =>
				{
					mainWindow.frame.Navigate(new EditBoat(list[boatIndex].boat_ID, list[boatIndex].Model, list[boatIndex].BoatType, list[boatIndex].Colour, list[boatIndex].Wood, list[boatIndex].BasePrice));
				};

				Border img = new Border();
				img.Background = Brushes.White;
				grid.Children.Add(img);
				Grid.SetRow(img, 0);

				StackPanel panelDesc = new StackPanel();
				panelDesc.Orientation = Orientation.Vertical;
				panelDesc.Background = new SolidColorBrush(Color.FromRgb(0, 65, 101));

				Label model = new Label();
				model.Content = list[i].Model;
				model.HorizontalAlignment = HorizontalAlignment.Center;
				model.FontSize = 16;

				Label price = new Label();
				price.Content = "₽" + list[i].BasePrice;
				price.HorizontalAlignment = HorizontalAlignment.Right;
				price.Margin = new Thickness(10, 0, 10, 0);

				panelDesc.Children.Add(model);
				panelDesc.Children.Add(createCatergory("Тип", list	[i].BoatType));
				panelDesc.Children.Add(createCatergory("Цвет", list[i].Colour));
				panelDesc.Children.Add(createCatergory("Дерево", list[i].Wood));
				panelDesc.Children.Add(price);

				grid.Children.Add(panelDesc);
				Grid.SetRow(panelDesc, 1);

				panel.Children.Add(grid);
			}
		}

		private void add_boat_MouseDown(object sender, MouseButtonEventArgs e)
		{
			mainWindow.frame.Navigate(new EditBoat());
        }

		private void ZoomInOut_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftCtrl)
				|| Keyboard.IsKeyDown(Key.RightCtrl)
			)
			{
				if (e.Delta > 0)
					if (scale < 4.0)
						scale *= 1.1;

				if (e.Delta < 0)
					if (scale > 0.2)
						scale /= 1.1;

				scrollViewer.LayoutTransform = new ScaleTransform(scale, scale);
			}
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
			{
				scale = 2.0;
				scrollViewer.LayoutTransform = new ScaleTransform(scale, scale);
			}
			else if (Application.Current.MainWindow.WindowState == WindowState.Normal)
			{
				scale = 1.0;
				scrollViewer.LayoutTransform = new ScaleTransform(scale, scale);
			}
		}

		private void search_MouseDown(object sender, MouseButtonEventArgs e)
		{
			string input = search_text.Text;
			string filter = input.Trim().Split(' ')[0].Trim();
			string x = string.Join(" ", input.Trim().Split(' ').Skip(1));

			List<Boat> newBoats = boats.Where(b =>
			{
				if (filter == "Модель:")
					return b.Model.Trim() == x;
				else if (filter == "Тип:")
					return b.BoatType.Trim() == x;
				else if(filter == "Цвет:")
					return b.Colour.Trim() == x;
				else if (filter == "Дерево:")
					return b.Wood.Trim() == x;
				else 
					return false;
			}).ToList();

			loadBoats(newBoats);
		}
	}
}
