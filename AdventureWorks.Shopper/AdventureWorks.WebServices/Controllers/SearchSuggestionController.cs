

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using AdventureWorks.WebServices.Repositories;

namespace AdventureWorks.WebServices.Controllers
{
    public class SearchSuggestionController : ApiController
    {
        private readonly IRepository<string> _searchSuggestionsRepository;

        public SearchSuggestionController()
            : this(new SearchSuggestionRepository())
        { }

        public SearchSuggestionController(IRepository<string> searchSuggestionsRepository)
        {
            _searchSuggestionsRepository = searchSuggestionsRepository;
        }

        //
        // GET: /SearchSuggestion/
        public IEnumerable<string> GetSearchSuggestions()
        {
            return _searchSuggestionsRepository.GetAll();
        }

        //
        // GET: /SearchSuggestion/
        public IEnumerable<string> GetSearchSuggestions(string searchTerm)
        {
            var items = _searchSuggestionsRepository.GetAll()
                .Where(s => s.StartsWith(searchTerm, StringComparison.CurrentCultureIgnoreCase))
                .Take(5);
            return items;
        }
    }
}
