using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using JachtZ.src.windows;
using JachtZ.src.pages;
using JachtZ.src.events;
using System.Xml.Linq;

namespace JachtZ
{
	public partial class MainWindow : Window
	{
		public static JachtZEntities db = new JachtZEntities();
		public static MainWindow instance;

		private ColumnDefinition column;
		private bool isDrag = false;

		private bool isBoatsExpand = false;
		private bool isAccessoriesExpand = false;
		private bool isClientsExpand = false;
		private bool isOrdersExpand = false;

		private double scale = 1.0;
		public MainWindow()
		{
			InitializeComponent();

			instance = this;
			column = main.ColumnDefinitions[0];

			Auth authWin = new Auth();
			authWin.AuthSaccessful += AuthWin_AutAuthSaccessful;

			authWin.Show();
			this.Hide();
		}

		public void AuthWin_AutAuthSaccessful(object sender, AuthEventArgs e)
		{
			username.Content = e.username;
			frame.Navigate(new Main());
			this.Show();
		}

		public static JachtZEntities GetJachtZEntities()
		{
			return db;
		}

		public static MainWindow GetInstance()
		{
			return instance;
		}

		public void SaveDataBase()
		{
			db.SaveChanges();
		}

		private void drag_MouseDown(object sender, MouseButtonEventArgs e)
		{
			isDrag = true;
			var draggable = sender as UIElement;
			draggable.CaptureMouse();
		}

		private void drag_MouseUp(object sender, MouseButtonEventArgs e)
		{
			isDrag = false;
			var draggable = sender as UIElement;
			draggable.ReleaseMouseCapture();
		}

		private void drag_MouseMove(object sender, MouseEventArgs e)
		{
			if (isDrag)
			{
				double x = e.GetPosition(this).X;
				if (x > 50 && x < this.Width - 50)
				{
					column.Width = new GridLength(x);
				}
			}
		}

		private void navList_Expand<T>(List<T> list)
		{
			StackPanel expandPanel = new StackPanel();
			expandPanel.Orientation = Orientation.Vertical;

			for (int i = 0; i < list.Count; i++)
			{
				StackPanel stackPanel = new StackPanel();
				stackPanel.Orientation = Orientation.Horizontal;

				Label label = new Label();
				label.Margin = new Thickness { Left = 40 };

				int index = i;

				if (list is List<Boat> boats)
				{
					label.Content = boats[i].Model;
					label.MouseDown += (o, e) =>
					{
						frame.Navigate(new EditBoat(boats[index].boat_ID, boats[index].Model, boats[index].BoatType, boats[index].Colour, boats[index].Wood, boats[index].BasePrice));
					};
					stackPanel.Children.Add(label);
				}
				else if (list is List<Accessory> accessories)
				{
					label.Content = accessories[i].AccName;
					stackPanel.Children.Add(label);
				}
				else if (list is List<Customer> clients)
				{
					label.Content = clients[i].FamilyName;
					stackPanel.Children.Add(label);
				}
				else if (list is List<Order> orders)
				{
					label.Content = orders[i].Date;
					stackPanel.Children.Add(label);
				}

				expandPanel.Children.Add(stackPanel);
			}

			if (list is List<Boat>)
			{
				nav_boats.Children.Add(expandPanel);
			}
			else if (list is List<Accessory>)
			{
				nav_accessories.Children.Add(expandPanel);
			}
			else if (list is List<Customer>)
			{
				nav_clients.Children.Add(expandPanel);
			}

			else if (list is List<Order>)
			{
				nav_orders.Children.Add(expandPanel);
			}

		}

		private void nav_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Points="5,2.5 5,12.5 12.5,7.5"
			// Points="12.5,4.5 5,12.5 12.5,12.5"

			Label element = (Label)sender;
			if (element.Name == "boats")
			{
				frame.Navigate(new Boats());
			}
			else if (element.Name == "accessories")
			{
				frame.Navigate(new Accessories());
			}
			else if (element.Name == "clients")
			{
				frame.Navigate(new Clients());
			}
			else if (element.Name == "orders")
			{
				frame.Navigate(new Orders());
			}
		}

		private void expand_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Grid element = (Grid)sender;
			if (element.Name == "expand_boats")
			{
				isBoatsExpand = !isBoatsExpand;

				if (!isBoatsExpand)
				{
					expander_boats.Fill = Brushes.White;
					expander_boats.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					navList_Expand(db.Boats.ToList());
				}
				else
				{
					expander_boats.Fill = Brushes.Transparent;
					expander_boats.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_boats.Children.Count > 1)
						nav_boats.Children.RemoveAt(1);
				}
			}
			else if (element.Name == "expand_accessories")
			{
				isAccessoriesExpand = !isAccessoriesExpand;

				if (!isAccessoriesExpand)
				{
					expander_accessories.Fill = Brushes.White;
					expander_accessories.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					navList_Expand(db.Accessories.ToList());
				}
				else
				{
					expander_accessories.Fill = Brushes.Transparent;
					expander_accessories.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_accessories.Children.Count > 1)
						nav_accessories.Children.RemoveAt(1);
				}
			}
			else if (element.Name == "expand_clients")
			{
				isClientsExpand = !isClientsExpand;

				if (!isClientsExpand)
				{
					expander_clients.Fill = Brushes.White;
					expander_clients.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					navList_Expand(db.Customers.ToList());
				}
				else
				{
					expander_clients.Fill = Brushes.Transparent;
					expander_clients.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_clients.Children.Count > 1)
						nav_clients.Children.RemoveAt(1);
				}
			}
			else if (element.Name == "expand_orders")
			{
				isOrdersExpand = !isOrdersExpand;

				if (!isOrdersExpand)
				{
					expander_orders.Fill = Brushes.White;
					expander_orders.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					navList_Expand(db.Orders.ToList());
				}
				else
				{
					expander_orders.Fill = Brushes.Transparent;
					expander_orders.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_orders.Children.Count > 1)
						nav_orders.Children.RemoveAt(1);
				}
			}
		}

		private void side_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
			{
				if (e.Key == Key.Add || e.Key == Key.OemPlus)
					if (scale < 4.0)
						scale *= 1.2;
				if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
					if (scale > 0.5)
						scale /= 1.2;

				side.LayoutTransform = new ScaleTransform(scale, scale);
			}
		}
	}
}
