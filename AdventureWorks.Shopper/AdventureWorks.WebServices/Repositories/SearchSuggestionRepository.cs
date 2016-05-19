

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventureWorks.WebServices.Repositories
{
    public class SearchSuggestionRepository : IRepository<string>
    {
        private static readonly IEnumerable<string> _searchSuggestions = PopulateSearchSuggestions();

        public IEnumerable<string> GetAll()
        {
            lock (_searchSuggestions)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _searchSuggestions.ToArray();
            }
        }

        public string GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public string Create(string item)
        {
            throw new NotImplementedException();
        }

        public bool Update(string item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> PopulateSearchSuggestions()
        {
            return new List<string>
            {
                "100", "1000", "150", "200", "2000", "250", "30", "300", "3000",
                "350", "38", "40", "400", "42", "44", "450", "46", "48", "50",
                "500", "52", "54", "550", "56", "58", "60", "62", "650", "70", "750",
                "all-purpose", "awc", "battery", "beam", "bib", "bike", "black",
                "blue", "bottle", "brakes", "cable", "cage", "cap", "chain",
                "classic", "crankset", "derailleur", "dissolver", "dual", "fender",
                "finger", "fork", "frame", "front", "gloves", "half", "handlebars",
                "headlights", "headset", "helmet", "hitch", "hl", "hydration",
                "jersey", "kit", "large", "ll", "lock", "logo", "long",
                "men's", "minipump", "ml", "mountain", "oz", "pack", "panniers",
                "patch", "patches", "pedal", "powered", "pump", "racing", "rack",
                "rear", "red", "road", "saddle", "seat", "set", "shirt", "short",
                "shorts", "sleeve", "socks", "sport", "sports", "stand", "taillights",
                "tee", "tights", "tire", "touring", "tube", "vest", "wash",
                "water", "weatherproof", "wheel", "women's", "xl", "yellow"
            };
        }
        
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}