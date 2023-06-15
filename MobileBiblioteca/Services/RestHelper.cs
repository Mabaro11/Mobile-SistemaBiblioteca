using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileBiblioteca.Services
{
    public class RestHelper<T>
    {
        public async Task<T> GetRestServiceDataAsync(String serviceAddress)
        {
            //Creamos una instancia de HttpClient
            var client = new HttpClient();
            //Asignamos la URL
            client.BaseAddress = new Uri(serviceAddress);
            //Llamada asíncrona al sitio
            var response = await client.GetAsync(client.BaseAddress);
            //Nos aseguramos de recibir una respuesta satisfactoria
            response.EnsureSuccessStatusCode();
            //Convertimos la respuesta a una variable string
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            //Se deserializa la cadena y se convierte en una instancia de WeatherResult
            var result = JsonConvert.DeserializeObject<T>(jsonResult);
            return result;
        }
    }
}
