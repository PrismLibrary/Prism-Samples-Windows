

using System.Collections.Generic;
using AdventureWorks.WebServices.Repositories;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Strings;

namespace AdventureWorks.WebServices.Controllers
{
    public class AddressController : ApiController
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController():this(new AddressRepository())
        {
            
        }

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        // GET /api/Address
        [Authorize]
        public IEnumerable<Address> GetAll()
        {
            return _addressRepository.GetAll(this.User.Identity.Name);
        }
        
        // POST /api/Address
        [Authorize]
        public HttpResponseMessage PostAddress(Address address)
        {
            if (address == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Resources.InvalidAddress);
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _addressRepository.AddUpdate(this.User.Identity.Name, address);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        // PUT /api/Address?defaultAddressId=[defaultAddressId]&addressType=[addressType]
        [Authorize]
        public HttpResponseMessage Put(string defaultAddressId, AddressType addressType)
        {
            _addressRepository.SetDefault(this.User.Identity.Name, defaultAddressId, addressType);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
