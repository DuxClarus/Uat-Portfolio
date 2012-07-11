using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L_SystemApplication
{
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

        public String getAxiomString()
        {
            return this.axiomString;
        }

        public void setAxiomString(String inAxiom)
        {
            this.axiomString = inAxiom;
        }

        public void setAxiom(Char inAxiom)
        {
            this.axiomString = inAxiom.ToString();
        }
    }
}
