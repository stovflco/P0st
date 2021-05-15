using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenPr0gramm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace P0st.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        private readonly string _username;

        //private bool _autoscroll = true;
        
        public ChatPage(string username)
        {
            _username = username;
            InitializeComponent();
            //this.MessagesControl.ChildAdded += ScrollViewOnChildAdded;

            ViewModel = App.Locator.ChatPageViewModel;
            this.BindingContext = ViewModel;
            
            //this.ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            //this.ViewModel.ScrollTo += ViewModelOnScrollTo;
        }

        private bool _isDisappeared = false;
        
        protected override void OnDisappearing()
        {
            _isDisappeared = true;
            this.ViewModel.OnDisappeared();
        }
        

        protected override void OnAppearing()
        {
            _isDisappeared = false;
            base.OnAppearing();
            ViewModel.LoadChat(this._username);
            RunScrollLoop();
        }

        private void RunScrollLoop()
        {
            Task.Run(() =>
            {
                var lastEnd = -1D;
                while (!_isDisappeared)
                {
                    Thread.Sleep(200);
                    if (!this.ViewModel.AutoScroll)
                    {
                        lastEnd = -1;
                        continue;
                    }
                    
                    var end = ScrollView.ContentSize.Height - ScrollView.Height;
                    if (end != lastEnd)
                    {
                        if (end == 0 && !this.ViewModel.AutoScroll)
                        {
                            this.ViewModel.AutoScroll = true;
                        }
                        Application.Current.Dispatcher.BeginInvokeOnMainThread(() => this.ScrollView.ScrollToAsync(0, end, false));
                    }
                    lastEnd = end;
                }
            });
        }
        

        //private async void ViewModelOnScrollTo(object sender, bool e)
        //{
        //    //await this.ScrollView.ScrollToAsync(this.MessagesControl.Children.First().LogicalChildren.Single(c => c.BindingContext == e),
        //    //    ScrollToPosition.Center, true);
//
        //    await Task.Run(() =>
        //    {
        //        Thread.Sleep(100);
        //        var start = DateTime.Now;
//
        //        while (start > DateTime.Now.AddSeconds(-5))
        //        {
        //            var end = ScrollView.ContentSize.Height - ScrollView.Height;
        //            if (!(end > 0)) continue;
        //            Application.Current.Dispatcher.BeginInvokeOnMainThread(() => this.ScrollView.ScrollToAsync(0, end, e));
        //            break;
        //        }
        //    });
        //}

        //private void ScrollViewOnChildAdded(object sender, ElementEventArgs e)
        //{
        //    if (!this.ViewModel.AutoScroll)
        //        return;
        //    
        //    this.ScrollDown();
        //}

        public ChatPageViewModel ViewModel { get; set; }
//
        //private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
        //{
        //    if (this.ViewModel.IsBusy)
        //        return;
        //    
        //    var h = ScrollView.ContentSize.Height - ScrollView.Height;
        //    var x = e.ScrollY;
        //    var d = h - x;
        //    this.ViewModel.ScrollDownVisible = !(this.ViewModel.AutoScroll = d < 30);
        //}
//
//
        //private void Button_OnClicked(object sender, EventArgs e)
        //{
        //    this.ScrollDown(true);
        //}
//
        //public void ScrollDown(bool animated = false)
        //{
        //    var end = ScrollView.ContentSize.Height - ScrollView.Height;
        //    this.ScrollView.ScrollToAsync(0, end, animated);
        //}

        //private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    if (e.PropertyName == nameof(ChatPageViewModel.AutoScroll) && this.ViewModel.AutoScroll)
        //    {
        //        if (!IsScrolledToEnd())
        //        {
        //            this.ScrollDown();
        //            return;
        //        }
        //    }
        //}
        
        private bool IsScrolledToEnd()
        {
            var h = ScrollView.ContentSize.Height - ScrollView.Height;
            var x = ScrollView.ScrollY;
            var d = h - x;
            return  d < 30;
        }
        
        private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
        {
            if (this.ScrollView.ScrollY < 20)
            {
                this.ViewModel.LoadOlder();
            }
            this.ViewModel.AutoScroll = IsScrolledToEnd();
        }
    }
}