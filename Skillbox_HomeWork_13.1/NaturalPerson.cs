using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_HomeWork_13._1
{
    class NaturalPerson : Client, IOperations, IEquatable<NaturalPerson>
    {
        /// <summary>
        /// Фамилия клиента
        /// </summary>
        public string LastName { get; set; }


        /// <summary>
        /// Имя клиента
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Возраст клиента
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Официальный доход клиента
        /// </summary>
        public int Income { get; set; }

        public NaturalPerson
            (decimal Deposit,
            decimal InterestRate,
            string OpenLog,
            decimal StartDepositForPercents,
            string CapitalizationPercents,
            string LastName,
            string FirstName,
            int Age,
            int Income) :
            base(Deposit, InterestRate, OpenLog, StartDepositForPercents, CapitalizationPercents)
        {
            this.LastName = LastName;
            this.FirstName = FirstName;
            this.Age = Age;
            this.Income = Income;
        }

        /// <summary>
        /// Метдо снятия денег со счёта
        /// </summary>
        /// <param name="dep">Текущая сумма на счёте</param>
        /// <param name="sum">Сумма к снятию со счёта</param>
        public void Withdrawal(decimal dep, decimal sum)
        {
            this.Deposit = dep - sum;
        }

        /// <summary>
        /// Метод пополнения счёта
        /// </summary>
        /// <param name="dep">Текущая сумма на счёте</param>
        /// <param name="sum">Сумма к зачислению на счёт</param>
        public void Depositing(decimal dep, decimal sum)
        {
            this.Deposit = dep + sum;
        }



        public bool Equals(NaturalPerson other)
        {
            return this.FirstName == other.FirstName && this.LastName == other.LastName && this.Age == other.Age;
        }

        public override void PercentYear(decimal InterestRate, decimal Deposit, decimal StartDepositForPercents, string CapitalizationPercents)
        {
            if (CapitalizationPercents == "yes")
            {
                this.Deposit = Deposit + ((Deposit / 100) * (InterestRate / 12));
            }
            else
            {
                this.Deposit = Deposit + ((StartDepositForPercents / 100) * (InterestRate / 12));
            }

        }


        public class SortByPersonLastName : IComparer<NaturalPerson>
        {
            public int Compare(NaturalPerson x, NaturalPerson y)
            {
                NaturalPerson X = (NaturalPerson)x;
                NaturalPerson Y = (NaturalPerson)y;
                return String.Compare(X.LastName, Y.LastName);
            }
        }

        public class SortByPersonDeposit : IComparer<NaturalPerson>
        {
            public int Compare(NaturalPerson x, NaturalPerson y)
            {
                NaturalPerson X = (NaturalPerson)x;
                NaturalPerson Y = (NaturalPerson)y;
                if (X.Deposit == Y.Deposit) return 0;
                else if (X.Deposit > Y.Deposit) return 1;
                else return -1;
            }
        }

        public class SortByPersonInterestRate : IComparer<NaturalPerson>
        {
            public int Compare(NaturalPerson x, NaturalPerson y)
            {
                NaturalPerson X = (NaturalPerson)x;
                NaturalPerson Y = (NaturalPerson)y;
                if (X.InterestRate == Y.InterestRate) return 0;
                else if (X.InterestRate > Y.InterestRate) return 1;
                else return -1;

            }
        }




    }
}
