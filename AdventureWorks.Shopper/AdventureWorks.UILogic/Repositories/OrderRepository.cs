// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
using Prism.Windows.AppModel;

namespace AdventureWorks.UILogic.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private const string OrderKey = "CurrentOrderKey";
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IShippingMethodService _shippingMethodService;
        private readonly ISessionStateService _sessionStateService;
        private Order _currentOrder;

        public OrderRepository(IOrderService orderService, IAccountService accountService, IShippingMethodService shippingMethodService, ISessionStateService sessionStateService)
        {
            _orderService = orderService;
            _accountService = accountService;
            _shippingMethodService = shippingMethodService;
            _sessionStateService = sessionStateService;
        }

        public Order CurrentOrder
        {
            get
            {
                if (_currentOrder != null)
                {
                    return _currentOrder;
                }

                var order = _sessionStateService.SessionState[OrderKey] as Order;
                return order;
            }
        }

        public async Task CreateBasicOrderAsync(string userId, ShoppingCart shoppingCart, Address shippingAddress, Address billingAddress, PaymentMethod paymentMethod)
        {
            var basicShippingMethod = await _shippingMethodService.GetBasicShippingMethodAsync();
            _currentOrder = await CreateOrderAsync(userId, shoppingCart, shippingAddress, billingAddress, paymentMethod, basicShippingMethod);
            _sessionStateService.SessionState[OrderKey] = _currentOrder;
        }

        public async Task<Order> CreateOrderAsync(string userId, ShoppingCart shoppingCart, Address shippingAddress, Address billingAddress, PaymentMethod paymentMethod, ShippingMethod shippingMethod)
        {
            Order order = new Order
                {
                    UserId = userId,
                    ShoppingCart = shoppingCart,
                    ShippingAddress = shippingAddress,
                    BillingAddress = billingAddress,
                    PaymentMethod = paymentMethod,
                    ShippingMethod = shippingMethod
                };

            order.Id = await _orderService.CreateOrderAsync(order);

            return order;
        }
    }
}
