﻿using MobileBiblioteca.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileBiblioteca.Views
{
    public partial class BooksPage : ContentPage
    {
        BookViewModel _viewModel;

        public BooksPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new BookViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }




        private async void SwipeItem_Invoked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Advertencia", "Desea eliminar este libro?", "Si", "No");
            if (answer)
            {
                var swipeview = sender as SwipeItem;
                var item = swipeview.CommandParameter;
                _viewModel.DeleteCommand.Execute(item);
                //_viewModel.RefreshCommand.Execute(item);
            }
        }
    }
}