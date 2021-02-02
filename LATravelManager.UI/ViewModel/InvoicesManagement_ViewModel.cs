using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace LATravelManager.UI.ViewModel
{
    public class InvoicesManagement_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors





        private string _PaymentType;


        public string PaymentType
        {
            get
            {
                return _PaymentType;
            }

            set
            {
                if (_PaymentType == value)
                {
                    return;
                }

                _PaymentType = value;
                RaisePropertyChanged();
            }
        }

        public InvoicesManagement_ViewModel(BasicDataManager basicDataManager, BookingWrapper booking = null,
            Personal_BookingWrapper personal = null, ThirdParty_Booking_Wrapper thirdParty = null)
        {
            this.basicDataManager = basicDataManager;
            this.repository = basicDataManager.Context;
            this.booking = booking;
            this.personal = personal;
            this.thirdParty = thirdParty;

            GetAllCompaniesCommand = new RelayCommand(async () => await GetAllCompaniesAsync(), true);
            CreateRecieptPreviewCommand = new RelayCommand(CreateRecieptPreview, CanCreatePreview);
            SaveChangesCommand = new RelayCommand(async () => { await repository.SaveAsync(); }, CanSaveChanges);

            SetPartner();
        }

        #endregion Constructors

        #region Fields

        private readonly BasicDataManager basicDataManager;
        private readonly BookingWrapper booking;

        private readonly Personal_BookingWrapper personal;

        private readonly GenericRepository repository;
        private readonly ThirdParty_Booking_Wrapper thirdParty;

        private ObservableCollection<City> _Cities;
        private ObservableCollection<Company> _Companies;

        private ObservableCollection<CompanyActivity> _CompanyActivities;

        private string _CompanyError;
        private ObservableCollection<Country> _Countries;
        private Partner _Partner;

        private Reciept _Reciept;
        private RecieptTypeEnum? _RecieptType;

        private Company _SelectedCompany;
        private RecieptSeries _SelectedSerie;
        private ObservableCollection<RecieptSeries> _Series;

        #endregion Fields

        #region Properties

        public ObservableCollection<City> Cities
        {
            get
            {
                return _Cities;
            }

            set
            {
                if (_Cities == value)
                {
                    return;
                }

                _Cities = value;
                RaisePropertyChanged();
            }
        }

        public CollectionView CitiesCV { get; set; }

        public ObservableCollection<Company> Companies
        {
            get
            {
                return _Companies;
            }

            set
            {
                if (_Companies == value)
                {
                    return;
                }

                _Companies = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<CompanyActivity> CompanyActivities
        {
            get
            {
                return _CompanyActivities;
            }

            set
            {
                if (_CompanyActivities == value)
                {
                    return;
                }

                _CompanyActivities = value;
                RaisePropertyChanged();
            }
        }

        public string CompanyError
        {
            get
            {
                return _CompanyError;
            }

            set
            {
                if (_CompanyError == value)
                {
                    return;
                }

                _CompanyError = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Country> Countries
        {
            get
            {
                return _Countries;
            }

            set
            {
                if (_Countries == value)
                {
                    return;
                }

                _Countries = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CreateRecieptPreviewCommand { get; set; }

        public RelayCommand GetAllCompaniesCommand { get; set; }

        public bool IsPartners => Partner != null;

        public Partner Partner
        {
            get
            {
                return _Partner;
            }

            set
            {
                if (_Partner == value)
                {
                    return;
                }

                _Partner = value;
                RaisePropertyChanged();
            }
        }

        public bool PrintEnabled => CanPrintReciept();

        public Reciept Reciept
        {
            get
            {
                return _Reciept;
            }

            set
            {
                if (_Reciept == value)
                {
                    return;
                }

                _Reciept = value;
                RaisePropertyChanged();
            }
        }

        public RecieptTypeEnum? RecieptType
        {
            get
            {
                return _RecieptType;
            }

            set
            {
                if (_RecieptType == value)
                {
                    return;
                }

                _RecieptType = value;
                SeriesCV.Refresh();
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveChangesCommand { get; set; }

        public Company SelectedCompany
        {
            get
            {
                return _SelectedCompany;
            }

            set
            {
                if (_SelectedCompany == value)
                {
                    return;
                }

                _SelectedCompany = value;
                if (Partner != null)
                {
                    Partner.CompanyInfo = value;
                }

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Partner));
            }
        }

        public RecieptSeries SelectedSerie
        {
            get
            {
                return _SelectedSerie;
            }

            set
            {
                if (_SelectedSerie == value)
                {
                    return;
                }

                _SelectedSerie = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<RecieptSeries> Series
        {
            get
            {
                return _Series;
            }

            set
            {
                if (_Series == value)
                {
                    return;
                }

                _Series = value;
                RaisePropertyChanged();
            }
        }

        public Visibility TaxDetailsVisibility { get; set; }

        public RelayCommand TestCommand { get; set; }

        #endregion Properties

        #region Methods

        public static string GetPath(string fileName, string folderpath, string fileExtension, bool hidden = false)
        {
            int i = 1;
            string resultPath;
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + folderpath;
            Directory.CreateDirectory(folder);
            DirectoryInfo di = Directory.CreateDirectory(folder);
            if (hidden)
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;

            resultPath = folder + fileName + fileExtension;
            while (File.Exists(resultPath))
            {
                i++;
                resultPath = folder + fileName + "(" + i + ")" + fileExtension;
            }
            return resultPath;
        }

        public bool CanPrintReciept()
        {
            return Reciept != null;
        }

        public bool CanSaveChanges()
        {
            return SelectedCompany != null && SelectedCompany.IsValidToPrint() && basicDataManager.HasChanges();
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            if (Partner != null && Partner.CompanyInfoId.HasValue && Partner.CompanyInfoId > 0)
            {
                Partner.CompanyInfo = await repository.GetByIdAsync<Company>(Partner.CompanyInfoId.Value);
                SelectedCompany = Partner.CompanyInfo;
                Companies = new ObservableCollection<Company>
                {
                    SelectedCompany
                };
            }
            CompanyActivities = new ObservableCollection<CompanyActivity>((await repository.GetAllAsync<CompanyActivity>()).OrderBy(a => a.Name));
            Cities = new ObservableCollection<City>(basicDataManager.Cities);
            CitiesCV = (CollectionView)CollectionViewSource.GetDefaultView(Cities);
            CitiesCV.Filter = CitiesFilter;

            Series = new ObservableCollection<RecieptSeries>((await repository.GetAllAsync<RecieptSeries>(s => !s.Disabled)).OrderBy(a => a.Letter));
            SeriesCV = (CollectionView)CollectionViewSource.GetDefaultView(Series);
            SeriesCV.Filter = SeriesFilter;

            Countries = basicDataManager.Countries;
            TaxDetailsVisibility = ShouldShowTaxDetails();
        }

        private bool SeriesFilter(object obj)
        {
            return RecieptType != null && obj is RecieptSeries s && s.RecieptType == RecieptType;
        }

        private CollectionView _SeriesCV;


        public CollectionView SeriesCV
        {
            get
            {
                return _SeriesCV;
            }

            set
            {
                if (_SeriesCV == value)
                {
                    return;
                }

                _SeriesCV = value;
                RaisePropertyChanged();
            }
        }

        public async Task PrintInvoice(Visual printArea)
        {
            using (var transaction = repository.Context.Database.BeginTransaction())
            {
                //try
                //{
                //    var standard = context.Standards.Add(new Standard() { StandardName = "1st Grade" });

                //    context.Students.Add(new Student()
                //    {
                //        FirstName = "Rama2",
                //        StandardId = standard.StandardId
                //    });
                //    context.SaveChanges();

                //    context.Courses.Add(new Course() { CourseName = "Computer Science" });
                //    context.SaveChanges();

                //    transaction.Commit();
                //}
                //catch (Exception ex)
                //{
                //    transaction.Rollback();
                //    Console.WriteLine("Error occurred.");
                //}

                string filename = @"\test";
                string subFolder = "\\" + GetSubFolder(Reciept);
                string tmpPath = GetPath(filename, "\\Invoices\\tmp", ".pdf", hidden: true);
                string finalPath = GetPath(filename, "\\Invoices" + subFolder, ".pdf");

                try
                {
                    MemoryStream lMemoryStream = new MemoryStream();
                    Package package = Package.Open(lMemoryStream, FileMode.Create);
                    XpsDocument doc = new XpsDocument(package);
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(printArea);
                    doc.Close();
                    package.Close();

                    using PdfSharp.Xps.XpsModel.XpsDocument pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
                    PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, tmpPath, 0);

                    File.Move(tmpPath, finalPath);

                    if (File.Exists(finalPath))
                    {
                        System.Diagnostics.Process.Start(finalPath);
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    //if fails rollback transaction, and delete any files created.

                    MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                }
            }
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public void RevertChanges()
        {
            repository.RollBack();
        }

        public Visibility ShouldShowTaxDetails()
        {
            return IsPartners ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void UpdateCities()
        {
            CitiesCV.Refresh();
        }

        private bool CanCreatePreview()
        {
            return (!IsPartners || (Partner.CompanyInfo != null)) && RecieptType != null && SelectedSerie != null && SelectedCompany.IsValidToPrint() && !string.IsNullOrEmpty(PaymentType);
        }

        //private bool CanCreateReciept()
        //{
        //    return RecieptType != null && SelectedSerie != null && SelectedCompany.IsValidToPrint();
        //}

        private bool CitiesFilter(object obj)
        {
            return SelectedCompany == null || (obj is City c && SelectedCompany != null && SelectedCompany.Country != null && c.Country.Id == SelectedCompany.Country.Id);
        }

        private void CreateRecieptPreview()
        {
            if (CanCreatePreview())
            {
                Reciept = new Reciept { Company = Partner.CompanyInfo, RecieptType = RecieptType.Value, Series = SelectedSerie };
            }
            RaisePropertyChanged(nameof(PrintEnabled));
        }

        private async Task GetAllCompaniesAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Companies = new ObservableCollection<Company>((await repository.GetAllAsync<Company>(c => !c.Disabled)).OrderBy(c => c.CompanyName));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private string GetSubFolder(Reciept reciept)
        {
            switch (reciept.RecieptType)
            {
                case RecieptTypeEnum.ServiceReciept:
                    return "ΑΠΥ";

                case RecieptTypeEnum.ServiceInvoice:
                    return "ΤΠΥ";

                case RecieptTypeEnum.AirTicketsReciept:
                    return "ΑΠΕ";

                case RecieptTypeEnum.FerryTicketsReciept:
                    return "ΑΠΑ";

                case RecieptTypeEnum.CancelationInvoice:
                    return "Ακυρωμένα";

                case RecieptTypeEnum.CreditInvoice:
                    return "Πιστωτικά";
            }

            return "Error";
        }

        private void SetPartner()
        {
            if (booking != null && booking.IsPartners && booking.Partner != null)
            {
                Partner = booking.Partner;
            }
            else if (personal != null && personal.IsPartners && personal.Partner != null)
            {
                Partner = personal.Partner;
            }
            else if (thirdParty != null && thirdParty.IsPartners && thirdParty.BuyerPartner != null)
            {
                Partner = thirdParty.BuyerPartner;
            }
        }

        #endregion Methods
    }
}