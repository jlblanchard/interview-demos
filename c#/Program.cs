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
            IStatService statService = new ConcreteStatService();

            AggregateCountryPopulation aggregateCountryPopulation = new AggregateCountryPopulation(statService, sql);

            var aggCountryPopulations = aggregateCountryPopulation.AggergateCountryData();

            foreach(var countryPopulation in aggCountryPopulations)
            {
                Console.WriteLine(String.Format("CountryName: {0}, Population: {1}", countryPopulation.CountryName, countryPopulation.Population));
            }

            Console.ReadLine();
        }
    }
}
