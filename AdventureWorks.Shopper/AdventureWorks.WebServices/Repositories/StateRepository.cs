

using System;
using System.Collections.Generic;
using System.Linq;
using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public class StateRepository : IRepository<State>
    {
        private static IEnumerable<State> _states = PopulateStates();

        public IEnumerable<State> GetAll()
        {
            lock (_states)
            {
                // Return new collection so callers can iterate independently on separate threads
                return _states.ToArray();
            }
        }

        public State GetItem(int id)
        {
            throw new NotImplementedException();
        }

        public State Create(State item)
        {
            throw new NotImplementedException();
        }

        public bool Update(State item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<State> PopulateStates()
        {
            return new List<State>
            {
                new State(new[] {"350-369"}) { Code = "AL", Name = "Alabama"},
                new State(new [] {"995-999"}) { Code = "AK", Name = "Alaska"},
                new State(new [] { "850-865" }) { Code = "AZ", Name = "Arizona"},
                new State(new [] { "716-729","755-755" }) { Code = "AR", Name = "Arkansas"},
                new State(new [] { "900-966" }) { Code = "CA", Name = "California"},
                new State(new [] { "800-816" }) { Code = "CO", Name = "Colorado"},
                new State(new [] { "060-069" }) { Code = "CT", Name = "Connecticut"},
                new State(new [] { "197-199" }) { Code = "DE", Name = "Delaware"},
                new State(new [] { "320-349" }) { Code = "FL", Name = "Florida"},
                new State(new [] { "300-319","398-399" }) { Code = "GA", Name = "Georgia"},
                new State(new [] { "967-968" }) { Code = "HI", Name = "Hawaii"},
                new State(new [] { "832-838" }) { Code = "ID", Name = "Idaho"},
                new State(new [] { "600-629" }) { Code = "IL", Name = "Illinois"},
                new State(new [] { "460-479" }) { Code = "IN", Name = "Indiana"},
                new State(new [] { "500-528" }) { Code = "IA", Name = "Iowa"},
                new State(new [] { "660-679" }) { Code = "KS", Name = "Kansas"},
                new State(new [] { "400-427" }) { Code = "KY", Name = "Kentucky"},
                new State(new [] { "700-714" }) { Code = "LA", Name = "Louisiana"},
                new State(new [] { "039-049"}) { Code = "ME", Name = "Maine"},
                new State(new [] { "206-219" }) { Code = "MD", Name = "Maryland"},
                new State(new [] { "010-027","055-055" }) { Code = "MA", Name = "Massachusetts"},
                new State(new [] { "480-499" }) { Code = "MI", Name = "Michigan"},
                new State(new [] { "550-567" }) { Code = "MN", Name = "Minnesota"},
                new State(new [] { "386-397" }) { Code = "MS", Name = "Mississippi"},
                new State(new [] { "630-658" }) { Code = "MO", Name = "Missouri"},
                new State(new [] { "590-599" }) { Code = "MT", Name = "Montana"},
                new State(new [] { "680-693" }) { Code = "NE", Name = "Nebraska"},
                new State(new [] { "889-898" }) { Code = "NV", Name = "Nevada"},
                new State(new [] { "030-039" }) { Code = "NH", Name = "New Hampshire"},
                new State(new [] { "070-089" }) { Code = "NJ", Name = "New Jersey"},
                new State(new [] { "870-884" }) { Code = "NM", Name = "New Mexico"},
                new State(new [] { "005,063","090-149" }) { Code = "NY", Name = "New York"},
                new State(new [] { "269-289" }) { Code ="NC", Name = "North Carolina"},
                new State(new [] { "580-588"}) { Code = "ND", Name = "North Dakota"},
                new State(new [] { "430-459" }) { Code = "OH", Name = "Ohio"},
                new State(new [] { "730-749" }) { Code = "OK", Name = "Oklahoma"},
                new State(new [] { "970-979" }) { Code = "OR", Name = "Oregon"},
                new State(new [] { "150-196" }) { Code = "PA", Name = "Pennsylvania"},
                new State(new [] { "028-029" }) { Code = "RI", Name = "Rhode Island"},
                new State(new [] { "290-299" }) { Code = "SC", Name = "South Carolina"},
                new State(new [] { "570-577" }) { Code = "SD", Name = "South Dakota"},
                new State(new [] { "70-385" }) { Code = "TN", Name = "Tennessee"},
                new State(new [] { "750-799","885-885" }) { Code = "TX", Name = "Texas"},
                new State(new [] { "840-847" }) { Code = "UT", Name = "Utah"},
                new State(new [] { "050-059" }) { Code ="VT", Name = "Vermont"},
                new State(new [] { "201-201","220-246" }) { Code = "VA", Name = "Virginia"},
                new State(new [] { "980-994" }) { Code = "WA", Name = "Washington"},
                new State(new [] { "530-549" }) { Code = "WI", Name = "Wisconsin"},
                new State(new [] { "247-268" }) { Code = "WV", Name = "West Virginia"},
                new State(new [] { "820-831" }) { Code = "WY", Name = "Wyoming"}
            };
        }


        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}