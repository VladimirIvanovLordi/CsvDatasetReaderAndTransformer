using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Analysis
{
    //header row
    //Created at	Closed at	Updated at	State	Labels	Comments	Number	Title	Url	Html url	Body	User name	User email	Milestone created at	Milestone due on	Milestone descrtiption	Milestone state	Milestone title
    internal class Model
    {
        [Name("Created at")]
        public string CreatedAt { get; set; }
        [Name("Labels")]
        public string Labels { get; set; }


        public DateTime CreatedAtConverted { get; private set; }

        public void ConvertCreatedAt()
        {
            CreatedAtConverted = DateTime.Parse(CreatedAt);
        }
    }
}
