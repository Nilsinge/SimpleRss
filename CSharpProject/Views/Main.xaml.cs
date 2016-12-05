using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using Logic.Entities;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Input;
using System.Windows.Documents;

namespace CSharpProject.Views
{
    public partial class MainWindow : Window
    {
        private static FeedCollection mainCollection = new FeedCollection();

        public MainWindow()
        {
            InitializeComponent();

            /*  DispatcherTimer, uppdateringskontrollsintervall på 10 min
                Kod anpassad från MSDN */
            DispatcherTimer automationTimer = new System.Windows.Threading.DispatcherTimer();
            automationTimer.Tick += new EventHandler(automationTimer_Tick);
            automationTimer.Interval = new TimeSpan(0, 10, 0);
            automationTimer.Start();
            btn_getMedia.Visibility = Visibility.Hidden;
        }


        private void btn_addFeedItem_Click(object sender, RoutedEventArgs e)
        {
            if (! (Tests.Validation.noText(txtFeedName)         || Tests.Validation.noText(txtUrl)
                || Tests.Validation.restrictLength(txtFeedName) || Tests.Validation.restrictLongerLength(txtUrl)
                || Tests.Validation.CorrectStartString(txtUrl)  || Tests.Validation.restrictMinLength(txtFeedName) 
                || Tests.Validation.minLengthLong(txtUrl)       || Tests.Validation.ContainsLetter(txtFeedName)))
            {
                TransferUserFeedInfo();
                txtUrl.Text = "";
                txtFeedName.Text = "";
            }
        }

    
        private void TransferUserFeedInfo()
        {
            string[] feedStore = new string[4];

            String newUrl = txtUrl.Text;
            String feedName = txtFeedName.Text;
            var catSelect = cbCategory.SelectionBoxItem;
            String selectedCategory = catSelect.ToString();
            string interval = TimeLogic.SetInterval(cbInterval.Text);

            feedStore[0] = newUrl;
            feedStore[1] = feedName;
            feedStore[2] = selectedCategory;
            feedStore[3] = interval;

            TransferInterface.UrlToDataHandler(feedStore);
            TransferInterface.InformationHandler(feedStore);

            CreateUserFeed(newUrl, feedName);
        }


        private void CreateUserFeed(string newUrl, string newFeedName)
        {
            try {
                var ERROR = ErrorMessage();
                if (ERROR == false)
                {
                    bool IsDuplicate = mainCollection.CheckForDuplicates(newFeedName);

                    if (IsDuplicate != true)
                    {
                        Feed newFeedToAdd = TransferInterface.TransmitFeed;
                        newFeedToAdd.Url = newUrl;
                        mainCollection.Add(newFeedToAdd);
                        UpdateFeedList();

                        MessageBox.Show("En ny feed har skapats", "Notis:");
                    }
                    else
                    {
                        MessageBox.Show("Det finns redan en Feed med denna titel", "Notis");
                    } } }

            catch (Exception ERROR)
            {
                MessageBox.Show(ERROR.Message);
            }
        }


        private void UpdateFeedList()
        {
            try {
                if (mainCollection != null && mainCollection.Count > 0)
                    FeedList.Items.Clear();
                for (int i = 0; i < mainCollection.Count; i++)
                {
                    FeedList.Items.Add(mainCollection[i]);
                }
            } catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        private void UpdateFeedItemList()
        {
            Feed selectedFeed = (Feed)FeedList.SelectedItem;
            if (mainCollection != null && selectedFeed != null && mainCollection.Count > 0)
            {
                for (int i = 0; i < selectedFeed.FeedItems.Count; i++)
                {
                    FeedItemList.Items.Add(selectedFeed.FeedItems[i]);
                }

                var feedUrl = selectedFeed.Url;
                var feedCat = selectedFeed.FeedCategory;
                txtUrl_Change.Text = feedUrl;
                cbCategory_Change.SelectedItem = feedCat;
            }
        }


        /* Visar den valda feedens alla feeditems */
        private void FeedList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblViewedOrNot.Content = "";
            FeedItemList.Items.Clear();
            UpdateFeedItemList();
        }


        private void btnRemoveFeed_Click(object sender, RoutedEventArgs e)
        {
            if (mainCollection != null && mainCollection.Count > 0)
            {
                try {
                    mainCollection.RemoveAt(FeedList.Items.IndexOf(FeedList.SelectedItem));
                    FeedList.Items.RemoveAt(FeedList.Items.IndexOf(FeedList.SelectedItem));
                    FeedItemList.Items.Clear();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }

            } else {
                MessageBox.Show("Det finns inget att ta bort.", "Notis:");
            }
        }


        /* Visa detaljerad information om feeditems */
        private void FeedItemList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                FeedItem selectedFeedItem = (FeedItem)FeedItemList.SelectedItem;
                txtFeedItemInfo.Text = "";
                lblViewedOrNot.Content = "";

                if (FeedItemList.Items.Count > 0)
                {
                    if (selectedFeedItem.PublicationDate != null)
                    {
                        txtFeedItemInfo.Inlines.Add(new Run(selectedFeedItem.PublicationDate) { FontStyle = FontStyles.Italic });
                    }

                    txtFeedItemInfo.Inlines.Add(new LineBreak());

                    if (selectedFeedItem.ItemTitle != null)
                    {
                        txtFeedItemInfo.Inlines.Add(new Run(selectedFeedItem.ItemTitle) { FontWeight = FontWeights.Bold });
                    }

                    txtFeedItemInfo.Inlines.Add(new LineBreak());
                    txtFeedItemInfo.Inlines.Add(new LineBreak());

                    if (selectedFeedItem.Description != null)
                    {
                        txtFeedItemInfo.Inlines.Add(selectedFeedItem.Description);
                    }

                    if (selectedFeedItem.MediaUrl != null)
                    {
                        btn_getMedia.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        btn_getMedia.Visibility = Visibility.Hidden;
                    }

                    if (selectedFeedItem.MediaUrl != null)
                    {
                        if (selectedFeedItem.ViewStatus == true)
                        {
                            lblViewedOrNot.Content = "Sett";
                        }
                        else if (selectedFeedItem.ViewStatus == false)
                        {
                            lblViewedOrNot.Content = "Ej sett";
                        }
                    } } }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }


        private void clearFeedList()
        {
            FeedList.Items.Clear();
        }


        /* Skapa sublista av feeds utifrån kategorival 
        (filtrering) via Linq */
        private void cbCategory_Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortByCategory();
        }


        private void SortByCategory()
        {
            try
            {
                var selectedCat = cbCategory_Sort.SelectedItem;
                if (mainCollection != null && mainCollection.Count > 0 && selectedCat != null)
                {
                    string strSelectedCat = selectedCat.ToString();
                    if (CbxShowAll.IsChecked == false)
                    {
                        var svarSortCat = from sortFeed in mainCollection
                                          where sortFeed.FeedCategory.ToString() == strSelectedCat
                                          select sortFeed;

                        clearFeedList();

                        foreach (var sortCat in svarSortCat)
                        {
                            FeedList.Items.Add(sortCat);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

        }


        /* ÄNDRA feedinfo (url och kategori) 
        då man trycker på ändraknappen */
        private void btn_change_Click(object sender, RoutedEventArgs e)
        {
            if (!(Tests.Validation.noText(txtUrl_Change)
                || Tests.Validation.restrictLongerLength(txtUrl_Change)
                || Tests.Validation.CorrectStartString(txtUrl_Change)
                || Tests.Validation.minLengthLong(txtUrl_Change)))
            {
                try
                {
                    String changeUrl = txtUrl_Change.Text;
                    String changeCategory = cbCategory_Change.SelectedItem.ToString();

                    Feed selectedFeed = (Feed)FeedList.SelectedItem;

                    if (mainCollection != null && mainCollection.Count > 0 && selectedFeed != null)
                    {
                        selectedFeed.Url = changeUrl;
                        var realChangeCategory = FeedCategory.CategoryMatcher(changeCategory);
                        selectedFeed.FeedCategory = realChangeCategory;

                        selectedFeed.FeedItems.Clear();
                        FeedItemList.Items.Clear();
                        TransferInterface.UpdateChecker(selectedFeed);
                        selectedFeed.TimeLastChecked = DateTime.Now;
                        UpdateFeedItemList();
                        SortByCategory();

                        MessageBox.Show("Feeden har ändrats", "Notis:");
                    }
                }
                catch (Exception myException)
                {
                    MessageBox.Show(myException.Message);
                }
            }
        }


        /*  InvalidateRequerySuggested tvingar kommandohanteraren  
       att prioritera händelse . */
        private void automationTimer_Tick(object sender, EventArgs e)
        {
            if (mainCollection != null && mainCollection.Count > 0)
            {
                mainCollection = TimeLogic.IntervalHandler(mainCollection);
                CommandManager.InvalidateRequerySuggested();
                UpdateFeedItemList();
            }
        }


        private void LoadMyCollection()
        {
            var Collection = TransferInterface.LoadMyCollection();
            mainCollection = Collection;
        }


        private void SaveMyCollection()
        {
            TransferInterface.SaveMyCollection(mainCollection);
        }


        private void SaveMyCategories()
        {
            var categoryStringList = FeedCategory.ConvertToStringList();
            TransferInterface.HandleCategorySaveRequest(categoryStringList);
        }


        private void LoadMyCategories()
        {
            var categoryStringList = TransferInterface.HandleCategoryLoadRequest();
            var restoredCategoryList = FeedCategory.ConvertToCategoryList(categoryStringList);
            FeedCategory.CategoryList = restoredCategoryList;
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveMyCollection();
            SaveMyCategories();
        }


        private void Window_Initialized(object sender, EventArgs e)
        {
            LoadMyCollection();
            UpdateFeedList();
            LoadMyCategories();
            FeedCategory.FillWithDummyCategories();
            FillWithCategories();
        }


        private void FillWithCategories()
        {
            cbCategory.Items.Clear();
            cbCategory_Sort.Items.Clear();
            cbCategory_Change.Items.Clear();
            lbCategory.Items.Clear();

            FeedCategory.CategoryList.ForEach(category => cbCategory.Items.Add(category));
            FeedCategory.CategoryList.ForEach(category => cbCategory_Sort.Items.Add(category));
            FeedCategory.CategoryList.ForEach(category => cbCategory_Change.Items.Add(category));
            FeedCategory.CategoryList.ForEach(category => lbCategory.Items.Add(category));
        }


        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            if (!(Tests.Validation.noText(txtAddCategory)
                || Tests.Validation.restrictLength(txtAddCategory)
                || Tests.Validation.restrictMinLength(txtAddCategory)))
            {
                try
                {
                    var newCategory = txtAddCategory.Text;
                    bool categoryExists;
                    FeedCategory.CategoryMatcher(newCategory, out categoryExists);

                    if (categoryExists == false)
                    {
                        FeedCategory.AddCategory(newCategory);

                        txtAddCategory.Clear();
                        FillWithCategories();
                    }
                    else
                    {
                        MessageBox.Show("Kategorin finns redan", "Notis");
                    }
                }
                catch (Exception sigma)
                {
                    MessageBox.Show(sigma.Message);
                }
            }
        }


        private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (lbCategory.SelectedItem != null)
                {
                    var categoryToRemove = lbCategory.SelectedItem as FeedCategory;
                    FeedCategory.RemoveCategory(categoryToRemove);

                    FillWithCategories();

                } else {
                    MessageBox.Show("Du måste välja en kategori", "Notis");
                }
            }
            catch (Exception upsilon)
            {
                MessageBox.Show(upsilon.Message);
            }
        }


        private bool ErrorMessage()
        {
            bool ERROR = false;
            var message = TransferInterface.ErrorMessage;
            if (message != null)
            {
                MessageBox.Show(message, "Notis");
                ERROR = true;
            }

            TransferInterface.ErrorMessage = null;
            return ERROR;
        }


        private void btn_getMedia_Click(object sender, RoutedEventArgs e)
        {
            if (mainCollection != null && FeedItemList.SelectedItem != null)
            {
                FeedItem selectedFeedItem = (FeedItem)FeedItemList.SelectedItem;
                if (selectedFeedItem.MediaUrl != null && (selectedFeedItem.MediaUrl.EndsWith(".mp4") || selectedFeedItem.MediaUrl.EndsWith(".mp3")
                  || selectedFeedItem.MediaUrl.EndsWith(".mov") || selectedFeedItem.MediaUrl.EndsWith(".m4v") || selectedFeedItem.MediaUrl.EndsWith(".flv")
                  || selectedFeedItem.MediaUrl.EndsWith(".wmv")))
                {
                    string mediaUrl = selectedFeedItem.MediaUrl;
                    var ERRORcheck = TransferInterface.HandleMediaDownloadRequest(mediaUrl);

                    if (ERRORcheck == false)
                    {
                        selectedFeedItem.ViewStatus = true;
                        lblViewedOrNot.Content = "Sett";
                    }
                    else
                    {
                        MessageBox.Show("Nedladdningen funkade ej.", "Notis");
                    }
                } else
                {
                    MessageBox.Show("Programet stödjer ej filformatet", "Notis");
                }
            }
        }


        private void CbxShowAll_Checked(object sender, RoutedEventArgs e)
        {
            try {
                if (mainCollection != null && mainCollection.Count > 0)
                {
                    var svarTotalCat = from sortFeed in mainCollection
                                       select sortFeed;
                    clearFeedList();

                    foreach (var cat in svarTotalCat)
                    {
                        FeedList.Items.Add(cat);
                    }
                }
            }
            catch (Exception Error)
            {
                MessageBox.Show(Error.Message);
            }
        }


        private void CbxShowAll_Unchecked(object sender, RoutedEventArgs e)
        {
            clearFeedList();
            SortByCategory();
        }
    }
}

