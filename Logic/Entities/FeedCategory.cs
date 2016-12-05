using System;
using System.Collections.Generic;

namespace Logic.Entities
{
    public class FeedCategory: Category 
    {
        public override string CategoryName { get; set; }
        public static List<FeedCategory> CategoryList { get; set; }

        public FeedCategory()
        {
        }

        public FeedCategory(string cat)
        {
            this.CategoryName = cat;
        }

        public static FeedCategory CategoryMatcher(string categoryToMatch)
        {
            var myCat = new FeedCategory();

            for (int i = 0; i < CategoryList.Count; i++)
            {
                if (CategoryList[i].CategoryName == categoryToMatch)
                {
                    myCat = CategoryList[i];
                    break;
                }
            }

            return myCat;
        }

        public static void CategoryMatcher(string categoryToMatch, out bool categoryExists)
        {
            categoryExists = false;

            for (int i = 0; i < CategoryList.Count; i++)
            {
                if (CategoryList[i].CategoryName == categoryToMatch)
                {
                    categoryExists = true; 
                    break;
                }
            }
        }


        public static List<FeedCategory> ConvertToCategoryList(List<string> strCategoryList)
        {
            List<FeedCategory> myCategoryList = new List<FeedCategory>();

                for (int i = 0; i < strCategoryList.Count; i++)
                {
                    myCategoryList.Add(new FeedCategory(strCategoryList[i]));
                }
            
            return myCategoryList;
        }


        public static List<string> ConvertToStringList ()
        {
            List<string> myStrCategoryList = new List<string>();
            for(int i = 0; i < CategoryList.Count; i++)
            {
                myStrCategoryList.Add(CategoryList[i].CategoryName);
            }

            return myStrCategoryList;
        }

        public static void AddCategory(string newCategoryToAdd)
        {
            try
            {
                CategoryList.Add(new FeedCategory(newCategoryToAdd));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public static void RemoveCategory(FeedCategory categoryToRemove)
        {
            try {
                CategoryList.Remove(categoryToRemove);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            } 
        }

        public static void FillWithDummyCategories()
        {
            if(CategoryList.Count == 0)
            {
                CategoryList.Add(new FeedCategory("Filosofi"));
                CategoryList.Add(new FeedCategory("Historia och arkeologi"));
                CategoryList.Add(new FeedCategory("Nyheter"));
                CategoryList.Add(new FeedCategory("Programmering"));
                CategoryList.Add(new FeedCategory("Politik"));
                CategoryList.Add(new FeedCategory("Psykologi"));
                CategoryList.Add(new FeedCategory("Statistik"));
            }
        } 

        public override string ToString()
        {
            return this.CategoryName;
        }

        // Demo och testmetod
        public override void PrintCategoryName()
        {
            Console.WriteLine(this.CategoryName);
        }

    }
}
