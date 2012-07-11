using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AntsAgentBasedModel
{
    class Food
    {
        private int foodQuantity;
        private int width = 12;
        private int height = 12;
        private int x;
        private int y;
        private Point location;
        private Image image = AntsAgentBasedModel.Properties.Resources.Food;
        public bool outOfFood = false;

        public Food(int inFoodQuantity, int inX, int inY)
        {
            this.foodQuantity = inFoodQuantity;
            this.x = inX;
            this.y = inY;
            this.location = new Point(x, y);
        }

        public void antTakesFood()
        {
            this.foodQuantity--;
            if(this.foodQuantity <= 0)
                outOfFood = true;
        }

        internal Image getImage()
        {
            return image;
        }

        internal Point getLocation()
        {
            return location;
        }

        internal int getWidth()
        {
            return width;
        }

        internal int getHeight()
        {
            return height;
        }
    }
}
