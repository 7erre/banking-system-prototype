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
        private readonly Repository repository;
        public MainWindow()
        {
            repository = new Repository();
            repository.Load();
            InitializeComponent();
            lvClients.ItemsSource = repository.ShowClients();
        }

        private void lvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvAccounts.ItemsSource = repository.ShowBankAccounts((lvClients.SelectedItem as Client).Id);
        }

        private void ButtonAddAccount_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Money.Text, out _)) return;

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

        private void ButtonTransferAmount_Click(object sender, RoutedEventArgs e)
        {

            if (!repository.MoneyTransfer(FromClientId.Text, FromAccountId.Text, ToClientId.Text, ToAccountId.Text, TransferAmount.Text))
            {
                MessageBox.Show("Ошибка в заполнение данных или нехватка денег на счету");
                return;
            }
            MessageBox.Show("Перевод успешно выполнен!");
            lvAccounts.Items.Refresh();
        }
    }
}
