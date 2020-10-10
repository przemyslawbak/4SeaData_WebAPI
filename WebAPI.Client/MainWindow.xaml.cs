using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using WebAPI.Models;

namespace WebAPI.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static HttpClient _client = new HttpClient();

        private string _pauseEndpoint = "";
        private string _stopEndpoint = "";
        private string _statusEndpoint = "";
        private string _key = "";

        private readonly bool _isLocalApi = true; //production: false

        public MainWindow()
        {
            GetConfig();
            TimedMehodTrigger();
            InitializeComponent();
            DataContext = this;
        }

        private void GetConfig()
        {
            if (!_isLocalApi)
            {
                _key = ConfigurationManager.AppSettings["clientToken"].ToString();
                _pauseEndpoint = ConfigurationManager.AppSettings["pauseEndpoint"].ToString() + "?key=" + _key;
                _stopEndpoint = ConfigurationManager.AppSettings["stopEndpoint"].ToString() + "?key=" + _key;
                _statusEndpoint = ConfigurationManager.AppSettings["statusEndpoint"].ToString() + "?key=" + _key;
            }
            else
            {
                _key = ConfigurationManager.AppSettings["clientToken"].ToString();
                _pauseEndpoint = "http://localhost:44342/api/updates/pausing-updates" + "?key=" + _key;
                _stopEndpoint = "http://localhost:44342/api/api/updates/stop-updates" + "?key=" + _key;
                _statusEndpoint = "http://localhost:44342/api/updates/get-status" + "?key=" + _key;
            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastStartedTime;
        public DateTime LastStartedTime
        {
            get => _lastStartedTime;
            set
            {
                _lastStartedTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastCompletedTime;
        public DateTime LastCompletedTime
        {
            get => _lastCompletedTime;
            set
            {
                _lastCompletedTime = value;
                OnPropertyChanged();
            }
        }

        private DateTime _nextPlannedTime;
        public DateTime NextPlannedTime
        {
            get => _nextPlannedTime;
            set
            {
                _nextPlannedTime = value;
                OnPropertyChanged();
            }
        }

        private bool _isUpdatingPaused;
        public bool IsUpdatingPaused
        {
            get => _isUpdatingPaused;
            set
            {
                _isUpdatingPaused = value;
                OnPropertyChanged();
            }
        }

        private bool _isUpdatingInProgress;
        public bool IsUpdatingInProgress
        {
            get => _isUpdatingInProgress;
            set
            {
                _isUpdatingInProgress = value;
                OnPropertyChanged();
            }
        }

        private bool _updatingDatabase;
        public bool UpdatingDatabase
        {
            get => _updatingDatabase;
            set
            {
                _updatingDatabase = value;
                OnPropertyChanged();
            }
        }

        private bool _finalizing;
        public bool Finalizing
        {
            get => _finalizing;
            set
            {
                _finalizing = value;
                OnPropertyChanged();
            }
        }

        private int _failedResultsQuantity;
        public int FailedResultsQuantity
        {
            get => _failedResultsQuantity;
            set
            {
                _failedResultsQuantity = value;
                OnPropertyChanged();
            }
        }

        private int _totalResultsQuantity;
        public int TotalResultsQuantity
        {
            get => _totalResultsQuantity;
            set
            {
                _totalResultsQuantity = value;
                OnPropertyChanged();
            }
        }

        private int _returnedVesselsInCurrent;
        public int ReturnedVesselsInCurrent
        {
            get => _returnedVesselsInCurrent;
            set
            {
                _returnedVesselsInCurrent = value;
                OnPropertyChanged();
            }
        }

        private string _lastUpdatedVessel;
        public string LastUpdatedVessel
        {
            get => _lastUpdatedVessel;
            set
            {
                _lastUpdatedVessel = value;
                OnPropertyChanged();
            }
        }

        private float _memoryMegabytesUsage;
        public float MemoryMegabytesUsage
        {
            get => _memoryMegabytesUsage;
            set
            {
                _memoryMegabytesUsage = value;
                OnPropertyChanged();
            }
        }

        private int _missingLat;
        public int MissingLat
        {
            get => _missingLat;
            set
            {
                _missingLat = value;
                OnPropertyChanged();
            }
        }

        private int _missingLon;
        public int MissingLon
        {
            get => _missingLon;
            set
            {
                _missingLon = value;
                OnPropertyChanged();
            }
        }

        private int _missingDest;
        public int MissingDest
        {
            get => _missingDest;
            set
            {
                _missingDest = value;
                OnPropertyChanged();
            }
        }

        private int _missingDra;
        public int MissingDra
        {
            get => _missingDra;
            set
            {
                _missingDra = value;
                OnPropertyChanged();
            }
        }

        private int _missingSpeed;
        public int MissingSpeed
        {
            get => _missingSpeed;
            set
            {
                _missingSpeed = value;
                OnPropertyChanged();
            }
        }

        private int _missingCog;
        public int MissingCog
        {
            get => _missingCog;
            set
            {
                _missingCog = value;
                OnPropertyChanged();
            }
        }

        private int _missingTime;
        public int MissingTime
        {
            get => _missingTime;
            set
            {
                _missingTime = value;
                OnPropertyChanged();
            }
        }

        private int _missingEtas;
        public int MissingEtas
        {
            get => _missingEtas;
            set
            {
                _missingEtas = value;
                OnPropertyChanged();
            }
        }

        private int _missingStat;
        public int MissingStat
        {
            get => _missingStat;
            set
            {
                _missingStat = value;
                OnPropertyChanged();
            }
        }

        private int _missingAreas;
        public int MissingAreas
        {
            get => _missingAreas;
            set
            {
                _missingAreas = value;
                OnPropertyChanged();
            }
        }

        private async void TimedMehodTrigger()
        {
            await DispatcherTimer_ExecuteStatusUpdate();

            RunStatusUpdates();
        }

        private void RunStatusUpdates()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += async (s, e) => await DispatcherTimer_ExecuteStatusUpdate();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }

        private async Task DispatcherTimer_ExecuteStatusUpdate()
        {
            StatusModel status = new StatusModel();

            try
            {
                HttpResponseMessage response = await _client.GetAsync(_statusEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    status = JsonConvert.DeserializeObject<StatusModel>(jsonString);

                    UpdateProerties(status);
                }
                else
                {
                    Status = "ERROR";
                    //TODO log
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }

            //TODO: update props
        }

        private void UpdateProerties(StatusModel status)
        {
            LastStartedTime = status.LastStartedTime;
            LastCompletedTime = status.LastCompletedTime;
            NextPlannedTime = status.NextPlannedTime;
            IsUpdatingPaused = status.IsUpdatingPaused;
            IsUpdatingInProgress = status.IsUpdatingInProgress;
            Finalizing = status.Finalizing;
            FailedResultsQuantity = status.FailedResultsQuantity;
            TotalResultsQuantity = status.TotalResultsQuantity;
            ReturnedVesselsInCurrent = status.ReurnedVesselsInCurrent;
            LastUpdatedVessel = status.LastUpdatedVessel;
            MemoryMegabytesUsage = status.MemoryMegabytesUsage;
            UpdatingDatabase = status.UpdatingDatabase;

            Status = ResolveStatus();
        }

        private string ResolveStatus()
        {
            string status = "ERROR";

            if (Finalizing)
            {
                status = "Finalizing";
            }
            else if (UpdatingDatabase) //TODO: not displayed
            {
                status = "Updating DB";
            }
            else if (IsUpdatingPaused)
            {
                status = "Paused";
            }
            else if (IsUpdatingInProgress)
            {
                status = "Running";
            }
            else
            {
                status = "Stopped";
            }

            return status;
        }

        private async void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            //todo: implement
        }

        private async void BtnPauseContinue_Click(object sender, RoutedEventArgs e)
        {
            //todo: implement
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
