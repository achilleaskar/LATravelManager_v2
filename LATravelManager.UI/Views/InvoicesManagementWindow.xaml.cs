using LATravelManager.UI.ViewModel;
using System.Windows;

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

        //private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (DataContext is InvoicesManagement_ViewModel u)
        //    {
        //        u.UpdateCities();
        //    }
        //}

       

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is InvoicesManagement_ViewModel u)
            {
                await u.PrintInvoice(printArea,scrollviewer);
                
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