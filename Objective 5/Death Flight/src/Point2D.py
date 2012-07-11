'''
Created on Apr 8, 2011

@author: Dux
'''

class Point2D:
    def __init__(self, xLocation, yLocation):
        self.x = xLocation
        self.y = yLocation
    
    def getX(self):
        return self.x
    def getY(self):
        return self.y
    def setX(self, variable):
        self.x = variable
    def setY(self, variable):
        self.y = variable