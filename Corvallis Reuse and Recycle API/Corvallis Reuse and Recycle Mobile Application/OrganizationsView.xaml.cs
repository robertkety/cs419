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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Corvallis_Reuse_and_Recycle_Mobile_Application
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrganizationsView : Page
    {
        public OrganizationsView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string name = e.Parameter as string;
            List<Organization> organizations = await DataAccess.GetItemOrganization(name);

            if (organizations.Count > 0)
            {
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
                ListOrganizations.Children.Add(textBlock);
            }
        }

        internal void ClickOrganization(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;
            string OrganizationId = _button.Tag.ToString();

            //Frame.Navigate(typeof(OrganizationsView), OrganizationId);
        }

        internal void ToggleMaps(object sender, RoutedEventArgs e)
        {
            //ToggleMapView
        }
    }
}
