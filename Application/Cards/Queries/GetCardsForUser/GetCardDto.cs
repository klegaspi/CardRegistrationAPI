namespace Application.Cards.Queries.GetCardsForUser
{
    public class GetCardDto
    {
        public int CardId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public long CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public int CVC { get; set; }
    }
}
