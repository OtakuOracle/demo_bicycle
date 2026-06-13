using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using demo_bicycle.Models;
using MsBox.Avalonia;

namespace demo_bicycle;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Guest_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CatalogWindow catalogWindow = new CatalogWindow();
        catalogWindow.Show();
        this.Close();

    }


    private async void Auth_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        using var context = new DiplomContext();
        var login = LoginTextBox.Text;
        var pass = PasswordTextBox.Text;

        var user = context.Users.FirstOrDefault(x => x.Login == login && x.Password == pass);

        if (user != null)
        {
            if (user.RoleId == 1)
            {
                Class1.isAdmin = true;
                Class1._user = user;

            }
            CatalogWindow catalogWindow = new CatalogWindow(user);
            catalogWindow.Show();
            this.Close();
        }
        else
        {
            var mess = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Некорректные данные", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await mess.ShowAsync();
        }
     

    }




}



