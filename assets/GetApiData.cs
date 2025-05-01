using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace CTTileToChallenge.assets
{
    public class GetApiData
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<TileData?> fetchApiData(string tileCode, int eventNumber)
        {
            string url = $"https://storage.googleapis.com/btd6-ct-map/events/{eventNumber}/tiles.json";

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string apiData = await response.Content.ReadAsStringAsync();
                var apiObject = JsonSerializer.Deserialize<Dictionary<string, TileData>>(apiData);

                if (apiObject != null && apiObject.TryGetValue(tileCode.ToUpper(), out TileData? tileData))
                {
                    return tileData;
                }
            }
            catch (HttpRequestException) { }

            return null;
        }

    }
}
