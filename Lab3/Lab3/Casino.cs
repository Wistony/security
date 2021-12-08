using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace Lab3
{
    public static class Casino
    {
        private const string ApiUrl = "http://95.217.177.249/casino/";

        public static Account CreateAccount()
        {
            string jsonResponse;
            do
            {
                var id = GenerateRandomId();
                var method = "createacc?id=" + id;
                jsonResponse = GetApiResponse(ApiUrl + method);
            } while (jsonResponse == "Error");
            
            return JsonSerializer.Deserialize<Account>(jsonResponse);
        }

        public static Bet MakeBet(string mode, string id, uint bet, int number)
        {
            var method = "play" + mode + "?id=" + id + "&bet=" + bet + "&number=" + number;
            var jsonResponse = GetApiResponse(ApiUrl + method);
            return JsonSerializer.Deserialize<Bet>(jsonResponse);
        }

        private static string GetApiResponse(string requestUrl)
        {
            string jsonResponse;
            try
            {
                var webResponse = WebRequest.CreateHttp(requestUrl).GetResponse();
                using var reader =
                        new StreamReader(webResponse.GetResponseStream()!);
                jsonResponse = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                jsonResponse = "Error";
            }
            
            return jsonResponse;
        }


        private static string GenerateRandomId()
        {
            var rnd = new Random();
            return rnd.Next(0, 100000).ToString();
        }
        
        
    }
}