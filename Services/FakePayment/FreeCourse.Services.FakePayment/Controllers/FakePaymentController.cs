using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            CreateOrderMessageCommand createOrderMessageCommand = new CreateOrderMessageCommand();
            createOrderMessageCommand.BuyerId = paymentDto.Order.BuyerId;
            createOrderMessageCommand.District = paymentDto.Order.Address.District;
            createOrderMessageCommand.Line = paymentDto.Order.Address.Line;
            createOrderMessageCommand.Province = paymentDto.Order.Address.Province;
            createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
            createOrderMessageCommand.ZipCode = paymentDto.Order.Address.ZipCode;

            paymentDto.Order.OrderItems.ForEach(s =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem()
                {
                    PictureUrl = s.PictureUrl,
                    Price = s.Price,
                    ProductId = s.ProductId,
                    ProductName = s.ProductName
                });
            });
            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

            return CreateActionResultInstance<NoContent>(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
