using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    //create exceptions to handle wage and phone number
    public class BeTweenFiftyException : ApplicationException
    {
        //default constructor
        public BeTweenFiftyException()
        {
        }
        //overloaded constructor
        public BeTweenFiftyException(string message)
            : base(message) {
        }
        //overloaded constructor
        public BeTweenFiftyException(string message, Exception inner)
            : base(message, inner) {
        }
    }

    public class InvalidPhoneNumberException : ApplicationException
    {
        //default constructor
        public InvalidPhoneNumberException()
        {
        }
        //overloaded constructor
        public InvalidPhoneNumberException(string message)
            : base(message) {
        }
        //overloaded constructor
        public InvalidPhoneNumberException(string message, Exception inner)
            : base(message, inner) {
        }
    }

    class Program
    {
        enum MenuChoice { Add = 1, Edit = 2, Pay = 3, List = 4, Exit = 5 };

        static void Main(string[] args)
        {
            bool exit = false;
            MenuChoice menuChoice;
            List<Employee> employeeList = new List<Employee>();
            while (exit == false)
            {
                menuChoice = getMenuChoice();
   
                switch(menuChoice)
                {
                    case MenuChoice.Add:
                        employeeList = addEmployee(employeeList);
                        break;
                    
                    case MenuChoice.Edit:
                        employeeList = editEmployees(employeeList);
                        break;

                    case MenuChoice.Pay:
                        payEmployees(employeeList);
                        break;

                    case MenuChoice.List:
                        listEmployees(employeeList);
                        break;

                    case MenuChoice.Exit:
                        exit = true;
                        break;
                    
                    default:
                        menuChoice = getMenuChoice();
                        break;

                }
            }
        }

        static MenuChoice getMenuChoice()
        {
            MenuChoice menuChoice;
            int menuResponse = 0;
            string choice = "";
            bool quitMenu = false;

            do{
            Console.WriteLine();
            Console.WriteLine("+========MAIN MENU========+");
            Console.WriteLine("| 1) Add an employee      |");
            Console.WriteLine("| 2) Update an employee   |");
            Console.WriteLine("| 3) Pay employees        |");
            Console.WriteLine("| 4) List employees       |");
            Console.WriteLine("| 5) Exit                 |");
            Console.WriteLine("+=========================+");
            Console.WriteLine();
            Console.Write(" Choice > ");
            choice = Console.ReadLine();
            quitMenu = Int32.TryParse(choice, out menuResponse);
            } while(!quitMenu);
            menuChoice = (MenuChoice)menuResponse;
            return menuChoice;
        }

        public static List<Employee> addEmployee(List<Employee> employeeList)
        {
            List<Employee> tempList = employeeList;
            if (employeeList.Count > 50)
            {
                Console.WriteLine("Error: Maximum number of employees have been added!");
                return employeeList;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine("ADD NEW EMPLOYEE");
                Console.WriteLine("----------------------------------------------------------------------");
                Console.WriteLine();
                Employee employee = createEmployee();
                employeeList.Add(employee);
                Console.WriteLine("*** New employee added ***");
                return tempList;
            }
        }

        public static List<Employee> editEmployees(List<Employee> employeeList)
        {
            List<Employee> tempList = employeeList;
            int employeeIndex = 0;
            string response = "";
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("EDIT EMPLOYEE");
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Enter the number of the emplyee you would like to edit: ");
            response = Console.ReadLine();

            if (Int32.TryParse(response, out employeeIndex))
            {
                Employee employee = employeeList[employeeIndex - 1];
                employeeList[employeeIndex - 1] = editEmployee(employee);
                Console.WriteLine("*** Edit employee successful ***");
                return employeeList;
            }
            else
            {
                Console.WriteLine("There seems to be no entry for this users.");
                Console.WriteLine("*** Edit employee failed ***");
                return tempList;
            }
        }

        public static void payEmployees(List<Employee> employeeList)
        {
            int employeeCounter = 0;
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("PAY EMPLOYEES");
            Console.WriteLine("-----------------------------------------------------------------------");

            foreach (Employee employee in employeeList)
            {
                employeeCounter++;
                if (employee.OnPayRoll)
                {
                    Console.WriteLine("=============== Employee #{0} ==================", employeeCounter);
                    Console.WriteLine("Name: {0} Pay: ${1}", employee.FirstName + " " + employee.LastName, employee.getPay());
                }
            }
        }

        public static void listEmployees(List<Employee> employeeList)
        {
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("LIST ALL EMPLOYEES");
            Console.WriteLine("-----------------------------------------------------------------------");
            int employeeCounter = 0;
            foreach (Employee employee in employeeList)
            {
                employeeCounter++;
                Console.WriteLine("=============== Employee #{0} ==================", employeeCounter);
                Console.WriteLine(employee.ToString());
                Console.WriteLine("================================================");
            }
        }

        public static Employee createEmployee()
        {
           Employee employee = new Employee();
           double number = 0;
           string response = "";
           bool onPayRoll = false;

           Console.Write("First Name: ");
           employee.FirstName = Console.ReadLine();
           Console.Write("Last Name: ");
           employee.LastName = Console.ReadLine();

           bool trying = true;
           //Start a loop in case of an exception/error
           while (trying)
           {
               //try the below code
               try
               {
                   //Attempt to receive a phone number
                   number = getPhone(employee, false);
                   employee.PhoneNumber = number.ToString();
                   trying = false;
               }
               catch (InvalidPhoneNumberException m)
               {
                   //Throw an error because there wasn't 10 numbers
                   Console.WriteLine("{0}", m.Message);
               }
               catch (Exception)
               {
                   //Throw an error because invalid format
                   Console.WriteLine("Please enter a phone number in one of the follow formats:\n(000)-000-0000\n 0000000000\n 000-000-0000");
               }
           }

            Console.Write("Job Title: ");
            employee.JobTitle = Console.ReadLine();
            
            //Same theory as the phone number applies below
            trying = true;
            while (trying)
            {
                try
                {
                    number = getWage(employee, false);
                    employee.Wage = number;
                    trying = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a number 5-50");
                }
            }

            //Same theory applies below
            trying = true;
            while (trying)
            {
                try
                {
                    number = getHours(employee, false);
                    employee.HoursWorked = number;
                    trying = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a number 5-50");
                }
            }
                      
            Console.Write("Address: ");
            employee.Address.Street = Console.ReadLine();
            Console.Write("Appartment or House Number: ");
            employee.Address.HouseNumber = Console.ReadLine();
            Console.Write("City: ");
            employee.Address.City = Console.ReadLine();
            Console.Write("State: ");
            employee.Address.State = Console.ReadLine();
            Console.Write("ZIP Code: ");
            employee.Address.Zip = Console.ReadLine();
            Console.Write("Is this employee on the payroll? (y/n): ");
            response = Console.ReadLine();
            while (onPayRoll == false)
            {
                if (response == "y")
                {
                    employee.OnPayRoll = true;
                    onPayRoll = true;
                }
                else if (response == "n")
                {
                    employee.OnPayRoll = false;
                    onPayRoll = true;
                }
                else
                {
                    Console.Write("Is this employee on the payroll? (y/n): ");
                    response = Console.ReadLine();
                }
            }
            return employee;
        }

        public static Employee editEmployee(Employee editMe)
        {
            Employee employee = editMe;
            string response = "";
            double number = 0;
            bool onPayRoll = false;

            //ask to change information
            Console.Write("First Name [{0}] : ", employee.FirstName);
            response = Console.ReadLine();
            //If there is not resposne, use the previously declared value
            if (response != "") employee.FirstName = response;
            Console.Write("Last Name [{0}]: ", employee.LastName);
            response = Console.ReadLine();
            if (response != "") employee.LastName = response;

            bool trying = true;
            //Start a loop in case of an exception/error
            while (trying)
            {
                //try the below code
                try
                {
                    //Attempt to receive a phone number
                    number = getPhone(employee, true);
                    employee.PhoneNumber = number.ToString();
                    trying = false;
                }
                catch (InvalidPhoneNumberException m)
                {
                    //Throw an error because there wasn't 10 numbers
                    Console.WriteLine("{0}", m.Message);
                }
                catch (Exception)
                {
                    //Throw an error because invalid format
                    Console.WriteLine("Please enter a phone number in one of the follow formats:\n(000)-000-0000\n 0000000000\n 000-000-0000");
                }
            }
           
            Console.Write("Job Title [{0}]: ", employee.JobTitle);
            response = Console.ReadLine();
            if (response != "") employee.JobTitle = response;

            trying = true;
            while (trying)
            {
                try
                {
                    number = getWage(employee, true);
                    employee.Wage = number;
                    trying = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a number 5-50");
                }
            }

            //Same theory applies below
            trying = true;
            while (trying)
            {
                try
                {
                    number = getHours(employee, true);
                    employee.HoursWorked = number;
                    trying = false;
                }
                catch (Exception)
                {
                    Console.WriteLine("Please enter a number 5-50");
                }
            }

            Console.Write("Address [{0}]: ", employee.Address.Street);
            response = Console.ReadLine();
            if (response != "") employee.Address.Street = response;
            Console.Write("Appartment [{0}]: ", employee.Address.HouseNumber);
            response = Console.ReadLine();
            if (response != "") employee.Address.HouseNumber = response;
            Console.Write("City [{0}]: ", employee.Address.City);
            response = Console.ReadLine();
            if (response != "") employee.Address.City = response;
            Console.Write("State [{0}]: ", employee.Address.State);
            response = Console.ReadLine();
            if (response != "") employee.Address.State = response;
            Console.Write("ZIP [{0}]: ", employee.Address.Zip);
            response = Console.ReadLine();
            if (response != "") employee.Address.Zip = response;
            string x;
            if (employee.OnPayRoll == true) x = "y";
            else x = "n";
            Console.Write("Is this employee on the payroll? (y/n) [{0}]: ", x);
            response = Console.ReadLine();
            while (onPayRoll == false)
            {
                if (response == "")
                {
                    onPayRoll = true;
                }
                else if (response == "y")
                {
                    employee.OnPayRoll = true;
                    onPayRoll = true;
                }
                else if (response == "n")
                {
                    employee.OnPayRoll = false;
                    onPayRoll = true;
                }
                else
                {
                    Console.Write("Is this employee on the payroll? (y/n) [{0}]: ", x);
                    response = Console.ReadLine();
                }
            }
            return employee;
        }

        public static Double getWage(Employee employee, bool edit)
        {
            if (edit == true)
            {
                Console.Write("Wage [{0}]:", employee.Wage);
            }
            else
            {
                Console.Write("Wage :");
            }
            double wageResponse = double.Parse(Console.ReadLine());
            if (wageResponse < 0.0 && wageResponse > 50.0)
            {
                throw (new BeTweenFiftyException("The number must be between 1 and 50"));
            }
            else
            {
                return wageResponse;
            }

        }

        public static int getHours(Employee employee, bool edit)
        {
            if (edit == true)
            {
                Console.Write("Hours [{0}]:", employee.HoursWorked);
            }
            else
            {
                Console.Write("Hours :");
            }
            int hoursResponse = int.Parse(Console.ReadLine());
            if ((hoursResponse < 0) && (hoursResponse > 50))
            {
                throw (new BeTweenFiftyException("The number must be between 1 and 50"));
            }
            else
            {
                return hoursResponse;
            }
        }

        public static long getPhone(Employee employee, bool edit)
        {
            if (edit == true)
            {
                Console.Write("Phone Number [{0}]:", employee.PhoneNumber);
            }
            else
            {
                Console.Write("Phone Number :");
            }
            long phoneResponse = long.Parse(Console.ReadLine().Replace("-", "").Replace("(", "").Replace(")", ""));
            string tester = phoneResponse.ToString();
            if (tester.Length != 10)
            {
                throw (new InvalidPhoneNumberException("Phone numbers must be 10 numbers in length."));
            }
            else
            {
                return phoneResponse;
            }
        }
    }
}