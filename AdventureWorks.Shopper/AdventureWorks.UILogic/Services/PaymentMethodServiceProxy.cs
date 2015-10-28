using AdventureWorks.UILogic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace AdventureWorks.UILogic.Services
{
    public class PaymentMethodServiceProxy : IPaymentMethodService
    {
        private readonly IAccountService _accountService;
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/PaymentMethod", Constants.ServerAddress);

        public PaymentMethodServiceProxy(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<ICollection<PaymentMethod>> GetPaymentMethodsAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(_clientBaseUrl));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Collection<PaymentMethod>>(responseContent);
            }
        }

        public async Task SavePaymentMethodAsync(PaymentMethod paymentMethod)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(new Uri(_clientBaseUrl), new HttpStringContent(JsonConvert.SerializeObject(paymentMethod), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                await response.EnsureSuccessWithValidationSupportAsync();
            }
        }

        public async Task SetDefault(string defaultPaymentMethodId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsync(new Uri(_clientBaseUrl + "?defaultPaymentMethodId=" + defaultPaymentMethodId), null);
                await response.EnsureSuccessWithValidationSupportAsync();
            }
        }
    }
}