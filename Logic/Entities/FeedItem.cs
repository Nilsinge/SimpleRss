using System;

namespace Logic.Entities
{
    [Serializable]
    public class FeedItem
    {
        public string Id { get; set; }
        public string ItemTitle { get; set; }
        public string MediaUrl { get; set; }
        public string PublicationDate { get; set; }
        public string Description { get; set; }
        public bool ViewStatus { get; set; }

        public FeedItem() {    }

        public FeedItem(string id, string itemTitle, string description)
        {
            this.Id = id;
            this.ItemTitle = itemTitle;
            this.Description = description;
            this.ViewStatus = false;
        }

        public FeedItem(string id, string itemTitle, string pubDate, string description)
        {
            this.Id = id;
            this.ItemTitle = itemTitle;
            this.Description = description;
            this.PublicationDate = pubDate;
            this.ViewStatus = false;
        }

        public FeedItem (string id, string itemTitle, string mediaUrl, string pubDate, string description)
        {
            this.Id = id;
            this.ItemTitle = itemTitle;
            this.MediaUrl = mediaUrl;
            this.PublicationDate = pubDate;
            this.Description = description;
            this.ViewStatus = false;
        }

        public override string ToString()
        {
            return this.ItemTitle + " " + this.PublicationDate;
        }
    }


}