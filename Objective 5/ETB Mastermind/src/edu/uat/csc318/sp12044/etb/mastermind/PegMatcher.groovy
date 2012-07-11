package edu.uat.csc318.sp12044.etb.mastermind

class PegMatcher {
	private Pegs userPegs
	private Pegs secretPegs
	private int whitePegCount
	private int blackPegCount
	
	public PegMatcher(Pegs inUserPegs, Pegs inSecretPegs) {
		this.userPegs = inUserPegs
		this.secretPegs = inSecretPegs
	}
	
	public int getBlackPegCount() {
		resetBlack()
		for (Peg userPeg : userPegs.getPegs()) {
			for(Peg secretPeg : secretPegs.getPegs()) {
				if(userPeg.isBlackPeg(secretPeg)) {
					this.blackPegCount++
					secretPeg.setMatched(true)
				}
			}
		}
		return blackPegCount
	}
	
	public int getWhitePegCount() {
		resetWhite()
		for (Peg userPeg : userPegs.getPegs()) {
			for(Peg secretPeg : secretPegs.getPegs()) {
				if(userPeg.isWhitePeg(secretPeg)) {
					this.whitePegCount++
					secretPeg.setMatched(true)
				}
			}
		}
		return whitePegCount
	}
	
	private void resetBlack() {
		this.blackPegCount = 0
		resetPegsMatchedStatus()
	}
	
	private void resetWhite() {
		this.whitePegCount = 0
		resetPegsMatchedStatus()
	}
	
	private void resetPegsMatchedStatus() {
		this.secretPegs.resetMatched()
	}
}
