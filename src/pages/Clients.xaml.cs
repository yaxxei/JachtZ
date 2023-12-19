using JachtZ.src.db;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
	public partial class Clients : Page
	{
		private MainWindow mainWindow = MainWindow.GetInstance();
		private JachtZEntities db = MainWindow.GetJachtZEntities();
		private List<Customer> clients;
		private List<Order> ordersList;

		private double scale = 1.0;

		public Clients()
		{
			InitializeComponent();

			clients = db.Customers.ToList();
			ordersList = db.Orders.ToList();
			loadClients(clients);
		}

		

		public void loadClients(List<Customer> list)
		{
			panel.Children.Clear();

			for (int i = 0; i < list.Count; i++)
			{
				Grid grid = new Grid();
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto)});
				grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0, GridUnitType.Auto)});
				grid.Margin = new Thickness(10, 0, 10, 0);
				grid.Width = 220;
				grid.Height = 150;
				grid.Background = new SolidColorBrush(Color.FromRgb(0, 65, 101));

				//int clientIndex = i;
				//grid.MouseDown += (o, e) =>
				//{
				//	mainWindow.frame.Navigate(new EditBoat(list[clientIndex].boat_ID, list[clientIndex].Model, list[clientIndex].BoatType, list[clientIndex].Colour, list[clientIndex].Wood, list[clientIndex].BasePrice));
				//};

				StackPanel infoPanel = new StackPanel();
				infoPanel.Orientation = Orientation.Vertical;
				infoPanel.Margin = new Thickness(10, 5, 10, 5);

				Label fname = new Label();
				fname.Content = list[i].FistName;
				fname.FontSize = 16;

				Label sname = new Label();
				sname.Content = list[i].FamilyName;
				sname.FontSize = 16;

				Label phone = new Label();
				phone.Content = list[i].Phone;
				phone.FontSize = 16;

				infoPanel.Children.Add(fname);
				infoPanel.Children.Add(sname);
				infoPanel.Children.Add(phone);

				StackPanel orderPanel = new StackPanel();
				orderPanel.Orientation = Orientation.Horizontal;
				orderPanel.Margin = new Thickness(10, 5, 10, 5);

				Label orders = new Label();
				orders.Content = "Заказы";
				orders.FontSize = 16;

				ComboBox ordersCombo = new ComboBox();
				ordersCombo.VerticalAlignment = VerticalAlignment.Center;
				ordersCombo.DisplayMemberPath = "Date";
				ordersCombo.ItemsSource = ordersList.Where(o => o.Customer_ID == clients[i].Customer_ID);

				orderPanel.Children.Add(orders);
				orderPanel.Children.Add(ordersCombo);

				grid.Children.Add(infoPanel);
				grid.Children.Add(orderPanel);
				Grid.SetRow(infoPanel, 0);
				Grid.SetRow(orderPanel, 1);

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
			List<Customer> newClients = clients.Where(c =>
			{
				if (input.ToLower() == c.FamilyName.ToLower().Trim()) return c.FamilyName == input;
				else if (input.ToLower() == c.FistName.ToLower().Trim()) return c.FistName == input;
				else if (input.ToLower() == c.Phone.ToLower().Trim()) return c.Phone == input;
				else return false;
			}).ToList();

			loadClients(newClients);
		}

	}
}
