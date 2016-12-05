using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Logic.Entities
{
    [Serializable]
    public class Feed : IDeepClonable
    {
        public string FeedTitle { get; set; }
        public List <FeedItem> FeedItems { get; set;  }
        public FeedCategory FeedCategory { get; set; }
        public string Url { get; set; }
        public DateTime TimeLastChecked { get; set; }
        public int UpdateInterval { get; set; }

        public Feed()
        {
            this.FeedItems = new List<FeedItem>();
            this.TimeLastChecked = DateTime.Now;
            this.UpdateInterval = 0;
        }

        public Feed(string title, FeedCategory myCat)
        {
            this.FeedTitle = title;
            this.FeedCategory = myCat;
            this.FeedItems = new List<FeedItem>();
            this.TimeLastChecked = DateTime.Now;
            this.UpdateInterval = 0;
        }

        // Enbart för kloning
        public Feed(string title, FeedCategory myCat, int updateInterval, string url)
        {
            this.FeedTitle = title;
            this.FeedCategory = myCat;
            this.FeedItems = new List<FeedItem>();
            this.UpdateInterval = updateInterval;
            this.Url = url;
        }

        public void addFeedItem(FeedItem newItem)
        {
            FeedItems.Add(newItem);
        }

        public void removeCategory (Feed selectedFeed)
        {
            this.FeedCategory = null;  
        }

        public override string ToString()
        {
            return this.FeedTitle;
        }

        public Object DeepClone()
        {
            Feed deepFeed = new Feed(this.FeedTitle, this.FeedCategory, this.UpdateInterval, this.Url);
            return deepFeed;
        }
    }
}