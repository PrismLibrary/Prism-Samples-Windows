

using System.Linq;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventureWorks.WebServices.Tests.Repositories
{
    [TestClass]
    public class PaymentMethodRepositoryFixture
    {
        private PaymentMethodRepository target = new PaymentMethodRepository();

        [TestInitialize]
        public void Init()
        {
            PaymentMethodRepository.Reset();
        }

        [TestMethod]
        public void AddUpdate_Adds()
        {
            Assert.IsNull(target.GetAll("TestUserName"));

            target.AddUpdate("TestUserName", new PaymentMethod());

            Assert.AreEqual(1, target.GetAll("TestUserName").Count());
        }

        [TestMethod]
        public void AddUpdate_Updates()
        {
            target.AddUpdate("TestUserName", new PaymentMethod { Id = "address1", CardholderName = "TestFName" });
            target.AddUpdate("TestUserName", new PaymentMethod { Id = "address1", CardholderName = "TestFirstName" });

            Assert.AreEqual(1, target.GetAll("TestUserName").Count());
            Assert.AreEqual("TestFirstName", target.GetAll("TestUserName").First().CardholderName);
        }

        [TestMethod]
        public void GetAll_ReturnsAll_ForSpecificUser()
        {
            target.AddUpdate("User1", new PaymentMethod { Id = "paymentMethod1" });
            target.AddUpdate("User2", new PaymentMethod { Id = "paymentMethod2" });
            target.AddUpdate("User2", new PaymentMethod { Id = "paymentMethod3" });

            Assert.AreEqual(1, target.GetAll("User1").Count());
            Assert.AreEqual(2, target.GetAll("User2").Count());
        }

        [TestMethod]
        public void SetDefault_ClearsOldDefaultAndUpdatesNewDefault()
        {
            target.AddUpdate("TestUserName", new PaymentMethod { Id = "1", IsDefault = true });
            target.AddUpdate("TestUserName", new PaymentMethod { Id = "2"});

            target.SetDefault("TestUserName", "2");

            Assert.IsFalse(target.GetAll("TestUserName").First(a=>a.Id == "1").IsDefault);
            Assert.IsTrue(target.GetAll("TestUserName").First(a => a.Id == "2").IsDefault);
        }
    }
}
