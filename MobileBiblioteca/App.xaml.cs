using Java.Lang;
using MobileBiblioteca.Services;
using MobileBiblioteca.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileBiblioteca
{
    public partial class App : Application
    {
        public string UrlBase { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            UrlBase = "http://mabaro.duckdns.org:5000";
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
