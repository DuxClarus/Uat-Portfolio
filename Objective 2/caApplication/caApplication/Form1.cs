using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Timers;

namespace caApplication
{
    public partial class formCA : Form
    {
        //initialize all the varaibles the program will need for CA
        private static Random rnd = new Random();
        private Cell[,] board = new Cell[20, 20];
        private Cell[,] nextBoard = new Cell[20, 20];
        private List<int> rulesInAffect = new List<int>();
        private System.Timers.Timer wait = new System.Timers.Timer(500);
        private int numberOfGenerations = 1;
        private Boolean run = false;
        private OpenFileDialog dialog = null;
        private Stream myStream = null;
        private Bitmap bmp = null;
        private Graphics gpx = null;
        //sets up the bitmap to the size of the picture box
        //and sets up the graphics to be the bitmap
        public formCA()
        {
            InitializeComponent();
            bmp = new Bitmap(pbBoard.Width, pbBoard.Height);
            gpx = Graphics.FromImage(bmp);
        }

        //randomly initializes the board to either dead or alive cells
        //then copies board to nextBoard to avoid re-instantiating the cells for nextBoard
        //then draws the board
        private void initializeBoardRandomly()
        {
            Boolean deadOrAlive;
            for (int x = 0; x < CellularAutomataConstants.GRID_WIDTH; x++)
            {
                for (int y = 0; y < CellularAutomataConstants.GRID_HEIGHT; y++)
                {
                    int intAliveOrDead = rnd.Next(0, 2);
                    if (intAliveOrDead == 0)
                        deadOrAlive = false;
                    else
                        deadOrAlive = true;
                    createCell(deadOrAlive, x, y);
                }
            }
            Array.Copy(board, nextBoard, board.Length);
            drawAfterInitialization();
        }
        //loads the board with either alive or dead cells from a .txt file containing a grid of 1s and 0s 20 by 20
        //then copies board to nextBoard to avoid re-instantiating the cells for nextBoard
        //then draws the board
        private void initializeBoardByFile()
        {
            int x = 0;
            int y = 0;
            Boolean deadOrAlive;
            using (StreamReader reader = new StreamReader(this.myStream))
            {
                String line;
                while ((line = reader.ReadLine()) != null)
                {
                    foreach (char character in line)
                    {
                        if (x > (CellularAutomataConstants.GRID_WIDTH - CellularAutomataConstants.INDEX_OFFSET))
                        {
                            x = 0;
                        }
                        if (character == '0')
                            deadOrAlive = false;
                        else
                            deadOrAlive = true;
                        createCell(deadOrAlive, x, y);
                        x++;
                    }
                    y++;
                }
            }
            Array.Copy(board, nextBoard, board.Length);
            drawAfterInitialization();
        }
        //simple method that creates a cell in position x,y 
        //and is alive or dead depending on the boolean varible
        private void createCell(Boolean deadOrAlive, int x, int y)
        {
            this.board[x, y] = new Cell(deadOrAlive, x, y);
        }
        //a simple method used to draw the board and update the picture box
        private void drawAfterInitialization()
        {
            drawBoard();
            updatePictureBox();
        }

        #region methods that update the board and cells
        //updates the board while run is true
        //also makes the program sleep for .5secs so the updates are visible
        //catches exception of null reference meaning user didnt intialize the board first
        private void updateBoardForever()
        {
            while (run == true)
            {
                try
                {
                    gpx.Clear(Color.White);
                    drawBoard();
                    nextGeneration();
                    updatePictureBox();
                    System.Threading.Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("You must initialize the board first by clicking Random or Load: " + ex.Message);
                    return;
                }
            }
        }
        //updates the board once
        //catches exception of null reference meaning user didnt intialize the board first
        private void updateBoardOnce()
        {
            try
            {
                gpx.Clear(Color.White);
                drawBoard();
                nextGeneration();
                updatePictureBox();
            }
            catch (Exception ex)
            {
                MessageBox.Show("You must initialize the board first by clicking Random or Load: " + ex.Message);
                return;
            }
        }
        //updates the board until the number of generations is reached
        //also makes the program sleep for .5secs so the updates are visible
        //catches exception of null reference meaning user didnt intialize the board first
        private void updateBoardGenerations()
        {
            int counter = 0;
            while (counter != numberOfGenerations)
            {
                try
                {
                    gpx.Clear(Color.White);
                    drawBoard();
                    nextGeneration();
                    updatePictureBox();
                    counter++;
                    System.Threading.Thread.Sleep(250);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("You must initialize the board first by clicking Random or Load: " + ex.Message);
                    return;
                }
            }
        }
        //draws the cells to the graphics
        private void drawBoard()
        {
            for (int x = 0; x < CellularAutomataConstants.GRID_WIDTH; x++)
            {
                for (int y = 0; y < CellularAutomataConstants.GRID_HEIGHT; y++)
                {
                    gpx.DrawImage(board[x, y].getImage(), board[x, y].getLocation());
                }
            }
        }
        //generates the next generation of cells and sets the cells 
        //to nextBoard then we copy nextBoard into board
        private void nextGeneration()
        {
            for (int x = 0; x < CellularAutomataConstants.GRID_WIDTH; x++)
            {
                for (int y = 0; y < CellularAutomataConstants.GRID_HEIGHT; y++)
                {
                    if (rulesInAffect.Contains(board[x, y].getNeighborhoodState(board)) != false)
                        nextBoard[x, y].setCurrentState(true);
                    else
                        nextBoard[x, y].setCurrentState(false);
                }
            }
            Array.Copy(nextBoard, board, nextBoard.Length);
        }
        //updates the pictureBox and displays new generation of cells
        private void updatePictureBox()
        {
            pbBoard.Image = bmp;
            pbBoard.Refresh();
        }
        #endregion

        #region button handler methods
        //sets run to true and calls updateBoardForever()
        private void btnRunForever_Click(object sender, EventArgs e)
        {
            run = true;
            updateBoardForever();
        }
        //calls updateBoardOnce
        private void btnStepByStep_Click(object sender, EventArgs e)
        {
            updateBoardOnce();
        }
        //calls updateBoardGenerations
        private void btnRunGenerations_Click(object sender, EventArgs e)
        {
            updateBoardGenerations();
        }
        //doesnt work but it sets run to false
        //i believe it doesn't work becuase while the board is running
        //I cant click on any other buttons because I never left the button click methods
        //I didn't know how to fix this obviously
        private void btnStop_Click(object sender, EventArgs e)
        {
            run = false;
        }
        //sets the number of generations
        private void btnSetGeneratiosn_Click(object sender, EventArgs e)
        {
            try
            {
                numberOfGenerations = Convert.ToInt32(txtBGenerations.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Must enter a number for the generations: " + ex.Message);
            }
        }
        //calls initializeBoardRandomly
        private void btnRandom_Click(object sender, EventArgs e)
        {
            initializeBoardRandomly();
        }
        //loads a .txt file from the computer
        //then calls initializeBoardByFile
        private void btnLoad_Click(object sender, EventArgs e)
        {
            dialog = new OpenFileDialog();
            dialog.InitialDirectory = "c:\\";
            dialog.Filter = "(*.txt)|*.txt";
            dialog.FilterIndex = 2;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.myStream = dialog.OpenFile();
                    if (myStream != null)
                    {
                        initializeBoardByFile();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Originial error: " + ex.Message);
                }
            }
        }
        #endregion

        //these are all the methods that add/remove the rules
        //in the rulesInAffect list
        //(#####)
        //self - left Neighbor - bottom Neighbor - right Neighbor - top Neighbor
        #region These methods handle the rules and there check states!
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox1.Checked == true)
                  rulesInAffect.Add(00000);
             else
                  rulesInAffect.Remove(00000);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox2.Checked == true)
                  rulesInAffect.Add(00001);
             else
                  rulesInAffect.Remove(00001);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox3.Checked == true)
                  rulesInAffect.Add(00010);
             else
                  rulesInAffect.Remove(00010);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox4.Checked == true)
                  rulesInAffect.Add(00011);
             else
                  rulesInAffect.Remove(00011);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
               rulesInAffect.Add(00100);
            else
                rulesInAffect.Remove(00100);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox6.Checked == true)
                  rulesInAffect.Add(00101);
             else
                  rulesInAffect.Remove(00101);
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox7.Checked == true)
                  rulesInAffect.Add(00110);

             else
                  this.rulesInAffect.Remove(00110);
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox8.Checked == true)
                  this.rulesInAffect.Add(00111);
             else
                  this.rulesInAffect.Remove(00111);
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox9.Checked == true)
                  this.rulesInAffect.Add(01000);
             else
                  this.rulesInAffect.Remove(01000);
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox10.Checked == true)
                   this.rulesInAffect.Add(01001);
            else
                this.rulesInAffect.Remove(01001);
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox11.Checked == true)
                  this.rulesInAffect.Add(01010);
             else
                  this.rulesInAffect.Remove(01010);
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox12.Checked == true)
                    this.rulesInAffect.Add(01011);
            else
                this.rulesInAffect.Remove(01011);
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox13.Checked == true)
                  this.rulesInAffect.Add(01100);
             else
                  this.rulesInAffect.Remove(01100);
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox14.Checked == true)
                  this.rulesInAffect.Add(01101);
             else
                  this.rulesInAffect.Remove(01101);
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox15.Checked == true)
                  this.rulesInAffect.Add(01110);
             else
                  this.rulesInAffect.Remove(01110);
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox16.Checked == true)
                    this.rulesInAffect.Add(01111);
            else
                this.rulesInAffect.Remove(01111);
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox17.Checked == true)
                  this.rulesInAffect.Add(10000);
             else
                  this.rulesInAffect.Remove(10000);
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox18.Checked == true)
                    this.rulesInAffect.Add(10001);
            else
                this.rulesInAffect.Remove(10001);
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox19.Checked == true)
                    this.rulesInAffect.Add(10010);
            else
                this.rulesInAffect.Remove(10010);
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox20.Checked == true)
                  this.rulesInAffect.Add(10011);
             else
                  this.rulesInAffect.Remove(10011);
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox21.Checked == true)
                    this.rulesInAffect.Add(10100);
            else
                this.rulesInAffect.Remove(10100);
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox22.Checked == true)
                  this.rulesInAffect.Add(10101);
             else
                  this.rulesInAffect.Remove(10101);
        }

        private void checkBox23_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox23.Checked == true)
                    this.rulesInAffect.Add(10110);
            else
                this.rulesInAffect.Remove(10110);
        }

        private void checkBox24_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox24.Checked == true)
                    this.rulesInAffect.Add(10111);
            else
                 this.rulesInAffect.Remove(10111);
        }

        private void checkBox25_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox25.Checked == true)
                    this.rulesInAffect.Add(11000);
            else
                this.rulesInAffect.Remove(11000);
        }

        private void checkBox26_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox26.Checked == true)
                    this.rulesInAffect.Add(11001);
            else
                this.rulesInAffect.Remove(11001);
        }

        private void checkBox27_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox27.Checked == true)
                    this.rulesInAffect.Add(11010);
            else
                this.rulesInAffect.Remove(11010);
        }

        private void checkBox28_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox28.Checked == true)
                    this.rulesInAffect.Add(11011);
            else
                this.rulesInAffect.Remove(11011);
        }

        private void checkBox29_CheckedChanged(object sender, EventArgs e)
        {
             if (checkBox29.Checked == true)
                  this.rulesInAffect.Add(11100);
             else
                  this.rulesInAffect.Remove(11100);
        }

        private void checkBox30_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox30.Checked == true)
                    this.rulesInAffect.Add(11101);
            else
                 this.rulesInAffect.Remove(11101);
        }

        private void checkBox31_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox31.Checked == true)
                    this.rulesInAffect.Add(11110);
            else
                this.rulesInAffect.Remove(11110);
        }

        private void checkBox32_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox32.Checked == true)
                    this.rulesInAffect.Add(11111);
            else
                this.rulesInAffect.Remove(11111);
        }
        #endregion
    }
}
