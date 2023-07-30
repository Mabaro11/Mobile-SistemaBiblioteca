using MobileBiblioteca.Models;
using MobileBiblioteca.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MobileBiblioteca.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        string urlBase = ((App)App.Current).UrlBase + "/api/";

        private QuantityResponse _bookQuantity;
        public QuantityResponse BookQuantity
        {
            get => _bookQuantity;
            set => SetProperty(ref _bookQuantity, value);
        }

        private QuantityResponse _readerQuantity;
        public QuantityResponse ReaderQuantity
        {
            get => _readerQuantity;
            set => SetProperty(ref _readerQuantity, value);
        }


        public AboutViewModel()
        {
            Title = "Inicio";
            Task.Run(async () => await ExecuteLoadQuantity());
        }

        async Task ExecuteLoadQuantity()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var servicio = new RestHelper<QuantityResponse>();
                    BookQuantity = await servicio.GetRestServiceDataAsync(this.urlBase + "book/quantity");
                    ReaderQuantity = await servicio.GetRestServiceDataAsync(this.urlBase + "reader/quantity");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}