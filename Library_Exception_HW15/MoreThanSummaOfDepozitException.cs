using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Exception_HW15
{
    public class MoreThanSummaOfDepozitException: Exception
    {
        // Исключение, вызываемое при вводе суммы для снятия или перевода с депозита, которая больше текущей суммы депозита
        public MoreThanSummaOfDepozitException(string Message):base(Message)
        {

        }
    }

    public class InvalidIdentityException: Exception
    {
        // Исключение, вызываемое при выборе одного и того же клиента отправителем и получателем
        public InvalidIdentityException(string Message):base(Message)
        {

        }
    }

    public class NoSenderSelectedException: Exception
    {
        // Исключение, вызываемое при выборе клиента получателя прежде выбора клиента отправителя перевода
        public NoSenderSelectedException(string Message) : base(Message)
        {

        }
    }

    public class SummaEqualsOrLessZeroException: Exception
    {
        // Исключение, вызываемое при вводе суммы перевода равной или меньше нуля
        public SummaEqualsOrLessZeroException(string Message) : base(Message)
        {

        }
    }

    public class SaveOrLoadDataEcxeption: Exception
    {
        // Исключение, вызываемое при попытке сохранить не загруженную или сгенерированную базу, при попытке загрузить не сгенерированную базу
        public SaveOrLoadDataEcxeption(string Message) : base(Message)
        {

        }
    }

}
