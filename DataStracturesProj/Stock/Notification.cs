using ConsoleTables;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock
{
    internal class Notification : INotification
    {
        public void BoxCellDelete(double width, double height)
        {
            Console.WriteLine($"The Box cell is Deleted From The Storage {width} X {height}"); ;
        }


        public void BuyFail()
        {
            Console.WriteLine("Buy Have Been Failed");
        }

        public bool BuyList(List<BoxView> list)
        {
            if (list.Count == 0)
            {
                Console.WriteLine("No Match Boxes");
                return false;
            }
            Console.WriteLine("We Got:");
            foreach (BoxView boxView in list)
                Console.WriteLine($"The Box Is : {boxView.Width} X {boxView.Height} Amount: {boxView.Amount}");

            Console.WriteLine("Do you accept? - 'y' Is Yes , any other botton  Is No");
            string key = Console.ReadLine();

            if (key == "y")
            {
                BuySuccess();
                return true;
            }
            else
            {
                BuyFail();
                return false;
            }
        }

        public void BuySuccess()
        {
            Console.WriteLine("Buy Succeeded");
        }

        public void ExpiredBox(double weight, double height)
        {
            Console.WriteLine($"The Box {weight} X {height} Have Expired");
        }

        public void MatchTimeBox(double weight, double height, TimeSpan t)
        {
            Console.WriteLine($"The Box {weight} X {height} Excist In The Stock More Than {t}");
            Console.WriteLine("-----------------------------------------------");
        }

        public void NoMatch()
        {
            Console.WriteLine("No Boxes Matched Your Request");
        }

        public void NoSupply()
        {
            Console.WriteLine("No Supply");
            Console.WriteLine("-----------------------------------------------");
        }

        public void ShowBox(double weight, double height, int amount)
        {
            var table = new ConsoleTable("Box Weight", "Box Height", "Amount");
            table.AddRow(weight, height, amount);
            table.Write();
            Console.WriteLine("-----------------------------------------------");
        }

        public void ShowSupply(List<BoxView> listSupply)
        {
            var table = new ConsoleTable("Box Weight", "Box Height", "Amount");
            foreach (var box in listSupply)
            {
                table.AddRow(box.Width, box.Height, box.Amount);
            }
            table.Write();
            Console.WriteLine("-----------------------------------------------");
        }

        public void TooMuch(double width, double height, int amount)
        {
            Console.WriteLine($"{width} X {height} BOXES GOT {amount} more than the store can handdle");
        }

        public void Wanted(double width, double height, int amount)
        {
            Console.WriteLine($"You Wanted {amount} boxes in {width} X {height} size");
        }
    }
}
