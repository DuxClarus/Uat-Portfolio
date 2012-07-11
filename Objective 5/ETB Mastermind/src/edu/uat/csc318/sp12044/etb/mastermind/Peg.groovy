package edu.uat.csc318.sp12044.etb.mastermind

class Peg {
	private String color
	private boolean matched
	private int index
	
	public Peg(String inColor, int inIndex) {
		this.color = inColor
		this.index = inIndex
		this.matched = false
	}
	
	public String getColor() {
		return color
	}
	
	public void setColor(String inColor) {
		this.color = inColor
	}
	
	public int getIndex() {
		return index
	}
	
	public void setIndex(int inIndex) {
		this.index = inIndex
	}
	
	public boolean getMatched() {
		return matched
	}
	
	public void setMatched(boolean inBool) {
		this.matched = inBool
	}
	
	public boolean isWhitePeg(Peg secretPeg) {
		return index != secretPeg.getIndex() && color == secretPeg.getColor() && secretPeg.getMatched() == false
	}
	
	public boolean isBlackPeg(Peg secretPeg) {
		return index == secretPeg.getIndex() && color == secretPeg.getColor() && secretPeg.getMatched() == false
	}
}

