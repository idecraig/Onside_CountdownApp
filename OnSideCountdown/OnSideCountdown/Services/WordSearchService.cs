using Newtonsoft.Json;
using OnSideCountdown.Enums;
using OnSideCountdown.Interfaces;
using OnSideCountdown.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OnSideCountdown.Services
{
    public class WordSearchService : IWordSearch
    {
        // Example of Request = REDRUM
        // Example of Response
        // [{"word":"MURDER","length":6,"conundrum":true},
        //  {"word":"DEMUR","length":5,"conundrum":false},
        //  {"word":"MURRE","length":5,"conundrum":false},
        //  {"word":"RUDER","length":5,"conundrum":false}]

        public List<WordResponse> CheckWord(string word)
        {
            var client = new HttpClient();
            var response = new HttpResponseMessage();

            try
            {
                client.DefaultRequestHeaders.Add("x-rapidapi-host", "danielthepope-countdown-v1.p.rapidapi.com");
                client.DefaultRequestHeaders.Add("x-rapidapi-key", "933b9e7ec9msh7b726d6dae7da15p1e3690jsna43e832a872e");
                response = Task.Run(() => client.GetAsync("https://danielthepope-countdown-v1.p.rapidapi.com/solve/" + word.Trim() + "?variance=1")).Result;
                var responseString = Task.Run(() => response.Content.ReadAsStringAsync()).Result;
                return (response.IsSuccessStatusCode) ? JsonConvert.DeserializeObject<List<WordResponse>>(responseString) : null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Dispose();
                response.Dispose();
            }
        }

        public string SuggestLetter(LetterType type)
        {
            var letters = type == LetterType.Vowel ? "AEIOU" : "BCDFGHJKLMNPQRSTVWXYZ";
            return letters.Substring((new Random()).Next(letters.Length - 1), 1);
        }
    }
}
