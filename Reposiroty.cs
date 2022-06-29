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
    internal class Repository<T>
    {
        private readonly List<Client> clients = new List<Client>();

        /// <summary>
        /// Добавляет клиента
        /// </summary>
        /// <param name="LastName">Фамилия</param>
        /// <param name="FirstName">Имя</param>
        /// <param name="PhoneNumber">Номер телефона</param>
        public void AddClient(string LastName, string FirstName, string PhoneNumber)
        {
            clients.Add(new Client(SetId(), LastName, FirstName, PhoneNumber));
        }

        /// <summary>
        /// Проверка Id клиента
        /// </summary>
        /// <param name="id">Id клмента</param>
        /// <returns></returns>
        public bool CheckClientId(string id)
        {
            if (!int.TryParse(id, out _))
                return false;
            foreach (var item in clients)
            {
                if (item.Id == int.Parse(id))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка Id счета у клиента
        /// </summary>
        /// <param name="clientId">Id клиента</param>
        /// <param name="accountId">Id счета</param>
        /// <returns></returns>
        public bool CheckAccountId(int clientId, string accountId)
        {
            if (!int.TryParse(accountId, out _))
                return false;
            return clients[clientId - 1].CheckId(int.Parse(accountId));
        }

        /// <summary>
        /// Проверка и перевод денег
        /// </summary>
        /// <param name="fromClientId">Id отправщика</param>
        /// <param name="fromAccountId">Id счета отправщкиа</param>
        /// <param name="toClientId">Id получателя</param>
        /// <param name="toAccountId">Id счета получателя</param>
        /// <param name="money">Сумма перевода</param>
        /// <returns></returns>
        public bool MoneyTransfer(T fromClientId, T fromAccountId, T toClientId, T toAccountId, T money)
        {
            if (!CheckClientId(fromClientId.ToString()) || !CheckClientId(toClientId.ToString()))
                return false;
            if (!CheckAccountId(int.Parse(fromClientId.ToString()), fromAccountId.ToString()) || !CheckAccountId(int.Parse(toClientId.ToString()), toAccountId.ToString()))
                return false;
            if (!int.TryParse(money.ToString(), out _))
                return false;
            if (!clients[int.Parse(fromClientId.ToString()) - 1].Transfer(int.Parse(fromAccountId.ToString()), int.Parse(money.ToString())))
            {
                clients[int.Parse(toClientId.ToString()) -1].AddMoney(int.Parse(toAccountId.ToString()),int.Parse(money.ToString()));
                return true;
            }
            return false;
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
        public void OpenBankAccount(int id, int money, string type)
        {
            clients[id - 1].AddBankAccount(money,type);
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

        /// <summary>
        /// Загрузка клиентов в List
        /// </summary>
        public void Load()
        {
            if (!File.Exists("Clients.json"))
                return;
            string json = File.ReadAllText("Clients.json");
            if (!Convert.ToBoolean(JObject.Parse(json)["ok"]))
                return;

            var loadedClients=JObject.Parse(json)["clients"].ToList();
            foreach (var item in loadedClients)
            {
                clients.Add(new Client(int.Parse(item["id"].ToString()), item["last_name"].ToString(), item["first_name"].ToString(), item["phone_number"].ToString()));
                clients[int.Parse(item["id"].ToString()) - 1].LoadBankAccounts(item["bank_account"]);
            }
        }

        /// <summary>
        /// Сохранение клиентов в json файл
        /// </summary>
        public void Save()
        {
            if (File.Exists("Clients.json"))
                File.Delete("Clients.json");

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
