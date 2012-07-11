import pygame #get the pygame library
import sys #get some of the python library stuff
import random
import math

from enums import *
from Gun import Gun
from Bullet import Bullet
from Point2D import Point2D
from SpriteClass import *
from MouseCrosshair import *



screenWidth = 800
screenHeight = 800

class Character(SpriteClass):
    def __init__(self, filename, x, y, width, height, columns, rows, xVelocity = 1, yVelocity = 1):
        SpriteClass.__init__(self, filename,x, y, width, height, columns, rows)
        self.characterModel = filename
        self.keyLeft = pygame.K_a
        self.keyRight = pygame.K_d
        self.keyUp = pygame.K_w
        self.keyDown = pygame.K_s
        self.keySpace = pygame.K_SPACE
        self.width = self.get_width()
        self.height = self.get_height()
        self.velocity = Point2D(xVelocity, yVelocity)
        self.keyboard = pygame.key.get_pressed()
        self.animationSwitchUpDown = False
        self.animationSwitchLeftRight = False
        self.isAlive = True    
        self.playerDirection = moveDirection.NULL
        self.attackFlag = moveDirection.NULL
        self.gun = Gun(True)
        self.newBullet = None
        self.leftMouseClick = (1,0,0)
        self.delay = 0
        self.firingRate = 10
        self.health = 100
        self.score = 0
        ###
        #variables for timer
        ###
        self.timer = 0 #timer to reset the path
        self.timingFlag = False #flag to set to make the path reset after the timer
        self.delay = 1000 #1 second
        self.setUpCrossHair()

    def damage(self, amount = 20):
        if self.health > 0:
            self.health -= amount
            
    def setUpSprite(self):
        self.set_image_color_key(255, 0, 255)
        self.set_animation_delay(500)
        self.play(True)  
            
    def moveUp(self):
        self.upperLeft.y  -= self.velocity.y
        self.updateLowerRight()
        
    def moveDown(self):
        self.upperLeft.y  += self.velocity.y
        self.updateLowerRight()
        
    def moveLeft(self):
        self.upperLeft.x  -= self.velocity.x
        self.updateLowerRight()
        
    def moveRight(self):
        self.upperLeft.x  += self.velocity.x
        self.updateLowerRight()
        

    def handleUpActions(self):
        self.set_animation_range(12, 16)
        self.moveUp()
        if(self.animationSwitchUpDown == False):
            self.set_animation_frame(12)
            self.animationSwitchUpDown = True
    
    def handleDownActions(self):
        self.set_animation_range(0, 4)
        self.moveDown()
        if(self.animationSwitchUpDown == False):
            self.set_animation_frame(0)
            self.animationSwitchUpDown = True
            
    def handleLeftActions(self):
        self.set_animation_range(4, 8)
        self.moveLeft()
        if(self.animationSwitchLeftRight == False):
            self.set_animation_frame(4)
            self.animationSwitchLeftRight = True
            
    def handleRightActions(self):
        self.set_animation_range(8, 12) 
        self.moveRight()
        if(self.animationSwitchLeftRight == False):
            self.set_animation_frame(8)  
            self.animationSwitchLeftRight = True
                    
    def handleInput(self):
        self.keyboard = pygame.key.get_pressed()
        if(self.keyboard[self.keyLeft]):
            self.handleLeftActions()
            self.playerDirection = moveDirection.LEFT
        
        elif(self.keyboard[self.keyRight]):
            self.handleRightActions()
            self.playerDirection = moveDirection.RIGHT

        if(not (self.keyboard[self.keyLeft] or self.keyboard[self.keyRight])):
            self.animationSwitchLeftRight = False
            

        if(self.keyboard[self.keyUp]):
            self.handleUpActions()
            self.playerDirection = moveDirection.UP
    
        elif(self.keyboard[self.keyDown]):
            self.handleDownActions()
            self.playerDirection = moveDirection.DOWN
            
        if(not (self.keyboard[self.keyUp] or self.keyboard[self.keyDown])):
            self.animationSwitchUpDown = False
        
        if(self.keyboard[self.keyLeft] == False) and (self.keyboard[self.keyRight] == False) and (self.keyboard[self.keyUp] == False) and (self.keyboard[self.keyDown] == False):
            self.playerDirection = moveDirection.NULL

    def setUpCrossHair(self):
        pygame.mouse.set_visible(False)
        self.crosshair = MouseCrosshair(Point2D(50,50), Point2D(30,30), "crosshair.bmp", 60, 30, 2, 1)
        self.crosshair.set_image_color_key(255, 255, 255)
        self.crosshair.set_animation_delay(200)
        self.crosshair.play(True)

    def handleAttack(self):
        if self.delay >= self.firingRate:
            self.delay = 0
        if(pygame.mouse.get_pressed() == self.leftMouseClick):
            if self.delay == 0:
                if(self.attackFlag != moveDirection.ATTACK):
                    self.fireBullet()
        else:
            self.attackFlag = moveDirection.NULL
        if self.delay != 0:
            self.delay += 1
            
    def fireBullet(self):
        self.newBullet = Bullet("Bullets.gif",self.upperLeft.x + (self.get_width() / 2), self.upperLeft.y + (self.get_height() / 2), 30, 30, 3, 3, self.crosshair)
        self.newBullet.set_animation_frame(6)
        self.gun.addBullet(self.newBullet)
        self.attackFlag = moveDirection.ATTACK
        self.delay = 1
            
            
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
    
    def updateLowerRight(self):
        self.lowerRight.x = self.upperLeft.x + self.width
        self.lowerRight.y = self.upperLeft.y + self.height
        
    def update(self, screen, enemyList):
        if self.health > 0:
            self.handleInput()
            self.handleAttack()
            self.checkScreenBounds()
            self.gun.update(screen, enemyList)
            for enemy in enemyList:
                if(enemy.health == 0):
                    self.score += 1000
            self.crosshair.update(screen)
          
