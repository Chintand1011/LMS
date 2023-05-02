using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OPBids.Common.BarcodeReader
{ 
  
    public class FileTools
    {
        public delegate void BarcodereaderDelegate(string message);
        public event BarcodereaderDelegate OnWriteFileFailed;
        public event BarcodereaderDelegate OnWriteSuccess;



        string _GhostScriptDirectory { get; set; }
        string _DestinationRootDir { get; set; }

        public FileTools(string GhostScriptDirectory, string DestinationRootDir)
        {
            _GhostScriptDirectory = GhostScriptDirectory;
            _DestinationRootDir = DestinationRootDir;
        }


        public Models.Document ProcessFiles(string sourceFileName, string contentType)
        {

            var model = new Models.Document();

            model.FileFullPath = sourceFileName;
            //first filename. overwrite if barcode found
            model.NewFileName = Path.GetFileName(sourceFileName);
            

            if (!System.IO.File.Exists(sourceFileName))
            {
                
                model.ProcessRemarks = "File does not exist";
                model.isSuccess = false;
                return model;
            }

            var newGuid = Guid.NewGuid().ToString().Replace("-", "");

            //get the fileType
            model.FileType = Path.GetExtension(sourceFileName);
            model.FileGuid = newGuid;
                                   

            var physicalFileName = Path.GetFileNameWithoutExtension(sourceFileName);

            model.FileNameWithoutExtension = physicalFileName;

            var newImageFileName = string.Format("{0}_{1}.jpg", physicalFileName, newGuid);

            var newPDFFileName = string.Format("{0}_{1}.pdf", physicalFileName, newGuid);


            var source = Path.Combine(_GhostScriptDirectory, sourceFileName);
            var destination = Path.Combine(_GhostScriptDirectory, newPDFFileName);

            if(contentType == "image/png" || contentType == "image/jpeg" || contentType == "image/jpg")
            {
                
                model.Barcode = "";
                
                var xName = model.FileGuid.Replace("-", "").Substring(0, 12);

                model.NewFileName = string.Format("{0}_{1}{2}", xName,xName, model.FileType); 

                var finalName = Path.Combine(_DestinationRootDir, "Document", model.NewFileName);

                File.Copy(sourceFileName, finalName);

                model.isSuccess = true;

                File.Delete(sourceFileName);

                return model;
            }
            

            //continue to pdf scanning
            File.Copy(source, destination, true);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = _GhostScriptDirectory + "gswin32c.exe ";
            startInfo.Arguments = string.Format(@"-dSAFER -dBATCH -dNOPAUSE -sDEVICE=png16m -r300 -sOutputFile={0}{1} {0}{2}", _GhostScriptDirectory, newImageFileName, newPDFFileName);
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Process.Start(startInfo).WaitForExit();

            var barCodes = new System.Collections.ArrayList();

            var iScans = 100;

            try
            {
                using (Bitmap originalImage = new Bitmap(Path.Combine(_GhostScriptDirectory,newImageFileName)))
                {

                    BarcodeImaging.FullScanPage(ref barCodes, originalImage, iScans);

                    foreach (var c in barCodes)
                    {
                        var codes = c;
                        model.Barcode = codes.ToString();
                        model.isSuccess = true;
                        model.NewFileName = string.Format("{0}_{1}{2}", model.Barcode,model.FileGuid.Replace("-","").Substring(0,12),model.FileType); //Path.Combine(_DestinationRootDir, model.Barcode, model.FileGuid, model.FileGuid);

                        break;
                    }

                }
                
                var finalName = Path.Combine(_DestinationRootDir,"Document", model.NewFileName);

                File.Copy(Path.Combine(_GhostScriptDirectory, newPDFFileName), finalName);

                File.Delete(Path.Combine(_GhostScriptDirectory, newImageFileName));
                File.Delete(Path.Combine(_GhostScriptDirectory, newPDFFileName));
                File.Delete(sourceFileName);

                if(OnWriteSuccess != null)
                {
                    OnWriteSuccess(String.Format("Copy file to : {0}", finalName));
                }

            }
            catch (Exception ex)
            {
                model.isSuccess = false;
                model.ProcessRemarks = ex.Message;

                if (OnWriteFileFailed != null)
                {
                    OnWriteSuccess(String.Format("Copy Failed : {0}", ex.Message));
                }

            }

            return model;
        }
    }
}
