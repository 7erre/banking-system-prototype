using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows;

namespace Banking_System_Prototype
{
    internal class Client
    {
        private readonly List<Bank_Account> bank_accounts = new List<Bank_Account>();

        /// <summary>
        /// Id клиента
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Проверяет существует ли счет с таким Id
        /// </summary>
        /// <param name="id">Id счета</param>
        /// <returns></returns>
        public bool CheckId(int id)
        {
            foreach (var item in bank_accounts)
            {
                if (item.Id == id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Устанавливает Id банковского счета
        /// </summary>
        /// <returns>Id</returns>
        private int SetBankAccountId()
        {
            if (bank_accounts.Count == 0)
                return 1;
            int i = 0;
            foreach (var el in bank_accounts)
            {
                i++;
                if (el.Id != i)
                    return i;
            }
            return bank_accounts.Count + 1;

        }

        /// <summary>
        /// Открывает банковский счет у клиента
        /// </summary>
        /// <param name="money">Деньги</param>
        public void AddBankAccount( string type)
        {
            if (bank_accounts.Count == 2)
            {
                MessageBox.Show("Больше 2-х счетов открывать нельзя");
                return;
            }
            foreach (var item in bank_accounts)
            {
                if (item.Type == type)
                {
                    MessageBox.Show("Счет с таким типом уже существует, выберите другой");
                    return;
                }
            }
            bank_accounts.Add(new Bank_Account(SetBankAccountId(), 0, type));
        }

        /// <summary>
        /// Закрывает банковский счет у клиента
        /// </summary>
        /// <param name="id">Id счёта</param>
        public void RemoveBankAccount(int id)
        {
            foreach (var el in bank_accounts)
            {
                if (el.Id == id)
                {
                    bank_accounts.RemoveAt(bank_accounts.IndexOf(el));
                    break;
                }
            }
        }

        /// <summary>
        /// Загружает банковские счета пользователя из Json файла
        /// </summary>
        /// <param name="json">bank_account</param>
        public void LoadBankAccounts(JToken json)
        {
            foreach (var item in json)
            {
                bank_accounts.Add(new Bank_Account(int.Parse(item["id"].ToString()),int.Parse(item["money"].ToString()), item["type"].ToString()));
            }
        }

        /// <summary>
        /// Получает банковские счета в виде JArray
        /// </summary>
        /// <returns>JArray</returns>
        public JArray GetBank_Accounts()
        {
            JArray accountsArray = new JArray();
            foreach (var account in bank_accounts)
            {
                JObject accountObj = new JObject()
                {
                    ["id"] = account.Id,
                    ["money"] = account.Money,
                    ["type"] = account.Type
                };
                accountsArray.Add(accountObj);
            }
            return accountsArray;
        }

        /// <summary>
        /// Добавляет деньги на счет
        /// </summary>
        /// <param name="id"></param>
        /// <param name="money"></param>
        public void AddMoney(int id, int money)
        {
            bank_accounts[id-1].Money += money;
        }

        /// <summary>
        /// Проверка и вычет денег со счета
        /// </summary>
        /// <param name="id"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        public bool Transfer(int id, int money)
        {
            if (bank_accounts[id-1].Type == "Депозитный")
                return true;
            if (bank_accounts[id-1].Money - money < 0)
                return true;
            bank_accounts[id-1].Money -= money;
            return false;
        }

        public List<Bank_Account> ShowAccount()
        {
            return bank_accounts;
        }

        public Client(int id, string lastName, string firstName, string phoneName)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            PhoneNumber = phoneName;
        }
    }
}
