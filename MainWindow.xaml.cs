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
using NLog;

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
            repository.ClientAdded += ClientAdded;
        }

        private void LvClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lvAccounts.ItemsSource = repository.ShowBankAccounts((lvClients.SelectedItem as Client).Id);
        }

        private void ClientAdded(string Msg)
        {
            MessageBox.Show(Msg);
        }

        private void ButtonAddAccount_Click(object sender, RoutedEventArgs e)
        {

            if (lvClients.SelectedItem == null) return;

            if (string.IsNullOrWhiteSpace(comboBox.Text)) { MessageBox.Show("Выберите тип счета"); return; }

            repository.OpenBankAccount((lvClients.SelectedItem as Client).Id,comboBox.Text);
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
                MessageBox.Show("Ошибка в заполнение данных или нехватка денег на счету или вы пытаетесь вывести деньги с депозитного счета");
                return;
            }
            repository.Save();
            lvAccounts.Items.Refresh();
        }

        private void ButtonTopUpAccount_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(Money.Text, out _)) return;

            if (lvClients.SelectedItem == null || lvAccounts.SelectedItem == null) return;

            repository.TopUpAccount((lvClients.SelectedItem as Client).Id,(lvAccounts.SelectedItem as Bank_Account).Id, int.Parse(Money.Text));
            repository.Save();
            lvAccounts.Items.Refresh();
        }
    }
}
