using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Entities
{
    public static class TimeLogic
    {

        public static FeedCollection IntervalHandler(FeedCollection mainCollection)
        {
            DateTime CurrentTime = DateTime.Now;

            if (mainCollection != null && mainCollection.Count > 0)
            {
                try
                {
                    for (int i = 0; i < mainCollection.Count; i++)
                    {
                        TimeSpan timeDiff = CurrentTime - mainCollection[i].TimeLastChecked;
                        var hoursSinceCheck = timeDiff.Hours;
                        Feed feedToCheck = (Feed)mainCollection[i].DeepClone();

                        if (mainCollection[i].UpdateInterval == 24 && hoursSinceCheck >= 24 ||
                            mainCollection[i].UpdateInterval == 1 && hoursSinceCheck >= 1)
                        {
                            TransferInterface.UpdateChecker(feedToCheck);

                            if (TransferInterface.TransmitFeed.FeedItems.Count > mainCollection[i].FeedItems.Count)
                            {
                                mainCollection[i] = TransferInterface.TransmitFeed;
                                mainCollection[i].TimeLastChecked = DateTime.Now;
                            }  }  }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }             
            }
            return mainCollection;
        }


        public static string SetInterval(string selectedInterval)
        {            
            string intervalTime;

            if (selectedInterval == "Varje dag")
            {
                intervalTime = "24";
            }
            else if (selectedInterval == "Varje timme")
            {
                intervalTime = "1";

            }
            else if (selectedInterval == "Aldrig")
            {
                intervalTime = "0";

            }
            else
            {
                intervalTime = null;
                Console.Write("Fel med tidslogiken");
            }

            return intervalTime;
        }




    }
}
