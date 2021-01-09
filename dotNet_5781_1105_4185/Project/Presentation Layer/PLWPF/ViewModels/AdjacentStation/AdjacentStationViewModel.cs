using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class AdjacentStationViewModel : BaseViewModel
    {
        private BO.AdjacentStations adjacent;
        public BO.AdjacentStations Adjacent
        {
            get => adjacent;
            set
            {
                adjacent = value;
                OnPropertyChanged(nameof(Adjacent));
            }
        }
        public RelayCommand RemoveAdjacents { get; }

        public AdjacentStationViewModel(BO.AdjacentStations adjacent)
        {
            Adjacent = adjacent;

            RemoveAdjacents = new RelayCommand(obj => _Remove());
        }
        private void _Remove()
		{
            BlWork(bl => bl.DeleteAdjacent(Adjacent)); 
            OnRemove();
        }

        public delegate void RemoveAdjacentEventHandler(object sender);
        public event RemoveAdjacentEventHandler Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);
    }
}
