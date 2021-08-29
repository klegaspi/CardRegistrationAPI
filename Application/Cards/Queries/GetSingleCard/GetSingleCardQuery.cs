using Application.Cards.Queries.GetCardsForUser;
using MediatR;

namespace Application.Cards.Queries.GetSingleCard
{
    public class GetSingleCardQuery : IRequest<GetCardDto>
    {
        public int UserId { get; set; }
        public int CardId { get; set; }
    }
}
