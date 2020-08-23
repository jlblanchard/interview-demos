using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started");
            Console.WriteLine("Getting DB Connection...");

            SqlQueries sql = new SqlQueries();

            var dbCountryPopulations = sql.GetCountryPopulations();

            foreach(var countryPopulation in dbCountryPopulations)
            {
                Console.WriteLine(String.Format("CountryName: {0}, Population: {1}", countryPopulation.CountryName, countryPopulation.Population));
            }

            Console.ReadLine();
        }
    }
}
