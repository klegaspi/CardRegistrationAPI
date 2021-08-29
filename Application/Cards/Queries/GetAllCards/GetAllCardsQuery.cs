using Application.Cards.Queries.GetCardsForUser;
using MediatR;
using System.Collections.Generic;

namespace Application.Cards.Queries.GetAllCards
{
    public class GetAllCardsQuery : IRequest<List<GetCardDto>>
    {
    }
}
