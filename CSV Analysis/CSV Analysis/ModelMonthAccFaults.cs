using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Analysis
{
    internal class ModelMonthAccFaults
    {
        [Name("Month")]
        public int Month { get; set; }
        [Name("Accumulated Faults")]
        public int AccumulatedFaults { get; set; }

        public ModelMonthAccFaults(int month, int accumulatedFaults) 
        {
            this.Month = month;
            this.AccumulatedFaults = accumulatedFaults;
        }
    }
}
