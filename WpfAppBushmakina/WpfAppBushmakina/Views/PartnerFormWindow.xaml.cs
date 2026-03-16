using System.Windows;
using Bushmakina.WPF.ViewModels;

namespace Bushmakina.WPF.Views
{
    public partial class PartnerFormWindow : Window
    {
        private readonly PartnerFormViewModel _vm;

        public PartnerFormWindow(PartnerFormViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (await _vm.SaveAsync())
            {
                DialogResult = true;
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}