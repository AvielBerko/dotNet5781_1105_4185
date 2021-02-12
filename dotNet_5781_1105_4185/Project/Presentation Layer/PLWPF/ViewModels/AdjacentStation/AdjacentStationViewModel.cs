using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class AdjacentStationViewModel : BaseViewModel
    {
        private BO.AdjacentStations _adjacents;
        public BO.AdjacentStations Adjacents
        {
            get => _adjacents;
            set
            {
                _adjacents = value;
                OnPropertyChanged(nameof(Adjacents));
            }
        }

        private int _fromStationCode;
        public BO.Station ToStation => Adjacents.GetOtherStation(_fromStationCode);

        public RelayCommand RemoveAdjacents { get; }

        public AdjacentStationViewModel(BO.AdjacentStations adjacent, int fromStationCode)
        {
            _fromStationCode = fromStationCode;
            Adjacents = adjacent;

            RemoveAdjacents = new RelayCommand(obj => _Remove());
        }
        private void _Remove()
        {
            OnRemove();
        }

        public delegate void RemoveAdjacentEventHandler(object sender);
        public event RemoveAdjacentEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
