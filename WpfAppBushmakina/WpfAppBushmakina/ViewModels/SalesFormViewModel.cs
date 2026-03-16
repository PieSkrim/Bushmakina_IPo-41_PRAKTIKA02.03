using System.Windows;
using Bushmakina.Domain.Entities;
using Bushmakina.Domain.Managers;

namespace Bushmakina.WPF.ViewModels
{
    public class SalesFormViewModel : ObservableObject
    {
        private readonly IPartnerManager _manager;
        private string _productName = string.Empty;
        private int _quantity;
        private DateTime _saleDate = DateTime.Now;

        public int PartnerId { get; set; }
        public string ProductName
        {
            get => _productName;
            set => SetField(ref _productName, value);
        }

        public int Quantity
        {
            get => _quantity;
            set => SetField(ref _quantity, value);
        }

        public DateTime SaleDate
        {
            get => _saleDate;
            set => SetField(ref _saleDate, value);
        }

        public SalesFormViewModel(IPartnerManager manager)
        {
            _manager = manager;
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || Quantity <= 0)
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var sale = new SalesRecordEntity
            {
                PartnerId = PartnerId,
                ProductName = ProductName,
                Quantity = Quantity,
                SaleDate = SaleDate.ToUniversalTime()
            };

            await _manager.RecordSaleAsync(sale);
            return true;
        }
    }
}