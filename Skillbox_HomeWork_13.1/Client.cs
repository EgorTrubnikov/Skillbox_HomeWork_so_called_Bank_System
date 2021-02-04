using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_HomeWork_13._1
{
    abstract public class Client
    {

        /// <summary>
        /// Счёт клиента р/с для юрлица и л/с для физлица
        /// </summary>
        public decimal Deposit { get; set; }

        /// <summary>
        /// Процентная ставка по вкладу
        /// </summary>
        public decimal InterestRate { get; set; }

        /// <summary>
        /// Журнал операций по движению средств на депозите клиента
        /// </summary>
        public ObservableCollection<string> logFile { get; set; }

        /// <summary>
        /// Поле, хранящее сумму депозита на начало периода, для расчёта начисления процентов 
        /// </summary>
        public decimal StartDepositForPercents { get; set; }

        /// <summary>
        /// Поле, указывающее на тип начисление процента - обычные или с капитализацией
        /// </summary>
        public string CapitalizationPercents { get; set; }

        public Client(decimal Deposit, decimal InterestRate, string OpenLog, decimal StartDepositForPercents, string CapitalizationPercents)
        {
            this.Deposit = Deposit;
            this.InterestRate = InterestRate;
            logFile = new ObservableCollection<string> { OpenLog };
            this.StartDepositForPercents = StartDepositForPercents;
            this.CapitalizationPercents = CapitalizationPercents;
        }

        /// <summary>
        /// Метод для начисления процентов
        /// </summary>
        /// <param name="InterestRate">Процентная ставка</param>
        /// <param name="Deposit">Сумма куда начисляется процент</param>
        /// <param name="StartDepositForPercents">Сумма с которой начисляется процент</param>
        /// <param name="CapitalizationPercents">Тип начисления процента</param>
        abstract public void PercentYear(decimal InterestRate, decimal Deposit, decimal StartDepositForPercents, string CapitalizationPercents);
    }
}

