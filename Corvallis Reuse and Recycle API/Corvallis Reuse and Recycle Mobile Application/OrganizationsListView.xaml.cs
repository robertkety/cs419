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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Corvallis_Reuse_and_Recycle_Mobile_Application
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrganizationsListView : Page
    {
        public static List<Organization> organizations;

        public OrganizationsListView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                string name = e.Parameter as string;
                organizations = await DataAccess.GetItemOrganization(name);
            }

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
            if (OrgList.Visibility == Visibility.Visible)
            {
                OrgList.Visibility = Visibility.Collapsed;
                OrgMap.Visibility = Visibility.Visible; 
                
                MapControl map = new MapControl();
                map.Name = "Map";
                map.MapServiceToken = MapService.ServiceToken;
                map.BorderThickness = new Thickness(1);
                map.MaxHeight = map.MinHeight = 500f;
                map.Center = new Geopoint(new BasicGeoposition() { Latitude = 44.567, Longitude = -123.279 });
                map.ZoomLevel = 12;
                map.LandmarksVisible = true;

                OrgMap.Children.Add(map);

                ToggleButton.Content = "List View";
            }
            else
            {
                OrgMap.Children.Clear();
                OrgMap.Visibility = Visibility.Collapsed;
                OrgList.Visibility = Visibility.Visible;
                ToggleButton.Content = "Map View";
            }
        }
    }
}
