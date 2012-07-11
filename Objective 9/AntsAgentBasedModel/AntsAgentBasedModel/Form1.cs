using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace AntsAgentBasedModel
{
    public partial class AntsWorldForm : Form
    {
        public static Random rnd = new Random();
        int numberOfAnts;
        int numberOfFood;
        bool playAnts = false;
        List<Ant> antList = new List<Ant>();
        List<Food> foodList = new List<Food>();
        Nest nest;
        World world;
        Bitmap bmp = null;
        Graphics gpx = null;

        public AntsWorldForm()
        {
            InitializeComponent();
            bmp = new Bitmap(AntsWorld.Width, AntsWorld.Height);
            gpx = Graphics.FromImage(bmp);
        }

        public void generateAnts()
        {
            for (int index = 1; index <= int.Parse(tbNumberOfAnts.Text); index++)
            {
                Ant ant = new Ant((AntsWorld.Width / 2), (AntsWorld.Height / 2), nest);
                antList.Add(ant);
            }
        }

        public void generateFood()
        {
            for (int index = 1; index <= int.Parse(tbNumberOfFood.Text); index++)
            {
                Food food = new Food(rnd.Next(0, 50), rnd.Next(0, AntsWorld.Width), rnd.Next(0, AntsWorld.Height));
                foodList.Add(food);
            }
        }

        private void genereateNest()
        {
            nest = new Nest((AntsWorld.Width / 2), (AntsWorld.Height / 2));
        }

        private void setUpWorld()
        {
            world = new World(AntsWorld.Width, AntsWorld.Height, antList, foodList, nest);
        }

        private void updateAntsWorld()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    AntsWorld.Image = bmp;
                    AntsWorld.Refresh();
                });
            }
            else
            {
                AntsWorld.Image = bmp;
                AntsWorld.Refresh();
            }
        }

        private void tbNumberOfAnts_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.numberOfAnts = int.Parse(tbNumberOfAnts.Text);
            }
            catch
            {
                MessageBox.Show("The number of ants must be an integer! " + tbNumberOfAnts.Text + " is not valid");
            }
        }

        private void tbNumberOfFood_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.numberOfFood = int.Parse(tbNumberOfFood.Text);
            }
            catch
            {
                MessageBox.Show("The number of ants must be an integer! " + tbNumberOfFood.Text + " is not valid");
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            playAnts = true;
            Thread playThread = new Thread(new ThreadStart(play));
            playThread.Start();
        }

        private void play()
        {
            while (playAnts)
            {
                gpx.Clear(Color.BlanchedAlmond);
                gpx = world.play(gpx);
                updateAntsWorld();
            }
        }

        private void btnSetUpWorld_Click(object sender, EventArgs e)
        {
            genereateNest();
            generateAnts();
            generateFood();
            setUpWorld();
        }
    }
}
