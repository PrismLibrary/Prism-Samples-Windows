

using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using AdventureWorks.WebServices.Strings;

namespace AdventureWorks.WebServices.Controllers
{
    [Authorize]
    public class OrderController : ApiController
    {
        private IRepository<Order> _orderRepository;

        public OrderController()
            : this(new OrderRepository())
        { }

        public OrderController(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET /api/order/id
        [HttpGet]
        public Order GetOrder(int id)
        {
            var order = _orderRepository.GetItem(id);

            if (order == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return order;
        }

        // POST /api/order/create
        [HttpPost]
        public HttpResponseMessage CreateOrder(Order order)
        {
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Resources.InvalidOrder);
            }

            if (ModelState.IsValid)
            {
                order = _orderRepository.Create(order);
                var response = Request.CreateResponse(HttpStatusCode.Created, order.Id);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = order.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // PUT /api/order/process 
        [HttpPut]
        public HttpResponseMessage ProcessOrder(int id, Order order)
        {
            if (order == null || id != order.Id)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Resources.InvalidOrder);
            }

            if (ModelState.IsValid)
            {
                // This is where you would add custom business logic validation (check stock, approve transaction, etc)
                // for instance, validate the transaction before performing the purchase

                if (order.ShoppingCart.ShoppingCartItems.Count < 1)
                {
                    ModelState.AddModelError("order.ShoppingCart", Resources.InvalidShoppingCart);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                var result = order.PaymentMethod.CardNumber != "22222" ? "APPROVED" : string.Format(CultureInfo.CurrentCulture, "Invalid Payment Method. Reason: {0}", "DECLINED_CONTACT_YOUR_BANK");
                if (result == "APPROVED")
                {
                    // This is where you would process the order. It is omitted for simplicity of the back end service.
                    _orderRepository.Delete(order.Id);
                    return Request.CreateResponse();
                }
                else
                {
                    ModelState.AddModelError("order.PaymentMethod", result);
                }
            }

            // Only get here if there are ModelState errors
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Used for testing
        /// </summary>
        /// <param name="resetData">Flag to differentiate the posts by query string parameter</param>
        [HttpPost]
        [AllowAnonymous]
        public void Reset(bool resetData)
        {
            if (resetData)
                _orderRepository.Reset();
        }
    }
}
