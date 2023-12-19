using JachtZ.src.db;
using System;
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

namespace JachtZ.src.pages
{
	/// <summary>
	/// Interaction logic for Orders.xaml
	/// </summary>
	public partial class Orders : Page
	{
		private MainWindow mainWindow = MainWindow.GetInstance();
		private JachtZEntities db = MainWindow.GetJachtZEntities();
		private List<Order> ordersList;
		private List<Contract> contractsList;

		private double scale = 1.0;

		public Orders()
		{
			InitializeComponent();

			ordersList = db.Orders.ToList();
			contractsList = db.Contracts.ToList();
			loadOrders(ordersList);
		}

		public Grid createCatergory(String c, String v)
		{
			Grid grid = new Grid();
			grid.ColumnDefinitions.Add(new ColumnDefinition());
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0, GridUnitType.Auto) });

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

		public void loadOrders(List<Order> list)
		{
			panel.Children.Clear();

			for (int i = 0; i < list.Count; i++)
			{
				Grid grid = new Grid();
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto) });
				grid.Margin = new Thickness(10, 0, 10, 0);
				grid.Width = double.NaN;
				grid.Height = 150;
				grid.Background = new SolidColorBrush(Color.FromRgb(0, 65, 101));

				//int clientIndex = i;
				//grid.MouseDown += (o, e) =>
				//{
				//	mainWindow.frame.Navigate(new EditBoat(list[clientIndex].boat_ID, list[clientIndex].Model, list[clientIndex].BoatType, list[clientIndex].Colour, list[clientIndex].Wood, list[clientIndex].BasePrice));
				//};

				Border border = new Border();
				border.Background = Brushes.Red;

				Label number = new Label();
				number.Content = list[i].Order_ID;
				number.HorizontalAlignment = HorizontalAlignment.Center;
				border.Child = number;

				StackPanel infoPanel = new StackPanel();
				infoPanel.Margin = new Thickness(10, 5, 10, 5);

				Button more = new Button();
				more.Content = "Больше";
				more.HorizontalAlignment = HorizontalAlignment.Right;

				int orderIndex = i;
				bool isOpen = false;

				more.Click += (e, o) => 
				{
					isOpen = !isOpen;

					if (isOpen)
					{
						grid.Height = 300;
						infoPanel.Children.Add(createCatergory("Лодка: ", list[orderIndex].Boat.Model));
						infoPanel.Children.Add(createCatergory("Готовность: ", contractsList.FirstOrDefault(c => c.Order_ID == list[orderIndex].Order_ID).ProductionProcess));
						infoPanel.Children.Add(createCatergory("Адрес доставки: ", list[orderIndex].DeliveryAddress));
						infoPanel.Children.Add(createCatergory("Предоплата: ", contractsList.FirstOrDefault(c => c.Order_ID == list[orderIndex].Order_ID).DepositPayed.ToString()));
						infoPanel.Children.Add(createCatergory("Общая сумма: ", contractsList.FirstOrDefault(c => c.Order_ID == list[orderIndex].Order_ID).ContractTotalPrice.ToString()));

						infoPanel.Children.Remove(more);
						infoPanel.Children.Add(more);
						more.Content = "Меньше";
					}
					else
					{
						grid.Height = 150;
						infoPanel.Children.RemoveRange(3, infoPanel.Children.Count);
						infoPanel.Children.Add(more);
						more.Content = "Больше";
					}

				};

				infoPanel.Children.Add(createCatergory("Продавец: ", list[i].SalesPerson.FirstName + " " + list[i].SalesPerson.FamilyName));
				infoPanel.Children.Add(createCatergory("Покупатель: ", list[i].Customer.FistName + " " + list[i].Customer.FamilyName));
				infoPanel.Children.Add(createCatergory("Дата оформления: ", list[i].Date.ToString()));
				infoPanel.Children.Add(more);

				grid.Children.Add(border);
				grid.Children.Add(infoPanel);
				Grid.SetRow(infoPanel, 1);

				panel.Children.Add(grid);
			}
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
			string input = search_text.Text.Trim();
			List<Order> newOrders = ordersList.Where(o =>
			{
				if (input.ToLower() == o.SalesPerson.FamilyName.ToLower().Trim()) return o.SalesPerson.FamilyName == input;
				else if (input.ToLower() == o.SalesPerson.FirstName.ToLower().Trim()) return o.SalesPerson.FirstName == input;
				else if (input.ToLower() == o.Customer.FistName.ToLower().Trim()) return o.Customer.FistName == input;
				else if (input.ToLower() == o.Customer.FamilyName.ToLower().Trim()) return o.Customer.FamilyName == input;
				else if (input.ToLower() == o.Boat.Model.ToLower().Trim()) return o.Boat.Model == input;
				else return false;
			}).ToList();

			loadOrders(newOrders);
		}
	}
}
