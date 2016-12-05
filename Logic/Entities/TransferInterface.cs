using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using System.Xml.Serialization;
using System.IO;

namespace Logic.Entities
{

    public class TransferInterface
    {
        public static Feed TransmitFeed { get; set; }
        public static string ErrorMessage { get; set; }


        public static void UrlToDataHandler(string[] myStringArray)
        {
            string url = myStringArray[0];
            Data.GetXml.Url = url;
        }

        public static void InformationHandler(string[] myStringArray)
        {
            string name = myStringArray[1];
            string cat = myStringArray[2];
            string interval = myStringArray[3];
            var category = FeedCategory.CategoryMatcher(cat);

            int intervalInt;
            int.TryParse(interval, out intervalInt);

            InitiateFeedConstruction(name, category, intervalInt);
        }


        public static void InitiateFeedConstruction(string name, FeedCategory category, int timeInterval)
        {
            Feed newFeed = new Feed();

            newFeed.FeedTitle = name;
            newFeed.FeedCategory = category;
            newFeed.UpdateInterval = timeInterval;

            Parsing.XmlOperation(newFeed);
        }

        public static void HandleCollectionSaveRequest(FeedCollection feedCollectionToSave)
        {
            Object FeedCoObject = (Object)feedCollectionToSave;
            Storage.SaveCollection(FeedCoObject);
        }

        public static FeedCollection HandleCollectionLoadRequest()
        {
            Object feedCollectionToLoad = Storage.LoadCollection();

            FeedCollection loadCollection = (FeedCollection)feedCollectionToLoad;

            return loadCollection;
        }

        public static void UpdateChecker(Feed myFeedToUpdate)
        {
            Data.GetXml.Url = myFeedToUpdate.Url;
            Parsing.XmlOperation(myFeedToUpdate);
        }

        public static void HandleCategorySaveRequest(List<string> categoryList)
        {
            Storage.SaveWriteCategories(categoryList);
        }

        public static List<string> HandleCategoryLoadRequest()
        {
            var categoryList = Storage.LoadReadCategories();
            return categoryList;
        }


        public static bool HandleMediaDownloadRequest(string media)
        {
            var ERROR = Data.Storage.DownloadFile(media);
            var problematicERROR = ErrorHandler(ERROR);
            return problematicERROR;
        }

        public static bool ErrorHandler(bool testERROR)
        {
            bool ERROR;

            if (testERROR == true)
            {
                ERROR = true;
            }
            else
            {
                ERROR = false;
            }

            return ERROR;
        }

        public static void ErrorHandler(bool testERROR, String message)
        {
            if (testERROR == true)
            {
                ErrorMessage = message;
            }
        }

        public static FeedCollection LoadMyCollection()
        {
            FeedCollection mainCollection = null;

            try
            {
                var MyFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MainCollection.xml";
                FeedCollection loadMyObject = null;

                if (File.Exists(MyFilePath))
                {
                    var xmlLoad = new XmlSerializer(typeof(FeedCollection));
                    var load = File.Open(MyFilePath, FileMode.Open);

                    loadMyObject = xmlLoad.Deserialize(load) as FeedCollection;
                    load.Flush();
                    load.Close();
                    mainCollection = loadMyObject;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return mainCollection;
        }


        public static void SaveMyCollection(FeedCollection mainCollection)
        {
            try
            {
                var MyFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MainCollection.xml";
                var xmlSave = new XmlSerializer(typeof(FeedCollection));
                var save = File.Open(MyFilePath, FileMode.Create);
                xmlSave.Serialize(save, mainCollection);
                save.Flush();
                save.Close();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
        }
}