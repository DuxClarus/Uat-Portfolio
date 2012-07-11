import math
from Point2D import *

class AiPathing():
    def __init__ (self, parabolaOrCos, xshift,yshift, delay = 1000):
        self.x = 0
        self.y = 0
        self.xshift = xshift
        self.yshift = yshift
        self.tempX = 0
        self.tempY = 0
        self.targetLocationX = 0
        self.targetLocationY = 0
        self.path = []
        self.pathlength = 0
        self.arrivedAtTarget = False
        self.pickParabola = parabolaOrCos
        self.amplitude = 50
        self.timer = 0 #timer to reset the path
        self.timingFlag = False #flag to set to make the path reset after the timer
        self.delay = delay #1 second
        self.tolerance = 5
        self.chooseParabola()

    def chooseParabola(self):
        if(self.pickParabola == 0):
            self.fillParabolaPointList()
        if(self.pickParabola == 1):
            self.fillBackwardsParabolaPointList()
        if(self.pickParabola == 2):
            self.fillCosPointList()
        if(self.pickParabola == 3):
            self.fillBackwardsCosPointList() 
                     
    def fillCosPointList(self):
        for x in range (0, 80):
            self.tempx = (x * 10)       
            self.y = (self.amplitude * math.cos(x) ) + 500
            self.point = Point2D(self.tempx, self.y)
            self.path.append(self.point)
            self.pathlength += 1
            
    def fillBackwardsCosPointList(self):
        for x in range (-80, 0):
            self.tempx = -(x * 10)       
            self.y = -(self.amplitude * math.cos(x) ) + 300
            self.point = Point2D(self.tempx, self.y)
            self.path.append(self.point)
            self.pathlength += 1

    def fillParabolaPointList(self):
        for x in range (-10, 12):
            self.tempx = x * 10 + 100 + self.xshift
            self.y = ( -3 * x * x + 50) + 250 +self.yshift
            point = Point2D(self.tempx, self.y)
            self.path.append(point)
            self.pathlength += 1
            
    def fillBackwardsParabolaPointList(self):
        for x in range (-10, 12):
            self.tempx = -(x * 10 + 100) + self.xshift
            self.y = ( -3 * x * x + 50) + 250 +self.yshift
            point = Point2D(self.tempx, self.y)
            self.path.append(point)
            self.pathlength += 1