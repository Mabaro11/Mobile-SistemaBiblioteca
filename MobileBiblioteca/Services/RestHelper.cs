using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
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
            //Se deserializa la cadena y se convierte en una instancia del tipo de objeto T
            var result = JsonConvert.DeserializeObject<T>(jsonResult);
            return result;
        }
        public async Task<T> PostRestServiceDataAsync(String serviceAddress, T body)
        {
            //convertimos elemento en Json.
            var jsonRequest = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.BaseAddress = new Uri(serviceAddress);
            var response = await client.PostAsync(client.BaseAddress,content);
            response.EnsureSuccessStatusCode();
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T>(jsonResult);
            return result;
        }
        public async Task<T> PutRestServiceDataAsync(String serviceAddress, T body)
        {
            var jsonRequest = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var client = new HttpClient();
            client.BaseAddress = new Uri(serviceAddress);
            var response = await client.PutAsync(client.BaseAddress, content);
            response.EnsureSuccessStatusCode();
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T>(jsonResult);
            return result;
        }
        public async Task<T> DeleteRestServiceDataAsync(String serviceAddress)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(serviceAddress);
            var response = await client.DeleteAsync(client.BaseAddress);
            response.EnsureSuccessStatusCode();
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<T>(jsonResult);
            return result;
        }
    }
}
