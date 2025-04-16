using iText.Kernel.Pdf;
using iText.Forms;
using iText.Forms.Fields;
using System;
using System.Reflection.PortableExecutable;

namespace Utilities
{
    public class PdfService
    {
        public class AuthorizationContractDetails
        {
            public string Date { get; set; }
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
            public string StartDate { get; set; }
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
            string templatePath = "Templates/2-mau-hop-dong-uy-quyen-cho-thue-nha_2105180427.pdf";
            using var memoryStream = new MemoryStream();
            using var pdfReader = new PdfReader(templatePath);
            using var pdfWriter = new PdfWriter(memoryStream);
            using var pdfDoc = new PdfDocument(pdfReader, pdfWriter);

            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);

            form.GetField("Date").SetValue(details.Date);
            //form.GetField("CertificationLocation").SetValue(details.CertificationLocation);
            form.GetField("PartyAName").SetValue(details.PartyAName);
            form.GetField("PartyABirthDate").SetValue(details.PartyABirthDate.ToString("dd/MM/yyyy"));
            form.GetField("PartyAIDNumber").SetValue(details.PartyAIDNumber);
            form.GetField("PartyAIDIssueDate").SetValue(details.PartyAIDIssueDate);
            form.GetField("PartyAIDIssuePlace").SetValue(details.PartyAIDIssuePlace);
            form.GetField("PartyAAddress").SetValue(details.PartyAAddress);
            form.GetField("PartyBName").SetValue(details.PartyBName);
            form.GetField("PartyBBirthDate").SetValue(details.PartyBBirthDate.ToString("dd/MM/yyyy"));
            form.GetField("PartyBIDNumber").SetValue(details.PartyBIDNumber);
            form.GetField("PartyBIDIssueDate").SetValue(details.PartyBIDIssueDate);
            form.GetField("PartyBIDIssuePlace").SetValue(details.PartyBIDIssuePlace);
            form.GetField("PartyBAddress").SetValue(details.PartyBAddress);
            form.GetField("ScopeOfAuthorization").SetValue(details.ScopeOfAuthorization);
            form.GetField("StartDate").SetValue(details.StartDate);
            form.GetField("Duration").SetValue(details.Duration);
            if (details.Fee.HasValue)
                form.GetField("Fee").SetValue(details.Fee.Value.ToString("N2"));
            form.GetField("FeePayer").SetValue(details.FeePayer);
            form.GetField("EffectiveDate").SetValue(details.EffectiveDate);
            form.GetField("ContractNumber").SetValue(details.ContractNumber);

            form.FlattenFields();
            pdfDoc.Close();

            return memoryStream.ToArray();
        }
    }
}