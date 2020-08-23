using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class SqlQueries
    {
        /// <summary>
        /// Retrieves a list of countries and their total populations
        /// </summary>
        /// <returns>A list with the country and population</returns>
        public List<CountryPopulation> GetCountryPopulations()
        {
            List<CountryPopulation> countryPopulations = new List<CountryPopulation>();
            IDbManager db = new SqliteDbManager();
            DbConnection conn = db.getConnection();

            if (conn == null)
            {
                Console.WriteLine("Failed to get connection");
            }
            var sqlCommand = conn.CreateCommand();
            sqlCommand.CommandText = "SELECT sum(Population) as TotalPopulation, CountryName FROM Country c JOIN State s ON c.CountryId = s.CountryId JOIN City cty ON s.StateId = cty.StateId GROUP BY c.CountryName";
            var dataReader = sqlCommand.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dataReader);
            foreach (DataRow row in dt.Rows)
            {
                countryPopulations.Add(new CountryPopulation() { CountryName = row["CountryName"].ToString(), Population = Convert.ToInt32(row["TotalPopulation"]) });
            }
            return countryPopulations;
        }
    }
}
