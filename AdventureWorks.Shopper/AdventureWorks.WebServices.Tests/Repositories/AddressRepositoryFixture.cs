

using System.Linq;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureWorks.WebServices.Tests.Repositories
{
    [TestClass]
    public class AddressRepositoryFixture
    {
        private AddressRepository target = new AddressRepository();

        [TestInitialize]
        public void Init()
        {
            AddressRepository.Reset();
        }

        [TestMethod]
        public void AddUpdate_Adds()
        {
            Assert.IsNull(target.GetAll("TestUserName"));

            target.AddUpdate("TestUserName", new Address());

            Assert.AreEqual(1, target.GetAll("TestUserName").Count());
        }

        [TestMethod]
        public void AddUpdate_Updates()
        {
            target.AddUpdate("TestUserName", new Address { Id = "address1", AddressType = AddressType.Shipping, FirstName = "TestFName"});
            target.AddUpdate("TestUserName", new Address { Id = "address1", AddressType = AddressType.Shipping, FirstName = "TestFirstName" });

            Assert.AreEqual(1, target.GetAll("TestUserName").Count());
            Assert.AreEqual("TestFirstName", target.GetAll("TestUserName").First().FirstName);
        }

        [TestMethod]
        public void GetAll_ReturnsAddresses_ForSpecificUser()
        {
            target.AddUpdate("User1", new Address { Id = "address1"});
            target.AddUpdate("User2", new Address { Id = "address2"});
            target.AddUpdate("User2", new Address { Id = "address3" });

            Assert.AreEqual(1, target.GetAll("User1").Count());
            Assert.AreEqual(2, target.GetAll("User2").Count());
        }

        [TestMethod]
        public void SetDefault_ClearsOldDefaultAndUpdatesNewDefault()
        {
            target.AddUpdate("TestUserName", new Address { Id = "1", AddressType = AddressType.Shipping, IsDefault = true});
            target.AddUpdate("TestUserName", new Address { Id = "2", AddressType = AddressType.Shipping});

            target.SetDefault("TestUserName", "2", AddressType.Shipping);

            Assert.IsFalse(target.GetAll("TestUserName").First(a=>a.Id == "1").IsDefault);
            Assert.IsTrue(target.GetAll("TestUserName").First(a => a.Id == "2").IsDefault);
        }
    }
}
