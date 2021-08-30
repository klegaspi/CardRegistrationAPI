using Domain.Entities;
using Infrastructure.Data;

namespace IntegrationTests.Common
{
    public class Utilities
    {
        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            db.Users.Add(new User { UserId = 1000, Name = "Kenshin" });
            db.Users.Add(new User { UserId = 1001, Name = "Kamila" });
            db.Cards.Add(new Card { CardNumber = 4111111111111111, CVC = 123, ExpiryMonth = 12, ExpiryYear = 2025, UserId = 1000  } );
            db.Cards.Add(new Card { CardNumber = 4012888888881881, CVC = 123, ExpiryMonth = 12, ExpiryYear = 2025, UserId = 1000 });
            db.Cards.Add(new Card { CardNumber = 5105105105105100, CVC = 200, ExpiryMonth = 8, ExpiryYear = 2024, UserId = 1001 });
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(ApplicationDbContext db)
        {
            db.Cards.RemoveRange(db.Cards);
            db.Users.RemoveRange(db.Users);
            InitializeDbForTests(db);
        }
    }
}
