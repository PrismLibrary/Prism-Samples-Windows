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
    public class LocationServiceProxy : ILocationService
    {
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Location", Constants.ServerAddress);

        public async Task<IReadOnlyCollection<string>> GetStatesAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(_clientBaseUrl));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReadOnlyCollection<string>>(responseContent);

                return result;
            }
        }
    }
}
