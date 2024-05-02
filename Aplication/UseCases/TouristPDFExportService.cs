using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Microsoft.Win32;
using QuestPDF.Infrastructure;
using System.IO;
using System.Windows;

namespace BookingApp.Aplication.UseCases
{
    public class TouristPDFExportService
    {

        public string filePath = "TourRequestStatistic.pdf";
        public Tourist tourist { get; set; }
        public TourRequest tourRequest { get; set; }
        public string year { get; set; }
        public double averageNumberOfPeople { get; set; }
        private TouristService touristService;
        public TouristPDFExportService(int touristId, string year, double averageNumberOfpeople)
        {
            // Export tourist to PDF
            QuestPDF.Settings.License = LicenseType.Community;
            tourRequest = new TourRequest();
            this.year = year;
            touristService = new TouristService(Injector.CreateInstance<ITouristRepository>());
            tourist = touristService.GetByUserId(touristId);
            this.averageNumberOfPeople = averageNumberOfpeople;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.FileName = "TourRequestStatistic";
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }

            Generate();
        }
        public void Generate()
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    Document.Create(container =>
                    {
                        container.Page(page =>
                        {
                            page.Size(PageSizes.A4);
                            page.Margin(1, Unit.Centimetre);
                            page.PageColor(Colors.White);
                            page.DefaultTextStyle(x => x.FontSize(20));


                            page.Header().Element(ComposeHeader);
                            page.Content().Element(ComposeContent);

                            page.Footer()
                                .PaddingBottom(10)
                                .AlignCenter()
                                .Text(x =>
                                {
                                    x.Span("Page ");
                                    x.CurrentPageNumber();
                                });
                        });
                    })
                .GeneratePdf(filePath);

                Process.Start("explorer.exe", filePath);

                }
            }
            catch (IOException)
            {
                // File is open
                Style style = Application.Current.FindResource("MessageStyle") as Style;
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("The file is currently open. Please close it and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning, style);
                return;
            }
            
        }
        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(25).Bold().FontColor(Colors.Green.Darken2);
            var textStyle = TextStyle.Default.FontSize(18).SemiBold().FontColor(Colors.Black);

            container.PaddingTop(10).Column(column =>
            {
                column.Item().AlignCenter().Text("Statistics on tour requests").Style(titleStyle);

                column.Item().PaddingTop(20).AlignLeft().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text($"Tourist:").Style(textStyle);

                        column.Item().Text(text =>
                        {
                            text.Span($"{tourist.FirstName}").FontColor(Colors.Green.Medium).SemiBold();
                        });

                        column.Item().Text(text =>
                        {
                            text.Span($"{tourist.LastName}").FontColor(Colors.Green.Medium).SemiBold();
                        });
                    });
                    row.RelativeItem().AlignRight().Column(column =>
                    {
                        column.Item().Text($"Export time:").Style(textStyle);

                        column.Item().AlignCenter().Text(text =>
                        {
                            text.Span($"{DateTime.Now:dd/MM/yyyy hh:mm}").FontColor(Colors.Green.Medium).SemiBold();
                        });
                    });
                });
            });
        }
        void ComposeContent(IContainer container)
        {
            var textStyle = TextStyle.Default.FontSize(16).FontColor(Colors.Black);
            container.PaddingTop(0).PaddingBottom(0).AlignMiddle().Column(column =>
            {

                column.Item().BorderBottom(2).BorderTop(2).BorderColor(Colors.Green.Medium).PaddingTop(80).PaddingBottom(80).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Image(@"C:\Users\User\Desktop\Sims\Projekat\sims-ra-2024-group-1-team-a\Resources\Images\TourImages\PyeChart.png");
                    row.RelativeItem().AlignRight().AlignCenter().PaddingTop(150).Text(text =>
                    {
                        text.Span($"The average number of people").Style(textStyle);
                        text.Span($" in accepted requests for {year}:").Style(textStyle);
                        text.Span($" {averageNumberOfPeople:F2}").SemiBold().Style(textStyle).FontColor(Colors.Red.Medium);
                    });
                });
                column.Item().BorderBottom(2).BorderTop(2).BorderColor(Colors.Green.Medium).PaddingTop(40).PaddingBottom(40).AlignLeft().Column(row =>
                {
                    row.Item().AlignRight().Image(@"C:\Users\User\Desktop\Sims\Projekat\sims-ra-2024-group-1-team-a\Resources\Images\TourImages\LanguageChart.png");
                });

                column.Item().BorderBottom(2).BorderTop(2).BorderColor(Colors.Green.Medium).PaddingTop(40).PaddingBottom(40).AlignLeft().Column(row =>
                {
                    row.Item().AlignRight().Image(@"C:\Users\User\Desktop\Sims\Projekat\sims-ra-2024-group-1-team-a\Resources\Images\TourImages\LocationChart.png");
                });
            });
        }
    }
}
