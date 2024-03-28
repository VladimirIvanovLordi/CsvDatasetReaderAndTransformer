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
            string csvToAnalyseFilePath = "C:\\Users\\Foxy\\source\\repos\\CSV Analysis\\CSV Analysis\\TestData\\calculator.csv";
            string outputPath = "C:\\Users\\Foxy\\source\\repos\\CSV Analysis\\CSV Analysis\\TestData\\calculator_reduced.csv";
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

            using var reader  = new StreamReader(csvToAnalyseFilePath);
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

            reducedRecords = reducedRecords.OrderBy(i => i.CreatedAtConverted).ToList();
            foreach (var record in reducedRecords)
            {
                if(faultsPerDay.ContainsKey(record.CreatedAtConverted.Date)) 
                {
                    faultsPerDay[record.CreatedAtConverted.Date]++;
                }
                else
                {
                    faultsPerDay.Add(record.CreatedAtConverted.Date, 1);
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

            using var writer = new StreamWriter(outputPath);
            using var csvOutput = new CsvWriter(writer, config);

            csvOutput.WriteRecords(reducedRecords);
            foreach (var day in dayAccumulatedFaults)
            {
                Console.WriteLine(day.Key + " - " + day.Value);
            }
            Console.WriteLine("Total rows: " + totalRowsNumber);
            Console.WriteLine("Selected rows: " + selectedRowsNumber);
        }
    }
}

