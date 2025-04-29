using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace CTTileToChallenge.api
{
    public class GetApiData
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<JsonElement?> FetchData(string tileCode, int eventNumber)
        {
            string url = $"https://storage.googleapis.com/btd6-ct-map/events/{eventNumber}/tiles.json";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string apiData = await response.Content.ReadAsStringAsync();
                var apiObject = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(apiData);

                if (apiObject != null && apiObject.TryGetValue(tileCode.ToUpper(), out JsonElement tileData))
                {
                    return tileData;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
    }
}
