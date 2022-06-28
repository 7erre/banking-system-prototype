using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Banking_System_Prototype
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Repository repository;
        public MainWindow()
        {
            repository = new Repository();
            repository.AddClient("badin", "roma", "891121212");
            repository.OpenBankAccount(1, 10000);
            repository.OpenBankAccount(1, 20000);
            repository.OpenBankAccount(1, 30000);

            InitializeComponent();
            lvClients.ItemsSource = repository.ShowClients();
        }

        private void lvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvAccounts.ItemsSource = repository.ShowBankAccounts((lvClients.SelectedItem as Client).Id);
        }

        private void ButtonAddAccount_Click(object sender, RoutedEventArgs e)
        {

            if (!int.TryParse(Money.Text, out int money)) return;

            if (lvClients.SelectedItem == null) return;

            repository.OpenBankAccount((lvClients.SelectedItem as Client).Id, int.Parse(Money.Text));
            repository.Save();
            lvAccounts.Items.Refresh();
        }

        private void ButtonRemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (lvAccounts.SelectedItem == null) return;
            repository.CloseBankAccount((lvClients.SelectedItem as Client).Id, (lvAccounts.SelectedItem as Bank_Account).Id);
            repository.Save();
            lvAccounts.Items.Refresh();
        }

        private void ButtonAddClient_Click(object sender, RoutedEventArgs e)
        {
            repository.AddClient(LastName.Text, FirstName.Text, PhoneNumber.Text);
            repository.Save();
            lvClients.Items.Refresh();
        }
    }
}
