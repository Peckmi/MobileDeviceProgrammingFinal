using InventoryApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryApp.ViewModels
{
    public class InventoryViewModel : BindableObject
    {
        public ObservableCollection<InventoryItem> Items { get; } = new ObservableCollection<InventoryItem>();

        public Command<InventoryItem> IncreaseOrderCommand { get; set; }
        public Command<InventoryItem> DecreaseOrderCommand { get; set; }
        public Command SendOrderCommand { get; set; }

        public InventoryViewModel()
        {
            LoadItems();
            IncreaseOrderCommand = new Command<InventoryItem>(item =>
            {
                item.QuantityToOrder++;
                OnPropertyChanged(nameof(Items));
            });
            DecreaseOrderCommand = new Command<InventoryItem>(item =>
            {
                if (item.QuantityToOrder > 0)
                {
                    item.QuantityToOrder--;
                    OnPropertyChanged(nameof(Items));
                }
            });
            SendOrderCommand = new Command(async () => await SendOrder());
        }

        private void LoadItems()
        {
            Items.Add(new InventoryItem { Name = "Laptop - High Performance", Price = 1200, Stock = 10 });
            Items.Add(new InventoryItem { Name = "Desktop PC - All-in-One", Price = 800, Stock = 15 });
            Items.Add(new InventoryItem { Name = "SSD - 1TB", Price = 100, Stock = 50 });
            Items.Add(new InventoryItem { Name = "External Hard Drive - 4TB", Price = 120, Stock = 30 });
            Items.Add(new InventoryItem { Name = "RAM - 16GB DDR4", Price = 60, Stock = 70 });
            Items.Add(new InventoryItem { Name = "Graphics Card - Mid Range", Price = 350, Stock = 20 });
            Items.Add(new InventoryItem { Name = "Office 365 License", Price = 150, Stock = 200 });
            Items.Add(new InventoryItem { Name = "Antivirus Software License", Price = 40, Stock = 150 });
            Items.Add(new InventoryItem { Name = "Network Switch - 24 Ports", Price = 200, Stock = 25 });
            Items.Add(new InventoryItem { Name = "Wi-Fi Router", Price = 90, Stock = 40 });
        }

        private async Task SendOrder()
        {
            string userEmail = await Application.Current.MainPage.DisplayPromptAsync(
                "Order Confirmation",
                "Please enter your email to send the order receipt:",
                "Send",
                "Cancel",
                placeholder: "Email",
                keyboard: Keyboard.Email);

            if (!string.IsNullOrWhiteSpace(userEmail))
            {
                var orderSummary = CreateOrderSummary();
                await SendEmailReceipt(userEmail, orderSummary);
            }
        }

        private string CreateOrderSummary()
        {
            var orderedItems = Items.Where(item => item.QuantityToOrder > 0);
            var orderSummary = string.Join("\n", orderedItems.Select(item =>
                $"{item.Name}: {item.QuantityToOrder} x {item.Price:C} = {item.QuantityToOrder * item.Price:C}"));
            return orderSummary;
        }

        private async Task SendEmailReceipt(string email, string orderSummary)
        {
            var total = Items.Sum(item => item.QuantityToOrder * item.Price);
            var message = new EmailMessage
            {
                Subject = "Your Order Receipt",
                Body = $"Here is the list of items you ordered:\n{orderSummary}\nTotal: {total:C}\nThank you for your order!",
                To = new List<string> { email }
            };
            try
            {
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                // Email is not supported on this device
                await Application.Current.MainPage.DisplayAlert("Error", "Email is not supported on this device.", "OK");
            }
            catch (System.Exception ex)
            {
                // Other exceptions
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
