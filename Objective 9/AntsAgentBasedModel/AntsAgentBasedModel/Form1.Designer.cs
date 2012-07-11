namespace AntsAgentBasedModel
{
    partial class AntsWorldForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AntsWorld = new System.Windows.Forms.PictureBox();
            this.lblNumberOfAnts = new System.Windows.Forms.Label();
            this.tbNumberOfAnts = new System.Windows.Forms.TextBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.tbNumberOfFood = new System.Windows.Forms.TextBox();
            this.lblNumberOfFood = new System.Windows.Forms.Label();
            this.btnSetUpWorld = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.AntsWorld)).BeginInit();
            this.SuspendLayout();
            // 
            // AntsWorld
            // 
            this.AntsWorld.Location = new System.Drawing.Point(196, 12);
            this.AntsWorld.Name = "AntsWorld";
            this.AntsWorld.Size = new System.Drawing.Size(1094, 764);
            this.AntsWorld.TabIndex = 0;
            this.AntsWorld.TabStop = false;
            // 
            // lblNumberOfAnts
            // 
            this.lblNumberOfAnts.AutoSize = true;
            this.lblNumberOfAnts.Location = new System.Drawing.Point(3, 55);
            this.lblNumberOfAnts.Name = "lblNumberOfAnts";
            this.lblNumberOfAnts.Size = new System.Drawing.Size(164, 13);
            this.lblNumberOfAnts.TabIndex = 1;
            this.lblNumberOfAnts.Text = "Enter the number of starting Ants!";
            // 
            // tbNumberOfAnts
            // 
            this.tbNumberOfAnts.Location = new System.Drawing.Point(6, 71);
            this.tbNumberOfAnts.Name = "tbNumberOfAnts";
            this.tbNumberOfAnts.Size = new System.Drawing.Size(161, 20);
            this.tbNumberOfAnts.TabIndex = 1;
            this.tbNumberOfAnts.TextChanged += new System.EventHandler(this.tbNumberOfAnts_TextChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(6, 337);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(161, 23);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "Play with ants!";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // tbNumberOfFood
            // 
            this.tbNumberOfFood.Location = new System.Drawing.Point(6, 117);
            this.tbNumberOfFood.Name = "tbNumberOfFood";
            this.tbNumberOfFood.Size = new System.Drawing.Size(161, 20);
            this.tbNumberOfFood.TabIndex = 2;
            this.tbNumberOfFood.TextChanged += new System.EventHandler(this.tbNumberOfFood_TextChanged);
            // 
            // lblNumberOfFood
            // 
            this.lblNumberOfFood.AutoSize = true;
            this.lblNumberOfFood.Location = new System.Drawing.Point(3, 101);
            this.lblNumberOfFood.Name = "lblNumberOfFood";
            this.lblNumberOfFood.Size = new System.Drawing.Size(167, 13);
            this.lblNumberOfFood.TabIndex = 4;
            this.lblNumberOfFood.Text = "Enter the number of starting Food!";
            // 
            // btnSetUpWorld
            // 
            this.btnSetUpWorld.Location = new System.Drawing.Point(6, 308);
            this.btnSetUpWorld.Name = "btnSetUpWorld";
            this.btnSetUpWorld.Size = new System.Drawing.Size(161, 23);
            this.btnSetUpWorld.TabIndex = 3;
            this.btnSetUpWorld.Text = "Set Up the World";
            this.btnSetUpWorld.UseVisualStyleBackColor = true;
            this.btnSetUpWorld.Click += new System.EventHandler(this.btnSetUpWorld_Click);
            // 
            // AntsWorldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1302, 798);
            this.Controls.Add(this.btnSetUpWorld);
            this.Controls.Add(this.tbNumberOfFood);
            this.Controls.Add(this.lblNumberOfFood);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.tbNumberOfAnts);
            this.Controls.Add(this.lblNumberOfAnts);
            this.Controls.Add(this.AntsWorld);
            this.Name = "AntsWorldForm";
            this.Text = "Ants World App!";
            ((System.ComponentModel.ISupportInitialize)(this.AntsWorld)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox AntsWorld;
        private System.Windows.Forms.Label lblNumberOfAnts;
        private System.Windows.Forms.TextBox tbNumberOfAnts;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TextBox tbNumberOfFood;
        private System.Windows.Forms.Label lblNumberOfFood;
        private System.Windows.Forms.Button btnSetUpWorld;
    }
}

