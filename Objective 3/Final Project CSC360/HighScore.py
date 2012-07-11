'''
Created on Jul 24, 2011

@author: jasferri
'''

import pygame

class HighScore():
    def __init__(self):
        self.score = 0
        self.highScore = []
        self.winFlag = 0
    
    def get_score(self):
        return self.score
    
    def get_highScore(self):
        self.getScoresFromFile()
        return self.highScore[0]
    
    def incrementScore(self, byHowMuch):
        if(byHowMuch == 1):
            self.score += 10
        if(byHowMuch == 2):
            self.score += 25
        if(byHowMuch == 3):
            self.score += 1000
    
    
    def getScoresFromFile(self):
        scoreList = open("highScores.txt")
        for score in scoreList:
            if len(self.highScore) <= 20:
                newItem = int(score)
                self.highScore.append(newItem)
        scoreList.close()
        
    def checkNewScore(self, score):
        newScoreWin = 0
        for index in self.highScore:
            if index < score:
                newScoreWin = score
        return newScoreWin
            
        
    def saveScores(self, score):
        self.highScore.sort(key=None, reverse=True)
        for index in self.highScore:
            if(self.checkNewScore(score) > index):
                if(self.winFlag != 1):
                    self.highScore.append(score)
                    self.winFlag = 1
                    
    def saveHighScoreList(self):
        self.highScore.sort(key=None, reverse=True)
        topScores = open("highScores.txt", 'w')
        counter = 1
        while counter <= 20:
            for score in self.highScore:
                counter += 1
                topScores.write(str(score) + "\n")
        topScores.close()
  
    def updateScore(self, score):
        self.saveScores(score)
        self.saveHighScoreList()
        
        
        
