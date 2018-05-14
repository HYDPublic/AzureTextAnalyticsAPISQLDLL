//Name:     dbo.TextAnalyticsAPI
//Desc:		Execute an API call against the Azure Cognitive Services Text Analytics API for usage in a SQL 2016 CLR DLL
//Author:   Rolf Tesmer (Mr.Fox SQL) - https://mrfoxsql.wordpress.com/
//Date:     03 Sep 2016

using System;
using System.Text;
using Microsoft.SqlServer.Server;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data;

public class TextAnalyticsAPIClass
{
    static async Task<String> CallEndpoint(HttpClient client, string uri, byte[] byteData)
    {
        using (var content = new ByteArrayContent(byteData))
        {
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //var JSONCallResponse = await client.PostAsync(uri, content); // fails
            var JSONCallResponse = client.PostAsync(uri, content).Result;
            return await JSONCallResponse.Content.ReadAsStringAsync();
        }
    }

    public static async void TextAnalyticsAPI(String APIBaseURL, String APIVersionURL, String APIKey, String FullText, String OperationID)
    {

        HttpClient client;
        byte[] byteData;
        string JSONCallString = "";
        string JSONCallResponse = "";
        string uri = "";

        // Define client http request header
        client = new HttpClient();
        client.BaseAddress = new Uri(APIBaseURL);
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // Define JSON Call Request body
        JSONCallString = "{\"documents\":[{\"id\":\"" + 1 + "\",\"text\":\"" + FullText.Replace("\"", "") + "\"}]}";
        byteData = Encoding.UTF8.GetBytes(JSONCallString);

        // Define JSON Call Request uri
        if (OperationID == "S")
            uri = APIBaseURL + APIVersionURL + "sentiment";
        if (OperationID == "P")
            uri = APIBaseURL + APIVersionURL + "keyPhrases";

        // Call the Text Analytics API
        var content = new ByteArrayContent(byteData);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        JSONCallResponse = await CallEndpoint(client, uri, byteData);

        // Return the JSON Result
        SqlDataRecord record = new SqlDataRecord(new SqlMetaData("JSONDocument", SqlDbType.NVarChar, 4000));
        record.SetSqlString(0, JSONCallResponse);
        SqlContext.Pipe.Send(record);
    }
}
