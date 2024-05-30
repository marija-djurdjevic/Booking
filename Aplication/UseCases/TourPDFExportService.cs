using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using BookingApp.Domain.Models;
using BookingApp.Domain.RepositoryInterfaces;
using Microsoft.Expression.Interactivity.Media;
using Microsoft.Win32;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BookingApp.Aplication.UseCases
{
    public class TourPDFExportService
    {
        public string FilePath { get; set; }
        public Tour Tour { get; set; }
        public double AverageNumberOfPeople { get; set; }
        private readonly TourService _tourService;

        public int Under18Count {  get; set; }
        public int Between18And50Count { get; set; }
        public string Fullname {  get; set; }

        public int Over50Count { get; set; }    
        public TourPDFExportService(int tourId, int under18,int between18and50,int over50,int averageNumberOfPeople,string fullname)
        {
            // Export tour to PDF
            QuestPDF.Settings.License = LicenseType.Community;
            _tourService = new TourService(Injector.CreateInstance<ITourRepository>(), Injector.CreateInstance<ILiveTourRepository>());
            Tour = _tourService.GetTourById(tourId);
            AverageNumberOfPeople = averageNumberOfPeople;
            Under18Count = under18;
            Between18And50Count=between18and50;
            Over50Count= over50;
            Fullname= fullname;

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                FileName = "TourStatistics"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                Generate();
            }
        }



        public void Generate()
        {
            try
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

                        page.Footer().BorderTop(2).BorderColor(Colors.Green.Medium).PaddingBottom(10).AlignCenter()
                            .Text(x => { x.Span("Page "); x.CurrentPageNumber(); });
                    });
                }).GeneratePdf(FilePath);

                MessageBoxResult result = MessageBox.Show("Pdf generated! Do you want to open file now?", "Open", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start("explorer.exe", FilePath);
                }
            }
            catch (IOException)
            {
                // File is open
                MessageBox.Show("The file is currently open. Please close it and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).Bold().FontColor(Colors.Black);
            var textStyle = TextStyle.Default.FontSize(18).SemiBold().FontColor(Colors.Black);

            container.PaddingTop(0).BorderBottom(2).BorderColor(Colors.Green.Medium).Column(column =>
            {
                column.Item().PaddingTop(5).AlignLeft().Row(row =>
                {
                    row.AutoItem().Width(55).AlignLeft().MaxWidth(50).ScaleToFit().Column(column =>
                    {
                        column.Item().AlignLeft().Height(50).Width(50).Image(ImageService.GetAbsolutePath(@"Resources\Icons\TouristIcons\mobile.png"));
                    });
                    row.RelativeItem().AlignLeft().Column(column =>
                    {
                        column.Item().AlignLeft().Text("BOOKING APP").Style(titleStyle).FontSize(28).FontFamily(Fonts.SegoeSD).FontColor(Colors.BlueGrey.Medium);
                    });
                });

                column.Item().AlignCenter().Text("Tour Statistics").Style(titleStyle);

                column.Item().PaddingTop(2).AlignLeft().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text($"Guide:").Style(textStyle);

                        column.Item().Text(text =>
                        { text.Span($"{Fullname}").FontColor(Colors.Green.Medium).SemiBold(); });
                    });
                    row.RelativeItem().AlignRight().Column(column =>
                    {
                        column.Item().Text($"Export date:").Style(textStyle);

                        column.Item().AlignCenter().Text(text =>
                        { text.Span($"{DateTime.Now:dd/MM/yyyy}").FontColor(Colors.Green.Medium).SemiBold(); });
                    });
                });
            });
        }


        void ComposeContent(IContainer container)
        {
            var textStyle = TextStyle.Default.FontSize(16).FontColor(Colors.Black);
            container.PaddingTop(-230).PaddingBottom(0).AlignMiddle().Column(column =>
            {

                column.Item().PaddingTop(20).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text("Tour Details").Style(textStyle).FontColor(Colors.Black).FontSize(24).SemiBold();

                });

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text($"   Tour Name: {Tour.Name}").Style(textStyle).FontColor(Colors.Black);

                });

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text($"   Location: {Tour.Location.City} , {Tour.Location.Country}").Style(textStyle).FontColor(Colors.Black);
                });

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text($"   Language: {Tour.Language}").Style(textStyle).FontColor(Colors.Black);
                });


                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text($"   Start Date and Time: {Tour.StartDateTime:dd/MM/yyyy HH:mm}").Style(textStyle).FontColor(Colors.Black);
                });

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text("   Tour Key Points:").Style(textStyle).FontColor(Colors.Black);
                });

                foreach (var keyPoint in Tour.KeyPoints)
                {
                    column.Item().PaddingTop(4).Text($"     {keyPoint.OrdinalNumber}. {keyPoint.Name}").Style(textStyle).FontColor(Colors.Black);
                }


               
                column.Item().PaddingTop(20).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text("Tourists Age Groups:").Style(textStyle).FontColor(Colors.Black).FontSize(24).SemiBold();
                });

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text("  Under 18 years: " + Under18Count).Style(textStyle).FontColor(Colors.Black);
                });

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text("  Between 18 and 50 years: " + Between18And50Count).Style(textStyle).FontColor(Colors.Black);
                });

                column.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem().AlignLeft().Text("  Over 50 years: " + Over50Count).Style(textStyle).FontColor(Colors.Black);
                });


            });
        }



    }
}
