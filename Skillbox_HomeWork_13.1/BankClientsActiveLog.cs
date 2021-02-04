using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_HomeWork_13._1
{
    
    static class BankClientsActiveLog
    {

        /// <summary>
        /// Коллекция сообщений о операциях с депозитами клиентов
        /// </summary>
        public static ObservableCollection<string> log;

        static BankClientsActiveLog()
        {
            log = new ObservableCollection<string>();
        }

        /// <summary>
        /// Метод, используемый для подписки на событие BankLog, добавляет сообщение о операции с депозитом клиента в коллекцию log
        /// </summary>
        /// <param name="obj">Экземпляр клиента</param>
        /// <param name="arg">Текстовая строка наименование операции</param>
        public static void AddLog(Client obj, string arg)
        {
            if (arg == "Операция начисления процентов клиентам прошла успешно.")
            {
                log.Add($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} {arg}");
                return;
            }
            if (obj.GetType() == typeof(Organization))
            {
                log.Add($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} клиент: {(obj as Organization).OrganizationName} операция: {arg}");
            }
            if (obj.GetType() == typeof(NaturalPerson))
            {
                log.Add($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} клиент: Имя: {(obj as NaturalPerson).LastName}, возраст {(obj as NaturalPerson).Age} операция: {arg}");
            }
            

        }

    }
}
