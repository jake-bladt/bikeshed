package net.pokernerd.homegame.poker;

import java.util.Comparator;
import java.util.HashMap;

public class PokerCard {

	public enum CardSuit {
		Clubs, Diamonds, Hearts, Spades		
	}
	
	public enum CardFace {
		Ace, Deuce, Trey, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
	}
	
	public static PokerCard fromSymbol(String sym)
		throws IllegalArgumentException
	{
		if(sym.length() != 2)
		{
			throw new IllegalArgumentException(String.format("'%s' is not a recognized card.", sym));
		}
		char[] symbols = sym.toLowerCase().toCharArray();
		Character faceChar = symbols[0];
		Character suitChar = symbols[1];
		
		if(!symbolToFaceMap.containsKey(faceChar) || !symbolToSuitMap.containsKey(suitChar))
		{
			throw new IllegalArgumentException(String.format("'%s' is not a recognized card.", sym));
		}
		
		return new PokerCard(symbolToFaceMap.get(faceChar), symbolToSuitMap.get(suitChar));
	}
	
	CardFace Face;
	CardSuit Suit;
	
	protected static final HashMap<Character, CardFace> symbolToFaceMap;
	protected static final HashMap<Character, CardSuit> symbolToSuitMap;
	protected static final HashMap<CardFace, String> faceToNameMap;
	protected static final HashMap<CardSuit, String> suitToNameMap;
	protected static final HashMap<CardFace, Character> faceToSymbolMap;
	protected static final HashMap<CardSuit, Character> suitToSymbolMap;
	
	static {
 		symbolToFaceMap = new HashMap<Character, CardFace>() {
			{
				put('a', CardFace.Ace);
				put('2', CardFace.Deuce);
				put('3', CardFace.Trey);
				put('4', CardFace.Four);
				put('5', CardFace.Five);
				put('6', CardFace.Six);
				put('7', CardFace.Seven);
				put('8', CardFace.Eight);
				put('9', CardFace.Nine);
				put('t', CardFace.Ten);
				put('j', CardFace.Jack);
				put('q', CardFace.Queen);
				put('k', CardFace.King);
			}
 		};
 		
 		symbolToSuitMap = new HashMap<Character, CardSuit>() {
 			{
 				put('c', CardSuit.Clubs);
 				put('d', CardSuit.Diamonds);
 				put('h', CardSuit.Hearts);
 				put('s', CardSuit.Spades);
 			}
 		};
 		
 		faceToNameMap = new HashMap<CardFace, String>() {
 			{
 				put(CardFace.Ace, "Ace");
 				put(CardFace.Deuce, "Two");
 				put(CardFace.Trey, "Three");
 				put(CardFace.Four, "Four");
 				put(CardFace.Five, "Five");
 				put(CardFace.Six, "Six");
 				put(CardFace.Seven, "Seven");
 				put(CardFace.Eight, "Eight");
 				put(CardFace.Nine, "Nine");
 				put(CardFace.Ten, "Ten");
 				put(CardFace.Jack, "Jack");
 				put(CardFace.Queen, "Queen");
 				put(CardFace.King, "King");
 			}
 		};
 		
 		suitToNameMap = new HashMap<CardSuit, String>() {
 			{
 				put(CardSuit.Clubs, "Clubs");
 				put(CardSuit.Diamonds, "Diamonds");
 				put(CardSuit.Hearts, "Hearts");
 				put(CardSuit.Spades, "Spades");
 			}
 		};
 		
 		faceToSymbolMap = new HashMap<CardFace, Character>() {
 			{
 				put(CardFace.Ace, 'A');
 				put(CardFace.Deuce, '2');
 				put(CardFace.Trey, '3');
 				put(CardFace.Four, '4');
 				put(CardFace.Five, '5');
 				put(CardFace.Six, '6');
 				put(CardFace.Seven, '7');
 				put(CardFace.Eight, '8');
 				put(CardFace.Nine, '9');
 				put(CardFace.Ten, 'T');
 				put(CardFace.Jack, 'J');
 				put(CardFace.Queen, 'Q');
 				put(CardFace.King, 'K');
 			}
 		};
 		
 		suitToSymbolMap = new HashMap<CardSuit, Character>() {
 			{
 				put(CardSuit.Clubs, 'c');
 				put(CardSuit.Diamonds, 'd');
 				put(CardSuit.Hearts, 'h');
 				put(CardSuit.Spades, 's');
 			}
 		};
 		
	}
	
	public PokerCard(CardFace face, CardSuit suit)
	{
		Face = face;
		Suit = suit;
	}
	
	public String toString()
	{
		return String.format("%s of %s", faceToNameMap.get(Face), suitToNameMap.get(Suit));
	}
	
	public String toSymbol()
	{
		return String.format("%s%s", faceToSymbolMap.get(Face), suitToSymbolMap.get(Suit));
	}
	
	public Boolean isLesserRankThan(PokerCard that)
	{
		return isLesserRankThan(that, new SuitFreeCardComparator());
	}

	public Boolean isLesserRankThan(PokerCard that, Comparator<PokerCard> comparator)
	{
		return (comparator.compare(this, that) < 0); 
	}
	
	public Boolean isGreaterRankThan(PokerCard that)
	{
		return isGreaterRankThan(that, new SuitFreeCardComparator());
	}

	public Boolean isGreaterRankThan(PokerCard that, Comparator<PokerCard> comparator)
	{
		return (comparator.compare(this, that) > 0); 
	}
	
	public Boolean isEqualRankTo(PokerCard that)
	{
		return isEqualRankTo(that, new SuitFreeCardComparator());
	}

	public Boolean isEqualRankTo(PokerCard that, Comparator<PokerCard> comparator)
	{
		return (comparator.compare(this, that) == 0); 
	}
	
	public Boolean is(PokerCard that)
	{
		return (this.Face == that.Face && this.Suit == that.Suit);
	}
	
	
}
