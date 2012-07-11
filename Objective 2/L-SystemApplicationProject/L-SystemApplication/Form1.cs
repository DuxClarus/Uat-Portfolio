using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace L_SystemApplication
{
    public partial class Form1 : Form
    {
        LSystemManager lsm = new LSystemManager();
        public Form1()
        {
            InitializeComponent();
            lstBoxRules.DataSource = lsm.getListOfRules();
        }

        private void btnEnterAxiom_Click(object sender, EventArgs e)
        {
            if (txtBoxAxiom.Text.Length == 1)
                lsm.getAxiom().setAxiomString(txtBoxAxiom.Text);
            else
            {
                AxiomErrorForm form = new AxiomErrorForm();
                form.Show();
            }
        }

        private void btnAddRule_Click(object sender, EventArgs e)
        {
            if (txtBoxPredeccessor.Text.Length == 1)
                lsm.addRule(txtBoxPredeccessor.Text, txtBoxSuccessor.Text);
            else
            {
                RuleErrorForm form = new RuleErrorForm();
                form.Show();
            }
        }

        private void btnEnterGenerations_Click(object sender, EventArgs e)
        {
            lsm.setGenerations(Int32.Parse(txtBoxNumberOfGenerations.Text));
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            txtBoxGenerations.Clear();
            for (int index = 0; index <= lsm.getGenerations(); index++)
            {
                lsm.evolveGeneration();
                txtBoxGenerations.AppendText(String.Format("Generation {0}: " + lsm.getStringBuilder().ToString() + "\n", index));
            }
        }
    }
}
