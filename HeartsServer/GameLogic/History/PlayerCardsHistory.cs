using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using System;

namespace HeartsServer.GameLogic.History
{
    public class PlayerCardsHistory : List<Card>
    {
        public Player Owner { get; set; }
    }
}
