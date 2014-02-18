package net.pokernerd.homegame.poker;

import junit.framework.Assert;
import net.pokernerd.homegame.poker.PokerCard.CardSuit;

import org.junit.Test;

public class PokerCardComparatorTests {
	
	@Test
	public void testGreaterRankWithDefaultComparator() {
		PokerCard c1 = new PokerCard(PokerCard.CardFace.Ace, PokerCard.CardSuit.Clubs);
		PokerCard c2 = new PokerCard(PokerCard.CardFace.Ten, PokerCard.CardSuit.Spades);
		Assert.assertTrue(c1.isGreaterRankThan(c2));
	}
	
	@Test
	public void testLesserRankWithDefaultComparator() {
		PokerCard c1 = new PokerCard(PokerCard.CardFace.Ace, PokerCard.CardSuit.Clubs);
		PokerCard c2 = new PokerCard(PokerCard.CardFace.Ten, PokerCard.CardSuit.Spades);
		Assert.assertTrue(c2.isLesserRankThan(c1));
	}
	
	@Test
	public void testEqualRankWithDefaultComparator() {
		PokerCard c1 = new PokerCard(PokerCard.CardFace.Ten, PokerCard.CardSuit.Clubs);
		PokerCard c2 = new PokerCard(PokerCard.CardFace.Ten, PokerCard.CardSuit.Spades);
		Assert.assertTrue(c1.isEqualRankTo(c2));
	}

}
