using Corvallis_Reuse_and_Recycle_Mobile_Application.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace Corvallis_Reuse_and_Recycle_Mobile_Application
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoriesView : Page
    {
        public CategoriesView()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.Loaded += PageLoaded;
        }
        internal async void PageLoaded(object sender, RoutedEventArgs e)
        {
            List<Category> categories = await DataAccess.GetCategories();

            if (categories.Count > 0)
            {
                

                foreach (Category category in categories)
                {
                    Button button = new Button();
                    button.Content = category.Name;
                    button.Tag = category.Id;
                    button.FontSize = 20;
                    button.Margin = new Thickness(10, 0, 10, 0);
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 214, 120, 20));
                    button.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 80, 119, 39));
                    button.Click += new RoutedEventHandler(ClickCategory);
                    // button.BorderThickness = new Thickness(0, 0, 0, 0);

                    Categories.Children.Add(button); 
                }
            }
            else
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Sorry, no categories available";
                textBlock.FontSize = 48;
                textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                textBlock.Margin = new Thickness(50, 0, 10, 10);
                textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                textBlock.VerticalAlignment = VerticalAlignment.Stretch;
                textBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 80, 119, 39));
                ListCategories.Children.Add(textBlock);
            }
        }

        internal void ClickCategory(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;
            string CategoryId = _button.Tag.ToString();

            Frame.Navigate(typeof(ItemsView), CategoryId);
        }
    }
}
