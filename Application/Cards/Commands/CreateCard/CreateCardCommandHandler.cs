using Domain.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Cards.Commands.CreateCard
{
    public class CreateCardCommandHandler : IRequestHandler<CreateCardCommand, int>
    {
        private readonly IApplicationDbContext _dbContext;
        public CreateCardCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var card = new Card
            {
                CardNumber = request.CardNumber,
                CVC = request.CVC,
                ExpiryMonth = request.ExpiryMonth,
                ExpiryYear = request.ExpiryYear,
                UserId = request.UserId
            };

            _dbContext.Cards.Add(card);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return card.CardId;
        }
    }
}
