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
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public List<Models.Manager> Managers { get; set; }
        public List<Models.ClientStatus> Statuses { get; set; }
        public Models.Client Client { get; set; }
        public static TextBox NameBox;
        public ClientWindow(Models.Client client, List<Models.Manager> managers, List<Models.ClientStatus> statuses)
        {
            InitializeComponent();
            Client = client;
            Managers = managers;
            Statuses = statuses;
            DataContext = this;
            NameBox = nameBox;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerCombobox.SelectedIndex == -1)
            {
                ManagerWarning.Fill = Brushes.Red;
                
                return;
            }else ManagerWarning.Fill = Brushes.White;
            if (StatusCombobox.SelectedIndex == -1)
            {
                StatusWarning.Fill = Brushes.Red;
                return;
            }
            else StatusWarning.Fill = Brushes.White;

            DialogResult = true;
        }
    }
}
