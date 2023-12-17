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
			loadBoats();
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

		public void loadBoats()
		{
			for (int i = 0; i < boats.Count; i++) 
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
					mainWindow.frame.Navigate(new EditBoat(boats[boatIndex].Model, boats[boatIndex].BoatType, boats[boatIndex].Colour, boats[boatIndex].Wood, boats[boatIndex].BasePrice));
				};

				Border img = new Border();
				img.Background = Brushes.Black;
				grid.Children.Add(img);
				Grid.SetRow(img, 0);

				StackPanel panelDesc = new StackPanel();
				panelDesc.Orientation = Orientation.Vertical;
				panelDesc.Background = new SolidColorBrush(Color.FromRgb(0, 65, 101));

				Label model = new Label();
				model.Content = boats[i].Model;
				model.HorizontalAlignment = HorizontalAlignment.Center;
				model.FontSize = 16;

				Label price = new Label();
				price.Content = "₽" + boats[i].BasePrice;
				price.HorizontalAlignment = HorizontalAlignment.Right;
				price.Margin = new Thickness(10, 0, 10, 0);

				panelDesc.Children.Add(model);
				panelDesc.Children.Add(createCatergory("Тип", boats[i].BoatType));
				panelDesc.Children.Add(createCatergory("Цвет", boats[i].Colour));
				panelDesc.Children.Add(createCatergory("Дерево", boats[i].Wood));
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
	}
}
