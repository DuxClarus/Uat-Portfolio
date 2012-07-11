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
    class Nest
    {
        private int width = 12;
        private int height = 12;
        private int x;
        private int y;
        private Point location;
        private int foodCounter = 0;
        private Image image = AntsAgentBasedModel.Properties.Resources.Nest;

        public Nest(int inX, int inY)
        {
            this.x = inX;
            this.y = inY;
            this.location = new Point(x, y);
        }

        internal void antDropsFood()
        {
            foodCounter++;
        }

        internal Point getLocation()
        {
            return location;
        }


        internal Image getImage()
        {
            return image;
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
