using MediatR;
using System.Collections.Generic;

namespace Application.Cards.Queries.GetCardsForUser
{
    public class GetCardsForUserQuery : IRequest<List<GetCardDto>>
    {
        public int UserId { get; set; }
    }
}
