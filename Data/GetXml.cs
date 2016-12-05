using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


/*

     static XmlReaderSettings readSettings = new XmlReaderSettings();
     XmlReader readXml = XmlReader.Create(url, readSettings);



     public void newReadSettings ()
     {
         readSettings.IgnoreWhitespace = true;
         readSettings.IgnoreComments = true;
     }

*/
//http://www.aftonbladet.se/rss.xml

namespace Data
{
    public class GetXml
    {
        public static String Url { get; set; }
        private static String xml = "";

        public static string getXmlInfo()
        {
            using (var client = new System.Net.WebClient())
            {
                try {
                    client.Encoding = Encoding.UTF8; // förspec att encoding ska ske i utf-8
                    xml = client.DownloadString(Url);
                    return xml;
                } catch
                {
                    Console.WriteLine("Felaktig Url");
                    xml = null;
                    return xml;
                }
            }
        }
    } 
}


// dom utgår från rotnoden (RSS), behöver ej inkl den i sökvägen
// Kan använda xpath för att välja en rotnod

// selectnodes ger nodlista 
// singlenod = en träff eller null
// item blir den första grejen 
// PÅ jakt efter title
// Hämta titeln 
// itererar item och hämtar alla enskilda titlar 

/* public class WriteXml
    {
        private string path = "";

        using(XmlWriter newWriter  = XmlWriter.Create(path)){

    */








