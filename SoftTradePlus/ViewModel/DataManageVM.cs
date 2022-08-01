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

        /// <summary>
        /// Checking manager creating
        /// </summary>
        /// <param name="manager"> Manager. new Manager when creating, existed manager when editing</param>
        /// <returns>Created/Edited Manager</returns>
        private Manager? Check_Manager(Manager manager)
        {
            // Show editing form
            ManagerWindow managerWindow = new ManagerWindow(manager);
            bool? result = managerWindow.ShowDialog();

            
            while (result == true)
            {
                // Checking manager name. When all good, break
                if (managerWindow.Manager.Name is not null &&
                    managerWindow.Manager.Name.Replace(" ", "").Length != 0)
                    break;

                // Else repeat
                managerWindow = new ManagerWindow(managerWindow.Manager);
                // and make border red
                managerWindow.textBox.BorderBrush = Brushes.Red;

                result = managerWindow.ShowDialog();

            }
            // if all is good, return manager
            if (result == true)
                return managerWindow.Manager;
            // else (when user close edit form), return null
            return null;
        }


        /// <summary>
        /// Command for add new manager
        /// </summary>
        public RelayCommand AddManager
        {
            get
            {
                return addManager ??
                    (addManager = new RelayCommand((o) =>
                    {
                        // Checking new manager
                        Manager? manager = Check_Manager(new Manager());

                        // if user created good manager, save him
                        if (manager is not null)
                        {
                            db.Managers.Add(manager);
                            db.SaveChanges();

                            // Load new manager list from db
                            Managers = db.Managers.ToList();

                            MainWindow.viewManager.ItemsSource = Managers;
                        }
                    }));
            }
        }


        /// <summary>
        /// Command for edit existed manager
        /// </summary>
        public RelayCommand EditManager
        {
            get
            {
                return editManager ??
                    (editManager = new RelayCommand((selectedItem) =>
                    {
                        // try to parse selected manager
                        Manager? manager = (Manager?)selectedItem;
                        if (manager is null)
                            return;

                        // Make copy of selected manager
                        // So editing won't be displayed on mainform
                        manager = new Manager
                        {
                            Name = manager.Name,
                            Id = manager.Id
                        };

                        // Find manager from DB
                        Manager? mg = db.Managers.FirstOrDefault(d => d.Id == manager.Id);
                        if (mg is null) return;

                        // Check manager editing
                        manager = Check_Manager(manager);

                        // Save changes if DB
                        if (manager is not null)
                        {
                            mg.Name = manager.Name;
                            
                            db.SaveChanges();
                            //MainWindow.viewManager.Items.Refresh();
                        }
                    }));
            }
        }


        /// <summary>
        /// Command to delete existed manager
        /// </summary>
        public RelayCommand DeleteManager
        {
            get
            {
                return deleteManager ??
                    (deleteManager = new RelayCommand((selectedItem) =>
                    {
                        // Try to parse selected manager
                        Manager? manager = (Manager?)selectedItem;
                        if (manager is null)
                            return;

                        // confirm deleting
                        View.ConfirmWindow confirmWindow = new View.ConfirmWindow();
                        if (confirmWindow.ShowDialog() == false) return;

                        // delete him and save
                        db.Managers.Remove(manager);
                        db.SaveChanges();

                        // update managers list
                        Managers = db.Managers.ToList();
                        MainWindow.viewManager.ItemsSource = Managers;

                        // update clients list
                        Clients = db.Clients.ToList();
                        MainWindow.viewClient.ItemsSource = Clients;

                    }));
            }
        }


        #endregion


        #region METHODS TO WORK WITH PRODUCTS


        /// <summary>
        /// Checking product creating
        /// </summary>
        /// <param name="product"> Product. New Product() when create new</param>
        /// <returns>Product when all good. null when user closed editing form</returns>
        private Product? Check_Product(Product product)
        {
            ProductWindow productWindos = new ProductWindow(product);
            bool? result = productWindos.ShowDialog();

            while (result == true)
            {
                // if all good, break while
                if (productWindos.Product.Name is not null &&
                    productWindos.Product.Name.Replace(" ", "").Length != 0 &&
                    productWindos.Product.Price != 0)
                    break;

                productWindos = new ProductWindow(productWindos.Product);

                // if name is'n correct, make it's border red
                if (productWindos.Product.Name is not null &&
                    productWindos.Product.Name.Replace(" ", "").Length != 0)
                    productWindos.NameBox.BorderBrush = Brushes.Red;

                // if price is'n correct, make it's border red
                if (productWindos.Product.Price == 0)
                    productWindos.PriceBox.BorderBrush = Brushes.Red;

                result = productWindos.ShowDialog();

            }
            if (result == true)
                return productWindos.Product;
            return null;
        }


        /// <summary>
        /// Command for add new product
        /// </summary>
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


        /// <summary>
        /// Command for edit existed product
        /// </summary>
        public RelayCommand EditProduct
        {
            get
            {
                return editProduct ??
                    (editProduct = new RelayCommand((selectedItem) =>
                    {
                        // try to parse selected product
                        Product? product = (Product?)selectedItem;
                        if (product is null)
                            return;

                        // make copy of selected product
                        // so user won't see editing on mainwindow
                        product = new Product
                        {
                            Price = product.Price,
                            Id = product.Id,
                            Is_sub = product.Is_sub,
                            Sub_end = product.Sub_end,
                            Name = product.Name
                        };

                        // find editing product in DB
                        Product? pd = db.Products.FirstOrDefault(d => d.Id == product.Id);
                        if (pd is null) return;

                        product = Check_Product(product);

                        // save changes
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
        

        /// <summary>
        /// Command for delete existed product
        /// </summary>
        public RelayCommand DeleteProduct
        {
            get
            {
                return deleteProduct ??
                    (deleteProduct = new RelayCommand((selectedItem) =>
                    {
                        // try to parse selected product
                        Product? product = (Product?)selectedItem;
                        if (product is null)
                            return;

                        // confirm deleting
                        View.ConfirmWindow confirmWindow = new View.ConfirmWindow();
                        if (confirmWindow.ShowDialog() == false) return;

                        // delete it and save
                        db.Products.Remove(product);
                        db.SaveChanges();

                        // update product list
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


        /// <summary>
        /// Method for checking client creating
        /// </summary>
        /// <param name="client">Client. new Client() when creating</param>
        /// <returns>Client or null</returns>
        private Client? Check_Client(Client client)
        {
            ClientWindow clientWindows = new ClientWindow(client, Managers, Statuses);
            bool? result = clientWindows.ShowDialog();

            while (result == true)
            {
                // if all good, break
                if (clientWindows.Client.Name is not null &&
                    clientWindows.Client.Name.Replace(" ", "").Length != 0)
                    break;

                clientWindows = new ClientWindow(clientWindows.Client, Managers, Statuses);
                clientWindows.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                // if name is'n correct, make it's border red
                if (clientWindows.Client.Name is not null &&
                    clientWindows.Client.Name.Replace(" ", "").Length != 0)
                    clientWindows.nameBox.BorderBrush = Brushes.Red;

                result = clientWindows.ShowDialog();

            }
            if (result == true)
                return clientWindows.Client;
            return null;
        }


        /// <summary>
        /// Command for client adding
        /// </summary>
        public RelayCommand AddClient
        {
            get
            {
                return addClient ??
                    (addClient = new RelayCommand((o) =>
                    {
                        // creating new client
                        Client? client = Check_Client(new Client());
                        
                        // save in DB
                        if (client is not null)
                        {
                            db.Clients.Add(client);
                            db.SaveChanges();

                            // update client list
                            Clients = db.Clients.ToList();

                            MainWindow.viewClient.ItemsSource = Clients;
                        }
                    }));
            }
        }


        /// <summary>
        /// Command for client editing
        /// </summary>
        public RelayCommand EditClient
        {
            get
            {
                return editClient ??
                    (editClient = new RelayCommand((selectedItem) =>
                    {
                        // try to parse selected client
                        Client? client = (Client?)selectedItem;
                        if (client is null)
                            return;

                        // copy selected client,
                        // so user won't see changes on mailform
                        client = new Client
                        {
                            Name = client.Name,
                            ClientStatus = client.ClientStatus,
                            Id = client.Id,
                            Manager = client.Manager
                        };

                        // find in DB client to edit
                        Client? cl = db.Clients.FirstOrDefault(d => d.Id == client.Id);
                        if (cl is null) return;

                        // editing client
                        client = Check_Client(client);

                        // edit client and save
                        if (client is not null)
                        {
                            cl.Name = client.Name;
                            cl.ClientStatus = client.ClientStatus;
                            cl.Manager = client.Manager;

                            db.SaveChanges();
                        }
                    }));
            }
        }


        /// <summary>
        /// Command for deleting client
        /// </summary>
        public RelayCommand DeleteClient
        {
            get
            {
                return deleteClient ??
                    (deleteClient = new RelayCommand((selectedItem) =>
                    {
                        // try to parse selected client
                        Client? client = (Client?)selectedItem;
                        if (client is null)
                            return;

                        // confirm deleting
                        View.ConfirmWindow confirmWindow = new View.ConfirmWindow();
                        if (confirmWindow.ShowDialog() == false) return;

                        // delete hom and save
                        db.Clients.Remove(client);
                        db.SaveChanges();

                        // load new client list
                        Clients = db.Clients.ToList();

                        MainWindow.viewClient.ItemsSource = Clients;

                    }));
            }
        }


        #endregion


        #region METHODS TO SELL PRODUCT


        /// <summary>
        /// Command to sell product.
        /// Product and client should be selected
        /// </summary>
        public RelayCommand SellProduct
        {
            get
            {
                return sellProduct ??
                    (sellProduct = new RelayCommand((selectedProduct) =>
                    {
                        // confirm selling
                        View.ConfirmWindow confirmWindow = new View.ConfirmWindow();
                        
                        // if confirm
                        if (confirmWindow.ShowDialog() == true && selectedProduct is not null && selected_Client is not null)
                        {
                            // find product and client
                            Product? sp = (Product)selectedProduct;
                            Client? cl = db.Clients.FirstOrDefault(d => d.Id == SelectedClient.Id);
                            Product? product = db.Products.FirstOrDefault(p => p.Id == sp.Id);
                            if (cl is null || product is null) return;
                            
                            // add product to client and save
                            cl.Products.Add(product);
                            db.SaveChanges();
                        }
                        else if (selectedProduct is null || selected_Client is null)
                        {
                            // show warning
                            View.ShowTextWindow showTextWindow = new View.ShowTextWindow("Выберите клиента и продукт!");
                            showTextWindow.ShowDialog();
                        }


                        
                    }));
            }
        }


        #endregion


        #region METHODS TO SHOW REPORTS


        /// <summary>
        /// Command to show client's products (that he bought)
        /// </summary>
        public RelayCommand ClietnsProducts
        {
            get
            {
                return clietnsProducts ??
                    (clietnsProducts = new RelayCommand((selectedItem) =>
                    {
                        // try to parse selected client
                        Client? client = (Client?)selectedItem;
                        if (client is null)
                            return;

                        // load from DB he's products
                        Client? cl = db.Clients.Include(dd => dd.Products).FirstOrDefault( dd => dd.Id == client.Id);
                        if (cl is null) return;

                        // show them
                        View.ClietnsProductsWindow clietnsProductsWindow = new View.ClietnsProductsWindow(cl.Products);
                        clietnsProductsWindow.ShowDialog();

                    }));
            }
        }


        /// <summary>
        /// Command to show manager's clients (that connected with manager)
        /// </summary>
        public RelayCommand ManagersClients
        {
            get
            {
                return managersClients ??
                    (managersClients = new RelayCommand((selectedItem) =>
                    {
                        // try to parse selected manager
                        Manager? manager = (Manager?)selectedItem;
                        if (manager is null)
                            return;

                        // load from DB he's clients
                        Manager? mg = db.Managers.Include(i => i.Clients).FirstOrDefault(dd => dd.Id == manager.Id);
                        if (mg is null) return;

                        // show them
                        View.ManagersClientsWindow managersClientsWindow = new View.ManagersClientsWindow(mg.Clients);
                        managersClientsWindow.ShowDialog();

                    }));
            }
        }

        #endregion
    }
}
