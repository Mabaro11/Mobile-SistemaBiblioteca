using MobileBiblioteca.ViewModels;
using Xamarin.Forms;

namespace MobileBiblioteca.Views
{
    public partial class NewBookPage : ContentPage
    {
        public NewBookPage()
        {
            InitializeComponent();
            BindingContext = new NewBookViewModel();

        }
    }
}