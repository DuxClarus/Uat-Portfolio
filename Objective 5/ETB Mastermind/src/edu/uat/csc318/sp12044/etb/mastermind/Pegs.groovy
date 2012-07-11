package edu.uat.csc318.sp12044.etb.mastermind

class Pegs {
	private def pegList = []

	public Pegs() {
		
	}
	
	public Pegs(Peg inPeg1, Peg inPeg2, Peg inPeg3, Peg inPeg4) {
		addPeg(inPeg1)
		addPeg(inPeg2)
		addPeg(inPeg3)
		addPeg(inPeg4)
	}
	
	public void addPeg(Peg inPeg) {
		if(pegList == null) {
			pegList = []
		}
		pegList.add(inPeg)
	}

	public List<Peg> getPegs() {
		if(pegList == null) {
			pegList = []
		}
		return pegList
	}

	public void resetMatched() {
		for(Peg peg : pegList) {
			peg.setMatched(false)
		}
	}
	
	public Peg getPegAtIndex(int inIndex) {
		return pegList[inIndex]
	}
}
