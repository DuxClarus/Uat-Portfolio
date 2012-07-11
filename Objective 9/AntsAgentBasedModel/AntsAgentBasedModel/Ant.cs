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

    enum Direction { NORTH = 1, EAST, SOUTH, WEST };
    enum State { RANDOM_MOVEMENT = 1, SMELLING_PHENOMENON, GOING_TO_NEST }

    class Ant
    {
        public static Random rnd = new Random();
        private int x;
        private int y;
        private int width = 4;
        private int height = 4;
        private int phenomenonAmount;
        private Nest nest;
        private Point location;
        private Point foodSource;
        private Point phenomenonSource;
        private State job;
        private Direction direction;
        private List<Phenomenon> phenomenonList;
        private Image image = AntsAgentBasedModel.Properties.Resources.Ant;
        private Image imageWithFood = AntsAgentBasedModel.Properties.Resources.AntWithFood;

        public Ant(int inX, int inY, Nest inNest)
        {
            this.x = inX;
            this.y = inY;
            this.nest = inNest;
            this.location = new Point(x, y);
            this.direction = randomGenerateDirection();
            this.phenomenonAmount = 150;
            this.phenomenonList = new List<Phenomenon>();
            this.job = State.RANDOM_MOVEMENT;
        }

        private Direction randomGenerateDirection()
        {
            int randomDirection = rnd.Next(1, 5);
            if (randomDirection == 1)
                return Direction.NORTH;
            if (randomDirection == 2)
                return Direction.EAST;
            if (randomDirection == 3)
                return Direction.SOUTH;
            if (randomDirection == 4)
                return Direction.WEST;
            throw new Exception("Direction cannot be resolved: " + randomDirection);
        }
        
        private void smell(List<Phenomenon> inPhenomenonList)
        {
            foreach (Phenomenon p in inPhenomenonList)
            {
                if (p.getLocation().X - 50 <= this.location.X && this.location.X <= (p.getLocation().X + p.getWidth() + 50))
                    if (p.getLocation().Y - 50  <= this.location.Y && this.location.Y <= (p.getLocation().Y + p.getHeight() + 50))
                    {
                        this.phenomenonSource = new Point(p.getSource().X, p.getSource().Y);
                        if (phenomenonSource.X == 0 && phenomenonSource.Y == 0)
                        {
                            this.job = State.RANDOM_MOVEMENT;
                        }
                        else
                        {
                            this.job = State.SMELLING_PHENOMENON;
                        }
                    }
                }
        }

        public void move(World world)
        {
            if (job == State.RANDOM_MOVEMENT)
            {
                switch (direction)
	            {
                case Direction.NORTH:
		            location.Y--;
		            break;
                case Direction.EAST:
                    location.X--;
		            break;
                case Direction.SOUTH:
                    location.Y++;
		            break;
                case Direction.WEST:
                    location.X++;
		            break;
                default:
                    throw new Exception("Direction is not recognized: " + direction);
	            }
                randomizeMovement();
                grabFood(world.getFoodList());
            }
            else if (job == State.SMELLING_PHENOMENON)
            {
                moveToPhenomenon();
            }
            else if (job == State.GOING_TO_NEST)
            {
                if (phenomenonAmount > 0)
                    dropPhenomenon(world);
                goToNest();
            }
            handleToroidalBounds(world.getHeight(), world.getWidth());
        }

        private void moveToPhenomenon()
        {
            if (location.X < phenomenonSource.X)
            {
                location.X++;
            }
            if (location.X > phenomenonSource.X)
            {
                location.X--;
            }
            if (location.Y < phenomenonSource.Y)
            {
                location.Y++;
            }
            if (location.Y > phenomenonSource.Y)
            {
                location.Y--;
            }

            if (phenomenonSource == this.location)
            {
                this.job = State.GOING_TO_NEST;
            }
        }
    
        private void randomizeMovement()
        {
            if (rnd.Next(100) < 1)
            {
                this.direction = randomGenerateDirection();
            }
        }

        private void dropPhenomenon(World world)
        {
            phenomenonAmount = (int)(phenomenonAmount - (phenomenonAmount * 0.025f));
            spreadPhenomenon((int)(phenomenonAmount * 0.1f));
            phenomenonList = HelperClass.help(phenomenonList, world);
            world.createPhenomenonSpot(phenomenonList);
            phenomenonList.Clear();
        }

        public void spreadPhenomenon(int inPhenomenonAmount)
        {
            for (int index = 0; index <= inPhenomenonAmount; index++)
            {
                this.phenomenonList.Add(new Phenomenon((location.X + rnd.Next(0, 6)), (location.Y + rnd.Next(0, 6)), foodSource));
            }
        }

        private void goToNest()
        {
            if(location.X < nest.getLocation().X)
            {
                location.X++;
            }
            if(location.X > nest.getLocation().X)
            {
                location.X--;
            }
            if(location.Y < nest.getLocation().Y)
            {
                location.Y++;
            }
            if (location.Y > nest.getLocation().Y)
            {
                location.Y--;
            }

            if (nest.getLocation() == this.location)
            {
                nest.antDropsFood();
                this.job = State.RANDOM_MOVEMENT;
                this.phenomenonAmount = 150;
            }
        }

        private void handleToroidalBounds(int WorldHeight, int WorldWidth)
        {
            if (location.X < 0)
                location.X = WorldHeight - 1;
            if (location.X >= WorldHeight)
                location.X = 0;
            if (location.Y < 0)
                location.Y = WorldWidth - 1;
            if (location.Y >= WorldWidth)
                location.Y = 0;
        }

        private void grabFood(List<Food> foodList)
        {
            foreach (Food food in foodList)
            {
                if (food.getLocation().X <= this.location.X && this.location.X <= (food.getLocation().X + food.getWidth()))
                    if (food.getLocation().Y <= this.location.Y && this.location.Y <= (food.getLocation().Y + food.getHeight()))
                    {
                        food.antTakesFood();
                        foodSource = food.getLocation();
                        this.job = State.GOING_TO_NEST;
                    }
            }
        }

        internal void update(World world)
        {
            if(job == State.RANDOM_MOVEMENT)
                smell(world.getPhenomenonList());
            move(world);
        }

        internal Image getImage()
        {
            if (job == State.GOING_TO_NEST)
                return this.imageWithFood;
            else
                return this.image;
        }

        internal Point getLocation()
        {
            return this.location;
        }

        internal int getX()
        {
            return this.x;
        }

        internal int getY()
        {
            return this.y;
        }

        internal int getWidth()
        {
            return this.width;
        }

        internal int getHeight()
        {
            return this.height;
        }
    }
}
