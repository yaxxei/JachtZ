using JachtZ.src.db;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
		private JachtZEntities db = MainWindow.GetJachtZEntities();
		private Boat boat;

		private int idValue { get; set; }
		private string modelValue { get; set; }
		private string typeValue { get; set; }
		private string colorValue { get; set; }
		private string woodValue { get; set; }
		private decimal? priceValue { get; set; }

		private bool isNew = false;

		private double scale = 1.0;
		public EditBoat()
		{
			InitializeComponent();

			isNew = true;

			boat = new Boat();
		}

		public EditBoat(int idValue, string modelValue, string typeValue, string colorValue, string woodValue, decimal? pricevalue)
		{
			InitializeComponent();

			this.idValue = idValue;
			this.modelValue = modelValue;
			this.typeValue = typeValue;
			this.colorValue = colorValue;
			this.woodValue = woodValue;
			this.priceValue = pricevalue;

			boat = db.Boats.ToList().FirstOrDefault(b => b.boat_ID == idValue);

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
				wood_combo.Visibility = Visibility.Visible;
			}
			else if (element.Name == "price_edit")
			{
				price.Visibility = Visibility.Collapsed;
				price_text.Visibility = Visibility.Visible;
			}
		}

		private void edit_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				if (model_text.IsFocused)
				{
					model.Visibility = Visibility.Visible;
					model_text.Visibility = Visibility.Collapsed;

					this.modelValue = model_text.Text;
				}
				else if (type_text.IsFocused)
				{
					type.Visibility = Visibility.Visible;
					type_text.Visibility = Visibility.Collapsed;

					this.typeValue = type_text.Text;
				}
				else if (color_text.IsFocused)
				{
					color.Visibility = Visibility.Visible;
					color_text.Visibility = Visibility.Collapsed;

					this.colorValue = color_text.Text;
				}
				else if (wood_combo.IsFocused)
				{
					wood.Visibility = Visibility.Visible;
					wood_combo.Visibility = Visibility.Collapsed;

					this.woodValue = wood_combo.Text;
				}
				else if (price_text.IsFocused)
				{
					price.Visibility = Visibility.Visible;
					price_text.Visibility = Visibility.Collapsed;

					this.priceValue = Decimal.Parse(price_text.Text);
				}
			}
		}

		private void save_changes_MouseDown(object sender, MouseButtonEventArgs e)
		{
			this.boat.Model = this.modelValue;
			this.boat.BoatType = this.typeValue;
			this.boat.Mast = this.typeValue == "Парусная лодка" ? true : false;
			this.boat.Colour = this.colorValue;
			this.boat.Wood = this.woodValue;
			this.boat.BasePrice = this.priceValue;
			this.boat.VAT = 0.18;
			this.boat.F11 = (double)(this.priceValue * 60);

			if (isNew)
				db.Boats.Add(this.boat);

			MainWindow.GetInstance().SaveDataBase();

			MessageBox.Show("Сохранение успешно");
		}

		private void delete_boat_MouseDown(object sender, MouseButtonEventArgs e)
		{
			delete_boat.Visibility = Visibility.Collapsed;
			sure.Visibility = Visibility.Visible;
		}

		private void sure_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Label element = (Label)sender;
			if (element.Name == "yes")
			{
				db.Boats.Remove(this.boat);
				MainWindow.GetInstance().SaveDataBase();

				MainWindow.GetInstance().frame.Navigate(new Boats());

				MessageBox.Show("Удаление успешно");
			}
			else if (element.Name == "no")
			{
				delete_boat.Visibility = Visibility.Visible;
				sure.Visibility = Visibility.Collapsed;
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

				editor.LayoutTransform = new ScaleTransform(scale, scale);
			}
		}
	}
}
