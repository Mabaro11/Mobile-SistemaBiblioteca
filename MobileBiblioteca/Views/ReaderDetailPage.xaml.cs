
using MobileBiblioteca.ViewModels;
using Xamarin.Forms;

namespace MobileBiblioteca.Views
{
    
    public partial class ReaderDetailPage : ContentPage
    {
        ReaderDetailViewModel _viewModel;

        public ReaderDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ReaderDetailViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //_viewModel.OnAppearing();
        }
    }
}