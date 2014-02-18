package net.pokernerd.homegame.server;

import java.util.UUID;

import org.junit.Test;
import junit.framework.Assert;

public class InMemoryDeckSourceTests {
	
	@Test
	public void testDealTwo() {
		
		InMemoryDeckSource src = new InMemoryDeckSource();
		UUID deckId = src.generateDeck();
		CardRequestResult res = src.requestCards(deckId, 2);
		
		Assert.assertEquals(CardRequestResult.ReturnValue.RetrievedCards, res.Return);
		Assert.assertEquals(2, res.Cards.size());
	}
	
	@Test
	public void testRunOutOfCards() {
		InMemoryDeckSource src = new InMemoryDeckSource();
		UUID deckId = src.generateDeck();
		CardRequestResult res = src.requestCards(deckId, 53);
		Assert.assertEquals(CardRequestResult.ReturnValue.RanOutOfCards, res.Return);
	}
	
	@Test
	public void testDeckDoesNotExist() {
		InMemoryDeckSource src = new InMemoryDeckSource();
		UUID deckId = UUID.randomUUID();
		CardRequestResult res = src.requestCards(deckId, 53);
		Assert.assertEquals(CardRequestResult.ReturnValue.DeckDoesNotExist, res.Return);
	}

}
