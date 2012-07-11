using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace caApplication
{
    class Cell
    {
        //sets up all the variables i will need for CA
        private int xPosition;
        private int yPosition;
        private Point location;
        private Cell[] neighbors;
        private Boolean currentState;
        //is there anyway to get around this by like having the pictures part of the project
        //and then have the project look at a content or image folder instead of manually setting the path
        //everytime I change computers my usb drive would change letters and I would run it and it would say
        //exception files not found! and then I'm like dammit how many times do I have to do this until i remember
        //apparently more than 20times X(
        private Image deadImage = Image.FromFile("I:\\caApplication\\caApplication\\DeadCell.png");
        private Image aliveImage = Image.FromFile("I:\\caApplication\\caApplication\\AliveCell.png");

        //public constructor that creates an array of cells
        //sets the current state of the cell, its x and y position,
        //and a point which is the x, y position * CELL_WIDTH(16), CELL_HEIGHT(16)
        public Cell(Boolean inDeadOrAlive, int inXPosition, int inYPosition)
        {
            neighbors = new Cell[5];
            currentState = inDeadOrAlive;
            xPosition = inXPosition;
            yPosition = inYPosition;
            location = new Point(xPosition * CellularAutomataConstants.CELL_WIDTH, yPosition * CellularAutomataConstants.CELL_HEIGHT);
        }
        //returns the location
        public Point getLocation()
        {
            return location;
        }
        //return the state
        public Boolean getCurrentState()
        {
            return currentState;
        }
        //sets the state
        public void setCurrentState(Boolean inDeadOrAlive)
        {
            currentState = inDeadOrAlive;
        }
        //returns the image of the cell depending if Alive or dead
        public Image getImage()
        {
            if (currentState == true)
                return aliveImage;
            else
                return deadImage;
        }
        //sets up the neighbors of the cells and puts them into
        //the neighborState array
        public void setNeighbors(Cell[,] grid)
        {
            //handle toporidal thing
            int leftOfX = xPosition - 1;
            int rightOfX = xPosition + 1;
            int bottomOfY = yPosition + 1;
            int topOfY = yPosition - 1;

            if (leftOfX == CellularAutomataConstants.BELOW_GRID)
                leftOfX = grid.GetUpperBound(0);
            if (rightOfX == CellularAutomataConstants.GRID_WIDTH)
                rightOfX = 0;
            if (bottomOfY == CellularAutomataConstants.GRID_HEIGHT)
                bottomOfY = 0;
            if (topOfY == CellularAutomataConstants.BELOW_GRID)
                topOfY = grid.GetUpperBound(1);

            neighbors[0] = this;
            neighbors[1] = grid[xPosition, topOfY];
            neighbors[2] = grid[rightOfX, yPosition];
            neighbors[3] = grid[xPosition, bottomOfY];
            neighbors[4] = grid[leftOfX, yPosition];
        }
        //returns the neighborState array in the same format as the rules
        public int getNeighborhoodState(Cell[,] grid)
        {
            setNeighbors(grid);
            String neighborhoodState = Convert.ToInt32(this.currentState).ToString() +
                Convert.ToInt32(neighbors[1].getCurrentState()).ToString() +
                Convert.ToInt32(neighbors[2].getCurrentState()).ToString() +
                Convert.ToInt32(neighbors[3].getCurrentState()).ToString() +
                Convert.ToInt32(neighbors[4].getCurrentState()).ToString();
            return Convert.ToInt32(neighborhoodState);
        }
        //override ToString method used for debugging
        public override string ToString()
        {
            return string.Format("I am a {0} cell, at location {1}, {2}", currentState, xPosition, yPosition);
        }

    }
}
