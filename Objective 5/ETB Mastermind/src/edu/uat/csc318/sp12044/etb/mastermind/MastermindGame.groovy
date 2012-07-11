package edu.uat.csc318.sp12044.etb.mastermind

import groovy.swing.SwingBuilder

import java.util.Random

class MastermindGame {

	private ObserverManager observerManager
	Random random = new Random()
	private def swingBuilder = new SwingBuilder()
	private final int MAX_ATTEMPTS = 8
	private final int WINNING_COUNT = 4
	private int userAttempts = 0
	private def gameWindow
	private def secretPegs
	private def colors = [
		"Red",
		"Blue",
		"Green",
		"Yellow",
		"Orange",
		"Brown",
		"Purple",
		"Cyan"
	]

	public MastermindGame() {
		observerManager = ObserverManager.getDefault()
	}

	private void run(){
		gameWindow = new MasterMindWindow()		
		gameWindow.open()
		play()
	}

	private void play(){
		generateSecretCode()
		while(userAttempts < MAX_ATTEMPTS) {
			if(gameWindow.getUserMadeGuess() == true) {
				gameWindow.setUserMadeGuess(false)
				userAttempts++
				int blackPegCount = gameWindow.getBlackPegCount()
				int whitePegCount = gameWindow.getWhitePegCount()
				observerManager.updateObservers()
				if(blackPegCount == WINNING_COUNT){
					swingBuilder.optionPane(message:'Victory!').createDialog(null, 'Master Mind Victory').show()
				}
			}
		}
		swingBuilder.optionPane(
				message:'Defeat!' + "\n" + "The correct code was " + secretPegs.getPegAtIndex(0).getColor() + ", "
				+ secretPegs.getPegAtIndex(1).getColor() + ", " + secretPegs.getPegAtIndex(2).getColor() + ", "
				+ secretPegs.getPegAtIndex(3).getColor()).createDialog(null, 'Master Mind Defeat').show()
	}

	private void generateSecretCode(){
		int secretColor1 = random.nextInt(colors.size())
		int secretColor2 = random.nextInt(colors.size())
		int secretColor3 = random.nextInt(colors.size())
		int secretColor4 = random.nextInt(colors.size())
		createSecretPegs(secretColor1, secretColor2, secretColor3, secretColor4)
	}

	private void createSecretPegs(int secretColor1, int secretColor2, int secretColor3, int secretColor4) {
		def peg1 = new Peg(colors[secretColor1], 0)
		def peg2 = new Peg(colors[secretColor2], 1)
		def peg3 = new Peg(colors[secretColor3], 2)
		def peg4 = new Peg(colors[secretColor4], 3)
		println(colors[secretColor1])
		println(colors[secretColor2])
		println(colors[secretColor3])
		println(colors[secretColor4])
		secretPegs = new Pegs(peg1, peg2, peg3, peg4)
		gameWindow.setSecretPegs(secretPegs)
	}
}