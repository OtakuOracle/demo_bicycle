using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo_bicycle.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using static System.Net.Mime.MediaTypeNames;

namespace demo_bicycle;

public partial class CatalogWindow : Window
{
    User localUser;
    public CatalogWindow()
    {
        InitializeComponent();
        using var context = new DiplomContext();
        Get();
        FioTextBlock.Text = "Гость";
        Visibility(3);

    }

    public CatalogWindow(User user)
    {
        InitializeComponent();
        localUser = user;
        using var context = new DiplomContext();
        Get();
        FioTextBlock.Text = user.FullName;
        Visibility(user.RoleId);

    }

    public void Visibility(int roleId)
    {
        switch(roleId)
        {
            case 1: Filter.IsVisible = true; SortCost.IsVisible = true; SortQuantity.IsVisible = true; SortDiscount.IsVisible = true; AddBut.IsVisible = true; break;
            case 2: Filter.IsVisible = true; SortCost.IsVisible = true; SortQuantity.IsVisible = true; SortDiscount.IsVisible = true; break;
     
        }
    }



        

    private void Get()
    {
        using var context = new DiplomContext();
        var allTovars = context.Tovars
                               .Include(x => x.Category)
                               .Include(x => x.Supplier)
                               .Include(x => x.Manufacturer)
                               .ToList();

        switch(SortCost.SelectedIndex)
        {
            case 0:
                allTovars = allTovars.OrderBy(x => x.Cost).ToList();
                break;
            case 1:
                allTovars = allTovars.OrderByDescending(x => x.Cost).ToList();
                break;
            default:
                allTovars = allTovars.OrderBy(x => x.Cost).ToList();
                break;
        }

        switch (SortQuantity.SelectedIndex)
        {
            case 0:
                allTovars = allTovars.OrderBy(x => x.Quantity).ToList();
                break;
            case 1:
                allTovars = allTovars.OrderByDescending(x => x.Quantity).ToList();
                break;
            default:
                allTovars = allTovars.OrderBy(x => x.Quantity).ToList();
                break;
        }


        switch (SortDiscount.SelectedIndex)
        {
            case 0:
                allTovars = allTovars.OrderBy(x => x.Discount).ToList();
                break;
            case 1:
                allTovars = allTovars.OrderByDescending(x => x.Discount).ToList();
                break;
            default:
                allTovars = allTovars.OrderBy(x => x.Discount).ToList();
                break;
        }

        if (Filter.SelectedIndex != -1)
        {
            switch (Filter.SelectedIndex)
            {
                case 0:
                    allTovars = allTovars;
                    break;
                case 1:
                    allTovars = allTovars.Where(x => x.Discount >= 0 && x.Discount <= 11.99).ToList();
                    break;
                case 2:
                    allTovars = allTovars.Where(x => x.Discount >= 12 && x.Discount <= 18.99).ToList();
                    break;
                case 3:
                    allTovars = allTovars.Where(x => x.Discount >= 19).ToList();
                    break;
            }
        }




        if (SearchBox.Text != null)
        {
            allTovars = allTovars.Where(x =>
            x.Article.ToLower().Contains(SearchBox.Text) ||
            x.Supplier.SupplierName.ToLower().Contains(SearchBox.Text) ||
            x.Manufacturer.ManufacturerName.ToLower().Contains(SearchBox.Text) ||
            x.Category.CategoryName.ToLower().Contains(SearchBox.Text) ||
            x.Description.ToLower().Contains(SearchBox.Text) 
            ).ToList();


        }
        TovarsBox.ItemsSource = allTovars;

    }




    private void SearchBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        Get();
    }

    private void Filter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();

    }

    private void SortQuantity_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();

    }

    private void SortCost_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();

    }

    private void SortDiscount_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();

    }


    private async void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();

    }

    private async void AddBut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AddEditWindow addEditWindow = new AddEditWindow();
        addEditWindow.Show();
        this.Close();

    }

    private async void TovarsBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
       if(TovarsBox.SelectedItem is Tovar tovar)
        {
            if(Class1.isAdmin == true)
            {
                AddEditWindow addEditWindow = new AddEditWindow(localUser, tovar);
                addEditWindow.Show();
                this.Close();
            }
            else
            {
                var mess = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Вы не можете редактировать", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await mess.ShowAsync();
            }
        }

    }


}