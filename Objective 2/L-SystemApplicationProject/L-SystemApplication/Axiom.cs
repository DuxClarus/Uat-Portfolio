using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L_SystemApplication
{
    //Basic Axiom class that holds a string for the axiom
    //the constructors allow the user to enter a string/char/axiom to set the axiom
    //then there are standard accessor and mutators for the axiom string variable
    class Axiom
    {
        private String axiomString;

        public Axiom()
        {
        }

        public Axiom(String inAxiom)
        {
            this.axiomString = inAxiom;
        }

        public Axiom(Char inAxiom)
        {
            this.axiomString = inAxiom.ToString();
        }

        public Axiom(Axiom inAxiom)
        {
            this.axiomString = inAxiom.getAxiomString();
        }

        public String getAxiomString()
        {
            return this.axiomString;
        }

        public void setAxiomString(String inAxiom)
        {
            this.axiomString = inAxiom;
        }

        public void setAxiomString(Char inAxiom)
        {
            this.axiomString = inAxiom.ToString();
        }
    }
}
