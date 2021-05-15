using Refit;
using System.Threading.Tasks;

namespace OpenPr0gramm
{
    public interface IPr0grammInboxService
    {
        [Get("/inbox/all")]
        Task<GetMessagesResponse<InboxItem>> GetAll();

        [Get("/inbox/messages")]
        Task<GetMessagesResponse<PrivateMessage>> GetPrivateMessages();

        [Get("/inbox/messages")]
        Task<GetMessagesResponse<PrivateMessage>> GetMessages(string with, long? older = null);

        [Post("/inbox/post")]
        Task<Pr0grammResponse> PostMessage([Body(BodySerializationMethod.UrlEncoded)]PrivateMessageData data);

        [Get("/inbox/unread")]
        Task<GetMessagesResponse<InboxItem>> GetUnreadMessages();

        [Get("/inbox/conversations")]
        Task<GetConversationsResponse> GetConversations(long? older);

        //[Get("/inbox/conversations")]
        //Task<GetMessagesResponse<Conversation>> GetMessages(string with);
    }
}
