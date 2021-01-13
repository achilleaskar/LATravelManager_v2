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
        #region Fields

        private readonly BasicDataManager basicDataManager;
        private readonly BookingWrapper booking;

        private readonly Personal_BookingWrapper personal;

        private readonly GenericRepository repository;
        private readonly ThirdParty_Booking_Wrapper thirdParty;

        private ObservableCollection<Company> _Companies;

        private ObservableCollection<CompanyActivity> _CompanyActivities;

        private Partner _Partner;

        private RecieptTypeEnum? _RecieptType;

        private Company _SelectedCompany;

        #endregion Fields

        #region Constructors

        public InvoicesManagement_ViewModel(BasicDataManager basicDataManager, BookingWrapper booking = null,
            Personal_BookingWrapper personal = null, ThirdParty_Booking_Wrapper thirdParty = null)
        {
            this.basicDataManager = basicDataManager;
            this.repository = basicDataManager.Context;
            this.booking = booking;
            this.personal = personal;
            this.thirdParty = thirdParty;

            GetAllCompaniesCommand = new RelayCommand(async () => await GetAllCompaniesAsync(), true);
            CreateRecieptPreviewCommand = new RelayCommand(async () => await CreateRecieptPreview(), CanCreateReciept);
            SaveChangesCommand = new RelayCommand(async () => { await repository.SaveAsync(); }, RepHasChanges);

            SetPartner();
        }

        public bool PrintEnabled => CanPrintReciept();

        public bool CanPrintReciept()
        {
            return Reciept != null;
        }

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

        private bool CanCreateReciept()
        {
            return RecieptType != null;
        }


        public Visibility TaxDetailsVisibility { get; set; }

        public Visibility ShouldShowTaxDetails()
        {
            return IsPartners ? Visibility.Visible : Visibility.Collapsed;
        }

        private Reciept _Reciept;

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

        private async Task CreateRecieptPreview()
        {
            if (CanCreatePreview())
            {
                Reciept = new Reciept { Company = Partner.CompanyInfo, RecieptType = RecieptType.Value };

                //switch (RecieptType.Value)
                //{
                //    case RecieptTypeEnum.ServiceReciept:
                //        Reciept = new ServiceReciept { Company = Partner.CompanyInfo };
                //        break;
                //    case RecieptTypeEnum.ServiceInvoice:
                //        Reciept = new ServiceInvoice { Company = Partner.CompanyInfo };
                //        break;
                //    case RecieptTypeEnum.AirTicketsReciept:
                //        Reciept = new AirTicketsReciept { Company = Partner.CompanyInfo };
                //        break;
                //    case RecieptTypeEnum.FerryTicketsReciept:
                //        Reciept = new FerryTicketsReciept { Company = Partner.CompanyInfo };
                //        break;
                //    case RecieptTypeEnum.CancelationInvoice:
                //        Reciept = new CancelationInvoice { Company = Partner.CompanyInfo };
                //        break;
                //    case RecieptTypeEnum.CreditInvoice:
                //        Reciept = new CreditInvoice { Company = Partner.CompanyInfo };
                //        break;
                //    default:
                //        break;
                //}
            }
            RaisePropertyChanged(nameof(PrintEnabled));
        }

        private bool CanCreatePreview()
        {
            return !IsPartners || (Partner.CompanyInfo != null && Partner.CompanyInfo.IsValidToPrint());
        }

        public void RevertChanges()
        {
            repository.RollBack();
        }

        #endregion Constructors

        #region Properties

        private ObservableCollection<City> _Cities;

        private ObservableCollection<Country> _Countries;

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

        public RelayCommand GetAllCompaniesCommand { get; set; }
        public RelayCommand CreateRecieptPreviewCommand { get; set; }
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

        public RelayCommand TestCommand { get; set; }

        #endregion Properties

        #region Methods

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            if (Partner != null && Partner.CompanyInfoId.HasValue && Partner.CompanyInfoId > 0)
            {
                Partner.CompanyInfo = await repository.GetByIdAsync<Company>(Partner.CompanyInfoId.Value);
                SelectedCompany = Partner.CompanyInfo;
                Companies = new ObservableCollection<Company>();
                Companies.Add(SelectedCompany);
            }
            CompanyActivities = new ObservableCollection<CompanyActivity>((await repository.GetAllAsync<CompanyActivity>()).OrderBy(a => a.Name));
            Cities = new ObservableCollection<City>(basicDataManager.Cities);
            CitiesCV = (CollectionView)CollectionViewSource.GetDefaultView(Cities);
            CitiesCV.Filter = CitiesFilter;
            Countries = basicDataManager.Countries;
            TaxDetailsVisibility = ShouldShowTaxDetails();
        }

        private bool CitiesFilter(object obj)
        {
            return obj is City c && SelectedCompany != null && c.Country.Id == SelectedCompany.Country.Id;
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }

        public bool RepHasChanges()
        {
            return repository != null && repository.HasChanges();
        }

        private async Task GetAllCompaniesAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Companies = new ObservableCollection<Company>((await repository.GetAllAsync<Company>()).OrderBy(c => c.CompanyName));
            Mouse.OverrideCursor = Cursors.Arrow;
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

        public CollectionView CitiesCV { get; set; }

        internal void UpdateCities()
        {
            CitiesCV.Refresh();
        }

        #endregion Methods
    }
}