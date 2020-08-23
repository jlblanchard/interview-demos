using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public interface ISqlQueries
    {
        List<CountryPopulation> GetCountryPopulations();
    }
}
