package edu.uat.csc318.sp12044.etb.mastermind

import groovy.swing.SwingBuilder
import java.awt.* //NOT GOOD PRACTICE
import javax.swing.* // MAY CAUSE NAME COLLISIONS



class ugliness {

	static main(args) {

		def hintCode = [
			"Blank",
			"Blank",
			"Blank",
			"Blank"
		]
		def winCode = [
			"Black",
			"Red",
			"Green",
			"Orange"
		]
		def enteredCode = ["Red", "Red", "Red", "Red"]
		def swingBuilder = new SwingBuilder()

		def masterMindGUI =  swingBuilder.frame(title: "Master Mind GUI", size : [500, 600], defaultCloseOperation : JFrame.EXIT_ON_CLOSE) {
			borderLayout()
			panel(constraints : BorderLayout.SOUTH) {
				panel() {
					gridLayout(rows : 1, columns : 3)
					comboBox(id: "comboOne", items:Colors, actionPerformed : {
						enteredCode[0] = comboOne.selectedItem
					})
					comboBox(id: "comboTwo", items:Colors, actionPerformed : {
						enteredCode[1] = comboTwo.selectedItem
					})
					comboBox(id: "comboThree", items:Colors, actionPerformed : {
						enteredCode[2] = comboThree.selectedItem
					})
					comboBox(id: "comboFour", items:Colors, actionPerformed : {
						enteredCode[3] = comboFour.selectedItem
					})
					button(text:"Enter Code", actionPerformed : {
						//check for win/lost conditions and add the new code to textAreaCenter
						if(textAreaCenter.lineCount >= 11)
							swingBuilder.optionPane(message:'No more guesses. Game Over').createDialog(null, 'Master Mind GUI').show()
						else if(winCode == enteredCode)
							swingBuilder.optionPane(message:'VICTORY!!').createDialog(null, 'Master Mind GUI').show()
						else
							textAreaCenter.text += ("\n" + enteredCode)

						//check if there code is close to winCode
						//this is really ugly... couldnt think of any other way to handle this problem.
						//check slot 1	
						if(enteredCode[0] == winCode[0]){
							hintCode[0] = "Red"
						}
						else if((enteredCode[0] == winCode[1]) || (enteredCode[0] == winCode[2]) ||(enteredCode[0] == winCode[3])) {
							hintCode[0] = "White"
						}
						else
						hintCode[0] = "Blank"
						
						//check slot 2
						if(enteredCode[1] == "Red"){
							hintCode[1] = "Red"
						}
						else if((enteredCode[1] == "Black") || (enteredCode[1] == "Green") ||(enteredCode[1] == "Orange")) {
							hintCode[1] = "White"
						}
						else
						hintCode[1] = "Blank"
						//check slot 3
						if(enteredCode[2] == "Green"){
							hintCode[2] = "Red"
						}
						else if((enteredCode[2] == "Red") || (enteredCode[2] == "Black") ||(enteredCode[2] == "Orange")) {
							hintCode[2] = "White"
						}
						else
						hintCode[2] = "Blank"
						//check slot 4
						if(enteredCode[3] == "Orange"){
							hintCode[3] = "Red"
						}
						else if((enteredCode[3] == "Red") || (enteredCode[3] == "Green") ||(enteredCode[3] == "Black")) {
							hintCode[3] = "White"
						}
						else
						hintCode[3] = "Blank"
						
						textAreaWest.text += ("\n" + hintCode)
					})
				}
			}
			panel(constraints : BorderLayout.CENTER){
				panel() {
					gridLayout(rows : 10, columns : 10)
					textArea(id: "textAreaCenter", text: "[Blank, Blank, Blank, Blank]", preferredSize: [150, 500])
				}
			}
			panel(constraints : BorderLayout.WEST){
				panel() {
					gridLayout(rows : 10, columns : 10)
					textArea(id: "textAreaWest", preferredSize:[150, 500], text: "[Blank, Blank, Blank, Blank]")
				}
			}
		}
		masterMindGUI.show()
	}
}