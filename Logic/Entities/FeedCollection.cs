using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logic.Entities
{
    [Serializable]
    public class FeedCollection: List<Feed>    
    {
        public FeedCollection() { }

        public void AddFeed (Feed newFeed)
        {
            this.Add(newFeed);
        }

        public bool CheckForDuplicates(string myString)
        {
            bool duplicateFound = false;

            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].FeedTitle == myString)
                {
                    duplicateFound = true;
                    break;
                }
            }
            return duplicateFound;
        }
    }
}
