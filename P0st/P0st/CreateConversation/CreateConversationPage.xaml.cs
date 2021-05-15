using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace P0st.CreateConversation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateConversationPage : ContentPage
    {
        public CreateConversationPage()
        {
            InitializeComponent();

            this.BindingContext = App.Locator.CreateConversationPageViewModel;
        }

        protected override void OnAppearing()
        {
            ((CreateConversationPageViewModel) this.BindingContext).OnInitialized();
        }
    }
}