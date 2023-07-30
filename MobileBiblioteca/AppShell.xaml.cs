using MobileBiblioteca.ViewModels;
using MobileBiblioteca.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MobileBiblioteca
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(BookDetailPage), typeof(BookDetailPage));
            Routing.RegisterRoute(nameof(NewBookPage), typeof(NewBookPage));

            Routing.RegisterRoute(nameof(ReaderDetailPage), typeof(ReaderDetailPage));
            Routing.RegisterRoute(nameof(NewReaderPage), typeof(NewReaderPage));

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
