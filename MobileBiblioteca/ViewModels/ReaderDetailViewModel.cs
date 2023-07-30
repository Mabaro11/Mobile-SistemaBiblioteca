using MobileBiblioteca.Models;
using MobileBiblioteca.Services;
using MobileBiblioteca.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using static Android.Resource;

namespace MobileBiblioteca.ViewModels
{
    [QueryProperty(nameof(ReaderId), nameof(ReaderId))]
    public class ReaderDetailViewModel : BaseViewModel
    {
        string urlBase = ((App)App.Current).UrlBase + "/api/";

        public string Id { get; set; }

        private string readerId;
        public string ReaderId
        {
            get { return readerId; }
            set {
                readerId = value;
                LoadReaderId(value);
            }
        }

        private Reader _reader;
        public Reader Reader
        {
            get => _reader;
            set => SetProperty(ref _reader, value);
        }

        private string _firstLetterName;
        public string FirstLetterName 
        { 
            get => _firstLetterName; 
            set => SetProperty(ref _firstLetterName, value);    
        }

        public Command UpdateReaderCommand { get; set; }

        public ReaderDetailViewModel()
        {
            Title = "Detalle";
            UpdateReaderCommand = new Command(ExecuteReaderUpdate);

        }

        public async void LoadReaderId(string readerId)
        {
            try
            {
                var url = urlBase + "reader/" + readerId;
                var servicio = new RestHelper<Reader>();
                var reader = await servicio.GetRestServiceDataAsync(url);

                Id = reader.id.ToString();
                Reader = reader;

                setFirstLetterName(reader.name);

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Book");
            }
        }

        private void setFirstLetterName(string name)
        {
            FirstLetterName =  name.Substring(0, 1);
        }

        async void ExecuteReaderUpdate()
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(NewReaderPage)}?{nameof(NewReaderViewModel.ReaderId)}={Id}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
