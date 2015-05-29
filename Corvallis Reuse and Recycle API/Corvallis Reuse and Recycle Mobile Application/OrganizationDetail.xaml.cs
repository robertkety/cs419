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
using System.Reflection;
using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Corvallis_Reuse_and_Recycle_Mobile_Application
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrganizationDetail : Page
    {
        public OrganizationDetail()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            string OrganizationId = e.Parameter.ToString();
            Organization result = await DataAccess.GetOrganization(OrganizationId);
            List<string> OrgDetails = new List<string>();
            Uri uriResult;
            HyperlinkButton hyperlinkBlock = new HyperlinkButton();

            OrgDetails.Add(result.Name);

            if (result.AddressLine1 != "") OrgDetails.Add(result.AddressLine1);
            if (result.AddressLine2 != "") OrgDetails.Add(result.AddressLine2);
            if (result.AddressLine3 != "") OrgDetails.Add(result.AddressLine3);
            if (result.ZipCode != "") OrgDetails.Add(await DataAccess.GetCityState(result.ZipCode));

            if (result.Phone != "") OrgDetails.Add(String.Format("({0}) {1}-{2}", result.Phone.Substring(0, 3), result.Phone.Substring(3, 3), result.Phone.Substring(6)));
            if (result.Hours != "") OrgDetails.Add(result.Hours);
            if (result.Notes != "") OrgDetails.Add(result.Notes);
            if (result.Website != "")
            {
                if (Uri.TryCreate(result.Website, UriKind.Absolute, out uriResult))
                {
                    hyperlinkBlock.FontSize = 24;
                    hyperlinkBlock.Margin = new Thickness(50, 0, 10, 10);
                    hyperlinkBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    hyperlinkBlock.VerticalAlignment = VerticalAlignment.Center;
                    hyperlinkBlock.Content = result.Website;
                    hyperlinkBlock.NavigateUri = uriResult;
                    hyperlinkBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 80, 119, 39));
                }
                else
                    OrgDetails.Add(result.Website);
            }

            foreach (string detail in OrgDetails)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = detail;
                textBlock.FontSize = 24;
                textBlock.TextWrapping = TextWrapping.WrapWholeWords;
                textBlock.Margin = new Thickness(50, 0, 10, 10);
                textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;
                textBlock.VerticalAlignment = VerticalAlignment.Stretch;
                textBlock.Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 80, 119, 39));
                OrganizationDetails.Children.Add(textBlock);
            }

            if (hyperlinkBlock != null)
                OrganizationDetails.Children.Add(hyperlinkBlock);

        }
    }
}
