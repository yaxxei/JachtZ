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
	/// Interaction logic for EditBoat.xaml
	/// </summary>
	public partial class EditBoat : Page
	{
		private string modelValue { get; set; }
		private string typeValue { get; set; }
		private string colorValue { get; set; }
		private string woodValue { get; set; }
		private decimal priceValue { get; set; }

		public EditBoat()
		{
			InitializeComponent();
		}

		public EditBoat(string modelValue, string typeValue, string colorValue, string woodValue, decimal pricevalue)
		{
			InitializeComponent();

			this.modelValue = modelValue;
			this.typeValue = typeValue;
			this.colorValue = colorValue;
			this.woodValue = woodValue;
			this.priceValue = pricevalue;

			loadBoatDesc();
		}

		public void loadBoatDesc()
		{
			if (modelValue != null)
			{
				model.Content = modelValue;
				type.Content = typeValue;
				color.Content = colorValue;
				wood.Content = woodValue;
				price.Content = priceValue;
			}
		}

		private void edit_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Image element = (Image) sender;
			if (element.Name == "model_edit")
			{
				model.Visibility = Visibility.Collapsed;
				model_text.Visibility = Visibility.Visible;
			}
			else if (element.Name == "type_edit")
			{
				type.Visibility = Visibility.Collapsed;
				type_text.Visibility = Visibility.Visible;
			}
			else if (element.Name == "color_edit")
			{
				color.Visibility = Visibility.Collapsed;
				color_text.Visibility = Visibility.Visible;
			}
			else if (element.Name == "wood_edit")
			{
				wood.Visibility = Visibility.Collapsed;
				wood_text.Visibility = Visibility.Visible;
			}
			else if (element.Name == "price_edit")
			{
				price.Visibility = Visibility.Collapsed;
				price_text.Visibility = Visibility.Visible;
			}
		}

		private void model_edit_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (model_text.IsFocused)
				{
					model.Visibility = Visibility.Visible;
					model_text.Visibility = Visibility.Collapsed;
				}
				else if (type_text.IsFocused)
				{
					type.Visibility = Visibility.Visible;
					type_text.Visibility = Visibility.Collapsed;
				}
				else if (color_text.IsFocused)
				{
					color.Visibility = Visibility.Visible;
					color_text.Visibility = Visibility.Collapsed;
				}
				else if (wood_text.IsFocused)
				{
					wood.Visibility = Visibility.Visible;
					wood_text.Visibility = Visibility.Collapsed;
				}
				else if (price_text.IsFocused)
				{
					price.Visibility = Visibility.Visible;
					price_text.Visibility = Visibility.Collapsed;
				}
			}
		}
	}
}
