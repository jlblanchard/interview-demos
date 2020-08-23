using NUnit.Framework;
using Backend;
using Moq;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {
        /// <summary>
        /// Tests the scenario when the service and db contain different countries
        /// </summary>
        [Test]
        public void AggergateCountryPopulation_NoDuplicateCountries()
        {
            // Setup
            var mockStatService = new Mock<IStatService>();
            mockStatService.Setup(service => service.GetCountryPopulations()).Returns(GetServiceList());
            var mockSqlQueries = new Mock<ISqlQueries>();
            mockSqlQueries.Setup(sql => sql.GetCountryPopulations()).Returns(GetSqlList());

            var aggregator = new AggregateCountryPopulation(mockStatService.Object, mockSqlQueries.Object);

            // Act
            var result = aggregator.AggergateCountryData();

            // Validate
            Assert.AreEqual("United States", result[0].CountryName);
            Assert.AreEqual(10000, result[0].Population);
            Assert.AreEqual("Canada", result[1].CountryName);
            Assert.AreEqual(100, result[1].Population);
            Assert.AreEqual("United Kingdom", result[2].CountryName);
            Assert.AreEqual(100, result[2].Population);
            Assert.AreEqual("Germany", result[3].CountryName);
            Assert.AreEqual(2000, result[3].Population);
        }

        /// <summary>
        /// Tests the case when all of the service results contain a duplicate from the database
        /// </summary>
        [Test]
        public void AggregateCountryPopulation_ServiceContainsDuplicate()
        {
            // Setup
            var mockStatService = new Mock<IStatService>();
            mockStatService.Setup(service => service.GetCountryPopulations()).Returns(GetServiceList());
            var mockSqlQueries = new Mock<ISqlQueries>();
            mockSqlQueries.Setup(sql => sql.GetCountryPopulations()).Returns(GetDuplicateSqlList());

            var aggregator = new AggregateCountryPopulation(mockStatService.Object, mockSqlQueries.Object);

            // Act
            var result = aggregator.AggergateCountryData();

            // Validate
            Assert.AreEqual("Canada", result[0].CountryName);
            Assert.AreEqual(100, result[0].Population);
            Assert.AreEqual("United States", result[1].CountryName);
            Assert.AreEqual(20002, result[1].Population);
            Assert.AreEqual("United Kingdom", result[2].CountryName);
            Assert.AreEqual(100, result[2].Population);
            Assert.AreEqual("Germany", result[3].CountryName);
            Assert.AreEqual(2000, result[3].Population);
        }

        /// <summary>
        /// Tests the case when the service duplicates all of the results from the database
        /// </summary>
        [Test]
        public void AggregateCountryPopulation_ServiceAllDuplicates()
        {
            // Setup
            var mockStatService = new Mock<IStatService>();
            mockStatService.Setup(service => service.GetCountryPopulations()).Returns(GetDuplicateServiceList());
            var mockSqlQueries = new Mock<ISqlQueries>();
            mockSqlQueries.Setup(sql => sql.GetCountryPopulations()).Returns(GetSqlList());

            var aggregator = new AggregateCountryPopulation(mockStatService.Object, mockSqlQueries.Object);

            // Act
            var result = aggregator.AggergateCountryData();

            // Validate
            Assert.AreEqual("United Kingdom", result[0].CountryName);
            Assert.AreEqual(100, result[0].Population);
            Assert.AreEqual("Germany", result[1].CountryName);
            Assert.AreEqual(2000, result[1].Population);
        }

        /// <summary>
        /// Creates a list of results to be returned by the mocked service
        /// </summary>
        /// <returns></returns>
        private List<Tuple<string, int>> GetServiceList()
        {
            List<Tuple<string, int>> countries = new List<Tuple<string, int>>();

            countries.Add(new Tuple<string, int>("United States", 10000));
            countries.Add(new Tuple<string, int>("Canada", 100));

            return countries;
        }

        /// <summary>
        /// Creates a list of results to be returned by the mocked db
        /// </summary>
        /// <returns></returns>
        private List<CountryPopulation> GetSqlList()
        {
            List<CountryPopulation> countries = new List<CountryPopulation>();

            countries.Add(new CountryPopulation() { CountryName = "United Kingdom", Population = 100 });
            countries.Add(new CountryPopulation() { CountryName = "Germany", Population = 2000 });

            return countries;
        }

        /// <summary>
        /// Creates a list of results to be returned by the mocked db that contains a duplicate from the mock service
        /// </summary>
        /// <returns></returns>
        private List<CountryPopulation> GetDuplicateSqlList()
        {
            List<CountryPopulation> countries = new List<CountryPopulation>();

            countries.Add(new CountryPopulation() { CountryName = "United States", Population = 20002 });
            countries.Add(new CountryPopulation() { CountryName = "United Kingdom", Population = 100 });
            countries.Add(new CountryPopulation() { CountryName = "Germany", Population = 2000 });

            return countries;
        }

        /// <summary>
        /// Creates a list of results to be returned by the mocked service that contains duplicate results from the mock db
        /// </summary>
        /// <returns></returns>
        private List<Tuple<string, int>> GetDuplicateServiceList()
        {
            List<Tuple<string, int>> countries = new List<Tuple<string, int>>();

            countries.Add(new Tuple<string, int>("Germany", 4000));
            countries.Add(new Tuple<string, int>("United Kingdom", 999));

            return countries;
        }
    }
}