using MobileBiblioteca.Models;
using MobileBiblioteca.Services;
using MobileBiblioteca.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace MobileBiblioteca.ViewModels
{

    [QueryProperty(nameof(BookId), nameof(BookId))]
    public class BookDetailViewModel : BaseViewModel
    {
        string urlBase = ((App)App.Current).UrlBase + "/api/";
       
        public string Id { get; set; }

        private Book book;
        public Book Book
        {
            get => book;
            set => SetProperty(ref book, value);
        }

        private Category category;
        public Category Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        private string bookId;
        public string BookId
        {
            get
            {
                return bookId;
            }
            set
            {
                bookId = value;
                LoadBookId(value);
                
            }
        }

        public Command UpdateBookCommand { get; set; }

        public BookDetailViewModel()
        {
            Title = "Detalle";
            UpdateBookCommand = new Command(ExecuteBookUpdate);
        }

        public async void LoadBookId(string bookId)
        {
            try
            {
                var url = urlBase + "book/" + bookId ;
                var servicio = new RestHelper<Book>();
                var book = await servicio.GetRestServiceDataAsync(url);


                Id = book.id.ToString();
                Book = book;

                LoadCategory(book.categoryID);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Book");
            }
        }

        public async void LoadCategory(int categoryId)
        {
            var url = urlBase + "category/" + categoryId;
            var servicio = new RestHelper<Category>();
            Category = await servicio.GetRestServiceDataAsync(url);

        }

        async void ExecuteBookUpdate()
        {
            try
            {
                await Shell.Current.GoToAsync($"{nameof(NewBookPage)}?{nameof(NewBookViewModel.BookId)}={Id}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

    }
}
