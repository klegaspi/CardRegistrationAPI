using Application.Cards.Queries.GetCardsForUser;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cards.Queries.GetAllCards
{
    public class GetAllCardsQueryHandler : IRequestHandler<GetAllCardsQuery, List<GetCardDto>>
    {
        private readonly IApplicationDbContext _dbContext;
        public GetAllCardsQueryHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<GetCardDto>> Handle(GetAllCardsQuery request, CancellationToken cancellationToken)
        {
            var cards = await _dbContext
                                    .Cards
                                    .Include(a=>a.User)
                                    .AsNoTracking()
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
