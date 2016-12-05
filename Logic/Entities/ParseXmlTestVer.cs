using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Logic.Entities
{
    internal class ParseXmlTestVer
    {
        public static void XmlOperation(Feed myFeed)
        {
            var xmlString = Data.GetXml.getXmlInfo(); //String?
            var dom = new XmlDocument();
            dom.LoadXml(xmlString);

            // Genererar feeditems som stoppas in i param myFeed
            foreach (XmlNode item in dom.DocumentElement.SelectNodes("channel/item"))
            {
                var title = item.SelectSingleNode("title").InnerText;
                var description = item.SelectSingleNode("description").InnerText;
                //var enclosure = item.SelectSingleNode("enclosure");
                //var enclosureUrl = enclosure.Attributes["url"].Value;
                //var itemPubDate = item.SelectSingleNode("pubDate").InnerText;
                var guid = item.SelectSingleNode("guid").Value;

                FeedItem myFeedItemTest = new FeedItem(guid, title, description);
                //FeedItem myFeedItem = new FeedItem(guid, title, enclosureUrl, itemPubDate, description);
                //myFeed.addFeedItem(myFeedItem);
                myFeed.addFeedItem(myFeedItemTest);
            }

            GUI_TransferInterface.TransmitFeed = myFeed;


            //// Skapar en ny feedcollection instans som tar emot feeden 
            //FeedCollection fdCollect = new FeedCollection();
            //fdCollect.AddFeed(myFeed);
        }

        //try {
        //    var media = item.SelectSingleNode("media");
        //    mediaUrl = media.Attributes["url"].Value;
        //}
        //catch (Exception e)
        //{
        //    var enclosure = item.SelectSingleNode("enclosure");
        //    enclosureUrl = enclosure.Attributes["url"].Value;
        //}
    }
}
