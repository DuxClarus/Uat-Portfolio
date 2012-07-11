using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Address : object
    {
        //class variables
        private string houseNumber = "";
        private string street = "";
        private string city = "";
        private string state = "";
        private string zip = "";

        //default constructor
        public Address()
        {
        }

        //overloaded constructor that takes in all address properties and sets them
        public Address(string inHouseNumber, string inStreet, string inCity, string inState, string inZip)
        {
            houseNumber = inHouseNumber;
            street = inStreet;
            city = inCity;
            state = inState;
            zip = inZip;
        }

        //overridden ToString() method that returns address properties as a string
        public override string ToString()
        {
            return string.Format("{0} {1} {2}, {3} {4}", houseNumber, Street, City, State, Zip);
        }

        //standard accessors and mutators for all properties
        public string HouseNumber
        {
            get { return houseNumber; }
            set { houseNumber = value; }
        }

        public string Street
        {
            get {return street;}
            set {street = value;}
        }

        public string City
        {
            get {return city;}
            set { city = value; }
        }

        public string State
        {
            get {return state;}
            set {state = value;}
        }

        public string Zip
        {
            get { return zip; }
            set { zip = value; }
        }
    }
}