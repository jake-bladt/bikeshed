package net.pokernerd.homegame.server;

import java.util.List;

import net.pokernerd.homegame.poker.PokerCard;

public class CardRequestResult {
	public enum ReturnValue { RetrievedCards, RuntimeError, RanOutOfCards, DeckDoesNotExist }
	
	public ReturnValue Return;
	public List<PokerCard> Cards;
	public Exception RuntimeException;
	
}
