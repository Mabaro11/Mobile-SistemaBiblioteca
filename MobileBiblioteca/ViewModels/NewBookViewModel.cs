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
using Xamarin.Forms;

namespace MobileBiblioteca.ViewModels
{
    [QueryProperty(nameof(BookId), nameof(BookId))]

    public class NewBookViewModel : BaseViewModel
    {
        string urlBase = ((App)App.Current).UrlBase + "/api/";

        //Properties
        private string _bookId;
        private string _bookTitle;
        public string _description;
        public string _editorial;
        public string _author;
        private Category _selectedCategory;

        public string BookId
        {
            get
            {
                return _bookId;
            }
            set
            {
                _bookId = value;
                LoadBookId(value);
            }
        }
        public string BookTitle
        {
            get => _bookTitle;
            set => SetProperty(ref _bookTitle, value);
        }                
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
        public string Editorial
        {
            get => _editorial;
            set => SetProperty(ref _editorial, value);
        }
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        //Commands
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public ReadOnlyObservableCollection<Category> Categories => _categories;
        private readonly ReadOnlyObservableCollection<Category> _categories;
        private SourceCache<Category, string> _sourceCache = new SourceCache<Category, string>(x => x.id.ToString());
        private readonly IDisposable _cleanUp;

        public NewBookViewModel()
        {
            Title = "Nuevo libro";

            // Initial data
            Task.Run(async () => await ExecuteLoadCategories());
            
            _cleanUp = _sourceCache.Connect()
            .RefCount()
            .Bind(out _categories)
            .DisposeMany()
            .Subscribe();

            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();

        }
        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(_bookTitle); 
        }

        async Task ExecuteLoadCategories()
        {
            IsBusy = true;

            try
            {
                _sourceCache.Clear();

                var url = urlBase +"category";
                var servicio = new RestHelper<List<Category>>();
                var categories = await servicio.GetRestServiceDataAsync(url);

                _sourceCache.AddOrUpdate(categories);
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

        public async void LoadBookId(string bookId)
        {
            try
            {
                var url = urlBase + "book/" + bookId;
                var servicio = new RestHelper<Book>();
                var book = await servicio.GetRestServiceDataAsync(url);

                BookTitle = book.title;
                Description =book.description;
                Author = book.author;
                Editorial = book.editorial;
                LoadCategory(book.categoryID);

            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Book");
            }
        }

        public void LoadCategory(int categoryId)
        {
               SelectedCategory = Categories.Where(x => x.id == categoryId).FirstOrDefault();

        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private void OnSave()
        {

            try
            {
                if (BookId == null)
                {
                    SaveBook();
                }
                else 
                { 
                    UpdateBook();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

        }

        private async void SaveBook()
        {
            IsBusy = true;

            try
            {

                Book newBook = new Book()
                {
                    id =  0,
                    title = BookTitle,
                    description = Description,
                    editorial = Editorial,
                    author = Author,
                    categoryID = SelectedCategory.id
                };

                var url = urlBase + "book";
                var servicio = new RestHelper<Book>();
                var book = await servicio.PostRestServiceDataAsync(url, newBook);

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

        private async void UpdateBook()
        {
            IsBusy = true;

            try
            {

                Book updateBook = new Book()
                {
                    id = Int32.Parse(BookId),
                    title = BookTitle,
                    description = Description,
                    editorial = Editorial,
                    author = Author,
                    categoryID = SelectedCategory.id
                };

                var url = urlBase + "book/" + BookId;
                var servicio = new RestHelper<Book>();
                var book = await servicio.PutRestServiceDataAsync(url, updateBook);

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

    }
}
