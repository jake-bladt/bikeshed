package net.pokernerd.homegame.server;

import java.util.concurrent.atomic.AtomicReference;
import java.util.List;
import java.util.UUID;

import net.pokernerd.homegame.poker.PokerCard;

public interface DeckSource {
	UUID generateDeck();
	CardRequestResult requestCards(UUID deckUUID, int count);
}
