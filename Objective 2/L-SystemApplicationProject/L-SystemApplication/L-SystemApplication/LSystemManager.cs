using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L_SystemApplication
{
    class LSystemManager
    {
        private List<Rule> ruleList;
        private Axiom axiom;
        private StringBuilder stringBuilder;
        private int numberOfGenerations;
        private bool axiomHandled;

        public LSystemManager()
        {
            this.axiom = new Axiom();
            this.ruleList = new List<Rule>();
            this.stringBuilder = new StringBuilder("", 100);
        }

        public LSystemManager(Axiom inAxiom)
        {
            this.axiom = inAxiom;
            this.ruleList = new List<Rule>();
            this.stringBuilder = new StringBuilder(axiom.getAxiomString(), 100);
        }

        public Axiom getAxiom()
        {
            return this.axiom;
        }

        public void setAxiom(String inAxiom)
        {
            this.axiom = new Axiom(inAxiom);
        }

        public int getGenerations()
        {
            return numberOfGenerations;
        }

        public void setGenerations(int inNumber)
        {
            this.numberOfGenerations = inNumber;
        }

        public List<Rule> getListOfRules()
        {
            return this.ruleList;
        }

        public void addRule(String inPredeccessor, String inSuccessor)
        {
            Rule rule = new Rule(inPredeccessor, inSuccessor);
            this.ruleList.Add(rule);
        }

        public String applyRule(Char inCharater)
        {
            foreach (Rule rule in ruleList)
            {
                if (rule.getPredecessor() == inCharater.ToString())
                    return rule.getSuccessor();
            }
            return "";
        }

        public void displayGenerations()
        {
            for (int index = 0; index <= this.numberOfGenerations; index++)
            {
                String tempString = "";
                StringBuilder tempStringBuilder = new StringBuilder(("Generation" + index +": "), 100);
                if (stringBuilder.ToString() == axiom.getAxiomString())
                {
                    tempStringBuilder.Append(axiom.getAxiomString());
                }
                else
                {
                    foreach (Char character in stringBuilder.ToString())
                    {
                        tempString = applyRule(character);
                        tempStringBuilder.Append(tempString);
                    }
                }
                this.stringBuilder.Clear();
                this.stringBuilder.Append(tempStringBuilder);
                Console.WriteLine(tempStringBuilder.ToString());
            }
        }
    }
}
