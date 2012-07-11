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
    class Phenomenon
    {
        public static Random rnd = new Random();
        private int width = 4;
        private int height = 4;
        private int phenomenonLife;
        private Point source;
        private Point location;


        public Phenomenon(int inX, int inY, Point inFoodSource)
        {
            this.phenomenonLife = 150;
            this.location = new Point(inX, inY);
            this.source = inFoodSource;
        }

        public void refresh()
        {
            phenomenonLife = 150;
        }

        public void updatePhenomenon()
        {
            phenomenonSpread();
            phenomenonLife -= 1;
        }

        public void phenomenonSpread()
        {
            this.location.X += rnd.Next(-1, 2);
            this.location.Y += rnd.Next(-1, 2);
        }

        public int getPhenomenonAmount()
        {
            return phenomenonLife;
        }

        internal Point getLocation()
        {
            return this.location;
        }

        internal int getWidth()
        {
            return this.width;
        }

        internal int getHeight()
        {
            return this.height;
        }
    
        internal Point getSource()
        {
            return this.source;
        }              
    }
}
