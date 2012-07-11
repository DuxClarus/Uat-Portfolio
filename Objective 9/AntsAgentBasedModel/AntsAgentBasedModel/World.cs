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
    class World
    {
        private int height;
        private int width;
        private Nest nest;
        private List<Ant> antList;
        private List<Food> foodList;
        private List<Food> foodListTemp;
        private List<Phenomenon> phenomenonList;

        public World(int inHeight, int inWidth, List<Ant> inAntList, List<Food> inFoodList, Nest inNest)
        {
            this.height = inHeight;
            this.width = inWidth;
            this.antList = inAntList;
            this.foodList = inFoodList;
            this.foodListTemp = inFoodList;
            this.phenomenonList = new List<Phenomenon>();
            this.nest = inNest;
        }

        public Graphics play(Graphics gpx)
        {
            if (phenomenonList.Count > 0)
            {
                foreach (Phenomenon p in phenomenonList)
                {
                    p.updatePhenomenon();
                    gpx.DrawImage((p.getPhenomenonAmount() >= 25) ? Constants.pheromoneImage : Constants.fadedPheromoneImage,  p.getLocation().X, p.getLocation().Y, p.getWidth(), p.getHeight());
                }
            }
            foreach (Ant ant in antList)
            {
                ant.update(this);
                gpx.DrawImage(ant.getImage(), ant.getLocation().X, ant.getLocation().Y, ant.getWidth(), ant.getHeight());
            }
            foreach (Food food in foodList)
            {
                gpx.DrawImage(food.getImage(), food.getLocation().X, food.getLocation().Y, food.getWidth(), food.getHeight());
            }
            deleteFood();
            deletePhenomenon();
            return gpx;
        }

        private void deletePhenomenon()
        {
            for (int index = phenomenonList.Count - 1; index >= 0; index--)
            {
                 if (phenomenonList[index].getPhenomenonAmount() <= 0)
                      phenomenonList.RemoveAt(index);
            }
        }

        private void deleteFood()
        {
            for (int index = 0; index < foodList.Count; index++)
            {
                if (foodList[index].outOfFood == true)
                    foodListTemp.Remove(foodListTemp[index]);
            }
            foodList = foodListTemp;
        }

        internal void createPhenomenonSpot(List<Phenomenon> inPhenomenonList)
        {
            phenomenonList.AddRange(inPhenomenonList);
        }

        internal int getHeight()
        {
            return this.height;
        }

        internal int getWidth()
        {
            return this.width;
        }

        internal List<Food> getFoodList()
        {
            return foodList;
        }

        internal List<Phenomenon> getPhenomenonList()
        {
            return phenomenonList;
        }
    }


}
