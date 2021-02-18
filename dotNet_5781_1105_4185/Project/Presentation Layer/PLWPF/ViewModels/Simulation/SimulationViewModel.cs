using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PL
{
    class SimulationViewModel : BaseViewModel
    {
        private TimeSpan _simulationTime;
        public TimeSpan SimulationTime
        {
            get => _simulationTime;
            set
            {
                _simulationTime = value;

                // Remove days if set
                if (_simulationTime.Days > 0)
                {
                    _simulationTime = _simulationTime.Add(TimeSpan.FromDays(-_simulationTime.Days));
                }

                OnPropertyChanged(nameof(SimulationTime));
            }
        }

        private int _simulationRate;
        public int SimulationRate
        {
            get => _simulationRate;
            set
            {
                _simulationRate = value;
                if (_simulationRate <= 0) _simulationRate = 1;

                OnPropertyChanged(nameof(SimulationRate));
            }
        }

        private ObservableCollection<BO.Station> _stations;
        public ObservableCollection<BO.Station> Stations
        {
            get => _stations;
            private set
            {
                _stations = value;
                OnPropertyChanged(nameof(Stations));
            }
        }

        private ObservableCollection<BO.LineTiming> _arrivingLines;
        public ObservableCollection<BO.LineTiming> ArrivingLines
        {
            get => _arrivingLines;
            private set
            {
                _arrivingLines = value;
                OnPropertyChanged(nameof(ArrivingLines));
            }
        }

        public ObservableCollection<BusLineViewModel> PassingBusLines { get; private set; }

        private BO.Station _selectedStation;
        public BO.Station SelectedStation
        {
            get => _selectedStation;
            set
            {
                _selectedStation = value;
                OnPropertyChanged(nameof(SelectedStation));

                ArrivingLines.Clear();

                BlWork(bl => bl.SetStationPanel(_selectedStation?.Code, _UpdateArriving));

                _ = GetPassingBusLinesFromBL();
            }
        }

        private BackgroundWorker _timeSimulation;
        public bool Started => _timeSimulation.IsBusy;

        public RelayCommand StartSimulation { get; }
        public RelayCommand StopSimulation { get; }

        public SimulationViewModel()
        {
            Stations = new ObservableCollection<BO.Station>();
            PassingBusLines = new ObservableCollection<BusLineViewModel>();
            ArrivingLines = new ObservableCollection<BO.LineTiming>();

            _ = GetStationsFromBL();

            SimulationTime = DateTime.Now.TimeOfDay;
            SimulationRate = 1;

            _timeSimulation = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            _timeSimulation.DoWork += _TimeSimulationDoWork;
            _timeSimulation.ProgressChanged += _TimeSimulationProgressChanged;
            _timeSimulation.RunWorkerCompleted += _TimeSimulationRunWorkerCompleted;

            StartSimulation = new RelayCommand(obj => _Start(), obj => !Started);
            StopSimulation = new RelayCommand(obj => _Stop(), obj => Started);
        }

        private async Task GetStationsFromBL()
        {
            await Load(async () =>
            {
                Stations = new ObservableCollection<BO.Station>((IEnumerable<BO.Station>)
                    await BlWorkAsync(bl => bl.GetAllStationsWithoutAdjacents())
                );
            });
        }

        private async Task GetPassingBusLinesFromBL()
        {
            await Load(async () =>
            {
                var vms = from bl in (IEnumerable<BO.BusLine>)
                            await BlWorkAsync(bl => bl.GetLinesPassingTheStation(_selectedStation.Code))
                          select new BusLineViewModel(bl);
                PassingBusLines = new ObservableCollection<BusLineViewModel>(vms);

                OnPropertyChanged(nameof(PassingBusLines));
            });
        }

        private void _Start()
        {
            _timeSimulation.RunWorkerAsync();
            OnPropertyChanged(nameof(Started));
        }

        private void _Stop()
        {
            _timeSimulation.CancelAsync();
        }

        private void _UpdateTime(TimeSpan time)
        {
            _timeSimulation.ReportProgress(0, time);

            if (_timeSimulation.CancellationPending)
            {
                BlWork(bl => bl.StopSimulation());
            }
        }

        private void _UpdateArriving(IEnumerable<BO.LineTiming> lineTiming)
        {
            Context.Invoke(() =>
            {
                ArrivingLines = new ObservableCollection<BO.LineTiming>(lineTiming);
            });
        }

        private void _TimeSimulationDoWork(object sender, DoWorkEventArgs e)
        {
            BlWork(bl => bl.StartSimulation(SimulationTime, SimulationRate, _UpdateTime));
        }

        private void _TimeSimulationProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SimulationTime = (TimeSpan)e.UserState;
        }

        private void _TimeSimulationRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartSimulation.RaiseCanExecuteChanged();
            ArrivingLines.Clear();
            OnPropertyChanged(nameof(Started));
        }
    }
}
