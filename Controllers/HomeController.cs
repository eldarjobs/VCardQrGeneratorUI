using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Diagnostics;
using System.Text;
using VCardQRGenerator.Models;

namespace VCardQRGenerator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(VCardDetails model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            string vcfContent = BuildVCardContent(model.Details);
            model.QrCode = GenerateQrCode(vcfContent);

            return View(model);
        }

        private string BuildVCardContent(Details details)
        {
            var sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCARD");
            sb.AppendLine("VERSION:3.0");
            sb.AppendLine($"FN:{details.FirstName} {details.LastName}");
            sb.AppendLine($"N:{details.LastName};{details.FirstName};;;;");
            sb.AppendLine($"EMAIL;TYPE=INTERNET:{details.Email}");
            sb.AppendLine($"TEL;TYPE=WORK:{details.PhoneNumber}");
            sb.AppendLine($"ADR;TYPE=HOME;LABEL=\"{details.City}, {details.Country}\"");
            sb.AppendLine($"ORG:{details.Company}");
            sb.AppendLine($"TITLE:{details.Job}");
            sb.AppendLine($"ADR;TYPE=WORK;LABEL=\"{details.ZIP}\"");
            sb.AppendLine($"TEL;TYPE=FAX:{details.Fax}");
            sb.AppendLine("END:VCARD");
            return sb.ToString();
        }

        private string GenerateQrCode(string vcfContent)
        {
            using (var ms = new MemoryStream())
            {
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(vcfContent, QRCodeGenerator.ECCLevel.H);
                var qrCode = new Base64QRCode(qrCodeData);
                return qrCode.GetGraphic(20);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new VCardError { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
