##imports
import pygame
import random
import sys
import pygame.mixer
from Cell import *
from Character import *
from Enemy import *
from Color import *
from MouseCrosshair import *
from Point2D import *
from MenuObject import *
from GameSound import *
from enums import *

##colors for the game
white = (255,255,255)
pink = (254,97,164)
red = (255,0,0)
orange = (255,102,0)
yellow = (255,255,0)
green = (0,255,0)
blue = (0,0,255)
darkBlue = (0,0,128)
black = (0,0,0)
zombie = (92,118,60)
dead = (92,118,160)

class Game:
    def __init__(self, screen, inClock):
        #Window variables
        self.screen = screen
        self.resolutionX = screen.get_width()
        self.resolutionY = screen.get_height()
        self.clock = inClock
        self.marginHUD = 100
        #Game variables
        self.cellSizeX = 16
        self.cellSizeY = 16
        self.xCells = int(self.resolutionX / self.cellSizeX)
        self.yCells = int(self.resolutionY / self.cellSizeY - self.marginHUD / self.cellSizeY)
        self.cellSizeDivisor = 2
        self.cellUpdate = 4
        self.deadCells = 0
        self.aliveCells = 0
        self.infectedCells = 0
        self.cellSet = [[Cell]*self.yCells for x in range(self.xCells)]
        self.dirtyCells = []
        self.updateTime = 0
        self.updateNumber = 0
        self.cellDeathLimit = 35
        self.gameState = gameState.MENU_STATE
        self.player = Character("finalizedmasterchiefsprite.gif", 100, 100, 288, 288, 4, 4, 10, 10)
        self.level = 1
        self.menuList = []
        self.enemyList = []
        #bool variables
        self.playerAlive = True
        self.enemyAlive = True
        self.enemyAI = True
        self.bMusic = True
        self.bTimer = False
        self.setUpGame = False
        self.crosInput = False
        ##music variables
        pygame.mixer.stop()
        pygame.mixer.init(44100, -16, 2, 2048)
        self.soundMaster = GameSound()
        ##keyboard variables
        self.keyboard = pygame.key.get_pressed()
        self.key1 = pygame.K_1
        self.key2 = pygame.K_2
        self.makeColorArray()
        self.setUpMouse()
        self.startMenu()
                
    def gamePrint(self, screen, text, xx, yy, color, fontSize):
        font = pygame.font.SysFont("Courier New",fontSize)
        ren = font.render(text,1,color)
        screen.blit(ren, (xx,yy))
        
    def makeColorArray(self):
        self.colorArray = []
        self.colorArray.append(black)
        self.colorArray.append(darkBlue)
        self.colorArray.append(blue)
        self.colorArray.append(green)
        self.colorArray.append(yellow)
        self.colorArray.append(orange)
        self.colorArray.append(red)
        self.colorArray.append(pink)
        self.colorArray.append(white)
        self.colorArray.append(zombie)
        self.colorArray.append(dead)
        
    def initializeGame(self, screen):
        self.setUpPlayer()
        self.setUpEnemy(screen)
        self.seedLevel();
        self.gameState = gameState.PLAY_STATE

    def setUpPlayer(self):
        self.playerAlive = True
        self.player = Character("finalizedmasterchiefsprite.gif", 100, 100, 288, 288, 4, 4, 10, 10)
        self.player.set_animation_delay(100)
        self.player.play(True)

    def setUpEnemy(self, screen):
        self.enemyList = []
        enemy = Enemy("Zombie.gif", screen.get_width() - 100, screen.get_height() - 100, 288, 288, 4, 4, self.enemyAI)
        self.enemyList.append(enemy)
        if self.level > 2:
            enemy = Enemy("Zombie.gif", screen.get_width() - 100, screen.get_height() - 700, 288, 288, 4, 4, True)
            self.enemyList.append(enemy)
        if self.level > 4:
            enemy = Enemy("Zombie.gif", screen.get_width() / 2, screen.get_height() / 2, 288, 288, 4, 4, True)
            self.enemyList.append(enemy)
        for enemy in self.enemyList:
            enemy.set_animation_delay(100)
            enemy.play(True)

    def setUpMouse(self):
        pygame.mouse.set_visible(False)
        self.crosshair = MouseCrosshair(Point2D(50, 50), Point2D(30, 30), "crosshair.bmp", 60, 30, 2, 1)
        self.crosshair.set_image_color_key(255, 255, 255)
        self.crosshair.set_animation_delay(200)
        self.crosshair.play(True)

    def seedLevel(self):
        for index1 in range(self.xCells):
            for index2 in range(self.yCells):
                self.cellSet[index1][index2] = Cell()
        for x in range(100):
            index1 = random.randint(0, self.xCells - 1)
            index2 = random.randint(0, self.yCells - 1)
            life = random.randint(1,8)
            self.cellSet[index1][index2].makeAlive()

    def incrementLevel(self, screen):
        self.level += 1
        self.initializeGame(screen)

    def startMenu(self):
        self.gameState = gameState.MENU_STATE
        keypress = pygame.key.get_pressed()
        screen = pygame.display.set_mode((self.resolutionX, self.resolutionY))
        self.menuList.append(MenuObject(Point2D(350, 380), 60, 35, "start"))
        self.menuList.append(MenuObject(Point2D(500, 700), 60, 35, "quit"))
        self.menuList.append(MenuObject(Point2D(60, 750), 60, 35, "music", True))
        self.menuList.append(MenuObject(Point2D(115, 750), 70, 35, "music", False))
 
    def updateMenu(self, screen):
        self.gamePrint(self.screen, "Start!", 350, 380, white, 30)
        self.gamePrint(self.screen, "Quit!", 500, 700, white, 30)
        self.gamePrint(self.screen, "Music?", 70, 700, white, 36)
        self.gamePrint(self.screen, "On", 60, 750, white, 30)
        self.gamePrint(self.screen, "Off", 115, 750, white, 30)
        if(pygame.mouse.get_pressed() == (0,0,1)):
                mouseX = pygame.mouse.get_pos()[0]
                mouseY = pygame.mouse.get_pos()[1]
                for menu in self.menuList:
                    if mouseX > menu.upperLeft.x and mouseX < menu.lowerRight.x:
                        if mouseY > menu.upperLeft.y and mouseY < menu.lowerRight.y:
                            self.selectOption(menu.type, menu, screen)
        msElapsed = self.clock.tick(30)
        self.crosshair.update(screen)
        pygame.display.update()
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit(); sys.exit();
                
    def selectOption(self, menuType, item, screen):
        if menuType == "start":
            self.initializeGame(screen);
        if menuType == "quit":
            pygame.quit();
            sys.exit();
        elif menuType == "music":
            self.bMusic = item.value

    def checkPlayerWalkingOnCells(self):
        indexX = int(self.player.upperLeft.x / self.cellSizeX)
        indexY = int(self.player.upperLeft.y / self.cellSizeY)
        while(indexX < self.player.lowerRight.x / self.cellSizeX):
            while(indexY < self.player.lowerRight.y / self.cellSizeY):
                if(self.checkX(indexX) and self.checkY(indexY)):
                    if(self.cellSet[indexX][indexY].bIsInfected == True):
                        self.cellSet[indexX][indexY].cureInfection()
                        self.player.score += 10
                indexY += 1
            indexX += 1

    def checkEnemyWalkingOnCells(self):
        for enemy in self.enemyList:
            if enemy.health > 0:
                indexX = int(enemy.upperLeft.x / self.cellSizeX)
                indexY = int(enemy.upperLeft.y / self.cellSizeY)
                while (indexX < enemy.lowerRight.x / self.cellSizeX):
                    while (indexY < enemy.lowerRight.y / self.cellSizeY):
                        if (self.checkX(indexX) and self.checkY(indexY)):
                            if (self.cellSet[indexX][indexY].getLife() == 8):
                                self.cellSet[indexX][indexY].makeZombie()
                        indexY += 1
                    indexX += 1

    def update(self, screen):
        if self.bMusic == True:
            if pygame.mixer.get_busy() == False:
                self.playSong()
        if self.gameState == gameState.PLAY_STATE:
            if self.updateNumber >= self.cellUpdate:
                self.updateNeighbors()
                self.updateNumber = 0
            else:
                self.updateNumber += 1
            self.player.update(screen, self.enemyList)
            self.checkPlayerWalkingOnCells();
            if self.playerAlive != True:
                self.gameOver();
            else:
                self.checkPlayerWalkingOnCells()
            for enemy in self.enemyList:
                enemy.update(screen, self.player)
                self.checkEnemyWalkingOnCells()
            self.updateCells()
            self.countCells()
            #random creations of gliders
            self.randomlyCreateGliders()
        elif self.gameState == gameState.MENU_STATE:
            self.updateMenu(screen)
        elif self.gameState == gameState.GAME_OVER:
            keypress = pygame.key.get_pressed()
            if keypress[pygame.K_SPACE]:
                self.initializeGame(screen)
        elif self.gameState == gameState.WIN_STATE:
            keypress = pygame.key.get_pressed()
            if keypress[pygame.K_SPACE]:
                self.incrementLevel(screen)

    def drawHUD(self, screen):
        #----CELLS ALIVE----#
        color = white
        if self.aliveCells < 1:
            color = red
        elif self.aliveCells < 100:
            color = orange
        gameText = "Cells Alive: " + str(self.aliveCells)
        self.gamePrint(screen, gameText, 5, 700, color, 12)

        #----CELLS DEAD----#
        color = white
        if self.deadCells >= 25:
            color = red
        elif self.deadCells >= 10:
            color = orange
        gameText = "Cells Dead: " + str(self.deadCells) + "/" + str(self.cellDeathLimit)
        self.gamePrint(screen, gameText, 5, 750, color, 12)

        #----INFECTED CELLS----#
        color = white
        if self.infectedCells >= 10:
            color = red
        gameText = "Cells Infected: " + str(self.infectedCells)
        self.gamePrint(screen, gameText, 150, 700, color, 12)

        #----LEVEL----#
        color = white
        gameText = "Level: " + str(self.level)
        self.gamePrint(screen, gameText, 150, 750, color, 12)

        #----PLAYER HP----#
        tempY = 700
        if self.player.health < 40:
            color = red
        elif self.player.health < 60:
            color = orange
        else:
            color = white
        gameText = "Health: " + str(self.player.health)
        self.gamePrint(screen, gameText, 300, tempY, color, 12)

        #----PLAYER SCORE----#
        color = white
        gameText = "Score: " + str(self.player.score)
        self.gamePrint(screen, gameText, 400, tempY, color, 12)
        tempY += 50
        
        if self.gameState == gameState.GAME_OVER:
            color = red
            gameText = "GAME OVER!"
            gameText1 = ("Press space to start a new game")
            self.gamePrint(screen, gameText, 350, 380, color, 32)
            self.gamePrint(screen, gameText1, 330, 410, color, 12)
            
        elif self.gameState == gameState.WIN_STATE:
            color = green
            gameText = "You win!"
            self.gamePrint(screen, gameText, 350, 380, color, 32)

    def levelWin(self):
        self.gameState = gameState.WIN_STATE

    def countCells(self):
        if self.bTimer == False:
            self.time = pygame.time.get_ticks()
            self.bTimer = True
        else:
            if self.time < pygame.time.get_ticks():
                self.deadCells = 0
                self.aliveCells = 0
                self.infectedCells = 0
                self.bTimer = False
                for indexX in range(self.xCells):
                    for indexY in range(self.yCells):
                        cell = self.cellSet[indexX][indexY]
                        if cell.bDead == True:
                            self.cellDeath()
                        elif cell.getLife() == 8:
                            self.aliveCells += 1
                        elif cell.getLife() == 9:
                            self.infectedCells +=1
                if self.aliveCells == 0:
                    self.gameOver()
                if self.infectedCells <= 0:
                    enemyAlive = False
                    for enemy in self.enemyList:
                        if enemy.health >= 0:
                            enemyAlive = True
                    if enemyAlive == False:
                        self.levelWin()

    def cellDeath(self):
        self.deadCells += 1
        if self.deadCells >= self.cellDeathLimit:
            self.gameOver()
                        
    def draw(self, screen):
        for indexX in range(self.xCells):
            for indexY in range(self.yCells):
                if (self.cellSet[indexX][indexY].getLife() != 0):
                    color = self.colorArray[int(self.cellSet[indexX][indexY].getLife())]
                    pygame.draw.rect(screen, color, (indexX * self.cellSizeX, indexY * self.cellSizeY, self.cellSizeX / self.cellSizeDivisor, self.cellSizeY / self.cellSizeDivisor))
        if self.player.health > 0:
            self.player.draw(screen)
        for enemy in self.enemyList:
            if enemy.health > 0:
                enemy.draw(screen)
    
    def updateCells(self):
        for indexX in range(self.xCells):
            for indexY in range(self.yCells):
                self.cellSet[indexX][indexY].update()

    def updateNeighbors(self):
        self.dirtyCells = []
        for indexX in range(self.xCells):
            for indexY in range(self.yCells):
                if self.cellSet[indexX][indexY].bConsidered == True:
                    self.updateCell(indexX, indexY)
        for x in range(0,len(self.dirtyCells), 2):
            self.updateCell(self.dirtyCells[x], self.dirtyCells[x+1])
            
    def updateCell(self, indexX, indexY):
        examinedCell = self.cellSet[indexX][indexY]
        if examinedCell.bIsInfected != True and examinedCell.bDead == False:
            examinedCell.bChecked = True
            cellsNear = 0
            for xDistance in range(3):
                for yDistance in range(3):
                    x = indexX + xDistance - 1
                    y = indexY + yDistance - 1
                    if (xDistance == 1 and yDistance == 1):
                        doNothing = True
                    else:
                        if (self.checkX(x) and self.checkY(y)):
                            cellBeingChecked = self.cellSet[x][y]
                            if cellBeingChecked.getLife() == 8:
                                cellsNear += 1
                            elif cellBeingChecked.bIsInfected == True:
                                examinedCell.addInfection()
                            if examinedCell.getLife() == 8 and cellBeingChecked.bConsidered == False:
                                cellBeingChecked.bConsidered = True
                                self.dirtyCells.append(x)
                                self.dirtyCells.append(y)
            examinedCell.setNeighbors(cellsNear)
            
    def checkX(self,x):
        if x >= 0 and x <= self.xCells - 1:
            return True
        else:
            return False
        
    def checkY(self,y):
        if y >= 0 and y <= self.yCells - 1:
            return True
        else:
            return False
        
    def gameOver(self):
        self.gameState = gameState.GAME_OVER

    def playSong(self):
        self.soundMaster.sound1.play()

    def randomlyCreateGliders(self):
        toBeCreated = random.randint(0, 50)
        if toBeCreated == 0:
            index1 = random.randint(10,self.xCells - 11)
            index2 = random.randint(10,self.yCells - 11)
            self.cellSet[index1 + 1][index2 + 1].makeAlive()
            self.cellSet[index1 + 1][index2 + 2].makeAlive()
            self.cellSet[index1 + 2][index2 + 2].makeAlive()
            self.cellSet[index1 + 2][index2 + 3].makeAlive()
            self.cellSet[index1 + 3][index2 + 2].makeAlive()
            self.cellSet[index1 + 5][index2 + 1].makeAlive()
            self.cellSet[index1 + 6][index2 + 1].makeAlive()
            self.cellSet[index1 + 6][index2 + 2].makeAlive()
            self.cellSet[index1 + 7][index2 + 1].makeAlive()
        elif toBeCreated == 1:
            index1 = random.randint(10,self.xCells - 11)
            index2 = random.randint(10,self.yCells - 11)
            self.cellSet[index1 + 1][index2 + 1].makeAlive()
            self.cellSet[index1 + 1][index2 + 2].makeAlive()
            self.cellSet[index1 + 2][index2 + 1].makeAlive()
            self.cellSet[index1 + 2][index2 + 3].makeAlive()
            self.cellSet[index1 + 3][index2 + 1].makeAlive()
        elif toBeCreated == 2:
            index1 = random.randint(10,self.xCells - 11)
            index2 = random.randint(10,self.yCells - 11)
            self.cellSet[index1 + 2][index2 + 1].makeAlive()
            self.cellSet[index1 + 3][index2 + 1].makeAlive()
            self.cellSet[index1 + 1][index2 + 2].makeAlive()
            self.cellSet[index1 + 3][index2 + 2].makeAlive()
            self.cellSet[index1 + 3][index2 + 3].makeAlive()
        elif toBeCreated == 3:
            index1 = random.randint(10,self.xCells - 11)
            index2 = random.randint(10,self.yCells - 11)
            self.cellSet[index1 + 2][index2 + 1].makeAlive()
            self.cellSet[index1 + 1][index2 + 3].makeAlive()
            self.cellSet[index1 + 2][index2 + 3].makeAlive()
            self.cellSet[index1 + 3][index2 + 3].makeAlive()
            self.cellSet[index1 + 3][index2 + 2].makeAlive()
        elif toBeCreated == 4:
            index1 = random.randint(10,self.xCells - 11)
            index2 = random.randint(10,self.yCells - 11)
            self.cellSet[index1 + 1][index2 + 1].makeAlive()
            self.cellSet[index1 + 1][index2 + 2].makeAlive()
            self.cellSet[index1 + 1][index2 + 3].makeAlive()
            self.cellSet[index1 + 2][index2 + 3].makeAlive()
            self.cellSet[index1 + 3][index2 + 2].makeAlive()


