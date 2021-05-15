using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace P0st.Startup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupPage : ContentPage
    {
        public StartupPage()
        {
            InitializeComponent();
            BindingContext = App.Locator.StartupPageViewModel;
            this.Appearing += OnAppearing;
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            var viewModel = (StartupPageViewModel) this.BindingContext;
            viewModel.OnProceed();
        }
    }
}