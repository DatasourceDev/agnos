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


namespace AgnosCMS.Common
{
   public class ReportUtil
   {
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
      Rectangle pageSize = document.PageSize;
      if (Report_Code == Page_Code.P0007 )
      {
         /*Logsheet*/
         if(Logsheet != null)
         {
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fontNormal = new Font(bf, 10, Font.NORMAL);
            Font fontH3 = new Font(bf, 14, Font.BOLD | Font.UNDERLINE);

            PdfPTable tableheader = new PdfPTable(1);
            tableheader.TotalWidth = pageSize.Width;
            PdfPCell hc1 = new PdfPCell(new Phrase(Resource.Logsheet, fontH3));
            hc1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            hc1.BorderWidth = 0;
            hc1.Padding = 5f;
            hc1.PaddingBottom = 20f;
            tableheader.AddCell(hc1);
            document.Add(tableheader);

            PdfPTable table = new PdfPTable(3);
            table.TotalWidth = pageSize.Width;
            table.SetWidthPercentage(new float[] { (float)(table.TotalWidth * 0.3), (float)(table.TotalWidth * 0.5), (float)(table.TotalWidth * 0.2) }, pageSize);


            PdfPCell c1 = new PdfPCell(new Phrase(Resource.Product_Code + ": " + Logsheet.Product_Code, fontNormal));
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.BorderWidth = 1;
            c1.BorderWidthBottom = 0;
            c1.BorderWidthRight = 0;
            c1.Padding = 5f;
            table.AddCell(c1);

            PdfPCell c2 = new PdfPCell(new Phrase(Resource.Template_Logsheet + ": " + Logsheet.Template_Logsheet.Template_Name, fontNormal));
            c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c2.BorderWidth = 1;
            c2.BorderWidthBottom = 0;
            c2.BorderWidthRight = 0;
            c2.Padding = 5f;
            table.AddCell(c2);

            PdfPCell c3 = new PdfPCell(new Phrase(Resource.Date + ": " + DateUtil.ToDisplayDate( Logsheet.Create_On), fontNormal));
            c3.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c3.BorderWidth = 1;
            c3.BorderWidthBottom = 0;
            c3.Padding = 5f;
            table.AddCell(c3);

            c1 = new PdfPCell(new Phrase(Resource.Lot_No + ": " + Logsheet.Lot_No, fontNormal));
            c1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c1.BorderWidth = 1;
            c1.BorderWidthTop = 0;
            c1.BorderWidthRight = 0;
            c1.Padding = 5f;
            c1.PaddingBottom = 10f;
            table.AddCell(c1);

            c2 = new PdfPCell(new Phrase(Resource.Work_Order_No + ": " + Logsheet.Work_Order_No, fontNormal));
            c2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            c2.BorderWidth = 1;
            c2.BorderWidthTop = 0;
            c2.BorderWidthRight = 0;
            c2.Padding = 5f;
            c2.PaddingBottom = 10f;
            table.AddCell(c2);

            c3 = new PdfPCell(new Phrase());
            c3.BorderWidth = 1;
            c3.BorderWidthTop = 0;
            c3.Padding = 5f;
            c3.PaddingBottom = 10f;
            table.AddCell(c3);
            document.Add(table);
            document.Add(new Paragraph("\n"));
         }
      }

   }
   // write on end of each page
   public override void OnEndPage(PdfWriter writer, Document document)
   {
      base.OnEndPage(writer, document);
      Rectangle pageSize = document.PageSize;
      int pageN = writer.PageNumber;
      String text = "";
      //String text = "Page " + pageN + " of ";
      float len = bf.GetWidthPoint(text, 8);
      cb.SetRGBColorFill(100, 100, 100);
      cb.BeginText();
      cb.SetFontAndSize(bf, 8);
      cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
         text,
          pageSize.GetRight(40),
          pageSize.GetBottom(30), 0);
      cb.EndText();
      cb.AddTemplate(template, pageSize.GetRight(40), pageSize.GetBottom(30));

   }

   //write on close of document
   public override void OnCloseDocument(PdfWriter writer, Document document)
   {
      base.OnCloseDocument(writer, document);
      template.BeginText();
      template.SetFontAndSize(bf, 8);
      template.SetTextMatrix(0, 0);
      //template.ShowText("" + (writer.PageNumber));
      template.ShowText("");
      template.EndText();

      
   }
}