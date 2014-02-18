package net.pokernerd.homegame.poker;

import java.util.Comparator;
import java.util.HashMap;

public class PokerCardComparator implements Comparator<PokerCard> {

	public static final HashMap<PokerCard.CardFace, Integer> defaultFaceRanks;
	
	static
	{
		defaultFaceRanks = new HashMap<PokerCard.CardFace, Integer>() {
			{
				put(PokerCard.CardFace.Deuce, 2);
				put(PokerCard.CardFace.Trey, 3);
				put(PokerCard.CardFace.Four, 4);
				put(PokerCard.CardFace.Five, 5);
				put(PokerCard.CardFace.Six, 6);
				put(PokerCard.CardFace.Seven, 7);
				put(PokerCard.CardFace.Eight, 8);
				put(PokerCard.CardFace.Nine, 9);
				put(PokerCard.CardFace.Ten, 10);
				put(PokerCard.CardFace.Jack, 11);
				put(PokerCard.CardFace.Queen, 12);
				put(PokerCard.CardFace.King, 13);
				put(PokerCard.CardFace.Ace, 14);
			}
		};
	}
	
	public static Comparator<PokerCard.CardFace> getDefaultFaceComparer()
	{
		return new Comparator<PokerCard.CardFace>() {
			public int compare(PokerCard.CardFace f1, PokerCard.CardFace f2)
			{
				return defaultFaceRanks.get(f1) - defaultFaceRanks.get(f2);
			}
		};
	}
	
	public static Comparator<PokerCard.CardSuit> getDefaultSuitComparer()
	{
		return new Comparator<PokerCard.CardSuit>() {
			public int compare(PokerCard.CardSuit s1, PokerCard.CardSuit s2)
			{
				// Default behavior for comparing cards is that suit doesn't matter.
				return 0;
			}
		};
	}
	
	protected Comparator<PokerCard.CardFace> faceComparer;
	protected Comparator<PokerCard.CardSuit> suitComparer;
	
	public int compare(PokerCard firstCard, PokerCard secondCard)
	{
		int faceResult = faceComparer.compare(firstCard.Face, secondCard.Face);
		if(faceResult == 0) {
			return suitComparer.compare(firstCard.Suit, secondCard.Suit);
		}
		else {
			return faceResult;
		}
		
	}

}
