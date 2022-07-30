using System;
using SoftTradePlus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SoftTradePlus.VievModel
{
    public class DataManageVM
    {
        ApplicationContext db = new ApplicationContext();
        public List<Client> Clients { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Product> Products { get; set; }
        public List<ClientStatus> Statuses { get; set; }
        RelayCommand? addManager;
        RelayCommand? editManager;
        RelayCommand? deleteManager;
        RelayCommand? addProduct;
        RelayCommand? editProduct;
        RelayCommand? deleteProduct;
        RelayCommand? addClient;
        RelayCommand? editClient;
        RelayCommand? deleteClient;
        public DataManageVM()
        {
            Clients = db.Clients.ToList();
            Managers = db.Managers.ToList();
            Products = db.Products.ToList();
            Statuses = db.ClientStatuses.ToList();
        }

        #region METHODS TO WORK WITH MANAGER

        public RelayCommand AddManager
        {
            get
            {
                return addManager ??
                    (addManager = new RelayCommand((o) =>
                    {
                        ManagerWindow managerWindow = new ManagerWindow(new Manager());
                        managerWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                        bool? result = managerWindow.ShowDialog();

                        while (result == true)
                        {
                            if (managerWindow.Manager.Name is not null &&
                                managerWindow.Manager.Name.Replace(" ", "").Length != 0)
                                break;
                            managerWindow = new ManagerWindow(managerWindow.Manager);
                            managerWindow.textBox.BorderBrush = Brushes.Red;
                            managerWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                            result = managerWindow.ShowDialog();

                        }

                        if (result == true)
                        {
                            Manager manager = managerWindow.Manager;
                            db.Managers.Add(manager);
                            db.SaveChanges();

                            //MainWindow.viewManager.ItemsSource = null;
                            Managers = db.Managers.ToList();

                            MainWindow.viewManager.ItemsSource = Managers;
                        }
                    }));
            }
        }

        public RelayCommand EditManager
        {
            get
            {
                return editManager ??
                    (editManager = new RelayCommand((selectedItem) =>
                    {
                        Manager? manager = (Manager?)selectedItem;
                        if (manager is null)
                            return;

                        Manager mg = db.Managers.FirstOrDefault(d => d.Id == manager.Id);
                        ManagerWindow managerWindow = new ManagerWindow(mg);
                        managerWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                        bool? result = managerWindow.ShowDialog();
                        
                        while (result == true)
                        {
                            if (managerWindow.Manager.Name is not null &&
                                managerWindow.Manager.Name.Replace(" ", "").Length != 0)
                                break;
                            managerWindow = new ManagerWindow(managerWindow.Manager);
                            managerWindow.textBox.BorderBrush = Brushes.Red;
                            managerWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                            result = managerWindow.ShowDialog();
                        }

                        if (result == true)
                        {
                            mg.Name = managerWindow.Manager.Name;
                            
                            db.SaveChanges();
                            //MainWindow.viewManager.Items.Refresh();
                        }
                    }));
            }
        }

        public RelayCommand DeleteManager
        {
            get
            {
                return deleteManager ??
                    (deleteManager = new RelayCommand((selectedItem) =>
                    {
                        Manager? manager = (Manager?)selectedItem;
                        if (manager is null)
                            return;
                        db.Managers.Remove(manager);
                        db.SaveChanges();

                        Managers = db.Managers.ToList();

                        MainWindow.viewManager.ItemsSource = Managers;

                    }));
            }
        }

        #endregion

        #region METHODS TO WORK WITH PRODUCTS
        private Product? Check_Product(Product product)
        {
            ProductWindow productWindos = new ProductWindow(product);
            productWindos.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            bool? result = productWindos.ShowDialog();

            while (result == true)
            {
                if (productWindos.Product.Name is not null &&
                    productWindos.Product.Name.Replace(" ", "").Length != 0 &&
                    productWindos.Product.Price != 0)
                    break;

                productWindos = new ProductWindow(productWindos.Product);
                productWindos.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                if (productWindos.Product.Name is not null &&
                    productWindos.Product.Name.Replace(" ", "").Length != 0)
                    productWindos.NameBox.BorderBrush = Brushes.Red;

                if (productWindos.Product.Price == 0)
                    productWindos.PriceBox.BorderBrush = Brushes.Red;

                result = productWindos.ShowDialog();

            }
            if (result == true)
                return productWindos.Product;
            return null;
        }

        public RelayCommand AddProduct
        {
            get
            {
                return addProduct ??
                    (addProduct = new RelayCommand((o) =>
                    {

                        Product? product = Check_Product(new Product());
                        if (product is not null)
                        {
                            db.Products.Add(product);
                            db.SaveChanges();

                            //MainWindow.viewManager.ItemsSource = null;
                            Products = db.Products.ToList();

                            MainWindow.viewProduct.ItemsSource = Products;
                        }
                    }));
            }
        }

        public RelayCommand EditProduct
        {
            get
            {
                return editProduct ??
                    (editProduct = new RelayCommand((selectedItem) =>
                    {
                        Product? product = (Product?)selectedItem;
                        if (product is null)
                            return;

                        Product? pd = db.Products.FirstOrDefault(d => d.Id == product.Id);
                        if (pd is null) return;

                        product = Check_Product(pd);

                        if (product is not null)
                        {
                            pd.Name = product.Name;

                            db.SaveChanges();
                            //MainWindow.viewManager.Items.Refresh();
                        }
                    }));
            }
        }

        public RelayCommand DeleteProduct
        {
            get
            {
                return deleteProduct ??
                    (deleteProduct = new RelayCommand((selectedItem) =>
                    {
                        Product? product = (Product?)selectedItem;
                        if (product is null)
                            return;
                        db.Products.Remove(product);
                        db.SaveChanges();

                        Products = db.Products.ToList();

                        MainWindow.viewProduct.ItemsSource = Products;

                    }));
            }
        }


        #endregion

        #region METHODS TO WORK WITH CUSTOMER
        private Client? Check_Client(Client client)
        {
            ClientWindow clientWindows = new ClientWindow(client, Managers, Statuses);
            clientWindows.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            bool? result = clientWindows.ShowDialog();

            while (result == true)
            {
                if (clientWindows.Client.Name is not null &&
                    clientWindows.Client.Name.Replace(" ", "").Length != 0)
                    break;

                clientWindows = new ClientWindow(clientWindows.Client, Managers, Statuses);
                clientWindows.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                if (clientWindows.Client.Name is not null &&
                    clientWindows.Client.Name.Replace(" ", "").Length != 0)
                    clientWindows.nameBox.BorderBrush = Brushes.Red;

                result = clientWindows.ShowDialog();

            }
            if (result == true)
                
                return clientWindows.Client;
            return null;
        }

        public RelayCommand AddClient
        {
            get
            {
                return addClient ??
                    (addClient = new RelayCommand((o) =>
                    {

                        Client? client = Check_Client(new Client());
                        if (client is not null)
                        {
                            db.Clients.Add(client);
                            db.SaveChanges();

                            //MainWindow.viewManager.ItemsSource = null;
                            Clients = db.Clients.ToList();

                            MainWindow.viewClient.ItemsSource = Clients;
                        }
                    }));
            }
        }

        public RelayCommand EditClient
        {
            get
            {
                return editClient ??
                    (editClient = new RelayCommand((selectedItem) =>
                    {
                        Client? client = (Client?)selectedItem;
                        if (client is null)
                            return;

                        Client? cl = db.Clients.FirstOrDefault(d => d.Id == client.Id);
                        if (cl is null) return;

                        client = Check_Client(cl);

                        if (client is not null)
                        {
                            cl.Name = client.Name;

                            db.SaveChanges();

                            Clients = db.Clients.ToList();

                            MainWindow.viewClient.ItemsSource = Clients;
                        }
                    }));
            }
        }

        public RelayCommand DeleteClient
        {
            get
            {
                return deleteClient ??
                    (deleteClient = new RelayCommand((selectedItem) =>
                    {
                        Client? client = (Client?)selectedItem;
                        if (client is null)
                            return;
                        db.Clients.Remove(client);
                        db.SaveChanges();

                        Clients = db.Clients.ToList();

                        MainWindow.viewClient.ItemsSource = Clients;

                    }));
            }
        }





        #endregion

        #region METHODS TO SELL PRODUCT



        #endregion
    }
}
