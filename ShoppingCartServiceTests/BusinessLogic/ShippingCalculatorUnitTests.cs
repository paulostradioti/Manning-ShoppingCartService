using System;
using System.Collections.Generic;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartServiceTests.Builders;
using Xunit;
using static ShoppingCartServiceTests.Builders.AddressBuilder;
using static ShoppingCartServiceTests.Builders.ItemBuilder;

namespace ShoppingCartServiceTests.BusinessLogic
{
    public class ShippingCalculatorUnitTests
    {
        public static List<object[]> DifferentAddressTypes()
        {
            return new()
            {
                new object[] {CreateAddress(street: "street 1"), CreateAddress(street: "street 2")},
                new object[] {CreateAddress(city: "city 1"), CreateAddress(city: "city 2")},
                new object[] {CreateAddress(country: "country 1"), CreateAddress("country 2")},
            };
        }

        public static List<object[]> AddressTypesWithRates()
        {
            return new()
            {
                new object[]
                {
                    CreateAddress(street: "street 1"),
                    CreateAddress(street: "street 2"),
                    ShippingCalculator.SameCityRate
                },
                new object[]
                {
                    CreateAddress(city: "city 1"),
                    CreateAddress(city: "city 2"),
                    ShippingCalculator.SameCountryRate
                },
                new object[]
                {
                    CreateAddress(country: "country 1"),
                    CreateAddress("country 2"),
                    ShippingCalculator.InternationalShippingRate
                },
            };
        }

        public static List<object[]> ShippingMethodsWithRates()
        {
            return new()
            {
                new object[] {ShippingMethod.Standard, 1.0},
                new object[] {ShippingMethod.Expedited, 1.2},
                new object[] {ShippingMethod.Priority, 2.0},
            };
        }

        // This is an example test, check the previous milestone tests and try to write the missing tests using parameters and builders
        [Theory]
        [MemberData(nameof(DifferentAddressTypes))]
        public void CalculateShippingCost_NoItems_Return0(Address source, Address destination)
        {
            var target = new ShippingCalculator(source);

            var cart = new CartBuilder()
                .WithShippingAddress(destination)
                .Build();

            var result = target.CalculateShippingCost(cart);

            Assert.Equal(0, result);
        }
    }
}