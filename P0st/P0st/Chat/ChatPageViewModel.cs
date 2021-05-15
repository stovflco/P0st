using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Views;
using OpenPr0gramm;
using P0st.Shared;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace P0st.Chat
{
    public class ChatPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private string _username;
        private ObservableCollection<PrivateMessage> _messages;
        private ISimpleAudioPlayer _audio = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer(); 
        private ISimpleAudioPlayer _sendAudio = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer(); 

        private bool _hasNewer;
        private bool _hasOlder;
        private bool _isBusy;
        private User _user;
        private bool _autoScroll = true;
        private bool _scrollDownVisible = true;
        private string _currentMessage;

        public ChatPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            this.NavigateBackCommand = new Command(this.OnNavigateBack);
            this.ScrollToEndCommand = new Command(this.OnScrollToEnd);
            this.SendMessageCommand = new Command(this.OnSendMessage);
            this._messages = new ObservableCollection<PrivateMessage>();
            var audioStream = GetType().Assembly.GetManifestResourceStream("P0st.post.wav");
            this._audio.Load(audioStream);
            var sendAudioStream = GetType().Assembly.GetManifestResourceStream("P0st.send.mp3");
            this._sendAudio.Load(sendAudioStream);
        }

        private async void OnSendMessage()
        {
            
            /*this._messages.Add(new PrivateMessage
            {
                Mark = this._user.Mark,
                Name = this._user.Name,
                CreatedAt = DateTime.Now,
                Sent = true,
                Message = this.CurrentMessage,
            });*/

            if (string.IsNullOrWhiteSpace(this._currentMessage))
                return;
            
            
            this._sendAudio.Play();
            var msg = new PrivateMessage
            {
                Mark = ClientFactory.CurrentUser.Account.Mark,
                Name = ClientFactory.CurrentUserName,
                Message = this._currentMessage,
                Sent = false,
                CreatedAt = DateTime.Now,
            };
            this.Messages.Add(msg);
            await Task.Run(() => SendAsync(msg));

            this.CurrentMessage = string.Empty;
        }

        private async Task SendAsync(PrivateMessage message)
        {
            await ClientFactory.Client.Inbox.SendMessage(this._user, message.Message);
            Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                message.Sent = true;
                var i = this.Messages.IndexOf(message);
                this.Messages[i] = null;
                this.Messages[i] = message;
            });
        }

        private void OnScrollToEnd()
        {
            this.AutoScroll = true;
        }

        private void OnNavigateBack()
        {
            this._navigationService.GoBack();
            this.Clear();
        }

        private void Clear()
        {
            this.User = new User
            {
                Name = "...",
                Mark = UserMark.Unbekannt
            };
            this.Messages.Clear();
            this._username = null;
            this._hasOlder = false;
            this._hasNewer = false;
        }

        public async void LoadChat(string username)
        {
            this.Clear();
            this.IsBusy = true;
            this._username = username;
            var user = await ClientFactory.Client.Profile.GetInfo(username, ItemFlags.All);
            this.User = user.User;

            var response = await ClientFactory.Client.Inbox.GetMessages(username);
            this._hasNewer = response.HasNewer;
            this._hasOlder = response.HasOlder;
            foreach (var message in response.Messages.OrderBy(m => m.CreatedAt))
            {
                this.Messages.Add(message);
            }
            //this.AutoScroll = true;
            this.IsBusy = false;
            this.RunMessageLoop();
        }

        private void RunMessageLoop()
        {
            Task.Run(async () =>
            {
                while (this._user != null && this._user.Id > 0)
                {
                    Thread.Sleep(1000);
                    if (this._isBusy)
                        continue;
                    var response = await ClientFactory.Client.Inbox.GetMessages(this._username);
                    this._hasNewer = response.HasNewer;
                    this._hasOlder = response.HasOlder;
                    var knownMessages = this._messages.ToList();

                    foreach (var message in response.Messages)
                    {
                        if (knownMessages.Any(m => m.Id == message.Id))
                            continue;

                        var last = knownMessages.LastOrDefault(m => m.CreatedAt < message.CreatedAt);
                        if (last == null)
                        {
                            knownMessages.Add(message);
                            Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
                            {
                                _messages.Add(message);
                                _audio.Play();
                            });
                            continue;
                        }
                        var i = knownMessages.IndexOf(last);
                        
                        knownMessages.Insert(i + 1, message);
                        Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
                        {
                            _messages.Insert(i + 1, message);
                            _audio.Play();
                        });
                    }
                    
                    Application.Current.Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var message in this._messages.Where(m => m.Id == 0).ToList())
                        {
                            _messages.Remove(message);
                        }
                    });
                }
            });
        }

        public async void LoadOlder()
        {
            if (!this._hasOlder || this.IsBusy)
                return;
            this.IsBusy = true;
            var response = await ClientFactory.Client.Inbox.GetMessages(this._username, (long)(this._messages.Min(m => m.CreatedAt) - new DateTime(1970,1,1)).TotalSeconds);
            this._hasNewer = response.HasNewer;
            this._hasOlder = response.HasOlder;
            foreach (var message in response.Messages.OrderBy(m => m.CreatedAt))
            {
                this.Messages.Insert(0, message);
            }
            
            this.IsBusy = false;
        }

        public User User
        {
            get => _user;
            set => this.Set(ref _user, value);
        }

        public ObservableCollection<PrivateMessage> Messages
        {
            get => _messages;
            set => this.Set(ref _messages, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.Set(ref _isBusy, value);
        }

        public bool AutoScroll
        {
            get => _autoScroll;
            set
            {
                this.Set(ref _autoScroll, value);
                this.RaisePropertyChanged(nameof(ScrollDownVisible));
            }
        }

        public bool ScrollDownVisible => !_autoScroll;

        public string CurrentMessage
        {
            get => _currentMessage;
            set => this.Set(ref _currentMessage, value);
        }

        public ICommand SendMessageCommand { get; }

        public ICommand NavigateBackCommand { get; }
        public ICommand ScrollToEndCommand { get; }

        public void OnDisappeared()
        {
            this.Clear();
        }
    }

    public class MessageViewModel
    {
        public string Name { get; set; }
        
    }
}