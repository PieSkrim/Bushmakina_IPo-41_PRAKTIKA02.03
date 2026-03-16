using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Bushmakina.Domain.Entities;
using Bushmakina.Domain.Managers;
using Bushmakina.Domain.Calculators;
using Bushmakina.WPF.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Bushmakina.WPF.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IPartnerManager _manager;
        private PartnerEntity? _selectedPartner;
        private ObservableCollection<PartnerEntity> _partners = new();
        private ObservableCollection<SalesRecordEntity> _salesHistory = new();
        private int _currentBonus = 0;
        private string _statusText = "Готов к работе";

        public ObservableCollection<PartnerEntity> Partners
        {
            get => _partners;
            set => SetField(ref _partners, value);
        }

        public ObservableCollection<SalesRecordEntity> SalesHistory
        {
            get => _salesHistory;
            set => SetField(ref _salesHistory, value);
        }

        public PartnerEntity? SelectedPartner
        {
            get => _selectedPartner;
            set
            {
                if (SetField(ref _selectedPartner, value))
                {
                    _ = LoadPartnerInfoAsync();
                }
            }
        }

        public int CurrentBonus
        {
            get => _currentBonus;
            set => SetField(ref _currentBonus, value);
        }

        public string StatusText
        {
            get => _statusText;
            set => SetField(ref _statusText, value);
        }

        public ICommand CreatePartnerCommand { get; }
        public ICommand ModifyPartnerCommand { get; }
        public ICommand DeletePartnerCommand { get; }
        public ICommand AddSaleCommand { get; }
        public ICommand CloseCommand { get; }

        public MainViewModel(IPartnerManager manager)
        {
            _manager = manager;
            CreatePartnerCommand = new DelegateCommand(async _ => await CreatePartnerAsync());
            ModifyPartnerCommand = new DelegateCommand(async _ => await ModifyPartnerAsync(), _ => SelectedPartner != null);
            DeletePartnerCommand = new DelegateCommand(async _ => await DeletePartnerAsync(), _ => SelectedPartner != null);
            AddSaleCommand = new DelegateCommand(async _ => await AddSaleAsync(), _ => SelectedPartner != null);
            CloseCommand = new DelegateCommand(_ => Application.Current.Shutdown());
        }

        public async Task LoadPartnersAsync()
        {
            try
            {
                var list = await _manager.LoadAllPartnersAsync();

                Partners.Clear();
                foreach (var p in list)
                {
                    var total = await _manager.CalculateTotalSalesAsync(p.Id);
                    p.CurrentBonus = BonusCalculator.CalculateBonusPercent(total);
                    Partners.Add(p);
                }
                StatusText = $"Загружено партнёров: {Partners.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}\n\nПроверите подключение к БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadPartnerInfoAsync()
        {
            if (SelectedPartner == null)
            {
                SalesHistory.Clear();
                CurrentBonus = 0;
                return;
            }

            try
            {
                var history = await _manager.LoadSalesHistoryAsync(SelectedPartner.Id);
                SalesHistory.Clear();
                foreach (var s in history) SalesHistory.Add(s);

                CurrentBonus = await _manager.GetBonusPercentAsync(SelectedPartner.Id);
                StatusText = $"Партнёр: {SelectedPartner.Name}, Бонус: {CurrentBonus}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CreatePartnerAsync()
        {
            var vm = App.Services.GetRequiredService<PartnerFormViewModel>();
            await vm.PrepareForNewAsync();
            var window = new PartnerFormWindow(vm);
            if (window.ShowDialog() == true)
            {
                await LoadPartnersAsync();
            }
        }

        private async Task ModifyPartnerAsync()
        {
            if (SelectedPartner == null) return;
            var vm = App.Services.GetRequiredService<PartnerFormViewModel>();
            await vm.PrepareForEditAsync(SelectedPartner.Id);
            var window = new PartnerFormWindow(vm);
            if (window.ShowDialog() == true)
            {
                await LoadPartnersAsync();
            }
        }

        private async Task DeletePartnerAsync()
        {
            if (SelectedPartner == null) return;
            if (MessageBox.Show($"Удалить '{SelectedPartner.Name}'?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await _manager.RemovePartnerAsync(SelectedPartner.Id);
                await LoadPartnersAsync();
            }
        }

        private async Task AddSaleAsync()
        {
            if (SelectedPartner == null) return;

            var vm = App.Services.GetRequiredService<SalesFormViewModel>();
            vm.PartnerId = SelectedPartner.Id;
            var window = new SalesEntryWindow(vm);
            if (window.ShowDialog() == true)
            {
                await LoadPartnerInfoAsync();
                await LoadPartnersAsync();
            }
        }
    }
}