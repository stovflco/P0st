﻿using System;
using System.Collections.Generic;

namespace OpenPr0gramm
{
#if FW
    [Serializable]
#endif
    public class GetCommentsResponse : Pr0grammResponse
    {
        public IReadOnlyList<ProfileComment> Comments { get; private set; }
        public bool HasOlder { get; private set; }
        public bool HasNewer { get; private set; }
        public CommentUser User { get; private set; }
    }


#if FW
    [Serializable]
#endif
}
