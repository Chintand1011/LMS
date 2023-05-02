using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Common.BarcodeReader.Models
{
    public class Document
    {
        public string FileGuid { get; set; }
        public string ActualFileName { get; set; }
        public string Barcode { get; set; }
        public string FileType { get; set; }
        public string ProcessRemarks { get; set; }
        public bool isSuccess { get; set; }

        public string FileFullPath { get; set; }

        public string FileNameWithoutExtension { get; set; }

        public string NewFileName { get; set; }
    }
}
