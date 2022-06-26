using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking_System_Prototype
{
    internal class Bank_Account
    {
        public Bank_Account(int id, int money)
        {
            Id = id;
            Money = money;
        }

        /// <summary>
        /// Id счета
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Деньги
        /// </summary>
        public int Money { get; set; }
    }
}
