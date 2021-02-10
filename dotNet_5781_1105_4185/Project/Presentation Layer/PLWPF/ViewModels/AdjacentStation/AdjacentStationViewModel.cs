using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class AdjacentStationViewModel : BaseViewModel
    {
        private BO.AdjacentStation adjacent;
        public BO.AdjacentStation Adjacent
        {
            get => adjacent;
            set
            {
                adjacent = value;
                OnPropertyChanged(nameof(Adjacent));
            }
        }
        public RelayCommand RemoveAdjacents { get; }

        public AdjacentStationViewModel(BO.AdjacentStation adjacent)
        {
            Adjacent = adjacent;

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
