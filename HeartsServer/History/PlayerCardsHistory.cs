using Hearts_server.GameLogic;
using Hearts_server.GameLogic.Cards;
using System;

namespace Hearts_server.History
{
    public class PlayerCardsHistory : List<Card>
    {
        public Player Owner { get; set; }
    }
}
