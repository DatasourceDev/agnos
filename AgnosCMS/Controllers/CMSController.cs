using AgnosCMS.Models;
using AgnosModel.Models;
using AgnosModel.Service;
using AppFramework;
using AppFramework.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SBSResourceAPI;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using AgnosModel;
using AppFramework.Util;

//using AxSerial;

using System.Diagnostics;
using AgnosCMS.Common;

namespace AgnosCMS.Controllers
{
    public class CMSController : ControllerBase
    {
        #region CMS Setup
        // GET: Configuration
        [HttpGet]
        public ActionResult CMSSetup(CMSSetupViewModel model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0009);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            if (model.operation == Operation.D)
                return CMSSetup(model);

            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post

            var cmsService = new CMSService();
            var cbService = new ComboService();

            var formatresult = cmsService.GetFormat();
            if (formatresult.Code == ReturnCode.SUCCESS)
                model.CMS_Formatlist = formatresult.Object as List<CMS_Format>;

            var drumresult = cmsService.GetDrumType();
            if (drumresult.Code == ReturnCode.SUCCESS)
                model.CMS_Drum_Typelist = drumresult.Object as List<CMS_Drum_Type>;

            var skipresult = cmsService.GetSkipPurg();
            if (skipresult.Code == ReturnCode.SUCCESS)
                model.CMS_Skip_Purginglist = skipresult.Object as List<CMS_Skip_Purging>;

            var chargeresult = cmsService.GetChargingControl();
            if (chargeresult.Code == ReturnCode.SUCCESS)
                model.CMS_Charging_Controllist = chargeresult.Object as List<CMS_Charging_Control>;

            var fillingStationresult = cmsService.GetFillingStation();
            if (fillingStationresult.Code == ReturnCode.SUCCESS)
                model.CMS_Filling_Stationlist = fillingStationresult.Object as List<CMS_Filling_Station>;

            var product = cmsService.GetProduct();
            if (product.Code == ReturnCode.SUCCESS)
                model.CMS_Productlist = product.Object as List<CMS_Product>;

            var drumControlresult = cmsService.GetDrumControl();
            if (drumControlresult.Code == ReturnCode.SUCCESS)
                model.CMS_Drum_Controllist = drumControlresult.Object as List<CMS_Drum_Control>;

            model.cProduct_ByCodelist = cbService.LstProductByCode();
            model.cProduct_Codelist = cbService.LstProduct();
            model.cStationlist = cbService.LstFillingStationType();
            model.cDrum_Typelist = cbService.LstDrumType();
            model.cActionlist = cbService.LstAction();

            return View(model);
        }

        [HttpPost]
        public ActionResult CMSSetup(CMSSetupViewModel model)
        {
            var cmsService = new CMSService(User.Identity.GetUserId());
            var cbService = new ComboService();

            if (model.tabAction == "format")
            {
                #region Format
                List<string> formatTemp = new List<string>();
                formatTemp.AddRange(
                   new string[] { "Format_Code_Format", "Lot_No_Length_Format", "Product_Code_Length_Format" }
                );

                foreach (var key in ModelState.Keys.ToList()
                   .Where(key => ModelState.ContainsKey(key) && (!formatTemp.Contains(key))))
                    ModelState[key].Errors.Clear();

                if (model.operation != Operation.D)
                {
                    if (ModelState.IsValid)
                    {
                        var criGet = new FormatCriteria();
                        criGet.Lot_No_Length = model.Lot_No_Length_Format;
                        criGet.Product_Code_Length = model.Product_Code_Length_Format;
                        var result = cmsService.GetFormat(criGet);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Format>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.Format_ID != model.Format_ID)
                                            ModelState.AddModelError("Lot_No_Length_Format", Resource.Message_Is_Duplicated);
                                    }
                                    else
                                        ModelState.AddModelError("Lot_No_Length_Format", Resource.Message_Is_Duplicated);
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var format = new CMS_Format();
                    if (model.operation == Operation.U || model.operation == Operation.D)
                    {
                        var cri = new FormatCriteria();
                        cri.Format_ID = model.Format_ID;
                        var result = cmsService.GetFormat(cri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var formats = result.Object as List<CMS_Format>;
                            if (formats != null && formats.Count() == 1)
                                format = formats.FirstOrDefault();
                        }
                    }

                    if (model.operation != Operation.D)
                    {
                        format.Format_Code = model.Format_Code_Format;
                        format.Lot_No_Length = model.Lot_No_Length_Format;
                        format.Product_Code_Length = model.Product_Code_Length_Format;
                    }

                    if (model.operation == Operation.C)
                        model.result = cmsService.InsertFormat(format);

                    else if (model.operation == Operation.U)
                        model.result = cmsService.UpdateFormat(format);

                    else if (model.operation == Operation.D)
                    {
                        format.Record_Status = Record_Status.Delete;
                        model.result = cmsService.UpdateFormat(format);
                        if (model.result.Code == ReturnCode.SUCCESS)
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Formatting };
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Formatting };

                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                }
                #endregion
            }
            else if (model.tabAction == "drumTypes")
            {
                #region Drum Types
                List<string> drumTemp = new List<string>();
                drumTemp.AddRange(new string[] { "Drum_Type_Drum" });

                foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!drumTemp.Contains(key))))
                    ModelState[key].Errors.Clear();

                if (model.operation != Operation.D)
                {
                    if (!string.IsNullOrEmpty(model.Drum_Type_Drum))
                    {
                        var dlength = model.Drum_Type_Drum.Length;
                        var flength = AppSetting.Drum_Type_Length;
                        if (dlength != flength)
                            ModelState.AddModelError("Drum_Type_Drum", Resource.Message_Is_Invalid + " (Please enter " + flength + " characters.)");
                    }

                    if (ModelState.IsValid)
                    {
                        var criGet = new DrumCriteria();
                        criGet.Drum_Type_Equals = model.Drum_Type_Drum;
                        var result = cmsService.GetDrumType(criGet);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Drum_Type>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.Drum_Type_ID != model.Drum_Type_ID)
                                            ModelState.AddModelError("Drum_Type_Drum", Resource.Message_Is_Duplicated);
                                    }
                                    else
                                        ModelState.AddModelError("Drum_Type_Drum", Resource.Message_Is_Duplicated);
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var drumType = new CMS_Drum_Type();
                    if (model.operation == Operation.U || model.operation == Operation.D)
                    {
                        var cri = new DrumCriteria();
                        cri.Drum_Type_ID = model.Drum_Type_ID;
                        var result = cmsService.GetDrumType(cri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var drumTypes = result.Object as List<CMS_Drum_Type>;
                            if (drumTypes != null && drumTypes.Count() == 1)
                                drumType = drumTypes.FirstOrDefault();
                        }
                    }

                    if (model.operation != Operation.D)
                        drumType.Drum_Type = model.Drum_Type_Drum;

                    if (model.operation == Operation.C)
                        model.result = cmsService.InsertDrumType(drumType);

                    else if (model.operation == Operation.U)
                        model.result = cmsService.UpdateDrumType(drumType);

                    else if (model.operation == Operation.D)
                    {
                        drumType.Record_Status = Record_Status.Delete;
                        model.result = cmsService.UpdateDrumType(drumType);
                        if (model.result.Code == ReturnCode.SUCCESS)
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Drum_Types };
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Drum_Types };

                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                }
                #endregion
            }
            else if (model.tabAction == "skipPurging")
            {
                #region Skip Purging
                List<string> skipTemp = new List<string>();
                skipTemp.AddRange(new string[] { "Product_Code_Skip", "Drum_Type_ID_Skip" });

                foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!skipTemp.Contains(key))))
                    ModelState[key].Errors.Clear();

                if (model.operation != Operation.D)
                {
                    if (ModelState.IsValid)
                    {
                        var criGet = new SkipPurgCriteria();
                        criGet.Product_Code_Equals = model.Product_Code_Skip;
                        criGet.Drum_Type_ID = model.Drum_Type_ID_Skip;
                        var result = cmsService.GetSkipPurg(criGet);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Skip_Purging>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.Skip_Purging_ID != model.Skip_Purging_ID)
                                        {
                                            ModelState.AddModelError("Product_Code_Skip", Resource.Message_Is_Duplicated);
                                            ModelState.AddModelError("Drum_Type_ID_Skip", Resource.Message_Is_Duplicated);
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Product_Code_Skip", Resource.Message_Is_Duplicated);
                                        ModelState.AddModelError("Drum_Type_ID_Skip", Resource.Message_Is_Duplicated);
                                    }
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var skipPurging = new CMS_Skip_Purging();
                    if (model.operation == Operation.U || model.operation == Operation.D)
                    {
                        var cri = new SkipPurgCriteria();
                        cri.Skip_Purging_ID = model.Skip_Purging_ID;
                        var result = cmsService.GetSkipPurg(cri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var skipPurgings = result.Object as List<CMS_Skip_Purging>;
                            if (skipPurgings != null && skipPurgings.Count() == 1)
                                skipPurging = skipPurgings.FirstOrDefault();
                        }
                    }

                    if (model.operation != Operation.D)
                    {
                        skipPurging.Product_Code = model.Product_Code_Skip;
                        skipPurging.Drum_Type_ID = model.Drum_Type_ID_Skip;
                    }

                    if (model.operation == Operation.C)
                        model.result = cmsService.InsertSkipPurg(skipPurging);

                    else if (model.operation == Operation.U)
                        model.result = cmsService.UpdateSkipPurg(skipPurging);

                    else if (model.operation == Operation.D)
                    {
                        skipPurging.Record_Status = Record_Status.Delete;
                        model.result = cmsService.UpdateSkipPurg(skipPurging);
                        if (model.result.Code == ReturnCode.SUCCESS)
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Skip_Purging };
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Skip_Purging };

                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                }
                #endregion
            }
            else if (model.tabAction == "chargingControl")
            {
                #region Charging Control
                List<string> chargingTemp = new List<string>();
                chargingTemp.AddRange(new string[] { "Drum_Code_Charging", "Max_Of_Change_Charging", "Action_Charging" });

                foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!chargingTemp.Contains(key))))
                    ModelState[key].Errors.Clear();

                if (model.operation != Operation.D)
                {
                    if (ModelState.IsValid)
                    {
                        var isCheck = checkDrumControl(model.Drum_Code_Charging);
                        if (isCheck != true)
                            ModelState.AddModelError("Drum_Code_Charging", Resource.Message_Is_Invalid + " please configuration in CMS Setup page.(Drum Control)");
                    }

                    if (ModelState.IsValid)
                    {
                        var criGet = new ChargingControlCriteria();
                        criGet.Drum_Code = model.Drum_Code_Charging;
                        var result = cmsService.GetChargingControl(criGet);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Charging_Control>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.Charging_Control_ID != model.Charging_Control_ID)
                                            ModelState.AddModelError("Drum_Code_Charging", Resource.Message_Is_Duplicated);
                                    }
                                    else
                                        ModelState.AddModelError("Drum_Code_Charging", Resource.Message_Is_Duplicated);
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var ChargingControl = new CMS_Charging_Control();
                    if (model.operation == Operation.U || model.operation == Operation.D)
                    {
                        var cri = new ChargingControlCriteria();
                        cri.Charging_Control_ID = model.Charging_Control_ID;
                        var result = cmsService.GetChargingControl(cri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var ChargingControls = result.Object as List<CMS_Charging_Control>;
                            if (ChargingControls != null && ChargingControls.Count() == 1)
                                ChargingControl = ChargingControls.FirstOrDefault();
                        }
                    }

                    if (model.operation != Operation.D)
                    {
                        ChargingControl.Drum_Code = model.Drum_Code_Charging;
                        ChargingControl.Max_Of_Change = model.Max_Of_Change_Charging;
                        ChargingControl.Action = model.Action_Charging;
                    }

                    if (model.operation == Operation.C)
                        model.result = cmsService.InsertChargingControl(ChargingControl);

                    else if (model.operation == Operation.U)
                        model.result = cmsService.UpdateChargingControl(ChargingControl);

                    else if (model.operation == Operation.D)
                    {
                        ChargingControl.Record_Status = Record_Status.Delete;
                        model.result = cmsService.UpdateChargingControl(ChargingControl);
                        if (model.result.Code == ReturnCode.SUCCESS)
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Charging_Control };
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Charging_Control };

                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                }
                #endregion
            }
            else if (model.tabAction == "fillingStations")
            {
                #region Filling Stations
                List<string> fillingTemp = new List<string>();
                fillingTemp.AddRange(new string[] { "Station_Code_Filling" });

                foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!fillingTemp.Contains(key))))
                    ModelState[key].Errors.Clear();

                if (model.operation != Operation.D)
                {
                    if (ModelState.IsValid)
                    {
                        var criGet = new FillingStationCriteria();
                        criGet.Station_Code_Equals = model.Station_Code_Filling;
                        var result = cmsService.GetFillingStation(criGet);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Filling_Station>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.Filling_Station_ID != model.Filling_Station_ID)
                                            ModelState.AddModelError("Station_Code_Filling", Resource.Message_Is_Duplicated);
                                    }
                                    else
                                        ModelState.AddModelError("Station_Code_Filling", Resource.Message_Is_Duplicated);
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var fillingStation = new CMS_Filling_Station();
                    if (model.operation == Operation.U || model.operation == Operation.D)
                    {
                        var cri = new FillingStationCriteria();
                        cri.Filling_Station_ID = model.Filling_Station_ID;
                        var result = cmsService.GetFillingStation(cri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var fillingStations = result.Object as List<CMS_Filling_Station>;
                            if (fillingStations != null && fillingStations.Count() == 1)
                                fillingStation = fillingStations.FirstOrDefault();
                        }
                    }

                    if (model.operation != Operation.D)
                        fillingStation.Station_Code = model.Station_Code_Filling;

                    if (model.operation == Operation.C)
                        model.result = cmsService.InsertFillingStation(fillingStation);

                    else if (model.operation == Operation.U)
                        model.result = cmsService.UpdateFillingStation(fillingStation);

                    else if (model.operation == Operation.D)
                    {
                        fillingStation.Record_Status = Record_Status.Delete;
                        model.result = cmsService.UpdateFillingStation(fillingStation);
                        if (model.result.Code == ReturnCode.SUCCESS)
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Filling_Stations };
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Filling_Stations };

                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                }
                #endregion
            }
            else if (model.tabAction == "product")
            {
                #region CMS Product
                List<string> productTemp = new List<string>();
                productTemp.AddRange(new string[] { "Filling_Station_ID_Product", "Product_Code", "Product_Name" });

                foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!productTemp.Contains(key))))
                    ModelState[key].Errors.Clear();

                if (model.operation != Operation.D)
                {

                    if (!string.IsNullOrEmpty(model.Product_Code))
                    {
                        var plength = model.Product_Code.Trim().Length;
                        if (plength != 5 && plength != 6)
                            ModelState.AddModelError("Product_Code", Resource.Message_Is_Invalid + " (Please enter 5 or 6 characters.)");
                    }

                    if (ModelState.IsValid)
                    {
                        var crit = new ProductCriteria();
                        crit.Product_Code_Equals = model.Product_Code;
                        crit.Filling_Station_ID = model.Filling_Station_ID_Product;
                        var result = cmsService.GetProduct(crit);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Product>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.CMS_Product_ID != model.CMS_Product_ID)
                                            ModelState.AddModelError("Product_Code", Resource.Message_Is_Duplicated);
                                    }
                                    else
                                        ModelState.AddModelError("Product_Code", Resource.Message_Is_Duplicated);
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var Product = new CMS_Product();
                    if (model.operation == Operation.U || model.operation == Operation.D)
                    {
                        var cri = new ProductCriteria();
                        cri.CMS_Product_ID = model.CMS_Product_ID;
                        var result = cmsService.GetProduct(cri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var Products = result.Object as List<CMS_Product>;
                            if (Products != null && Products.Count() == 1)
                                Product = Products.FirstOrDefault();
                        }
                    }

                    if (model.operation != Operation.D)
                    {
                        Product.Filling_Station_ID = model.Filling_Station_ID_Product;
                        Product.Product_Code = model.Product_Code.Trim();
                        Product.Product_Name = model.Product_Name;
                    }

                    if (model.operation == Operation.C)
                        model.result = cmsService.InsertProduct(Product);

                    else if (model.operation == Operation.U)
                        model.result = cmsService.UpdateProduct(Product);

                    else if (model.operation == Operation.D)
                    {
                        Product.Record_Status = Record_Status.Delete;
                        model.result = cmsService.UpdateProduct(Product);
                        if (model.result.Code == ReturnCode.SUCCESS)
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Product_List };
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Product_List };

                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                }
                #endregion
            }
            else if (model.tabAction == "drumControl")
            {
                #region Drum Control
                List<string> chargingTemp = new List<string>();
                chargingTemp.AddRange(new string[] { "Product_Code_Control", "Drum_Type_ID_Control", "Running_Number_Control" });

                foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey(key) && (!chargingTemp.Contains(key))))
                    ModelState[key].Errors.Clear();

                if (model.Running_Number_Control.HasValue)
                {
                    if (model.Running_Number_Control > 0 && model.Running_Number_Control < 20)
                    {

                    }
                    else
                        ModelState.AddModelError("Running_Number_Control", Resource.Message_Is_Invalid);
                }

                if (model.operation != Operation.D)
                {
                    if (ModelState.IsValid)
                    {
                        var criGet = new DrumControlCriteria();
                        criGet.Product_Code_Equals = model.Product_Code_Control;
                        criGet.Drum_Type_ID = model.Drum_Type_ID_Control;
                        criGet.Running_Number = model.Running_Number_Control;
                        var result = cmsService.GetDrumControl(criGet);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Drum_Control>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.Drum_Control_ID != model.Drum_Control_ID)
                                        {
                                            ModelState.AddModelError("Product_Code_Control", Resource.Message_Is_Duplicated);
                                            ModelState.AddModelError("Drum_Type_ID_Control", Resource.Message_Is_Duplicated);
                                            ModelState.AddModelError("Running_Number_Control", Resource.Message_Is_Duplicated);
                                        }
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("Product_Code_Control", Resource.Message_Is_Duplicated);
                                        ModelState.AddModelError("Drum_Type_ID_Control", Resource.Message_Is_Duplicated);
                                        ModelState.AddModelError("Running_Number_Control", Resource.Message_Is_Duplicated);
                                    }
                                }
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    var DrumControl = new CMS_Drum_Control();
                    if (model.operation == Operation.U || model.operation == Operation.D)
                    {
                        var cri = new DrumControlCriteria();
                        cri.Drum_Control_ID = model.Drum_Control_ID;
                        var result = cmsService.GetDrumControl(cri);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var DrumControls = result.Object as List<CMS_Drum_Control>;
                            if (DrumControls != null && DrumControls.Count() == 1)
                                DrumControl = DrumControls.FirstOrDefault();
                        }
                    }

                    if (model.operation != Operation.D)
                    {
                        DrumControl.Product_Code = model.Product_Code_Control;
                        DrumControl.Drum_Type_ID = model.Drum_Type_ID_Control;
                        DrumControl.Running_Number = model.Running_Number_Control;
                    }

                    if (model.operation == Operation.C)
                        model.result = cmsService.InsertDrumControl(DrumControl);

                    else if (model.operation == Operation.U)
                        model.result = cmsService.UpdateDrumControl(DrumControl);

                    else if (model.operation == Operation.D)
                    {
                        DrumControl.Record_Status = Record_Status.Delete;
                        model.result = cmsService.UpdateDrumControl(DrumControl);
                        if (model.result.Code == ReturnCode.SUCCESS)
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.Drum_Control };
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.Drum_Control };

                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                    }

                    if (model.result.Code == ReturnCode.SUCCESS)
                        return RedirectToAction("CMSSetup", new AppRouteValueDictionary(model));
                }
                #endregion
            }

            var formatresult = cmsService.GetFormat();
            if (formatresult.Code == ReturnCode.SUCCESS)
                model.CMS_Formatlist = formatresult.Object as List<CMS_Format>;

            var drumresult = cmsService.GetDrumType();
            if (drumresult.Code == ReturnCode.SUCCESS)
                model.CMS_Drum_Typelist = drumresult.Object as List<CMS_Drum_Type>;

            var skipresult = cmsService.GetSkipPurg();
            if (skipresult.Code == ReturnCode.SUCCESS)
                model.CMS_Skip_Purginglist = skipresult.Object as List<CMS_Skip_Purging>;

            var chargeresult = cmsService.GetChargingControl();
            if (chargeresult.Code == ReturnCode.SUCCESS)
                model.CMS_Charging_Controllist = chargeresult.Object as List<CMS_Charging_Control>;

            var fillingStationresult = cmsService.GetFillingStation();
            if (fillingStationresult.Code == ReturnCode.SUCCESS)
                model.CMS_Filling_Stationlist = fillingStationresult.Object as List<CMS_Filling_Station>;

            var product = cmsService.GetProduct();
            if (product.Code == ReturnCode.SUCCESS)
                model.CMS_Productlist = product.Object as List<CMS_Product>;

            var drumControlresult = cmsService.GetDrumControl();
            if (drumControlresult.Code == ReturnCode.SUCCESS)
                model.CMS_Drum_Controllist = drumControlresult.Object as List<CMS_Drum_Control>;

            model.cProduct_ByCodelist = cbService.LstProductByCode();
            model.cProduct_Codelist = cbService.LstProduct();
            model.cStationlist = cbService.LstFillingStationType();
            model.cDrum_Typelist = cbService.LstDrumType();
            model.cActionlist = cbService.LstAction();

            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0009);
            if (prole != null)
                model.Modify = prole.Modify;

            return View(model);
        }

        #endregion

        #region CMS Purge

        [HttpGet]
        public ActionResult CMSPurge(CMSPurgeViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0012);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post
            var cmsService = new CMSService();

            var cri = new CMSCriteria();
            cri.Drum_Code = model.Drum_Code;
            cri.Date_From = model.Date_From;
            cri.Date_To = model.Date_To;
            cri.Not_Yet_Deliver = true; //show only not yet deliver 
            var result = cmsService.GetCMSPurge(cri);
            if (result.Code == ReturnCode.SUCCESS)
                model.purgelist = result.Object as List<CMS_Purge>;

            return View(model);
        }

        [HttpGet]
        public ActionResult CMSPurgeInfo(CMSPurgeViewModels pmodel, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0012);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            var model = new CMSPurgeInfoViewModels();
            model.operation = pmodel.operation;
            model.Purge_ID = pmodel.Purge_ID;
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult;
            var cmsService = new CMSService();

            if (model.operation == Operation.C)
            {

            }
            else if (model.operation == Operation.U)
            {
                var cri = new CMSCriteria();
                cri.Purge_ID = model.Purge_ID;
                cri.include = false;
                var result = cmsService.GetCMSPurge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var purges = result.Object as List<CMS_Purge>;
                    if (purges != null && purges.Count() == 1)
                    {
                        var purge = purges.FirstOrDefault();
                        model.Drum_Code = purge.Drum_Code;
                        model.Initial_Weight = purge.Initial_Weight;
                        model.Final_Weight = purge.Final_Weight;
                        if (purge.Delivery_Status == Delivery_Status.Completed)
                            model.Completed = true;
                    }
                }
            }
            else if (model.operation == Operation.D)
                return CMSPurgeInfo(model);

            return View(model);
        }

        [HttpPost]
        public ActionResult CMSPurgeInfo(CMSPurgeInfoViewModels model)
        {
            var cmsService = new CMSService(User.Identity.GetUserId());
            model.result = new ServiceResult();

            if (model.operation != Operation.D)
            {
                if (ModelState.IsValid)
                {
                    if (model.operation == Operation.C)
                    {
                        var isCheck = checkDrumControl(model.Drum_Code);
                        if (isCheck != true)
                        {
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND) + " please configuration in CMS Setup page.(Drum Control)", Field = Resource.Drum_Code };
                            ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);
                        }
                        else
                        {
                            var isUse = checkDrumUse(model.Drum_Code);
                            if (isUse == true)
                            {
                                model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_IN_USE, Msg = "has been charged but not yet delivered", Field = Resource.Drum_Code };
                                ModelState.AddModelError("Drum_Code", Resource.Message_Is_Duplicated);
                            }
                            else
                            {
                                var InCharge = checkDrumInCharge(model.Drum_Code);
                                if (InCharge != true)
                                {
                                    model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND) + "please create information in CMS Charge page.", Field = Resource.Drum_Code };
                                    ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);
                                }
                            }
                        }
                    }

                    if (ModelState.IsValid)
                    {
                        var criGet = new CMSCriteria();
                        criGet.Drum_Code = model.Drum_Code;
                        criGet.Not_Yet_Deliver = true;
                        var result = cmsService.GetCMSPurge(criGet);
                        if (result.Code == ReturnCode.SUCCESS)
                        {
                            var dups = result.Object as List<CMS_Purge>;
                            if (dups != null && dups.Count() > 0)
                            {
                                var dup = dups.FirstOrDefault();
                                if (dup != null)
                                {
                                    if (model.operation == Operation.U)
                                    {
                                        if (dup.Purge_ID != model.Purge_ID)
                                            ModelState.AddModelError("Drum_Code", Resource.Message_Is_Duplicated + " please go thru filling process.");
                                    }
                                    else
                                        ModelState.AddModelError("Drum_Code", Resource.Message_Is_Duplicated + " please go thru filling process.");
                                }
                            }
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var purge = new CMS_Purge();
                if (model.operation == Operation.C)
                {
                }
                else if (model.operation == Operation.U || model.operation == Operation.D)
                {
                    var cri = new CMSCriteria();
                    cri.Purge_ID = model.Purge_ID;
                    cri.include = true;
                    var result = cmsService.GetCMSPurge(cri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var purges = result.Object as List<CMS_Purge>;
                        if (purges != null && purges.Count() == 1)
                            purge = purges.FirstOrDefault();
                    }
                }

                if (model.operation != Operation.D)
                {
                    purge.Drum_Code = model.Drum_Code;
                    purge.Initial_Weight = model.Initial_Weight;
                    purge.Final_Weight = model.Final_Weight;
                }

                if (model.operation == Operation.C)
                    model.result = cmsService.InsertCMSPurge(purge);

                else if (model.operation == Operation.U)
                    model.result = cmsService.UpdateCMSPurge(purge);

                else if (model.operation == Operation.D)
                {
                    purge.Record_Status = Record_Status.Delete;
                    model.result = cmsService.UpdateCMSPurge(purge);
                    if (model.result.Code == ReturnCode.SUCCESS)
                        model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.CMS_Purge };
                    else
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.CMS_Purge };

                    return RedirectToAction("CMSPurge", new AppRouteValueDictionary(model));
                }

                if (model.result.Code == ReturnCode.SUCCESS)
                {
                    return RedirectToAction("CMSPurge", new AppRouteValueDictionary(model));
                }
            }
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0012);
            if (prole != null)
                model.Modify = prole.Modify;

            return View(model);
        }

        #endregion

        #region CMS Charge

        [HttpGet]
        public ActionResult CMSCharge(CMSChargeViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0013);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post

            var cmsService = new CMSService();
            var resultremove = cmsService.DeleteSaveNewCMSChargeNotUse(AppSetting.Station_Code);

            var cri = new CMSCriteria();
            cri.Drum_Code = model.Drum_Code;
            cri.Lot_No = model.Lot_No;
            cri.Not_Yet_Deliver = true; //show only not yet deliver 
            cri.Sort_By = Sort_Type.Drum_Code;
            cri.Drum_Code_Have_Value = true;
            if (AppSetting.Is_Station)
            {
                cri.Station_Code_Equals = AppSetting.Station_Code;
                cmsService.DeleteSaveNewCMSChargeNotUse(cri.Station_Code_Equals);
            }
            var result = cmsService.GetCMSCharge(cri);
            if (result.Code == ReturnCode.SUCCESS)
                model.chargelist = result.Object as List<CMS_Charge>;

            return View(model);
        }

        [HttpGet]
        public ActionResult CMSChargeVerify(CMSChargeViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0013);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post

            return View(model);
        }

        [HttpPost]
        public ActionResult CMSChargeVerify(CMSChargeViewModels model)
        {
            var uService = new UserService();
            var cmsService = new CMSService(User.Identity.GetUserId());
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0013);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = new ServiceResult();

            if (string.IsNullOrEmpty(model.Drum_Code))
                ModelState.AddModelError("Drum_Code", Resource.Message_Is_Required);

            if (string.IsNullOrEmpty(model.Lot_No))
                ModelState.AddModelError("Lot_No", Resource.Message_Is_Required);

            if (ModelState.IsValid)
            {
                var isCheck = checkDrumControl(model.Drum_Code);
                if (isCheck != true)
                {
                    model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND) + " please configuration in CMS Setup page.(Drum Control)", Field = Resource.Drum_Code };
                    ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);
                }
                else
                {
                    var isUse = checkDrumUse(model.Drum_Code);
                    if (isUse == true)
                    {
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_IN_USE, Msg = "has been charged but not yet delivered", Field = Resource.Drum_Code };
                        ModelState.AddModelError("Drum_Code", Resource.Message_Is_Duplicated);
                    }

                    var purge = checkDrumFromPurge(model.Drum_Code);
                    if (purge != true)
                    {
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = "has been not purge. please go thru purge process.", Field = Resource.Drum_Code };
                        ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var cms_format = new CMS_Format();
                var cms_formats = new List<CMS_Format>();
                var format_result = cmsService.GetFormat();
                if (format_result.Code == ReturnCode.SUCCESS)
                    cms_formats = format_result.Object as List<CMS_Format>;

                if (cms_formats == null)
                    model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND), Field = Resource.Lot_No + " " + Resource.Formatting };

                var lognolength = model.Lot_No.Length.ToString();
                int length = NumUtil.ParseInteger(lognolength);
                if (length > 0)
                {
                    int _product_length = 0;
                    cms_format = cms_formats.Where(w => w.Lot_No_Length == length).FirstOrDefault();
                    if (cms_format != null)
                    {
                        model.Format_ID = cms_format.Format_ID;
                        _product_length = cms_format.Product_Code_Length.HasValue ? cms_format.Product_Code_Length.Value : 0;
                        if (_product_length > 0)
                        {
                            var _product_length_of_lot_no = model.Drum_Code.Substring(0, _product_length);
                            var _produc_length_of_drun_code = model.Lot_No.Substring(0, _product_length);
                            if (!string.IsNullOrEmpty(_produc_length_of_drun_code) && !string.IsNullOrEmpty(_product_length_of_lot_no))
                            {
                                if (_produc_length_of_drun_code.Contains(_product_length_of_lot_no))
                                {
                                    CMS_Product product = new CMS_Product();
                                    var cri = new ProductCriteria();
                                    cri.Product_Code_Equals = _produc_length_of_drun_code;

                                    if (AppSetting.Is_Station)
                                        cri.Station_Code_Equals = AppSetting.Station_Code;

                                    var result = cmsService.GetProduct(cri);
                                    if (result.Code == ReturnCode.SUCCESS)
                                    {
                                        var products = result.Object as List<CMS_Product>;
                                        if (products != null && products.Count() > 0)
                                        {
                                            product = products.FirstOrDefault();
                                            if (product != null)
                                            {
                                                int max_no_of_charging = 0, no_of_charging = 0;
                                                no_of_charging = getNoOfCharging(model.Drum_Code);
                                                var cri2 = new ChargingControlCriteria();
                                                String prodCode, drumType, action;
                                                if (DrumControl(model.Drum_Code, out prodCode, out drumType))
                                                {
                                                    cri2.Drum_Code = prodCode + drumType;
                                                    if (!string.IsNullOrEmpty(cri2.Drum_Code))
                                                    {
                                                        int maxNoOfChargin;
                                                        var verifypass = getMaxNoOfCharging(cri2, out maxNoOfChargin, out action);
                                                        max_no_of_charging = maxNoOfChargin;
                                                        if (action == Charging_Control_Action.Discard)
                                                        {
                                                            if (no_of_charging > max_no_of_charging)
                                                                ModelState.AddModelError("Message_Error", "Discard!  The drum code max No. of charging is " + max_no_of_charging + " . please go thru charging control.");
                                                        }
                                                    }
                                                }

                                                if (ModelState.IsValid)
                                                {
                                                    var charge = new CMS_Charge();
                                                    charge.Drum_Code = model.Drum_Code;
                                                    charge.Lot_No = model.Lot_No;
                                                    charge.Product_Code = product.Product_Code;
                                                    charge.Filling_Station_ID = getFillingStation(product.Product_Code);
                                                    charge.Quantity_Scanned = getQuantityScannedByLotNo(model.Lot_No);
                                                    charge.No_Of_Charging = no_of_charging;
                                                    charge.Max_No_Of_Charging = max_no_of_charging;
                                                    model.result = cmsService.InsertCMSCharge(charge);
                                                    if (model.result.Code == ReturnCode.SUCCESS)
                                                    {
                                                        model.Charge_ID = charge.Charge_ID;
                                                        model.operation = Operation.U;
                                                        model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS), Field = Resource.CMS_Charge };
                                                    }
                                                    else
                                                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_INSERT, Msg = Error.GetMessage(ReturnCode.ERROR_INSERT), Field = Resource.CMS_Charge };

                                                    model.Page_Action = "New";
                                                    return RedirectToAction("CMSChargeInfo", model);
                                                }
                                            }
                                        }
                                    }
                                    ModelState.AddModelError("Message_Error", "The product code not map in cms product . please check again!");
                                }
                                else
                                    ModelState.AddModelError("Message_Error", "The product code in drum code field not same lot no. field. please check again!");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(_produc_length_of_drun_code))
                                    ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);

                                if (string.IsNullOrEmpty(_product_length_of_lot_no))
                                    ModelState.AddModelError("Lot_No", Resource.Message_Is_Invalid);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Lot_No", Resource.The + " " + Resource.Lot_No + " " + Resource.Field + " " + Resource.Formatting + " is invalid");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CMSChargeInfo(CMSChargeViewModels pmodel)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0013);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            var model = new CMSChargeInfoViewModels();
            model.operation = pmodel.operation;
            model.Charge_ID = pmodel.Charge_ID;
            model.Is_Clone = pmodel.Is_Clone;
            model.Format_ID = pmodel.Format_ID;
            model.Page_Action = pmodel.Page_Action;

            model.Modify = prole.Modify;
            model.View = prole.View;

            var cmsService = new CMSService();
            var cbService = new ComboService();
            if (model.operation == Operation.U)
            {
                model.cStationlist = cbService.LstFillingStationType(includeAll: true);
                model.cProductlist = cbService.LstProductByCode(includeAll: true);
                var cri1 = new CMSCriteria();
                cri1.Charge_ID = model.Charge_ID;
                cri1.include = false;
                var result = cmsService.GetCMSCharge(cri1);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var charges = result.Object as List<CMS_Charge>;
                    if (charges != null && charges.Count() == 1)
                    {
                        var charge = charges.FirstOrDefault();
                        if (charge != null)
                        {
                            model.Charge_ID = charge.Charge_ID;
                            model.Drum_Code = charge.Drum_Code;
                            model.Lot_No = charge.Lot_No;
                            model.Product_Code = charge.Product_Code;
                            model.Product_Code_Dispaly = charge.Product_Code;
                            model.Filling_Station_ID = charge.Filling_Station_ID;
                            model.Filling_Station_ID_Dispaly = charge.Filling_Station_ID;
                            model.Quantity_Scanned = charge.Quantity_Scanned.HasValue ? charge.Quantity_Scanned.Value : 1;
                            model.No_Of_Charging = charge.No_Of_Charging.HasValue ? charge.No_Of_Charging.Value : 0;
                            model.Max_No_Of_Charging = charge.Max_No_Of_Charging.HasValue ? charge.Max_No_Of_Charging.Value : 0;
                            model.Initial_Weight = charge.Initial_Weight;
                            model.Final_Weight = charge.Final_Weight;
                            model.Delivery_Status = charge.Delivery_Status;
                        }
                    }
                }
            }
            else if (model.operation == Operation.D)
                return CMSChargeInfo(model);

            var cri2 = new ChargingControlCriteria();
            String prodCode, drumType, action;
            var retDrum = DrumControl(model.Drum_Code, out prodCode, out drumType);
            cri2.Drum_Code = prodCode + drumType;

            if (!string.IsNullOrEmpty(cri2.Drum_Code))
            {
                int maxNoOfChargin;
                var verifypass = getMaxNoOfCharging(cri2, out maxNoOfChargin, out action);
                model.Max_No_Of_Charging = maxNoOfChargin;
                model.Charging_Cl_Action = action;
            }

            model.IsOverLoad = false;
            if (model.No_Of_Charging > model.Max_No_Of_Charging)
                model.IsOverLoad = true;

            if (model.Page_Action == "New")
                model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_INSERT), Field = Resource.CMS_Charge + " (Draft) " };

            if (string.IsNullOrEmpty(model.Drum_Code))
                model.Is_Clone = true;

            return View(model);
        }

        [HttpPost]
        public ActionResult CMSChargeInfo(CMSChargeInfoViewModels model)
        {
            var cmsService = new CMSService(User.Identity.GetUserId());
            model.result = new ServiceResult();
            if (ModelState.IsValid)
            {
                if (model.Is_Clone == true)
                {
                    var isCheck = checkDrumControl(model.Drum_Code);
                    if (isCheck != true)
                    {
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = Error.GetMessage(ReturnCode.ERROR_DATA_NOT_FOUND) + " please configuration in CMS Setup page.(Drum Control)", Field = Resource.Drum_Code };
                        ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);
                    }
                    else
                    {
                        var isUse = checkDrumUse(model.Drum_Code);
                        if (isUse == true)
                        {
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_IN_USE, Msg = "has been charged but not yet delivered", Field = Resource.Drum_Code };
                            ModelState.AddModelError("Drum_Code", Resource.Message_Is_Duplicated);
                        }
                        else
                        {
                            var checkFormat = checkDrumProductFormat(model.Format_ID, model.Drum_Code, model.Product_Code);
                            if (checkFormat != true)
                            {
                                model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = "in drum code field not same lot no. field. please check again!", Field = Resource.Product_Code };
                                ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);
                            }

                            var purge = checkDrumFromPurge(model.Drum_Code);
                            if (purge != true)
                            {
                                model.result = new ServiceResult() { Code = ReturnCode.ERROR_DATA_NOT_FOUND, Msg = "has been not purge. please go thru purge process.", Field = Resource.Drum_Code };
                                ModelState.AddModelError("Drum_Code", Resource.Message_Is_Invalid);
                            }
                        }
                    }
                    model.No_Of_Charging = getNoOfCharging(model.Drum_Code);
                }
            }

            if (ModelState.IsValid)
            {
                var charge = new CMS_Charge();
                if (model.operation == Operation.U || model.operation == Operation.D)
                {
                    var cri = new CMSCriteria();
                    cri.Charge_ID = model.Charge_ID;
                    cri.include = true;
                    var result = cmsService.GetCMSCharge(cri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var charges = result.Object as List<CMS_Charge>;
                        if (charges != null && charges.Count() == 1)
                            charge = charges.FirstOrDefault();
                    }
                }

                if (model.operation != Operation.D)
                {
                    charge.Drum_Code = model.Drum_Code;
                    charge.Initial_Weight = model.Initial_Weight;
                    charge.Final_Weight = model.Final_Weight;
                    charge.Filling_Station_ID = model.Filling_Station_ID;
                    charge.Lot_No = model.Lot_No;
                    charge.Quantity_Scanned = model.Quantity_Scanned;
                    charge.No_Of_Charging = model.No_Of_Charging.HasValue ? model.No_Of_Charging.Value : 0;
                    charge.Max_No_Of_Charging = model.Max_No_Of_Charging;
                    charge.Product_Code = model.Product_Code;
                }

                if (model.operation == Operation.U)
                    model.result = cmsService.UpdateCMSCharge(charge);

                else if (model.operation == Operation.D)
                {
                    charge.Record_Status = Record_Status.Delete;
                    model.result = cmsService.UpdateCMSCharge(charge);
                    if (model.result.Code == ReturnCode.SUCCESS)
                        model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.CMS_Charge };
                    else
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.CMS_Charge };

                    return RedirectToAction("CMSCharge", new AppRouteValueDictionary(model));
                }

                if (model.result.Code == ReturnCode.SUCCESS)
                {
                    if (model.Save_Mode == "SaveAndNew")
                    {
                        CMSChargeViewModels NewModel = InitCMSChargeClone(charge.Charge_ID);
                        NewModel.Format_ID = model.Format_ID;
                        return RedirectToAction("CMSChargeInfo", NewModel);
                    }
                    else
                        return RedirectToAction("CMSCharge", new AppRouteValueDictionary(model));
                }
            }

            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0013);
            if (prole != null)
                model.Modify = prole.Modify;

            var cbService = new ComboService();
            model.cProductlist = cbService.LstProductByCode();
            model.Product_Code_Dispaly = model.Product_Code;
            model.Filling_Station_ID_Dispaly = model.Filling_Station_ID;
            model.cStationlist = cbService.LstFillingStationType();

            return View(model);
        }

        private CMSChargeViewModels InitCMSChargeClone(int? pChargeID)
        {
            var cmsService = new CMSService();
            var model = new CMSChargeViewModels();
            model.Is_Clone = true;
            model.Page_Action = "New";

            var cri = new CMSCriteria();
            cri.Charge_ID = pChargeID;
            cri.include = false;
            var result = cmsService.GetCMSCharge(cri);
            if (result.Code == ReturnCode.SUCCESS)
            {
                var charges = result.Object as List<CMS_Charge>;
                if (charges != null && charges.Count() == 1)
                {
                    var charge = charges.FirstOrDefault();
                    if (charge != null)
                    {
                        var chargeclone = new CMS_Charge();
                        chargeclone.Lot_No = charge.Lot_No;
                        chargeclone.Drum_Code = "";
                        chargeclone.Product_Code = charge.Product_Code;
                        chargeclone.Filling_Station_ID = charge.Filling_Station_ID;
                        chargeclone.Quantity_Scanned = getQuantityScannedByLotNo(charge.Lot_No);
                        //chargeclone.No_Of_Charging = getNoOfCharging(charge.Drum_Code);
                        model.result = cmsService.InsertCMSCharge(chargeclone);
                        if (model.result.Code == ReturnCode.SUCCESS)
                        {
                            model.Charge_ID = chargeclone.Charge_ID;
                            model.operation = Operation.U;
                            model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS), Field = Resource.CMS_Charge };
                        }
                        else
                            model.result = new ServiceResult() { Code = ReturnCode.ERROR_INSERT, Msg = Error.GetMessage(ReturnCode.ERROR_INSERT), Field = Resource.CMS_Charge };
                    }
                }
            }
            return model;
        }

        public ActionResult CheckDrumCode(String pDrumCode)
        {
            var Max_No_Of_Charging = 0;
            var Charging_Cl_Action = String.Empty;
            var Verifypass = true; //change to true as user doesn't want to check this value 
            var No_Of_Charging = 0;
            var Msg = String.Empty;
            var Verify_Purge = false;

            if (!string.IsNullOrEmpty(pDrumCode))
            {
                var cri2 = new ChargingControlCriteria();
                String prodCode, drumType, action;
                var retDrum = DrumControl(pDrumCode, out prodCode, out drumType);
                cri2.Drum_Code = prodCode + drumType;

                int maxNoOfChargin;
                Verifypass = getMaxNoOfCharging(cri2, out maxNoOfChargin, out action);
                Max_No_Of_Charging = maxNoOfChargin;
                Charging_Cl_Action = action;

                No_Of_Charging = getNoOfCharging(pDrumCode);
                Verify_Purge = checkDrumFromPurge(pDrumCode);
            }
            //if (!Verifypass)
            //{
            //   Msg = "Max. No. Of Charging has not found. please configuration in CMS Setup page.";
            //}

            return Json(new
            {
                Max_No_Of_Charging = Max_No_Of_Charging,
                Charging_Cl_Action = Charging_Cl_Action,
                No_Of_Charging = No_Of_Charging,
                Verifypass = Verifypass,
                Msg = Msg,
                Verify_Purge = Verify_Purge
            }, JsonRequestBehavior.AllowGet);
        }

        private int getNoOfCharging(String pDrumCode)
        {
            int No_Of_Charging = 0;
            if (!string.IsNullOrEmpty(pDrumCode))
            {
                String prodCode, drumType;
                var retDrum = DrumControl(pDrumCode, out prodCode, out drumType);
                var drumCode = prodCode + drumType;

                var cmsService = new CMSService();
                var criCharge = new CMSCriteria();
                criCharge.Product_Code = prodCode;
                criCharge.Drum_Code_Equals = pDrumCode;
                //criCharge.Not_Yet_Deliver = true;
                criCharge.Drum_Code_Have_Value = true;
                //if (AppSetting.Is_Station)
                //   criCharge.Station_Code_Equals = AppSetting.Station_Code;

                var resultCharge = cmsService.GetCMSCharge(criCharge);
                if (resultCharge.Code == ReturnCode.SUCCESS)
                {
                    if (drumCode != null && drumCode.Length > 1)
                    {
                        var ChargingControls = resultCharge.Object as List<CMS_Charge>;
                        if (ChargingControls != null && ChargingControls.Count() > 0)
                        {
                            if (ChargingControls != null)
                                No_Of_Charging = ChargingControls.Where(w => w.Drum_Code == pDrumCode).Count();
                        }
                    }
                }
            }
            No_Of_Charging = No_Of_Charging + 1;
            return No_Of_Charging;
        }

        private int getFillingStation(string pProduuctCode)
        {
            int FillingStationID = 0;
            var cmsService = new CMSService();
            var criPro = new ProductCriteria();
            criPro.Product_Code_Equals = pProduuctCode;
            criPro.Station_Code_Equals = AppSetting.Station_Code;
            var resultPro = cmsService.GetProduct(criPro);
            if (resultPro.Code == ReturnCode.SUCCESS)
            {
                var products = resultPro.Object as List<CMS_Product>;
                if (products != null && products.Count() == 1)
                {
                    CMS_Product product = products.FirstOrDefault();
                    if (product != null)
                        FillingStationID = product.Filling_Station_ID.HasValue ? product.Filling_Station_ID.Value : 0;
                }
            }
            return FillingStationID;
        }

        private bool checkDrumControl(String pDrumCode)
        {
            if (!string.IsNullOrEmpty(pDrumCode))
            {
                pDrumCode = pDrumCode.Trim();
                var drum_length = pDrumCode.Length;
                int _drum_Type_Length = NumUtil.ParseInteger(AppSetting.Drum_Type_Length);
                for (int i = 5; i < 7; i++)
                {
                    var _product_code = String.Empty;
                    var _drum_type = String.Empty;
                    var _running_no = 0;
                    if (drum_length > i)
                    {
                        _product_code = pDrumCode.Substring(0, i);
                        if (drum_length >= (i + _drum_Type_Length))
                        {
                            _drum_type = pDrumCode.Substring(i, _drum_Type_Length);
                            _running_no = drum_length - (i + _drum_Type_Length);

                            List<CMS_Drum_Control> Drum_Controls = new List<CMS_Drum_Control>();
                            var cmsService = new CMSService();
                            var criCharge = new DrumControlCriteria();
                            criCharge.Product_Code_Equals = _product_code;
                            criCharge.Drum_Type_Equals = _drum_type;
                            //criCharge.Running_Number = _running_no;
                            var drumControlresult = cmsService.GetDrumControl(criCharge);
                            if (drumControlresult.Code == ReturnCode.SUCCESS)
                            {
                                Drum_Controls = drumControlresult.Object as List<CMS_Drum_Control>;
                                if (Drum_Controls != null && Drum_Controls.Count() > 0)
                                {
                                    var Drum_Control = Drum_Controls.FirstOrDefault();
                                    if (Drum_Control != null)
                                        return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool DrumControl(String pDrumCode, out String prodCode, out String drumType)
        {
            prodCode = string.Empty;
            drumType = string.Empty;
            if (!string.IsNullOrEmpty(pDrumCode))
            {
                pDrumCode = pDrumCode.Trim();
                var drum_length = pDrumCode.Length;
                int _drum_Type_Length = NumUtil.ParseInteger(AppSetting.Drum_Type_Length);
                for (int i = 5; i < 7; i++)
                {
                    var _product_code = String.Empty;
                    var _drum_type = String.Empty;

                    var _running_no = 0;
                    if (drum_length > i)
                    {
                        _product_code = pDrumCode.Substring(0, i);
                        if (drum_length >= (i + _drum_Type_Length))
                        {
                            _drum_type = pDrumCode.Substring(i, _drum_Type_Length);
                            _running_no = drum_length - (i + _drum_Type_Length);

                            //List<CMS_Drum_Control> Drum_Controls = new List<CMS_Drum_Control>();
                            var cmsService = new CMSService();
                            var criCharge = new DrumControlCriteria();
                            criCharge.Product_Code_Equals = _product_code;
                            criCharge.Drum_Type_Equals = _drum_type;
                            //criCharge.Running_Number = _running_no;
                            var drumControlresult = cmsService.GetDrumControl(criCharge);
                            if (drumControlresult.Code == ReturnCode.SUCCESS)
                            {
                                var Drum_Controls = drumControlresult.Object as List<CMS_Drum_Control>;
                                if (Drum_Controls != null && Drum_Controls.Count() > 0)
                                {
                                    var Drum_Control = Drum_Controls.FirstOrDefault();
                                    if (Drum_Control != null)
                                        prodCode = _product_code;
                                    drumType = _drum_type;
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool checkDrumUse(String pDrumCode)
        {
            if (!string.IsNullOrEmpty(pDrumCode))
            {
                var cmsService = new CMSService();
                var cri = new CMSCriteria();
                cri.Drum_Code = pDrumCode;
                cri.Not_Yet_Deliver = true;
                if (AppSetting.Is_Station)
                    cri.Station_Code_Equals = AppSetting.Station_Code;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var chargelist = result.Object as List<CMS_Charge>;
                    if (chargelist != null && chargelist.Count() > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool checkDrumInCharge(String pDrumCode)
        {
            if (!string.IsNullOrEmpty(pDrumCode))
            {
                var cmsService = new CMSService();
                var cri = new CMSCriteria();
                cri.Drum_Code = pDrumCode;
                //cri.Not_Yet_Deliver = false;
                if (AppSetting.Is_Station)
                    cri.Station_Code_Equals = AppSetting.Station_Code;
                var result = cmsService.GetCMSCharge(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var chargelist = result.Object as List<CMS_Charge>;
                    if (chargelist != null && chargelist.Count() > 0)
                    {
                        if (chargelist.Where(w => w.Delivery_Status != Delivery_Status.Completed).Count() == 0)
                            return true;
                        else
                            return false;
                    }
                    else
                        return true;
                }
            }
            return false;
        }

        private bool checkDrumProductFormat(int pFormatID, String pDrumCode, String pProductCode)
        {
            if (pFormatID == 0)
                pFormatID = getFormat(pDrumCode, pProductCode);

            if (!string.IsNullOrEmpty(pDrumCode) && pFormatID > 0 && !string.IsNullOrEmpty(pProductCode))
            {
                var cmsService = new CMSService();
                var cri = new FormatCriteria();
                cri.Format_ID = pFormatID;
                var result = cmsService.GetFormat(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var cms_formats = result.Object as List<CMS_Format>;
                    if (cms_formats != null && cms_formats.Count() == 1)
                    {
                        var cms_format = cms_formats.FirstOrDefault();
                        if (cms_format != null)
                        {
                            int plength = cms_format.Product_Code_Length.HasValue ? cms_format.Product_Code_Length.Value : 0;
                            int dlength = pDrumCode.Length;
                            if (dlength > plength)
                            {
                                var _product_length_of_drum_code = pDrumCode.Substring(0, plength);
                                if (!string.IsNullOrEmpty(_product_length_of_drum_code) && !string.IsNullOrEmpty(pProductCode))
                                {
                                    if (_product_length_of_drum_code.Contains(pProductCode))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool checkDrumFromPurge(String pDrumCode)
        {
            //ก่อน charge  check ว่า drum นั้นโดน purge ก่อน
            if (!string.IsNullOrEmpty(pDrumCode))
            {
                var verify = false;
                var cmsService = new CMSService();
                var drum_length = pDrumCode.Length;
                int _drum_Type_Length = NumUtil.ParseInteger(AppSetting.Drum_Type_Length);
                for (int i = 5; i < 7; i++)
                {
                    var _product_code = String.Empty;
                    var _drum_type = String.Empty;
                    if (drum_length > i)
                    {
                        _product_code = pDrumCode.Substring(0, i);
                        if (drum_length > (i + _drum_Type_Length))
                        {
                            _drum_type = pDrumCode.Substring(i, _drum_Type_Length);
                            List<CMS_Skip_Purging> CMS_Skips = new List<CMS_Skip_Purging>();
                            var criCharge = new SkipPurgCriteria();
                            criCharge.Product_Code = _product_code;
                            criCharge.Drum_Type = _drum_type;
                            var drumControlresult = cmsService.GetSkipPurg(criCharge);
                            if (drumControlresult.Code == ReturnCode.SUCCESS)
                            {
                                CMS_Skips = drumControlresult.Object as List<CMS_Skip_Purging>;
                                if (CMS_Skips != null && CMS_Skips.Count() > 0)
                                {
                                    var CMS_Skip = CMS_Skips.FirstOrDefault();
                                    if (CMS_Skip != null)
                                    {
                                        verify = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (!verify)
                {
                    var cri = new CMSCriteria();
                    cri.Drum_Code = pDrumCode;
                    //filter drum delivered out
                    //cri.Not_Yet_Deliver = true;
                    var result = cmsService.GetCMSPurge(cri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var purges = result.Object as List<CMS_Purge>;
                        if (purges != null && purges.Count() > 0)
                        {
                            if (purges.Where(w => w.Delivery_Status != Delivery_Status.Completed).Count() > 0)
                                return true;
                            else
                                return false;
                        }
                        else
                            return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        public ActionResult Reload_Station(String pProductCode)
        {
            var cbService = new ComboService();
            List<ComboRow> combolist = new List<ComboRow>();
            if (!String.IsNullOrEmpty(pProductCode))
            {
                combolist = cbService.LstStationByProductCode(pProductCode);
            }
            return Json(combolist, JsonRequestBehavior.AllowGet);
        }

        private int getQuantityScannedByLotNo(String pLotNo)
        {
            int Quantity_Scanned = 0;
            if (!string.IsNullOrEmpty(pLotNo))
            {
                var cmsService = new CMSService();
                var cri = new CMSCriteria();
                cri.Lot_No_Equals = pLotNo;
                cri.Drum_Code_Have_Value = true;
                var resultCharge = cmsService.GetCMSCharge(cri);
                if (resultCharge.Code == ReturnCode.SUCCESS)
                {
                    var ChargingControls = resultCharge.Object as List<CMS_Charge>;
                    if (ChargingControls != null && ChargingControls.Count() > 0)
                        Quantity_Scanned = ChargingControls.Count();
                }
            }
            Quantity_Scanned = Quantity_Scanned + 1;
            return Quantity_Scanned;
        }

        private bool getMaxNoOfCharging(ChargingControlCriteria pcri, out int pMaxNoOfCharging, out String pAction)
        {
            pMaxNoOfCharging = 1;
            pAction = string.Empty;
            var cmsService = new CMSService();
            var charging_result = cmsService.GetChargingControl(pcri);
            if (charging_result.Code == ReturnCode.SUCCESS)
            {
                var cms_chargings = charging_result.Object as List<CMS_Charging_Control>;
                if (cms_chargings != null)
                {
                    var cms_charging = cms_chargings.FirstOrDefault();
                    if (cms_charging != null)
                    {
                        pMaxNoOfCharging = cms_charging.Max_Of_Change.HasValue ? cms_charging.Max_Of_Change.Value : 0;
                        pAction = cms_charging.Action;
                        return true;
                    }
                }
            }
            return false;
        }

        private int getFormat(String pLotNo, String pProductCode)
        {
            int format_ID = 0;
            if (!string.IsNullOrEmpty(pLotNo) && !string.IsNullOrEmpty(pProductCode))
            {
                var cmsService = new CMSService();
                var cri = new FormatCriteria();
                cri.Lot_No_Length = pLotNo.Length;
                cri.Product_Code_Length = pProductCode.Length;
                var result = cmsService.GetFormat(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var formats = result.Object as List<CMS_Format>;
                    if (formats != null && formats.Count() > 0)
                    {
                        var format = formats.FirstOrDefault();
                        format_ID = format.Format_ID;
                    }
                }
            }
            return format_ID;
        }

        [HttpGet]
        public ActionResult ExcelPrint(CMSChargeViewModels model)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0013);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;


            var cmsService = new CMSService();
            var resultremove = cmsService.DeleteSaveNewCMSChargeNotUse(AppSetting.Station_Code);

            var cri = new CMSCriteria();
            cri.Drum_Code = model.Drum_Code;
            cri.Lot_No = model.Lot_No;
            cri.Not_Yet_Deliver = true; //show only not yet deliver 
            cri.Sort_By = Sort_Type.Drum_Code;
            cri.Drum_Code_Have_Value = false;
            if (AppSetting.Is_Station)
            {
                cri.Station_Code_Equals = AppSetting.Station_Code;
                cmsService.DeleteSaveNewCMSChargeNotUse(cri.Station_Code_Equals);
            }
            var result = cmsService.GetCMSCharge(cri);
            if (result.Code == ReturnCode.SUCCESS)
                model.chargelist = result.Object as List<CMS_Charge>;


            var currentdate = StoredProcedure.GetCurrentDate();
            var csv = RenderPartialViewAsString("CMSChargeExcel", model);
            System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename= ChargeTransaction" + currentdate.ToString("ddMMyyyy") + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />");
            StringWriter sw = new StringWriter();
            sw.Write(csv);
            System.Web.UI.HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View(model);
        }

        //[HttpGet]
        //public ActionResult CMSChargePrint(CMSChargeViewModels model)
        //{
        //   var uService = new UserService();
        //   var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0013);
        //   if (prole == null)
        //      return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
        //   if (prole.View == null || prole.View == false)
        //      return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

        //   var cmsService = new CMSService();
        //   var cri = new CMSCriteria();
        //   cri.Drum_Code = model.Drum_Code;
        //   cri.Lot_No = model.Lot_No;
        //   var result = cmsService.GetCMSCharge(cri);
        //   if (result.Code == ReturnCode.SUCCESS)
        //      model.chargelist = result.Object as List<CMS_Charge>;

        //   var htmlToConvert = RenderPartialViewAsString("CMSChargePrint", model);
        //   Response.Clear();
        //   Response.ClearContent();
        //   Response.ClearHeaders();
        //   Response.ContentType = "application/pdf";
        //   Response.Charset = Encoding.UTF8.ToString();
        //   Response.HeaderEncoding = Encoding.UTF8;
        //   Response.ContentEncoding = Encoding.UTF8;
        //   Response.Buffer = true;
        //   StringReader sr = new StringReader(htmlToConvert);
        //   Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 40);
        //   pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());


        //   HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //   var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //   var action = new PdfAction(PdfAction.PRINTDIALOG);
        //   writer.SetOpenAction(action);

        //   var pageevent = new PDFPageEvent();
        //   writer.PageEvent = pageevent;
        //   pdfDoc.Open();
        //   htmlparser.Parse(sr);
        //   pdfDoc.Close();
        //   Response.End();
        //   return View(model);
        //}
        #endregion

        #region CMS Delivery

        [HttpGet]
        public ActionResult CMSDelivery(CMSDeliveryViewModels model, ServiceResult msgresult)
        {
            var uService = new UserService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0015);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult; // return result from http post
            var cmsService = new CMSService();

            var cri = new DeliveryCriteria();
            cri.Delivery_Order_No = model.Delivery_Order_No;
            //cri.Not_Completed = true;

            var result = cmsService.GetCMSDelivery(cri);
            if (result.Code == ReturnCode.SUCCESS)
                model.Deliverylist = result.Object as List<CMS_Delivery>;

            return View(model);
        }

        [HttpGet]
        public ActionResult CMSDeliveryInfo(CMSDeliveryViewModels pmodel, ServiceResult msgresult)
        {
            var uService = new UserService();
            var cmsService = new CMSService();
            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0015);
            if (prole == null)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });
            if (prole.View == null || prole.View == false)
                return RedirectToAction("ErrorPage", "Account", new ErrorViewModel() { Message = Resource.Message_Access_Denied });

            ModelState.Clear();
            var model = new CMSDeliveryInfoViewModels();
            model.operation = pmodel.operation;
            model.Delivery_ID = pmodel.Delivery_ID;
            model.Modify = prole.Modify;
            model.View = prole.View;
            model.result = msgresult;

            var cbService = new ComboService();
            model.cProduct_Codelist = cbService.LstProductByCode();
            var deltal = new List<CMSDeliveryDetail>();
            if (model.operation == Operation.C)
            {
            }
            else if (model.operation == Operation.U)
            {
                var cri = new DeliveryCriteria();
                cri.Delivery_ID = model.Delivery_ID;
                cri.include = false;
                var result = cmsService.GetCMSDelivery(cri);
                if (result.Code == ReturnCode.SUCCESS)
                {
                    var deliverys = result.Object as List<CMS_Delivery>;
                    if (deliverys != null && deliverys.Count() == 1)
                    {
                        var delivery = deliverys.FirstOrDefault();
                        model.Delivery_Order_No = delivery.Delivery_Order_No;
                        model.Completed = delivery.Completed.HasValue ? delivery.Completed.Value : false;

                        if (delivery.Completed.HasValue && delivery.Completed.Value)
                            model.cProduct_Codelist = cbService.LstProductByCode(includeAll: true);

                        foreach (var row in delivery.CMS_Delivery_Detail)
                        {
                            var lrow = new CMSDeliveryDetail();
                            lrow.CMS_Delivery_Detail_ID = row.CMS_Delivery_Detail_ID;
                            lrow.Product_Code = row.Product_Code;
                            lrow.No_Of_Containers = row.No_Of_Containers.HasValue ? row.No_Of_Containers.Value : 1;
                            lrow.Date_Delivered = DateUtil.ToDisplayDate(row.Date_Delivered);
                            if (row.Drum_Code != null)
                                lrow.Drum_Code = row.Drum_Code.Split(new char[] { ',', ',' });
                            lrow.Row_Type = RAction.Edit;
                            //if (!string.IsNullOrEmpty(row.Product_Code))
                            //    lrow.cDrum_Codelist = cbService.LstDrumCode(row.Product_Code);
                            deltal.Add(lrow);
                        }
                        model.Product_Rows = deltal.ToArray();
                    }
                }
            }
            else if (model.operation == Operation.D)
                return CMSDeliveryInfo(model);


            return View(model);
        }

        [HttpPost]
        public ActionResult CMSDeliveryInfo(CMSDeliveryInfoViewModels model)
        {
            var cmsService = new CMSService(User.Identity.GetUserId());
            if (model.operation != Operation.D)
            {
                if (model.Product_Rows == null)
                    ModelState.AddModelError("Product_Rows", Resource.The + " " + Resource.Product + " " + Resource.Is_Rrequired);
                else
                {
                    if (model.Product_Rows.Count() > 0)
                    {
                        if (model.Product_Rows.Where(w => w.Row_Type != RAction.Delete).Count() == 0)
                            ModelState.AddModelError("Product_Rows", Resource.The + " " + Resource.Product + " " + Resource.Is_Rrequired);

                        var i = 0;
                        foreach (var row in model.Product_Rows)
                        {
                            if (row.Drum_Code != null && row.Row_Type != RAction.Delete)
                            {
                                row.Drum_Code = row.Drum_Code.Where(item => item != string.Empty).ToArray();
                                if (row.Drum_Code != null && row.No_Of_Containers < row.Drum_Code.Length)
                                    ModelState.AddModelError(NameUtil.GenCMSProduct.GenMapName(i, "Drum_Code"), Resource.The + " " + Resource.Drum_Code + " can't more than " + Resource.No_Of_Containers);
                            }
                            i++;
                        }
                    }
                    else
                        ModelState.AddModelError("Product_Rows", Resource.The + " " + Resource.Product + " " + Resource.Is_Rrequired);
                }

                if (ModelState.IsValid)
                {
                    var cri = new DeliveryCriteria();
                    cri.Delivery_Order_No_Equals = model.Delivery_Order_No;
                    var result = cmsService.GetCMSDelivery(cri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var checkdups = result.Object as List<CMS_Delivery>;
                        if (checkdups != null && checkdups.Count() > 0)
                        {
                            var dup = checkdups.FirstOrDefault();
                            if (dup != null)
                            {
                                if (model.operation == Operation.U)
                                {
                                    if (dup.Delivery_ID != model.Delivery_ID)
                                        ModelState.AddModelError("Delivery_Order_No", Resource.Message_Is_Duplicated);
                                }
                                else
                                    ModelState.AddModelError("Delivery_Order_No", Resource.Message_Is_Duplicated);
                            }
                        }
                    }
                }
            }

            if (ModelState.IsValid)
            {
                var delivery = new CMS_Delivery();
                if (model.operation == Operation.C)
                {

                }
                else if (model.operation == Operation.U || model.operation == Operation.D)
                {
                    var cri = new DeliveryCriteria();
                    cri.Delivery_ID = model.Delivery_ID;
                    cri.include = true;
                    var result = cmsService.GetCMSDelivery(cri);
                    if (result.Code == ReturnCode.SUCCESS)
                    {
                        var deliverys = result.Object as List<CMS_Delivery>;
                        if (deliverys != null && deliverys.Count() == 1)
                            delivery = deliverys.FirstOrDefault();
                    }
                }

                if (model.operation != Operation.D)
                {
                    delivery.Delivery_Order_No = model.Delivery_Order_No;
                    delivery.Record_Status = Record_Status.Active;
                    delivery.CMS_Delivery_Detail.Clear();
                    if (model.Product_Rows != null && model.Product_Rows.Count() > 0)
                    {
                        var currentdate = StoredProcedure.GetCurrentDate();
                        foreach (var row in model.Product_Rows)
                        {
                            if (row.Row_Type == RAction.Add || row.Row_Type == RAction.Edit)
                            {
                                var p = new CMS_Delivery_Detail();
                                if (row.Row_Type == RAction.Edit)
                                    p.CMS_Delivery_Detail_ID = row.CMS_Delivery_Detail_ID.HasValue ? row.CMS_Delivery_Detail_ID.Value : 0;

                                p.Delivery_ID = delivery.Delivery_ID;
                                p.Product_Code = row.Product_Code;
                                p.No_Of_Containers = row.No_Of_Containers;

                                if (row.Drum_Code != null && row.Drum_Code.Length > 0)
                                    p.Drum_Code = string.Join(",", row.Drum_Code).Replace(" ", "");

                                if (model.Save_Mode == "ManualScan")
                                    p.Date_Delivered = currentdate;
                                else
                                    p.Date_Delivered = DateUtil.ToDate(row.Date_Delivered);

                                delivery.CMS_Delivery_Detail.Add(p);
                            }
                        }
                    }
                }

                if (model.Save_Mode == "ManualScan")
                    delivery.Completed = true;

                if (model.operation == Operation.C)
                    model.result = cmsService.InsertCMSDelivery(delivery);

                else if (model.operation == Operation.U)
                    model.result = cmsService.UpdateCMSDelivery(delivery);
                else if (model.operation == Operation.D)
                {
                    delivery.Record_Status = Record_Status.Delete;
                    model.result = cmsService.UpdateCMSDelivery(delivery);
                    if (model.result.Code == ReturnCode.SUCCESS)
                        model.result = new ServiceResult() { Code = ReturnCode.SUCCESS, Msg = Success.GetMessage(ReturnCode.SUCCESS_DELETE), Field = Resource.CMS_Delivery };
                    else
                        model.result = new ServiceResult() { Code = ReturnCode.ERROR_DELETE, Msg = Error.GetMessage(ReturnCode.ERROR_DELETE), Field = Resource.CMS_Delivery };

                    return RedirectToAction("CMSDelivery", new AppRouteValueDictionary(model));
                }

                if (model.result.Code == ReturnCode.SUCCESS)
                {
                    if (model.Save_Mode == "ManualScan")
                    {
                        if (model.operation == Operation.C | model.operation == Operation.U)
                        {
                            var cri = new DeliveryCriteria();
                            cri.Delivery_ID = (Nullable<int>)model.result.Object;
                            cri.include = true;
                            var result = cmsService.GetCMSDelivery(cri);
                            if (result.Code == ReturnCode.SUCCESS)
                            {
                                var _deliverys = result.Object as List<CMS_Delivery>;
                                if (_deliverys != null && _deliverys.Count() == 1)
                                {
                                    var _delivery = _deliverys.FirstOrDefault();
                                    if (_delivery != null)
                                    {
                                        var result2 = new MobileService().UpdateCMSDelivery(_deliverys);
                                        if (result2.Code != ReturnCode.SUCCESS)
                                        {
                                            model.result = new ServiceResult()
                                            {
                                                Code = ReturnCode.ERROR_UPDATE,
                                                Msg = Error.GetMessage(ReturnCode.ERROR_UPDATE) + " (Work Flow Error)",
                                                Field = Resource.CMS_Delivery,
                                            };
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return RedirectToAction("CMSDelivery", new AppRouteValueDictionary(model));
                }
            }
            var uService = new UserService();
            var cbService = new ComboService();

            var prole = uService.ValidatePageRole(User.Identity.GetUserId(), Page_Code.P0015);
            if (prole != null)
                model.Modify = prole.Modify;

            model.cProduct_Codelist = cbService.LstProductByCode();

            return View(model);
        }

        public ActionResult AddProduct(int pIndex)
        {
            var cmsService = new CMSService();
            var cbService = new ComboService();
            var model = new CMSDeliveryDetail()
            {
                Index = pIndex,
                Row_Type = RAction.Add,
                No_Of_Containers = 1,
            };
            model.cProduct_Codelist = cbService.LstProductByCode();
            if (model.cProduct_Codelist != null && model.cProduct_Codelist.Count > 0)
                model.cDrum_Codelist = cbService.LstDrumCode(model.cProduct_Codelist[0].Text, false, true);

            return PartialView("_ProductRow", model);
        }

        public ActionResult ReloadDrumCode(String pProductCode)
        {
            var cbService = new ComboService();
            List<ComboRow> combolist = new List<ComboRow>();
            if (!String.IsNullOrEmpty(pProductCode))
                combolist = cbService.LstDrumCode(pProductCode, false, true);

            return Json(combolist, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region com port test

        //[HttpGet]
        //public ActionResult GetWeight()
        //{
        //   var weight = "0";
        //   var errmsg = "";
        //   try
        //   {
        //      var blnFound = false;
        //      var strComPort = "COMPORT1";
        //      var oComPort = new cmsComPort.WeightScale();

        //      var canread = false;
        //      oComPort.writeCommPort(strComPort);
        //      if (oComPort.OpenCom(1))
        //      {
        //         oComPort.ClearCom();
        //         while (!blnFound)
        //         {
        //            var blnCheck = oComPort.ReadCom();
        //            if (oComPort.VData.Length > 0)
        //            {
        //               if (oComPort.VData[0] == '0')
        //               {
        //                  //	strData = Trim(Right(oComPort.VData, Len(oComPort.VData) - 1))
        //                  var strData = oComPort.VData.Trim();
        //                  if (!string.IsNullOrEmpty(strData))
        //                  {
        //                     var sp = strData.Split(' ');
        //                     if (sp.Length > 0)
        //                     {
        //                        blnFound = true;
        //                        weight = sp[sp.Length - 1];
        //                        canread = true;
        //                     }

        //                  }
        //               }
        //            }
        //            if (!blnCheck)
        //               blnFound = true;
        //         }

        //         if (!canread)
        //         {
        //            errmsg = "The COM cable is attach properly to the communication port or terminal port.";
        //         }
        //      }
        //      else
        //      {
        //         errmsg = "Cannot open comport.";
        //      }
        //   }
        //   catch (Exception ex)
        //   {
        //      errmsg = ex.Message;
        //   }

        //   return Json(new { weight = weight, errmsg = errmsg }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult GetWeight2()
        //{
        //   var strWeight = string.Empty;
        //   var errmsg = "";
        //   try
        //   {
        //      ComPort objComport = new ComPort();
        //      objComport.Clear();
        //      Debug.WriteLine("'***********APP DEBUG***********' " + DateTime.Now + "ActiveXperts Serial Port Component ");
        //      Debug.WriteLine("'***********APP DEBUG***********' " + "Version : " + objComport.Version);
        //      Debug.WriteLine("'***********APP DEBUG***********' " + "Module : " + objComport.Module);
        //      Debug.WriteLine("'***********APP DEBUG***********' " + "LicenseStatus : " + objComport.LicenseStatus);
        //      Debug.WriteLine("'***********APP DEBUG***********' " + "LicenseKey : " + objComport.LicenseKey);

        //      //if (!Directory.Exists(@"c:\agnos1\agnos_log_out"))
        //      //   Directory.CreateDirectory(@"c:\agnos1\agnos_log_out");

        //      //var path = @"c:\agnos1\agnos_log_out\";
        //      //objComport.LogFile = path + "SerialPort" + DateTime.Now + ".log";

        //      var canread = false;

        //      objComport.LogFile = Path.GetTempPath() + "SerialPort.log";
        //      Debug.WriteLine("'***********APP DEBUG***********' " + Path.GetTempPath() + "SerialPort.log");

        //      objComport.Device = AppSetting.ComPort_Name; ;

        //      // If a direct port is used, query for the baudrate
        //      if (objComport.Device.Substring(0, 3) == "COM")
        //         objComport.BaudRate = 9600;

        //      objComport.Open();
        //      Debug.WriteLine("'***********APP DEBUG***********' " + objComport.LastError + " (" + objComport.GetErrorDescription(objComport.LastError) + ")");
        //      if (objComport.LastError == 0)
        //      {
        //         // Set all Read functions (e.g. ReadString) to timeout after a specified number of millisconds
        //         objComport.ComTimeout = 200;
        //         var i = 0;

        //         strWeight = "notempty";
        //         while (strWeight != "")
        //         {
        //            strWeight = objComport.ReadString();

        //            //if (i != 2)
        //            //   strWeight = "F   1000.0";
        //            //else
        //            //   strWeight = "0    998.0";

        //            if (strWeight != "")
        //            {
        //               if (strWeight.Length > 0)
        //               {
        //                  if (strWeight[0] == '0')
        //                  {
        //                     var sp = strWeight.Split(' ');
        //                     if (sp.Length > 0)
        //                     {
        //                        strWeight = sp[sp.Length - 1];
        //                        Debug.WriteLine("'***********APP DEBUG***********' " + "  Weight - " + strWeight);
        //                        canread = true;
        //                        break;
        //                     }
        //                  }
        //               }
        //            }
        //            Debug.WriteLine("'***********APP DEBUG***********' " + "Weight (" + ++i + "):" + strWeight);
        //         }

        //         if (!canread)
        //            errmsg = "The COM cable is attach properly to the communication port or terminal port.";

        //         objComport.Close();
        //         Debug.WriteLine("'***********APP DEBUG***********' " + objComport.LastError + " (" + objComport.GetErrorDescription(objComport.LastError) + ")");
        //      }
        //      else
        //      {
        //         errmsg = objComport.LastError + " (" + objComport.GetErrorDescription(objComport.LastError) + ")";
        //         objComport.Close();
        //      }
        //      Debug.WriteLine("'***********APP DEBUG***********' " + "Close");
        //   }
        //   catch (Exception ex)
        //   {
        //      errmsg = ex.Message;
        //   }
        //   if (string.IsNullOrEmpty(strWeight))
        //      strWeight = "0";

        //   return Json(new { weight = strWeight, errmsg = errmsg }, JsonRequestBehavior.AllowGet);
        //}

        //public String ReadResponse(AxSerial.ComPort objComport)
        //{
        //   string str;
        //   string strweight = string.Empty;
        //   str = "notempty";
        //   while (str != "")
        //   {
        //      str = objComport.ReadString();
        //      if (str != "")
        //      {
        //         var sp = str.Split(' ');
        //         if (sp.Length > 0)
        //         {
        //            strweight = sp[sp.Length - 1];
        //            Debug.WriteLine("'***********APP DEBUG***********' " + "  <- " + strweight);
        //         }
        //      }
        //   }
        //   return strweight;
        //}

        #endregion
    }
}