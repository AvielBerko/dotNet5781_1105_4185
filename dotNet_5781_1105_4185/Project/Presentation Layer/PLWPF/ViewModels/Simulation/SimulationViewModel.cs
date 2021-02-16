using System;
using System.Collections.Generic;
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


        private BackgroundWorker _simulation;
        public bool Started => _simulation.IsBusy;

        public RelayCommand StartSimulation { get; }
        public RelayCommand StopSimulation { get; }

        public SimulationViewModel()
        {
            SimulationTime = DateTime.Now.TimeOfDay;
            SimulationRate = 1;

            _simulation = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true,
            };
            _simulation.DoWork += _SimulationDoWork;
            _simulation.ProgressChanged += _SimulationProgressChanged;
            _simulation.RunWorkerCompleted += _SimulationRunWorkerCompleted;

            StartSimulation = new RelayCommand(obj => _Start(), obj => !Started);
            StopSimulation = new RelayCommand(obj => _Stop(), obj => Started);
        }

        private void _Start()
        {
            _simulation.RunWorkerAsync();
            OnPropertyChanged(nameof(Started));
        }

        private void _Stop()
        {
            _simulation.CancelAsync();
        }

        private void _SimulationDoWork(object sender, DoWorkEventArgs e)
        {
            BlWork(bl => bl.StartSimulation(SimulationTime, SimulationRate, _UpdateTime));
        }

        private void _UpdateTime(TimeSpan time)
        {
            _simulation.ReportProgress(0, time);

            if (_simulation.CancellationPending)
            {
                BlWork(bl => bl.StopSimulation());
            }
        }

        private void _SimulationProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            SimulationTime = (TimeSpan)e.UserState;
        }

        private void _SimulationRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            StartSimulation.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(Started));
        }
    }
}
