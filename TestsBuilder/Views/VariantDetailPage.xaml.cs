using System.Collections.ObjectModel;
using TestsBuilder.Models;
using TestsBuilder.Services;

namespace TestsBuilder.Views;

public partial class VariantDetailPage : ContentPage
{
    private readonly IDbService _dbService;
    private User _user;
    private Variant _variant;
    public VariantDetailPage(Variant variant, IDbService dbService, User user)
    {
        InitializeComponent();
        BindingContext = this;
        _dbService = dbService;
        _variant = variant;
        _user = user; 
        titleLabel.Text = $"������� {variant.VariantNumber}";
        variantEntry.Text = variant.VariantExpression;
        expTitleLabel.Text = _dbService.GetExpressionById(variant.ExpId).Title;
        LoadAnswer();
    }

    private async void Tests_Clicked(object sender, EventArgs e)
    {
        // ���������� ������� ��� ������ "Settings"
        // ����� �� ������ ��������� ��������� �� �������� �������� ��� ������� ���������� ���� � �����������
        await Navigation.PushAsync(new TestsPage(_user, _dbService));
    }

    private async void Materials_Clicked(object sender, EventArgs e)
    {
        // ���������� ������� ��� ������ "Logout"
        // ����� �� ������ ��������� ����� ������������ �� ������� ������ � ������� �� �������� �����
    }

    private async void Profile_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage(_user, _dbService));
    }

    private async void AddAnswerButton_Clicked(object sender, EventArgs e)
    {
        var answer = variantAnswerEntry.Text;
        if (answer == "")
        {
            await DisplayAlert("������", "������� ���������� �����", "��");
        }
        else
        {
            Variant variant = new Variant
            {
                ExpId = _variant.ExpId,
                VariantAnswer = answer,
                VariantExpression = _variant.VariantExpression,
                VariantNumber = _variant.VariantNumber,
                VariantId = _variant.VariantId,
            };
            _dbService.UpdateVariant(variant);
            await DisplayAlert("�����", "����� ������� ��������", "��");

            var variantsPage = new VariantsPage(_dbService.GetExpressionById(_variant.ExpId), _dbService, _user);

            // ��������� �� �������� � ����������
            await Navigation.PushAsync(variantsPage);
        }
    }
    private async void LoadAnswer()
    {
        var hasAnswer = _variant.VariantAnswer;
        // ���������, ���� �� ��� �����
        if (hasAnswer !="")
        {
            variantAnswerButton.Text = "�������� �����";
            variantAnswerEntry.Text = hasAnswer;
        }
        else
        {
            variantAnswerButton.Text = "�������� �����";
        }
    }
}