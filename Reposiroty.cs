using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Banking_System_Prototype
{
    internal class Repository
    {
        private readonly List<Client> clients = new List<Client>();
        public void AddClient(string LastName, string FirstName, string PhoneNumber)
        {
            clients.Add(new Client(SetId(), LastName, FirstName, PhoneNumber));
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

        public void Save()
        {
            if (File.Exists("Clients.json"))
            {
                File.Delete("Clients.json");
            }
            JArray clientsArray = new JArray();
            JObject mainTree = new JObject
            {
                ["ok"] = true
            };
            foreach (var client in clients)
            {
                JObject clientObj = new JObject
                {
                    ["id"] = client.Id,
                    ["last_name"] = client.LastName,
                    ["first_name"] = client.FirstName,
                    ["phone_number"] = client.PhoneNumber,
                    ["bank_account"] = clients[client.Id - 1].GetBank_Accounts()
                };
                clientsArray.Add(clientObj);
                
            }

            mainTree["clients"]=clientsArray;
            string json = mainTree.ToString();
            File.AppendAllText("Clients.json", json);

        }

    }
}
