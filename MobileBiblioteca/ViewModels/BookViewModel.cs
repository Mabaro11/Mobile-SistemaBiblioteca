using MobileBiblioteca.Models;
using MobileBiblioteca.Services;
using MobileBiblioteca.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileBiblioteca.ViewModels
{

    public class BookViewModel : BaseViewModel
    {
        private Book _selectedBook;
        public ObservableCollection<Book> Books { get; }
        public Command LoadBooksCommand { get; }
        public Command AddBookCommand { get; }
        public Command<Book> BookTapped { get; }

        public BookViewModel()
        {
            Title = "Libros";
            Books = new ObservableCollection<Book>();
            LoadBooksCommand = new Command(async () => await ExecuteLoadBooksCommand());

            BookTapped = new Command<Book>(OnBookSelected);

            //AddBookCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadBooksCommand()
        {
            IsBusy = true;

            try
            {
                Books.Clear();

                var url = "http://192.168.1.3:5000/api/book";
                var servicio = new RestHelper<List<Book>>();
                var books = await servicio.GetRestServiceDataAsync(url);


                foreach (var book in books)
                {
                    Books.Add(book);
              
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

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Book SelectedItem
        {
            get => _selectedBook;
            set
            {
                SetProperty(ref _selectedBook, value);
                OnBookSelected(value);
            }
        }

        async void OnBookSelected(Book book)
        {
            if (book == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={book.id}");
        }



    }


}
