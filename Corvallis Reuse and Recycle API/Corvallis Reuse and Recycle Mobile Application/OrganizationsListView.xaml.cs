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
using Windows.Services.Maps;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.Storage.Streams;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Corvallis_Reuse_and_Recycle_Mobile_Application
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrganizationsListView : Page
    {
        public static List<Organization> organizations;
        public static MapControl map = new MapControl();
                    
        public static Geopoint Location(double Latitude, double Longitude)
        {
            BasicGeoposition result = new BasicGeoposition();
            result.Latitude = Latitude;
            result.Longitude = Longitude;
            try
            {
                return new Geopoint(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return null;
        }

        public static Geopoint Corvallis = Location(44.5674383, -123.2783545);

        public OrganizationsListView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            OrgMap.Children.Clear();
            Organizations.Children.Clear();
            map = new MapControl();
            OrgMap.Visibility = Visibility.Collapsed;
            OrgList.Visibility = Visibility.Visible;
            ToggleButton.Content = "Map View";
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                string name = e.Parameter as string;
                organizations = await DataAccess.GetItemOrganization(name);
            }

            map.Name = "Map";
            map.MapServiceToken = MapService.ServiceToken;
            map.BorderThickness = new Thickness(1);
            map.MaxHeight = map.MinHeight = 500f;
            map.Center = Corvallis;
            map.ZoomLevel = 12;
            map.LandmarksVisible = false;

            if (organizations.Count > 0)
            {
                // Load Organizations in List Format
                foreach (Organization organization in organizations)
                {
                    Button button = new Button();
                    button.Content = organization.Name;
                    button.Tag = organization.Id;
                    button.FontSize = 20;
                    button.Margin = new Thickness(25, 5, 10, 5);
                    button.HorizontalAlignment = HorizontalAlignment.Stretch;
                    button.VerticalAlignment = VerticalAlignment.Stretch;
                    button.Click += new RoutedEventHandler(ClickOrganization);
                    Organizations.Children.Add(button);

                    string locationString = await GetLocation(organization);
                    MapIcon icon = new MapIcon();

                    icon.Location = await GetGeopoint(locationString);
                    icon.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    icon.Title = organization.Name;
                    icon.Image = GetImage(organization);
                    icon.ZIndex = 100;

                    map.MapElements.Add(icon);
                }
            }
            else
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Sorry, no organizations available";
                textBlock.FontSize = 48;
                textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                textBlock.Margin = new Thickness(50, 0, 10, 10);
                textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                textBlock.VerticalAlignment = VerticalAlignment.Stretch;
                Organizations.Children.Add(textBlock);
            }
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
            try
            {
                if (OrgList.Visibility == Visibility.Visible)
                {
                    OrgList.Visibility = Visibility.Collapsed;
                    OrgMap.Visibility = Visibility.Visible;

                    ToggleButton.Content = "List View";

                    OrgMap.Children.Add(map);
                }
                else
                {
                    OrgMap.Children.Clear();
                    OrgMap.Visibility = Visibility.Collapsed;
                    OrgList.Visibility = Visibility.Visible;
                    ToggleButton.Content = "Map View";
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
        }

        private async Task<Geopoint> GetGeopoint(string LocationName)
        {
            try
            {
                MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(LocationName, Corvallis, 1);
                if (result.Status == MapLocationFinderStatus.Success)
                    return result.Locations.FirstOrDefault().Point;
            }
            catch (Exception ex)
            {
                if (LocationName == "")
                    Debug.WriteLine("Empty String Street Address");
                else
                    Debug.WriteLine(ex);
            }
            
            return Corvallis;
        }

        private async Task<string> GetLocation(Organization org)
        {
            string locationString = "";

            if (org.AddressLine1 != "")
                locationString += org.AddressLine1 + "\n";
            if (org.AddressLine2 != "")
                locationString += org.AddressLine1 + "\n";
            if (org.AddressLine3 != "")
                locationString += org.AddressLine1 + "\n";
            if (org.ZipCode != "")
                locationString += await DataAccess.GetCityState(org.ZipCode);

            return locationString;
        }

        private IRandomAccessStreamReference GetImage(Organization org)
        {
            switch ((Enums.offering)org.Offering)
            {
                case (Enums.offering.reuse):
                    return RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map-pin-green-hi.png"));
                    //return RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/icon_map_small.gif"));
                case (Enums.offering.recycle):
                    return RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map-pin-blue-hi.png"));
                case (Enums.offering.both):
                    return RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map-pin-purple-hi.png"));
                default:
                    break;
            }

            return null;
        }
    }
}
