package net.pokernerd.homegame.poker;

import junit.framework.Assert;
import org.junit.Test;

public class PokerCardTests {

	@Test
	public void testSymbolToString() {
		PokerCard ah = PokerCard.fromSymbol("Ah");
		Assert.assertEquals("Ace of Hearts", ah.toString());
		
		PokerCard jc = PokerCard.fromSymbol("Jc");
		Assert.assertEquals("Jack of Clubs", jc.toString());
		
		PokerCard td = PokerCard.fromSymbol("Td");
		Assert.assertEquals("Ten of Diamonds", td.toString());
		
	}
	
	@Test
	public void testEnumToSymbol() {
		PokerCard ah = new PokerCard(PokerCard.CardFace.Ace, PokerCard.CardSuit.Hearts);
		Assert.assertEquals("Ah", ah.toSymbol());
		
		PokerCard jc = new PokerCard(PokerCard.CardFace.Jack, PokerCard.CardSuit.Clubs);
		Assert.assertEquals("Jc", jc.toSymbol());
		
		PokerCard td = new PokerCard(PokerCard.CardFace.Ten, PokerCard.CardSuit.Diamonds);
		Assert.assertEquals("Td", td.toSymbol());
		
	}

}
