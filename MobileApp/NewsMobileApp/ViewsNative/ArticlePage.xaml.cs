using NewsMobileApp.TempServices;

namespace NewsMobileApp.ViewsNative;

public partial class ArticlePage : ContentPage
{
    private readonly Guid _articleId;
    private readonly IRequestsService _requestsService;

	public ArticlePage(Guid articleId)
	{
		InitializeComponent();
        _articleId = articleId;
        RootComponentControl.Parameters = new Dictionary<string, object> { { "articleid", _articleId } };
        _requestsService = Application.Current.Handler.MauiContext
            .Services.GetService<IRequestsService>();
    }

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string action = await DisplayActionSheet("������� ������", "������",
                "�������", "�� �������, ��� ������ ������� ������?");
            if (action == "�������")
            {
                if (!await _requestsService.DeleteArticleAsync(_articleId))
                    throw new Exception("��������� ����������� ������ �������!");
                await DisplayAlert("�������", "������ �������!", "OK");
                await Navigation.PopAsync();
            }
        }
        catch(Exception ex) 
        {
            await DisplayAlert("������!", $"��� �������� ������ ��������� ������: {ex.Message}", "OK");
        }
    }

    private async void EditButton_Clicked(object sender, EventArgs e) =>
        await Navigation.PushAsync(new ArticleDetailPage(_articleId));
}