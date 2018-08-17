using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Xunit;

namespace SimpleToDo.Web.IntegrationTest.Helper
{
    public static class AntiForgeryHelper
    {
        private static string _antiForgeryToken;
        private static SetCookieHeaderValue _antiForgeryCookie;

        public static Regex AntiForgeryFormFieldRegex = new Regex(
            @"\<input name=""__RequestVerificationToken"" type=""hidden"" value=""([^""]+)"" \/\>");

        public static async Task<string> EnsureAntiForgeryTokenAsync(HttpClient client)
        {
            if (_antiForgeryToken != null)
                return _antiForgeryToken;

            var response = await client.GetAsync("/ToDoList/Create");
            response.EnsureSuccessStatusCode();

            _antiForgeryCookie = TryGetAntiForgeryCookie(response);

            Assert.NotNull(_antiForgeryCookie);

            AddCookieToDefaultRequestHeader(client, _antiForgeryCookie);

            _antiForgeryToken = await GetAntiForgeryToken(response);

            Assert.NotNull(_antiForgeryToken);

            return _antiForgeryToken;
        }

        private static SetCookieHeaderValue TryGetAntiForgeryCookie(HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> values))
            {
                return SetCookieHeaderValue.ParseList(values.ToList())
                    .SingleOrDefault(
                        c => c.Name.StartsWith(
                            ".AspNetCore.AntiForgery.",
                            StringComparison.InvariantCultureIgnoreCase));
            }

            return null;
        }

        private static void AddCookieToDefaultRequestHeader(
            HttpClient client,
            SetCookieHeaderValue antiForgeryCookie)
        {
            client.DefaultRequestHeaders.Add(
                "Cookie",
                new CookieHeaderValue(antiForgeryCookie.Name, antiForgeryCookie.Value)
                    .ToString());
        }

        private static async Task<string> GetAntiForgeryToken(HttpResponseMessage response)
        {
            var responseHtml = await response.Content.ReadAsStringAsync();
            var match = AntiForgeryFormFieldRegex.Match(responseHtml);

            return match.Success ? match.Groups[1].Captures[0].Value : null;
        }
    }
}