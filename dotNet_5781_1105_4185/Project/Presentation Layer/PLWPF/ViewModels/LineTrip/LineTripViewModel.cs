using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL
{
    public class LineTripViewModel : BaseViewModel, IDataErrorInfo
    {
        public BO.LineTrip LineTrip { get; }

        public TimeSpan StartTime
        {
            get => LineTrip.StartTime;
            set
            {
                LineTrip.StartTime = value;

                // Remove days if set
                if (StartTime.Days > 0)
                {
                    LineTrip.StartTime = StartTime.Add(TimeSpan.FromDays(-StartTime.Days));
                }

                if (!OneTime)
                {
                    var finishTime = FinishTime ?? TimeSpan.Zero;
                    if (finishTime <= StartTime)
                    {
                        finishTime = finishTime.Add(TimeSpan.FromDays(1));
                        FinishTime = finishTime;
                    }
                }

                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(LineTrip));
            }
        }

        public TimeSpan? FinishTime
        {
            get => LineTrip.Frequencied?.FinishTime;
            set
            {
                // Checking if is in next day
                var finishTime = value ?? TimeSpan.Zero;

                // Remove days if set
                if (finishTime.Days > 0)
                {
                    finishTime = finishTime.Add(TimeSpan.FromDays(-finishTime.Days));
                }

                if (finishTime <= StartTime)
                {
                    finishTime = finishTime.Add(TimeSpan.FromDays(1));
                }

                LineTrip.Frequencied = new BO.FrequnciedTrip
                {
                    FinishTime = finishTime,
                    Frequency = LineTrip.Frequencied?.Frequency ?? TimeSpan.Zero,
                };

                OnPropertyChanged(nameof(FinishTime));
                OnPropertyChanged(nameof(LineTrip));
            }
        }

        public TimeSpan? Frequency
        {
            get => LineTrip.Frequencied?.Frequency;
            set
            {
                var freq = value ?? TimeSpan.Zero;

                // Remove days if set
                if (freq.Days > 0)
                {
                    freq = freq.Add(TimeSpan.FromDays(-freq.Days));
                }

                LineTrip.Frequencied = new BO.FrequnciedTrip
                {
                    Frequency = freq,
                    FinishTime = LineTrip.Frequencied?.FinishTime ?? TimeSpan.Zero,
                };

                OnPropertyChanged(nameof(Frequency));
                OnPropertyChanged(nameof(LineTrip));
            }
        }

        public bool OneTime
        {
            get => LineTrip.Frequencied == null;
            set
            {
                if (value)
                {
                    LineTrip.Frequencied = null;
                }
                else
                {
                    // Also sets the frequency
                    FinishTime = TimeSpan.Zero;
                }

                OnPropertyChanged(nameof(OneTime));
                OnPropertyChanged(nameof(FinishTime));
                OnPropertyChanged(nameof(Frequency));
                OnPropertyChanged(nameof(LineTrip));
            }
        }

        private bool _isColliding;
        public bool IsColliding
        {
            get => _isColliding;
            set
            {
                _isColliding = value;
                OnPropertyChanged(nameof(IsColliding));
            }
        }

        public RelayCommand Remove { get; }

        public LineTripViewModel(BO.LineTrip trip)
        {
            LineTrip = trip;
            Remove = new RelayCommand(obj => _Remove());
        }

        private void _Remove()
        {
            OnRemove();
        }

        public event Action<LineTripViewModel> RemoveLineTrip;
        protected virtual void OnRemove() => RemoveLineTrip?.Invoke(this);

        public string this[string columnName]
        {
            get
            {
                switch(columnName)
                {
                    case nameof(Frequency):
                        return ValidateFrequency().ErrorContent as string;
                    case nameof(StartTime):
                    case nameof(FinishTime):
                    case nameof(OneTime):
                    default:
                        return null;
                }
            }
        }

        private ValidationResult ValidateFrequency()
        {
            if (OneTime) return ValidationResult.ValidResult;

            try
            {
                BlWork(bl => bl.ValidateLineTripFrequency(LineTrip.Frequencied?.Frequency ?? TimeSpan.Zero));
                return ValidationResult.ValidResult;
            }
            catch (BO.BadLineTripFrequencyException ex)
            {
                return new ValidationResult(false, ex.Message);
            }
        }

        public string Error => throw new NotImplementedException();
    }
}
