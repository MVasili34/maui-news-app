using NewsMobileApp.Models;
using NewsMobileApp.TempServices;
using System.Net.Mail;
using CryptographyTool;
using Newtonsoft.Json;

namespace NewsMobileApp.ViewsNative;

public partial class RegisterPage : ContentPage
{
    private RegisterModel viewModel => BindingContext as RegisterModel;
    private readonly IRequestsService _requestsService;

	public RegisterPage(IRequestsService requestsService)
	{
		InitializeComponent();
        _requestsService = requestsService;
        BindingContext = new RegisterModel();
    }

    private async void Register_Clicked(object sender, EventArgs e)
    {
        try
        {
            MailAddress mailAddress = new(viewModel.EmailAddress);
            if (string.IsNullOrWhiteSpace(viewModel.UserName) || viewModel.UserName.Length < 3)
                throw new ArgumentException("������������ �������.");
            if (viewModel.Password != PasswordShow2.Text)
                throw new InvalidDataException("������ �� ���������.");
            if (!StrongPasswordChecker.PasswordCheck(viewModel.Password))
                throw new InvalidDataException("������� ������ ������! ��������:\n1) �� ����� 8 ���������;\n2) ���� ��������� �����;" +
                    "\n3) ���� �������� �����;\n4) ���� �����;\n5) ���� ����������� ������;\n6) ��� ��������;");
            BlockControls(false);
            await Task.Delay(3000);
            string serial = JsonConvert.SerializeObject(viewModel);
            await DisplayAlert("Success", serial, "OK");

            Preferences.Set("userId", Guid.NewGuid().ToString());
            Preferences.Set("userName", UserName.Text);
            Preferences.Set("emailAddress", EmailSend.Text);
            Preferences.Set("password", PasswordShow1.Text);
            await Navigation.PopModalAsync();
            //Application.Current.MainPage = new AppShell();
        }
        catch (FormatException)
        {
            EmailSend.TextColor = Color.Parse("#ED1C24");
        }
        catch (ArgumentNullException)
        {
            await DisplayAlert("������!", "��� ����������� ��������� �������������� ����� ����������� �����.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("������!", ex.Message, "OK");
        }
        BlockControls(true);
    }

    private async void UsersAgreement_Tapped(object sender, TappedEventArgs e) => await
       Navigation.PushModalAsync(new TermsOfUseAgreePage(true));

    private void ShowPassword_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (showPassword.IsChecked)
        {
            PasswordShow1.IsPassword = false;
            PasswordShow2.IsPassword = false;
            return;
        }
        PasswordShow1.IsPassword = true;
        PasswordShow2.IsPassword = true;
    }

    private void EmailSend_TextChanged(object sender, TextChangedEventArgs e) =>
        EmailSend.TextColor = Color.Parse("#ffffff");

    private async void BackButton_Clicked(object sender, EventArgs e) => await Navigation.PopModalAsync();

    private void BlockControls(bool isBusy)
    {
        BackButton.IsEnabled = isBusy;
        UserName.IsReadOnly = !isBusy;
        EmailSend.IsReadOnly = !isBusy;
        PasswordShow1.IsReadOnly = !isBusy;
        PasswordShow2.IsReadOnly = !isBusy;
        Register.IsEnabled = isBusy;
        UserAgrrement.IsEnabled = isBusy;
    }
}