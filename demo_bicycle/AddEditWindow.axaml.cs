using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using demo_bicycle.Models;
using MsBox.Avalonia;

namespace demo_bicycle;

public partial class AddEditWindow : Window
{
    User localUser;
    private string ImageName;
    private string currentphoto;
    private Tovar updatetovar; 

    public AddEditWindow() //add
    {
        InitializeComponent();
        DataContext = new Tovar();
        TovarIdTextBox.IsVisible = false;

        LoadCat();
        LoadMan();
        LoadSup();

        //кнопки
        AddBut.IsVisible = true;
        DeleteBut.IsVisible = false;
        EditBut.IsVisible = false;



    }


    public AddEditWindow(User user)
    {
        localUser = user;
    }

    public AddEditWindow(User user, Tovar tovar) //edit
    {
        InitializeComponent();
        using var context = new DiplomContext();
        localUser = user;
        updatetovar = tovar;
        DataContext = updatetovar;
        TovarIdTextBox.IsVisible = true;
        ImageBox.Source = updatetovar.GetPhoto;


        LoadCat();
        LoadMan();
        LoadSup();

        //кнопки
        AddBut.IsVisible = false;
        DeleteBut.IsVisible = true;
        EditBut.IsVisible = true;



        Manufacturer.SelectedItem = updatetovar.Manufacturer.ManufacturerName;
        Supplier.SelectedItem = updatetovar.Supplier.SupplierName;
        Category.SelectedItem = updatetovar.Category.CategoryName;
    }


    
   private void LoadMan()
    {
        using var context = new DiplomContext();
        Manufacturer.ItemsSource = context.Manufacturers.Select(x => x.ManufacturerName).ToList();

    }


    private void LoadCat()
    {
        using var context = new DiplomContext();
        Category.ItemsSource = context.Categories.Select(x => x.CategoryName).ToList();

    }


    private void LoadSup()
    {
        using var context = new DiplomContext();
        Supplier.ItemsSource = context.Suppliers.Select(x => x.SupplierName).ToList();

    }



    private async void AddBut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try 
        {
            using var context = new DiplomContext();
            var newTovar = DataContext as Tovar;


            if(string.IsNullOrEmpty(TitleBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле название пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if (string.IsNullOrEmpty(DescriptionBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле описание пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if (string.IsNullOrEmpty(CostBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле цена пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (string.IsNullOrEmpty(UnitBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле ед.измерения пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (string.IsNullOrEmpty(QuantityBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле количсетво пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if (string.IsNullOrEmpty(DiscountBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле скидка пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if(Supplier.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле поставщик пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if(Manufacturer.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле производитель пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if(Category.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Поле категория пусто", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if(ImageBox.Source == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Картинка не выбрана", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }


            decimal.TryParse(CostBox.Text, out decimal cost);
            int.TryParse(DiscountBox.Text, out int discount);
            int.TryParse(QuantityBox.Text, out int quantity);

            if(cost > 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Цена меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if(discount > 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Cкидка меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if (quantity > 0)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Количество меньше 0", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }


            newTovar!.Photo = "/images" + ImageName;

            newTovar.Manufacturer = context.Manufacturers.FirstOrDefault(x => x.ManufacturerName == Manufacturer.SelectedItem!.ToString())!;
            newTovar.Supplier = context.Suppliers.FirstOrDefault(x => x.SupplierName == Supplier.SelectedItem!.ToString())!;
            newTovar.Category = context.Categories.FirstOrDefault(x => x.CategoryName == Category.SelectedItem!.ToString())!;


            context.Add(newTovar);
            await context.SaveChangesAsync();


            var mess = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар создан", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await mess.ShowAsync();

            if(Class1.isAdmin == true)
            {
                CatalogWindow catalogWindow = new CatalogWindow(Class1._user);
                catalogWindow.Show();
                this.Close();
            }



        }
        catch (Exception ex)
        {
            var exep = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", exep, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();
        }


    }


    private async void DeleteBut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new DiplomContext();
        var tovarId = updatetovar.TovarId;
        var tovarToDelete = context.Tovars.Where(x => x.TovarId == tovarId).FirstOrDefault();

        context.Tovars.Remove(tovarToDelete!);
        await context.SaveChangesAsync();

        var mess = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар удален", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await mess.ShowAsync();

        if (Class1.isAdmin == true)
        {
            CatalogWindow catalogWindow = new CatalogWindow(Class1._user);
            catalogWindow.Show();
            this.Close();
        }

    }


    private async void EditBut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try 
        {
            using var context = new DiplomContext();
            var updatetovar = DataContext as Tovar;


            if(string.IsNullOrEmpty(TitleBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            if (string.IsNullOrEmpty(DescriptionBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (string.IsNullOrEmpty(CostBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (string.IsNullOrEmpty(UnitBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (string.IsNullOrEmpty(QuantityBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (string.IsNullOrEmpty(DiscountBox.Text))
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (Category.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (Manufacturer.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
            if (Supplier.SelectedItem == null)
            {
                var error = MessageBoxManager.GetMessageBoxStandard("", "", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }

            updatetovar.Manufacturer = context.Manufacturers.FirstOrDefault(x => x.ManufacturerName == Manufacturer.SelectedItem!.ToString())!;
            updatetovar.Supplier = context.Suppliers.FirstOrDefault(x => x.SupplierName == Supplier.SelectedItem!.ToString())!;
            updatetovar.Category = context.Categories.FirstOrDefault(x => x.CategoryName == Category.SelectedItem!.ToString())!;



            if (!string.IsNullOrEmpty(ImageName))
            {
                updatetovar?.Photo = "images/" + ImageName;
            }
            else if(!string.IsNullOrEmpty(currentphoto))
            {
                updatetovar?.Photo = currentphoto;
            }


            context.Tovars.Update(updatetovar!);
            await context.SaveChangesAsync();

            var mess = MessageBoxManager.GetMessageBoxStandard("Успех", "Товар обновлен", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await mess.ShowAsync();

            if (Class1.isAdmin == true)
            {
                CatalogWindow catalogWindow = new CatalogWindow(Class1._user);
                catalogWindow.Show();
                this.Close();
            }

        }
        catch(Exception ex)
        {
            var excep = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", excep, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await error.ShowAsync();

        }

    }



    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Добавить изображение",
            FileTypeChoices = new[]
            {
            FilePickerFileTypes.All
            }
        });

        if (file != null)
        {
            ImageBox.Source = new Bitmap(file.Path.LocalPath);
            ImageName = Guid.NewGuid().ToString() + ".png";
            var targetPath = AppDomain.CurrentDomain.BaseDirectory + "/images/" + ImageName;
            File.Copy(file.Path.LocalPath, targetPath);

        }
    }



    private async void BackBut_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(Class1.isAdmin == true)
        {
            var catalogWindow = new CatalogWindow(Class1._user);
            catalogWindow.Show();
            this.Close();
        }

    }

}