using DynamicData;
using MobileBiblioteca.Models;
using MobileBiblioteca.Services;
using MobileBiblioteca.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileBiblioteca.ViewModels
{
    public class ReaderViewModel : BaseViewModel
    {
        string urlBase = ((App)App.Current).UrlBase + "/api/reader";

        public ReadOnlyObservableCollection<Reader> Readers => _readers;

        private readonly ReadOnlyObservableCollection<Reader> _readers;

        private SourceCache<Reader, string> _sourceCache = new SourceCache<Reader, string>(x => x.id.ToString());

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }


        private readonly IDisposable _cleanUp;

        public Command AddReaderCommand { get; }
        public Command<Reader> ReaderTapped { get; }
        public Command RefreshReaderCommand { get; set; }
        public Command<Reader> DeleteReaderCommand { get; }


        public ReaderViewModel()
        {
            Title = "Lectores";

            AddReaderCommand = new Command(OnAddReader);
            ReaderTapped = new Command<Reader>(OnReaderSelected);
            DeleteReaderCommand = new Command<Reader>(ExecuteRemoveReader);
            RefreshReaderCommand = new Command(async () => await ExecuteLoadReader());

            //Search logic
            Func<Reader, bool> readerFilter(string text) => reader =>
            {
                return string.IsNullOrEmpty(text) || reader.name.ToLower().Contains(text.ToLower());
            };

            var filterPredicate = this.WhenAnyValue(x => x.SearchText)
                                      .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                                      .DistinctUntilChanged()
                                      .Select(readerFilter);

            _cleanUp = _sourceCache.Connect()
            .RefCount()
            .Filter(filterPredicate)
            .Bind(out _readers)
            .DisposeMany()
            .Subscribe();
        }

        async Task ExecuteLoadReader()
        {
            IsBusy = true;
            try
            {
                _sourceCache.Clear();

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var servicio = new RestHelper<List<Reader>>();
                    var readers = await servicio.GetRestServiceDataAsync(urlBase);

                    _sourceCache.AddOrUpdate(readers);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ExecuteRemoveReader(Reader reader)
        {
            try
            {
                var url = this.urlBase + $"/{reader.id}";

                _sourceCache.Edit((update) =>
                {
                    update.Remove(reader);
                });

                var servicio = new RestHelper<Reader>();
                var readers = await servicio.DeleteRestServiceDataAsync(url);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        async void OnReaderSelected(Reader reader)
        {
            if (reader == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ReaderDetailPage)}?{nameof(ReaderDetailViewModel.ReaderId)}={reader.id}");
        }

        private async void OnAddReader(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewReaderPage));
        }

    }
}
