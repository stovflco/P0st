using System;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using OpenPr0gramm;
using P0st.Chat;
using P0st.ChatList;
using P0st.CreateConversation;
using P0st.Login;
using P0st.Services;
using P0st.Shared;
using P0st.Startup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("ionicons.otf", Alias = "ionicons")]
[assembly: ExportFont("fa5ps.otf", Alias = "fa5ps")]
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace P0st
{
    public partial class App : Application
    {  
        private static ViewModelLocator _locator;
        public static ViewModelLocator Locator => _locator ?? (_locator = new ViewModelLocator());

        public App()
        {
            InitializeComponent();
            
            this.RegisterServices();

            var navigationService = this.CreateNavigationService();
            var navigationPage = new NavigationPage()
            {
                BackgroundColor = (Color) Application.Current.Resources["BackgroundColor"],
            };
            navigationPage.Pushed += (sender, args) =>
            {
                args.Page.Style = (Style) Application.Current.Resources["PageStyle"];
            };
            navigationService.Initialize(navigationPage);
            navigationPage.PushAsync(new StartupPage());
            MainPage = navigationPage;
        }

        public void RegisterServices()
        {
            ClientFactory.ApiClient = new Pr0grammApiClient();
            ClientFactory.Client = new Pr0grammClient(ClientFactory.ApiClient);
            
            var settingsService = new SettingsService();
            SimpleIoc.Default.Register<SettingsService>(() => settingsService);
        }

        private NavigationService CreateNavigationService()
        {
            NavigationService navigationService;
            if (!SimpleIoc.Default.IsRegistered<INavigationService>())
            {
                // Setup navigation service:
                navigationService = new NavigationService();
 
                // Configure pages:
                navigationService.Configure(nameof(StartupPage), typeof(StartupPage));
                navigationService.Configure(nameof(LoginPage), typeof(LoginPage));
                navigationService.Configure(nameof(ChatListPage), typeof(ChatListPage));
                navigationService.Configure(nameof(CreateConversationPage), typeof(CreateConversationPage));
                navigationService.Configure(nameof(ChatPage), typeof(ChatPage));
 
                // Register NavigationService in IoC container:
                SimpleIoc.Default.Register<INavigationService>(() => navigationService);
            }
 
            else
                navigationService = SimpleIoc.Default.GetInstance<INavigationService>() as NavigationService;
            
            return navigationService;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}