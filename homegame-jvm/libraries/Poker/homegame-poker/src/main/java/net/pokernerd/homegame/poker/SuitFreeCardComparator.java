package net.pokernerd.homegame.poker;

public class SuitFreeCardComparator extends PokerCardComparator {

	public SuitFreeCardComparator() {
		faceComparer = PokerCardComparator.getDefaultFaceComparer();
		suitComparer = PokerCardComparator.getDefaultSuitComparer();
	}

}
