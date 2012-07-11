using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
//Added
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Point = System.Drawing.Point;
using System.Windows.Forms;
using System.IO;

namespace Referencerator3_0
{
    partial class Game1
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
        public void InitializeComponent(Form window)
        {
            this.components = new System.ComponentModel.Container();

            this.statusStrip = new StatusStrip();
            this.pnlRight = new Panel();
            this.tbProps = new TabControl();
            this.tbpPolys = new TabPage();
            this.tbpWeapons = new TabPage();
            this.tbpScene = new TabPage();
            this.tbpFurniture = new TabPage();
            this.tbpVechiles = new TabPage();

            this.flowLayoutPanelPolys = new FlowLayoutPanel();
            this.flowLayOutPanelWeapons = new FlowLayoutPanel();
            this.flowLayoutPanelScene = new FlowLayoutPanel();
            this.flowLayoutFurniture = new FlowLayoutPanel();
            this.flowLayoutVechiles = new FlowLayoutPanel();

            this.menuStrip1 = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.loadToolStripMenuItem = new ToolStripMenuItem();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.gridToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.tsGridSnapping = new ToolStripMenuItem();
            this.controlsToolStripMenuItem1 = new ToolStripMenuItem();
            this.pnlRight.SuspendLayout();
            this.tbProps.SuspendLayout();
            this.tbpPolys.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.window.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 653);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1227, 22);
            this.statusStrip.TabIndex = 3;
            this.statusStrip.Text = "statusStrip1";
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlRight.Controls.Add(this.tbProps);
            this.pnlRight.Dock = DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(1044, Screen.PrimaryScreen.WorkingArea.Height);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(183, 629);
            this.pnlRight.TabIndex = 4;
            // 
            // tbProps
            // 
            this.tbProps.Alignment = TabAlignment.Right;
            this.tbProps.Controls.Add(this.tbpPolys);
            this.tbProps.Controls.Add(this.tbpWeapons);
            this.tbProps.Controls.Add(this.tbpScene);
            this.tbProps.Controls.Add(this.tbpFurniture);
            this.tbProps.Controls.Add(this.tbpVechiles);
            this.tbProps.Dock = DockStyle.Right;
            this.tbProps.Location = new System.Drawing.Point(2, 0);
            this.tbProps.Multiline = true;
            this.tbProps.Name = "tbProps";
            this.tbProps.RightToLeft = RightToLeft.No;
            this.tbProps.SelectedIndex = 0;
            this.tbProps.Size = new System.Drawing.Size(181, Screen.PrimaryScreen.WorkingArea.Height);
            this.tbProps.TabIndex = 0;
            // 
            // tbpPolys
            // 
            this.tbpPolys.AutoScroll = true;
            this.tbpPolys.BackColor = System.Drawing.SystemColors.Control;
            this.tbpPolys.Controls.Add(this.flowLayoutPanelPolys);
            this.tbpPolys.Location = new System.Drawing.Point(4, Screen.PrimaryScreen.WorkingArea.Height);
            this.tbpPolys.Name = "tbpPolys";
            this.tbpPolys.Padding = new Padding(3);
            this.tbpPolys.Size = new System.Drawing.Size(154, 621);
            this.tbpPolys.TabIndex = 0;
            this.tbpPolys.Text = "Basic Polys";
            // 
            // flowLayoutPanelPolys
            // 
            this.flowLayoutPanelPolys.AutoScroll = true;
            this.flowLayoutPanelPolys.Location = new System.Drawing.Point(16, 3);
            this.flowLayoutPanelPolys.Name = "flowLayoutPanelPolys";
            this.flowLayoutPanelPolys.Size = new System.Drawing.Size(135, Screen.PrimaryScreen.WorkingArea.Height);
            this.flowLayoutPanelPolys.TabIndex = 0;
            this.flowLayoutPanelPolys.Dock = DockStyle.Right;
            // 
            // tbpWeapons
            // 
            this.tbpWeapons.AutoScroll = true;
            this.tbpWeapons.BackColor = System.Drawing.SystemColors.Control;
            this.tbpWeapons.Controls.Add(this.flowLayOutPanelWeapons);
            this.tbpWeapons.Location = new System.Drawing.Point(4, 4);
            this.tbpWeapons.Name = "tbpWeapons";
            this.tbpWeapons.Padding = new Padding(3);
            this.tbpWeapons.Size = new System.Drawing.Size(154, 621);
            this.tbpWeapons.TabIndex = 1;
            this.tbpWeapons.Text = "Weapons";
            // 
            // flowLayoutPanelWeapons
            // 
            this.flowLayOutPanelWeapons.AutoScroll = true;
            this.flowLayOutPanelWeapons.Location = new System.Drawing.Point(16, 3);
            this.flowLayOutPanelWeapons.Name = "flowLayoutPanelPolys";
            this.flowLayOutPanelWeapons.Size = new System.Drawing.Size(135, Screen.PrimaryScreen.WorkingArea.Height);
            this.flowLayOutPanelWeapons.TabIndex = 0;
            this.flowLayOutPanelWeapons.Dock = DockStyle.Right;
            // 
            // tbpScene
            // 
            this.tbpScene.AutoScroll = true;
            this.tbpScene.BackColor = System.Drawing.SystemColors.Control;
            this.tbpScene.Controls.Add(this.flowLayoutPanelScene);
            this.tbpScene.Location = new System.Drawing.Point(4, 4);
            this.tbpScene.Name = "tbpScene";
            this.tbpScene.RightToLeft = RightToLeft.No;
            this.tbpScene.Size = new System.Drawing.Size(154, 621);
            this.tbpScene.TabIndex = 2;
            this.tbpScene.Text = "Scene Assests";
            // 
            // flowLayoutPanelScene
            // 
            this.flowLayoutPanelScene.AutoScroll = true;
            this.flowLayoutPanelScene.Location = new System.Drawing.Point(16, 3);
            this.flowLayoutPanelScene.Name = "flowLayoutPanelScene";
            this.flowLayoutPanelScene.Size = new System.Drawing.Size(135, Screen.PrimaryScreen.WorkingArea.Height);
            this.flowLayoutPanelScene.TabIndex = 0;
            this.flowLayoutPanelScene.Dock = DockStyle.Right;
            // 
            // tbpFurniture
            // 
            this.tbpFurniture.AutoScroll = true;
            this.tbpFurniture.BackColor = System.Drawing.SystemColors.Control;
            this.tbpFurniture.Controls.Add(this.flowLayoutFurniture);
            this.tbpFurniture.Location = new System.Drawing.Point(4, Screen.PrimaryScreen.WorkingArea.Height);
            this.tbpFurniture.Name = "tbpFurniture";
            this.tbpFurniture.Padding = new Padding(3);
            this.tbpFurniture.Size = new System.Drawing.Size(154, 621);
            this.tbpFurniture.TabIndex = 0;
            this.tbpFurniture.Text = "Furniture";
            // 
            // flowLayoutPanelPolys
            // 
            this.flowLayoutFurniture.AutoScroll = true;
            this.flowLayoutFurniture.Location = new System.Drawing.Point(16, 3);
            this.flowLayoutFurniture.Name = "flowLayoutFurniture";
            this.flowLayoutFurniture.Size = new System.Drawing.Size(135, Screen.PrimaryScreen.WorkingArea.Height);
            this.flowLayoutFurniture.TabIndex = 0;
            this.flowLayoutFurniture.Dock = DockStyle.Right;
            // 
            // tbpVechiels
            // 
            this.tbpVechiles.AutoScroll = true;
            this.tbpVechiles.BackColor = System.Drawing.SystemColors.Control;
            this.tbpVechiles.Controls.Add(this.flowLayoutVechiles);
            this.tbpVechiles.Location = new System.Drawing.Point(4, Screen.PrimaryScreen.WorkingArea.Height);
            this.tbpVechiles.Name = "tbpVechiles";
            this.tbpVechiles.Padding = new Padding(3);
            this.tbpVechiles.Size = new System.Drawing.Size(154, 621);
            this.tbpVechiles.TabIndex = 0;
            this.tbpVechiles.Text = "Vechiles";
            // 
            // flowLayoutPanelPolys
            // 
            this.flowLayoutVechiles.AutoScroll = true;
            this.flowLayoutVechiles.Location = new System.Drawing.Point(16, 3);
            this.flowLayoutVechiles.Name = "flowLayoutVechiels";
            this.flowLayoutVechiles.Size = new System.Drawing.Size(135, Screen.PrimaryScreen.WorkingArea.Height);
            this.flowLayoutVechiles.TabIndex = 0;
            this.flowLayoutVechiles.Dock = DockStyle.Right;

            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1227, 24);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new EventHandler(saveToolStripMenuItem_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new EventHandler(loadToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.gridToolStripMenuItem,
            this.tsGridSnapping,
            this.controlsToolStripMenuItem1});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // gridToolStripMenuItem
            // 
            this.gridToolStripMenuItem.Checked = true;
            this.gridToolStripMenuItem.CheckOnClick = true;
            this.gridToolStripMenuItem.CheckState = CheckState.Checked;
            this.gridToolStripMenuItem.Name = "gridToolStripMenuItem";
            this.gridToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gridToolStripMenuItem.Text = "Grid";
            this.gridToolStripMenuItem.Click += new EventHandler(gridToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // controlsToolStripMenuItem
            // 
            this.tsGridSnapping.Name = "controlsToolStripMenuItem";
            this.tsGridSnapping.Size = new System.Drawing.Size(152, 22);
            this.tsGridSnapping.Text = "Grid Snapping";
            this.tsGridSnapping.CheckOnClick = true;
            this.tsGridSnapping.Checked = false;
            this.tsGridSnapping.Click += new EventHandler(tsGridSnapping_Click);
            // 
            // controlsToolStripMenuItem1
            // 
            this.controlsToolStripMenuItem1.Name = "controlsToolStripMenuItem1";
            this.controlsToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.controlsToolStripMenuItem1.Text = "Controls";

            window.Controls.Add(this.pnlRight);
            window.Controls.Add(this.statusStrip);
            window.Controls.Add(this.menuStrip1);

            this.pnlRight.ResumeLayout(false);
            this.tbProps.ResumeLayout(false);
            this.tbpPolys.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.window.ResumeLayout(false);
            this.window.PerformLayout();

        }
        private void LoadAllButtons()
        {
            GenerateButtons(this.flowLayoutPanelPolys, "Basic Polys");
            GenerateButtons(this.flowLayOutPanelWeapons, "Weapons");
            GenerateButtons(this.flowLayoutPanelScene, "Scene Assets");
            GenerateButtons(this.flowLayoutFurniture, "Furniture");
            GenerateButtons(this.flowLayoutVechiles, "Vechiles");
        }

        private void GenerateButtons(FlowLayoutPanel panel, string folderName)
        {
            int buttonCount = GetFileCount(folderName);
            buttons = new Button[buttonCount];

            for (int index = 0; index < buttonCount; index++)
            {
                string buttonName = GetFileName(folderName, index);
                var button = new Button();

                button.Text = buttonName;
                button.Tag = "Models\\" + folderName + "\\" + buttonName;
                
                button.Click += new EventHandler(loadObject_Click);

                button.Size = new System.Drawing.Size(100, 75);
                buttons[index] = button;
                panel.Controls.Add(button);
            }
        }
        
        private string GetFileName(string folderName, int count)
        {
            string[] files = Directory.GetFiles(@"..\..\x86\Debug\Content\Models\" + folderName);
            return Path.GetFileNameWithoutExtension(files[count]);
        }

        private int GetFileCount(string folderName)
        {
            int fileCount = 0;
            string[] files = Directory.GetFiles(@"..\..\x86\Debug\Content\Models\" + folderName);
            foreach (string file in files)
                fileCount++;

            return fileCount;
        }

        #endregion

        private StatusStrip statusStrip;

        private Panel pnlRight;
        private TabControl tbProps;
        private TabPage tbpPolys;
        private TabPage tbpWeapons;
        private TabPage tbpScene;
        private TabPage tbpFurniture;
        private TabPage tbpVechiles;

        private FlowLayoutPanel flowLayoutPanelPolys;
        private FlowLayoutPanel flowLayOutPanelWeapons;
        private FlowLayoutPanel flowLayoutPanelScene;
        private FlowLayoutPanel flowLayoutVechiles;
        private FlowLayoutPanel flowLayoutFurniture;

        private Button[] buttons;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem loadToolStripMenuItem;
        private ToolStripMenuItem gridToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem tsGridSnapping;
        private ToolStripMenuItem controlsToolStripMenuItem1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
    }
}
