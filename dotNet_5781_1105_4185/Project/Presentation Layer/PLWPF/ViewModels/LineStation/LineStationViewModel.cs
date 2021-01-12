using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class LineStationViewModel : BaseViewModel
    {
        public BO.LineStation LineStation { get; }

        private bool isLast;
        public bool IsLast
        {
            get => isLast;
            set
            {
                isLast = value;
                OnPropertyChanged(nameof(IsLast));
            }
        }

        public RelayCommand Remove { get; }
        public RelayCommand InverseAdjacents { get; }
        public RelayCommand Insert { get; }

        public LineStationViewModel(BO.LineStation lineStation)
        {
            LineStation = lineStation;

            Remove = new RelayCommand(obj => OnRemove());
            InverseAdjacents = new RelayCommand(obj => _InverseAdjacents());
            Insert = new RelayCommand(obj => OnInsertStation());
        }

        private void _InverseAdjacents()
        {
        }

        public delegate void MyEventHandler(LineStationViewModel sender);

        public event MyEventHandler InsertStation;
        protected virtual void OnInsertStation() => InsertStation?.Invoke(this);
        public event MyEventHandler RemoveLineStation;
        protected virtual void OnRemove() => RemoveLineStation?.Invoke(this);
    }
}
