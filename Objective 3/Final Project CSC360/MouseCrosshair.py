'''
Created on Jul 23, 2011

@author: jasferri
'''
import pygame
from SpriteClass import *


class MouseCrosshair(SpriteClass):    
    def __init__(self, upperLeft, extensity, filename, width, height, columns, rows):
        SpriteClass.__init__(self, filename, pygame.mouse.get_pos()[0], pygame.mouse.get_pos()[1], width, height, columns, rows)
        self.upperLeft = upperLeft
        self.extensity = extensity
    
    def updateLocation(self):
        self.upperLeft.x = pygame.mouse.get_pos()[0] - self.get_width() / 2
        self.upperLeft.y = pygame.mouse.get_pos()[1] - self.get_height() / 2
        self.checkEdges()
		
    def checkEdges(self):
        if self.upperLeft.x < 0:
            self.upperLeft.x = 0
        elif self.upperLeft.x > 1600:
            self.upperLeft.x = 1600
        if self.upperLeft.y < 0:
            self.upperLeft.y = 0
        elif self.upperLeft.y > 800:
            self.upperLeft.y = 800
			
    def update(self, screen):
        self.updateLocation()
        self.draw(screen)
