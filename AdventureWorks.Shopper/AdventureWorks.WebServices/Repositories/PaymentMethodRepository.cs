

using System.Collections.Generic;
using System.Linq;
using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public class PaymentMethodRepository : IPaymentMethodRepository
    {
        private static Dictionary<string, List<PaymentMethod>> _paymentMethodsDictionary = new Dictionary<string, List<PaymentMethod>>(); 

        public IEnumerable<PaymentMethod> GetAll(string userName)
        {
            return _paymentMethodsDictionary.ContainsKey(userName) ? _paymentMethodsDictionary[userName] : null;
        }

        public void AddUpdate(string userName, PaymentMethod paymentMethod)
        {
            if (!_paymentMethodsDictionary.ContainsKey(userName))
            {
                _paymentMethodsDictionary[userName] = new List<PaymentMethod>();
            }

            var userPaymentMethods = _paymentMethodsDictionary[userName];
            var matchingPaymentMethod = userPaymentMethods.Find(a => a.Id == paymentMethod.Id);
            if (matchingPaymentMethod != null)
            {
                userPaymentMethods.Remove(matchingPaymentMethod);
            }
            _paymentMethodsDictionary[userName].Add(paymentMethod);
        }

        public void SetDefault(string userName, string defaultPaymentMethodId)
        {
            var paymentMethods = _paymentMethodsDictionary[userName];
            
            //Clear old default payment methods
            var oldDefaults = paymentMethods.Where(a => a.IsDefault);
            foreach (var oldDefault in oldDefaults)
            {
                oldDefault.IsDefault = false;
            }

            var defaultPaymentMethod = paymentMethods.Find(a => a.Id == defaultPaymentMethodId);
            if (defaultPaymentMethod != null)
            {
                defaultPaymentMethod.IsDefault = true;
            }
        }

        public static void Reset()
        {
            _paymentMethodsDictionary = new Dictionary<string, List<PaymentMethod>>();
        }

    }
}