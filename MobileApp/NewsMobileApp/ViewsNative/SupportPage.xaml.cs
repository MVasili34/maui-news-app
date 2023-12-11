using NewsMobileApp.ViewModels;

namespace NewsMobileApp.ViewsNative;

public partial class SupportPage : ContentPage
{
    private readonly EmailViewModel _emailViewModel;
    private readonly string[] Themes = new[] {
            "�������� �� ������",
            "�������� � ������� �������",
            "������� ��������������",
            "������"
    };

    public SupportPage(EmailViewModel emailViewModel)
	{
		InitializeComponent();
        ComboBox1.ItemsSource = Themes;
        ComboBox1.SelectedIndex = 0;
        _emailViewModel = emailViewModel;
        BindingContext = _emailViewModel;
    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {
        try
        {
            _emailViewModel.Subject = Themes[ComboBox1.SelectedIndex];
            if (string.IsNullOrEmpty(_emailViewModel.Body))
            {
                await DisplayAlert("������", $"������ ��������� ������", "OK");
                return;
            }

            EmailMessage message = new()
            {
                To = new List<string>(_emailViewModel.To.Split(";")),
                Subject = _emailViewModel.Subject,
                Body = _emailViewModel.Body ,
            };

            message.Body += $"\n\n--\n��������� ������������: {Preferences.Get("userId", Guid.NewGuid().ToString())}" +
                $"\n ������������� ���������: {Guid.NewGuid()}";


            //await DisplayAlert("����������", $"��� ������ ��������� ����� ������ ���������. {_emailViewModel.Body}", "OK");
            await Email.Default.ComposeAsync(message);
        }
        catch (FeatureNotSupportedException ex)
        {
            await DisplayAlert("������", $"���������� �� �������������: {ex.Message}", "OK");
        }
        catch (Exception ex) 
        {
            await DisplayAlert("������", $"������: {ex.Message}", "OK");
        }
    }
}