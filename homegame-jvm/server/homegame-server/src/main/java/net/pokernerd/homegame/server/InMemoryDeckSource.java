package net.pokernerd.homegame.server;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.UUID;

import net.pokernerd.homegame.poker.PokerCard;
import net.pokernerd.homegame.poker.PokerDeck;

public class InMemoryDeckSource implements DeckSource {

	protected HashMap<UUID, PokerDeck> Decks;
	
	public InMemoryDeckSource() {
		Decks = new HashMap<UUID, PokerDeck>();
	}
	
	public UUID generateDeck() {
		UUID deckId = UUID.randomUUID();
		PokerDeck newDeck = new PokerDeck();
		newDeck.Shuffle();
		Decks.put(deckId, newDeck);		
		return deckId;
	}

	public CardRequestResult requestCards(UUID deckUUID, int count) {
		PokerDeck deck = Decks.get(deckUUID);
		CardRequestResult ret = new CardRequestResult();
		
		if(null == deck)
		{
			ret.Return = CardRequestResult.ReturnValue.DeckDoesNotExist;
		}
		else
		{
			synchronized (deck) {
				List<PokerCard> dealtCards = new ArrayList<PokerCard>();
				for(int i = 0; i < count; i++) {
					PokerCard card = deck.Deal();
					if(null == card) {
						ret.Return = CardRequestResult.ReturnValue.RanOutOfCards;
						return ret;
					}
					dealtCards.add(card);
				}
				ret.Return = CardRequestResult.ReturnValue.RetrievedCards;
				ret.Cards = dealtCards;
			}
		}
		
		return ret;
	}

}
