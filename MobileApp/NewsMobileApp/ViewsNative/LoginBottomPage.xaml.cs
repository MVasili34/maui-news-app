using NewsMobileApp.Models;
using NewsMobileApp.TempServices;

namespace NewsMobileApp.ViewsNative;

public partial class LoginBottomPage
{
    private AuthorizeModel viewModel => BindingContext as AuthorizeModel;
    private readonly IRequestsService _requestService;

	public LoginBottomPage(IRequestsService requestService)
	{
		InitializeComponent();
  
        _requestService = requestService;
        BindingContext = new AuthorizeModel()
        {
            EmailAddress = Preferences.Get("emailAddress", string.Empty),
            Password = Preferences.Get("password", string.Empty)
        };
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        await DismissAsync();
        Application.Current.MainPage = new AppShell();
        /*
        try
        {
            MailAddress mailAddress = new(viewModel.EmailAddress);
            if (!StrongPasswordChecker.PasswordCheck(viewModel.Password))
                throw new InvalidDataException("������� ������ ������! ��������:\n1) �� ����� 8 ���������;\n2) ���� ��������� �����;" +
                    "\n3) ���� �������� �����;\n4) ���� �����;\n5) ���� ����������� ������;\n6) ��� ��������;");
            BlockControls(false);
            await Task.Delay(3000);
            string serial = JsonConvert.SerializeObject(viewModel);
            await Application.Current.MainPage.DisplayAlert("Success", serial, "OK");
            Application.Current.MainPage = new AppShell();
            await DismissAsync();
        }
        catch (FormatException)
        {
            EmailSend.TextColor = Color.Parse("#ED1C24");
        }
        catch (ArgumentException)
        {
            await Application.Current.MainPage.DisplayAlert("������!", "��� ����������� ��������� �������������� ����� ����������� �����.", "OK");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("������!", ex.Message, "OK");
        }
        BlockControls(true);
        */
    }

    private async void RegisterClicked_Tapped(object sender, TappedEventArgs e)
    {
        await DismissAsync();
        await Navigation.PushModalAsync(new RegisterPage(_requestService));
    }

    private void ShowPassword_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (showPassword.IsChecked)
        {
            PasswordShow.IsPassword = false;
            return;
        }
        PasswordShow.IsPassword = true;
    }

    private void EmailShow_TextChanged(object sender, TextChangedEventArgs e) => 
        EmailSend.TextColor = Color.Parse("#ffffff");

    private void BlockControls(bool isBusy)
    {
        IsCancelable = isBusy;
        EmailSend.IsReadOnly = !isBusy;
        PasswordShow.IsReadOnly = !isBusy;
        Login.IsEnabled = isBusy;
        RegisterButton.IsEnabled = isBusy;
    }
}