'''
            Credits:
            Ethan Behar - Programmer
            Nick Ferri - Sound Engineer
'''

import pygame
import random
import sys
import time
from Character import Character
from Color import Color
from Enemy import Enemy
from Game import Game
from enums import *

def gamePrint(screen,text,xx,yy,color):
    font = pygame.font.SysFont("Courier New",18)
    ren = font.render(text,1,color)
    screen.blit(ren, (xx,yy))

pygame.init()
clock = pygame.time.Clock()
resolutionX = 800
resolutionY = 800
black = (0,0,0)
white = (255,255,255)
screen = pygame.display.set_mode((resolutionX, resolutionY))

cellGame = Game(screen, clock)
cellGame.gameState = gameState.MENU_STATE

keypress = pygame.key.get_pressed()
while(keypress[pygame.K_ESCAPE] == False):
    keypress = pygame.key.get_pressed()

    screen.fill(black) #Clears the screen

    msElapsed = clock.tick(30) #Frames per second
    cellGame.update(screen) #tells the game to update its objects
    if(cellGame.gameState == gameState.PLAY_STATE):
        cellGame.draw(screen)
    cellGame.drawHUD(screen)
    #updates our display with all of the changes
    #gamePrint(screen, str(clock.get_fps()), 0,0, white)
    pygame.display.update()

   
    for event in pygame.event.get(): #Exits the program if the X is pressed.
        if event.type == pygame.QUIT:
            pygame.quit(); sys.exit();
            
pygame.quit() #Exits if loop is broken (Esc pressed)
sys.exit()
