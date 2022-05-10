using Microsoft.AspNetCore.Mvc;
using QRCoder;
using QuizExperiment.Admin.Server.Services;
using System.Diagnostics;

namespace QuizExperiment.Admin.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Quiz : ControllerBase
    {
        private Random random = new Random();

        [HttpGet("newid")]
        public string GenerateClientQuizId([FromQuery]string questionSetId)
        {
            Debug.WriteLine($"Generating new Client Quiz Id for QuestionSet: {questionSetId}");
            var s = string.Join("", Enumerable.Range(1, 4).Select(index => random.Next(0,9).ToString()).ToArray());
            return s;
        }

        [HttpGet("qrcode")]
        public IActionResult GetQRCode([FromQuery]string? clientQuizId)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}?clientQuizId={clientQuizId}";
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(baseUrl, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(qrCodeData);
            var qrCodeAsBytes = qrCode.GetGraphic(20);

            return File(qrCodeAsBytes, "image/png");
        }
    }
    
}
