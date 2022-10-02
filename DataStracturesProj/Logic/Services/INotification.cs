using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface INotification
    {
        void TooMuch(double width, double height, int amount); // Too much items added from the same order

        void NoMatch();
        void NoSupply();

        void ShowSupply(List<BoxView> listSupply);

        void ShowBox(double weight,double height,int amount);

        void BoxCellDelete(double width, double height);
        void Wanted(double width, double height, int amount);

        bool BuyList(List<BoxView> list);

        void BuySuccess();

        void BuyFail();

        void ExpiredBox(double weight, double height);

        void MatchTimeBox(double weight, double height, TimeSpan t);


    }
}
