using Application.Cards.Queries.GetCardsForUser;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cards.Queries.GetSingleCard
{
    public class GetSingleCardQueryHandler : IRequestHandler<GetSingleCardQuery, GetCardDto>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetSingleCardQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetCardDto> Handle(GetSingleCardQuery request, CancellationToken cancellationToken)
        {
            var card = await _dbContext
                .Cards
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.UserId == request.UserId && a.CardId == request.CardId);

            if (card == null)
            {
                return null;
            }

            return new GetCardDto
            {
                CardId = card.CardId,
                CardNumber = card.CardNumber,
                CVC = card.CVC,
                ExpiryMonth = card.ExpiryMonth,
                ExpiryYear = card.ExpiryYear,
                Name = card.User.Name,
                UserId = card.User.UserId
            };
        }
    }
}
