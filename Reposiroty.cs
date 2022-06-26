using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System_Prototype
{
    internal class Repository
    {
        private List<Client> clients = new List<Client>();
        public void AddClient(string LastName, string FirstName, string PhoneNumber)
        {
            clients.Add(new Client(SetId(), LastName, FirstName, PhoneNumber));
        }

        /// <summary>
        /// Открывает банковский счет
        /// </summary>
        /// <param name="id">Id клинта </param>
        /// <param name="money">Деньги</param>
        public void OpenBankAccount(int id, int money)
        {
            clients[id - 1].AddBankAccount(money);
        }

        /// <summary>
        /// Закрывает банковский счет
        /// </summary>
        /// <param name="clientId">Id клиента</param>
        /// <param name="accountId">Id банковского счета</param>
        public void CloseBankAccount(int clientId, int accountId)
        {
            clients[clientId - 1].RemoveBankAccount(accountId);
        }

        /// <summary>
        /// Устанавливает Id клиенту
        /// </summary>
        /// <returns>Id</returns>
        private int SetId()
        {
            if (clients.Count == 0)
                return 1;
            return clients.Count + 1;
        }

        /// <summary>
        /// Список банковских считов у клиента
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <returns>Список банковских считов у клиента</returns>
        public List<Bank_Account> ShowBankAccounts(int id)
        {
            return clients[id - 1].ShowAccount();
        }

        /// <summary>
        /// Показываает список клиентов
        /// </summary>
        /// <returns>Список клиентов</returns>
        public List<Client> ShowClients()
        {
            return clients;
        }
    }
}
