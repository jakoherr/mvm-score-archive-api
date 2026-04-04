using Mvm.Score.Archive.Service.Parts;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Mvm.Score.Archive.Service.PdfGenerator;

public class PdfGeneratorService : IPdfGeneratorService
{
    public PdfGeneratorService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public Stream GeneratePartPdfAsStream(Part part)
    {
        Stream stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);

                page.Content()
                    .AlignMiddle()
                    .AlignCenter()
                    .Column(column =>
                    {
                        column.Spacing(15);

                        column.Item().Text("⚠ Stimme nicht gefunden ⚠")
                            .FontSize(26)
                            .Bold()
                            .FontColor(Colors.Red.Darken2);

                        column.Item().Text($"Instrument: {part.Instrument}")
                            .FontSize(16);

                        column.Item().Text($"Stimme: {part.PartNumber}")
                            .FontSize(16);

                        column.Item().Text($"Stimmung: {part.Tuning}")
                            .FontSize(16);

                        column.Item().Text("Die angeforderte Stimme konnte nicht gefunden werden!")
                            .FontSize(14)
                            .Italic()
                            .FontColor(Colors.Grey.Darken1);
                    });
            });
        }).GeneratePdf(stream);

        stream.Position = 0;

        return stream;
    }
}