using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using AutoExif.Models;
using System.Diagnostics;
using System.Security.AccessControl;

namespace AutoExif.ViewModels
{
    class ShellViewModel : Screen
    {
        private string _exifToolPath;
        private string _excelInputFile;
        private string _imagesFolderPath;
        private readonly ExcelReader _excelReader;
        private readonly MetaWriter _metaWriter;


        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        public string ExifToolPath
        {
            get => _exifToolPath; set
            {
                Set(ref _exifToolPath, value);
                NotifyOfPropertyChange(nameof(CanStartProcessing));
            }
        }
        public string ExcelInputFile
        {
            get => _excelInputFile; set
            {
                Set(ref _excelInputFile, value);
                NotifyOfPropertyChange(nameof(CanStartProcessing));
                NotifyOfPropertyChange(nameof(CanOpenExcelFile));
            }
        }


        public ShellViewModel(ExcelReader excelReader, MetaWriter metaWriter)
        {
            var exif = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Roaming\\GeoSetter\\tools\\exiftool.exe");

            if (File.Exists(exif))
            {
                ExifToolPath = exif;
                Properties.Settings.Default.EXIFTOOL_LOCATION = exif;
                Properties.Settings.Default.Save();
            }
            else
            {
                ExifToolPath = Properties.Settings.Default.EXIFTOOL_LOCATION;
            }

            _excelReader = excelReader;
            _metaWriter = metaWriter;
        }

        public void BrowseForExifToolLocation()
        {
            var exeFilter = new CommonFileDialogFilter("Executable File", "*.exe");
            using (var ofd = new CommonOpenFileDialog())
            {
                ofd.Filters.Add(exeFilter);
                var rslt = ofd.ShowDialog();
                if (rslt != CommonFileDialogResult.Ok)
                    return;

                ExifToolPath = ofd.FileName;
                Properties.Settings.Default.EXIFTOOL_LOCATION = ofd.FileName;
                Properties.Settings.Default.Save();
            }
        }

        public void BrowseForExcelInputFile()
        {
            var excelFilter = new CommonFileDialogFilter("Excel Files", "*.xlsx");
            using (var ofd = new CommonOpenFileDialog())
            {
                ofd.Filters.Add(excelFilter);
                var rslt = ofd.ShowDialog();
                if (rslt != CommonFileDialogResult.Ok)
                    return;

                ExcelInputFile = ofd.FileName;
            }
        }

        public bool CanStartProcessing
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ExifToolPath) &&
                       !string.IsNullOrWhiteSpace(ExcelInputFile);
            }
        }

        public bool CanOpenExcelFile => !string.IsNullOrWhiteSpace(ExcelInputFile);


        public void OpenExcelFile()
        {
            Process.Start(ExcelInputFile);
        }

        public async void StartProcessing()
        {
            IsBusy = true;

            await Task.Run(() =>
            {
                var metadatas = _excelReader.Read(ExcelInputFile).ToArray();

                for (int i = 0; i < metadatas.Length; i++)
                {
                    var meta = metadatas[i];

                    if (string.IsNullOrWhiteSpace(meta.PhotoPath))
                        return;

                    if (!File.Exists(meta.PhotoPath))
                        continue;

                    _metaWriter.WriterMeta(meta, meta.PhotoPath);
                }

                for (int i = 0; i < metadatas.Length; i++)
                {
                    string ext = Path.GetExtension(metadatas[i].PhotoPath);
                    var backup = Path.ChangeExtension(metadatas[i].PhotoPath, ext + "_original");
                    if (!File.Exists(backup))
                        continue;

                    File.Delete(backup);
                }
            });


            IsBusy = false;

        }
    }
}
