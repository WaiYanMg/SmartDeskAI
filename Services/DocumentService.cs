using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;

namespace Smart_Desk_AI.Services
{
    public class DocumentService
    {
        private readonly IWebHostEnvironment _env;

        public DocumentService(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Load a policy PDF from the Policies folder
       
public Task<string> LoadPolicyAsync(string fileName)
{
    var path = Path.Combine(_env.ContentRootPath, "Policies", fileName);

    if (!File.Exists(path))
        return Task.FromResult($"Policy document '{fileName}' not found.");

    var extension = Path.GetExtension(fileName).ToLower();

    if (extension == ".pdf")
        return Task.FromResult(ExtractFromPdf(path));

    if (extension == ".docx")
        return Task.FromResult(ExtractFromDocx(path));

    if (extension == ".txt")
        return Task.FromResult(File.ReadAllText(path));

    return Task.FromResult(string.Empty);
}
        // Extract text from uploaded form file
        public async Task<string> ExtractFromUploadAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            var tempPath = Path.GetTempFileName();

            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var text = extension == ".pdf"
                ? ExtractFromPdf(tempPath)
                : ExtractFromDocx(tempPath);

            File.Delete(tempPath);
            return text;
        }

        private string ExtractFromPdf(string path)
        {
            var text = new System.Text.StringBuilder();
            using var pdf = PdfDocument.Open(path);
            foreach (var page in pdf.GetPages())
                text.AppendLine(page.Text);
            return text.ToString();
        }

        private string ExtractFromDocx(string path)
        {
            var text = new System.Text.StringBuilder();
            using var doc = WordprocessingDocument.Open(path, false);
            var body = doc.MainDocumentPart?.Document?.Body;
            if (body != null)
                text.Append(body.InnerText);
            return text.ToString();
        }
    }
}