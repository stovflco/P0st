using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using OpenPr0gramm;
using P0st.ChatList;
using P0st.Extensions;
using P0st.Services;
using P0st.Shared;
using Xamarin.Forms;

namespace P0st.Login
{
    public class LoginPageViewModel : ViewModelBase
    {
	    private readonly NavigationService _navigation;
	    private readonly SettingsService _settingsService;
	    private string _username;
		private string _password;
		private ImageSource _captchaImage;
		private string _captchaToken;
		private string _captcha;
		private ICommand _loginCommand;
		private string _hint;

		public LoginPageViewModel(INavigationService navigationService, SettingsService settingsService)
		{
			_navigation = (NavigationService)navigationService;
			_settingsService = settingsService;
		}

		
		public async void OnInitialized()
		{
			await this.OnRequestCaptcha();
			this.LoginCommand = new Command(this.OnLogin);
		}

		private async Task OnRequestCaptcha()
		{
			var captcha = await ClientFactory.Client.User.RequestCaptcha();
			this.CaptchaImage = ImageSource.FromStream(() => new MemoryStream(captcha.Captcha.FromUri()));
			this.CaptchaToken = captcha.Token;
			this.Captcha = string.Empty;
		}

		private async void OnLogin()
		{
			if (string.IsNullOrEmpty(this._username))
			{
				this.Hint = "Der Benutzername darf nicht leer sein.";
				return;
			}

			if (string.IsNullOrEmpty(this._password))
			{
				this.Hint = "Das Passwort darf nicht leer sein.";
				return;
			}

			if (string.IsNullOrWhiteSpace(this._captcha))
			{
				this.Hint = "Das Captcha darf nicht leer sein.";
				return;
			}
			
			var login = await ClientFactory.Client.User.LogIn(this._username, this._password, this._captchaToken,
				this._captcha);

			if (!login.Success)
			{
				await this.OnRequestCaptcha();
				
				if (login.Ban != null && login.Ban.IsBanned)
				{
					this.Hint = $"Du bist gebannt bis {login.Ban.Until:dd.MM.yyyy HH:mm}. Grund: {login.Ban.Reason}";
					return;
				}
				this.Hint = "Login fehlgeschlagen.";
				return;
			}
			//var c = this._client.GetCookies().GetCookies(new Uri("https://pr0gramm.com/")).Cast<Cookie>();

			var cookies = ClientFactory.Client.GetCookies().GetCookies(new Uri(ClientConstants.ProtocolPrefix + ClientConstants.HostName + "/"));
			var meCookie = cookies["me"]?.Value ?? throw new Exception();
			_settingsService.Me = meCookie;
			ClientFactory.CurrentUser = await ClientFactory.Client.User.GetInfo();

			NavigateNext();
		}

		private async void NavigateNext()
        {
			//var me = await ClientFactory.Client.User.GetInfo();
			//if (me.Account == null)
			//	return;

			//var loginPage = _navigation.NavigationStack.FirstOrDefault();
			//await _navigation.PushAsync(new WallPage
			//{
			//	BindingContext = new WallViewModel(_navigation)
			//});
			//if(loginPage != null)
			//	_navigation.RemovePage(loginPage);
			_navigation.NavigateAndCleanTo(nameof(ChatListPage));
		}

		public string Hint
		{
			get => _hint;
			set => this.Set(ref _hint, value);
		}

		public string Username
		{
			get => _username;
			set => this.Set(ref _username, value);
		}

		public string Password
		{
			get => _password;
			set => this.Set(ref _password, value);
		}

		public ImageSource CaptchaImage
		{
			get => _captchaImage;
			set => this.Set(ref _captchaImage, value);
		}

		public string Captcha
		{
			get => _captcha;
			set => this.Set(ref _captcha, value);
		}

		public string CaptchaToken
		{
			get => _captchaToken;
			set => this.Set(ref _captchaToken, value);
		}

		public ICommand LoginCommand
		{
			get => _loginCommand;
			set => this.Set(ref _loginCommand, value);
		}
    }
}