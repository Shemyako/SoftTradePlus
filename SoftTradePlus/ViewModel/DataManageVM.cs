using System;
using SoftTradePlus.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;

namespace SoftTradePlus.VievModel
{
    public class DataManageVM
    {
        ApplicationContext db = new ApplicationContext();
        // Lists to be displayed for user
        public List<Client> Clients { get; set; }
        public List<Manager> Managers { get; set; }
        public List<Product> Products { get; set; }
        public List<ClientStatus> Statuses { get; set; }

        // Commands
        RelayCommand? addManager;
        RelayCommand? editManager;
        RelayCommand? deleteManager;
        RelayCommand? addProduct;
        RelayCommand? editProduct;
        RelayCommand? deleteProduct;
        RelayCommand? addClient;
        RelayCommand? editClient;
        RelayCommand? deleteClient;
        RelayCommand? sellProduct;
        RelayCommand? clietnsProducts;
        RelayCommand? managersClients;

        public DataManageVM()
        {
            // Load lists
            Clients = db.Clients.ToList();
            Managers = db.Managers.ToList();
            Products = db.Products.ToList();
            Statuses = db.ClientStatuses.ToList();
        }

        #region METHODS TO WORK WITH MANAGER

        private Manager? Check_Manager(Manager manager)
        {
            ManagerWindow managerWindow = new ManagerWindow(manager);
            bool? result = managerWindow.ShowDialog();

            while (result == true)
            {
                if (managerWindow.Manager.Name is not null &&
                    managerWindow.Manager.Name.Replace(" ", "").Length != 0)
                    break;

                managerWindow = new ManagerWindow(managerWindow.Manager);
                managerWindow.textBox.BorderBrush = Brushes.Red;

                result = managerWindow.ShowDialog();

            }
            if (result == true)
                return managerWindow.Manager;
            return null;
        }

        public RelayCommand AddManager
        {
            get
            {
                return addManager ??
                    (addManager = new RelayCommand((o) =>
                    {
                        Manager? manager = Check_Manager(new Manager());

                    if (manager is not null)
                        {
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
                        manager = new Manager
                        {
                            Name = manager.Name,
                            Id = manager.Id
                        };

                        Manager? mg = db.Managers.FirstOrDefault(d => d.Id == manager.Id);
                        if (mg is null) return;

                        manager = Check_Manager(manager);

                        if (manager is not null)
                        {
                            mg.Name = manager.Name;
                            
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
                            if (product.Is_sub == false)
                                product.Sub_end = null;

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
                        product = new Product
                        {
                            Price = product.Price,
                            Id = product.Id,
                            Is_sub = product.Is_sub,
                            Sub_end = product.Sub_end,
                            Name = product.Name
                        };

                        Product? pd = db.Products.FirstOrDefault(d => d.Id == product.Id);
                        if (pd is null) return;

                        product = Check_Product(product);

                        if (product is not null)
                        {
                            if (product.Is_sub == false)
                                product.Sub_end = null;

                            pd.Price = product.Price;
                            pd.Id = product.Id;
                            pd.Is_sub = product.Is_sub;
                            pd.Sub_end = product.Sub_end;
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

        private Client selected_Client;
        public Client SelectedClient
        {
            get { return selected_Client; }
            set { selected_Client = value; }
        }
        private Client? Check_Client(Client client)
        {
            ClientWindow clientWindows = new ClientWindow(client, Managers, Statuses);
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
                        client = new Client
                        {
                            Name = client.Name,
                            ClientStatus = client.ClientStatus,
                            Id = client.Id,
                            Manager = client.Manager
                        };

                        Client? cl = db.Clients.FirstOrDefault(d => d.Id == client.Id);
                        if (cl is null) return;

                        client = Check_Client(client);

                        if (client is not null)
                        {
                            cl.Name = client.Name;
                            cl.ClientStatus = client.ClientStatus;
                            cl.Manager = client.Manager;

                            db.SaveChanges();

                            //Clients = db.Clients.ToList();

                            //MainWindow.viewClient.ItemsSource = Clients;
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

        public RelayCommand SellProduct
        {
            get
            {
                return sellProduct ??
                    (sellProduct = new RelayCommand((selectedProduct) =>
                    {
                        
                        View.ConfirmWindow confirmWindow = new View.ConfirmWindow();
                        if (confirmWindow.ShowDialog() == true && selectedProduct is not null && selected_Client is not null)
                        {
                            Product? sp = (Product)selectedProduct;
                            Client? cl = db.Clients.FirstOrDefault(d => d.Id == SelectedClient.Id);
                            Product? product = db.Products.FirstOrDefault(p => p.Id == sp.Id);
                            if (cl is null || product is null) return;
                            
                            
                            cl.Products.Add(product);
                            db.SaveChanges();
                        }
                        else
                        {
                            View.ShowTextWindow showTextWindow = new View.ShowTextWindow("Выберите клиента и продукт!");
                            showTextWindow.ShowDialog();
                        }


                        
                    }));
            }
        }

        #endregion

        #region METHODS TO SHOW REPORTS

        public RelayCommand ClietnsProducts
        {
            get
            {
                return clietnsProducts ??
                    (clietnsProducts = new RelayCommand((selectedItem) =>
                    {
                        Client? client = (Client?)selectedItem;
                        if (client is null)
                            return;

                        //Client? cl = db.Clients.FirstOrDefault(d => d.Id == client.Id);
                        Client? cl = db.Clients.Include(dd => dd.Products).FirstOrDefault( dd => dd.Id == client.Id);
                        if (cl is null) return;

                        View.ClietnsProductsWindow clietnsProductsWindow = new View.ClietnsProductsWindow(cl.Products);
                        clietnsProductsWindow.ShowDialog();

                    }));
            }
        }

        public RelayCommand ManagersClients
        {
            get
            {
                return managersClients ??
                    (managersClients = new RelayCommand((selectedItem) =>
                    {
                        Manager? manager = (Manager?)selectedItem;
                        if (manager is null)
                            return;

                        Manager? mg = db.Managers.Include(i => i.Clients).FirstOrDefault(dd => dd.Id == manager.Id);
                        if (mg is null) return;

                        View.ManagersClientsWindow managersClientsWindow = new View.ManagersClientsWindow(mg.Clients);
                        managersClientsWindow.ShowDialog();

                    }));
            }
        }

        #endregion
    }
}
