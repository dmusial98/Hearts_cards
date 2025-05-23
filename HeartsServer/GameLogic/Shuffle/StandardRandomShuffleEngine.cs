﻿using Hearts_server.GameLogic.Cards;
using HeartsServer.GameLogic.Consts;

namespace HeartsServer.GameLogic.Shuffle
{
	public class StandardRandomShuffleEngine : BaseShuffleEngine
	{
		public override List<List<Card>> Shuffle(Card[] cards)
		{
			if (cards is null || base.IsAnyCardNull(cards))
				throw new ArgumentNullException(nameof(cards));
			if (!base.IsCorrectNumberOfCards(cards) || !AreAllCardsDifferent(cards))
				throw new ArgumentException(nameof(cards));

			List<List<Card>> cardsForPlayers = new List<List<Card>>();

			Random random = new Random();
			var shuffledArray = cards.OrderBy(x => random.Next()).ToArray();

			for (int i = 0; i < NumbersConsts.PLAYERS_NUMBER_CONST; i++)
				cardsForPlayers.Add(shuffledArray[(NumbersConsts.CARDS_FOR_PLAYER_CONST * i)..((i + 1) * NumbersConsts.CARDS_FOR_PLAYER_CONST)].ToList());

			return cardsForPlayers;
		}
	}
}
