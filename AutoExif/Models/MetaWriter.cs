using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoExif.Models
{
    class MetaWriter
    {
        private const string _command = "-@ \"{0}\" -common_args \"{1}\"";

        public void WriterMeta(Metadata metadata, string photoFile)
        {

            string argFile = metadata.GenerateArgFile();

            var exifExectutable = Properties.Settings.Default.EXIFTOOL_LOCATION;

            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = exifExectutable,
                Arguments = string.Format(_command, argFile, photoFile),
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            Debug.WriteLine("Writing meta data to file: " + photoFile);

            try
            {
                using (var process = Process.Start(processStartInfo))
                {
                    process.WaitForExit();
                    process.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
             
            }
        }
    }
}
