using MobileBiblioteca.Models;
using MobileBiblioteca.Services;
using Org.Apache.Http.Authentication;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using static Android.Util.EventLogTags;

namespace MobileBiblioteca.ViewModels
{
    [QueryProperty(nameof(ReaderId), nameof(ReaderId))]

    public class NewReaderViewModel : BaseViewModel
    {
        string urlBase = ((App)App.Current).UrlBase + "/api/";

        //Properties
        private string _readerId;
        public string _name;
        public string _address;
        public string _email;
        public string _phone;
        public string _dni;

        public string ReaderId
        {
            get
            {
                return _readerId;
            }
            set
            {
                _readerId = value;
                LoadReaderId(value);
            }
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }
        public string Dni
        {
            get => _dni;
            set => SetProperty(ref _dni, value);
        }


        //Commands
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public NewReaderViewModel()
        {
            Title = "Nuevo reader";

            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(_name) &&
                !string.IsNullOrWhiteSpace(_address) &&
                !string.IsNullOrWhiteSpace(_email) && 
                !string.IsNullOrWhiteSpace(_phone) &&
                !string.IsNullOrWhiteSpace(_dni);
        }

        private void OnSave()
        {

            try
            {
                if (ReaderId == null)
                {
                    SaveReader();
                }
                else
                {
                    UpdateReader();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void SaveReader()
        {
            IsBusy = true;

            try
            {

                Reader newReader = new Reader()
                {
                    id = 0,
                    name = Name,
                    address = Address,
                    email = Email,
                    phone = Phone,
                    dni = Dni
                };

                var url = urlBase + "reader";
                var servicio = new RestHelper<Reader>();
                var reader = await servicio.PostRestServiceDataAsync(url, newReader);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            await Shell.Current.GoToAsync("..");
        }

        private async void UpdateReader()
        {
            IsBusy = true;

            try
            {

                Reader updateReader = new Reader()
                {
                    id = Int32.Parse(ReaderId),
                    name = Name,
                    address = Address,
                    email = Email,
                    phone = Phone,
                    dni = Dni
                };

                var url = urlBase + "reader/" + ReaderId;
                var servicio = new RestHelper<Reader>();
                var reader = await servicio.PutRestServiceDataAsync(url, updateReader);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

            await Shell.Current.GoToAsync("..");
        }

        public async void LoadReaderId(string readerId)
        {
            try
            {
                var url = urlBase + "reader/" + readerId;
                var servicio = new RestHelper<Reader>();
                var reader = await servicio.GetRestServiceDataAsync(url);

                Name = reader.name;
                Address = reader.address;
                Dni = reader.dni;
                Phone = reader.phone;
                Email = reader.email;

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Reader");
            }
        }

    }
}
