package net.pokernerd.homegame.poker;

import java.util.HashMap;

public class PokerDeck {
	
	public HashMap<Integer, PokerCard> Cards;
	
	public PokerDeck() {
		
		Cards = new HashMap<Integer, PokerCard>();
		Integer pos = 0;
		
		for(PokerCard.CardSuit s : PokerCard.CardSuit.values()) {
			for(PokerCard.CardFace f : PokerCard.CardFace.values()) {
				Cards.put(pos, new PokerCard(f, s));
				pos++;
			}
		}
	}
	
	public void Shuffle() {
		HashMap<Integer, PokerCard> oldOrder = Cards;
		Cards = new HashMap<Integer, PokerCard>();
		
		for(Integer i = 0; i < 52; i++) {
			// Retrieve a card from the first 52 - i
			Integer sourceCardPos = (int)(Math.random() * (52.0 - i));
			Cards.put(i, oldOrder.get(sourceCardPos));
	
			// Shift all of the remaining cards over one position so that the deck doesn't have any gaps.
			for(Integer j = sourceCardPos; j < 52; j++) {
				oldOrder.put(j, oldOrder.get(j + 1));
			}
		}
		
	}
	
	public Boolean contains(PokerCard soughtCard)
	{
		for(PokerCard deckCard : Cards.values())
		{
			if(deckCard.is(soughtCard)) return true;
		}
		return false;
	}
	
	protected Integer CardCount = 52;
	
	public PokerCard Deal()
	{
		PokerCard ret = null;
		if(CardCount > 0) {
			ret = Cards.get(--CardCount);
			Cards.put(CardCount, null);
		}
		return ret;
	}
	

}
