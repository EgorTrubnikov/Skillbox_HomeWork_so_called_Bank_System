using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_HomeWork_13._1
{
   public class Organization : Client, IOperations, IEquatable<Organization>
    {
        /// <summary>
        /// Наименование юрлица
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        ///Число сотрудников
        /// </summary>
        public int EmployeeCount { get; set; }

        /// <summary>
        /// Годовой доход организации
        /// </summary>
        public int YearIncome { get; set; }

        public Organization
            (decimal Deposit,
            decimal InterestRate,
            string OpenLog,
            decimal StartDepositForPercents,
            string CapitalizationPercents,
            string OrganizationName,
            int EmployeeCount,
            int YearIncome) :
            base(Deposit, InterestRate, OpenLog, StartDepositForPercents, CapitalizationPercents)
        {
            this.OrganizationName = OrganizationName;
            this.EmployeeCount = EmployeeCount;
            this.YearIncome = YearIncome;
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

        public bool Equals(Organization other)
        {
            return this.OrganizationName == other.OrganizationName && this.Deposit == other.Deposit;
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


        public class SortByOrgName : IComparer<Organization>
        {
            public int Compare(Organization x, Organization y)
            {
                Organization X = (Organization)x;
                Organization Y = (Organization)y;
                return String.Compare(X.OrganizationName, Y.OrganizationName);
            }


        }

        public class SortByOrgDeposit : IComparer<Organization>
        {
            public int Compare(Organization x, Organization y)
            {
                Organization X = (Organization)x;
                Organization Y = (Organization)y;
                if (X.Deposit == Y.Deposit) return 0;
                else if (X.Deposit > Y.Deposit) return 1;
                else return -1;
            }
        }

        public class SortByOrgInterestRate : IComparer<Organization>
        {
            public int Compare(Organization x, Organization y)
            {
                Organization X = (Organization)x;
                Organization Y = (Organization)y;
                if (X.InterestRate == Y.InterestRate) return 0;
                else if (X.InterestRate > Y.InterestRate) return 1;
                else return -1;
            }
        }

        static public Organization operator +(Organization X, decimal Sum)
        {
            Organization Y = new Organization(X.Deposit + Sum, X.InterestRate, X.logFile[0], X.StartDepositForPercents, X.CapitalizationPercents, X.OrganizationName, X.EmployeeCount, X.YearIncome);
            for(int i = 1; i < X.logFile.Count; i++)
            {
                Y.logFile.Add(X.logFile[i]);
            }
            return Y;
            
        }
    }
}
