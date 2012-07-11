'''
Created on Jul 23, 2011

@author: jasferri
'''
class Gun():
    def __init__(self, playerOrEnemy):
        self.bulletArray = []
        self.isPlayer = playerOrEnemy
        
    def addBullet(self, newBullet):
        self.bulletArray.append(newBullet)
        
    def bulletControl(self, screen, enemyList):
        for bullet in self.bulletArray:
            if((bullet.upperLeft.x <= 0) or (bullet.upperLeft.x >= screen.get_width()) or (bullet.upperLeft.y <= 0) or (bullet.upperLeft.y >= screen.get_height() - 115)):
                    self.bulletArray.remove(bullet)
                    break
            if(self.isPlayer):
                for enemy in enemyList:
                    if bullet.upperLeft.x <= enemy.lowerRight.x and bullet.lowerRight.x >= enemy.upperLeft.x:
                        if bullet.upperLeft.y <= enemy.lowerRight.y and bullet.lowerRight.y >= enemy.upperLeft.y:
                            self.bulletArray.remove(bullet)
                            enemy.damage()
            else:
                if bullet.upperLeft.x <= enemyList.lowerRight.x and bullet.lowerRight.x >= enemyList.upperLeft.x:
                    if bullet.upperLeft.y <= enemyList.lowerRight.y and bullet.lowerRight.y >= enemyList.upperLeft.y:
                        self.bulletArray.remove(bullet)
                        enemyList.damage()                
            bullet.update(screen)

    def update(self, screen, enemyList):
        self.bulletControl(screen, enemyList)
        
        
