using Application.Cards.Commands.CreateCard;
using Application.Cards.Queries.GetAllCards;
using Application.Cards.Queries.GetCardsForUser;
using Application.Cards.Queries.GetSingleCard;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebUI.Controllers;
using Xunit;

namespace UnitTests
{
    public class CardControllerTests
    {
        #region CreateCard tests
        [Fact]
        public async Task CreateCard_ValidData_Returns201StatusCode()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            int userId = 1;
            object cardId = 100; 
            var dto = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2030,
                UserId = userId
            };
            mediator.Setup(a => a.Send(dto, new CancellationToken())).Returns(Task.FromResult(cardId));
            var sut = new CardController(mediator.Object);
            
            //Act
            var result = (CreatedAtActionResult)await sut.CreateCardForUser(userId, dto);

            //Assert
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task CreateCard_InvalidCardNumber_ReturnsBadRequest()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            int userId = 1;
            object cardId = 100;
            var dto = new CreateCardDto
            {
                CardNumber = 1,
                CVC = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2030,
                UserId = userId
            };
            mediator.Setup(a => a.Send(dto, new CancellationToken())).Returns(Task.FromResult(cardId));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.CreateCardForUser(userId, dto);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCard_InvalidCVC_ReturnsBadRequest()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            int userId = 1;
            object cardId = 100;
            var dto = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 1,
                ExpiryMonth = 12,
                ExpiryYear = 2030,
                UserId = userId
            };
            mediator.Setup(a => a.Send(dto, new CancellationToken())).Returns(Task.FromResult(cardId));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.CreateCardForUser(userId, dto);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCard_InvalidExpiryMonth_ReturnsBadRequest()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            int userId = 1;
            object cardId = 100;
            var dto = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 123,
                ExpiryMonth = 20,
                ExpiryYear = 2030,
                UserId = userId
            };
            mediator.Setup(a => a.Send(dto, new CancellationToken())).Returns(Task.FromResult(cardId));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.CreateCardForUser(userId, dto);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCard_InvalidExpiryYear_ReturnsBadRequest()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            int userId = 1;
            object cardId = 100;
            var dto = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 123,
                ExpiryMonth = 12,
                ExpiryYear = 1980,
                UserId = userId
            };
            mediator.Setup(a => a.Send(dto, new CancellationToken())).Returns(Task.FromResult(cardId));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.CreateCardForUser(userId, dto);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateCard_InvalidUserId_ReturnsBadRequest()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            int userId = 2;
            object cardId = 100;
            var dto = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                UserId = 1
            };
            mediator.Setup(a => a.Send(dto, new CancellationToken())).Returns(Task.FromResult(cardId));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.CreateCardForUser(userId, dto);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }
        #endregion

        #region GetSingleCardForUser tests

        [Fact]
        public async Task GetSingleCardForUser_ExistingRecord_Returns200StatusCode()
        {
            //Arrange
            var mediator = new Mock<IMediator>();

            var card = new GetCardDto
            {
                CardId = 100,
                CardNumber = 5555555555554444,
                CVC = 123,
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                Name = "Jane",
                UserId = 1
            };

            mediator.Setup(a => a.Send(It.IsAny<GetSingleCardQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(card));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.GetSingleCardForUser(card.UserId, card.CardId);

            //Assert
            var okResult = (OkObjectResult)result.Result;
            var actual = (GetCardDto)okResult.Value;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            actual.CardId.Should().Be(card.CardId);
            actual.CardNumber.Should().Be(card.CardNumber);
            actual.CardId.Should().Be(card.CardId);
            actual.ExpiryMonth.Should().Be(card.ExpiryMonth);
            actual.ExpiryYear.Should().Be(card.ExpiryYear);
            actual.Name.Should().Be(card.Name);
            actual.UserId.Should().Be(card.UserId);
        }

        [Fact]
        public async Task GetSingleCardForUser_NonExistingRecord_Returns404StatusCode()
        {
            //Arrange
            var mediator = new Mock<IMediator>();

            GetCardDto card = null;

            mediator.Setup(a => a.Send(It.IsAny<GetSingleCardQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(card));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.GetSingleCardForUser(100, 200);

            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }
        #endregion

        #region GetCardsForUser tests

        [Fact]
        public async Task GetCardsForUser_WithData_Returns200StatusCode()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            var userId = 1;
            var cards = new List<GetCardDto>
            {
                new GetCardDto {
                    CardId = 100,
                    CardNumber = 5555555555554444,
                    CVC = 123,
                    ExpiryMonth = 12,
                    ExpiryYear = 2025,
                    Name = "Jane",
                    UserId = 1
                },
                new GetCardDto {
                    CardId = 101,
                    CardNumber = 5105105105105100,
                    CVC = 113,
                    ExpiryMonth = 9,
                    ExpiryYear = 2025,
                    Name = "Jane",
                    UserId = 1
                }
            };

            mediator.Setup(a => a.Send(It.IsAny<GetCardsForUserQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(cards));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.GetCardsForUserId(userId);

            //Assert
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<GetCardDto>)okResult.Value;
            actual.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetCardsForUser_WithoutData_ReturnsNoRecords()
        {
            //Arrange
            var mediator = new Mock<IMediator>();
            var userId = 1;
            var cards = new List<GetCardDto>();

            mediator.Setup(a => a.Send(It.IsAny<GetCardsForUserQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(cards));
            var sut = new CardController(mediator.Object);

            //Act
            var result = await sut.GetCardsForUserId(userId);

            //Assert
            var okResult = (OkObjectResult)result.Result;
            var actual = (List<GetCardDto>)okResult.Value;
            actual.Count.Should().Be(0);
        }
        #endregion

        #region GetAllCards tests
        //[Fact]
        //public async Task GetAllCards_WithData_Returns200StatusCode()
        //{
        //    //Arrange
        //    var mediator = new Mock<IMediator>();
        //    var cards = new List<GetCardDto>
        //    {
        //        new GetCardDto {
        //            CardId = 100,
        //            CardNumber = 5555555555554444,
        //            CVC = 123,
        //            ExpiryMonth = 12,
        //            ExpiryYear = 2025,
        //            Name = "Jane",
        //            UserId = 1
        //        },
        //        new GetCardDto {
        //            CardId = 101,
        //            CardNumber = 5105105105105100,
        //            CVC = 113,
        //            ExpiryMonth = 9,
        //            ExpiryYear = 2025,
        //            Name = "Jane",
        //            UserId = 1
        //        },
        //        new GetCardDto {
        //            CardId = 101,
        //            CardNumber = 5555555555554444,
        //            CVC = 878,
        //            ExpiryMonth = 5,
        //            ExpiryYear = 2027,
        //            Name = "Kristian",
        //            UserId = 2
        //        }
        //    };

        //    mediator.Setup(a => a.Send(It.IsAny<GetAllCardsQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(cards));
        //    var sut = new CardController(mediator.Object);

        //    //Act
        //    var result = await sut.GetAllCards();

        //    //Assert
        //    var okResult = (OkObjectResult)result.Result;
        //    var actual = (List<GetCardDto>)okResult.Value;
        //    actual.Count.Should().Be(3);
        //}

        //[Fact]
        //public async Task GetAllCards_WithoutData_ReturnsNoRecords()
        //{
        //    //Arrange
        //    var mediator = new Mock<IMediator>();
        //    var cards = new List<GetCardDto>();

        //    mediator.Setup(a => a.Send(It.IsAny<GetAllCardsQuery>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(cards));
        //    var sut = new CardController(mediator.Object);

        //    //Act
        //    var result = await sut.GetAllCards();

        //    //Assert
        //    var okResult = (OkObjectResult)result.Result;
        //    var actual = (List<GetCardDto>)okResult.Value;
        //    actual.Count.Should().Be(0);
        //}
        #endregion
    }
}