using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Employee : Person
    {
        //mutators and accessors for variablese
        public bool OnPayRoll { get; set; }
        public string JobTitle { get; set; }
        public double Wage { get; set; }
        public double HoursWorked { get; set; }

        //standard constuctor
        public Employee()
        {
        }

        //overloaded constructor
        public Employee(bool inPayRoll, string inJobTitle, double inWage, double inHoursWorked,
                        string inFirstName, string inLastName, string inPhoneNumber, Address inAddress)
        {
            this.OnPayRoll = inPayRoll;
            this.JobTitle = inJobTitle;
            this.Wage = inWage;
            this.HoursWorked = inHoursWorked;
            this.FirstName = inFirstName;
            this.LastName = inLastName;
            this.PhoneNumber = inPhoneNumber;
            this.Address = inAddress;
        }

        //returns the information of the employee
        public override string ToString()
        {
            return string.Format("First Name: {0} Last Name: {1} Phone Number: {2}\r\n" +
                                  "Job Title: {3}, Wage: {4}, Hours Worked: {5}, On Pay Roll: {6}" +
                                  Address.ToString(), 
                                  FirstName, LastName, PhoneNumber, JobTitle, Wage, HoursWorked, OnPayRoll);
        }

        //returns the pay of the employee
        public double getPay()
        {
            if (OnPayRoll == true)
                return Wage * HoursWorked;
            else
                return 0;
        }
    }
}
