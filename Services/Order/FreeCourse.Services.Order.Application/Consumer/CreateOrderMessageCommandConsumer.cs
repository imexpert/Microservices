using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAdress = new Domain.OrderAggregate.Address(
                context.Message.Province,
                context.Message.District,
                context.Message.Street,
                context.Message.ZipCode,
                context.Message.Line);

            Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(
                context.Message.BuyerId, newAdress);

            context.Message.OrderItems.ForEach(s =>
            {
                newOrder.AddOrderItem(s.ProductId, s.ProductName, s.Price, s.PictureUrl);
            });


            await _orderDbContext.Orders.AddAsync(newOrder);

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
