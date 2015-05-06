using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Menu_and_Topbar_app__Windows_Phone_8._1_1.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Menu_and_Topbar_app__Windows_Phone_8._1_1
{
    public sealed partial class MainPage : ViewPage
    {
        public MainPage()
            : base()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required; //OPTIONAL
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage));
        }
    }
}
