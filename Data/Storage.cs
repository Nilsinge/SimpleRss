using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Permissions;
using System.Net;
using System.Threading;

namespace Data
{
    public class Storage
    {
        public static void SaveCollection(Object collectionToSave)
        {
            var LogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MainCollection.xml";
            //Directory.CreateDirectory(LogFilePath);
            var xmlSave = new XmlSerializer(typeof(Object));
            //var save = File.Open(@"C:\Temp\MainCollection.xml", FileMode.Create);
            var save = File.Open(LogFilePath, FileMode.Create);
            xmlSave.Serialize(save, collectionToSave);
            save.Flush();
            save.Close();
        }

        public static Object LoadCollection()
        {
            Object loadMyObject = null;

            if (File.Exists(@"MainCollection.xml"))
            {
                var xmlLoad = new XmlSerializer(typeof(Object));
                var load = File.Open(@"MainCollection.xml", FileMode.Open);

                loadMyObject = xmlLoad.Deserialize(load) as Object;
                load.Flush();
                load.Close();
            }

            return loadMyObject;

        }

        public static void SaveWriteCategories(List<string> myCategoryList)
        {
            try
            {
                var CategoryFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Categories.xml";
                XmlWriterSettings set = new XmlWriterSettings();
                set.Indent = true;

                XmlWriter myXmlWriter = XmlWriter.Create(CategoryFilePath, set);

                myXmlWriter.WriteStartDocument();
                myXmlWriter.WriteStartElement("Kategorilista");
                myXmlWriter.WriteStartElement("Categories");

                for (int i = 0; i < myCategoryList.Count; i++)
                {
                    myXmlWriter.WriteStartElement("Category");
                    myXmlWriter.WriteString(myCategoryList[i]);
                    myXmlWriter.WriteEndElement();
                }

                myXmlWriter.WriteEndElement();
                myXmlWriter.WriteEndElement();

                myXmlWriter.Flush();
                myXmlWriter.Close();
            }
            catch (Exception e)
            { Console.WriteLine(e); }
        }


        public static List<string> LoadReadCategories()
        {
            var CategoryFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Categories.xml";

            List<string> myCategoryList = new List<string>();

            if (File.Exists(CategoryFilePath))
            {
                XmlReaderSettings set = new XmlReaderSettings();
                set.IgnoreWhitespace = true;
                set.IgnoreComments = true;

                XmlReader myXmlReader = XmlReader.Create(CategoryFilePath, set);

                if (myXmlReader.ReadToDescendant("Category"))
                {
                    do
                    {
                        myXmlReader.ReadStartElement("Category");
                        string category = myXmlReader.ReadContentAsString();
                        myCategoryList.Add(category);

                    } while (myXmlReader.ReadToNextSibling("Category"));
                }

                myXmlReader.Close();
            }
            return myCategoryList;
        }

        public static bool DownloadFile(string downLoadUrl)
        {
            bool Error;

            try {
                Uri webUrl = new Uri(downLoadUrl);
                string file = Path.GetFileName(webUrl.AbsolutePath);
                var DownloadFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string filePath = DownloadFilePath + @"\" + file;

                WebClient mediaClient = new WebClient();
                mediaClient.DownloadFile(downLoadUrl, filePath);
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception e)
            {
                Error = true;
                return Error;
            }

            Error = false;
            return Error;
        }
    }
}