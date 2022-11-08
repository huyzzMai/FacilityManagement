using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ResponseModel.LogResponse
{
    public class LogStatisticResponse
    {
        public int currentMonth;
        public int NumberOfCreatedFeedback;
        public int NumberOfAcceptedFeedback;
        public int NumberOfClosedFeedback;
        public int NumberOfDeniedFeedback;
        public float FinishedRatio;
        public string mvpFixerName;
    }
}
