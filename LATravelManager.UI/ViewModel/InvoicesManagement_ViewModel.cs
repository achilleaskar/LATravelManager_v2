using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using LATravelManager.Model.Pricing.Invoices;
using LATravelManager.Model.Services;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LATravelManager.UI.ViewModel
{
    public class InvoicesManagement_ViewModel : MyViewModelBaseAsync
    {
        #region Constructors

        public InvoicesManagement_ViewModel(BasicDataManager basicDataManager, BookingWrapper booking = null,
            Personal_BookingWrapper personal = null, ThirdParty_Booking_Wrapper thirdParty = null, object parameter = null)
        {
            this.basicDataManager = basicDataManager;
            this.repository = basicDataManager.Context;
            this.booking = booking;
            this.personal = personal;
            this.thirdParty = thirdParty;
            this.parameter = parameter;
            GetAllCompaniesCommand = new RelayCommand(async () => await GetAllCompaniesAsync(), true);
            CreateRecieptPreviewCommand = new RelayCommand(CreateRecieptPreview, CanCreatePreview);
            SaveChangesCommand = new RelayCommand(async () => { await repository.SaveAsync(); }, CanSaveChanges);

            SetPartner();
        }

        #endregion Constructors

        #region Fields

        private readonly BasicDataManager basicDataManager;
        private readonly BookingWrapper booking;
        private readonly object parameter;
        private readonly Personal_BookingWrapper personal;
        private readonly GenericRepository repository;
        private readonly ThirdParty_Booking_Wrapper thirdParty;
        private bool _CanChangeSerie;
        private bool _ChangesAfterPreviewCreated = true;
        private ObservableCollection<CompanyActivity> _CompanyActivities;
        private string _CompanyError;
        private ObservableCollection<CustomerWrapper> _Customers;
        private bool _IsNotProforma;

        private bool _IsProforma;

        private Partner _Partner;

        private string _PaymentType;

        private Reciept _Reciept;

        private DateTime _RecieptDate;

        private RecieptTypeEnum? _RecieptType;

        private Company _SelectedCompany;

        private CustomerWrapper _SelectedCustomer;

        private RecieptSeries _SelectedSerie;

        private ObservableCollection<RecieptSeries> _Series;

        private CollectionView _SeriesCV;

        private decimal _ServiceAmmount;

        #endregion Fields

        #region Properties

        public bool CanChangeSerie
        {
            get
            {
                return _CanChangeSerie;
            }

            set
            {
                if (_CanChangeSerie == value)
                {
                    return;
                }

                _CanChangeSerie = value;
                RaisePropertyChanged();
            }
        }

        public bool ChangesAfterPreviewCreated
        {
            get
            {
                return _ChangesAfterPreviewCreated;
            }

            set
            {
                if (_ChangesAfterPreviewCreated == value)
                {
                    return;
                }

                _ChangesAfterPreviewCreated = value;
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

        //public CollectionView CitiesCV { get; set; }
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

        public RelayCommand CreateRecieptPreviewCommand { get; set; }

        //        _Countries = value;
        //        RaisePropertyChanged();
        //    }
        //}
        public ObservableCollection<CustomerWrapper> Customers
        {
            get
            {
                return _Customers;
            }

            set
            {
                if (_Customers == value)
                {
                    return;
                }

                _Customers = value;
                RaisePropertyChanged();
            }
        }

        //    set
        //    {
        //        if (_Countries == value)
        //        {
        //            return;
        //        }
        public RelayCommand GetAllCompaniesCommand { get; set; }

        public bool IsNotProforma
        {
            get
            {
                return _IsNotProforma;
            }

            set
            {
                if (_IsNotProforma == value)
                {
                    return;
                }

                _IsNotProforma = value;
                RaisePropertyChanged();
            }
        }
        //        _Cities = value;
        //        RaisePropertyChanged();
        //    }
        //}
        //public ObservableCollection<Country> Countries
        //{
        //    get
        //    {
        //        return _Countries;
        //    }
        public bool IsPartners => Partner != null;

        public bool IsProforma
        {
            get
            {
                return _IsProforma;
            }

            set
            {
                if (_IsProforma == value)
                {
                    return;
                }

                _IsProforma = value;
                RaisePropertyChanged();
            }
        }
        //    set
        //    {
        //        if (_Cities == value)
        //        {
        //            return;
        //        }
        public DateTime MaxDate { get; set; } = DateTime.Today;

        //public ObservableCollection<City> Cities
        //{
        //    get
        //    {
        //        return _Cities;
        //    }
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
                ChangesAfterPreviewCreated = true;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(PrintEnabled));
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

        public DateTime RecieptDate
        {
            get
            {
                return _RecieptDate;
            }

            set
            {
                if (_RecieptDate == value)
                {
                    return;
                }

                _RecieptDate = value;
                ChangesAfterPreviewCreated = true;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(PrintEnabled));
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
                if (SeriesCV != null)
                {
                    SeriesCV.Refresh();
                    if (SeriesCV.Count == 1)
                    {
                        SelectedSerie = (RecieptSeries)SeriesCV.GetItemAt(0);
                        CanChangeSerie = false;
                    }
                    else
                    {
                        CanChangeSerie = true;
                    }
                }
                IsProforma = value == RecieptTypeEnum.Proforma;
                IsNotProforma = !IsProforma;
                ChangesAfterPreviewCreated = true;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(PrintEnabled));
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
                if (Partner != null && !Partner.Person)
                {
                    Partner.CompanyInfo = value;
                }

                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Partner));
            }
        }

        public CustomerWrapper SelectedCustomer
        {
            get
            {
                return _SelectedCustomer;
            }

            set
            {
                if (_SelectedCustomer == value)
                {
                    return;
                }

                _SelectedCustomer = value;
                ChangesAfterPreviewCreated = true;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(PrintEnabled));
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
                ChangesAfterPreviewCreated = true;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(PrintEnabled));
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

        //internal void UpdateCities()
        //{
        //    CitiesCV.Refresh();
        //}
        public decimal ServiceAmmount
        {
            get
            {
                return _ServiceAmmount;
            }

            set
            {
                if (_ServiceAmmount == value)
                {
                    return;
                }

                _ServiceAmmount = value;
                RaisePropertyChanged();
            }
        }

        public bool ServiceAmmountVisible => parameter is Service;

        public bool ShowCustomers => !IsPartners || (Partner != null && Partner.Person);

        public bool ShowPartner => IsPartners && (Partner == null || !Partner.Person);

        public Visibility TaxDetailsVisibility { get; set; }

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
            return Reciept != null && !ChangesAfterPreviewCreated;
        }

        public bool CanSaveChanges()
        {
            return SelectedCompany != null && SelectedCompany.IsValidToPrint() && basicDataManager.HasChanges();
        }

        public override async Task LoadAsync(int id = 0, MyViewModelBaseAsync previousViewModel = null, MyViewModelBase parent = null)
        {
            RecieptDate = DateTime.Today;
            if (Partner != null && Partner.CompanyInfoId.HasValue && Partner.CompanyInfoId > 0 && !Partner.Person)
            {
                //Partner.CompanyInfo = await repository.GetByIdAsync<Company>(Partner.CompanyInfoId.Value);
                SelectedCompany = await repository.GetByIdAsync<Company>(Partner.CompanyInfoId.Value);
                //Companies = new ObservableCollection<Company>
                //{
                //    SelectedCompany
                //};
            }
            else
            {
                if (booking != null)
                {
                    Customers = booking.Customers;
                }
                else if (personal != null)
                {
                    Customers = personal.CustomerWrappers;
                }
                else
                {
                    throw new NotImplementedException("Μη ολοκληρωμένη λειτουργία");
                }
            }
            //CompanyActivities = new ObservableCollection<CompanyActivity>((await repository.GetAllAsync<CompanyActivity>()).OrderBy(a => a.Name));
            //Cities = new ObservableCollection<City>(basicDataManager.Cities);
            //CitiesCV = (CollectionView)CollectionViewSource.GetDefaultView(Cities);
            //CitiesCV.Filter = CitiesFilter;

            Series = new ObservableCollection<RecieptSeries>((await repository.GetAllAsync<RecieptSeries>(s => !s.Disabled && s.AgencyId == StaticResources.User.BaseLocation)).OrderBy(a => a.Letter));
            SeriesCV = (CollectionView)CollectionViewSource.GetDefaultView(Series);
            SeriesCV.Filter = SeriesFilter;

            //Countries = basicDataManager.Countries;
            TaxDetailsVisibility = ShouldShowTaxDetails();
            SelectProperRecieptType();
        }

        public async Task PrintInvoice(Border printArea)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            string filename = @"\" + GetFileName(Reciept);
            Reciept.RecieptDescription = GetRecieptDescription(Reciept);
            string subFolder = "\\" + GetSubFolder(Reciept);
            string tmpPath = GetPath(filename, "\\Invoices\\tmp", ".pdf", hidden: true);
            string finalPath = GetPath(filename, "\\Invoices" + subFolder, ".pdf");

            using var transaction = repository.Context.Database.BeginTransaction();
            try
            {
                await repository.Context.Entry(SelectedSerie).ReloadAsync();
                SelectedSerie.CurrentNumber++;
                Reciept.RecieptNumber = SelectedSerie.CurrentNumber;
                await repository.SaveAsync();

                if (printArea != null && printArea.FindName("RecieptNumber") is TextBlock tb)
                {
                    tb.Text = SelectedSerie.CurrentNumber.ToString();
                    printArea.UpdateLayout();
                }
                else
                {
                    throw new NullReferenceException("Σφάλμα κατα την εκτύπωση του τιμολογίου. Δοκιμάστε ξανα.");
                }
                Reciept.Company = null;
                MemoryStream lMemoryStream = new MemoryStream();
                Package package = Package.Open(lMemoryStream, FileMode.Create);
                XpsDocument doc = new XpsDocument(package);
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);
                writer.Write(printArea);
                doc.Close();
                package.Close();

                using PdfSharp.Xps.XpsModel.XpsDocument pdfXpsDoc = PdfSharp.Xps.XpsModel.XpsDocument.Open(lMemoryStream);
                PdfSharp.Xps.XpsConverter.Convert(pdfXpsDoc, tmpPath, 0);
                SelectedSerie.LastPrint = RecieptDate.Date;
                await repository.SaveAsync();

                using (FileStream fs = new FileStream(tmpPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    FileInfo file = new FileInfo(tmpPath);
                    if (file.Exists)
                    {
                        Reciept.Content = File.ReadAllBytes(tmpPath);
                        Reciept.FileName = file.Name;
                    }
                }
                Reciept.Date = DateTime.Now;
                repository.Add(Reciept);
                //if (booking != null)
                //{
                //    booking.Reciept = true;
                //}
                //else if (personal != null)
                //{
                //    personal.Reciept = true;
                //}
                await repository.SaveAsync();
                transaction.Commit();

                File.Move(tmpPath, finalPath);

                if (File.Exists(finalPath))
                {
                    System.Diagnostics.Process.Start(finalPath);
                }
            }
            catch (Exception ex)
            {
                //if fails rollback transaction, and delete any files created.
                transaction.Rollback();
                if (ex is DbUpdateConcurrencyException)
                    MessengerInstance.Send(new ShowExceptionMessage_Message("Η αρίθμηση έχει αλλάξει. Παρακαλώ δοκιμάστε ξανά."));
                else
                    MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
                if (File.Exists(tmpPath))
                {
                    File.Delete(tmpPath);
                }
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException("Μη ολοκληρωμένη λειτουργία");
        }

        public void RevertChanges()
        {
            repository.RollBack();
        }

        public Visibility ShouldShowTaxDetails()
        {
            return IsPartners ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool CanCreatePreview()
        {
            if (IsPartners && Partner != null && !Partner.Person)
            {
                if (Partner.CompanyInfo == null || !Partner.CompanyInfo.IsValidToPrint())
                {
                    return false;
                }
            }
            else
            {
                if (SelectedCustomer == null)
                {
                    return false;
                }
            }

            if (RecieptType == null || SelectedSerie == null || string.IsNullOrEmpty(PaymentType))
            {
                return false;
            }
            if (parameter is Service)
            {
                return ServiceAmmount > 0;
            }
            return true;
        }

        private void CreateRecieptPreview()
        {
            if ((!IsPartners || (Partner != null && Partner.Person)) && SelectedCustomer != null)
            {
                SelectedCompany = new Company
                {
                    CompanyName = SelectedCustomer.FullName,
                    Id = SelectedCustomer.Id,
                    Name = SelectedCustomer.Surename
                };
            }

            Reciept = new Reciept { Company = SelectedCompany, RecieptType = RecieptType.Value, Series = SelectedSerie, Date = RecieptDate };
            if (booking != null)
            {
                Reciept = new Reciept { Company = SelectedCompany, RecieptType = RecieptType.Value, Series = SelectedSerie, Date = RecieptDate, BookingId = booking.Model.Id };

                RecieptItem item = new RecieptItem
                {
                    Amount = booking.FullPrice,
                    Dates = booking.DatesFull,
                    Discount = booking.IsPartners ? booking.FullPrice - booking.NetPrice : 0,
                    FinalAmount = booking.IsPartners ? booking.NetPrice : booking.FullPrice,
                    Description = booking.GetPacketDescriptionForReciept().TrimEnd('.'),
                    Names = booking.Names,
                    Pax = booking.Customers.Count,
                    ReservationId = booking.Id
                };

                if (parameter != null && parameter is Payment p)
                {
                    item.Amount = p.Amount;
                    item.Discount = 0;
                    item.FinalAmount = item.Amount - item.Discount;
                }
                Reciept.RecieptItems.Add(item);

                foreach (var Ritem in Reciept.RecieptItems)
                {
                    Reciept.Total += Ritem.Amount;
                    Reciept.Discount += Ritem.Discount;
                }
                Reciept.FinalAmount = Reciept.Total - Reciept.Discount;
            }
            else if (personal != null)
            {
                var reservation = new ReservationWrapper { Id = personal.Id, PersonalModel = personal, CustomersList = personal.Customers.ToList(), CreatedDate = personal.CreatedDate };
                Reciept = new Reciept { Company = SelectedCompany, RecieptType = RecieptType.Value, Series = SelectedSerie, Date = RecieptDate, Personal_BookingId = personal.Model.Id };

                RecieptItem item = null;

                if (parameter != null)
                {
                    if (parameter is Payment p)
                    {
                        item = new RecieptItem
                        {
                            Amount = p.Amount,
                            Dates = reservation.DatesWithYear,
                            Description = personal.GetPacketDescription().TrimEnd('.'),
                            Names = reservation.Names,
                            Pax = personal.CustomerWrappers.Count,
                            ReservationId = personal.Id,
                            Discount = 0,
                            FinalAmount = p.Amount
                        };
                    }
                    else if (parameter is Service s)
                    {
                        item = new RecieptItem
                        {
                            Amount = ServiceAmmount,
                            Dates = s.GetDates(),
                            Description = s.GetDescription(),
                            Names = reservation.Names,
                            Pax = personal.CustomerWrappers.Count,
                            ReservationId = s.Id,
                            Discount = 0,
                            FinalAmount = ServiceAmmount
                        };
                    }
                }
                if (item == null)
                {
                    item = new RecieptItem
                    {
                        Amount = personal.FullPrice,
                        Dates = reservation.DatesWithYear,
                        Description = personal.GetPacketDescription().TrimEnd('.'),
                        Names = reservation.Names,
                        Pax = personal.CustomerWrappers.Count,
                        ReservationId = personal.Id,
                        Discount = 0,
                        FinalAmount = personal.FullPrice
                    };
                }
                Reciept.RecieptItems.Add(item);

                foreach (var Ritem in Reciept.RecieptItems)
                {
                    Reciept.Total += Ritem.Amount;
                    Reciept.Discount += Ritem.Discount;
                }
                Reciept.FinalAmount = Reciept.Total - Reciept.Discount;
            }
            else if (thirdParty != null)
            {
                throw new NotImplementedException("Μη ολοκληρωμένη λειτουργία");
            }

            if (RecieptType==RecieptTypeEnum.CreditInvoice)
            {
                foreach (var item in Reciept.RecieptItems)
                {
                    item.FinalAmount*=-1;
                }

                Reciept.FinalAmount*=-1;
            }

            ChangesAfterPreviewCreated = false;
            RaisePropertyChanged(nameof(PrintEnabled));
        }

        private async Task GetAllCompaniesAsync()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            //Companies = new ObservableCollection<Company>((await repository.GetAllAsync<Company>(c => !c.Disabled)).OrderBy(c => c.CompanyName));
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private string GetFileName(Reciept reciept)
        {
            var sb = new StringBuilder();
            if (reciept.Company.CompanyName.Length > 20)
                sb.Append(reciept.Company.CompanyName.Substring(0, 20));
            else
                sb.Append(reciept.Company.CompanyName);
            sb.Append(reciept.Dates);

            return ReplaceForbidenChars(sb.ToString());
        }

        private string GetRecieptDescription(Reciept reciept)
        {
            if (reciept.RecieptItems?.Count > 0)
                return reciept.RecieptItems[0].Description;
            return "";
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

        //private bool CitiesFilter(object obj)
        //{
        //    return SelectedCompany == null || (obj is City c && SelectedCompany != null && SelectedCompany.Country != null && c.Country.Id == SelectedCompany.Country.Id);
        //}
        private string ReplaceForbidenChars(string v)
        {
            return string.Join("_", v.Split(Path.GetInvalidFileNameChars()));
        }

        private void SelectProperRecieptType()
        {
            if (parameter is PlaneService || personal != null && personal.Services.All(p => p is PlaneService))
            {
                RecieptType = RecieptTypeEnum.AirTicketsReciept;
            }
        }
        private bool SeriesFilter(object obj)
        {
            return RecieptType != null && obj is RecieptSeries s && s.RecieptType == RecieptType;
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