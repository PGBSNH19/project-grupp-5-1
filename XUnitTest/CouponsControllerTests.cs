using AutoMapper;
using Backend.Controllers;
using Backend.DTO;
using Backend.Models;
using Backend.Services.Interfaces;
using Backend.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestBackend
{
    public class CouponsControllerTests
    {
        [Fact]
        public async Task GetAll_Status200OKIsType_ListOfAllCoupons()
        {
            //Arrange
            var mockRepo = new Mock<ICouponRepository>();
            mockRepo.Setup(repo => repo.GetAll()).Returns(Task.FromResult(GetTestCoupons()));

            var logger = Mock.Of<ILogger<CouponsController>>();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IList<CouponDTO>>(It.IsAny<IList<Coupon>>()))
                .Returns((List<Coupon> source) => new List<CouponDTO>()
                {
                    new CouponDTO(){Id = 1},
                    new CouponDTO(){Id = 2}
                });
            
            var controller = new CouponsController(logger, mockRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IList<CouponDTO>>(okResult.Value);
            var coupons = returnValue;
            Assert.Equal(2, coupons.Count());
            
        }

        [Theory]
        [InlineData(1, 1)]
        public async Task GetById_Status200OKIsType_ReturnedIdContains(int input, int expectedId)
        {
            //Arrange
            var mockRepo = new Mock<ICouponRepository>();
            mockRepo.Setup(repo => repo.Get(input)).Returns(Task.FromResult(GetTestSession()));

            var logger = Mock.Of<ILogger<CouponsController>>();

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<CouponDTO>(It.IsAny<Coupon>()))
                .Returns((Coupon source) => new CouponDTO()
                {
                    Id = 1
                });

            var controller = new CouponsController(logger, mockRepo.Object, mockMapper.Object);

            // Act
            var result = await controller.GetById(input);   
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CouponDTO>(okResult.Value);
            var coupon = returnValue;
            Assert.Equal(expectedId, coupon.Id);
            Assert.Contains(coupon.Id.ToString(), coupon.Id.ToString());
        }

        [Fact]
        public async Task GetById_GetReturnsNotFound()
        {
            // Arrange
            var mockRepo = new Mock<ICouponRepository>();
            mockRepo.Setup(repo => repo.Get(2)).Returns(Task.FromResult(GetTestSession()));

            var logger = Mock.Of<ILogger<CouponsController>>();

            var mapp = Mock.Of<IMapper>();
            var mockMapper = new Mock<IMapper>();

            var controller = new CouponsController(logger, mockRepo.Object, mockMapper.Object);

            // Act
            IActionResult actionResult = await controller.GetById(1);

            // Assert
            Assert.IsAssignableFrom<NotFoundObjectResult>(actionResult);
        }

        private Coupon GetTestSession()
        {
            var session = new Coupon
            {
                Id = 1,
                Code = "XUNIT TEST"
            };

            return session;
        }

        private IList<Coupon> GetTestCoupons()
        {
            var sessions = new List<Coupon>();
            sessions.Add(new Coupon()
            {
                Id = 1,
                Code = "Coupon1",

            });
            sessions.Add(new Coupon()
            {
                Id = 2,
                Code = "Coupon2",

            });
            return sessions;
        }
    }
}
