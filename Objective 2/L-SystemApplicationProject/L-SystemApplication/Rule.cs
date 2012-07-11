using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L_SystemApplication
{
    //Basic Rule that class that holds a predecessor and successor as a string
    //the constructors allow the user to enter strings for the successor
    //and an axiom or string for the predecessor
    //then there are the basic mutators and accessors
    //and an overridden function of the ToString method to display the
    //Rule properly in the GUI's listBox
    class Rule
    {
        private String predecessor;
        private String successor;

        public Rule()
        {
        }

        public Rule(String inPredecessor, String inSuccessor)
        {
            this.predecessor = inPredecessor;
            this.successor = inSuccessor;
        }

        public Rule(Axiom inPredecessor, String inSuccessor)
        {
            this.predecessor = inPredecessor.getAxiomString();
            this.successor = inSuccessor;
        }

        public void setRule(String inPredecessor, String inSuccessor)
        {
            this.predecessor = inPredecessor;
            this.successor = inSuccessor;
        }

        public String getSuccessor()
        {
            return successor;
        }

        public String getPredecessor()
        {
            return predecessor;
        }

        public override string ToString()
        {
            return (predecessor + "->" + successor);
        }
    }
}
