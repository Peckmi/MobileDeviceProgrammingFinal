using Xamarin.Forms;
using MobileDeviceProgrammingFinal.ViewModels;
using System;

namespace MobileDeviceProgrammingFinal.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new InventoryViewModel();
        }

        
    }
}
