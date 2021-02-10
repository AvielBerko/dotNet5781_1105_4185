using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL
{
    public class BusLineViewModel : BaseViewModel
    {
        private BO.BusLine busLine;
        public BO.BusLine BusLine
        {
            get => busLine;
            set
            {
                busLine = value;
                OnPropertyChanged(nameof(BusLine));
            }
        }
        public string StartName
        {
            get
            {
                try
                {
                    return busLine.Route.First().Station.Name;
                }
                catch
                {
                    return "Not Set";
                }
            }
        }
        public string EndName
        {
            get
            {
                try
                {
                    return busLine.Route.Last().Station.Name;
                }
                catch
                {
                    return "Not Set";
                }
            }
        }
        public bool HasFullRoute => (bool)BlWork(bl => bl.BusLineHasFullRoute(busLine.ID));

        public RelayCommand RemoveBusLine { get; }
        public RelayCommand UpdateBusLine { get; }
        public RelayCommand DuplicateBusLine { get; }

        public BusLineViewModel(BO.BusLine busLine)
        {
            BusLine = busLine;

            RemoveBusLine = new RelayCommand(async obj => await _Remove());
            DuplicateBusLine = new RelayCommand(async obj => await _Duplicate());
            UpdateBusLine = new RelayCommand(obj => _Update());
        }

        private void _Update()
        {
            var vm = new AddUpdateBusLineViewModel(BusLine.ID);
            if (DialogService.ShowAddUpdateBusLineDialog(vm) == DialogResult.Ok)
            {
                OnUpdate();
            }
        }

        private async Task _Duplicate()
        {
            await Load(async () =>
            {
                var duplicated = (BO.BusLine)await BlWorkAsync(bl => bl.DuplicateBusLine(busLine.ID));
                OnDupliate(duplicated);
            });
        }

        private async Task _Remove()
        {
            await Load(async () =>
            {
                await BlWorkAsync(bl => bl.DeleteBusLine(busLine.ID));
                OnRemove();
            });
        }

        public event Action<object, BO.BusLine> Duplicate;
        protected virtual void OnDupliate(BO.BusLine duplicated) => Duplicate?.Invoke(this, duplicated);

        public event Action<object> Remove;
        protected virtual void OnRemove() => Remove?.Invoke(this);

        public event Action<object> Update;
        protected virtual void OnUpdate() => Update?.Invoke(this);
    }
}
