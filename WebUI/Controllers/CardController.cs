using Application.Cards.Commands.CreateCard;
using Application.Cards.Queries.GetAllCards;
using Application.Cards.Queries.GetCardsForUser;
using Application.Cards.Queries.GetSingleCard;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebUI.Constants;
using WebUI.Utilities;

namespace WebUI.Controllers
{
    [ApiController]
    [Route("")]
    public class CardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(ApiRoutes.Card.CreateCardForUser)]
        public async Task<IActionResult> CreateCardForUser(int userId, CreateCardDto payload)
        {
            if (payload == null)
            {
                return BadRequest();
            }

            if (!userId.Equals(payload.UserId))
            {
                return BadRequest();
            }

            var validationResult = CardValidator.ValidateCardDetails(payload);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ErrorMessage);
            }

            var command = new CreateCardCommand
            {
                CardNumber = payload.CardNumber,
                CVC = payload.CVC,
                ExpiryMonth = payload.ExpiryMonth,
                ExpiryYear = payload.ExpiryYear,
                UserId = payload.UserId
            };

            var cardId = await _mediator.Send(command);

            return CreatedAtAction("GetSingleCardForUser", new { userId = payload.UserId, cardId = cardId }, command);
        }

        [HttpGet(ApiRoutes.Card.GetSingleCardForUser)]
        public async Task<ActionResult<GetCardDto>> GetSingleCardForUser(int userId, int cardId)
        {
            if (userId <= 0 || cardId <= 0)
            {
                return BadRequest();
            }

            var query = new GetSingleCardQuery { UserId = userId, CardId = cardId };
            var response = await _mediator.Send(query);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet(ApiRoutes.Card.GetCardsForUser)]
        public async Task<ActionResult<List<GetCardDto>>> GetCardsForUserId(int userId)
        {
            if (userId <= 0)
            {
                return BadRequest();
            }

            var query = new GetCardsForUserQuery { UserId = userId };
            var response = await _mediator.Send(query);

            return Ok(response);
        }

        [HttpGet(ApiRoutes.Card.GetAllCards)]
        public async Task<ActionResult<List<GetCardDto>>> GetAllCards()
        {
            var query = new GetAllCardsQuery();
            var response = await _mediator.Send(query);

            return Ok(response);
        }
    }
}
