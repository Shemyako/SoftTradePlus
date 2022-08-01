using Microsoft.EntityFrameworkCore;
using SoftTradePlus;
using SoftTradePlus.Models;

namespace SoftTradeUnitTest
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void ProductCreating()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();
            Product product = new Product
            {
                Name = "Товар123",
                Price = 123,
                Is_sub = true,
                Sub_end = DateTime.Now
            };
            int count = db.Products.Select(x => x.Name == product.Name).Count();

            // act
            db.Products.Add(product);
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Products.Select(o => o.Name == product.Name).Count(),
                count + 1);

            // deleting changes
            db.Products.Remove(product);
            db.SaveChanges();
        }

        [TestMethod]
        public void ProductDeleting()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();
            Product product = new Product
            {
                Name = "Товар123",
                Price = 123,
                Is_sub = true,
                Sub_end = DateTime.Now
            };
            db.Products.Add(product);
            db.SaveChanges();
            int count = db.Products.Select(x => x.Name == "Изменённое имя").Count();

            // act
            db.Products.Remove(product);
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Products.Select(o => o.Name == product.Name).Count(),
                count - 1);
        }

        [TestMethod]
        public void ProductEditing()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();
            Product product = new Product
            {
                Name = "Товар123",
                Price = 123,
                Is_sub = true,
                Sub_end = DateTime.Now
            };
            int count = db.Products.Select(x => x.Name == "Изменённое имя").Count();
            db.Products.Add(product);
            db.SaveChanges();

            // act
            product.Name = "Изменённое имя";
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Products.Select(o => o.Name == product.Name).Count(),
                count + 1);

            // deleting changes
            db.Products.Remove(product);
            db.SaveChanges();
        }

        [TestMethod]
        public void SellingProduct()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();

            Product product = new Product
            {
                Name = "Товар123",
                Price = 123,
                Is_sub = true,
                Sub_end = DateTime.Now
            };
            db.Products.Add(product);

            ClientStatus status = new ClientStatus
            {
                Name = "Статус клиента"
            };
            db.ClientStatuses.Add(status);

            Manager manager = new Manager
            {
                Name = "Менеджер1234567890"
            };
            db.Managers.Add(manager);
            db.SaveChanges();

            Client client = new Client
            {
                Name = "Клиент1234567890",
                Manager = manager,
                ClientStatus = status
            };
            db.Clients.Add(client);
            db.SaveChanges();

            // act
            client.Products.Add(product);
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Clients.Include(dd => dd.Products).FirstOrDefault(dd => dd.Id == client.Id).Products.Count,
                1);

            // deleting changes
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
}
