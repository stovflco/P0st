using System.Net;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using OpenPr0gramm;
using P0st.ChatList;
using P0st.Login;
using P0st.Services;
using P0st.Shared;

namespace P0st.Startup
{
    public class StartupPageViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private readonly SettingsService _settingsService;

        public StartupPageViewModel(INavigationService navigationService, SettingsService settingsService)
        {
            _navigationService = (NavigationService)navigationService;
            _settingsService = settingsService;
        }


        public async void OnProceed()
        {
            var isLogin = false;
            
            if (!string.IsNullOrEmpty(this._settingsService.Me))
            {
                var cookies = ClientFactory.ApiClient.GetCookies();
                cookies.Add(new Cookie("me", this._settingsService.Me, "/", ClientConstants.HostName));
                ClientFactory.CurrentUser = await ClientFactory.Client.User.GetInfo();
                this._navigationService.NavigateAndCleanTo(nameof(ChatListPage));
                return;
            }
            
            this._navigationService.NavigateAndCleanTo(nameof(LoginPage));
        }
    }
}