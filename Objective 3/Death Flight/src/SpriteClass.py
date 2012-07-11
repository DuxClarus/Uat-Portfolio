import pygame #get the pygame library
import sys #get some of the python library stuff
pygame.init()


#===========================================================================
#sprite_class
#
#This class is a sprite handler for pygame.  The goal of this class was to
#maximize ease of use.  Much of the functionality is based on DarkBASIC sprites
#===========================================================================
class SpriteClass:
    
    def __init__(self,filename,x,y,width,height,columns,rows):
        self.image = pygame.image.load(filename)        
        self.width = width
        self.height = height
        self.rows = rows
        self.columns = columns        
        self.x = x
        self.y = y
        self.frameHeight = self.height//self.rows
        self.frameWidth = self.width//self.columns
        self.scaledFrameHeight = self.frameHeight
        self.scaledFrameWidth = self.frameWidth
        self.frame = pygame.Rect(0,0,self.frameWidth,self.frameHeight)
        self.max_frames = self.rows*self.columns
        self.animate = False
        self.frameNumber = 0 #frame number has to be zero based
        self.frameX = 0
        self.frameY = 0
        self.rowNumber = 0
        self.colNumber = 0
        self.animationRange = (0,self.max_frames)
        self.delayTimer = 100
        self.timer = 0
        self.angle = 0
        self.rotated = False
        self.scale = 1
        self.scaled = False
        self.original = self.image
        self.image_list = self.load_sliced_sprites()
        self.mod_image_list = self.image_list
        self.transparent_color = (0,0,0)
    
        
    #===========================================================================
    #  Draw
    #  Parameters:
    #     screen - this is the display which the sprite will be drawn on
    #  Return: none
    #
    #  This function causes the sprite to draw itself to the screen
    #===========================================================================
    def draw(self,screen):
        if (self.animate) and (pygame.time.get_ticks() - self.timer >= self.delayTimer):
            self.timer = pygame.time.get_ticks()
            self.advance_frame()

 
        screen.blit(self.mod_image_list[self.frameNumber],(self.x,self.y))

    #===========================================================================
    # load_sliced_sprites
    # Parameters: none
    # Return:
    #     images - this is the list of subimages that make up the animation
    #
    # This is a helper function used to cut a sprite-sheet into its animation frames
    # the frames are stored in an image list and are used in subsequent functions
    #===========================================================================
    def load_sliced_sprites(self):
    #===========================================================================
    #  '''
    #  Specs :
    #     Master can be any height.
    #     Sprites frames width must be the same width
    #     Master width must be len(frames)*frame.width
    #  Assuming you ressources directory is named "ressources"
    #  '''
    #===========================================================================
        images = []
        # master_image = pygame.image.load(os.path.join('resources', filename)).convert_alpha()
    
        for i in range(self.max_frames):
            self.frameNumber = i
            self.get_animation_frame()
            # images.append(self.image.subsurface((i*self.frameWidth,0,self.frameWidth,self.frameHeight)))
            images.append(self.image.subsurface((self.frameX, self.frameY, self.frameWidth, self.frameHeight)))
        return images

    #===========================================================================
    # play
    # Parameters:
    #     value - this is a true/false value that turns animation on and off
    # Return: none
    #
    # This function allows the user to turn animation on and off
    #===========================================================================
    def play(self, value):
        self.animate = value
    
    #===========================================================================
    # set_animation_delay
    # Parameters:
    #     value - this is an integer value that sets number of miliseconds delay
    # Return: none
    #
    # This function sets the amount of delay between each animation frame
    #===========================================================================
    def set_animation_delay(self,value):
        self.delayTimer = value

    #===========================================================================
    # set_animation_range
    # Parameters:
    #     first - this is the starting animation frame
    #     last - this is the last animation frame
    # Return: none
    #
    # This function specifies a set animation range, the sprite will only animate
    # over the specified frame range.
    #===========================================================================
    def set_animation_range(self,first,last):
        self.animationRange = (first,last)
        self.get_animation_frame()
    
    #===========================================================================
    # set_animation_frame
    # Parameters:
    #     frame - the frame number being set
    # Returns: none
    #
    # This function sets the current animation frame and sets up the background
    # stuff needed to calculate the sprite sheet frame
    #===========================================================================
    def set_animation_frame(self,frame):
        self.frameNumber = frame
        self.rowNumber = (self.animationRange[0] - self.colNumber) / self.columns
        self.colNumber=(self.frameNumber%self.columns)
        self.frameX = self.colNumber*self.frameWidth
        self.frameY = self.rowNumber*self.frameHeight
    
    #===========================================================================
    # advance_frame
    # Parameters: none
    # Retunrs: none
    #
    # This function advances the animation frame and does some range checking to
    # correctly loop the animation over the animation range
    #===========================================================================      
    def advance_frame(self):
        self.get_animation_frame()
        self.frameNumber += 1

        if(self.frameNumber >= self.animationRange[1]) or (self.frameNumber >= self.max_frames):
            self.frameNumber = self.animationRange[0]
    #===========================================================================
    # get_animation_frame
    # Parameters: none
    # Return: none
    #
    # This function resets frame information.  It's a helper function to cut up
    # the sprite sheet
    #===========================================================================
    def get_animation_frame(self):
        self.frameX = (self.frameNumber % self.columns) * self.frameWidth
        self.frameY = int(self.frameNumber / self.columns) * self.frameHeight  
        self.frame = (self.frameX,self.frameY,self.frameWidth, self.frameHeight)
    
    #===========================================================================
    # rotate_sprite
    # Parameters:
    #     angle - this is the angle the sprite will be rotated to Counter Clockwise
    # Returns: none
    #
    # This function rotates the sprite.  This is accomplished by applying the affect
    # to all the animation frames in the image_list
    #===========================================================================
    def rotate_sprite(self,angle):
        self.angle = angle
        self.rotated = True
        # temp = self.image.subsurface(self.frame)
        # rotate = pygame.transform.rotate
        # self.image = rotate(temp, self.angle)
        for index in range(self.max_frames):
            self.mod_image_list[index] = pygame.transform.rotate(self.image_list[index],self.angle)
    #===========================================================================
    # scale_sprite
    # Parameters:
    #     scale - this is the scale by which the sprite will be increased
    # Returns: none
    #
    # This function enlarges the sprite by the scale specified.  Right now it only
    # does integers ... this will need to be updated
    #===========================================================================
    def scale_sprite(self,scale):
        self.scale = scale
        self.scaled = True
        self.scaledFrameWidth = self.frameWidth * self.scale
        self.scaledFrameHeight = self.frameHeight * self.scale
        if self.image_list != []:
            for index in range(self.max_frames):
                self.mod_image_list[index] = pygame.transform.scale(self.image_list[index],(self.scaledFrameWidth,self.scaledFrameHeight))
    #===========================================================================
    # set_image_color_key
    # Parameters:
    #     r - red value of the RGB color
    #     g - green value of the RGB color
    #     b - blue value of the RGB color
    # Returns: none
    #
    # This function sets the transparent color for the sprite.  This is applied
    # to all animation frames stored in image_list
    #===========================================================================
    def set_image_color_key(self,r,g,b):
        self.transparent_color = (r,g,b)
        
        if self.image_list != []:
            self.image.set_colorkey(self.transparent_color)
            for index in range(self.max_frames):
                self.image_list[index].set_colorkey(self.transparent_color)
                self.mod_image_list[index].set_colorkey(self.transparent_color)

