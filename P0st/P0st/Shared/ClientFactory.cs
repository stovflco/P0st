using OpenPr0gramm;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace P0st.Shared
{
    internal static class ClientFactory
    {
        private static Pr0grammApiClient _apiClient;
        private static Pr0grammClient _client;

        public static Pr0grammClient Client { get => _client; set => _client = value; }
        public static Pr0grammApiClient ApiClient { get => _apiClient; set => _apiClient = value; }
        public static GetUserInfoResponse CurrentUser { get; set; }
        public static string CurrentUserName => _apiClient.GetMeCookie().N;
    }
}
