using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Echelon.Data.Entities.Users;
using Echelon.Infrastructure.AutoMapper.Profiles;
using NUnit.Framework;

namespace Echelon.Tests.Mapping
{
    public class MapperTests
    {
        [Test]
        public void Test_UserProfile_MappingSuccess()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<UserProfile>(); });


            var tempUserEntity = new TempUserEntity
            {
                Email = "Test@Test.com",
                Password = "TestPassword",
                UserName = "TestDisplayName"
            };
            var userEntity = Mapper.Instance.Map<UserEntity>(tempUserEntity);

            Assert.That(userEntity.UserName.Equals(tempUserEntity.UserName));
        }

        public class ProductProfile : Profile
        {
            public ProductProfile()
            {
                CreateMap<Product, List<AnalyticsItem>>().ConvertUsing(x => new List<AnalyticsItem>()
                {
                    new AnalyticsItem("name", x.Name),
                    new AnalyticsItem("id", x.Sku),
                    new AnalyticsItem("price", x.Price),
                    new AnalyticsItem("brand", x.Brand),
                    new AnalyticsItem("category", x.Category),
                    new AnalyticsItem("list", "Recommended Products"),
                    new AnalyticsItem("position", 1)
                });
            }
        }

        [Test]
        public void Method_Scenario_Result()
        {
            Mapper.Initialize(cfg => { cfg.AddProfile<ProductProfile>(); });
            var count = 1;

            var Products = new List<Product>()
            {
                new Product {Name = "test1", Brand = "brand1", Category = "category1", Price = 10, Sku = "sku1"},
                new Product {Name = "test2", Brand = "brand2", Category = "category2", Price = 12, Sku = "sku2"},
                new Product {Name = "test3", Brand = "brand3", Category = "category3", Price = 13, Sku = "sku3"},
                new Product {Name = "test4", Brand = "brand4", Category = "category4", Price = 14, Sku = "sku4"}
            };

            //var list = Mapper.Instance.Map<List<List<AnalyticsItem>>>(Products, opt => opt.ServiceCtor()["position"] = count++);
            //var list = Mapper.Instance.Map<List<List<AnalyticsItem>>>(Products);
            //list.ForEach(x =>
            //{
            //    var t = x.Where(y => y.Key == "position");
            //    foreach (var analyticsItem in t)
            //    {
            //        analyticsItem.Value = count++;
            //    }
            //});

            //var enumerable = list.Select(x =>
            //{
            //    foreach (var analyticsItem in x.Where(y => y.Key == "position"))
            //    {
            //        analyticsItem.Value = count++;
            //    }
            //    return x;
            //});

            var list = Products.Select((a, ix) =>
            {
                var b = Mapper.Instance.Map<List<AnalyticsItem>>(a);
                
                foreach (var b1 in b.Where(y => y.Key == "position"))
                {
                    b1.Value = count++;
                }

                return b;
            }).ToList();

            Console.WriteLine(@"Done");
        }
    }

    public class AnalyticsItem
    {
        public AnalyticsItem(string key, object value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
        public string Sku { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
    }
}