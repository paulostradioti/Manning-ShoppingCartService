using System.Collections.Generic;
using AutoMapper;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using ShoppingCartServiceTests.Builders;
using Xunit;
using static ShoppingCartServiceTests.Builders.AddressBuilder;
using static ShoppingCartServiceTests.Builders.ItemBuilder;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class CheckOutEngineUnitTests
    {
        private readonly IMapper _mapper;

        public CheckOutEngineUnitTests()
        {
            // Ideally do not write any test related logic here
            // Only infrastructure and environment setup

            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));

            _mapper = config.CreateMapper();
        }

        [Theory]
        [InlineData(CustomerType.Standard, 0)]
        [InlineData(CustomerType.Premium, 10)]
        public void CalculateTotals_DiscountBasedOnCustomerType(CustomerType customerType, double expectedDiscount)
        {
            // use Builders and the parameters to write test
        }


        [Theory]
        [InlineData(ShippingMethod.Standard)]
        [InlineData(ShippingMethod.Express)]
        [InlineData(ShippingMethod.Expedited)]
        [InlineData(ShippingMethod.Priority)]
        public void CalculateTotals_StandardCustomer_TotalEqualsCostPlusShipping(ShippingMethod shippingMethod)
        {
            // use Builders and the parameters to write test
        }

        [Fact]
        public void CalculateTotals_MoreThanOneItem_TotalEqualsCostPlusShipping()
        {
            // use Builders and reduce arrange code

            var originAddress = new Address { Country = "country", City = "city 1", Street = "street" };
            var destinationAddress = new Address { Country = "country", City = "city 2", Street = "street" };

            var target = new CheckOutEngine(new ShippingCalculator(originAddress), _mapper);

            var cart = new Cart
            {
                CustomerType = CustomerType.Standard,
                Items = new() { new Item { ProductId = "prod-1", Price = 2, Quantity = 3 } },
                ShippingAddress = destinationAddress
            };

            var result = target.CalculateTotals(cart);

            Assert.Equal((2 * 3) + result.ShippingCost, result.Total);
        }

        // This test was left as in the full solution for reference
        [Fact]
        public void CalculateTotals_PremiumCustomer_TotalEqualsCostPlusShippingMinusDiscount()
        {
            var originAddress = CreateAddress(city: "city 1");
            var destinationAddress = CreateAddress(city: "city 2");

            var target = new CheckOutEngine(new ShippingCalculator(originAddress), _mapper);

            var cart = new CartBuilder()
                .WithCustomerType(CustomerType.Premium)
                .WithShippingAddress(destinationAddress)
                .WithItems(new List<Item>
                {
                    CreateItem(price: 2, quantity: 3)
                })
                .Build();
            var result = target.CalculateTotals(cart);

            Assert.Equal((((2 * 3) + result.ShippingCost) * 0.9), result.Total);
        }
    }
}