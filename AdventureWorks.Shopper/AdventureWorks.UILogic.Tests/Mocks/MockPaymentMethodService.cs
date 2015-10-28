

using System.Collections.ObjectModel;
using System.Linq;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockPaymentMethodService : IPaymentMethodService
    {
        public MockPaymentMethodService()
        {
            PaymentMethods = new List<PaymentMethod>();
        }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public Task<ICollection<PaymentMethod>> GetPaymentMethodsAsync()
        {
            if (PaymentMethods == null) return Task.FromResult<ICollection<PaymentMethod>>(null);
            return Task.FromResult<ICollection<PaymentMethod>>(new ReadOnlyCollection<PaymentMethod>(PaymentMethods));
        }

        public Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            var matchingPaymentMethod = PaymentMethods.FirstOrDefault(p => p.Id == paymentMethod.Id);
            if (matchingPaymentMethod != null)
            {
                PaymentMethods.Remove(matchingPaymentMethod);
            }
            PaymentMethods.Add(paymentMethod);
            return Task.Delay(0);
        }

        public Task SetDefault(string defaultPaymentMethodId)
        {
            var matchingPaymentMethod = PaymentMethods.FirstOrDefault(p => p.Id == defaultPaymentMethodId);
            matchingPaymentMethod.IsDefault = true;
            return Task.Delay(0);
        }

        public void SaveEntity(PaymentMethod paymentMethod)
        {
            PaymentMethods.Add(paymentMethod);
        }
    }
}
