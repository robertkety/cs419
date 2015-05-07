﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Menu_and_Topbar_app__Windows_Phone_8._1_1.Controls
{
    public class FrameContainer : ContentControl
    {
        public UIElement TopBarPanel
        {
            get { return (UIElement)GetValue(TopBarPanelProperty); }
            set { SetValue(TopBarPanelProperty, value); }
        }

        public static readonly DependencyProperty TopBarPanelProperty =
            DependencyProperty.Register("TopBarPanel", typeof(UIElement), typeof(FrameContainer), new PropertyMetadata(null));

        public UIElement MenuPanel
        {
            get { return (UIElement)GetValue(MenuPanelProperty); }
            set { SetValue(MenuPanelProperty, value); }
        }

        public static readonly DependencyProperty MenuPanelProperty =
            DependencyProperty.Register("MenuPanel", typeof(UIElement), typeof(FrameContainer), new PropertyMetadata(null));



        public bool UpdateMenuOnPageNavigated
        {
            get { return (bool)GetValue(UpdateMenuOnPageNavigatedProperty); }
            set { SetValue(UpdateMenuOnPageNavigatedProperty, value); }
        }

        public static readonly DependencyProperty UpdateMenuOnPageNavigatedProperty =
            DependencyProperty.Register("UpdateMenuOnPageNavigated", typeof(bool), typeof(FrameContainer), new PropertyMetadata(false));

        public bool UpdateTopBarOnPageNavigated
        {
            get { return (bool)GetValue(UpdateTopBarOnPageNavigatedProperty); }
            set { SetValue(UpdateTopBarOnPageNavigatedProperty, value); }
        }

        public static readonly DependencyProperty UpdateTopBarOnPageNavigatedProperty =
            DependencyProperty.Register("UpdateTopBarOnPageNavigated", typeof(bool), typeof(FrameContainer), new PropertyMetadata(false));

        public bool LockMenu { get; set; }



        private ContentControl firstContentPresenter, topBar, menu;
        private Border menuButton;
        private Frame appFrame;
        private Grid mainGrid, topBarGrid;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            firstContentPresenter = GetTemplateChild("FirstContentPresenter") as ContentControl;
            if (firstContentPresenter != null)
                appFrame = firstContentPresenter.Content as Frame;

            loadMenuAndBar(true);
            appFrame.Navigated += (sender, e) => loadMenuAndBar();

            topBar = GetTemplateChild("TopBar") as ContentControl;
            topBarGrid = GetTemplateChild("TopBarGrid") as Grid;
            menu = GetTemplateChild("Menu") as ContentControl;

            menuButton = GetTemplateChild("MenuButton") as Border;
            menuButton.Tapped += (sender, e) =>
            {
                if (isMenuOpened)
                    CloseMenu();
                else
                    OpenMenu();
            };

            mainGrid = GetTemplateChild("MainGrid") as Grid;

            var view = ApplicationView.GetForCurrentView();
            if (view != null && view.DesiredBoundsMode != ApplicationViewBoundsMode.UseCoreWindow)
                view.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);

            var statusBar = StatusBar.GetForCurrentView();
            if (statusBar != null)
            {
                statusBar.BackgroundColor = new Color() { R = 0, G = 0, B = 0 };
                statusBar.ForegroundColor = new Color() { R = 255, G = 255, B = 255 };
                statusBar.BackgroundOpacity = 0;
            }

            gestureHandler();

            DisplayInformation.GetForCurrentView().OrientationChanged += FrameContainer_OrientationChanged;
        }

        async void FrameContainer_OrientationChanged(DisplayInformation sender, object args)
        {
            if (sender.CurrentOrientation == DisplayOrientations.Portrait)
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    await statusBar.ShowAsync();
                    topBarGrid.Height = topBarGrid.ActualHeight + 25;
                }
            }
            else
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    await statusBar.HideAsync();
                    topBarGrid.Height = topBarGrid.ActualHeight - 25;
                }
            }
        }

        bool isMenuOpened = false;

        public void OpenMenu()
        {
            if (mainGrid == null || LockMenu)
                return;

            var storyboard = mainGrid.Resources["OpenMenu"] as Storyboard;
            if (storyboard == null)
                return;

            storyboard.Begin();
            isMenuOpened = true;
        }

        public void CloseMenu()
        {
            if (mainGrid == null)
                return;

            var storyboard = mainGrid.Resources["CloseMenu"] as Storyboard;
            if (storyboard == null)
                return;

            storyboard.Begin();
            isMenuOpened = false;
        }

        private void loadMenuAndBar(bool force = false)
        {
            if (UpdateMenuOnPageNavigated || force)
                MenuPanel = (((Frame)firstContentPresenter.Content).Content as ViewPage).MenuContent;
            if (UpdateTopBarOnPageNavigated || force)
                TopBarPanel = (((Frame)firstContentPresenter.Content).Content as ViewPage).TopBarContent;

        }

        private GestureRecognizer gestureRecognizer = new GestureRecognizer();

        private void gestureHandler()
        {
            gestureRecognizer.GestureSettings = Windows.UI.Input.GestureSettings.Tap | Windows.UI.Input.GestureSettings.Hold | Windows.UI.Input.GestureSettings.RightTap | Windows.UI.Input.GestureSettings.CrossSlide;

            mainGrid.PointerCanceled += OnPointerCanceled;
            mainGrid.PointerPressed += OnPointerPressed;
            mainGrid.PointerReleased += OnPointerReleased;
            mainGrid.PointerMoved += OnPointerMoved;

            CrossSlideThresholds cst = new CrossSlideThresholds();
            cst.SelectionStart = 2;
            cst.SpeedBumpStart = 3;
            cst.SpeedBumpEnd = 4;
            cst.RearrangeStart = 5;
            gestureRecognizer.CrossSlideHorizontally = true;
            gestureRecognizer.CrossSlideThresholds = cst;

            gestureRecognizer.CrossSliding += gestureRecognizer_CrossSliding;
        }

        void OnPointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs args)
        {
            this.gestureRecognizer.ProcessDownEvent(args.GetCurrentPoint(mainGrid));
            mainGrid.CapturePointer(args.Pointer);
            args.Handled = true;
        }

        void OnPointerCanceled(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs args)
        {
            this.gestureRecognizer.CompleteGesture();
            args.Handled = true;
        }

        void OnPointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs args)
        {
            this.gestureRecognizer.ProcessUpEvent(args.GetCurrentPoint(mainGrid));
            args.Handled = true;
        }

        void OnPointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs args)
        {
            this.gestureRecognizer.ProcessMoveEvents(args.GetIntermediatePoints(mainGrid));
        }

        double xpoint = -1;

        void gestureRecognizer_CrossSliding(GestureRecognizer sender, Windows.UI.Input.CrossSlidingEventArgs args)
        {
            if (args.CrossSlidingState == CrossSlidingState.Started)
                xpoint = args.Position.X;

            if (args.CrossSlidingState == CrossSlidingState.Completed)
            {
                if (xpoint == -1)
                    return;

                if (args.Position.X - xpoint > 0)
                    OpenMenu();
                else
                    CloseMenu();

                xpoint = -1;
            }
        }

    }
}
