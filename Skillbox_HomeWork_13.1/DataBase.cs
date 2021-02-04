using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillbox_HomeWork_13._1
{
    static class DataBase
    {
        static public List<Organization> Org;

        static public List<NaturalPerson> Person;

        static public List<Client> Clients;

        static DataBase()
        {
            Org = new List<Organization>();

            Person = new List<NaturalPerson>();

            Clients = new List<Client>();
        }
            



    }
}
