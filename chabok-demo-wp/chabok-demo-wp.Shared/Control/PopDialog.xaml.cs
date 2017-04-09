using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
#if WINDOWS_PHONE_APP
            using Windows.Phone.UI.Input;
#else

#endif

using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace chabok_demo_wp.Control
{
    public sealed partial class PopDialog : UserControl
    {

        public Brush BehindBackground
        {
            get { return PopupDialogGrid.Background; }
            set { PopupDialogGrid.Background = value; }
        }

        public new object Content
        {
            get { return ContentControl.Content; }
            set { ContentControl.Content = value; }
        }

        public bool IsShowOnDesign
        {
            get { return PopupDialog.IsOpen; }
            set { PopupDialog.IsOpen = value; }
        }

        private bool _isHideClickBehind;

        public bool IsHideClickBehind
        {
            get { return _isHideClickBehind; }
            set { _isHideClickBehind = value; }
        }

        private bool _isHideClickOnContentControl;
        public bool IsHideClickOnContentControl
        {
            get { return _isHideClickOnContentControl; }
            set { _isHideClickOnContentControl = value; }
        }

        public PopDialog()
        {
            this.InitializeComponent();

            SizeChanged+=OnSizeChanged;

            #region Set size to popup grid 

            var size = GetWindowSize();
            ChangeDialogSize(size);

            #endregion

            PopupDialogGrid.PointerReleased+=PopupDialogGridOnPointerReleased;

            ContentControl.PointerReleased+=ContentControlOnPointerReleased;
        }

        private void ContentControlOnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (IsHideClickOnContentControl && pointerRoutedEventArgs.OriginalSource == ContentControl)
            {
                Hide();
            }
        }

        private void PopupDialogGridOnPointerReleased(object sender, PointerRoutedEventArgs pointerRoutedEventArgs)
        {
            if (IsHideClickBehind && pointerRoutedEventArgs.OriginalSource == PopupDialogGrid)
            {
                Hide();
            }
        }

        public async void Show()
        {
            await Window.Current.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate
            {
                PopupDialog.IsOpen = true;
            });
        }

        public async void Hide()
        {
            await Window.Current.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate
            {
                PopupDialog.IsOpen = false;
            });
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            ChangeDialogSize(sizeChangedEventArgs.NewSize);
        }

        private void ChangeDialogSize(Size size)
        {

            double height = size.Height;
            double width = size.Width;

            PopupDialogGrid.Width = width;
            PopupDialogGrid.Height = height;
        }

        private Size GetWindowSize()
        {
            var size = new Size()
            {
                Width = Window.Current.Bounds.Width,
                Height = Window.Current.Bounds.Height
            };

            return size;
        }
    }
}
