using System;
using System.Globalization;
using System.IO;
using iText.Forms;
using iText.Forms.Fields;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;

namespace Utilities
{
    public class PdfService
    {
        public class AuthorizationContractDetails
        {
            public List<int> SelectedRoom { get; set; }
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

            if (!File.Exists(templatePath))
                throw new FileNotFoundException($"Template file not found at: {templatePath}");
            if (!File.Exists(fontPath))
                throw new FileNotFoundException($"Font file not found at: {fontPath}");

            using var memoryStream = new MemoryStream();
            using var pdfReader = new PdfReader(templatePath);
            using var pdfWriter = new PdfWriter(memoryStream);
            using var pdfDoc = new PdfDocument(pdfReader, pdfWriter);

            PdfFont font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H,
                                                      PdfFontFactory.EmbeddingStrategy.FORCE_EMBEDDED);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            form.SetGenerateAppearance(true);

            foreach (var field in form.GetAllFormFields())
            {
                PdfFormField pdfField = field.Value;
                pdfField.SetFont(font)
                        .SetFontSize(0);
            }

            form.GetField("ContractNumber").SetValue(details.ContractNumber);

            // Format ngày sinh A
            form.GetField("PartyABirthDate")
                .SetValue(details.PartyABirthDate.ToString("dd-MM-yyyy"));
            form.GetField("PartyAName").SetValue(details.PartyAName);
            form.GetField("PartyAIDNumber").SetValue(details.PartyAIDNumber);
            // Format ngày cấp CMND/CCCD A
            form.GetField("PartyAIDIssueDate")
                .SetValue(DateTime.Parse(details.PartyAIDIssueDate)
                              .ToString("dd-MM-yyyy"));
            form.GetField("PartyAIDIssuePlace").SetValue(details.PartyAIDIssuePlace);
            form.GetField("PartyAAddress").SetValue(details.PartyAAddress);

            form.GetField("PartyBName").SetValue(details.PartyBName);
            // Format ngày sinh B
            form.GetField("PartyBBirthDate")
                .SetValue(details.PartyBBirthDate.ToString("dd-MM-yyyy"));
            form.GetField("PartyBIDNumber").SetValue(details.PartyBIDNumber);
            // Format ngày cấp CMND/CCCD B
            form.GetField("PartyBIDIssueDate")
                .SetValue(DateTime.Parse(details.PartyBIDIssueDate)
                              .ToString("dd-MM-yyyy"));
            form.GetField("PartyBIDIssuePlace").SetValue(details.PartyBIDIssuePlace);
            form.GetField("PartyBAddress").SetValue(details.PartyBAddress);

            form.GetField("ScopeOfAuthorization").SetValue(details.ScopeOfAuthorization);
            form.GetField("Duration").SetValue(details.Duration);

            DateTime startDate = details.StartDate;
            string dayStart = startDate.Day.ToString("d2");
            string monthStart = startDate.Month.ToString("d2");
            string yearStart = startDate.Year.ToString();

            form.GetField("DayStart").SetValue(dayStart);
            form.GetField("MonthStart").SetValue(monthStart);
            form.GetField("YearStart").SetValue(yearStart);

            if (details.Fee.HasValue)
            {
                var feeText = details.Fee.Value
                    .ToString("N0", CultureInfo.GetCultureInfo("vi-VN"));
                form.GetField("Fee").SetValue(feeText);
            }

            form.GetField("FeePayer").SetValue(details.FeePayer);
            // Format ngày hiệu lực
            form.GetField("EffectiveDate")
                .SetValue(DateTime.Parse(details.EffectiveDate)
                              .ToString("dd-MM-yyyy"));
            var scopeText = details.SelectedRoom != null && details.SelectedRoom.Any()
        ? $"Quản lý {details.SelectedRoom.Count} phòng: {string.Join(", ", details.SelectedRoom.Select(id => $"Phòng {id}"))}"
        : details.ScopeOfAuthorization;
            form.FlattenFields();
            pdfDoc.Close();

            return memoryStream.ToArray();
        }
    }
}