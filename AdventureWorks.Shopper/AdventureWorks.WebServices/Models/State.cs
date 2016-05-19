

using System.Collections.Generic;

namespace AdventureWorks.WebServices.Models
{
    public class State
    {
        public State(IList<string> validZipCodeRanges)
        {
            ValidZipCodeRanges = validZipCodeRanges;
        }

        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public IList<string> ValidZipCodeRanges { get; private set; }
    }
}