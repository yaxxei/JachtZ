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

		private void navList_Open<T>(List<T> list)
		{
			StackPanel expandPanel = new StackPanel();
			expandPanel.Orientation = Orientation.Vertical;

			for (int i = 0; i < list.Count; i++)
			{
				StackPanel stackPanel = new StackPanel();
				stackPanel.Orientation = Orientation.Horizontal;

				Label label = new Label();
				label.Margin = new Thickness { Left = 40 };

				if (list is List<Boat> boats)
				{
					label.Content = boats[i].Model;
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

			StackPanel element = (StackPanel)sender;
			if (element.Name == "nav_boats")
			{
				isBoatsExpand = !isBoatsExpand;

				if (!isBoatsExpand)
				{
					expand_boats.Fill = Brushes.White;
					expand_boats.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					frame.Navigate(new Boats());

					navList_Open(db.Boats.ToList());
				}
				else
				{
					expand_boats.Fill = Brushes.Transparent;
					expand_boats.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_boats.Children.Count > 1)
						nav_boats.Children.RemoveAt(1);
				}
			}
			else if (element.Name == "nav_accessories")
			{
				isAccessoriesExpand = !isAccessoriesExpand;

				if (!isAccessoriesExpand)
				{
					expand_accessories.Fill = Brushes.White;
					expand_accessories.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					frame.Navigate(new Accessories());

					navList_Open(db.Accessories.ToList());
				}
				else
				{
					expand_accessories.Fill = Brushes.Transparent;
					expand_accessories.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_accessories.Children.Count > 1)
						nav_accessories.Children.RemoveAt(1);
				}
			}
			else if (element.Name == "nav_clients")
			{
				isClientsExpand = !isClientsExpand;

				if (!isClientsExpand) 
				{
					expand_clients.Fill = Brushes.White;
					expand_clients.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					frame.Navigate(new Clients());

					navList_Open(db.Customers.ToList());
				}
				else
				{
					expand_clients.Fill = Brushes.Transparent;
					expand_clients.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_clients.Children.Count > 1)
						nav_clients.Children.RemoveAt(1);
				}
			}
			else if (element.Name == "nav_orders")
			{
				isOrdersExpand = !isOrdersExpand;

				if (!isOrdersExpand)
				{
					expand_orders.Fill = Brushes.White;
					expand_orders.Points = new PointCollection() {
						new Point(12.5, 4.5),
						new Point(5, 12.5),
						new Point(12.5, 12.5)
					};

					frame.Navigate(new Orders());

					navList_Open(db.Orders.ToList());
				}
				else
				{
					expand_orders.Fill = Brushes.Transparent;
					expand_orders.Points = new PointCollection() {
						new Point(5, 2.5),
						new Point(5, 12.5),
						new Point(12.5, 7.5)
					};

					if (nav_orders.Children.Count > 1)
						nav_orders.Children.RemoveAt(1);
				}
			}
		}
	}
}
