using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using OpenPr0gramm;
using P0st.Chat;
using P0st.Shared;
using Xamarin.Forms;

namespace P0st.CreateConversation
{
    public class CreateConversationPageViewModel: ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private bool _isBusy;
        private GetProfileInfoResponse _currentUser;

        public CreateConversationPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.SearchCommand = new Command<string>(this.OnSearch);
            this.NavigateBackCommand = new Command(this.OnNavigateBack);
            this.NavigateChatCommand = new Command<string>(this.OnNavigateChat);
        }

        private void OnNavigateChat(string username)
        {
            this.CurrentUser = null;
            this._navigationService.GoBack();
            this._navigationService.NavigateTo(nameof(ChatPage), username);
        }

        private void OnNavigateBack()
        {
            this.CurrentUser = null;
            this._navigationService.GoBack();
        }

        private async void OnSearch(string username)
        {
            if (string.IsNullOrEmpty(username) || this.IsBusy)
                return;
            
            this.IsBusy = true;
            try
            {
                var user = await ClientFactory.Client.Profile.GetInfo(username, ItemFlags.All);
                this.CurrentUser = user;
                this.IsBusy = false;
                return;
            }
            catch (Exception)
            {
                
            }
            
            this.CurrentUser = null;
            this.IsBusy = false;
        }

        public ICommand SearchCommand { get; }
        public ICommand NavigateBackCommand { get; }
        public ICommand NavigateChatCommand { get; }

        public void OnInitialized()
        {
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.Set(ref _isBusy, value);
        }

        public GetProfileInfoResponse CurrentUser
        {
            get => _currentUser;
            set => this.Set(ref _currentUser, value);
        }
    }
}