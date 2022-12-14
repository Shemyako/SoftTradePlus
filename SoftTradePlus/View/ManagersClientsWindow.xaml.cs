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

namespace SoftTradePlus.View
{
    /// <summary>
    /// Логика взаимодействия для ManagersClientsWindow.xaml
    /// </summary>
    public partial class ManagersClientsWindow : Window
    {
        public List<Models.Client> Clients { get; set; }
        public ManagersClientsWindow(List<Models.Client> clients)
        {
            InitializeComponent();
            Clients = clients;
            DataContext = this;
        }
    }
}
