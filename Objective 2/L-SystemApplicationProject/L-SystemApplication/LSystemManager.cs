using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace L_SystemApplication
{
    //LSystemManager manages a list of rules, the axiom, a stringbuilder, and a interger of generations
    //The default constructors instantiate the List of Rules, Axiom and Stringbuilder. 
    //Or the overloaded contructor that takes in an axiom class and uses that instead of instantiating the Axiom itself
    //Then there are basic mutators and accessors for all of the variables
    //Lastly, there is the applyRule and evolveGenerations
    //evolveGenerations takes whats in the stringbuilder and sends it into applyRule
    //applyRule will take that in as a string and break it up into characters
    //it will check the characters against eachs rule predeccessor
    //if there is a match applyRule will add the rule's successor to a tempString it returns
    class LSystemManager
    {
        private BindingList<Rule> ruleList;
        private Axiom axiom;
        private StringBuilder stringBuilder;
        private int numberOfGenerations;

        public LSystemManager()
        {
            this.axiom = new Axiom();
            this.ruleList = new BindingList<Rule>();
            this.stringBuilder = new StringBuilder();
        }

        public LSystemManager(Axiom inAxiom)
        {
            this.axiom = inAxiom;
            this.ruleList = new BindingList<Rule>();
            this.stringBuilder = new StringBuilder();
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

        public BindingList<Rule> getListOfRules()
        {
            return this.ruleList;
        }

        public StringBuilder getStringBuilder()
        {
            return stringBuilder;
        }

        public void addRule(String inPredeccessor, String inSuccessor)
        {
            Rule rule = new Rule(inPredeccessor, inSuccessor);
            this.ruleList.Add(rule);
        }

        //int ruleListCount and ruleApplied are
        //used to handle if the character doesnt match up to a rule
        //so to avoid the unmatched letter being added for each Rule
        //or not added at all
        //we have a counter that checks for the last rule
        //and a boolean value saying if a rule has been applied or not
        //so if we checked the last rule and no rule has been applied
        //add this character to the string
        public String applyRule(String inString)
        {
            String tempString = "";
            foreach (Char character in inString)
            {
                int ruleListCount = 0;
                bool ruleApplied = false;
                foreach (Rule rule in ruleList)
                {
                    ruleListCount++;
                    if (rule.getPredecessor() == character.ToString())
                    {
                        tempString += rule.getSuccessor();
                        ruleApplied = true;
                    }
                    else
                        if (ruleListCount == this.ruleList.Count && ruleApplied == false)
                            tempString += character.ToString();
                }
            }
            return tempString;
        }

        public void evolveGeneration()
        {
            if (stringBuilder.Length == 0)
                stringBuilder.Append(axiom.getAxiomString());
            else
            {
                String tempString = applyRule(stringBuilder.ToString());
                stringBuilder.Clear();
                stringBuilder.Append(tempString);
            }
        }
    }
}
