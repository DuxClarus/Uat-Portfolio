using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace L_SystemApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //testing l-system classes
            Axiom axiom = new Axiom("A");
            LSystemManager lsm = new LSystemManager(axiom);
            lsm.setGenerations(7);
            Console.WriteLine(lsm.getAxiom().getAxiomString()+ " "+ lsm.getGenerations());
            lsm.addRule("A", "AB");
            lsm.addRule("B", "A");
            foreach (Rule rule in lsm.getListOfRules())
            {
                Console.WriteLine(rule.getRule());
            }

            lsm.displayGenerations();


            //sets up gui
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
