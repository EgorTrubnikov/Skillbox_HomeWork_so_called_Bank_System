using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using System.IO;
using Library_Exception_HW15;

namespace Skillbox_HomeWork_13._1
{
    public static class ExtMethod
    {
        public static void DepositUp(this Organization X, decimal sum)
        {
            X.Deposit += sum;
        }
    }
    

    delegate void ActiveLog(Client obj, string arg);
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        event ActiveLog BankLog;

       

        Random rnd;
        public MainWindow()
        {
            InitializeComponent();
            rnd = new Random();

            XListOrg.ItemsSource = DataBase.Org;
            XListPerson.ItemsSource = DataBase.Person;
            BankLog += BankClientsActiveLog.AddLog;
            XListBoxBankLog.ItemsSource = BankClientsActiveLog.log;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------Юридические лица-----------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        // Вывод данных по клиенту в текст блоки в карточке клиента
        private void XListOrg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XTbOrgName.Text = (XListOrg.SelectedValue as Organization).OrganizationName;
            XTbEmployeeCountValue.Text = (XListOrg.SelectedValue as Organization).EmployeeCount.ToString();
            XTbYearIncomeValue.Text = (XListOrg.SelectedValue as Organization).YearIncome.ToString("# ### ###");
            XTbDeposit.Text = (XListOrg.SelectedValue as Organization).Deposit.ToString("# ### ###");
            XTbInterestRate.Text = (XListOrg.SelectedValue as Organization).InterestRate.ToString();
            XLbLogFile.ItemsSource = (XListOrg.SelectedValue as Organization).logFile;
        }

        /// <summary>
        /// Обработка нажатия кнопки Пополнить счёт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XBtnTopUpAnAccount_Click(object sender, RoutedEventArgs e)
        {
            decimal sum;
            try
            {
                sum = Convert.ToDecimal(Interaction.InputBox("Введите сумму к зачислению на счёт.", "Пополнение счёта."));
                (XListOrg.SelectedValue as Organization).logFile.Add
                    ($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} Депозит пополнен на {sum} руб.");

                //(XListOrg.SelectedValue as Organization) += sum; - вот так не работает?????
                //DataBase.Org[0] += sum; - вот так работает

                // Вот метод расширения
                (XListOrg.SelectedValue as Organization).DepositUp(sum);
                // Вместо строчки ниже
                //(XListOrg.SelectedValue as Organization).Depositing((XListOrg.SelectedValue as Organization).Deposit, sum);
                BankLog?.Invoke((XListOrg.SelectedValue as Organization), $"Депозит пополнен на {sum} руб.");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            XListOrg.Items.Refresh();
            XTbDeposit.Text = (XListOrg.SelectedValue as Organization).Deposit.ToString("# ### ###");
        }

        /// <summary>
        /// Обработка нажатия кнопки Снять со счёта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XBtnWithdraw_Click(object sender, RoutedEventArgs e)
        {
            decimal sum;
            try
            {
                sum = Convert.ToDecimal(Interaction.InputBox("Введите сумму к снятию со счёта.", "Снятие со счёта."));
                if (sum > (XListOrg.SelectedValue as Organization).Deposit)
                {
                    throw new MoreThanSummaOfDepozitException("Сумма к снятию не может быть больше суммы депозита!!!");
                }
                (XListOrg.SelectedValue as Organization).logFile.Add
                    ($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} С депозита снято {sum} руб.");
                (XListOrg.SelectedValue as Organization).Withdrawal((XListOrg.SelectedValue as Organization).Deposit, sum);
                BankLog?.Invoke((XListOrg.SelectedValue as Organization), $"С депозита снято {sum} руб.");
            }
            catch(MoreThanSummaOfDepozitException ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            XListOrg.Items.Refresh();
            XTbDeposit.Text = (XListOrg.SelectedValue as Organization).Deposit.ToString("# ### ###");
        }


        //--------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------Физические лица-----------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        // Вывод данных по клиенту в текст блоки в карточке клиента
        private void XListPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            XTbPersonLastName.Text = (XListPerson.SelectedValue as NaturalPerson).LastName;
            XTbPersonFirstName.Text = (XListPerson.SelectedValue as NaturalPerson).FirstName;
            XTbPersonAge.Text = (XListPerson.SelectedValue as NaturalPerson).Age.ToString();
            XTbPersonMonthIncome.Text = (XListPerson.SelectedValue as NaturalPerson).Income.ToString("# ### ###");
            XTbPersonDeposit.Text = (XListPerson.SelectedValue as NaturalPerson).Deposit.ToString("# ### ###");
            XTbPersonInterestRate.Text = (XListPerson.SelectedValue as NaturalPerson).InterestRate.ToString();
            XListBoxPersonLogFile.ItemsSource = (XListPerson.SelectedValue as NaturalPerson).logFile;
        }
        /// <summary>
        /// Обработка нажатия кнопки Пополнить счёт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XBtnPersonTopUpAnAccount_Click(object sender, RoutedEventArgs e)
        {
            decimal sum;
            try
            {
                sum = Convert.ToDecimal(Interaction.InputBox("Введите сумму к зачислению на счёт.", "Пополнение счёта."));
                (XListPerson.SelectedValue as NaturalPerson).logFile.Add
                    ($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} Депозит пополнен на {sum} руб.");
                (XListPerson.SelectedValue as NaturalPerson).Depositing((XListPerson.SelectedValue as NaturalPerson).Deposit, sum);
                BankLog?.Invoke((XListPerson.SelectedValue as NaturalPerson), $"Депозит пополнен на {sum} руб.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            XListPerson.Items.Refresh();
            XTbPersonDeposit.Text = (XListPerson.SelectedValue as NaturalPerson).Deposit.ToString("# ### ###");
        }

        /// <summary>
        /// Обработка нажатия кнопки Снять со счёта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XBtnPersonWithdraw_Click(object sender, RoutedEventArgs e)
        {
            decimal sum;
            try
            {
                sum = Convert.ToDecimal(Interaction.InputBox("Введите сумму к снятию со счёта.", "Снятие со счёта."));
                if (sum > (XListPerson.SelectedValue as NaturalPerson).Deposit)
                {
                    throw new MoreThanSummaOfDepozitException("Сумма к снятию не может быть больше суммы депозита!!!");
                }
                (XListPerson.SelectedValue as NaturalPerson).logFile.Add
                    ($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} С депозита снято {sum} руб.");
                (XListPerson.SelectedValue as NaturalPerson).Withdrawal((XListPerson.SelectedValue as NaturalPerson).Deposit, sum);
                BankLog?.Invoke((XListPerson.SelectedValue as NaturalPerson), $"С депозита снято {sum} руб.");
            }
            catch (MoreThanSummaOfDepozitException ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            XListPerson.Items.Refresh();
            XTbPersonDeposit.Text = (XListPerson.SelectedValue as NaturalPerson).Deposit.ToString("# ### ###");
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------Переводы------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        // Выбор типа клиента (Юр.лицо или физ.лицо) на вкладке Переводы с помощью радио баттонов
        private void XRBtnOrgTake_Checked(object sender, RoutedEventArgs e)
        {
            XComboEnd.Items.Refresh();
            XComboEnd.ItemsSource = DataBase.Org;
            XComboEnd.Items.Refresh();
        }

        private void XBtnPersonTake_Checked(object sender, RoutedEventArgs e)
        {
            XComboEnd.Items.Refresh();
            XComboEnd.ItemsSource = DataBase.Person;
            XComboEnd.Items.Refresh();
        }

        private void XRBtnOrgGive_Checked(object sender, RoutedEventArgs e)
        {
            XComboStart.Items.Refresh();
            XComboStart.ItemsSource = DataBase.Org;
            XComboStart.Items.Refresh();
        }

        private void XBtnPersonGive_Checked(object sender, RoutedEventArgs e)
        {
            XComboStart.Items.Refresh();
            XComboStart.ItemsSource = DataBase.Person;
            XComboStart.Items.Refresh();
        }
        //-----------------------------------------------------------------------
        // Сортировка списков клиентов с помощью радио баттонов.
        //-----------------------------------------------------------------------

        // Юр. Лица
        private void XRadioBtnOrgOrgName_Checked(object sender, RoutedEventArgs e)
        {
            DataBase.Org.Sort(new Organization.SortByOrgName());
            XListOrg.Items.Refresh();
        }

        private void XRadioBtnOrgDeposit_Checked(object sender, RoutedEventArgs e)
        {

            DataBase.Org.Sort(new Organization.SortByOrgDeposit());
            XListOrg.Items.Refresh();
        }

        private void XRadioBtnOrgInterestRate_Checked(object sender, RoutedEventArgs e)
        {
            DataBase.Org.Sort(new Organization.SortByOrgInterestRate());
            XListOrg.Items.Refresh();
        }
        // Сортировка списков клиентов с помощью радио баттонов.
        // Физ. Лица
        private void XRadioBtnPersonLastName_Checked(object sender, RoutedEventArgs e)
        {
            DataBase.Person.Sort(new NaturalPerson.SortByPersonLastName());
            XListPerson.Items.Refresh();
        }

        private void XRadioBtnPersonDeposit_Checked(object sender, RoutedEventArgs e)
        {
            DataBase.Person.Sort(new NaturalPerson.SortByPersonDeposit());
            XListPerson.Items.Refresh();
        }

        private void XRadioBtnPersonInterestRate_Checked(object sender, RoutedEventArgs e)
        {
            DataBase.Person.Sort(new NaturalPerson.SortByPersonInterestRate());
            XListPerson.Items.Refresh();
        }


        // Выбор клиента отправителя (проверка что бы он небыл идентичным клиенту получателю)
        private void XComboStart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (XTextResultTrunsfer.Text != null)
            {
                XTextResultTrunsfer.Text = default;
            }
            if (DataBase.Clients.Count > 0)
            {
                DataBase.Clients.RemoveAt(0);
            }
            DataBase.Clients.Insert(0, XComboStart.SelectedValue as Client);
            if (XComboEnd.SelectedValue != null)
            {
                try
                {
                    if (DataBase.Clients[0] == DataBase.Clients[1])
                    {
                        XComboStart.Text = default;
                        throw new InvalidIdentityException("Клиент получатель не может быть идентичным Клиенту отправителю.");

                    }
                }
                catch(InvalidIdentityException ex)
                {
                    MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }

        }



        // Выбор клиента получателя (проверка что бы он небыл идентичным клиенту отправителю)
        private void XComboEnd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (XTextResultTrunsfer.Text != null)
            {
                XTextResultTrunsfer.Text = default;
            }

            if (DataBase.Clients.Count > 1)
            {
                DataBase.Clients.RemoveAt(1);
            }

            if (DataBase.Clients.Count == 0)
            {
                MessageBox.Show("Сначала выберите клиента отправителя.", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
                XComboEnd.Text = default;
                return;
            }

            DataBase.Clients.Insert(1, XComboEnd.SelectedValue as Client);
            if (XComboStart.SelectedValue != null)
            {
                try
                {
                    if (DataBase.Clients[0] == DataBase.Clients[1])
                    {
                        XComboEnd.Text = default;
                        throw new InvalidIdentityException("Клиент получатель не может быть идентичным Клиенту отправителю.");

                    }
                }
                catch (InvalidIdentityException ex)
                {
                    MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        // Нажатие кнопки Осуществить перевод
        private void XBtnTransfer_Click(object sender, RoutedEventArgs e)
        {
            decimal sumTransfer;
            try
            {
                // Ввод в текст бокс сумму перевода
                sumTransfer = Convert.ToDecimal(XTextBoxSumOfTransfer.Text);

                if (sumTransfer <= 0)
                {
                    XTextBoxSumOfTransfer.Text = default;
                    throw new SummaEqualsOrLessZeroException("Cумма к снятию не может быть равна или меньше 0.");
                }
                if(sumTransfer > (XComboStart.SelectedValue as Client).Deposit)
                {
                    XTextBoxSumOfTransfer.Text = default;
                    throw new MoreThanSummaOfDepozitException("Сумма к снятию не может быть больше суммы депозита!!!");
                }
                DataBase.Clients[0].Deposit = DataBase.Clients[0].Deposit - sumTransfer;
                DataBase.Clients[0].logFile.Add
                    ($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} С депозита осуществлён перевод на {sumTransfer} руб.");

                BankLog?.Invoke(DataBase.Clients[0], $"С депозита осуществлён перевод на {sumTransfer} руб.");


                DataBase.Clients[1].Deposit = DataBase.Clients[1].Deposit + sumTransfer;
                DataBase.Clients[1].logFile.Add
                    ($"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} На депозит начислен перевод на {sumTransfer} руб.");

                BankLog?.Invoke(DataBase.Clients[1], $"На депозит начислен перевод на {sumTransfer} руб.");

                XTextBoxSumOfTransfer.Text = default;
                sumTransfer = 0;
                XListOrg.Items.Refresh();
                XListPerson.Items.Refresh();

                // Вывод в текст блок в карточке клиента сумму депозита после операции
                if ((XListOrg.SelectedValue) != null && XListOrg.SelectedValue as Organization == XComboStart.SelectedValue as Organization)
                {
                    XTbDeposit.Text = (XComboStart.SelectedValue as Organization).Deposit.ToString("# ### ###");
                }
                if ((XListOrg.SelectedValue) != null && XListOrg.SelectedValue as Organization == XComboEnd.SelectedValue as Organization)
                {
                    XTbDeposit.Text = (XComboEnd.SelectedValue as Organization).Deposit.ToString("# ### ###");
                }
                if ((XListPerson.SelectedValue) != null && XListPerson.SelectedValue as NaturalPerson == XComboStart.SelectedValue as NaturalPerson)
                {
                    XTbPersonDeposit.Text = (XComboStart.SelectedValue as NaturalPerson).Deposit.ToString("# ### ###");
                }
                if ((XListPerson.SelectedValue) != null && XListPerson.SelectedValue as NaturalPerson == XComboEnd.SelectedValue as NaturalPerson)
                {
                    XTbPersonDeposit.Text = (XComboEnd.SelectedValue as NaturalPerson).Deposit.ToString("# ### ###");
                }

                XTextResultTrunsfer.Text = "Перевод осуществлён успешно.";
            }
            catch(SummaEqualsOrLessZeroException ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(MoreThanSummaOfDepozitException ex)
            {
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                XTextBoxSumOfTransfer.Text = default;
                MessageBox.Show($"{ex.Message}\nЛибо вы вводите правильные данные, либо ищете другую работу! Ю Ноу?", "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------Кнопки генерации, загрузки и сохранения базы------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------------

        //Кнопка генерации новой базы
        private void ButtonGeneration_Click(object sender, RoutedEventArgs e)
        {
           
            if (DataBase.Org.Count == 0 && DataBase.Person.Count == 0)
            {
                for (int i = 0; i < 100; i++)
                {
                    decimal tempDeposit = rnd.Next(100, 1000) * 1000;
                    int tempForFlag = rnd.Next(0, 2);
                    string flag = (tempForFlag == 1) ? "yes" : "no";
                    DataBase.Org.Add(new Organization(
                        tempDeposit,
                        rnd.Next(10, 15),
                        $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} Создан новый депозит.",
                        tempDeposit,
                        flag,
                        Names.FirstSomething[rnd.Next(0, 29)] + Names.SecondSomething[rnd.Next(0, 29)],
                        rnd.Next(5, 101),
                        rnd.Next(100, 1000) * 10000
                        ));
                }
                for (int i = 0; i < 100; i++)
                {
                    decimal tempDeposit = rnd.Next(10, 1000) * 1000;
                    int tempForFlag = rnd.Next(0, 2);
                    string flag = (tempForFlag == 1) ? "yes" : "no";
                    DataBase.Person.Add(new NaturalPerson(
                        rnd.Next(10, 1000) * 1000,
                        rnd.Next(10, 20),
                        $"{DateTime.Now.ToLongDateString()} {DateTime.Now.ToLongTimeString()} Создан новый депозит.",
                        tempDeposit,
                        flag,
                        Names.LastN[rnd.Next(0, 34)],
                        Names.FirstN[rnd.Next(0, 34)],
                        rnd.Next(18, 80),
                        rnd.Next(10, 100) * 1000));
                }
                MessageBox.Show("Базы клиентов успешно сгенерированы.", "Information.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("База уже сгенерирована или загружена из файла. \nДля новой генерации выйдите и зайдите в систему повторно.", "Informations.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Кнопка загрузки базы из файла
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            if (DataBase.Org.Count != 0 && DataBase.Person.Count != 0)
            {
                MessageBox.Show("База уже сгенерирована или загружена из файла. \nДля новой загрузки выйдите и зайдите в систему повторно.", "Informations.", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                string orgBase = File.ReadAllText("_BaseOfOrganizations.json");
                var jOrgArray = JObject.Parse(orgBase)["Organizations:"].ToArray();
                DataBase.Org.Clear();
                foreach (var jO in jOrgArray)
                {
                    var log = jO["logFile"].ToArray();
                    Organization organization = new Organization(
                     Convert.ToDecimal(jO["Deposit"].ToString()),
                     Convert.ToDecimal(jO["InterestRate"].ToString()),
                     log[0]["1"].ToString(),
                     Convert.ToDecimal(jO["StartDepositForPercents"].ToString()),
                     jO["CapitalizationPercents"].ToString(),
                     jO["OrganizationName"].ToString(),
                     Convert.ToInt32(jO["EmployeeCount"].ToString()),
                     Convert.ToInt32(jO["YearIncome"].ToString())
                     );
                    if (log.Length > 1)
                    {
                        for (int i = 1; i < log.Length; i++)
                        {
                            organization.logFile.Add(log[i][$"{i + 1}"].ToString());
                        }
                    }
                    DataBase.Org.Add(organization);
                }

                string personBase = File.ReadAllText("_BaseOfNaturalPersones.json");
                var jPersonArray = JObject.Parse(personBase)["Persones:"].ToArray();
                DataBase.Person.Clear();
                foreach (var jP in jPersonArray)
                {
                    var log = jP["logFile"].ToArray();
                    NaturalPerson naturalPerson = new NaturalPerson(
                        Convert.ToDecimal(jP["Deposit"].ToString()),
                        Convert.ToDecimal(jP["InterestRate"]),
                        log[0]["1"].ToString(),
                        Convert.ToDecimal(jP["StartDepositForPercents"].ToString()),
                        jP["CapitalizationPercents"].ToString(),
                        jP["LastName"].ToString(),
                        jP["FirstName"].ToString(),
                        Convert.ToInt32(jP["Age"].ToString()),
                        Convert.ToInt32(jP["Income"].ToString())
                        );
                    if (log.Length > 1)
                    {
                        for (int i = 1; i < log.Length; i++)
                        {
                            naturalPerson.logFile.Add(log[i][$"{i + 1}"].ToString());
                        }
                    }
                    DataBase.Person.Add(naturalPerson);
                }
                MessageBox.Show("Базы клиентов успешно загружены.", "Information.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Перед загрузкой базы, её нужно сгенерировать и сохранить!", "WTF", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Кнопка сохранения базы в файл
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (DataBase.Org.Count != 0 && DataBase.Person.Count != 0)
            {
                JObject OrganizationBaseMainTree = new JObject();
                OrganizationBaseMainTree["BaseType:"] = "OrganizationClients";
                JArray OrgjArray = new JArray();
                for (int i = 0; i < DataBase.Org.Count; i++)
                {
                    JObject Organization = new JObject();
                    Organization["OrganizationName"] = DataBase.Org[i].OrganizationName;
                    Organization["EmployeeCount"] = DataBase.Org[i].EmployeeCount;
                    Organization["YearIncome"] = DataBase.Org[i].YearIncome;
                    Organization["Deposit"] = DataBase.Org[i].Deposit;
                    Organization["InterestRate"] = DataBase.Org[i].InterestRate;
                    JArray log = new JArray();
                    for (int j = 0; j < DataBase.Org[i].logFile.Count; j++)
                    {
                        JObject LogFile = new JObject();
                        LogFile[$"{j + 1}"] = DataBase.Org[i].logFile[j];
                        log.Add(LogFile);
                    }
                    Organization["logFile"] = log;
                    Organization["StartDepositForPercents"] = DataBase.Org[i].StartDepositForPercents;
                    Organization["CapitalizationPercents"] = DataBase.Org[i].CapitalizationPercents;
                    OrgjArray.Add(Organization);
                }
                OrganizationBaseMainTree["Organizations:"] = OrgjArray;
                string jOrg = OrganizationBaseMainTree.ToString();
                File.WriteAllText("_BaseOfOrganizations.json", jOrg);

                JObject PersonBaseMainTree = new JObject();
                PersonBaseMainTree["BaseType:"] = "NaturalPersonClients";
                JArray PersonjArray = new JArray();
                for (int i = 0; i < DataBase.Person.Count; i++)
                {
                    JObject NatPerson = new JObject();
                    NatPerson["LastName"] = DataBase.Person[i].LastName;
                    NatPerson["FirstName"] = DataBase.Person[i].FirstName;
                    NatPerson["Age"] = DataBase.Person[i].Age;
                    NatPerson["Income"] = DataBase.Person[i].Income;
                    NatPerson["Deposit"] = DataBase.Person[i].Deposit;
                    NatPerson["InterestRate"] = DataBase.Person[i].InterestRate;
                    JArray log = new JArray();
                    for (int j = 0; j < DataBase.Person[i].logFile.Count; j++)
                    {
                        JObject LogFile = new JObject();
                        LogFile[$"{j + 1}"] = DataBase.Person[i].logFile[j];
                        log.Add(LogFile);
                    }
                    NatPerson["logFile"] = log;
                    NatPerson["StartDepositForPercents"] = DataBase.Person[i].StartDepositForPercents;
                    NatPerson["CapitalizationPercents"] = DataBase.Person[i].CapitalizationPercents;
                    PersonjArray.Add(NatPerson);
                }
                PersonBaseMainTree["Persones:"] = PersonjArray;
                string jPerson = PersonBaseMainTree.ToString();
                File.WriteAllText("_BaseOfNaturalPersones.json", jPerson);

                MessageBox.Show("Базы клиентов успешно сохранены.", "Information.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Перед сохранением базы, её нужно сгенерировать.", "WTF", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Кнопка начисления процентов
        private void ButtonDemo_Click(object sender, RoutedEventArgs e)
        {
            if (XTbInfoForPercents.Text != null)
            {
                XTbInfoForPercents.Text = default;
            }
            MessageBox.Show("Реализация начисления процентов - учебно - тренировочный режим, не учитывающий возможные снятия, пополнения и переводы по счёту." , "Инфомэйшн", MessageBoxButton.OK, MessageBoxImage.Question);
            try
            {
                if(DataBase.Org.Count == 0 || DataBase.Person.Count == 0)
                {
                    throw new Exception("База не сгенерирована или не загружена.");
                }
                for (int i = 0; i < 12; i++)
                {
                    foreach (var organizationOne in DataBase.Org)
                    {
                        organizationOne.PercentYear(organizationOne.InterestRate, organizationOne.Deposit, organizationOne.StartDepositForPercents, organizationOne.CapitalizationPercents);
                        organizationOne.logFile.Add($"Some time Ежемесячное начисление процента на депозит. {organizationOne.Deposit.ToString("# ### ### .##")}");
                        if (i == 11)
                        {
                            organizationOne.StartDepositForPercents = organizationOne.Deposit;
                        }
                    }
                    foreach (var personOne in DataBase.Person)
                    {
                        personOne.PercentYear(personOne.InterestRate, personOne.Deposit, personOne.StartDepositForPercents, personOne.CapitalizationPercents);
                        personOne.logFile.Add($"Some time Ежемесячное начисление процента на депозит {personOne.Deposit.ToString("# ### ### .##")}");
                        if (i == 11)
                        {
                            personOne.StartDepositForPercents = personOne.Deposit;
                        }
                    }
                }
                XTbInfoForPercents.Text = "Операция выполнена успешно.";
                BankLog?.Invoke(DataBase.Org[0], "Операция начисления процентов клиентам прошла успешно.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "WTF", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

    }
}
