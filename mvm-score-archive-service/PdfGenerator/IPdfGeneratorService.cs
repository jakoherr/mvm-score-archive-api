using Mvm.Score.Archive.Service.Parts;

namespace Mvm.Score.Archive.Service.PdfGenerator;

public interface IPdfGeneratorService
{
    Stream GeneratePartPdfAsStream(Part part);
}