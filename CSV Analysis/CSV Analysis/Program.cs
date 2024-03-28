using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Analysis
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string csvToAnalyseFilePath = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\" + "TestData\\calculator.csv";
            string outputPath = AppDomain.CurrentDomain.BaseDirectory + "..\\..\\..\\" + "\\TestData\\calculator_reduced.csv";
            //header row
            //Created at	Closed at	Updated at	State	Labels	Comments	Number	Title	Url	Html url	Body	User name	User email	Milestone created at	Milestone due on	Milestone descrtiption	Milestone state	Milestone title

            //keywords
            //fixed, fixing, bug

            List<Model> reducedRecords = new List<Model>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = "\t"
            };

            using var reader = new StreamReader(csvToAnalyseFilePath);
            using var csvInput = new CsvReader(reader, config);

            var allRecords = csvInput.GetRecords<Model>();

            int totalRowsNumber = 0;
            int selectedRowsNumber = 0;
            foreach (var record in allRecords)
            {

                if (record.Labels.Contains("fixed") ||
                    record.Labels.Contains("fixing") ||
                    record.Labels.Contains("bug"))
                {
                    record.ConvertCreatedAt();
                    reducedRecords.Add(record);
                    selectedRowsNumber++;
                }

                totalRowsNumber++;
            }

            Dictionary<DateTime, int> faultsPerDay = new Dictionary<DateTime, int>();
            Dictionary<DateTime, int> faultsPerWeek = new Dictionary<DateTime, int>();
            Dictionary<(int, int), int> faultsPerMonth = new Dictionary<(int, int), int>();

            // reducedRecords = reducedRecords.OrderBy(i => i.CreatedAtConverted.Date).ToList();
            foreach (var record in reducedRecords)
            {
                if (faultsPerDay.ContainsKey(record.CreatedAtConverted.Date))
                {
                    faultsPerDay[record.CreatedAtConverted.Date]++;
                }
                else
                {
                    faultsPerDay.Add(record.CreatedAtConverted.Date, 1);
                }

                if (faultsPerMonth.ContainsKey((record.CreatedAtConverted.Year, record.CreatedAtConverted.Month)))
                {
                    faultsPerMonth[(record.CreatedAtConverted.Year, record.CreatedAtConverted.Month)]++;
                }
                else
                {
                    faultsPerMonth.Add((record.CreatedAtConverted.Year, record.CreatedAtConverted.Month), 1);
                }
            }



            Dictionary<int, int> dayAccumulatedFaults = new Dictionary<int, int>();
            int dayCounter = 1;
            int accumulatedFaults = 0;
            foreach (var day in faultsPerDay)
            {

                accumulatedFaults += day.Value;
                dayAccumulatedFaults.Add(dayCounter, accumulatedFaults);
                dayCounter++;
            }
            accumulatedFaults = 0;
            foreach (var month in faultsPerMonth)
            {
                accumulatedFaults += month.Value;
                Console.WriteLine("Month: " + month.Key + " | Accumulated faults: " + accumulatedFaults);
            }

            using var writer = new StreamWriter(outputPath);
            using var csvOutput = new CsvWriter(writer, config);

            csvOutput.WriteRecords(reducedRecords);
            //foreach (var day in dayAccumulatedFaults)
            //{
            //    Console.WriteLine("Day: " + day.Key + " | Accumulated faults: " + day.Value);
            //}
            //foreach (var month in faultsPerMonth)
            //{
            //    Console.WriteLine("Month: " + month.Key + " | Accumulated faults: " + month.Value);
            //}
            // Console.WriteLine("Total rows: " + totalRowsNumber);
            // Console.WriteLine("Selected rows: " + selectedRowsNumber);
        }
    }
}

