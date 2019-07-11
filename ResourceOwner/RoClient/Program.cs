using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RoClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Console.Out.WriteLineAsync("Identity Server RESOURCEOWNER  Test-Client Application...");


            #region Identity Server ve Token Istegi Olusturma
            var client = new HttpClient();
            /*
             * Identity Model Kütüphanesi Ids4 kullandığı OAuth2 protokolü için client oluşturmada kolaylık sağlar.
             * Identity Server root adresini veriyoruz. Bu kütüphane token endpoint'i kendisi discover (keşfeder) eder.
             * */
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");

            if (disco.IsError)
            {
                Console.WriteLine($"ERROR -- >{disco.Error}");
                return;
            }

            //Endpoint'den token almak için  Ids4'a client bilgileri ile request gönderiyoruz.
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "roClient",
                ClientSecret = "secret",
                Scope = "roApi",
                UserName ="admin",
                Password = "1234"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            #endregion


            #region Api adresine istek gönderme

            //Client uygulamada alınan access token header'a Bearer token olarak eklenir.
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:50481/api/identity");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                return;
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }


            #endregion
        }
    }
}
