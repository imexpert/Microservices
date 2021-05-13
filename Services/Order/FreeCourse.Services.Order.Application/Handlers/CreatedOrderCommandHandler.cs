using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreatedOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreatedOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAdress = new Address(
                request.AddressDto.Province,
                request.AddressDto.District,
                request.AddressDto.Street,
                request.AddressDto.ZipCode, 
                request.AddressDto.Line);

            Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAdress);
            request.OrderItems.ForEach(s =>
            {
                newOrder.AddOrderItem(s.ProductId, s.ProductName, s.Price, s.PictureUrl);
            });

            await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return Response<CreatedOrderDto>.Success(new CreatedOrderDto() { OrderId = newOrder.Id }, 200);
        }
    }
}
