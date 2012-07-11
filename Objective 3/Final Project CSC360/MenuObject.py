from Point2D import *
from pygame import draw

class MenuObject:
    def __init__(self, upperLeft, width, height, menuType, value = 0):
        self.upperLeft = upperLeft
        self.width = width
        self.height = height
        self.lowerRight = Point2D(self.upperLeft.x + self.width, self.upperLeft.y + self.height)
        self.type = menuType
        self.value = value
    def draw(self, screen):
        draw.rect(screen, (255,255,255), (self.upperLeft.x, self.upperLeft.y, self.width, self.height)) 

