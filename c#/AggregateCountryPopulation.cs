using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class AggregateCountryPopulation
    {
        private IStatService statService;
        private ISqlQueries sql;

        public AggregateCountryPopulation(IStatService statService, ISqlQueries sql)
        {
            this.statService = statService;
            this.sql = sql;
        }

        /// <summary>
        /// Aggergates country population data from two different data sources
        /// </summary>
        /// <returns>Returns the aggregated population data</returns>
        public List<CountryPopulation> AggergateCountryData()
        {
            List<CountryPopulation> countries = new List<CountryPopulation>();

            var serviceResults = statService.GetCountryPopulations();
            var dbResults = sql.GetCountryPopulations();

            // Find the countries that overlap between the service and the database
            var duplicateList = GetDuplicateCountriesList(serviceResults, dbResults);

            // Take all items from service that don't exist in the database
            foreach(var serviceCountry in serviceResults)
            {
                if (!duplicateList.Contains(serviceCountry.Item1))
                {
                    countries.Add(new CountryPopulation() { CountryName = serviceCountry.Item1, Population = serviceCountry.Item2 });
                }
            }

            // Take all items in the database that exist in the service and only exist in the database
            countries.AddRange(dbResults);

            return countries;
        }

        /// <summary>
        /// Retrieves a list of countries that exist in both data sources. Assumes that the Country Name matches between both data sources (ex: US and US not United States and US)
        /// </summary>
        /// <param name="service">The results from the service country list</param>
        /// <param name="database">The results from the database country list</param>
        /// <returns></returns>
        private List<string> GetDuplicateCountriesList(List<Tuple<string, int>> service, List<CountryPopulation> database)
        {
            var serviceCountryList = service.Select(s => s.Item1).ToList();
            var databaseCountryList = database.Select(d => d.CountryName).ToList();

            return serviceCountryList.Intersect(databaseCountryList).ToList();
        }
    }
}
