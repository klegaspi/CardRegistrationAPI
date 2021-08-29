using Application.Cards.Commands.CreateCard;
using Application.Cards.Queries.GetCardsForUser;
using FluentAssertions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class CardControllerTests : TestBase
    {
        #region GetSingleCardForUser tests
        [Fact]
        public async Task GetSingleCardForUser_WithCard_ReturnsSuccess()
        {
            //Arrange
            var userId = 1;
            var cardId = 1;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards/{cardId}");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            var card = JsonConvert.DeserializeObject<GetCardDto>(result);
            card.CardNumber.Should().Be(4111111111111111);
            card.CVC.Should().Be(123);
            card.ExpiryMonth.Should().Be(12);
            card.ExpiryYear.Should().Be(2025);
            card.Name.Should().Be("Kenshin");
        }

        [Fact]
        public async Task GetSingleCardForUser_UserIdIsNotExisting_ReturnsNotFound()
        {
            //Arrange
            var nonExistingUserId = 500;
            var cardId = 1;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{nonExistingUserId}/cards/{cardId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSingleCardForUser_UserIdIsZero_ReturnsBadRequest()
        {
            //Arrange
            var userId = 0;
            var cardId = 1;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards/{cardId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetSingleCardForUser_UserIdIsNegative_ReturnsBadRequest()
        {
            //Arrange
            var userId = -1;
            var cardId = 1;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards/{cardId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetSingleCardForUser_CardIdIsNotExisting_ReturnsNotFound()
        {
            //Arrange
            var nonExistingUserId = 1;
            var cardId = 1000;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{nonExistingUserId}/cards/{cardId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetSingleCardForUser_CardIdIsZero_ReturnsBadRequest()
        {
            //Arrange
            var userId = 1;
            var cardId = 0;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards/{cardId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetSingleCardForUser_CardIdIsNegative_ReturnsBadRequest()
        {
            //Arrange
            var userId = 1;
            var cardId = -1;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards/{cardId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region GetCardsForUser tests
        [Fact]
        public async Task GetCardsForUser_WithData_ReturnsSuccess()
        {
            //Arrange
            var userId = 1;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            var cards = JsonConvert.DeserializeObject<List<GetCardDto>>(result);
            cards.Count.Should().Be(2);
        }

        [Fact]
        public async Task GetCardsForUser_UserIdIsNotExisting_Returns200StatusCode()
        {
            //Arrange
            var nonExistingUserId = 500;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{nonExistingUserId}/cards");
            var result = response.Content.ReadAsStringAsync().Result;
            var cards = JsonConvert.DeserializeObject<List<GetCardDto>>(result);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            cards.Count.Should().Be(0);
        }

        [Fact]
        public async Task GetCardsForUser_UserIdIsZero_ReturnsBadRequest()
        {
            //Arrange
            var userId = 0;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetCardsForUser_UserIdIsNegative_ReturnsBadRequest()
        {
            //Arrange
            var userId = -1;

            //Act
            var response = await TestClient.GetAsync($"/api/v1/users/{userId}/cards");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region GetAllCards tests
        [Fact]
        public async Task GetAllCards_WithData_ReturnsSuccess()
        {
            //Arrange

            //Act
            var response = await TestClient.GetAsync($"/api/v1/cards");

            //Assert
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            var cards = JsonConvert.DeserializeObject<List<GetCardDto>>(result);
            cards.Count.Should().Be(3);
        }
        #endregion

        #region CreateCardForUser tests
        [Fact]
        public async Task CreateCardForUser_Returns201StatusCode()
        {
            //Arrange
            var userId = 1;

            //Act
            var payload = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 199,
                ExpiryMonth = 12,
                ExpiryYear = 2025,
                UserId = userId
            };
            var myContent = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
            var response = await TestClient.PostAsync($"/api/v1/users/{userId}/cards/", stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task CreateCardForUser_InvalidCreditCardNumber_ReturnsBadRequest()
        {
            //Arrange
            var userId = 1;

            //Act
            var payload = new CreateCardDto
            {
                CardNumber = 1,
                CVC = 234,
                ExpiryMonth = 8,
                ExpiryYear = 2025,
                UserId = userId
            };
            var myContent = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
            var response = await TestClient.PostAsync($"/api/v1/users/{userId}/cards/", stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateCardForUser_InvalidCVC_ReturnsBadRequest()
        {
            //Arrange
            var userId = 1;

            //Act
            var payload = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 1992,
                ExpiryMonth = 8,
                ExpiryYear = 2025,
                UserId = userId
            };
            var myContent = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
            var response = await TestClient.PostAsync($"/api/v1/users/{userId}/cards/", stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateCardForUser_InvalidExpiryMonth_ReturnsBadRequest()
        {
            //Arrange
            var userId = 1;

            //Act
            var payload = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 192,
                ExpiryMonth = 17,
                ExpiryYear = 2025,
                UserId = userId
            };
            var myContent = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
            var response = await TestClient.PostAsync($"/api/v1/users/{userId}/cards/", stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateCardForUser_InvalidExpiryYear_ReturnsBadRequest()
        {
            //Arrange
            var userId = 1;

            //Act
            var payload = new CreateCardDto
            {
                CardNumber = 5555555555554444,
                CVC = 199,
                ExpiryMonth = 12,
                ExpiryYear = 1988,
                UserId = userId
            };
            var myContent = JsonConvert.SerializeObject(payload);
            var stringContent = new StringContent(myContent, UnicodeEncoding.UTF8, "application/json");
            var response = await TestClient.PostAsync($"/api/v1/users/{userId}/cards/", stringContent);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion
    }
}