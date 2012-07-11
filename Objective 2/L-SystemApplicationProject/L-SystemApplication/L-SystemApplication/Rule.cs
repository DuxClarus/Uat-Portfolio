using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L_SystemApplication
{

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

        public String getRule()
        {
            return (predecessor+"->"+successor);
        }

        public String getSuccessor()
        {
            return successor;
        }

        public String getPredecessor()
        {
            return predecessor;
        }
    }
}
