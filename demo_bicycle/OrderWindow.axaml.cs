using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo_bicycle.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace demo_bicycle;

public partial class OrderWindow : Window
{

    public OrderWindow()
    {
        InitializeComponent();
        using var context = new DiplomContext();
        Get();
    }

    private void Get()
    {
        using var context = new DiplomContext();
        var allOrders = context.Orders
                               .Include(x => x.Status)
                               .Include(x => x.PickUpPoint)
                               .ToList();

        OrdersBox.ItemsSource = allOrders;

    }

    private async void BackBut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Class1.isAdmin == true)
        {
            var catalogWindow = new CatalogWindow(Class1._user);
            catalogWindow.Show();
            this.Close();
        }

    }


}