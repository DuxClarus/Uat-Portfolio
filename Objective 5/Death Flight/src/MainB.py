import pygame #get the pygame library
import sys #get some of the python library stuff
import random
from SpriteClass import *
from Point2D import *
from AiPathing import *

pygame.init()
clock = pygame.time.Clock()

screenWidth = 800
screenHeight = 600

screen = pygame.display.set_mode((screenWidth, screenHeight))
background = pygame.image.load("backgroundimage.jpg")
msElasped = clock.tick(30)

black = (0,0,0)
white = (255,255,255)
red = (255,0,0)
green = (0,255,0)
blue = (0,0,255)
purple = (255,0,255)
    
def gameprint(screen,text,xx,yy,color):
    font = pygame.font.SysFont("Courier New",18)
    ren = font.render(text,1,color)
    screen.blit(ren, (xx,yy))

class bullet(SpriteClass):
    def __init__(self, filename, x, y, width, height, columns, rows,  xVelocity= 0, yVelocity= 0):
        SpriteClass.__init__(self, filename, x, y, width, height, columns, rows)
        self.width = width
        self.height = height
        self.keySpace = pygame.K_SPACE
        self.keyboard = pygame.key.get_pressed()
        self.upperLeft = Point2D(self.x, self.y)
        self.lowerRight = Point2D( (self.x + self.width ) ,(self.y + self.height) )
        self.velocity = Point2D(xVelocity, yVelocity)

    def move(self):
        self.y -= self.velocity.y

    def update(self, screen):
        self.move()
        self.draw(screen)
        
class enemyBullet(bullet):
    def __init__(self, filename, x, y, width, height, columns, rows,  xVelocity=2, yVelocity=2):
        bullet.__init__(self, filename, x, y, width, height, columns, rows, xVelocity, yVelocity)
        self.bulletOffSetX = 20
        self.bulletOffSetY = 0
        self.yAtCollision = 0
        self.animationYoffSet = 10
        self.isAlive = True
        self.collisionHappened = False
        self.x = x + self.bulletOffSetX 
        self.y = y + self.bulletOffSetY

    def move(self):
        self.y += self.velocity.y
        
    def setUpSprite(self):
        self.set_image_color_key(255,255,255)
        self.set_animation_frame(0)
        self.set_animation_range(0,0)
        self.play(True)
    
    def blowUpBullet(self):
        self.yAtCollision = self.y
        self.velocity.y = 2
        self.set_animation_frame(1)
        self.set_animation_range(1,1)
    
    def toggleIsAliveFalse(self):
        if(self.y == self.yAtCollision + self.animationYoffSet):
            self.isAlive = False
        
    def checkPlayerCollision(self, player):
        if(self.collisionHappened == False):
            if(player.y + player.height >= self.y + self.height) and (player.y <= self.y) and (player.x <= self.x) and (player.x - player.width >= self.x + self.width):
                if(player.collisionHappened == False):
                    player.displayDeathAnimations()
                self.collisionHappened = True
                if (self.collisionHappened == True):
                    self.blowUpBullet()
    
    def update(self, screen, player):
        self.checkPlayerCollision(player)
        if(self.collisionHappened == True):
            self.toggleIsAliveFalse()
        self.move()
        self.draw(screen)
        
class playerBullet(bullet):
    def __init__(self, filename, x, y, width, height, columns, rows,  xVelocity=2, yVelocity=2):
        bullet.__init__(self, filename, x, y, width, height, columns, rows, xVelocity, yVelocity)
        self.bulletOffSetX = 20
        self.bulletOffSetY = 0
        self.yAtCollision = 0
        self.animationYoffSet = 10
        self.isAlive = True
        self.collisionHappened = False
        self.x = x + self.bulletOffSetX 
        self.y = y + self.bulletOffSetY
    
    def setUpSprite(self):
        self.set_image_color_key(255,255,255)
        self.set_animation_frame(0)
        self.set_animation_range(0,0)
        self.play(True)
    
    def blowUpBullet(self):
        self.yAtCollision = self.y
        self.velocity.y = 2
        self.set_animation_frame(1)
        self.set_animation_range(1,1)
    
    def toggleIsAliveFalse(self):
        if(self.y == self.yAtCollision - self.animationYoffSet):
            self.isAlive = False
        
    def checkEnemyCollision(self, enemy):
        if(self.collisionHappened == False):
            if(enemy.y + 70 >= self.y) and (enemy.x <= self.x + self.width) and (enemy.x + enemy.width >= self.x):
                enemy.displayDeathAnimations()
                self.collisionHappened = True
                if(self.collisionHappened == True):
                    self.blowUpBullet()
    
    def checkBossCollision(self, enemy):
        if(self.collisionHappened == False) and (enemy.collisionHappened == False):
            if(enemy.y + 50 >= self.y) and (enemy.x <= self.x + self.width) and (enemy.x + enemy.width >= self.x):
                enemy.displayDeathAnimations()
                self.collisionHappened = True
                if(self.collisionHappened == True):
                    self.blowUpBullet()
    
    def update(self, screen, enemyList, boss, game):
        if(game.gameState == 1):
            for enemy in enemyList:
                if(enemy.collisionHappened != True):
                    self.checkEnemyCollision(enemy)
        if(game.gameState == 5):
                self.checkBossCollision(boss)
        if(self.collisionHappened == True):
            self.toggleIsAliveFalse()
        self.move()
        self.draw(screen)

class enemyFighterJet(SpriteClass):
    #parabola enter 0
    #cos enter 1
    #backwards cos enter 2
    def __init__(self, filename, x, y, width, height, columns, rows, parabolaOrCos, xShift, yShift,  xVelocity = 1, yVelocity = 1):
        SpriteClass.__init__(self, filename, x, y, width, height, columns, rows)
        self.width = 72
        self.height = 70
        self.yAtCollision = 0
        self.animationYoffSet = 50
        self.velocity = Point2D(xVelocity, yVelocity)
        self.isAlive = True
        self.collisionHappened = False
        self.gavePoints = False
        self.fireBullet = False
        self.fireState = 0
        ###
        #variables for timers
        ###
        self.timer = 0 #timer to reset the path
        self.timingFlag = False #flag to set to make the path reset after the timer
        self.delay = 100 #.10 second
        ###
        #variables for aipathing
        ###
        self.pathing = AiPathing(parabolaOrCos, xShift, yShift)
        self.targetLocation = None
        self.targetLocationIndex = 0
        self.targetLocation = self.pathing.path[self.targetLocationIndex]
        if( parabolaOrCos == 0):
            self.resetPosition = Point2D(-11 * 10 + 100 + self.pathing.xshift, (-3 * -11 * -11 +50)+ 250 + self.pathing.yshift) #reset off the screen
        else:
            self.resetPosition = Point2D(-50, self.pathing.path[0].y) #reset off the screen 
        self.x = self.resetPosition.x
        self.y = self.resetPosition.y
        self.upperLeft = Point2D(self.x, self.y)
        self.lowerRight = Point2D( (self.x + self.width ) ,(self.y + self.height) )
    
    
    ####All of this handles the movement and aipathing
    def move(self):
        if(self.collisionHappened == False):
            self.moveToTargetLocation()
        else:
            self.y += self.velocity.y
        
    def moveToTargetLocation(self):
        self.IteratePathNode()
        #Use Standard Hunter-Killer AI to get to each path node
        if(self.pathing.timingFlag == False):
            self.HunterKillerPather()
    
    def IteratePathNode(self):     
        if(self.pathing.timingFlag == True) and (pygame.time.get_ticks() - self.pathing.timer >= self.pathing.delay):                        
            self.setTargetLocation()
            self.pathing.timingFlag = False   
            
        #if (self.targetLocation.x == self.x) and (self.targetLocation.y == self.y) and (not self.pathing.timingFlag):
        if(self.locationTest() == True) and (self.pathing.timingFlag == False):
            self.targetLocationIndex += 1
            if(self.targetLocationIndex > self.pathing.pathlength-1 ):
                self.startPathDelay()
            else:   
                self.setTargetLocation()
            #if(self.pathing.path[self.targetLocationIndex].y < 0):
            #OK this should cause a delay
            
    def HunterKillerPather(self):
        if (self.targetLocation.x > self.x):
            self.set_animation_frame(4)
            self.set_animation_range(4,5)
            self.x += self.velocity.x
        if (self.targetLocation.x < self.x):
            self.set_animation_frame(2)
            self.set_animation_range(2,3)
            self.x -= self.velocity.x
        if (self.targetLocation.y > self.y):
            self.y += self.velocity.y
        if (self.targetLocation.y < self.y):
            self.y -= self.velocity.y        

    def setTargetLocation(self):
        #while(self.pathing.path[self.targetLocationIndex] == self.pathing.path[self.targetLocationIndex]):
        self.targetLocation = self.pathing.path[self.targetLocationIndex]
        #break   
        
    def startPathDelay(self):
        self.pathing.timer = pygame.time.get_ticks()
        self.pathing.timingFlag = True
        self.x = self.resetPosition.x - 20 
        #- 20 to accommodate for the resetPosition... it shows a part of the wing with the backwards parabolas
        self.y = self.resetPosition.y
        self.targetLocationIndex = 0

    def locationTest(self):
        if(self.x > (self.targetLocation.x - self.pathing.tolerance) and (self.x < self.targetLocation.x + self.pathing.tolerance)):
            if(self.y > (self.targetLocation.y - self.pathing.tolerance) and (self.y < self.targetLocation.y + self.pathing.tolerance)):
                return True
            else:
                return False
        else:
            return False
                 
    ####End of movement and aipathing
    
    def setUpSprite(self, frame = 0, frameRangeBegin = 0, frameRangeEnd = 1, delay = 500):
        self.set_image_color_key(255,255,255)
        self.set_animation_frame(frame)
        self.set_animation_range(frameRangeBegin, frameRangeEnd)
        self.set_animation_delay(500)
        self.play(True)
        
    def updateLowerRight(self):
        self.lowerRight.x = self.upperLeft.x + self.width
        self.lowerRight.y = self.upperLeft.y + self.height
        
    def displayDeathAnimations(self):
        self.collisionHappened = True
        self.yAtCollision = self.y
        self.velocity.y = 1
        self.set_animation_delay(200)
        self.set_animation_frame(5)
        self.set_animation_range(5,10)
    
    def toggleIsAliveFalse(self, game):
        if(self.y == self.yAtCollision + self.animationYoffSet):
            self.isAlive = False
            if(self.gavePoints == False):
                self.gavePoints = True
                game.addPoints(100)
    
    def checkToShootPlayer(self, player):
        if(self.collisionHappened == False) and (self.y >= 0):
            #if(self.x > (self.targetLocation.x - self.pathing.tolerance) and (self.x < self.targetLocation.x + self.pathing.tolerance)):
                #if(self.y > (self.targetLocation.y - self.pathing.tolerance) and (self.y < self.targetLocation.y + self.pathing.tolerance)):
            if(pygame.time.get_ticks() - self.timer >= self.delay):
                if(self.fireBullet == True):
                    self.timer = pygame.time.get_ticks()
                    self.fireBullet = False
                    self.fireState = 0
                else:
                    self.fireBullet = True
                    self.timer = pygame.time.get_ticks()
                            
            if(self.fireBullet == True) and self.fireState == 0:
                self.shootLaser()
                self.fireState = 1

    def shootLaser(self):
        if(self.targetLocationIndex % 2 == 0):
            game.createEnemyLaser(self)        
                            
    def update(self, screen, player, game):
        self.updateLowerRight() 
        if(self.collisionHappened == True):
            self.toggleIsAliveFalse(game)
        self.move()
        self.draw(screen)
 
class enemyFlyingPig(SpriteClass):
    #parabola enter 0
    #cos enter 1
    #backwards cos enter 2
    def __init__(self, filename, x, y, width, height, columns, rows, parabolaOrCos, xShift, yShift,  xVelocity = 1, yVelocity = 1):
        SpriteClass.__init__(self, filename, x, y, width, height, columns, rows)
        self.width = 60
        self.height = 40
        self.yAtCollision = 0
        self.animationYoffSet = 50
        self.velocity = Point2D(xVelocity, yVelocity)
        self.isAlive = True
        self.collisionHappened = False
        ###
        #variables for aipathing
        ###
        self.pathing = AiPathing(parabolaOrCos, xShift, yShift)
        self.targetLocation = None
        self.targetLocationIndex = 0
        self.targetLocation = self.pathing.path[self.targetLocationIndex]
        if( parabolaOrCos == 3):
            self.resetPosition = Point2D(850, self.pathing.path[0].y) #reset off the screen
        elif(parabolaOrCos == 2):
            self.resetPosition = Point2D(-50, self.pathing.path[0].y) #reset off the screen
#        else:
#            self.resetPosition = Point2D(850, self.pathing.path[0].y) #reset off the screen
        self.x = self.resetPosition.x
        self.y = self.resetPosition.y
        self.upperLeft = Point2D(self.x, self.y)
        self.lowerRight = Point2D( (self.x + self.width ) ,(self.y + self.height) )
    
    
    ####All of this handles the movement and aipathing
    def move(self):
        if(self.collisionHappened == False):
            self.moveToTargetLocation()
        else:
            self.y += self.velocity.y
        
    def moveToTargetLocation(self):
        self.IteratePathNode()
        #Use Standard Hunter-Killer AI to get to each path node
        if(self.pathing.timingFlag == False):
            self.HunterKillerPather()
    
    def IteratePathNode(self):     
        if(self.pathing.timingFlag == True) and (pygame.time.get_ticks() - self.pathing.timer >= self.pathing.delay):                        
            self.setTargetLocation()
            self.pathing.timingFlag = False   
            
        #if (self.targetLocation.x == self.x) and (self.targetLocation.y == self.y) and (not self.pathing.timingFlag):
        if(self.locationTest() == True) and (self.pathing.timingFlag == False):
            self.targetLocationIndex += 1
            if(self.targetLocationIndex > self.pathing.pathlength-1 ):
                self.startPathDelay()
            else:   
                self.setTargetLocation()
            #if(self.pathing.path[self.targetLocationIndex].y < 0):
            #OK this should cause a delay
            
    def HunterKillerPather(self):
        if (self.targetLocation.x > self.x):
            self.x += self.velocity.x
        if (self.targetLocation.x < self.x):
            self.x -= self.velocity.x
        if (self.targetLocation.y > self.y):
            self.y += self.velocity.y
        if (self.targetLocation.y < self.y):
            self.y -= self.velocity.y        

    def setTargetLocation(self):
        #while(self.pathing.path[self.targetLocationIndex] == self.pathing.path[self.targetLocationIndex]):
        self.targetLocation = self.pathing.path[self.targetLocationIndex]
        #break   
        
    def startPathDelay(self):
        self.pathing.timer = pygame.time.get_ticks()
        self.pathing.timingFlag = True
        self.x = self.resetPosition.x
        self.y = self.resetPosition.y
        self.targetLocationIndex = 0

    def locationTest(self):
        if(self.x > (self.targetLocation.x - self.pathing.tolerance) and (self.x < self.targetLocation.x + self.pathing.tolerance)):
            if(self.y > (self.targetLocation.y - self.pathing.tolerance) and (self.y < self.targetLocation.y + self.pathing.tolerance)):
                return True
            else:
                return False
        else:
            return False
                 
    ####End of movement and aipathing
    
    def setUpSprite(self, frame = 1, frameRangeBegin = 1, frameRangeEnd = 3, delay = 500):
        self.set_image_color_key(0,0,0)
        self.set_animation_frame(frame)
        self.set_animation_range(frameRangeBegin, frameRangeEnd)
        self.set_animation_delay(delay)
        self.play(True)
        
    def updateLowerRight(self):
        self.lowerRight.x = self.upperLeft.x + self.width
        self.lowerRight.y = self.upperLeft.y + self.height
        
    def displayDeathAnimations(self):
        self.collisionHappened = True
        self.yAtCollision = self.y
        self.velocity.y = 1
        self.set_animation_delay(200)
        self.set_animation_frame(5)
        self.set_animation_range(5,10)
    
    def toggleIsAliveFalse(self):
        if(self.y == self.yAtCollision + self.animationYoffSet):
            self.isAlive = False       
                            
    def update(self, screen, player):
        self.updateLowerRight()
        if(self.collisionHappened == True):
            self.toggleIsAliveFalse()
        self.move()
        self.draw(screen)
 
class enemyBoss(SpriteClass):
    def __init__(self, filename, x, y, width, height, columns, rows,xVelocity = 1, yVelocity = 1):
        SpriteClass.__init__(self, filename, x, y, width, height, columns, rows)
        self.width = 50
        self.height = 30
        self.yAtCollision = 0
        self.animationYoffSet = 115
        self.velocity = Point2D(xVelocity, yVelocity)
        self.isAlive = True
        self.collisionHappened = False
        self.hitPoints = 15
        self.gavePoints = False
        self.fireBullet = False
        self.fireState = 0
        self.moveRight = False
        ###
        #variables for timers
        ###
        self.timer = 0 #timer to reset the path
        self.timingFlag = False #flag to set to make the path reset after the timer
        self.delay = 10 #.010 second

    def move(self):
        if(self.x > 0) and (self.collisionHappened == False) and (self.moveRight == False):
            self.set_animation_frame(0)
            self.set_animation_range(0, 2)
            self.x -= self.velocity.x
            if(self.x == 0):
                self.moveRight = True
        if(self.x == 0) and (self.collisionHappened == False) or (self.moveRight == True):
            self.set_animation_frame(2)
            self.set_animation_range(2, 4)
            self.x += self.velocity.x
            if(self.x >= 740):
                self.moveRight = False
                
    def deathMove(self):
        self.y += self.velocity.y
        
    def decrementHitPoints(self):
        self.hitPoints -= 1   
        
    def setUpSprite(self, frame = 0, frameRangeBegin = 0, frameRangeEnd = 1, delay = 500):
        self.set_image_color_key(255,0,255)
        self.set_animation_frame(frame)
        self.scale_sprite(2)
        self.set_animation_range(0,3)
        self.set_animation_delay(500)
        self.play(True)
        
    def displayDeathAnimations(self):
        self.decrementHitPoints()
        if(self.hitPoints <= 0):
            self.collisionHappened = True
            self.yAtCollision = self.y
            self.velocity.y = 1
            self.velocity.x = 1
            self.set_animation_delay(500)
            self.set_animation_frame(3)
            self.set_animation_range(4,8)
    
    def toggleIsAliveFalse(self, game):
        if(self.y == self.yAtCollision + self.animationYoffSet):
            self.isAlive = False
            if(self.gavePoints == False):
                self.gavePoints = True
                game.addPoints(1500)
    
    def checkToShootPlayer(self):
        if(self.collisionHappened == False): 
            if(pygame.time.get_ticks() - self.timer >= self.delay):
                if(self.fireBullet == True):
                    self.timer = pygame.time.get_ticks()
                    self.fireBullet = False
                    self.fireState = 0
                else:
                    self.fireBullet = True
                    self.timer = pygame.time.get_ticks()
                            
            if(self.fireBullet == True) and self.fireState == 0:
                self.shootLaser()
                self.fireState = 1

    def shootLaser(self):
        game.createEnemyLaser(self)        
                            
    def update(self, screen, player, game):
        if(self.collisionHappened == True):
            self.deathMove()
            self.toggleIsAliveFalse(game)
        else:
            self.move()
        self.draw(screen)
        
class PlayerClass(SpriteClass):
    def __init__(self, filename, x, y, width, height, columns, rows,  xVelocity = 1, yVelocity = 1):
        SpriteClass.__init__(self, filename, x, y, width, height, columns, rows)
        self.keyLeft = pygame.K_LEFT
        self.keyRight = pygame.K_RIGHT
        self.keyUp = pygame.K_UP
        self.keyDown = pygame.K_DOWN
        self.keySpace = pygame.K_SPACE
        self.width = 72
        self.height = 70
        self.fireState = 0
        self.isAlive = True
        self.yAtCollision = 0
        self.animationYoffSet = 60
        self.upperLeft = Point2D(self.x, self.y)
        self.lowerRight = Point2D((self.x + self.width ) ,(self.y + self.height))
        self.velocity = Point2D(xVelocity, yVelocity)
        self.keyboard = pygame.key.get_pressed()
        self.animationSwitch = False
        self.collisionHappened = False    
        self.isAlive = True    
        ###
        #variables for timer
        ###
        self.timer = 0 #timer to reset the path
        self.timingFlag = False #flag to set to make the path reset after the timer
        self.delay = 1000 #1 second

    def setUpSprite(self):
        self.set_image_color_key(255, 0, 255)
        self.revertToFlyAnimation()
        self.set_animation_delay(500)
        self.play(True)
        
    def displayDeathAnimations(self):
        self.collisionHappened = True
        self.yAtCollision = self.y
        self.velocity.y = 1
        self.set_animation_delay(200)
        self.set_animation_frame(8)
        self.set_animation_range(8,14)

    
    def deathMove(self):
        self.y -= self.velocity.y
        
    def toggleIsAliveFalse(self, game):
        if(self.y == self.yAtCollision - self.animationYoffSet):
            self.isAlive = False
    
    def revertToFlyAnimation(self):
        self.set_animation_frame(0)
        self.set_animation_range(0, 1)
    
    def flyRightAnimation(self):
        self.set_animation_frame(1)
        self.set_animation_range(1, 3)
        
    def flyLeftAnimation(self):
        self.set_animation_frame(4)
        self.set_animation_range(4, 6)
        
    def moveLeft(self):
        self.x  -= self.velocity.x
        self.updateLowerRight()
        
    def moveRight(self):
        self.x  += self.velocity.x
        self.updateLowerRight()
        
    def moveUp(self):
        self.y  -= self.velocity.y
        self.updateLowerRight()
        
    def moveDown(self):
        self.y  += self.velocity.y
        self.updateLowerRight()
    
    def handleRightActions(self):
        self.moveRight()
        if(self.animationSwitch == False):
            self.flyRightAnimation()
            self.animationSwitch = True
            
    def handleLeftActions(self):
        self.moveLeft()
        if(self.animationSwitch == False):
            self.flyLeftAnimation()
            self.animationSwitch = True

    def checkScreenBounds(self):
        if(self.y <= 0):
            self.y = 1
        if(self.y + self.height >= screenHeight):
            self.y = screenHeight - self.height - 1 
        if(self.x <= 0):
            self.x = 1
        if(self.x + self.width>= screenWidth):
            self.x = screenWidth - self.width - 1
                    
    def handleInput(self):
            self.keyboard = pygame.key.get_pressed()
            if(self.collisionHappened == False):
                if(self.keyboard[self.keyLeft]):
                    self.handleLeftActions()
                    
                if(self.keyboard[self.keyRight]):
                    self.handleRightActions()
        
                if(not (self.keyboard[self.keyLeft] or self.keyboard[self.keyRight])):
                    self.animationSwitch = False
        
                if(self.keyboard[self.keyUp]):
                    self.moveUp()
                    self.revertToFlyAnimation()
            
                if(self.keyboard[self.keyDown]):
                    self.moveDown()
                    self.revertToFlyAnimation()
            
                if(self.keyboard[self.keySpace]== True and self.fireState == 0):
                    self.fireState = 1
                    
                if(self.keyboard[self.keySpace] == False and self.fireState == 1):
                    game.createPlayerLaser()
                    self.fireState = 0

    
    def updateLowerRight(self):
        self.lowerRight.x = self.upperLeft.x + self.width
        self.lowerRight.y = self.upperLeft.y + self.height
        
    def update(self, screen, game):
        self.checkScreenBounds()
        self.handleInput()
        if(self.collisionHappened == True):
            self.deathMove()
            self.toggleIsAliveFalse(game)
        self.draw(screen)

class ShooterGame():
    def __init__(self):
        self.gameState = 0
        self.player = None
        #self.enemy = None
        self.boss = None
        self.bullet = None
        self.pig = None
        self.bulletPlayerList = []
        self.bulletEnemyList = []
        self.numberOfEnemies = 10
        self.deadEnemies = 0
        self.rebuildEnemies = False
        self.lives = 3
        self.wave = 1
        self.score = 0
        self.enemyList = []
        self.pigList = []
        self.playerList = [] #also the lives
        self.keyboard = pygame.key.get_pressed()
        self.keyP = pygame.K_p
        self.keySpace = pygame.K_SPACE
        self.startTextPicture = pygame.image.load("startingText.bmp")
        self.gameOverTextPicture = pygame.image.load("gameOverText.bmp")
        self.winTextPicture = pygame.image.load("winningText.bmp")
        self.pauseKeyDown = False
        self.setUp = False
        self.textPositionY1 = 10
        self.textPositionY2 = 25
        self.textPositionY3 = 40
        self.textPositionX1 = (screenWidth / 2) - 85        

#make enemy bullet
    def createPlayerLaser(self):
        self.bullet = playerBullet("LaserSprite.bmp", self.player.x, self.player.y, 66, 30, 2, 1, 7, 7)
        self.bullet.setUpSprite()
        self.bulletPlayerList.append(self.bullet)

    def createEnemyLaser(self, enemy):
        self.bullet = enemyBullet("MissileSprite.bmp", enemy.x, enemy.y, 66, 30, 2, 1, 7, 7)
        self.bullet.setUpSprite()
        self.bulletEnemyList.append(self.bullet)
    
    def checkToDeletePlayerBullet(self, Bullet):
        if(Bullet.y < 0) or (Bullet.isAlive == False):
            self.bulletPlayerList.remove(Bullet)
    
    def checkToDeleteEnemyBullet(self, Bullet):
        if(Bullet.y > screenHeight) or (Bullet.isAlive == False):
            self.bulletEnemyList.remove(Bullet)
    
    def removeAllBullets(self):
        self.bulletEnemyList = []
        self.bulletPlayerList = []
                      
    def setUpGame(self):
        if(self.setUp == False):
            self.buildPlayer()
            self.buildEnemies()
            self.setUp = True
            
    def buildPlayer(self):
        #build player
        self.player = PlayerClass("AirplaneSprite.bmp", 500, 500, 504, 140, 7, 2, 5, 5)            
        self.player.setUpSprite()
    
    def buildBoss(self):
        #build boss
        self.boss = enemyBoss("enemyBoss.bmp", 800, 100, 200, 118, 4, 2, 10, 10)
        self.boss.setUpSprite()

    
    def buildEnemies(self):
        for i in range(0,self.numberOfEnemies):
            enemy = enemyFighterJet("enemyFighterJet.bmp", random.randint(35,screenWidth-25), 0, 360, 140, 5, 2, random.randint(0,1), random.randint(0,750), -10, 5, 5)
            enemy.setUpSprite(3, 3, 3, 0)
            self.enemyList.append(enemy)
        
            enemy = enemyFlyingPig("FlyingPigL2R.bmp", 0, 0, 120, 81, 3, 2, 2, 0, 0, 4, 4)
            enemy.setUpSprite()
            self.pigList.append(enemy)
            enemy = enemyFlyingPig("FlyingPigR2L.bmp", 0, 0, 120, 81, 3, 2, 3, 0, 0, 4, 4)
            enemy.setUpSprite()
            self.pigList.append(enemy)

    def createNewWave(self, numberOfEnemies):
        self.numberOfEnemies = numberOfEnemies
        for i in range(0,self.numberOfEnemies):
            enemy = enemyFighterJet("enemyFighterJet.bmp", random.randint(35,screenWidth-25), 0, 360, 140, 5, 2, random.randint(0,1), random.randint(0,750), -10, 5, 5)
            enemy.setUpSprite(3, 3, 3, 0)
            self.enemyList.append(enemy)
    
    def deleteEnemy(self, enemy):
        self.deadEnemies += 1
        self.enemyList.remove(enemy)
    
    def deletePlayer(self, player):
        self.playerList.remove(player)
    
    def addPoints(self, points):
        self.score += points
        
    def displayGameInfo(self):
        gameprint(screen, "Lives: " + str(self.lives), 1, 5, black)
        gameprint(screen, "Score: " + str(self.score), 1, 20, black)
        gameprint(screen, "Enemies: " + str(len(self.enemyList)), 1, 35, black)
        gameprint(screen, "Wave: " + str(self.wave), 1, 50, black)    


    def displayGameInfoBoss(self):
        gameprint(screen, "Lives: " + str(self.lives), 1, 5, black)
        gameprint(screen, "Score: " + str(self.score), 1, 20, black)
        gameprint(screen, "Enemies: 1", 1, 35, black)
        gameprint(screen, "Wave: " + str(self.wave), 1, 50, black)
        gameprint(screen, "Boss HP: " + str(self.boss.hitPoints), 1, 65, black)
        
    def reset(self):
        self.bulletEnemyList = []
        self.bulletPlayerList = []
        self.enemyList = []
        self.pigList = []
        self.deadEnemies = 0
        self.lives = 3
        self.score = 0
        self.buildEnemies()
        self.buildPlayer()
        
    def gameOver(self, pressedKey):
        screen.blit(self.gameOverTextPicture, (0,0))
        if(pressedKey[pygame.K_c] == True):
            self.reset()
            self.gameState = 1
    
    def winOver(self, pressedKey):
        screen.blit(self.winTextPicture, (0,0))
        if(pressedKey[pygame.K_r] == True):
            self.reset()
            self.gameState = 1
        if(pressedKey[pygame.K_ESCAPE ] == True):
            pygame.quit(); sys.exit();
    
            
    def togglePause(self):
        self.keyboard = pygame.key.get_pressed()
#Toggles Pause on and off
        if(self.keyboard[self.keyP] == True) and (self.pauseKeyDown == False):
            self.pauseKeyDown = True
            if(self.gameState == 1):
                self.gameState = 2
            else:
                self.gameState = 1
                 
        if(self.keyboard[self.keyP] == False) and (self.pauseKeyDown == True):
            self.pauseKeyDown = False
    
    def gameStateSetUp(self):
        self.setUpGame()
        screen.blit(self.startTextPicture, (0,0))
        if(self.keyboard[pygame.K_b]):
            self.gameState = 1

    def gameStateRun(self):
        for bullet in self.bulletPlayerList:
            self.checkToDeletePlayerBullet(bullet)
        for bullet in self.bulletEnemyList:
            self.checkToDeleteEnemyBullet(bullet)
        
        if(self.player.isAlive == True):
            self.player.update(screen, self)
        else:
            self.gameState = 3
        
        for enemy in self.enemyList:
            if(enemy.isAlive):
                enemy.update(screen, self.player, self)
                enemy.checkToShootPlayer(self.player)
            else:
                self.deleteEnemy(enemy)
        
        for pig in self.pigList:
            pig.update(screen, self.player)
        
        for bullet in self.bulletPlayerList:
            bullet.update(screen, self.enemyList, self.boss, self)
        for bullet in self.bulletEnemyList:
            bullet.update(screen, self.player)
    
    def bossRun(self):
        for bullet in self.bulletPlayerList:
            self.checkToDeletePlayerBullet(bullet)
        for bullet in self.bulletEnemyList:
            self.checkToDeleteEnemyBullet(bullet)
        
        if(self.player.isAlive == True):
            self.player.update(screen, self)
        else:
            self.gameState = 3
        
        if(self.boss.isAlive == True):
            self.boss.update(screen, self.player, self)
            self.boss.checkToShootPlayer()
        else:
            self.gameState = 6

        
        for pig in self.pigList:
            pig.update(screen, self.player)
        
        for bullet in self.bulletPlayerList:
            bullet.update(screen, self.enemyList, self.boss, self)
        for bullet in self.bulletEnemyList:
            bullet.update(screen, self.player)
        
    def checkToCreateEnemyWave(self):     
        if(len(self.enemyList) == 0) and (self.wave == 1):
            self.createNewWave(15)
            self.wave = 2
        if(len(self.enemyList) == 0) and (self.wave == 2):
            self.createNewWave(20)
            self.wave = 3
            
    def createBoss(self):
        self.buildBoss()
        self.gameState = 5           

    def gameStatePause(self):
        gameprint(screen, "Pause, Press P to Unpause", (screenHeight / 2), (screenWidth / 2), purple)
        self.player.draw(screen)
        for bullet in self.bulletList:
            bullet.draw(screen)
         
    def run(self):
        self.keyboard = pygame.key.get_pressed()
        self.togglePause() 
#Set Up State
        if(self.gameState == 0):
            self.gameStateSetUp()
#Run State
        if(self.gameState == 1):
            self.displayGameInfo() 
            self.gameStateRun()
            self.checkToCreateEnemyWave()
            if(len(self.enemyList) <= 0) and (self.wave == 3):
                self.createBoss()         
#Pause State
        if(self.gameState == 2):
            self.gameStatePause()
#Death State
        if(self.gameState == 3):
            if(self.lives != 1):
                self.buildPlayer()
                self.removeAllBullets()
                self.gameState = 1
                self.lives -= 1
            else:
                self.reset()
                self.gameState = 4
#Game Over State
        if(self.gameState == 4):
            self.gameOver(self.keyboard)
#Boss Fight State
        if(self.gameState == 5):
            self.displayGameInfoBoss() 
            self.bossRun()
#You Win State
        if(self.gameState ==6):
            self.winOver(self.keyboard)

            
                
            

game = ShooterGame()
while(True):
    screen.blit(background, (0,0))
    game.run()
    msElasped = clock.tick(60)
    pygame.display.update()
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            pygame.quit(); sys.exit();