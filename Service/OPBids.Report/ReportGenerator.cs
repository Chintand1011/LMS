using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPBids.Report
{
    //Install-Package Microsoft.ReportingServices.ReportViewerControl.WebForms -Version 140.1000.523
    //VS2017 Report Designer: https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftRdlcReportDesignerforVisualStudio-18001
    public class ReportGenerator
    {
        public string ReportPath { get; set; }

        //List<ReportDataSource> _DataSources;

        Dictionary<string, string> _ReportParameters;

        public ReportGenerator()
        {
            //this._DataSources = new List<ReportDataSource>();
            this._ReportParameters = new Dictionary<string, string>();
        }

        public void AddDataSource(string name, object datasource)
        {
            //this._DataSources.Add(new ReportDataSource()
            //{
            //    Name = name,
            //    Value = datasource
            //});
        }

        public void AddParameter(string name, string value)
        {
            //this._ReportParameters.Add(name, value);
        }

        public byte[] GetPdf(string reportName)
        {
            //try
            //{
            //    Warning[] warnings;
            //    string[] streamIds;
            //    string mimeType = string.Empty;
            //    string encoding = string.Empty;
            //    string extension = string.Empty;

            //    ReportViewer viewer = new ReportViewer();
            //    viewer.ProcessingMode = ProcessingMode.Local;
            //    viewer.LocalReport.EnableExternalImages = true;

            //    viewer.LocalReport.ReportEmbeddedResource = string.Format("OPBids.Report.{0}.rdlc", reportName);
            //    viewer.LocalReport.Refresh();

            //    ReportParameterInfoCollection rptParams = viewer.LocalReport.GetParameters();


            //    if (rptParams != null && rptParams.Count > 0)
            //    {
            //        //TODO: make logo optional
            //        List<ReportParameter> reportparams = new List<ReportParameter>();

            //        if (rptParams.Any(p => p.Name == "logo"))
            //        {
            //            reportparams.Add(new ReportParameter("logo", "file:///" + System.Web.HttpContext.Current.Server.MapPath("~/Images/logo.jpg")));//quality issue using .png
            //        }


            //        foreach (var item in this._ReportParameters)
            //        {
            //            if (rptParams.Any(p => p.Name == item.Key))
            //            {
            //                reportparams.Add(new ReportParameter(item.Key, item.Value));
            //            }
            //            else
            //            {
            //                throw new Exception("Parameter " + item.Key + " not found.");
            //            }
            //        }

            //        viewer.LocalReport.SetParameters(reportparams.ToArray());
            //    }


            //    this._DataSources.ForEach(ds =>
            //        viewer.LocalReport.DataSources.Add(ds)
            //    );




                byte[] bytes = new byte[2];// viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                return bytes;
            //}
            //catch (Exception ex)
            //{
            //    //Common.LogHelper.LogInfo(ex.Message);
            //    throw;
            //}
            
        }
    }
}
