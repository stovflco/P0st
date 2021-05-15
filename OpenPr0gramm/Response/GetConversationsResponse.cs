using System.Collections.Generic;

namespace OpenPr0gramm
{
    public class GetConversationsResponse : Pr0grammResponse
    {
        public IReadOnlyList<ConversationUser> Conversations { get; private set; }
        public bool AtEnd { get; private set; }
    }
}
