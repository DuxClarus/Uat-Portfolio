'''
Created on Jul 23, 2011

@author: jasferri
'''
import math
import random
import pygame
from SpriteClass import *
from Point2D import Point2D
from Color import *
from enums import *


class Bullet(SpriteClass):
    def __init__(self, filename, x, y, width, height, columns, rows, targetLocation):
        SpriteClass.__init__(self, filename,x, y, width, height, columns, rows)
        self.target = Point2D(targetLocation.upperLeft.x + targetLocation.get_width() /2, targetLocation.upperLeft.y + targetLocation.get_height()/2)
        self.velocity = Point2D(10,10)
        self.calculateVelocity()
        
    def calculateVelocity(self):
        xDistance = (self.target.x - self.upperLeft.x)
        yDistance = (self.target.y - self.upperLeft.y)
        radian = math.atan2(yDistance, xDistance)
        self.velocity.x = math.cos(radian) * 25
        self.velocity.y = math.sin(radian) * 25
        
    def moveToTarget(self):
        self.upperLeft.x += self.velocity.x
        self.upperLeft.y += self.velocity.y
        self.updateLowerRight()
        
    def updateLowerRight(self):
        self.lowerRight.x = self.upperLeft.x + self.get_width()
        self.lowerRight.y = self.upperLeft.y + self.get_height()

    def update(self, screen):
        self.moveToTarget()
        self.draw(screen)
        
            
