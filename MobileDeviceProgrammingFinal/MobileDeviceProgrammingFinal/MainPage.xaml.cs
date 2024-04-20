using Xamarin.Forms;
using InventoryApp.ViewModels;

namespace InventoryApp.Views
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
