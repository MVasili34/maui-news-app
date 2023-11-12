using NewsMobileApp.TempServices;
using NewsMobileApp.ViewModels;

namespace NewsMobileApp.ViewsNative;

public partial class AdminUserSettingPage : ContentPage
{
	private readonly INewsService _newsService;
    private UserViewModel viewModel => BindingContext as UserViewModel;

	public AdminUserSettingPage(UserViewModel _viewModel)
	{
		InitializeComponent();
        _newsService = Application.Current.Handler
            .MauiContext.Services.GetService<INewsService>();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
#if WINDOWS
		PickerHeight.HeightRequest = 110;
#endif
        base.OnAppearing();

    }

    private async void Delete_Clicked(object sender, EventArgs e)
    {

    }

    private async void Submit_Clicked(object sender, EventArgs e)
    {

    }

    private void Update_Clicked(object sender, EventArgs e)
    {

    }
}