using OPBids.Service.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using OPBids.Entities.Common;
using OPBids.Entities.View.Shared;
using OPBids.Service.Data;
using OPBids.Entities.View.ProjectRequest;
using OPBids.Service.Utilities;
using OPBids.Service.Models.ProjectRequest;
using OPBids.Report;
using OPBids.Common;

namespace OPBids.Service.Logic.Shared
{
    public class ReportsLogic : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        public Result<byte[]> ProjectRequestReport(Payload payload)
        {
            try
            {



                IEnumerable<Report.Datasets.ProjectRequestDS> projRequest = (from proj in db.ProjectRequests
                                                                             where proj.id == payload.id
                                                                             select new Report.Datasets.ProjectRequestDS()
                                                                             {
                                                                                 id = proj.id,
                                                                                 title = proj.title,
                                                                                 description = proj.description,
                                                                                 estimated_budget = proj.estimated_budget
                                                                             }
                             );

                IEnumerable<Report.Datasets.ProjectRequestItemsDS> projRequestItems = (from item in db.ProjectRequestItems
                                                                                       where item.project_id == payload.id
                                                                                       select new Report.Datasets.ProjectRequestItemsDS()
                                                                                       {
                                                                                           id = item.id,
                                                                                           description = item.description,
                                                                                           unit = item.unit,
                                                                                           quantity = item.quantity,
                                                                                           unit_cost = item.unit_cost
                                                                                       }
                             );


                ReportGenerator gen = new ReportGenerator();

                gen.AddDataSource("dsProjectRequest", projRequest);

                gen.AddDataSource("dsProjectRequestItems", projRequestItems);

                Result<byte[]> _result = new Result<byte[]>();

                _result.value = gen.GetPdf("ProjectInfoSample");


                return _result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Result<byte[]> PostingApproval(Payload payload)
        {
            try
            {
                IEnumerable<Report.Datasets.ProjectRequestDS> projRequest = (from proj in db.ProjectRequests
                                                                             join g in db.ProjectGrantees on proj.grantee equals g.id.ToString() into gt
                                                                             from pg in gt.DefaultIfEmpty()
                                                                             join cat in db.ProjectCategory on proj.category equals cat.id.ToString() into catt
                                                                             from pcat in catt.DefaultIfEmpty()
                                                                             where proj.id == payload.id
                                                                             select new Report.Datasets.ProjectRequestDS()
                                                                             {
                                                                                 id = proj.id,
                                                                                 title = proj.title,
                                                                                 description = proj.description,
                                                                                 estimated_budget = proj.estimated_budget,
                                                                                 pr_number = proj.pr_number,
                                                                                 batch_id = proj.batch_id,
                                                                                 grantee = pg.grantee_name,
                                                                                 approved_budget = proj.approved_budget,
                                                                                 category = pcat.proj_cat
                                                                             }
                                                                            );

                var batch_id = projRequest.Select(pr => pr.batch_id).FirstOrDefault();
                List<Report.Datasets.ProjectRequestBatchDS> projBatch = new List<Report.Datasets.ProjectRequestBatchDS>();

                if (batch_id != null && batch_id > 0)
                {
                    projBatch = (from batch in db.ProjectRequestBatch
                                 where batch.id == batch_id
                                 select new Report.Datasets.ProjectRequestBatchDS()
                                 {
                                     id = batch.id,
                                     conspost_by = batch.conspost_by,
                                     conspost_date_command = batch.conspost_date_command,
                                     conspost_date_lobby = batch.conspost_date_lobby,
                                     conspost_date_reception = batch.conspost_date_reception,
                                     mmda_publish_by = batch.mmda_publish_by,
                                     mmda_publish_date = batch.mmda_publish_date,
                                     newspaper_post_by = batch.newspaper_post_by,
                                     newspaper_post_date = batch.newspaper_post_date,
                                     newspaper_sent_date = batch.newspaper_sent_date,
                                     newspaper_publisher = batch.newspaper_publisher,
                                     newspaper_receive_by = batch.newspaper_received_by,
                                     philgeps_publish_by = batch.philgeps_publish_by,
                                     philgeps_publish_date = batch.philgeps_publish_date
                                 }
                        ).ToList();
                }
                

                ReportGenerator gen = new ReportGenerator();

                gen.AddDataSource("dsProjectRequest", projRequest);

                gen.AddDataSource("dsProjectRequestBatch", projBatch);

                Result<byte[]> _result = new Result<byte[]>();
                _result.value = gen.GetPdf("PostingApproval");

                return _result;
            }
            catch (Exception ex)
            {
                //Common.LogHelper.LogInfo(ex.Message);
                throw;
            }
        }

        public Result<byte[]> NOA(Payload payload)
        {
            try
            {
                var qry = (from proj in db.ProjectRequests
                           join pb in db.ProjectBid on proj.id equals pb.project_request_id
                           join supp in db.Supplier on pb.created_by equals supp.user_id
                           where proj.id == payload.id && pb.bid_status == Common.Constant.BidStatus.Awarded
                           select new
                           {
                               project_title = proj.title,
                               project_estimated_budget = proj.estimated_budget,
                               supplier_name = supp.comp_name,
                               supplier_address = supp.address,
                               supplier_contact_person = supp.contact_person
                           });

                var qryItem = qry.FirstOrDefault();

                List<Report.Datasets.ProjectRequestDS> projRequest = new List<Report.Datasets.ProjectRequestDS>();
                List<Report.Datasets.SupplierDS> supplier = new List<Report.Datasets.SupplierDS>();

                string custom1_budget_in_words = "";
                if (qryItem != null)
                {
                    projRequest.Add(new Report.Datasets.ProjectRequestDS() {
                        title = qryItem.project_title,
                        estimated_budget = qryItem.project_estimated_budget
                    });

                    supplier.Add(new Report.Datasets.SupplierDS()
                    {
                        comp_name = qryItem.supplier_name,
                        contact_person = qryItem.supplier_contact_person
                    });

                    decimal estimated_budget = qryItem.project_estimated_budget;

                    custom1_budget_in_words = CommonMethods.ConvertToWords(estimated_budget);
                    
                }

                ReportGenerator gen = new ReportGenerator();


                gen.AddParameter("custom1", custom1_budget_in_words);

                gen.AddDataSource("dsProjectRequest", projRequest);

                gen.AddDataSource("dsSupplier", supplier);

                Result<byte[]> _result = new Result<byte[]>();

                _result.value = gen.GetPdf("NOA");


                return _result;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public Result<byte[]> InvitationToBid(Payload payload)
        {
            ReportGenerator gen = new ReportGenerator();

            //payload.id = batch id

            var batch = (from prb in db.ProjectRequestBatch
                         where prb.id == payload.id
                         select new
                         {
                             prb.id,
                             prb.pre_bid_date,
                             prb.pre_bid_place,
                             prb.bid_deadline_date,
                             prb.bid_deadline_place,
                             prb.bid_opening_date,
                             prb.bid_opening_place
                         }
                         ).FirstOrDefault();

            gen.AddParameter("preBidDateTime", batch.pre_bid_date.HasValue ? batch.pre_bid_date.Value.ToString("MMMM dd, yyyy, dddd, hh:mm tt") : "");
            gen.AddParameter("preBidPlace", batch.pre_bid_place);

            gen.AddParameter("deadlineDateTime", batch.bid_deadline_date.HasValue ? batch.bid_deadline_date.Value.ToString("MMMM dd, yyyy, dddd, hh:mm tt") : "");
            gen.AddParameter("deadlinePlace", batch.bid_deadline_place);

            gen.AddParameter("openingBidDateTime", batch.bid_opening_date.HasValue ? batch.bid_opening_date.Value.ToString("MMMM dd, yyyy, dddd, hh:mm tt") : "");
            gen.AddParameter("openingBidPlace", batch.bid_opening_place);

            if (batch.bid_deadline_date.HasValue)
            {
            //TODO: confirm
                DateTime submissionDate = batch.bid_deadline_date.Value.Date + new TimeSpan(17, 0, 0);
                gen.AddParameter("submissionDateTime", submissionDate.ToString("MMMM dd, yyyy, dddd, hh:mm tt"));
            }






            IEnumerable<Report.Datasets.ProjectRequestDS> projRequest = (from prb in db.ProjectRequestBatch
                                                                         where prb.id == payload.id
                                                                         join proj in db.ProjectRequests on prb.id equals proj.batch_id
                                                                         join src in db.SourceFunds on proj.source_fund equals src.id.ToString() into ps
                                                                         from prj_src in ps.DefaultIfEmpty()
                                                                         join g in db.ProjectGrantees on proj.grantee equals g.id.ToString() into gt
                                                                         from pg in gt.DefaultIfEmpty()
                                                                         select new Report.Datasets.ProjectRequestDS()
                                                                         {
                                                                             id = proj.id,
                                                                             title = proj.title,
                                                                             description = proj.description,
                                                                             estimated_budget = proj.estimated_budget,
                                                                             approved_budget = proj.approved_budget,
                                                                             source_fund = prj_src.source_description,
                                                                             grantee = pg.grantee_name
                                                                         }
                         );





            gen.AddDataSource("dsProjectRequest", projRequest);


            Result<byte[]> _result = new Result<byte[]>();

            _result.value = gen.GetPdf("InvitationToBid");
            return _result;
            
        }

        public Result<byte[]> AbstractofBids(Payload payload)
        {
            try
            {

            
            ReportGenerator gen = new ReportGenerator();
                
            IEnumerable<Report.Datasets.ProjectRequestDS> projRequest = (from proj in db.ProjectRequests
                                                                         where proj.id == payload.id
                                                                         select new Report.Datasets.ProjectRequestDS()
                                                                         {
                                                                             id = proj.id,
                                                                             title = proj.title,
                                                                             approved_budget = proj.approved_budget

                                                                         });

            IEnumerable<Report.Datasets.ProjectRequestBatchDS> batch = (from prb in db.ProjectRequestBatch
                                                                        join proj in db.ProjectRequests on prb.id equals proj.batch_id
                                                                        where proj.id == payload.id
                                                                        select new Report.Datasets.ProjectRequestBatchDS()
                                                                        {
                                                                             id = prb.id,
                                                                             bid_opening_date = prb.bid_opening_date,
                                                                             bid_opening_place = prb.bid_opening_place
                                                                         }
                                                                         );

                IEnumerable<Report.Datasets.ProjectBidsDS> bids = (from bid in db.ProjectBid
                                                                   join proj in db.ProjectRequests on bid.project_request_id equals proj.id
                                                                   join creator in db.AccessUser on proj.created_by equals creator.id
                                                                   where bid.project_request_id == payload.id
                                                                   select new Report.Datasets.ProjectBidsDS()
                                                                   {
                                                                       id = bid.id,
                                                                       bid_amount = bid.bid_amount,
                                                                       bid_bond = bid.bid_bond,
                                                                       bid_form = bid.bid_form,
                                                                       duration = bid.duration,
                                                                       variance = bid.variance,
                                                                       created_by = string.Concat(creator.first_name, " ", creator.mi, " ", creator.last_name),
                                                                   }
                                                                     ).ToList();

                
                gen.AddDataSource("dsProjectRequest", projRequest);
                gen.AddDataSource("dsProjectRequestBatch", batch);
                gen.AddDataSource("dsProjectBids", bids);
                
            Result<byte[]> _result = new Result<byte[]>();

            _result.value = gen.GetPdf("AbstractofBids");

            return _result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Result<byte[]> PostQualification(Payload payload)
        {
            try
            {

                var qry = (from proj in db.ProjectRequests
                           join pb in db.ProjectBid on proj.id equals pb.project_request_id
                           join supp in db.Supplier on pb.created_by equals supp.user_id
                           where proj.id == payload.id && pb.bid_status == Common.Constant.BidStatus.Awarded
                           select new
                           {
                               project_title = proj.title,
                               supplier_name = supp.comp_name
                           });

                var qryItem = qry.FirstOrDefault();

                List<Report.Datasets.ProjectRequestDS> projRequest = new List<Report.Datasets.ProjectRequestDS>();
                List<Report.Datasets.SupplierDS> supplier = new List<Report.Datasets.SupplierDS>();

                if (qryItem != null)
                {
                    projRequest.Add(new Report.Datasets.ProjectRequestDS() { title = qryItem.project_title });

                    supplier.Add(new Report.Datasets.SupplierDS() { title = qryItem.supplier_name });
                }

                ReportGenerator gen = new ReportGenerator();

                gen.AddDataSource("dsProjectRequest", projRequest);

                gen.AddDataSource("dsSupplier", supplier);

                Result<byte[]> _result = new Result<byte[]>();

                _result.value = gen.GetPdf("PostQualification");


                return _result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Result<byte[]> LCBMemo(Payload payload)
        {
            try
            {

                var projRequest = (from proj in db.ProjectRequests
                                   where proj.id == payload.id
                                   select new Report.Datasets.ProjectRequestDS
                                   {
                                       title = proj.title
                                   });


                var projBids = (from proj in db.ProjectRequests
                                join pb in db.ProjectBid on proj.id equals pb.project_request_id
                                join supp in db.Supplier on pb.created_by equals supp.user_id
                                where proj.id == payload.id && pb.bid_status != Common.Constant.BidStatus.Disqualified && pb.bid_status != null && pb.bid_status != ""
                                select new Report.Datasets.ProjectBidsDS
                                {
                                    bid_amount = pb.bid_amount,
                                    bidder = supp.comp_name
                                });


                ReportGenerator gen = new ReportGenerator();

                gen.AddDataSource("dsProjectRequest", projRequest);

                gen.AddDataSource("dsProjectBids", projBids);

                Result<byte[]> _result = new Result<byte[]>();

                _result.value = gen.GetPdf("LCBMemo");


                return _result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Result<byte[]> LCBNotice(Payload payload)
        {
            try
            {

                var qry = (from proj in db.ProjectRequests
                           join pb in db.ProjectBid on proj.id equals pb.project_request_id
                           join supp in db.Supplier on pb.created_by equals supp.user_id
                           where proj.id == payload.id && pb.bid_status == Common.Constant.BidStatus.Awarded
                           select new
                           {
                               project_title = proj.title,
                               supplier_name = supp.comp_name,
                               supplier_address = supp.address,
                               supplier_contact_person = supp.contact_person
                           });

                var qryItem = qry.FirstOrDefault();

                List<Report.Datasets.ProjectRequestDS> projRequest = new List<Report.Datasets.ProjectRequestDS>();
                List<Report.Datasets.SupplierDS> supplier = new List<Report.Datasets.SupplierDS>();

                if (qryItem != null)
                {
                    projRequest.Add(new Report.Datasets.ProjectRequestDS() { title = qryItem.project_title });

                    supplier.Add(new Report.Datasets.SupplierDS() {
                        comp_name = qryItem.supplier_name,
                        contact_person = qryItem.supplier_contact_person,
                        address = qryItem.supplier_address
                    });
                }

                ReportGenerator gen = new ReportGenerator();

                gen.AddDataSource("dsProjectRequest", projRequest);

                gen.AddDataSource("dsSupplier", supplier);

                Result<byte[]> _result = new Result<byte[]>();

                _result.value = gen.GetPdf("LCB");


                return _result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Result<byte[]> NOP(Payload payload)
        {
            try
            {

                var qry = (from proj in db.ProjectRequests
                           join pb in db.ProjectBid on proj.id equals pb.project_request_id
                           join supp in db.Supplier on pb.created_by equals supp.user_id
                           where proj.id == payload.id && pb.bid_status == Common.Constant.BidStatus.Awarded
                           select new
                           {
                               project_title = proj.title,
                               project_start_date = proj.start_date,
                               supplier_name = supp.comp_name,
                               supplier_address = supp.address,
                               supplier_contact_person = supp.contact_person
                           });

                var qryItem = qry.FirstOrDefault();

                List<Report.Datasets.ProjectRequestDS> projRequest = new List<Report.Datasets.ProjectRequestDS>();
                List<Report.Datasets.SupplierDS> supplier = new List<Report.Datasets.SupplierDS>();

                string custom1_dateformat = "";
                if (qryItem != null)
                {
                    projRequest.Add(new Report.Datasets.ProjectRequestDS() { title = qryItem.project_title });

                    supplier.Add(new Report.Datasets.SupplierDS()
                    {
                        comp_name = qryItem.supplier_name,
                        contact_person = qryItem.supplier_contact_person,
                        address = qryItem.supplier_address
                    });

                    if (qryItem.project_start_date.HasValue)
                    {
                        DateTime start_date = qryItem.project_start_date.Value;

                        custom1_dateformat = start_date.Day.ToString() + CommonMethods.getDaySuffix(start_date.Day) + start_date.ToString("' of' MMMM, yyyy");

                    }
                }

                ReportGenerator gen = new ReportGenerator();

               
                gen.AddParameter("custom1", custom1_dateformat);

                gen.AddDataSource("dsProjectRequest", projRequest);

                gen.AddDataSource("dsSupplier", supplier);

                Result<byte[]> _result = new Result<byte[]>();

                _result.value = gen.GetPdf("NOP");


                return _result;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        
    }
}


