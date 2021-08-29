using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Constants
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Card
        {
            public const string CreateCardForUser = Base + "/users/{userId:int}/cards";
            public const string GetSingleCardForUser = Base + "/users/{userId:int}/cards/{cardId:int}";
            public const string GetCardsForUser = Base + "/users/{userId:int}/cards";
            public const string GetAllCards = Base + "/cards";
        }
    }
}
