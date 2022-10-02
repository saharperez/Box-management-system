using DataStracturesPrj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Logic
{
    public class Manager
    {
        private readonly BST<BoxBase> boxes;//Main Tree
        private readonly MyLinkedList<TimeData> timeList;//Main Time Data List


        Timer timer;
        TimeSpan firstCheck;
        TimeSpan checkPeriod;

        private readonly int maxAmount;//Max Amount Of Boxes In The Same Size
        private readonly int splits;//How much diffrent sizes you can buy
        INotification notification;

        public Manager(int maxAmount, int splits, INotification notification, int firstChk, int checkPrd)
        {
            boxes = new BST<BoxBase>();
            timeList = new MyLinkedList<TimeData>();
            this.maxAmount = maxAmount;
            this.splits = splits;
            this.notification = notification;
            firstCheck = new TimeSpan(0, firstChk,0);
            checkPeriod = new TimeSpan(0, checkPrd, 0);
            timer = new Timer(CheckExpiredBoxes, null, firstCheck, checkPeriod);
        }



        public void AddSupply(double weight, double height, int amount)
        {
            BoxBase tmpBase = new BoxBase(weight);
            BoxBase tmpResultBase;
            BoxHeight tmpHeight = new BoxHeight(height);
            BoxHeight tmpResultHeight;
            bool result = boxes.Search(tmpBase, out tmpResultBase);
            if (result)
            {
                result = tmpResultBase.BoxHeight.Search(tmpHeight, out tmpResultHeight);
                if (!result)// BOX DOES NOT EXCIST BASE + NO HEIGHT 
                {
                    tmpResultBase.BoxHeight.Add(tmpHeight);
                    tmpHeight.Amount += amount;
                    timeList.AddFirst(new TimeData(tmpResultBase, tmpHeight));
                    tmpHeight.TimeDataNode = timeList.ReturnFirst();
                    OverItemsCheck(tmpResultBase, tmpHeight);
                }
                else// BOX EXCIST BASE + HEIGHT - ADD AMOUNT
                {
                    tmpResultHeight.Amount += amount;
                    OverItemsCheck(tmpResultBase, tmpResultHeight);
                }

            }
            else// BOX DOES NOT EXCIST NO BASE + NO HEIGHT
            {
                boxes.Add(tmpBase);
                tmpBase.BoxHeight.Add(tmpHeight);
                tmpHeight.Amount += amount;
                timeList.AddFirst(new TimeData(tmpBase, tmpHeight));
                tmpHeight.TimeDataNode = timeList.ReturnFirst();
                OverItemsCheck(tmpBase, tmpHeight);
            }
        }
        private void OverItemsCheck(BoxBase box, BoxHeight boxHeight)
        {
            if (boxHeight.Amount > maxAmount)
            {
                notification.TooMuch(box.Width, boxHeight.Height, boxHeight.Amount - maxAmount);
                boxHeight.Amount = maxAmount;
            }
        }//Check If Too Much Boxes Have Been Added


        public void ShowAllSupply()
        {
            if (boxes.IsEmpty())
            {
                notification.NoSupply();
                return;
            }
            //StringBuilder tmpString = new StringBuilder();
            List<BoxView> listView = new List<BoxView>();
            foreach (var boxBase in boxes)
            {
                foreach (var boxHeight in boxBase.BoxHeight)
                {
                    //tmpString.AppendLine(boxBase.ToString() + boxHeight.ToString());
                    listView.Add(new BoxView(boxBase, boxHeight, false, boxHeight.Amount));//Using The Class From Buy Function
                }
            }
            notification.ShowSupply(listView);
        }

        public void ShowBox(double weight, double height)
        {
            BoxBase boxBase = new BoxBase(weight);
            BoxHeight boxHeight = new BoxHeight(height);
            bool result = boxes.Search(boxBase, out boxBase);
            if (result)
            {
                result = boxBase.BoxHeight.Search(boxHeight, out boxHeight);
                if (result) notification.ShowBox(boxBase.Width, boxHeight.Height, boxHeight.Amount);
                else notification.NoSupply();
            }
            else notification.NoSupply();
        }
        public void Buy(double width, double height, int amount = 1)
        {
            notification.Wanted(width, height, amount);
            if (boxes.IsEmpty())
            {
                notification.NoSupply();
                return;
            }
            int amountCounter = 0;
            int splitCounter = 0;
            List<BoxView> buyBoxes = new List<BoxView>();
            BoxBase tmpBase = new BoxBase(width);
            BoxBase tmpResultBase;
            BoxHeight tmpHeight = new BoxHeight(height);
            BoxHeight tmpResultHeight;
            while (amountCounter < amount && splitCounter < splits)
            {
                bool result = boxes.FindBestMatch(tmpBase, out tmpResultBase);
                if (!result) break;
                else
                {
                    result = tmpResultBase.BoxHeight.FindBestMatch(tmpHeight, out tmpResultHeight);
                    if (!result && amountCounter > 0)
                    {
                        tmpBase = new BoxBase(tmpResultBase.Width + 1);
                        tmpHeight = new BoxHeight(width);
                    }
                    else if (!result) break;
                    else
                    {
                        if (tmpResultHeight.Amount <= (amount - amountCounter))//IF THE AMOUNT OF BOXES IN STOCK FROM THE SAME SIZE SMALLER OR EQUAL TO WHAT THE CUST NEED
                        {
                            buyBoxes.Add(new BoxView(tmpResultBase, tmpResultHeight, false, tmpResultHeight.Amount));
                            amountCounter += tmpResultHeight.Amount;
                            splitCounter++;
                            tmpHeight = new BoxHeight(tmpResultHeight.Height + 1);//Adding To The Height InOrder To Get Bigger Parameter
                        }
                        else //IF THE AMOUNT OF BOXES IN STOCK BIGGER TO WHAT THE CUST NEED
                        {
                            buyBoxes.Add(new BoxView(tmpResultBase, tmpResultHeight, true, amount - amountCounter));
                            splitCounter = splits;
                        }
                    }
                }
            }
            bool answer = notification.BuyList(buyBoxes);
            if (answer)
            {
                foreach (var box in buyBoxes)
                {
                    if (!box.Flag)
                    {
                        OutOfStock(box.BoxBase, box.BoxHeight);//Removing The Items From The Trees
                        timeList.RemoveNode(box.BoxHeight.TimeDataNode);//Removing The Item From The List
                    }
                    else
                    {
                        box.BoxHeight.Amount -= box.Amount;
                        timeList.RellocateToStart(box.BoxHeight.TimeDataNode);
                    }
                }
            }
        }
        private void OutOfStock(BoxBase width, BoxHeight height)
        {

            width.BoxHeight.RemoveSingleItem(height);
            if (width.BoxHeight.IsEmpty()) boxes.RemoveSingleItem(width);
            notification.BoxCellDelete(width.Width, height.Height);
        }//Delete Last Boxes From Stock If Customer Bought Them


        private void CheckExpiredBoxes(object state)//Check If Box Stayed In The Store More Than X Amount Of Days
        {
            if (timeList.IsEmpty())
            {
                notification.NoSupply();
                return; // Check Before Entering 
            }
            foreach (var timeData in timeList)
            {
                if (timeData != null && timeData.Update.Add(new TimeSpan(0, 24, 0, 0)) < DateTime.Now)
                {
                    notification.ExpiredBox(timeData.Width, timeData.Height);
                    OutOfStock(timeData.BoxBase, timeData.BoxHeight);
                    timeList.RemoveLast();


                }
                else break;  // Got to the part where the time span is less , Can Exit The ForEach
            }
        }
        public void ChkBoxStay(TimeSpan t)//Check If Box Excist More Than T time
        {
            if (timeList.IsEmpty())
            {
                notification.NoSupply();
                notification.NoMatch();
                return;
            }
            foreach (var timeData in timeList)
            {
                if (timeData != null && timeData.Update.Add(t) < DateTime.Now)
                {
                    notification.MatchTimeBox(timeData.Width, timeData.Height, t);
                }
                else break;// Got to the part where the time span is less , Can Exit The ForEach
            }
        }


        
    }
}
