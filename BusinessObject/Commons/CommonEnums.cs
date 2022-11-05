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
            public const int USER = 2;
            public const int FIXER = 3;
        }

        public class DEPARTMENTSTATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;
            public const int BUSY = 2;
            public const int REMOVEBUSY = 3;
        }
        public class DEVICETYPETATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;
        }
        public class DEVICESTATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;
        }
        public class USERSTATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;
            public const int BAN = 2;
            public const int REMOVEBAN = 3;
        }

        public class ROOMSTATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;
        }

        public class FEEDBACKSTATUS
        {
            public const int PENDING = 1;
            public const int CLOSE = 2;
            public const int DENY = 3;
            public const int ACCEPT = 4;
        }

        public class LOGSTATUS
        {
            public const int FEEDBACK_CREATE = 1;
            public const int FEEDBACK_DENY = 2;
            public const int FEEDBACK_ACCEPT = 3;
            public const int FEEDBACK_CLOSE = 4;

        }
    }
}
