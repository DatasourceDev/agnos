using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFramework.Control;
using AgnosModel.Models;
using AppFramework;
using System.Configuration;
using System.IO;
//using Enterprise01;
using AppFramework.Util;
namespace AgnosModel.Service 
{
    //public class ExchequerService
    //{
    //    readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //    public Toolkit InitializeToolkit()
    //    {
    //        try
    //        {
    //            logger.Debug("InitializeToolkit");
    //            logger.Debug("Directory " + ConfigurationManager.AppSettings["ToolKitEnterpriseDirectory"]);

    //            if (!Directory.Exists(ConfigurationManager.AppSettings["ToolKitEnterpriseDirectory"]))
    //                logger.Debug("Not Have Directory " + ConfigurationManager.AppSettings["ToolKitEnterpriseDirectory"]);

    //            logger.Debug("Directory " + ConfigurationManager.AppSettings["ToolKitDataDirectory"]);
    //            if (!Directory.Exists(ConfigurationManager.AppSettings["ToolKitDataDirectory"]))
    //                logger.Debug("Not Have Directory " + ConfigurationManager.AppSettings["ToolKitDataDirectory"]);


    //            var _toolkit = new Toolkit();
    //            logger.Debug("New Toolkit sucessfully.");
    //            _toolkit.Configuration.EnterpriseDirectory = ConfigurationManager.AppSettings["ToolKitEnterpriseDirectory"];
    //            logger.Debug("EnterpriseDirectory sucessfully.");
    //            _toolkit.Configuration.DataDirectory = ConfigurationManager.AppSettings["ToolKitDataDirectory"];
    //            logger.Debug("DataDirectory sucessfully.");
    //            return _toolkit;
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.Error(ex);
    //            throw;
    //        }
    //    }

    //    public List<ComboRow> LstExchquerProduct(bool hasBlank = false)
    //    {
    //        using (var db = new AgnosDBContext())
    //        {
    //            var clist = new List<ComboRow>();

    //            if (hasBlank)
    //                clist.Add(new ComboRow { Value = "", Text = "-", Desc = "-" });

    //            var _toolkit = InitializeToolkit();

    //            var result = new ServiceResult();
    //            try
    //            {
    //                if (_toolkit.Status == TToolkitStatus.tkClosed) _toolkit.OpenToolkit();
    //                var txStock = (IStock7)_toolkit.Stock;
    //                txStock.Index = TStockIndex.stIdxCode;

    //                txStock.GetFirst();
    //                var retVal = 0;
    //                while (retVal == 0)
    //                {

    //                    if ((txStock.stType == TStockType.stTypeProduct) || (txStock.stType == TStockType.stTypeBillOfMaterials))
    //                    {
    //                        var stkCode = txStock.stCode;
    //                        var stkDesc = txStock.stDesc[1];
    //                        clist.Add(new ComboRow() { Value = stkCode.Trim(), Text = stkCode.Trim(), Desc = stkDesc });
    //                    }
    //                    //if (clist.Count >= 10)
    //                    //break;
    //                    retVal = txStock.GetNext();
    //                }
    //                // result.Object = clist;
    //                result.Code = ReturnCode.SUCCESS;
    //            }
    //            catch
    //            {

    //            }
    //            finally
    //            {
    //                _toolkit.CloseToolkit();
    //            }

    //            return clist;
    //        }
    //    }

    //    public ServiceResult GetExchquerProductTrans(ProductTransCriteria cri = null)
    //    {
    //        var currentdate = StoredProcedure.GetCurrentDate();
    //        var trans = new List<Product_Transaction>();
    //        var result = new ServiceResult();
    //        var _toolkit = InitializeToolkit();
    //        try
    //        {
    //            logger.Debug("open toolkit.");
    //            if (_toolkit.Status == TToolkitStatus.tkClosed) _toolkit.OpenToolkit();
    //            logger.Debug("opened toolkit.");
    //            var txStock = (IStock7)_toolkit.Stock;
    //            txStock.Index = TStockIndex.stIdxCode;
    //            txStock.GetFirst();
    //            var retVal = 0;

    //            while (retVal == 0)
    //            {
    //                //Product to be shown both products and BOM
    //                if ((txStock.stType == TStockType.stTypeProduct) || (txStock.stType == TStockType.stTypeBillOfMaterials))
    //                {
    //                    if (txStock.stCode.Trim().ToLower() == cri.Product_Code.Trim().ToLower())
    //                    {
    //                        var stkCode = txStock.stCode.Trim();
    //                        if (txStock.stValuationMethod == TStockValuationType.stValSerial)
    //                        {
    //                            ISerialBatch stkSerial = (ISerialBatch)txStock.stSerialBatch;
    //                            stkSerial.Index = TSerialBatchIndex.snIdxSerialNo;
    //                            int a = stkSerial.GetFirst();
    //                            var batchno = "";
    //                            while (a == 0)
    //                            {

    //                                if ((batchno != stkSerial.snBatchNo) && ((decimal)(stkSerial.snBatchQuantity) > 0))
    //                                // &&  (stkSerial.snOutDate != "")
    //                                //if ((decimal)(stkSerial.snBatchQuantity) > 0)
    //                                {

    //                                    trans.Add(new Product_Transaction()
    //                                    {
    //                                        Transaction_ID = stkCode + stkSerial.snBatchNo,
    //                                        Product_Code = stkCode,
    //                                        Product_Name = stkCode, //txStock.stDesc[1],
    //                                        Lot_No = stkSerial.snBatchNo, //"CA-1215A020800" + i,
    //                                        Unit = txStock.stUnitOfStock,// "AC-000" + i,
    //                                        Total_Receiving = (decimal)(stkSerial.snBatchQuantity),// i + 3493,
    //                                        Receiving_Date = currentdate,
    //                                        Expiring_Date = DateUtil.ToDisplayDate(DateUtil.ToDate2(stkSerial.snUseByDate))
    //                                        //Expiring_Date =  DateTime.ParseExact(stkSerial.snUseByDate, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).ToString("dd/MM/yyyy") //retrieve from exchq is yyyymmdd
    //                                    });
    //                                    batchno = stkSerial.snBatchNo;
    //                                }
    //                                a = stkSerial.GetNext();
    //                            }
    //                        }
    //                        else
    //                        {
    //                            //In case of product is using FIFO
    //                            trans.Add(new Product_Transaction()
    //                            {
    //                                Transaction_ID = "NA",
    //                                Product_Code = stkCode,
    //                                Product_Name = stkCode, //txStock.stDesc[1],
    //                                Lot_No = "", //"CA-1215A020800" + i,
    //                                Unit = txStock.stUnitOfStock,// "AC-000" + i,
    //                                Total_Receiving = (decimal)(txStock.stQtyInStock),// i + 3493,
    //                                Receiving_Date = currentdate,
    //                                Expiring_Date = null
    //                            });

    //                        }
    //                    }
    //                }
    //                // if (trans.Count == 1)
    //                //break;

    //                retVal = txStock.GetNext();
    //            }
    //            result.Code = ReturnCode.SUCCESS; ;
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.Error(ex);
    //        }
    //        finally
    //        {
    //            _toolkit.CloseToolkit();
    //        }
    //        result.Object = trans;
    //        return result;
    //    }


    //    public List<ComboRow> LstExchquerWorkOrder(bool hasBlank = false)
    //    {
    //        using (var db = new AgnosDBContext())
    //        {
    //            var clist = new List<ComboRow>();
    //            var _toolkit = InitializeToolkit();

    //            var result = new ServiceResult();
    //            try
    //            {
    //                var trans = new List<Product_Transaction>();
    //                if (_toolkit.Status == TToolkitStatus.tkClosed) _toolkit.OpenToolkit();
    //                var tx = (ITransaction9)_toolkit.Transaction;
    //                tx.Index = TTransactionIndex.thIdxOurRef;

    //                tx.GetFirst();
    //                var retVal = 0;

    //                while (retVal == 0)
    //                {
    //                    if (tx.thDocType == TDocTypes.dtWOR)
    //                    {
    //                        var txWORCode = tx.thOurRef;
    //                        clist.Add(new ComboRow() { Value = txWORCode, Text = txWORCode, Desc = txWORCode });
    //                    }
    //                    retVal = tx.GetNext();
    //                }
    //                result.Object = trans;
    //                result.Code = ReturnCode.SUCCESS;
    //            }
    //            catch
    //            {

    //            }
    //            finally
    //            {
    //                _toolkit.CloseToolkit();
    //            }

    //            return clist;
    //        }
    //    }


    //    public void SetExchquerWorkOrder(Logsheet logsheet = null)
    //    {
    //        var currentdate = StoredProcedure.GetCurrentDate();
    //        var _toolkit = InitializeToolkit();
    //        var tx = (ITransaction9)_toolkit.Transaction;
    //        tx.Index = TTransactionIndex.thIdxOurRef;

    //        try
    //        {
    //            logger.Debug("open toolkit.");
    //            if (_toolkit.Status == TToolkitStatus.tkClosed) _toolkit.OpenToolkit();
    //            logger.Debug("opened toolkit.");
    //            int retVal = tx.GetEqual(tx.BuildOurRefIndex(logsheet.Work_Order_No));

    //            if (retVal == 0)
    //            {
    //                var txUpdate = (ITransaction9)tx.Update();
    //                var txLines = txUpdate.thLines;
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            logger.Error(ex);
    //        }
    //        finally
    //        {
    //            _toolkit.CloseToolkit();
    //        }
    //    }


    //}
   /* test data*/
  public class ExchequerService
  {
      readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

      public List<ComboRow> LstExchquerProduct(bool hasBlank = false)
      {
          using (var db = new AgnosDBContext())
          {
              var clist = new List<ComboRow>();
              if (hasBlank)
                  clist.Add(new ComboRow { Value = "", Text = "-" });
              clist.Add(new ComboRow() { Value = "P0001", Text = "P0001", Desc = "P0001 Name" });
              clist.Add(new ComboRow() { Value = "P0002", Text = "P0002", Desc = "P0002 Name" });
              clist.Add(new ComboRow() { Value = "P0003", Text = "P0003", Desc = "P0003 Name" });
              return clist;
          }
      }

      public ServiceResult GetExchquerProductTrans(ProductTransCriteria cri = null)
      {
          var currentdate = StoredProcedure.GetCurrentDate();
          var trans = new List<Product_Transaction>();
          var result = new ServiceResult();


          for (var i = 1; i < 6; i++)
          {
              trans.Add(new Product_Transaction()
              {
                  Transaction_ID = i.ToString(),
                  Product_Code = cri.Product_Code,
                  Product_Name = "XXXXXXXXX",
                  Lot_No = "CA-1215A020800" + i,
                  Unit = "AC-000" + i,
                  Total_Receiving = i + 3493,
                  Receiving_Date = currentdate
              });
          }
          result.Code = ReturnCode.SUCCESS;
          result.Object = trans;
          return result;
      }



      public List<ComboRow> LstExchquerWorkOrder(bool hasBlank = false)
      {
          using (var db = new AgnosDBContext())
          {
              var clist = new List<ComboRow>();
              return clist;
          }
      }

      public void SetExchquerWorkOrder(Logsheet logsheet = null)
      {

      }
  }

}
