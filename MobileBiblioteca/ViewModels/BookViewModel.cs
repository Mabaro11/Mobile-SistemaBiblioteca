using DynamicData;
using MobileBiblioteca.Models;
using MobileBiblioteca.Services;
using MobileBiblioteca.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileBiblioteca.ViewModels
{

    public class BookViewModel : BaseViewModel
    {

        string urlBase = ((App)App.Current).UrlBase + "/api/book";

        private Book _selectedBook;
        public Book SelectedItem
        {
            get => _selectedBook;
            set
            {
                SetProperty(ref _selectedBook, value);
                OnBookSelected(value);
            }
        }

        public Command AddBookCommand { get; }
        public Command<Book> BookTapped { get; }
        public Command RefreshCommand { get; set; }
        public Command<Book> DeleteCommand { get; }

        public ReadOnlyObservableCollection<Book> Books => _books;

        private readonly ReadOnlyObservableCollection<Book> _books;

        private SourceCache<Book, string> _sourceCache = new SourceCache<Book, string>(x => x.id.ToString());

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

        private string _searchText;

        private readonly IDisposable _cleanUp;


        public BookViewModel()
        {
            Title = "Libros";

            AddBookCommand = new Command(OnAddBook);
            BookTapped = new Command<Book>(OnBookSelected);
            DeleteCommand = new Command<Book>(ExecuteRemove);
            RefreshCommand = new Command(async () => await ExecuteLoadBooks());


            //Search logic
            Func<Book, bool> bookFilter(string text) => book =>
            {
                return string.IsNullOrEmpty(text) || book.title.ToLower().Contains(text.ToLower());
            };

            var filterPredicate = this.WhenAnyValue(x => x.SearchText)
                                      .Throttle(TimeSpan.FromMilliseconds(250), RxApp.TaskpoolScheduler)
                                      .DistinctUntilChanged()
                                      .Select(bookFilter);

            _cleanUp = _sourceCache.Connect()
            .RefCount()
            .Filter(filterPredicate)
            .Bind(out _books)
            .DisposeMany()
            .Subscribe();
        }

        async Task ExecuteLoadBooks()
        {
            IsBusy = true;
            try
            {
                _sourceCache.Clear();

                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    var servicio = new RestHelper<List<Book>>();
                    var books = await servicio.GetRestServiceDataAsync(this.urlBase);

                    _sourceCache.AddOrUpdate(books);
                }               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally {
                IsBusy = false;
            }
        }

        private async void ExecuteRemove(Book book)
        {
            try
            {
                var url = this.urlBase + $"/{book.id}";

                _sourceCache.Edit((update) =>
                {
                    update.Remove(book);
                });

                var servicio = new RestHelper<Book>();
                var books = await servicio.DeleteRestServiceDataAsync(url);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        async void OnBookSelected(Book book)
        {
            if (book == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(BookDetailPage)}?{nameof(BookDetailViewModel.BookId)}={book.id}");
        }

        private async void OnAddBook(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewBookPage));
        }


    }


}
