package edu.uat.csc318.sp12044.etb.mastermind

import groovy.swing.SwingBuilder

import java.awt.BorderLayout

import javax.swing.ImageIcon
import javax.swing.JFrame

class MasterMindWindow extends MastermindGame implements Observer {
	private ObserverManager observerManager
	private String[] userGuess = ["", "", "", ""]
	private int imageCounter = 1
	private int rowCounter = 0
	private def userPegs = new Pegs()
	private def secretPegs = new Pegs()
	private boolean userMadeGuess = false
	private def swingBuilder
	private def window
	private def pegMatcher
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

	public MasterMindWindow() {
		observerManager = ObserverManager.getDefault()
		observerManager.addObserver(this)
		setupGUI()
	}

	public void open() {
		window.show()
	}

	public void setSecretPegs(Pegs inSecretPegs) {
		secretPegs = inSecretPegs
	}

	public Pegs getSecretPegs() {
		return secretPegs
	}

	public Pegs getUserPegs() {
		return userPegs
	}

	public void setUserMadeGuess(boolean inMadeGuess) {
		userMadeGuess = inMadeGuess
	}

	public boolean getUserMadeGuess() {
		return userMadeGuess
	}

	public int getBlackPegCount() {
		pegMatcher = new PegMatcher(userPegs, secretPegs)
		return pegMatcher.getBlackPegCount()
	}

	public int getWhitePegCount() {
		pegMatcher = new PegMatcher(userPegs, secretPegs)
		return pegMatcher.getWhitePegCount()
	}

	private void setupGUI() {
		swingBuilder = new SwingBuilder()
		window = swingBuilder.frame(title: "Masterm Mind Game", size : [800, 600], defaultCloseOperation : JFrame.EXIT_ON_CLOSE) {
			borderLayout()
			tabbedPane(id : "guessWindow", constraints : BorderLayout.WEST, preferredSize : [380, 590]) {
				panel(title : "User Guesses") {
					tableLayout {
						tr{
							td{ label(id:"peg11", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg12", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg13", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg14", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"peg21", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg22", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg23", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg24", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"peg31", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg32", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg33", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg34", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"peg41", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg42", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg43", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg44", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"peg51", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg52", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg53", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg54", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"peg61", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg62", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg63", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg64", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"peg71", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg72", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg73", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg74", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"peg81", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg82", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg83", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"peg84", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
					}
				}
			}
			tabbedPane(id : "logWindow", constraints : BorderLayout.EAST, preferredSize : [380, 590]) {
				panel(title : "Log") {
					tableLayout {
						tr{
							td{ label(id:"logPeg11", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg12", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg13", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg14", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"logPeg21", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg22", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg23", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg24", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"logPeg31", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg32", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg33", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg34", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"logPeg41", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg42", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg43", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg44", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"logPeg51", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg52", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg53", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg54", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"logPeg61", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg62", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg63", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg64", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"logPeg71", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg72", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg73", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg74", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
						tr{
							td{ label(id:"logPeg81", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg82", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg83", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
							td{ label(id:"logPeg84", icon:imageIcon("white.png"), iconTextGap:10, text:" ") }
						}
					}
				}
			}
			panel(constraints : BorderLayout.SOUTH) {
				gridLayout(rows : 1, columns : 5)
				comboBox(id : "firstGuess", items : colors, actionPerformed : { userGuess[0] = firstGuess.selectedItem } )
				comboBox(id : "secondGuess", items : colors, actionPerformed : { userGuess[1] = secondGuess.selectedItem } )
				comboBox(id : "thirdGuess", items : colors, actionPerformed : { userGuess[2] = thirdGuess.selectedItem } )
				comboBox(id : "fourthGuess", items : colors, actionPerformed : { userGuess[3] = fourthGuess.selectedItem } )
				button(text : "Make Guess", actionPerformed : { makeGuess() })
			}
		}
	}

	private void makeGuess() {
		setUserMadeGuess(true)
		createUserPegs()
	}

	@Override
	public void update() {
		displayUserGuess()
		displayLogGuess()
	}
	private void displayUserGuess() {
		imageCounter = 1
		rowCounter++
		for(def color in userGuess) {
			def object = "peg${rowCounter}${imageCounter}"
			def pictureName = color + "Peg.png"
			java.net.URL imageURL = getClass().getResource(pictureName)
			ImageIcon image = new ImageIcon(imageURL)
			swingBuilder."$object".setIcon(image)
			imageCounter++
		}
	}

	private void displayLogGuess() {
		int blackPegCount = getBlackPegCount()
		int whitePegCount = getWhitePegCount()
		displayBlackPegs(blackPegCount)
		displayWhitePegs(whitePegCount)
	}

	private void displayBlackPegs(int blackPegCount) {
		for(int index = 1; index <= blackPegCount; index++) {
			def object = "logPeg${rowCounter}${index}"
			java.net.URL imageURL = getClass().getResource("blackPeg.png")
			ImageIcon image = new ImageIcon(imageURL)
			swingBuilder."$object".setIcon(image)
		}
	}

	private void displayWhitePegs(int whitePegCount) {
		int offset = getBlackPegCount()
		for(int index = 1; index <= whitePegCount; index++) {
			int offsetIndex = index + offset
			if(offsetIndex == 5)
				return
			def object = "logPeg${rowCounter}${offsetIndex}"
			java.net.URL imageURL = getClass().getResource("whitePeg.png")
			ImageIcon image = new ImageIcon(imageURL)
			swingBuilder."$object".setIcon(image)
		}
	}

	private void createUserPegs() {
		def peg1 = new Peg(userGuess[0], 0)
		def peg2 = new Peg(userGuess[1], 1)
		def peg3 = new Peg(userGuess[2], 2)
		def peg4 = new Peg(userGuess[3], 3)
		userPegs = new Pegs(peg1, peg2, peg3, peg4)
	}
}
