using QuestPDF.Fluent;
using QuestPDF.Helpers;
using WEBAPI_m1IL_1.Models;

public class PdfService
{
    public static MemoryStream GenerateInvoicePdf(AdsItems ad)
    {
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Row(row =>
                    {
                        row.RelativeItem()
                            .Column(column =>
                            {
                                column.Item().Text("Vendeur :")
                                    .Bold();
                                column.Item().Text("Campus Igensia");
                                column.Item().Text("123 Rue de l'Annonce");
                                column.Item().Text("31000 Toulouse, France");
                                column.Item().Text("ipi@blagnac.com");
                            });

                        row.ConstantItem(200)
                            .Column(column =>
                            {
                                column.Item().AlignRight().Text($"Facture N° {ad.Id}")
                                    .FontSize(16).Bold();
                                column.Item().AlignRight().Text($"Date : {DateTime.UtcNow:dd/MM/yyyy}");
                            });
                    });

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        column.Item().Text("Détails de l'annonce").FontSize(14).Bold().Underline();

                        column.Item().Text($"Titre : {ad.Title}");
                        column.Item().Text($"Catégorie : {ad.Category}");
                        column.Item().Text($"Lieu : {ad.Location}");
                        column.Item().Text($"Description : {ad.Description}");
                        column.Item().Text($"Date de publication : {DateTime.Now:dd/MM/yyyy}");

                        column.Item().PaddingTop(15).Text("Résumé de paiement").FontSize(14).Bold().Underline();

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Text("Montant total :").Bold();
                            row.ConstantItem(100).AlignRight().Text($"{ad.Price:0.00} €").Bold();
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(txt =>
                    {
                        txt.Span("Page ");
                        txt.CurrentPageNumber();
                        txt.Span(" / ");
                        txt.TotalPages();
                    });
            });
        });

        var stream = new MemoryStream();
        document.GeneratePdf(stream);
        stream.Position = 0;
        return stream;
    }

}