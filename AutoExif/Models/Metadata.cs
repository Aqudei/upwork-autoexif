using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AutoExif.Models
{
    class Metadata
    {
        public string PhotoPath { get; internal set; }

        // Location Tab
        public string Latitude { get; set; } // = 33.76506
        public string Longitude { get; set; } // = -84.38994
        public string SexagisemalLatitude { get; set; } // = ''
        public string SexagisemalLongitude { get; set; } // = ''
        public string DestinationLatitude { get; set; } // = 33.76506
        public string DestinationLongitude { get; set; } // = -84.38994
        public string CountryCode { get; set; } // = USA
        public string State { get; set; } // = Georgia
        public string LocationCity { get; set; } // = Atlanta

        // Source / description Tab
        public string ArtistName { get; set; } // = Car Accident Lawyer Atlanta Helpline
        public string CaptionWriter { get; set; } // = Car Accident Lawyer Atlanta Helpline
        public string Credit { get; set; } // = Car Accident Lawyer Atlanta Helpline
        public string Source { get; set; } // = (GMB LINK)
        public string URL { get; set; } // =  (URL)
        public string Copyright { get; set; } // = 55 Ivan Allen Jr Blvd NW #400 Atlanta, GA 30308
        public string ObjectName { get; set; } // = Car crash and auto wreck vehicle motor truck accident need professional legal consultation
        public string Headline { get; set; } // = Best Auto Accident Attorneys Near Me Atlanta Georgia
        public string Caption { get; set; } // = Traffic collision and vehicle car crash injury lawyers
        public string SpecialInstructions { get; set; } // = EMPTY

        // Categories / Keywords Tab
        public string Category { get; set; } // = Personal Injury Attorney
        public string Keywords { get; set; } // = ''

        // Contact Tab
        public string Byline { get; set; } // = Car Accident Lawyer Atlanta Helpline
        public string BylineTitle { get; set; } // = vehicle traffic accident injury attorneys
        public string Address { get; set; } // = 55 Ivan Allen Jr Blvd NW #400 Atlanta, GA 30308
        public string ContactURL { get; set; } // = 
        public string Email { get; set; } // = 
        public string Phone { get; set; } // = (470) 264-3330
        public string ContactCity { get; set; } // = Atlanta
        public string PostalCode { get; set; } // = 30308
        public string StateCountry { get; set; } // = Georgia
        public string Country { get; set; } // = United States of America

        // Extras
        public string LatitudeRef { get; set; }  // N / S
        public string LongitudeRef { get; set; } // E / W
        public string DestinationLatitudeRef { get; set; }
        public string DestinationLongitudeRef { get; set; }

        public string GenerateArgFile()
        {
            var templateArg = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template.arg");
            var argText = File.ReadAllText(templateArg);

            argText = argText.Replace("{Latitude}", Latitude);
            argText = argText.Replace("{Longitude}", Longitude);
            argText = argText.Replace("{CountryCode}", CountryCode);
            argText = argText.Replace("{State}", State);
            argText = argText.Replace("{LocationCity}", LocationCity);
            argText = argText.Replace("{ArtistName}", ArtistName);
            argText = argText.Replace("{CaptionWriter}", CaptionWriter);
            argText = argText.Replace("{Credit}", Credit);
            argText = argText.Replace("{Source}", Source);
            argText = argText.Replace("{URL}", URL);
            argText = argText.Replace("{Copyright}", Copyright);
            argText = argText.Replace("{ObjectName}", ObjectName);
            argText = argText.Replace("{Headline}", Headline);
            argText = argText.Replace("{Caption}", Caption);
            argText = argText.Replace("{Latitude}", Latitude);
            argText = argText.Replace("{StateCountry}", StateCountry);
            argText = argText.Replace("{Country}", Country);

            argText = CleanUnfilled(argText);

            var tempFile = Path.GetTempFileName();
            using (var stream = File.OpenWrite(tempFile))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(argText);
            }

            return tempFile;
        }

        private string CleanUnfilled(string argText)
        {
            var rgx = new Regex(@"{.*}");

            return rgx.Replace(argText, "");
        }
    }
}
