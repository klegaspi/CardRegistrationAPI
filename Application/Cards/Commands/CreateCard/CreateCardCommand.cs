using MediatR;

namespace Application.Cards.Commands.CreateCard
{
    public class CreateCardCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public long CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int CVC { get; set; }
    }
}
