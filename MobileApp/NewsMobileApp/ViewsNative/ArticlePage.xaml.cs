using NewsMobileApp.Models;
using NewsMobileApp.TempServices;

namespace NewsMobileApp.ViewsNative;

public partial class ArticlePage : ContentPage
{
    private readonly Article _article;
    private readonly IRequestsService _requestsService;

	public ArticlePage(Article article)
	{
		InitializeComponent();
        _article = article;
        RootComponentControl.Parameters = new Dictionary<string, object> { { "articleid", _article.ArticleId } };
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
                if (!await _requestsService.DeleteArticleAsync(_article.ArticleId))
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
        await Navigation.PushAsync(new ArticleDetailPage(_article.ArticleId));

    private async void Share_Clicked(object sender, EventArgs e) => await Share.Default.RequestAsync(
        new ShareTextRequest {
            Title = _article.Title,
            Text = $"{_article.Title}\n{_article.Subtitle}...\n\n��� � ������ ������ � ���������� Star News!",
            Uri = _article.Image
        });
}