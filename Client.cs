using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System_Prototype
{
    internal class Client
    {
        private List<Bank_Account> bank_accounts = new List<Bank_Account>();

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
        /// Открывает банковский счет у клиента
        /// </summary>
        /// <param name="money">Деньги</param>
        public void AddBankAccount(int money)
        {
            bank_accounts.Add(new Bank_Account(SetBankAccountId(), money));
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
        /// Устанавливает Id банковского счета
        /// </summary>
        /// <returns>Id</returns>
        private int SetBankAccountId()
        {
            if (bank_accounts.Count == 0)
                return 1;
            int i = 0;
            //bank_accounts.Sort();   // Сделать сортировку по Id
            foreach (var el in bank_accounts)
            {
                i++;
                if (el.Id != i)
                    return i;
            }
            return bank_accounts.Count + 1;

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
