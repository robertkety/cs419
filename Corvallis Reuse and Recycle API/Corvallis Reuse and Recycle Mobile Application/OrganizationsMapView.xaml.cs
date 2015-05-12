using Corvallis_Reuse_and_Recycle_Mobile_Application.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Corvallis_Reuse_and_Recycle_Mobile_Application
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrganizationsMapView : Page
    {
        public static List<Organization> organizations;

        public OrganizationsMapView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                organizations = (List<Organization>)e.Parameter;

            OrgMap.Center = new Geopoint(new BasicGeoposition() { Latitude = 44.567, Longitude = -123.279 });
            OrgMap.ZoomLevel = 12;
            OrgMap.LandmarksVisible = true;

        }

        internal void ClickOrganization(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;
            string OrganizationId = _button.Tag.ToString();
            string OrganizationName = _button.Content.ToString();

            Frame.Navigate(typeof(OrganizationDetail), OrganizationId);
        }

        internal void ToggleMaps(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrganizationsListView));
        }
    }
}
