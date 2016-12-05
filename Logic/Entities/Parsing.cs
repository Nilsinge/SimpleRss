using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace Logic.Entities
{
    internal class Parsing
    {
        public static void XmlOperation(Feed myFeed)
        {
            var xmlString = Data.GetXml.getXmlInfo();
            var dom = new XmlDocument();

            if (xmlString != null)
            {
                try
                {
                    dom.LoadXml(xmlString);

                    foreach (XmlNode item in dom.DocumentElement.SelectNodes("channel/item"))
                    {
                        var title = item.SelectSingleNode("title").InnerText;
                        var description = item.SelectSingleNode("description").InnerText;
                        var itemPubDate = item.SelectSingleNode("pubDate").InnerText;
                        var guid = item.SelectSingleNode("guid").Value;
                        string mediaUrl = null;

                        var enclosure = item.SelectSingleNode("enclosure");
                        if (enclosure != null) { mediaUrl = enclosure.Attributes["url"].Value; }

                        var media = item.SelectSingleNode("media");
                        if (media != null) { mediaUrl = media.Attributes["url"].Value; }

                        FeedItem myFeedItem = new FeedItem(guid, title, mediaUrl, itemPubDate, description);
                        myFeed.addFeedItem(myFeedItem);
                    }

                    TransferInterface.TransmitFeed = myFeed;
                }
                catch
                {
                    string message = "URL:en måste inkludera en RSS-feed";
                    bool ERROR = true;
                    TransferInterface.ErrorHandler(ERROR, message);
                }
            }
            else
            {
                string message = "Felaktig URL!";
                bool ERROR = true;
                TransferInterface.ErrorHandler(ERROR, message);
            }
        }
    }
}
