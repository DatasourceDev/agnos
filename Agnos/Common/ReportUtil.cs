using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Web.Mvc;
using SBSResourceAPI;
using AgnosModel.Models;
using AppFramework.Util;
using Agnos.Common;
using iTextSharp.text.html;

namespace Agnos.Common
{
   public enum PdfPCellBorderType : int
   {
      FullBorder = 1,
      TopLeft,
      TopCenter,
      TopRight,
      MidLeft,
      MidCenter,
      MidRight,
      ButtomLeft,
      ButtomCenter,
      ButtomRight,
      None
   }

   public class CustomITextSharp
   {
      private static void SetCellBorderWidth(PdfPCell c1, PdfPCellBorderType bordertype)
      {
         c1.BorderWidth = 1;
         //if (bordertype == PdfPCellBorderType.TopLeft)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthTop = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.TopCenter)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthTop = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.TopRight)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthRight = 1;
         //   c1.BorderWidthTop = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.MidLeft)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthTop = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.MidCenter)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthTop = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.MidRight)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthRight = 1;
         //   c1.BorderWidthTop = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.ButtomLeft)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthTop = 1;
         //   c1.BorderWidthBottom = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.ButtomCenter)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthTop = 1;
         //   c1.BorderWidthBottom = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.ButtomRight)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthRight = 1;
         //   c1.BorderWidthTop = 1;
         //   c1.BorderWidthBottom = 1;
         //}
         //else if (bordertype == PdfPCellBorderType.FullBorder)
         //{
         //   c1.BorderWidthLeft = 1;
         //   c1.BorderWidthRight = 1;
         //   c1.BorderWidthTop = 1;
         //   c1.BorderWidthBottom = 1;
         //}
      }

      public static void CellImage(PdfPTable table, Image img, Font font, PdfPCellBorderType bordertype, int rolspan = 0, int colspan = 0, BaseColor bgcolor = null)
      {
         PdfPCell c1 = new PdfPCell();
         c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
         c1.AddElement(img);
         c1.Padding = 5f;
         SetCellBorderWidth(c1, bordertype);
     
         if (rolspan > 0) c1.Rowspan = rolspan;
         if (colspan > 0) c1.Colspan = colspan;
       
         table.AddCell(c1);
      }

      public static void Cell(PdfPTable table, string txt, Font font, PdfPCellBorderType bordertype, int rolspan = 0, int colspan = 0, BaseColor bgcolor = null, bool settextdefault = true)
      {

         if (string.IsNullOrEmpty(txt) && settextdefault)
            txt = "-";

         PdfPCell c1 = new PdfPCell(new Phrase(txt, font));
         c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
         SetCellBorderWidth(c1, bordertype);
        
         c1.Padding = 5f;
         if (rolspan > 0) c1.Rowspan = rolspan;
         if (colspan > 0) c1.Colspan = colspan;

         if (bgcolor != null)
            c1.BackgroundColor =bgcolor;
         table.AddCell(c1);
      }
   }


}

public class PDFPageEvent : iTextSharp.text.pdf.PdfPageEventHelper
{
   // This is the contentbyte object of the writer
   PdfContentByte cb;

   // we will put the final number of pages in a template
   PdfTemplate template;

   // this is the BaseFont we are going to use for the header / footer
   BaseFont bf = null;

   // This keeps track of the creation time
   public DateTime PrintTime { get; set; }
   public byte[] Logoleft { get; set; }
   public byte[] LogoRight { get; set; }
   public string Title { get; set; }
   public string HeaderLeft { get; set; }
   public string HeaderRight { get; set; }
   public Font HeaderFont { get; set; }
   public Font FooterFont { get; set; }
   public string Footer1 { get; set; }
   public string Footer2 { get; set; }

   public string Approval { get; set; }
   public string Lotgsheet_Status { get; set; }
   public string Report_Code { get; set; }
   public Logsheet Logsheet { get; set; }

   // write on top of document
   public override void OnOpenDocument(PdfWriter writer, Document document)
   {
      base.OnOpenDocument(writer, document);
      try
      {
         if (PrintTime == null)
         {
            PrintTime = DateTime.Now;
         }
         bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
         cb = writer.DirectContent;
         template = cb.CreateTemplate(50, 50);
      }
      catch (DocumentException de)
      {
         Console.WriteLine(de.Message);
      }
      catch (System.IO.IOException ioe)
      {
         Console.WriteLine(ioe.Message);
      }
   }








   // write on start of each page
   public override void OnStartPage(PdfWriter writer, Document document)
   {
      base.OnStartPage(writer, document);
      if (Report_Code == Page_Code.P0007)
      {
         /*Logsheet*/
         if (Logsheet != null)
         {
            Rectangle pageSize = document.PageSize;
            int pageN = writer.PageNumber;
            String text = "Page No. " + pageN + " / ";
            float len = bf.GetWidthPoint(text, 8);
            cb.SetRGBColorFill(100, 100, 100);
            cb.BeginText();
            cb.SetFontAndSize(bf, 8);
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
               text,
                pageSize.GetRight(40),
                pageSize.GetTop(30), 0);
            cb.EndText();
            cb.AddTemplate(template, pageSize.GetRight(40), pageSize.GetTop(30));            
         }
      }

   }
   // write on end of each page
   public override void OnEndPage(PdfWriter writer, Document document)
   {
      base.OnEndPage(writer, document);
      //Rectangle pageSize = document.PageSize;
      //int pageN = writer.PageNumber;
      //String text = "";
      ////String text = "Page " + pageN + " of ";
      //float len = bf.GetWidthPoint(text, 8);
      //cb.SetRGBColorFill(100, 100, 100);
      //cb.BeginText();
      //cb.SetFontAndSize(bf, 8);
      //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
      //   text,
      //    pageSize.GetRight(40),
      //    pageSize.GetBottom(30), 0);
      //cb.EndText();
      //cb.AddTemplate(template, pageSize.GetRight(40), pageSize.GetBottom(30));
   }

   //write on close of document
   public override void OnCloseDocument(PdfWriter writer, Document document)
   {

      base.OnCloseDocument(writer, document);
      template.BeginText();
      template.SetFontAndSize(bf, 8);
      template.SetTextMatrix(0, 0);
      template.ShowText((writer.PageNumber).ToString());
      template.ShowText("");
      template.EndText();
   }
}