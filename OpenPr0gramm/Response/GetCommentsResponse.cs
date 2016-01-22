﻿using System;
using System.Collections.Generic;

namespace OpenPr0gramm
{
#if FW
    [Serializable]
#endif
    public class GetCommentsResponse : Pr0grammResponse
    {
        public IReadOnlyList<ProfileComment> Comments { get; set; }
        public bool HasOlder { get; set; }
        public bool HasNewer { get; set; }
        public CommentUser User { get; set; }
    }
}
