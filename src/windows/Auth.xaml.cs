using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using JachtZ.src.db;
using JachtZ.src.events;

namespace JachtZ.src.windows
{
	/// <summary>
	/// Interaction logic for Auth.xaml
	/// </summary>
	public partial class Auth : Window
	{
		private JachtZEntities db = MainWindow.GetJachtZEntities();
		private List<SalesPerson> salesPersons;

		public event EventHandler<AuthEventArgs> AuthSaccessful;

		public Auth()
		{
			InitializeComponent();

			salesPersons = db.SalesPersons.ToList();
		}

		private void Auth_Click(object sender, RoutedEventArgs e)
		{
			if (login_text.Text.Length > 0 && password_text.Text.Length > 0) 
			{
				foreach (SalesPerson person in salesPersons)
				{
					if (person.FirstName == login_text.Text
						&& person.FirstName == password_text.Text)
					{
						AuthSaccessful?.Invoke(this, new AuthEventArgs($"{person.FirstName} {person.FamilyName}"));

						this.Close();
					} 
					else
					{
						input.Children.Add(errorLabel("Неверный логин или пароль"));
					}
				}
			}
			else
			{
				input.Children.Add(errorLabel("Все поля должны быть заполнены"));
			}
		}

		private Label errorLabel(String error)
		{
			if (input.Children.Count == 3) 
			{
				input.Children.RemoveAt(2);

				Label label = new Label();
				label.Content = error;
				label.HorizontalAlignment = HorizontalAlignment.Center;
				label.Foreground = Brushes.Red;
				label.Margin = new Thickness { Top = -10 };

				return label;
			}
			else
			{
				Label label = new Label();
				label.Content = error;
				label.HorizontalAlignment = HorizontalAlignment.Center;
				label.Foreground = Brushes.Red;
				label.Margin = new Thickness { Top = -10 };

				return label;
			}
		}

	}
}
