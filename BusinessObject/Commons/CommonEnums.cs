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
        }

        public class DEPARTMENTSTATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;
        }

        public class USERSTATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;  
        }

        public class ROOMSTATUS
        {
            public const int ACTIVE = 0;
            public const int INACTIVE = 1;
        }
    }
}
