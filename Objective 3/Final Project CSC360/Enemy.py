import pygame #get the pygame library
import sys #get some of the python library stuff
import random
import math

from SpriteClass import *
from Bullet import *
from enums import *
from Gun import *

screenWidth = 800
screenHeight = 800

class Enemy(SpriteClass):
    def __init__(self, filename, x, y, width, height, columns, rows, enemyAI):
        SpriteClass.__init__(self, filename,x, y, width, height, columns, rows)
        self.attackKey = pygame.K_RCTRL
        self.keyLeft = pygame.K_LEFT
        self.keyRight = pygame.K_RIGHT
        self.keyUp = pygame.K_UP
        self.keyDown = pygame.K_DOWN
        self.width = self.get_width()
        self.height = self.get_height()
        self.targetLocationUL = Point2D(0,0)
        self.targetLocationLR = Point2D(0,0)
        self.resetTargetLocation = 0
        self.atTargetLocation = True
        self.velocity = Point2D(3, 3)
        self.animationSwitchUpDown = False
        self.animationSwitchLeftRight = False
        self.isAlive = True
        self.health = 100
        self.gun = Gun(False)
        self.fireDelay = 30
        self.delay = 0
        self.attackFlag = moveDirection.NULL
        ###
        #variables for timer
        ###
        self.timer = 0 #timer to reset the path
        self.timingFlag = False #flag to set to make the path reset after the timer
        self.delay = 1000 #1 second
        
    def damage(self, amount = 7):
        if self.health > 0:
            self.health -= amount
            
    def setUpSprite(self):
        self.set_animation_delay()
        self.play(True)
            
    def handleAI(self):
        if(self.atTargetLocation == True):
            self.generateTargetLocation()
             
        if(self.targetLocationUL.x < self.upperLeft.x):
            self.upperLeft.x  -= self.velocity.x
            self.updateLowerRight()
            
        if(self.targetLocationUL.x > self.upperLeft.x):
            self.upperLeft.x  += self.velocity.x
            self.updateLowerRight() 
    
        if(self.targetLocationUL.y < self.upperLeft.y):
            self.upperLeft.y  -= self.velocity.y
            self.updateLowerRight()
        
        if(self.targetLocationUL.y > self.upperLeft.y):
            self.upperLeft.y  += self.velocity.y
            self.updateLowerRight()
        
        if(self.upperLeft.x <= self.targetLocationLR.x) and (self.lowerRight.x >= self.targetLocationUL.x):
            if(self.upperLeft.y <= self.targetLocationLR.y) and (self.lowerRight.y >= self.targetLocationUL.y):
                self.atTargetLocation = True
    
    def generateTargetLocation(self):
        self.targetLocationUL.x = random.randint(5, screenWidth - 70)
        self.targetLocationUL.y = random.randint(5, screenHeight - 190)
        self.targetLocationLR.x = self.targetLocationUL.x + self.get_width()
        self.targetLocationLR.y = self.targetLocationUL.y + self.get_height()
        self.atTargetLocation = False
        
    def handleAttack(self, player):
        self.fireDelay = 50
        if(self.delay >= self.fireDelay):
            if(self.attackFlag != moveDirection.ATTACK):
                newBullet = Bullet("Bullets.gif", self.upperLeft.x + (self.get_width() / 2), self.upperLeft.y + (self.get_height() / 2), 30, 30, 3, 3, player)
                newBullet.play(True)
                newBullet.scale_sprite(3)
                self.gun.addBullet(newBullet)
                self.attackFlag = moveDirection.ATTACK
            else:
                self.attackFlag = moveDirection.NULL
            self.delay = 0
        else:
            self.delay += random.randint(2, 5)

    def updateLowerRight(self):
        self.lowerRight.x = self.upperLeft.x + self.width
        self.lowerRight.y = self.upperLeft.y + self.height

    def checkScreenBounds(self):
        if(self.upperLeft.y <= 0):
            self.upperLeft.y = 1
        if(self.upperLeft.y + self.height >= screenHeight - 115):
            self.upperLeft.y = screenHeight - self.height - 115 
        if(self.upperLeft.x <= 0):
            self.upperLeft.x = 1
        if(self.upperLeft.x + self.width >= screenWidth):
            self.upperLeft.x = screenWidth - self.width - 1
        self.updateLowerRight()
        
    def update(self, screen, player):
        if self.health > 0:
            self.checkScreenBounds()
            self.handleAI()
            self.handleAttack(player)
            self.gun.update(screen, player)
           
