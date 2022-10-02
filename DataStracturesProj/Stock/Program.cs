using EasyConsole;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stock
{
    public class Program
    {
        static void Main(string[] args) => MainPage();
        public static void MainPage()
        {
            Console.WriteLine("Welcome To My Boxes Storage - Created On April 2022");
            Console.WriteLine("Please Choose The Max Amount Of Each Box Of The Same Size In Your Store");
            int maxAmount = ConfigurationCheck();
            Console.WriteLine("Please Choose The Max Interations Shown When Buying");
            int maxIterations = ConfigurationCheck();
            Console.WriteLine("Please Choose The Timer First Check In Minutes");
            int firstChk = ConfigurationCheck();
            Console.WriteLine("Please Choose The Timer Period Check In Minutes");
            int periodChk = ConfigurationCheck();
            Notification thing = new Notification();
            Manager myManager = new Manager(maxAmount, maxIterations, thing, firstChk, periodChk);
            double weight;
            double height;
            int amount;
            while (true)
            {
                Console.WriteLine("MainMenu Of The Storage");

                var menu = new EasyConsole.Menu()// MAIN MENU 
                .Add("Show All Supply", () =>
                {
                    Output.WriteLine("All Supply Selected");
                    ShowAllSupply(myManager);

                })//All Supply Option
                .Add("Show Box", () =>
                    {
                        Output.WriteLine("Show Box Selected");
                        Console.WriteLine("Enter The Weight Of The Box");
                        bool result = double.TryParse(Console.ReadLine(), out weight);
                        while (!result || weight <= 0)
                        {
                            Console.WriteLine("Try Again");
                            result = double.TryParse(Console.ReadLine(), out weight);
                        }
                        Console.WriteLine("Enter The Height Of The Box");
                        result = double.TryParse(Console.ReadLine(), out height);
                        while (!result || height <= 0)
                        {
                            Console.WriteLine("Try Again");
                            result = double.TryParse(Console.ReadLine(), out height);
                        }
                        ShowBox(myManager, weight, height);
                    })//Show Box Option
                .Add("Add Supply", () =>
                    {
                        Output.WriteLine("Add Supply Selected");
                        ConsoleInput(out weight, out height, out amount);
                        AddSupply(myManager, weight, height, amount);
                    })//Add Supply Option
                .Add("Buy", () =>
                {
                    Output.WriteLine("Buy Selected");
                    ConsoleInput(out weight, out height, out amount);
                    Buy(myManager, weight, height, amount);

                })//Buy Option
                .Add("Check Box Date", () =>
                {
                    Output.WriteLine("Check Box Date Selected");
                    Console.WriteLine("Enter The Amount Of Hours You Want To Display More Than");
                    int days;
                    bool result = int.TryParse(Console.ReadLine(), out days);
                    while (!result)
                    {
                        Console.WriteLine("Try Again");
                        result = int.TryParse(Console.ReadLine(), out days);
                    }
                    CheckBoxDate(myManager, days);
                })//Check If Expired Option
                .Add("Exit", () => Environment.Exit(0));
                menu.Display();
            }
        }
        private static void ConsoleInput(out double weight, out double height, out int amount)
        {
            Console.WriteLine("Enter The Weight Of The Box");
            bool result = double.TryParse(Console.ReadLine(), out weight);
            while (!result || weight <= 0)
            {
                Console.WriteLine("Try Again");
                result = double.TryParse(Console.ReadLine(), out weight);
            }
            Console.WriteLine("Enter The Height Of The Box");
            result = double.TryParse(Console.ReadLine(), out height);
            while (!result || height <= 0)
            {
                Console.WriteLine("Try Again");
                result = double.TryParse(Console.ReadLine(), out height);
            }
            Console.WriteLine("Enter The Amount Of The Box");
            result = int.TryParse(Console.ReadLine(), out amount);
            while (!result || amount <= 0)
            {
                Console.WriteLine("Try Again");
                result = int.TryParse(Console.ReadLine(), out amount);
            }
        }
        public static void Buy(Manager manager, double weight, double height, int amount) => manager.Buy(weight, height, amount);
        public static void AddSupply(Manager manager, double weight, double height, int amount) => manager.AddSupply(weight, height, amount);
        public static void ShowAllSupply(Manager manager) => manager.ShowAllSupply();
        public static void ShowBox(Manager manager, double weight, double height) => manager.ShowBox(weight, height);
        public static void CheckBoxDate(Manager manager, int hours)
        {
            manager.ChkBoxStay(new TimeSpan(hours, 0, 0));
        }
        private static int ConfigurationCheck()// The First Check Of Valid Input In The Starting Console ReadLine
        {
            int num;
            bool result = int.TryParse(Console.ReadLine(), out num);
            while(!result || num <= 0)
            {
                Console.WriteLine("Try Again");
                result = int.TryParse(Console.ReadLine(), out num);
            }
            return num;
        }
    }
}
 