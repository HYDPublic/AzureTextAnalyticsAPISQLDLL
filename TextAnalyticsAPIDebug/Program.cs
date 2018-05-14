using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TextAnalyticsAPIClass;

namespace TextAnalyticsAPIDebug
{
    class Program
    {
        static int Main()
        {
            string JSONCallResponse = "";
            string APIBaseURL = "https://westus.api.cognitive.microsoft.com/";
            string APIVersionURL = "text/analytics/v2.0/";
            string APIKey = "c992924e0ce8433d856698b071c9bb86";
            string FullText = "The hotel was amazing!";
            string OperationID = "S";

            TextAnalyticsAPIClass.TextAnalyticsAPI( APIBaseURL,  APIVersionURL,  APIKey,  FullText,  OperationID);
            return 1;
        }
    }
}
