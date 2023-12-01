using NewsMobileApp.TempServices;
using System.Net.Mail;
using CryptographyTool;
using NewsMobileApp.ViewModels;

namespace NewsMobileApp.ViewsNative;

public partial class LoginBottomPage
{
    private AuthorizeViewModel viewModel => BindingContext as AuthorizeViewModel;
    private readonly IRequestsService _requestService;

	public LoginBottomPage(IRequestsService requestService)
	{
		InitializeComponent();
  
        _requestService = requestService;
        BindingContext = new AuthorizeViewModel()
        {
            EmailAddress = Preferences.Get("emailAddress", string.Empty),
            Password = Preferences.Get("password", string.Empty)
        };
    }

    private async void Login_Clicked(object sender, EventArgs e)
    {
        //await DismissAsync();
        //Application.Current.MainPage = new AppShell();
        try
        {
            MailAddress mailAddress = new(viewModel.EmailAddress);
            if (!StrongPasswordChecker.PasswordCheck(viewModel.Password))
                throw new InvalidDataException("������� ������ ������! ��������:\n1) �� ����� 8 ���������;\n2) ���� ��������� �����;" +
                    "\n3) ���� �������� �����;\n4) ���� �����;\n5) ���� ����������� ������;\n6) ��� ��������;");
            BlockControls(false);
            UserViewModel model = await _requestService.LoginUserAsync(viewModel);
            if (model is null)
                throw new Exception("��������� ������ �������!");
            Preferences.Set("userId", model.UserId.ToString());
            Preferences.Set("userName", model.UserName);
            Preferences.Set("emailAddress", model.EmailAddress);
            Preferences.Set("phone", model.Phone);
            if (model.DateOfBirth.HasValue)
                Preferences.Set("dateOfBirth", model.DateOfBirth.Value.ToDateTime(new(0,0,0)));
            else
                Preferences.Set("dateOfBirth", null);
            Preferences.Set("password", viewModel.Password);
            Preferences.Set("roleId", model.RoleId);
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
        EmailSend.TextColor = Application.Current.PlatformAppTheme == AppTheme.Dark ? Color.Parse("#ffffff") : Color.Parse("#000000");

    private void BlockControls(bool isBusy)
    {
        IsCancelable = isBusy;
        EmailSend.IsReadOnly = !isBusy;
        PasswordShow.IsReadOnly = !isBusy;
        Login.IsEnabled = isBusy;
        RegisterButton.IsEnabled = isBusy;
    }
}