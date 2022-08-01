using SoftTradePlus;
using SoftTradePlus.Models;

namespace SoftTradeUnitTest
{
    [TestClass]
    public class ManagerTest
    {

        [TestMethod]
        public void TestManagerDeleting()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();
            Manager manager = new Manager
            {
                Name = "��������1234567890"
            };
            db.Managers.Add(manager);
            db.SaveChanges();
            int count = db.Managers.Select(x => x.Name == "��������� ���").Count();

            // act
            db.Managers.Remove(manager);
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Managers.Select(o => o.Name == manager.Name).Count(),
                count - 1);
        }
        [TestMethod]
        public void TestManagersClreation()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();
            Manager manager = new Manager
            {
                Name = "��������1234567890"
            };
            int count = db.Managers.Select(x=>x.Name == manager.Name).Count();

            // act
            db.Managers.Add(manager);
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Managers.Select(o => o.Name == manager.Name).Count(),
                count+1);

            // deleting changes
            db.Managers.Remove(manager);
            db.SaveChanges();
        }

        [TestMethod]
        public void TestManagerEditing()
        {
            // arrange
            ApplicationContext db = new ApplicationContext();
            Manager manager = new Manager
            {
                Name = "��������1234567890"
            };
            int count = db.Managers.Select(x => x.Name == "��������� ���").Count();
            db.Managers.Add(manager);
            db.SaveChanges();

            // act
            manager.Name = "��������� ���";
            db.SaveChanges();

            // assert
            Assert.AreEqual(db.Managers.Select(o => o.Name == manager.Name).Count(),
                count + 1);

            // deleting changes
            db.Managers.Remove(manager);
            db.SaveChanges();

        }
    }
}