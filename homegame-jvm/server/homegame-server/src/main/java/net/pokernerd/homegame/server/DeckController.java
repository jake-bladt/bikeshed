package net.pokernerd.homegame.server;

import java.util.UUID;

public class DeckController {

	protected DeckSource Source;
	
	public DeckController(DeckSource source)
	{
		Source = source;
	}
	
	UUID requestDeck() {
		UUID ret = Source.generateDeck();
		return ret;
	}

}
