using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Person
    {
        //accessors and mutators for varaibles
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }

        //standard constructor
        public Person()
        {
            this.Address = new Address();
        }

        //overloaded constructor
        public Person(string inFirstName, string inLastName, string inPhoneNumber, Address inAddress)
        {
            FirstName = inFirstName;
            LastName = inLastName;
            PhoneNumber = inPhoneNumber;
            this.Address = inAddress;
        }

        //returns person information 
        public override string ToString()
        {
            return string.Format("{0} {1} \r\n " + Address.ToString(), FirstName, LastName);
        }
    }
}
