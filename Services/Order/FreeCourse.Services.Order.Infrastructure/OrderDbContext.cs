﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastructure
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SHEMA = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {

        }

        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SHEMA);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SHEMA);

            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(s => s.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(s => s.Address).WithOwner();

            base.OnModelCreating(modelBuilder);
        }
    }
}
