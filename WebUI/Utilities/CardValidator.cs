using Application.Cards.Commands.CreateCard;
using System;
using System.Text.RegularExpressions;

namespace WebUI.Utilities
{
    public static class CardValidator
    {
        public static (bool IsValid, string ErrorMessage) ValidateCardDetails(CreateCardDto payload)
        {
            if (CreditCardValidator.Luhn.CheckLuhn(payload.CardNumber.ToString()) == false)
            {
                return (false, "Invalid credit card number");
            }

            if (!Regex.IsMatch(payload.CVC.ToString(), @"\b\d{3}\b"))
            {
                return (false, "Invalid CVC");
            }

            if (payload.ExpiryMonth < 1 || payload.ExpiryMonth > 12)
            {
                return (false, "Invalid expiry month");
            }

            if (!IsValidExpiryYear(payload.ExpiryMonth, payload.ExpiryYear))
            {
                return (false, "Invalid expiry year");
            }

            return (true, string.Empty);
        }

        public static bool IsValidExpiryYear(int month, int year)
        {
            DateTime dateValue;

            if (DateTime.TryParse(new DateTime(year, month, 1).ToShortDateString(), out dateValue))
                if (dateValue < DateTime.Now)
                    return false;
                else
                    return true;
            else
                return false;
        }
    }
}
