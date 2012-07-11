'''
Created on Jul 24, 2011

@author: jasferri
'''
'''
Just add
import pygame.mixer
pygame.mixer.stop()
pygame.mixer.init(44100,-16,2,2048)
to the file that will create the sound object
'''
import pygame

class GameSound():
    def __init__(self):
        self.sound1 = pygame.mixer.Sound("48CellTheme.wav")
