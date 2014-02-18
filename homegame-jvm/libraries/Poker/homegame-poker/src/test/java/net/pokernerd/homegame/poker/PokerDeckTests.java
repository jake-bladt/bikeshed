package net.pokernerd.homegame.poker;

import junit.framework.Assert;
import org.junit.Test;

public class PokerDeckTests {
	
	@Test
	public void testShuffle()
	{
		PokerDeck deck = new PokerDeck();
		deck.Shuffle();
		
		for(PokerCard.CardSuit s : PokerCard.CardSuit.values()) {
			for(PokerCard.CardFace f : PokerCard.CardFace.values()) {
				PokerCard testCard = new PokerCard(f, s);
				Assert.assertTrue(String.format("Deck does not contain %s.", testCard.toString()), deck.contains(testCard));
			}
		}
				
	}
	
	@Test
	public void testDeal()
	{
		PokerDeck deck = new PokerDeck();
		for(int i = 0; i < 52; i++) {
			PokerCard card = deck.Deal();
			Assert.assertNotNull(card);
		}
		
		PokerCard card53 = deck.Deal();
		Assert.assertNull(card53);
		
	}
}
