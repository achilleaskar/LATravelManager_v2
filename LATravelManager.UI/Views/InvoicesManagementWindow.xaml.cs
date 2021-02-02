using LATravelManager.UI.ViewModel;
using System;
using System.IO;
using System.IO.Packaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace LATravelManager.UI.Views
{
    /// <summary>
    /// Interaction logic for InvoicesManagementWindow.xaml
    /// </summary>
    public partial class InvoicesManagementWindow : Window
    {
        public InvoicesManagementWindow()
        {
            InitializeComponent();
            // //var html ="<!DOCTYPE html> <html lang='en'> <head> <meta http-equiv='Content-Type' content='text/html; charset=UTF-8'> <title>Invoice</title> <meta name='description' content='The HTML5 Herald'> <meta name='author' content='Achilleas Kar'> <style type='text/css'>
            // //body { position: relative; overflow: auto; width: 21cm; height: 29.7cm; padding: 1cm; border: none; } th { padding: 1.5px; font: bold 12px 'Arial'; white-space: nowrap; border: 1px solid black; background-color: lightgray; } td { padding-top: 1.5px; font: 12px 'Arial'; white-space: wrap; } table
            // //{ width: 100%; } table, th, td { border-collapse: collapse; } .logo { position: absolute; margin-top: 22px; margin-left: 5cm; margin-right: 5cm; width: 10.5cm; z-index: -1; } .tdleft { text-align: left; } .tdright { text-align: right; } .borderedtable td { border: 1px solid black; } .textCenter td
            // //{ text-align: center; } .textCenter { text-align: center; } .textCenterandtop { text-align: center; vertical-align: top; } .textbold { font-weight: bold; } #toptable td { font: bold 12px 'Arial'; } #detailsTable thead th { border-bottom: 2px solid black; } #detailsTable table { border: 1px solid black; table-layout: fixed; }
            // //#detailsTable { width: 100%; margin-bottom: 5px; } #footertable { width: 100%; } #footertable th { border-bottom: 2px solid black; } #footertable td { padding: 2px 4px; } .fulltableheight { height: 148px; } .halftableheight { height: 72px; } .dynamictable td { padding: 3px 0px 3px 5px; }
            // //.dynamictable { margin-top: 3px; border: 1px solid black; } </style> </head> <body> <div> <img class='logo' src='data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAfQAAACaCAYAAABfX7oUAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAB0D0lEQVR4Xu2dB2AcxdmG3yt7VTr1YsuSe8OA
            //// </div> <table id='toptable'> <tbody> <tr> <td class='tdleft'>LA TRAVEL</td> <td class='tdright'>LA TRAVEL</td> </tr> <tr> <td class='tdleft'>ΔΗΜΗΤΡΗΣ ΠΑΠΑΓΕΩΡΓΙΟΥ ΚΑΙ ΣΙΑ Ε.Ε.</td> <td class='tdright'>DIMITRIS PAPAGEWRGIOU &amp; SIA E.E.</td> </tr> <tr> <td class='tdleft'>ΓΡΑΦΕΙΟ ΓΕΝΙΚΟΥ ΤΟΥΡΙΣΜΟΥ</td> <td class='tdright'>Travel Agency</td> </tr> <tr style='height: 10px;'> <td class='tdleft'></td> <td class='tdright'></td> </tr> <tr> <td class='tdleft'>Ερμού 36, 4ος Όροφος</td> <td class='tdright'>36, Ermou str., 4th Floor, P.C. 54623</td> </tr> <tr> <td class='tdleft'>Θεσσαλονίκη, Τ.Κ.54623</td> <td class='tdright'>Thessaloniki, Macedonia Greece</td> </tr> <tr> <td class='tdleft'>Τηλ : +30 2310260986</td> <td class='tdright'>Tel : +30 2310260986</td> </tr> <tr> <td class='tdleft'>Email : sales@latravel.gr</td> <td class='tdright'>Email :sales@latravel.gr</td> </tr> <tr> <td class='tdleft'>Web site : www.latravel.gr</td>
            ////<td class='tdright'>Web site : www.latravel.gr</td> </tr> <tr> <td class='tdleft'>ΑΦΜ: 801252307, ΔΟΥ: Δ' Θεσσαλονίκης</td> <td class='tdright'>Tax No.: 801252307, Tax Office: D' Thessalonikis</td> </tr> </tbody> </table> <div> <table id='detailsTable'> <tbody> <tr style='height: 50%;'> <td rowspan='2' > <table style='margin-left: -1px; '
            ////class='fulltableheight' style='width: 70%; table-layout: fixed;'> <col width='115'> <col width='165' align-text='left'> <col width='35'> <thead> <tr> <th colspan='3'>ΣΤΟΙΧΕΙΑ ΠΕΛΑΤΗ / CUSTOMER INFORMATION</th> </tr> </thead> <tbody> <tr> <td>ΚΩΔ. ΠΕΛΑΤΗ / CODE:</td> <td colspan='2'>545</td> </tr> <tr> <td class='textbold'>
            // //ΕΠΩΝΥΜΙΑ / NAME:</td> <td colspan='2' class='textbold'>ΑΘΩΣ ΕΛΛΑΣ Ι.Κ Ε</td> </tr> <tr> <td>ΕΠΑΓΓΕΛΜΑ / ACTIVITY:</td> <td colspan='2'>ΓΡΑΦΕΙΟ ΓΕΝΙΚΟΥ ΤΟΥΡΙΣΜΟΥ</td> </tr> <tr> <td>ΔΙΕΥΘΥΝΣΗ / ADDRES:</td> <td colspan='2'>ΜΗΤΡΟΠΟΛΕΩΣ 16</td> </tr> <tr> <td style='width: 36%;'>ΠΟΛΗ / CITY:</td> <td style='width: 50%;'>ΘΕΣΣΑΛΟΝΙΚΗ</td> <td style='width: 14%;'>54624</td> </tr> <tr> <td>ΤΗΛΕΦΩΝΟ / PHONE:</td> <td colspan='2'>2310284300</td> </tr> <tr> <td class='textbold'>ΑΦΜ / VAT No:</td> <td colspan='2' class='textbold'>998307424</td> </tr> <tr> <td>Δ.Ο.Υ / TAX OFFICE:</td> <td colspan='2'>Α' ΘΕΣ/ΝΙΚΗΣ</td> </tr> </tbody> </table> </td> <td style='vertical-align: top;'> <table class=' borderedtable textCenter halftableheight' style=' margin-left: 1px;' > <col width='160'> <col width='80'> <thead> <tr style=' height: 1px; '> <th>ΕΙΔΟΣ ΠΑΡΑΣΤΑΤΙΚΟΥ / TYPE OF INVOICE</th> <th>ΑΡΙΘΜΟΣ / NUMBER</th> </tr> </thead> <tbody> <tr> <td>ΤΙΜΟΛΟΓΙΟ ΠΑΡΟΧΗΣ ΥΠΗΡΕΣΙΩΝ </td> <td style=' width: 100px; font-size: 12px; '>812</td> </tr> </tbody> </table> </td> </tr> <tr > <td > <table class=' borderedtable textCenter halftableheight ' style=' margin-left: 1px; margin-top: 1px; vertical-align: bottom;'> <col width='160'> <col width='80'> <thead> <tr style=' height: 1px; '> <th>ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ / PAYMENT</th> <th style=' font-size: 10px; '>ΗΜΕΡΟΜΗΝΙΑ / DATE</th> </tr> </thead> <tbody> <tr> <td>On Credit / Με Πίστωση</td> <td style=' width: 120px; font-size: 12px; '>04/03/2020</td> </tr> <tr> <td colspan=' 2 ' style=' text-align: right; padding-right: 10px; '> <span style=' font-weight: bold; '>Σελίδα / page</span> 1 <span style=' font-weight: bold; '> από/out of</span> 1 </td> </tr> </tbody> </table> </td> </tr> </tbody> </table> </div> <table id='listheader '> <colgroup> <col width='max-content '> <col width='100px '> <col width='100px '> <col width='100px '> </colgroup> <tbody> <tr> <th>ΠΕΡΙΓΡΑΦΗ / DESCRIPTION</th> <th>ΑΞΙΑ / VALUE</th> <th>Φ.Π.Α. / VAT</th> <th> ΤΕΛΙΚΗ ΑΞΙΑ / <br> TOTAL VALUE </th> </tr> </tbody> </table> <div style='height: 15cm; border-left: 1px dotted black; border-right: 1px dotted black;'> <table class=' dynamictable ' style=' margin-right: 4px;'> <colgroup> <col width=' max-content '> <col width=' auto '> <col width=' auto '> <col width=' 100 '> <col width=' 100 '> <col width=' 99 '> </colgroup> <tbody> <tr> <td colspan=' 3 '>BANSKO</td> <td class='textCenterandtop' rowspan='4' style='border-left:1px solid black ;'>342,00</td> <td class='textCenterandtop ' rowspan='4' style='border-left:1px solid black ;'>0,00</td> <td class='textCenterandtop ' rowspan='4' style='border-left:1px solid black ;'>342,00</td> </tr> <tr> <td>29/2/2020</td> <td>Κράτηση / Reservation:4835</td> <td>Άτομα / Pax: 2</td> </tr> <tr> <td>HELLAS TRAVEL</td> </tr> <tr> <td colspan=' 3 '> DIMITRIADOU EVANGELIA BALTAS VASILEIOS </td> </tr> </tbody> </table> </div> <div id='footertable'> <table style='border: 1px solid black; '> <colgroup> <col width='auto'> <col width='max-content '> <col width='auto'> <col width='8px'> <col width='165px'> <col width='60px'> </colgroup> <thead> <tr> <th colspan='3'>ΠΑΡΑΤΗΡΗΣΕΙΣ / NOTES</th> <th style='background: transparent; border: 0px;'></th> <th colspan='2'></th> </tr> </thead> <tbody> <tr> <td style='font-size: 10px;' colspan='3'>Ο ΦΠΑ ΣΥΜΠΕΡΙΛΑΜΒΑΝΕΤΑΙ ΒΑΣΕΙ ΤΟΥ ΑΡΘΡΟΥ 43N2859/2000 ΕΙΔΙΚΟ ΚΑΘΕΣΤΩΣ ΤΑΞΙΔΙΩΤΙΚΩΝ ΠΡΑΚΤΟΡΕΙΩΝ </td> <td rowspan='5' style='border-right: 1px solid black; border-left: 1px solid black;'> </td> <td style='border-right: 1px solid black;'>ΑΞΙΑ / ΝΕΤ</td> <td style='text-align: right;'>342,00</td> </tr> <tr> <td style='font-size: 10px;' colspan='3'>VAT IS INCLUDED ACCORDING TO THE 43L2859/2000 REGUALATION REGARDING TRAVEL AGENCIES</td> <td style='border-right: 1px solid black;'> ΦΠΑ / VAT </td> <td style='text-align: right;'>0,00</td> </tr> <tr> <td colspan='3'> </td> <td style='border-right: 1px solid black;'> ΕΚΠΤΩΣΗ / DISCOUNT </td> <td style='text-align: right;'>38,00</td> </tr> <tr> <td colspan='3' style='border-bottom: 1px solid black;'> <b> ΣΤΕΛΕΧΟΣ ΠΕΛΑΤΗ/CUSTOMER </b> </td> <td class='textbold' rowspan='2' style='border-top: 1px solid black; border-right: 1px solid black;background-color: lightgray; vertical-align: top;'> ΤΕΛΙΚΗ ΑΞΙΑ / TOTAL </td> <td class='textbold' rowspan='2' style='text-align: right; border-top: 1px solid black; background-color: lightgray; vertical-align: top;'> 342,00<br />EURO </td> </tr> <tr> <td> Εθνική:<br/>Πειραιώς:<br/>Eurobank: </td> <td> IBAN GR4201102120000021200713903<br/>IBAN GR6301712150006215147358413<br/>IBAN GR29026069000004100201406798 </td> <td > Αρ.Λογαριασµού: 212/007139-03<br> Αρ.Λογαριασµού: 6215147358413<br> Αρ.Λογαριασµού: 0026.0690.41.0201406798<br> </td> </tr> </tbody> </table> </div> </div> </body> </html>";

            // // wbSample.NavigateToString(html);
            //// Grid x = InvoceGrid;
            // PrintDialog printDlg = new PrintDialog();
            // PageMediaSize pageSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            // Size Size = new Size(printDlg.PrintableAreaWidth - 30, printDlg.PrintableAreaHeight - 30);
            //// InvoceGrid.Measure(Size);
            // //InvoceGrid.Arrange(new Rect(15, 15, Size.Width, Size.Height));

            // printDlg.PrintTicket.PageMediaSize = pageSize;
            // if (printDlg.ShowDialog() == true)
            // {
            //     printDlg.PrintVisual(printArea, "asdf");
            // }

            //comboBox1.Loaded += delegate
            //{
            //    var textBox1 = (TextBox)comboBox1.Template. FindName(
            //      "PART_EditableTextBox", comboBox1);
            //    var popup1 = (Popup)comboBox1.Template.FindName("PART_Popup", comboBox1);

            //    textBox1.TextChanged += delegate
            //    {
            //        popup1.IsOpen = true;
            //        comboBox1.Items.Filter += a =>
            //        {
            //            return textBox1.Text.Length == 0 || (a.ToString().ToUpper().
            //              Contains(textBox1.Text.
            //              Substring(0, Math.Max(textBox1.SelectionStart, 1)).ToUpper()));
            //        };
            //    };
            //};
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is InvoicesManagement_ViewModel u && u.CanSaveChanges())
            {
                MessageBoxResult result = MessageBox.Show("Υπάρχουν μη αποθηκευμένες αλλαγές, θέλετε σίγουρα να κλείσετε?", "Προσοχή", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                else
                    u.RevertChanges();
            }
            Helpers.StaticResources.Close(this);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is InvoicesManagement_ViewModel u)
            {
                u.UpdateCities();
            }
        }

       

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is InvoicesManagement_ViewModel u)
            {
                await u.PrintInvoice(printArea);
            }

            //    MemoryStream lMemoryStream = new MemoryStream();
            //Package package = Package.Open(lMemoryStream, FileMode.Create);
            //XpsDocument doc = new XpsDocument(package);
            //XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
            //writer.Write(printArea);
            //doc.Close();
            //package.Close();
            //string filename = @"\test.pdf";
            //string path = GetPath(filename,"\\Invoices");

            //PdfSharp.Xps.XpsModel.XpsDocument pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
            //PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, path, 0);
            //PrintDialog printDlg = new PrintDialog();
            //PageMediaSize pageSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            ////Size Size = new Size(printDlg.PrintableAreaWidth - 30, printDlg.PrintableAreaHeight - 30);

            //printDlg.PrintTicket.PageMediaSize = pageSize;
            //if (printDlg.ShowDialog() == false)
            //{
            //    printDlg.PrintVisual(printArea, "Παραστατικό");
            //}
        }
    }
}