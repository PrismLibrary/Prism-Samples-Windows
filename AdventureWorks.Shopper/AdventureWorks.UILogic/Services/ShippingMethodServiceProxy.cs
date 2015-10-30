using AdventureWorks.UILogic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace AdventureWorks.UILogic.Services
{
    public class ShippingMethodServiceProxy : IShippingMethodService
    {
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/ShippingMethod/", Constants.ServerAddress);

        public async Task<IEnumerable<ShippingMethod>> GetShippingMethodsAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(new Uri(_clientBaseUrl));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IEnumerable<ShippingMethod>>(responseContent);
                return result;
            }
        }

        public async Task<ShippingMethod> GetBasicShippingMethodAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(new Uri(_clientBaseUrl + "basic"));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ShippingMethod>(responseContent);
                return result;
            }
        }
    }
}
