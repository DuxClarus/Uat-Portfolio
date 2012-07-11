namespace L_SystemApplication
{
    partial class Form1
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
            this.lblAxiom = new System.Windows.Forms.Label();
            this.txtBoxAxiom = new System.Windows.Forms.TextBox();
            this.btnEnterAxiom = new System.Windows.Forms.Button();
            this.lblRule = new System.Windows.Forms.Label();
            this.txtBoxPredeccessor = new System.Windows.Forms.TextBox();
            this.lblArrow = new System.Windows.Forms.Label();
            this.txtBoxSuccessor = new System.Windows.Forms.TextBox();
            this.btnAddRule = new System.Windows.Forms.Button();
            this.lstBoxRules = new System.Windows.Forms.ListBox();
            this.lblListOfRules = new System.Windows.Forms.Label();
            this.lblLSystem = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnEnterGenerations = new System.Windows.Forms.Button();
            this.lblGenerations = new System.Windows.Forms.Label();
            this.txtBoxNumberOfGenerations = new System.Windows.Forms.TextBox();
            this.txtBoxGenerations = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblAxiom
            // 
            this.lblAxiom.AutoSize = true;
            this.lblAxiom.Location = new System.Drawing.Point(22, 17);
            this.lblAxiom.Name = "lblAxiom";
            this.lblAxiom.Size = new System.Drawing.Size(38, 13);
            this.lblAxiom.TabIndex = 0;
            this.lblAxiom.Text = "Axiom:";
            // 
            // txtBoxAxiom
            // 
            this.txtBoxAxiom.Location = new System.Drawing.Point(67, 17);
            this.txtBoxAxiom.Name = "txtBoxAxiom";
            this.txtBoxAxiom.Size = new System.Drawing.Size(100, 20);
            this.txtBoxAxiom.TabIndex = 1;
            // 
            // btnEnterAxiom
            // 
            this.btnEnterAxiom.Location = new System.Drawing.Point(173, 17);
            this.btnEnterAxiom.Name = "btnEnterAxiom";
            this.btnEnterAxiom.Size = new System.Drawing.Size(75, 23);
            this.btnEnterAxiom.TabIndex = 2;
            this.btnEnterAxiom.Text = "Enter Axiom";
            this.btnEnterAxiom.UseVisualStyleBackColor = true;
            this.btnEnterAxiom.Click += new System.EventHandler(this.btnEnterAxiom_Click);
            // 
            // lblRule
            // 
            this.lblRule.AutoSize = true;
            this.lblRule.Location = new System.Drawing.Point(22, 44);
            this.lblRule.Name = "lblRule";
            this.lblRule.Size = new System.Drawing.Size(32, 13);
            this.lblRule.TabIndex = 3;
            this.lblRule.Text = "Rule:";
            // 
            // txtBoxPredeccessor
            // 
            this.txtBoxPredeccessor.Location = new System.Drawing.Point(67, 44);
            this.txtBoxPredeccessor.Name = "txtBoxPredeccessor";
            this.txtBoxPredeccessor.Size = new System.Drawing.Size(100, 20);
            this.txtBoxPredeccessor.TabIndex = 4;
            // 
            // lblArrow
            // 
            this.lblArrow.AutoSize = true;
            this.lblArrow.Location = new System.Drawing.Point(170, 44);
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Size = new System.Drawing.Size(16, 13);
            this.lblArrow.TabIndex = 5;
            this.lblArrow.Text = "->";
            // 
            // txtBoxSuccessor
            // 
            this.txtBoxSuccessor.Location = new System.Drawing.Point(192, 44);
            this.txtBoxSuccessor.Name = "txtBoxSuccessor";
            this.txtBoxSuccessor.Size = new System.Drawing.Size(100, 20);
            this.txtBoxSuccessor.TabIndex = 6;
            // 
            // btnAddRule
            // 
            this.btnAddRule.Location = new System.Drawing.Point(298, 44);
            this.btnAddRule.Name = "btnAddRule";
            this.btnAddRule.Size = new System.Drawing.Size(75, 23);
            this.btnAddRule.TabIndex = 7;
            this.btnAddRule.Text = "Add Rule";
            this.btnAddRule.UseVisualStyleBackColor = true;
            this.btnAddRule.Click += new System.EventHandler(this.btnAddRule_Click);
            // 
            // lstBoxRules
            // 
            this.lstBoxRules.FormattingEnabled = true;
            this.lstBoxRules.Location = new System.Drawing.Point(25, 131);
            this.lstBoxRules.Name = "lstBoxRules";
            this.lstBoxRules.Size = new System.Drawing.Size(173, 225);
            this.lstBoxRules.TabIndex = 8;
            // 
            // lblListOfRules
            // 
            this.lblListOfRules.AutoSize = true;
            this.lblListOfRules.Location = new System.Drawing.Point(22, 115);
            this.lblListOfRules.Name = "lblListOfRules";
            this.lblListOfRules.Size = new System.Drawing.Size(80, 13);
            this.lblListOfRules.TabIndex = 9;
            this.lblListOfRules.Text = "Your Rules are:";
            // 
            // lblLSystem
            // 
            this.lblLSystem.AutoSize = true;
            this.lblLSystem.Location = new System.Drawing.Point(220, 115);
            this.lblLSystem.Name = "lblLSystem";
            this.lblLSystem.Size = new System.Drawing.Size(81, 13);
            this.lblLSystem.TabIndex = 11;
            this.lblLSystem.Text = "Your L-System: ";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(298, 105);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 12;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnEnterGenerations
            // 
            this.btnEnterGenerations.Location = new System.Drawing.Point(254, 72);
            this.btnEnterGenerations.Name = "btnEnterGenerations";
            this.btnEnterGenerations.Size = new System.Drawing.Size(91, 23);
            this.btnEnterGenerations.TabIndex = 13;
            this.btnEnterGenerations.Text = "Set Generations";
            this.btnEnterGenerations.UseVisualStyleBackColor = true;
            this.btnEnterGenerations.Click += new System.EventHandler(this.btnEnterGenerations_Click);
            // 
            // lblGenerations
            // 
            this.lblGenerations.AutoSize = true;
            this.lblGenerations.Location = new System.Drawing.Point(22, 72);
            this.lblGenerations.Name = "lblGenerations";
            this.lblGenerations.Size = new System.Drawing.Size(124, 13);
            this.lblGenerations.TabIndex = 14;
            this.lblGenerations.Text = "Number Of Generations: ";
            // 
            // txtBoxNumberOfGenerations
            // 
            this.txtBoxNumberOfGenerations.Location = new System.Drawing.Point(148, 72);
            this.txtBoxNumberOfGenerations.Name = "txtBoxNumberOfGenerations";
            this.txtBoxNumberOfGenerations.Size = new System.Drawing.Size(100, 20);
            this.txtBoxNumberOfGenerations.TabIndex = 15;
            // 
            // txtBoxGenerations
            // 
            this.txtBoxGenerations.Location = new System.Drawing.Point(223, 132);
            this.txtBoxGenerations.Multiline = true;
            this.txtBoxGenerations.Name = "txtBoxGenerations";
            this.txtBoxGenerations.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBoxGenerations.Size = new System.Drawing.Size(335, 225);
            this.txtBoxGenerations.TabIndex = 16;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 369);
            this.Controls.Add(this.txtBoxGenerations);
            this.Controls.Add(this.txtBoxNumberOfGenerations);
            this.Controls.Add(this.lblGenerations);
            this.Controls.Add(this.btnEnterGenerations);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.lblLSystem);
            this.Controls.Add(this.lblListOfRules);
            this.Controls.Add(this.lstBoxRules);
            this.Controls.Add(this.btnAddRule);
            this.Controls.Add(this.txtBoxSuccessor);
            this.Controls.Add(this.lblArrow);
            this.Controls.Add(this.txtBoxPredeccessor);
            this.Controls.Add(this.lblRule);
            this.Controls.Add(this.btnEnterAxiom);
            this.Controls.Add(this.txtBoxAxiom);
            this.Controls.Add(this.lblAxiom);
            this.Name = "Form1";
            this.Text = "L-System Application";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAxiom;
        private System.Windows.Forms.TextBox txtBoxAxiom;
        private System.Windows.Forms.Button btnEnterAxiom;
        private System.Windows.Forms.Label lblRule;
        private System.Windows.Forms.TextBox txtBoxPredeccessor;
        private System.Windows.Forms.Label lblArrow;
        private System.Windows.Forms.TextBox txtBoxSuccessor;
        private System.Windows.Forms.Button btnAddRule;
        private System.Windows.Forms.ListBox lstBoxRules;
        private System.Windows.Forms.Label lblListOfRules;
        private System.Windows.Forms.Label lblLSystem;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Button btnEnterGenerations;
        private System.Windows.Forms.Label lblGenerations;
        private System.Windows.Forms.TextBox txtBoxNumberOfGenerations;
        private System.Windows.Forms.TextBox txtBoxGenerations;
    }
}

