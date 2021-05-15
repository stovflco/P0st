using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P0st.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace P0st.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            this.BindingContext = App.Locator.LoginPageViewModel;
        }

        protected override void OnAppearing()
        {
            ((LoginPageViewModel)this.BindingContext).OnInitialized();
        }
    }
}