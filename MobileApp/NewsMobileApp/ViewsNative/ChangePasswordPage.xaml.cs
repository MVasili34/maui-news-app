using NewsMobileApp.TempServices;
using NewsMobileApp.ViewModels;
using CryptographyTool;

namespace NewsMobileApp.ViewsNative;

public partial class ChangePasswordPage : ContentPage
{
    private ChangePasswordViewModel viewModel => BindingContext as ChangePasswordViewModel;
    private readonly IRequestsService _requestService;

	public ChangePasswordPage()
	{
		InitializeComponent();
        BindingContext = new ChangePasswordViewModel {
            EmailAddress = Preferences.Get("emailAddress", "Error!")
        };
        _requestService = Application.Current.Handler
           .MauiContext.Services.GetService<IRequestsService>();
    }



    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (viewModel.NewPassword != viewModel.NewPasswordVerify)
                throw new Exception("����� ������ ������!");
            if (!StrongPasswordChecker.PasswordCheck(viewModel.NewPassword))
                throw new Exception("������� ������ ������! ��������:\n1) �� ����� 8 ���������;\n2) ���� ��������� �����;" +
                    "\n3) ���� �������� �����;\n4) ���� �����;\n5) ���� ����������� ������;\n6) ��� ��������;");
            Submit.IsEnabled = false;
            if (!await _requestService.ChangeUserPasswordAsync(viewModel))
                throw new Exception("��������� ������ �������!");
            await DisplayAlert("�����!", "������ ������.", "OK");
            Preferences.Set("password", viewModel.NewPassword);
            await Navigation.PopAsync();
        }
        catch (HttpRequestException ex)
        {
            await DisplayAlert("������!", ex.Message, "OK");
        }
        catch (Exception ex) 
        {
            await DisplayAlert("������!", ex.Message, "OK");
        }
        Submit.IsEnabled = true;
    }

    private void ShowPassword_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (ShowPassword.IsChecked)
        {
            NewPassword1.IsPassword = false;
            NewPassword2.IsPassword = false;
            OldPassword.IsPassword = false;
            return;
        }
        NewPassword1.IsPassword = true;
        NewPassword2.IsPassword = true;
        OldPassword.IsPassword = true;
    }
}