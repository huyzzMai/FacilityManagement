using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Commons
{
    public static class CommonEnums
    {
        public class ROLE
        {
            public const int ADMIN = 1;
            public const int STUDENT = 2;
        }

        public class FEEDBACKSTATUS
        {
            public const int PENDING = 1;
            public const int FIXED = 2;
            public const int DENY = 3;
        }
    }
}
