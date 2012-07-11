class Cell:
    def __init__(self):
        self.life = 0
        self.neighbors = -1
        self.bConsidered = False
        self.bChecked = False
        self.bIsInfected = False
        self.bDead = False
        self.infectCounters = 0
        self.infectLimit = 20
        self.deathLimit = 100
        
    def changeLife(self, amount = 1):
        self.life += amount
        self.checkLife()
        
    def getLife(self):
        return self.life
    
    def setLife(self, amount):
        self.life = amount
        self.checkLife()
        
    def checkLife(self):
        if self.bIsInfected != True:
            if self.life > 8:
                self.life = 8
            if self.life < 0:
                self.life = 0
                self.infectCounters = 0
                self.bConsidered = False
        else:
            self.life = 9
            
    def makeAlive(self):
        if self.bDead == False:
            self.life = 8
            self.bConsidered = True
            
    def setNeighbors(self, amount):
        self.neighbors = amount
        
    def getNeighbors(self):
        return self.neighbors
    
    def cureInfection(self):
        if self.bDead == False:
            self.bIsInfected = False
            self.makeAlive()
            self.infectCounters = 0
            
    def makeZombie(self):
        self.life = 9
        self.bIsInfected = True
        
    def makeDead(self):
        self.life = 10
        self.bDead = True
        self.bIsInfected = True
        
    def addInfection(self):
        self.infectCounters += 1
        
    def update(self):
        if self.bDead == False:
            if self.infectCounters >= self.infectLimit:
                if self.infectCounters >= self.deathLimit:
                    self.makeDead()
                else:
                    self.makeZombie()
            if self.bIsInfected != True:
                if self.neighbors != -1:
                    if self.neighbors < 2:
                        self.changeLife(-0.2)
                    elif self.neighbors > 3:
                        self.changeLife(-0.2)
                    elif self.neighbors == 3:
                        self.makeAlive()
                    elif self.neighbors == 2 and self.life != 8:
                        self.changeLife(-0.2)
                    self.neighbors = -1
                if self.bChecked == True and self.life != 8:
                    self.bConsidered = False
                self.bChecked = False
                if self.bConsidered == False:
                    self.changeLife(-0.25)
            else:
                self.infectCounters += 1
