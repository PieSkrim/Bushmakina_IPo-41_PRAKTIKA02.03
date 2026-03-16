using System.Windows;
using Bushmakina.WPF.ViewModels;

namespace Bushmakina.WPF.Views
{
    public partial class SalesEntryWindow : Window
    {
        private readonly SalesFormViewModel _vm;

        public SalesEntryWindow(SalesFormViewModel vm)
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