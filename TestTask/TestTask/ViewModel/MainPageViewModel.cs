using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using TestTask.Model;
using TestTask.View;
using Xamarin.Forms;

namespace TestTask.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _status;
        private double _progress;
        private string _dateText;
        private string _productName;
        private string _quantity;
        private string _validationMessage;
        private Product _productSelected;
        private DateTime? _date;
        private bool _isBusy;
        private bool _validDataComplete;
        private ReadWriteProductsInFile _readWriteToFile;
        private string _filePath;
        private string[] _dateFormats = new[] { "dd.MM.yyyy", "dd.MM.yy", "ddMMyy" };
        

        public ObservableCollection<Product> Items { get; set; } = new ObservableCollection<Product>();

        public DateTime? Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                    ValidateFields();
                }
            }
        }

        public bool ValidDataComplete
        {
            get => _validDataComplete;
            set
            {
                _validDataComplete = value;
                OnPropertyChanged(nameof(ValidDataComplete));
            }
            
        }
        
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        public string DateText
        {
            get => _dateText;
            set
            {
                if (_dateText != value)
                {
                    if (DateTime.TryParseExact(value,
                    _dateFormats,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        _dateText = value;
                        Date = parsedDate;
                    }
                    else
                    {
                        _dateText = value;
                        Date = null;
                    }
                    OnPropertyChanged(nameof(DateText));
                    ValidateFields();
                }
            }
        }

        public string ProductName
        {
            get => _productName;
            set
            {
                if (_productName != value)
                {
                    _productName = value;
                    OnPropertyChanged(nameof(ProductName));
                    ValidateFields();
                }
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    ValidateFields();
                }
            }
        }

        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                _validationMessage = value;
                OnPropertyChanged(nameof(ValidationMessage));
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(IsNotBusy)); 
            }
        }

        public bool IsNotBusy => !IsBusy;

        public ICommand AddProductCommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand ViewContentCommand { get; }
        public ICommand GoToMainPageCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }

        public MainPageViewModel()
        {
            AddProductCommand = new Command(async () => await AddProduct());
            ViewContentCommand = new Command(async () => await GoToContentOverview());
            ClearCommand = new Command(ClearForm);
            GoToMainPageCommand = new Command(GoToMainPage);
            EditCommand = new Command<Product>(EditItem);
            DeleteCommand = new Command<Product>(DeleteItem);
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "tempData.json");
            _readWriteToFile = new ReadWriteProductsInFile(_filePath, Items);
            InitializeItemsCollection();
        }

        private void InitializeItemsCollection()
        {
            if (File.Exists(_filePath))
            {
                Items = _readWriteToFile.ReadFromFile();
            }
        }

        private async void GoToMainPage()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task GoToContentOverview()
        {
            IsBusy = true;
            await SimulateRequest(1000, "Загрузка значений из базы данных...");
            await Application.Current.MainPage.Navigation.PushAsync(new ContentOverview(this));
            Status = string.Empty;
            ClearForm();
            IsBusy = false;
        }

        private async Task SimulateRequest(int miliseconds, string message)
        {
            Status = message;
            Progress = 0; 

            for (int i = 1; i <= 10; i++)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Progress = i / 10.0;
                });

                await Task.Delay(200); 
            }

            Progress = 0;
            Status = "";
        }


        public void EditItem(Product item)
        {
            if (item == null) return;
            Status = $"\n Редактирование {item.ProductName}";
            _productSelected = item;
            ProductName = item.ProductName;
            Quantity = item.QuantityOfGoods.ToString("F3", CultureInfo.GetCultureInfo("ru-RU"));
            DateText = item.ProductionDate.ToString("dd.MM.yyyy");
            Date = item.ProductionDate;
            OnPropertyChanged(nameof(Date));

            Application.Current.MainPage.Navigation.PopAsync();
        }

        public void DeleteItem(Product item)
        {
            if(item == null) return;
            else Items.Remove(item);
            _readWriteToFile.WriteToFile();
        }

        private async Task AddProduct()
        {
            if (!string.IsNullOrEmpty(ValidationMessage)) return;
            else if (_productSelected != null)
            {
                _productSelected.ProductionDate = (DateTime)Date;
                _productSelected.ProductName = ProductName;
                _productSelected.QuantityOfGoods = float.Parse(Quantity, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"));
                _productSelected = null;
            }
            else
            {
                if (!string.IsNullOrEmpty(ProductName) | Date != null | !string.IsNullOrEmpty(Quantity) | _productSelected == null)
                {
                    Items.Add(new Product
                    {
                        ProductionDate = (DateTime)Date,
                        ProductName = ProductName,
                        QuantityOfGoods = float.Parse(Quantity, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"))
                    });
                }
            }
            IsBusy = true;
            await SimulateRequest(3000, "Запись в базу данных...");
            _readWriteToFile.WriteToFile();
            ClearForm();
            IsBusy = false;
        }

        public void ClearForm()
        {
            DateText = "";
            ProductName = "";
            Quantity = "";
            ValidationMessage = "";
            Status = "";
            _productSelected = null;
            ValidDataComplete = false;
            ValidateFields();
        }

        private void ValidateFields()
        {
            ValidDataComplete = false;
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                ValidationMessage = "Наименование не может быть пустым";
                return;
            }
            if (ProductName.Length < 5)
            {
                ValidationMessage = "Наименование должно содержать минимум 5 символов";
                return;
            }
            if (string.IsNullOrWhiteSpace(Quantity)) 
            {
                ValidationMessage = "Количество не может быть равно нулю.";
                return;
            }
            if (Date == null) 
            {
                ValidationMessage = "Некорректная дата. Используйте формат ДД.ММ.ГГ, ДД.ММ.ГГГГ, ДДММГГ";
                return;
            }
            ValidDataComplete = true;
            ValidationMessage = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}