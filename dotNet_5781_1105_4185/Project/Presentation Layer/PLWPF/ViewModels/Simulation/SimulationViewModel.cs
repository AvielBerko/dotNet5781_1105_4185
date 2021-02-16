using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private bool _started = false;
        public bool Started
        {
            get => _started;
            set
            {
                _started = value;
                OnPropertyChanged(nameof(Started));
            }
        }

        public RelayCommand StartSimulation { get; }
        public RelayCommand StopSimulation { get; }

        public SimulationViewModel()
        {
            SimulationTime = DateTime.Now.TimeOfDay;
            SimulationRate = 1;

            StartSimulation = new RelayCommand(obj => _Start(), obj => !_started);
            StopSimulation = new RelayCommand(obj => _Stop(), obj => _started);
        }

        private void _Start()
        {
            Started = true;
        }

        private void _Stop()
        {
            Started = false;
        }
    }
}
