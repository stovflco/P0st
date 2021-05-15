using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using OpenPr0gramm;
using P0st.Chat;
using P0st.CreateConversation;
using P0st.Services;
using P0st.Shared;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;

namespace P0st.ChatList
{
    public class ChatListPageViewModel : ViewModelBase
    {
        private readonly NavigationService _navigationService;
        private ObservableCollection<ConversationUser> _users;
        private bool _isBusy = true;
        private bool _hasMore = false;
        private ISimpleAudioPlayer _knock = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer(); 

        public ChatListPageViewModel(INavigationService navigationService)
        {
            _navigationService = (NavigationService) navigationService;
            _users = new ObservableCollection<ConversationUser>();
            CreateConversationCommand = new Command(OnCreateConversation);
            OpenChatCommand = new Command<string>(OnOpenChat);
            var audioStream = GetType().Assembly.GetManifestResourceStream("P0st.knock.mp3");
            this._knock.Load(audioStream);
            
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Disappeared))
            {
                if (this.Disappeared)
                {
                    //this.Clear();
                }
            }
        }

        private void OnOpenChat(string username)
        {
            this._navigationService.NavigateTo(nameof(ChatPage), username);
        }

        private bool _initialized;

        public void OnInitialized()
        {
            if (_initialized)
                return;
            _initialized = true;

            Task.Run(async () =>
                {
                    //var messages = await ClientFactory.Client.Inbox.GetAllMessages();
                    var conversations = await ClientFactory.Client.Inbox.GetConversations();
                    _hasMore = !conversations.AtEnd;
                    //var um = await ClientFactory.Client.Inbox.GetUnreadMessages();

                    Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        this.Users = new ObservableCollection<ConversationUser>(conversations.Conversations);
                        this.IsBusy = false;
                    });
                })
                .ContinueWith((t) => RunConversationLoop());
        }

        private void RunConversationLoop()
        {
            Task.Run(async () =>
            {
                //var l = 0;
                while (this.Disappeared)
                {
                    //l++;
                    Thread.Sleep(2000);
                    var response = await ClientFactory.Client.Inbox.GetConversations();
                    this._hasMore = !response.AtEnd;
                    var knownConversations = this._users.ToList();
                    var c = response.Conversations.ToList();
                    //if (l == 10)
                    //{
                    //    c.Add(new ConversationUser
                    //    {
                    //        Mark = UserMark.Fliesentisch,
                    //        Name = "BLAAA",
                    //        UnreadCount = 1,
                    //        LastMessageAt = DateTime.Now,
                    //        Blocked = false
                    //    });
                    //}
                    foreach (var conversation in c)
                    {
                        var known = knownConversations.FirstOrDefault(m => m.Name == conversation.Name);
                        if (known != null)
                        {
                            known.Blocked = conversation.Blocked;
                            known.Mark = conversation.Mark;
                            known.UnreadCount = conversation.UnreadCount;
                            known.LastMessageAt = conversation.LastMessageAt;
                            continue;
                        }
                        
                        knownConversations.Add(conversation);
                    }
                    
                    
                    Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        var i = -1;
                        var c = _users.Count;
                        foreach (var conversation in knownConversations.OrderByDescending(c => c.LastMessageAt))
                        {
                            i++;
                            if (!this._users.Contains(conversation))
                            {
                                _knock.Play();
                            }
                            
                            if (i >= c)
                            {
                                _users.Add(conversation);
                                continue;
                            }
                            
                            if(_users[i] == conversation)
                                continue;

                            _users[i] = conversation;
                        }
                    });
                }
            });
        }
        
        public async void LoadMore()
        {
            if (!this._hasMore || this.IsBusy)
                return;

            this.IsBusy = true;

            var conversations = await ClientFactory.Client.Inbox.GetConversations(
                (long) (this._users.Max(u => u.LastMessageAt) - new DateTime(1970, 01, 01)).TotalSeconds);

            this._hasMore = !conversations.AtEnd;
            
            foreach (var conversation in conversations.Conversations)
            {
                this.Users.Add(conversation);
            }
            this.IsBusy = false;
        }

        private void OnCreateConversation()
        {
            _navigationService.NavigateTo(nameof(CreateConversationPage));
        }

        public ObservableCollection<ConversationUser> Users
        {
            get => _users;
            set => this.Set(ref _users, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.Set(ref _isBusy, value);
        }

        public ICommand CreateConversationCommand { get; }
        public ICommand OpenChatCommand { get; }
        public bool Disappeared { get; set; }
    }
}