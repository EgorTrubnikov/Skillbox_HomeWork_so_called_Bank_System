using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_HomeWork_13._1
{
    public interface IOperations
    {
        /// <summary>
        /// Метод снятия денег с депозита
        /// </summary>
        /// <param name="deposit">Текущий депозит</param>
        /// <param name="sum">Сумма к снятию</param>
        void Withdrawal(decimal dep, decimal sum);
        /// <summary>
        /// Метод пополнения счёта
        /// </summary>
        /// <param name="deposit">Текеущий депозит</param>
        /// <param name="sum">Сумма к пополнению</param>
        void Depositing(decimal dep, decimal sum);
    }
}
