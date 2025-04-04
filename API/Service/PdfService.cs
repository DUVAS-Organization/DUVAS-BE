using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;
using System;
using System.Reflection.PortableExecutable;
using iText.Kernel.Font; // Thêm namespace này
using iText.IO.Font;    // Thêm namespace này

namespace Utilities
{
    public class PdfService
    {
        public class AuthorizationContractDetails
        {
            //public DateTime Date { get; set; }
            //public string CertificationLocation { get; set; }
            public int PartyAId { get; set; }
            public string PartyAName { get; set; }
            public DateTime PartyABirthDate { get; set; }
            public string PartyAIDNumber { get; set; }
            public string PartyAIDIssueDate { get; set; }
            public string PartyAIDIssuePlace { get; set; }
            public string PartyAAddress { get; set; }
            public int PartyBId { get; set; }
            public string PartyBName { get; set; }
            public DateTime PartyBBirthDate { get; set; }
            public string PartyBIDNumber { get; set; }
            public string PartyBIDIssueDate { get; set; }
            public string PartyBIDIssuePlace { get; set; }
            public string PartyBAddress { get; set; }
            public string ScopeOfAuthorization { get; set; }
            public DateTime StartDate { get; set; }
            public string Duration { get; set; }
            public decimal? Fee { get; set; }
            public string FeePayer { get; set; }
            public string EffectiveDate { get; set; }
            public string PartyASignature { get; set; }
            public string PartyBSignature { get; set; }
            public string ContractNumber { get; set; }
        }

        public byte[] GenerateAuthorizationContractPdf(AuthorizationContractDetails details)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string templatePath = "Templates/hop-dong-uy-quyen.pdf";
            string fontPath = "Fonts/arial.ttf";
            //string templatePath = Path.Combine(basePath, "Templates", "hop-dong-uy-quyen.pdf");
            //string fontPath = Path.Combine(basePath, "Fonts", "arial.ttf"); // Đường dẫn tới file font

            // Kiểm tra file tồn tại
            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template file not found at: {templatePath}");
            if (!File.Exists(fontPath))
                throw new FileNotFoundException($"Font file not found at: {fontPath}");

            using var memoryStream = new MemoryStream();
            using var pdfReader = new PdfReader(templatePath);
            using var pdfWriter = new PdfWriter(memoryStream);
            using var pdfDoc = new PdfDocument(pdfReader, pdfWriter);

            // Tạo font từ file .ttf
            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H, PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            // Đặt font cho tất cả các trường
            foreach (var field in form.GetAllFormFields())
            {
                PdfFormField pdfField = field.Value;
                pdfField.SetFont(font); // Áp dụng font hỗ trợ tiếng Việt
            }

            form.GetField("ContractNumber").SetValue(details.ContractNumber);

            //// Lấy giá trị ngày tháng năm từ details.PartyBBirthDate
            //DateTime date = details.Date;

            //    // Tách riêng ngày, tháng, năm
            //    string day = date.Day.ToString("d2");    // Đảm bảo 2 chữ số (VD: 01)
            //    string month = date.Month.ToString("d2"); // Đảm bảo 2 chữ số (VD: 04)
            //    string year = date.Year.ToString();      // Năm đầy đủ (VD: 2025)

            //// Gán giá trị vào các field riêng biệt
            //form.GetField("Day").SetValue(day);     // Ngày
            //form.GetField("Month").SetValue(month); // Tháng
            //form.GetField("Year").SetValue(year);   // Năm

            //form.GetField("CertificationLocation").SetValue(details.CertificationLocation);
            form.GetField("PartyAName").SetValue(details.PartyAName);

            form.GetField("PartyABirthDate").SetValue(details.PartyABirthDate.ToString("dd/MM/yyyy"));

            //    // Lấy giá trị ngày tháng năm từ details.PartyABirthDate
            //    DateTime birthDateA = details.PartyABirthDate;

            //    // Tách riêng ngày, tháng, năm
            //    string dayA = birthDateA.Day.ToString("d2");    // Đảm bảo 2 chữ số (VD: 01)
            //    string monthA = birthDateA.Month.ToString("d2"); // Đảm bảo 2 chữ số (VD: 03)
            //    string yearA = birthDateA.Year.ToString();      // Năm đầy đủ (VD: 2025)

            //// Gán giá trị vào các field riêng biệt
            //form.GetField("PartyADay").SetValue(dayA);     // Ngày
            //form.GetField("PartyAMonth").SetValue(monthA); // Tháng
            //form.GetField("PartyAYear").SetValue(yearA);   // Năm

            form.GetField("PartyAIDNumber").SetValue(details.PartyAIDNumber);
            form.GetField("PartyAIDIssueDate").SetValue(details.PartyAIDIssueDate);
            form.GetField("PartyAIDIssuePlace").SetValue(details.PartyAIDIssuePlace);
            form.GetField("PartyAAddress").SetValue(details.PartyAAddress);
            form.GetField("PartyBName").SetValue(details.PartyBName);

            form.GetField("PartyBBirthDate").SetValue(details.PartyBBirthDate.ToString("dd/MM/yyyy"));

            //    // Lấy giá trị ngày tháng năm từ details.PartyBBirthDate
            //    DateTime birthDateB = details.PartyBBirthDate;

            //    // Tách riêng ngày, tháng, năm
            //    string dayB = birthDateB.Day.ToString("d2");    // Đảm bảo 2 chữ số (VD: 01)
            //    string monthB = birthDateB.Month.ToString("d2"); // Đảm bảo 2 chữ số (VD: 04)
            //    string yearB = birthDateB.Year.ToString();      // Năm đầy đủ (VD: 2025)

            //// Gán giá trị vào các field riêng biệt
            //form.GetField("PartyBDay").SetValue(dayB);     // Ngày
            //form.GetField("PartyBMonth").SetValue(monthB); // Tháng
            //form.GetField("PartyBYear").SetValue(yearB);   // Năm

            form.GetField("PartyBIDNumber").SetValue(details.PartyBIDNumber);
            form.GetField("PartyBIDIssueDate").SetValue(details.PartyBIDIssueDate);
            form.GetField("PartyBIDIssuePlace").SetValue(details.PartyBIDIssuePlace);
            form.GetField("PartyBAddress").SetValue(details.PartyBAddress);
            form.GetField("ScopeOfAuthorization").SetValue(details.ScopeOfAuthorization);
            form.GetField("Duration").SetValue(details.Duration);

            // Lấy giá trị ngày tháng năm từ details.PartyBBirthDate
            DateTime startDate = details.StartDate;

                // Tách riêng ngày, tháng, năm
                string dayStart = startDate.Day.ToString("d2");    // Đảm bảo 2 chữ số (VD: 01)
                string monthStart = startDate.Month.ToString("d2"); // Đảm bảo 2 chữ số (VD: 04)
                string yearStart = startDate.Year.ToString();      // Năm đầy đủ (VD: 2025)

            // Gán giá trị vào các field riêng biệt
            form.GetField("DayStart").SetValue(dayStart);     // Ngày
            form.GetField("MonthStart").SetValue(monthStart); // Tháng
            form.GetField("YearStart").SetValue(yearStart);   // Năm

                if (details.Fee.HasValue)
            form.GetField("Fee").SetValue(details.Fee.Value.ToString("N2"));
            form.GetField("FeePayer").SetValue(details.FeePayer);
            form.GetField("EffectiveDate").SetValue(details.EffectiveDate);
            form.FlattenFields();
            pdfDoc.Close();

            return memoryStream.ToArray();
        }
    }
}