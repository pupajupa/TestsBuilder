using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TestsBuilder.Models;
using TestsBuilder.Services;

namespace TestsBuilder.Views
{
    public partial class VariantsPage : ContentPage, INotifyPropertyChanged
    {
        private readonly IDbService _dbService;
        private TestsBuilder.Models.User _user;
        private ObservableCollection<Variant> _variants;
        private Exp _exp;
        public VariantsPage(Exp exp, IDbService dbService, TestsBuilder.Models.User user)
        {
            InitializeComponent();
            BindingContext = this;
            _dbService = dbService;
            _user = user;
            _exp = exp;
            titleLabel.Text = exp.Title;
            LoadVariants();
        }

        public ObservableCollection<Variant> Variants
        {
            get => _variants;
            set
            {
                _variants = value;
                OnPropertyChanged(nameof(Variants));
            }
        }

        private void LoadVariants()
        {
            IEnumerable<Variant> variants = new List<Variant>();
            if (_dbService.GetExpressionVariants(_exp.ExpId).Count() != 0)
            {
                variants = _dbService.GetExpressionVariants(_exp.ExpId);
            }

            Variants = new ObservableCollection<Variant>(variants);
            VariantsListView.ItemsSource = Variants;
        }

        private async void Tests_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TestsPage(_user, _dbService));
        }

        private async void Materials_Clicked(object sender, EventArgs e)
        {
            // Обработчик события для кнопки "Материалы"
            // Здесь вы можете выполнить навигацию на страницу материалов или открыть другую страницу
        }

        private async void Profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(_user, _dbService));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void ExpressionsItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Variant tappedVariant)
            {
                // Создаем новую страницу для отображения списка вариантов и передаем выбранный тест в качестве параметра
                var variantDetailPage = new VariantDetailPage(tappedVariant, _dbService, _user);

                // Переходим на страницу с вариантами
                await Navigation.PushAsync(variantDetailPage);
            }

            // Сбрасываем выделение элемента списка
            ((ListView)sender).SelectedItem = null;
        }
    }
}
