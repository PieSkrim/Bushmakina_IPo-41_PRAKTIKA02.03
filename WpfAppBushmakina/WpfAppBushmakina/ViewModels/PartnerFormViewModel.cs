using System.Collections.ObjectModel;
using System.Windows;
using Bushmakina.Domain.Entities;
using Bushmakina.Domain.Managers;

namespace Bushmakina.WPF.ViewModels
{
    public class PartnerFormViewModel : ObservableObject
    {
        private readonly IPartnerManager _manager;
        private PartnerEntity _partner = new();
        private ObservableCollection<PartnerTypeEntity> _types = new();
        private int _selectedTypeId;

        public PartnerEntity Partner
        {
            get => _partner;
            set => SetField(ref _partner, value);
        }

        public ObservableCollection<PartnerTypeEntity> Types
        {
            get => _types;
            set => SetField(ref _types, value);
        }

        public int SelectedTypeId
        {
            get => _selectedTypeId;
            set => SetField(ref _selectedTypeId, value);
        }

        public PartnerFormViewModel(IPartnerManager manager)
        {
            _manager = manager;
        }

        public async Task PrepareForEditAsync(int id)
        {
            await LoadTypesAsync();
            var p = await _manager.GetPartnerAsync(id);
            if (p != null)
            {
                Partner = p;
                SelectedTypeId = p.TypeId;
            }
        }

        public async Task PrepareForNewAsync()
        {
            await LoadTypesAsync();
            Partner = new PartnerEntity();
            SelectedTypeId = Types.FirstOrDefault()?.Id ?? 0;
        }

        private async Task LoadTypesAsync()
        {
            var list = await _manager.LoadPartnerTypesAsync();
            Types.Clear();
            foreach (var t in list) Types.Add(t);
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Partner.Name))
            {
                MessageBox.Show("Введите наименование", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Partner.TypeId = SelectedTypeId;
            await _manager.StorePartnerAsync(Partner);
            return true;
        }
    }
}