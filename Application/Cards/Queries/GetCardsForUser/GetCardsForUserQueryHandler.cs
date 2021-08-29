using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cards.Queries.GetCardsForUser
{
    public class GetCardsForUserQueryHandler : IRequestHandler<GetCardsForUserQuery, List<GetCardDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetCardsForUserQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<GetCardDto>> Handle(GetCardsForUserQuery request, CancellationToken cancellationToken)
        {
            var cards = await _dbContext
                                    .Cards
                                    .Include(a=>a.User)
                                    .AsNoTracking()
                                    .Where(a => a.UserId == request.UserId)
                                    .ToListAsync();

            return cards.Select(card => new GetCardDto
            {
                CardId = card.CardId,
                CardNumber = card.CardNumber,
                CVC = card.CVC,
                ExpiryMonth = card.ExpiryMonth,
                ExpiryYear = card.ExpiryYear,
                Name = card.User.Name,
                UserId = card.User.UserId
            }).ToList();
        }
    }
}
