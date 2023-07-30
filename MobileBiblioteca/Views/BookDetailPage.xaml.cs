using MobileBiblioteca.ViewModels;
using Xamarin.Forms;

namespace MobileBiblioteca.Views
{
    public partial class BookDetailPage : ContentPage
    {
        public BookDetailPage()
        {
            InitializeComponent();
            BindingContext = new BookDetailViewModel();
        }
    }
}