using MobileBiblioteca.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileBiblioteca.Views
{
    public partial class ReaderPage : ContentPage
    {
        ReaderViewModel _viewModel;

        public ReaderPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ReaderViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Advertencia", "Desea eliminar este lector?", "Si", "No");
            if (answer)
            {
                var swipeview = sender as SwipeItem;
                var item = swipeview.CommandParameter;
                _viewModel.DeleteReaderCommand.Execute(item);
                //_viewModel.RefreshReaderCommand.Execute(item);
            }
        }
    }
}