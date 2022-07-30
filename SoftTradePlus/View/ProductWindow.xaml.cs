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
using System.Windows.Shapes;

namespace SoftTradePlus
{
    /// <summary>
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        public Models.Product Product;
        public static TextBox price_box;
        public static DatePicker date_picker; 
        public ProductWindow(Models.Product product)
        {
            InitializeComponent();
            Product = product;
            Product.Is_sub = false;
            DataContext = Product;
            price_box = PriceBox;
            date_picker = DatePicker;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(price_box.Text, out _))
            {
                price_box.BorderBrush = Brushes.Gray;
                return;
            }

            DialogResult = true;
        }
    }
}
