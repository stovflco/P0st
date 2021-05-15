using GalaSoft.MvvmLight.Ioc;
using P0st.Chat;
using P0st.ChatList;
using P0st.CreateConversation;
using P0st.Login;
using P0st.Startup;

namespace P0st.Services
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<StartupPageViewModel>();
            SimpleIoc.Default.Register<LoginPageViewModel>();
            SimpleIoc.Default.Register<ChatListPageViewModel>();
            SimpleIoc.Default.Register<CreateConversationPageViewModel>();
            SimpleIoc.Default.Register<ChatPageViewModel>();
        }

        public StartupPageViewModel StartupPageViewModel => SimpleIoc.Default.GetInstance<StartupPageViewModel>();
        public LoginPageViewModel LoginPageViewModel => SimpleIoc.Default.GetInstance<LoginPageViewModel>();
        public ChatListPageViewModel ChatListPageViewModel => SimpleIoc.Default.GetInstance<ChatListPageViewModel>();
        public CreateConversationPageViewModel CreateConversationPageViewModel => SimpleIoc.Default.GetInstance<CreateConversationPageViewModel>();
        public ChatPageViewModel ChatPageViewModel => SimpleIoc.Default.GetInstance<ChatPageViewModel>();
    }
}