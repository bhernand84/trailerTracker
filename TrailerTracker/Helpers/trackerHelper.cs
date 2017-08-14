using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.FileIO;

namespace TrailerTracker.Helpers
{
    public class TrailerAsset
    {
        public string esn { get; set; }
        public string trailerNumber { get; set; }
        public DateTime lastCheckInTime { get; set; }
    }

    public class TrailerPULS
    {
        public string esn { get; set; }
        public DateTime lastCheckInTime { get; set; }
    }

    public class trackerHelper
    {
        private static List<TrailerAsset> _trailerAssets;
        public static List<TrailerAsset> getAllTrailers()
        {
            if (_trailerAssets == null)
            {
                List<TrailerAsset> listOfTrailers = new List<TrailerAsset>();
                var listOfPULS = getCheckedInTrailers();

                TextFieldParser parser = new TextFieldParser(Settings.assetListDirectory);

                parser.HasFieldsEnclosedInQuotes = true;
                parser.SetDelimiters(",");

                string[] fields;
                string[] columnNames = parser.ReadFields();

                var indexOfEsn = Array.FindIndex(columnNames, m => m == "ESN");
                var indexOfTrailerNumber = Array.FindIndex(columnNames, m => m == "Trailer Number");
                while (!parser.EndOfData)
                {
                    var trailerObject = new TrailerAsset();

                    fields = parser.ReadFields();
                    trailerObject.esn = fields[indexOfEsn];
                    trailerObject.trailerNumber = fields[indexOfTrailerNumber];
                    var matchingPuls = listOfPULS.FirstOrDefault(m => m.esn == trailerObject.esn);
                    if (matchingPuls != null)
                    {
                        trailerObject.lastCheckInTime = matchingPuls.lastCheckInTime;
                    }
                    listOfTrailers.Add(trailerObject);
                }

                parser.Close();
                _trailerAssets = listOfTrailers;
            }
            return _trailerAssets;
        }

        public static List<TrailerPULS> getCheckedInTrailers()
        {
            List<TrailerPULS> checkedInTrailers = new List<TrailerPULS>();


            TextFieldParser parser = new TextFieldParser(Settings.pulsDataDirectory);

            parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");

            string[] fields;
            string[] columnNames = parser.ReadFields();

            var indexOfEsn = Array.FindIndex(columnNames, m => m == "ESN");
            var indexOfLastUpdated = Array.FindIndex(columnNames, m => m == "LAST ID REPORT");
            while (!parser.EndOfData)
            {
                var trailerPULS = new TrailerPULS();

                fields = parser.ReadFields();
                trailerPULS.esn = fields[indexOfEsn].Replace("\"", "").Replace("=", "").Replace(" ","") ;
                if(fields[indexOfLastUpdated] != "")
                {
                    trailerPULS.lastCheckInTime = DateTime.Parse(fields[indexOfLastUpdated]);
                }

                if (trailerPULS.lastCheckInTime != null && trailerPULS.lastCheckInTime >= new DateTime(2017,1,1))
                {
                    checkedInTrailers.Add(trailerPULS);
                }

            }

            parser.Close();


            return checkedInTrailers;
        }
    }
}