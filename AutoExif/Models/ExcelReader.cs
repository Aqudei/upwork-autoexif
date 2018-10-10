using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace AutoExif.Models
{
    class ExcelReader
    {
        public IEnumerable<Metadata> Read(string excelFile)
        {
            using (var package = new ExcelPackage())
            using (var stream = File.OpenRead(excelFile))
            {
                package.Load(stream);
                var workbook = package.Workbook;
                var worksheet = workbook.Worksheets.FirstOrDefault();

                for (int i = 3; i <= worksheet.Dimension.Rows; i++)
                {
                    var column = 1;
                    var newMetaData = new Metadata
                    {
                        PhotoPath = worksheet.Cells[i, column++].GetValue<string>(),
                        Latitude = worksheet.Cells[i, column++].GetValue<string>(),
                        Longitude = worksheet.Cells[i, column++].GetValue<string>(),
                        SexagisemalLatitude = worksheet.Cells[i, column++].GetValue<string>(),
                        SexagisemalLongitude = worksheet.Cells[i, column++].GetValue<string>(),
                        DestinationLatitude = worksheet.Cells[i, column++].GetValue<string>(),
                        DestinationLongitude = worksheet.Cells[i, column++].GetValue<string>(),
                        CountryCode = worksheet.Cells[i, column++].GetValue<string>(),
                        State = worksheet.Cells[i, column++].GetValue<string>(),
                        LocationCity = worksheet.Cells[i, column++].GetValue<string>(),
                        ArtistName = worksheet.Cells[i, column++].GetValue<string>(),
                        CaptionWriter = worksheet.Cells[i, column++].GetValue<string>(),
                        Credit = worksheet.Cells[i, column++].GetValue<string>(),
                        Source = worksheet.Cells[i, column++].GetValue<string>(),
                        URL = worksheet.Cells[i, column++].GetValue<string>(),
                        Copyright = worksheet.Cells[i, column++].GetValue<string>(),
                        ObjectName = worksheet.Cells[i, column++].GetValue<string>(),
                        Headline = worksheet.Cells[i, column++].GetValue<string>(),
                        Caption = worksheet.Cells[i, column++].GetValue<string>(),
                        SpecialInstructions = worksheet.Cells[i, column++].GetValue<string>(),
                        Category = worksheet.Cells[i, column++].GetValue<string>(),
                        Keywords = worksheet.Cells[i, column++].GetValue<string>(),
                        Byline = worksheet.Cells[i, column++].GetValue<string>(),
                        BylineTitle = worksheet.Cells[i, column++].GetValue<string>(),
                        Address = worksheet.Cells[i, column++].GetValue<string>(),
                        ContactURL = worksheet.Cells[i, column++].GetValue<string>(),
                        Email = worksheet.Cells[i, column++].GetValue<string>(),
                        Phone = worksheet.Cells[i, column++].GetValue<string>(),
                        ContactCity = worksheet.Cells[i, column++].GetValue<string>(),
                        PostalCode = worksheet.Cells[i, column++].GetValue<string>(),
                        StateCountry = worksheet.Cells[i, column++].GetValue<string>(),
                        Country = worksheet.Cells[i, column++].GetValue<string>(),
                    };

                    yield return newMetaData;
                }
            }
        }
    }
}
