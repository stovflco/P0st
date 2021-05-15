using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace P0st.ChatList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatListPage : ContentPage
    {
        public ChatListPage()
        {
            InitializeComponent();
            this.ViewModel = App.Locator.ChatListPageViewModel;
            this.BindingContext = this.ViewModel;
        }

        public ChatListPageViewModel ViewModel { get; }

        protected override void OnAppearing()
        {
            this.ViewModel.Disappeared = false;
            ((ChatListPageViewModel) this.BindingContext).OnInitialized();
        }

        protected override void OnDisappearing()
        {
            this.ViewModel.Disappeared = true;
        }

        private void ScrollView_OnScrolled(object sender, ScrolledEventArgs e)
        {
            var h = ScrollView.ContentSize.Height - ScrollView.Height;
            var x = e.ScrollY;
            var d = h - x;
            if(d<50)
                ((ChatListPageViewModel) this.BindingContext).LoadMore();
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await this.DisplayAlert("BLA", "a", "b");
            var input = await this.DisplayPromptAsync("Neuen Sprach", "Gebe den Namen vom Benutzer ein.");
        }
    }
}