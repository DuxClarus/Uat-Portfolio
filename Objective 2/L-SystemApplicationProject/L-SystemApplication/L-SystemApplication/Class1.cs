using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace L_SystemApplication
{
    public class Class1
    {
        static void Mainaa(string[] args)
        {
            Axiom axiom = new Axiom("A");
            LSystemManager lsm = new LSystemManager(axiom);
            Console.WriteLine(lsm.getAxiom());
        }
    }
}
