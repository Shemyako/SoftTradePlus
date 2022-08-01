using SoftTradePlus;
using SoftTradePlus.Models;

namespace SoftTradeUnitTest
{

    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void TestClientDeleting()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();

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

            int count = db.Clients.Select(x => x.Name == "Изменённое имя").Count();

            // act
            db.Clients.Remove(client);
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Clients.Select(o => o.Name == client.Name).Count(),
                count - 1);

            // deleting changes
            db.Managers.Remove(manager);
            db.ClientStatuses.Remove(status);
            db.SaveChanges();
        }

        [TestMethod]
        public void TestClientClreation()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();

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

            int count = db.Clients.Select(x => x.Name == client.Name).Count();

            // act
            db.Clients.Add(client);
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Clients.Select(o => o.Name == client.Name).Count(),
                count + 1);

            // deleting changes
            db.Clients.Remove(client);
            db.Managers.Remove(manager);
            db.ClientStatuses.Remove(status);
            db.SaveChanges();
        }

        [TestMethod]
        public void TestClientEditing()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();

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

            int count = db.Clients.Select(x => x.Name == "Изменённое имя").Count();
            db.Clients.Add(client);
            db.SaveChanges();

            // act
            client.Name = "Изменённое имя";
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Clients.Select(o => o.Name == client.Name).Count(),
                count + 1);

            // deleting changes
            db.Clients.Remove(client);
            db.Managers.Remove(manager);
            db.ClientStatuses.Remove(status);
            db.SaveChanges();

        }

    }
}
