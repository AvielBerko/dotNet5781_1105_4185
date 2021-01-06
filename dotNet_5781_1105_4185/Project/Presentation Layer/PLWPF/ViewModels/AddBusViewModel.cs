using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PL
{
	public class AddBusViewModel : BaseViewModel, IDataErrorInfo, IDialogHelper
	{
		BO.Bus bus;

		public int RegNum 
		{ 
			get => bus.Registration.Number;
			set
			{
				bus.Registration = new BO.Registration(value, bus.Registration.Date);
				OnPropertyChanged(nameof(RegNum));
			} 
		}
		public DateTime RegDate 
		{
			get => bus.Registration.Date;
			set
			{
				bus.Registration = new BO.Registration(bus.Registration.Number, value);
				OnPropertyChanged(nameof(RegDate));
			}
		}
		public int Kilometrage 
		{
			get => bus.Kilometrage;
			set
			{
				bus.Kilometrage = value;
				OnPropertyChanged(nameof(Kilometrage));
			}
		}
		public BO.BusTypes Type
		{
			get => bus.Type;
			set
			{
				bus.Type = value;
				OnPropertyChanged(nameof(Type));
			}
		}

		public RelayCommand Ok { get; }
		public RelayCommand Cancel { get; }

		public AddBusViewModel()
		{
			Ok = new RelayCommand(obj => _Ok(), obj => ValidateRegistration().IsValid);
			Cancel = new RelayCommand(obj => _Cancel());
		}

		private void _Ok()
		{
			BlWork(bl => bl.AddBus(bus));
			OnAddedBus(bus);
			OnRequestClose(true);
		}

		private void _Cancel()
		{
			OnRequestClose(false);
		}

		public event DialogService.RequestClose RequestClose;
		protected virtual void OnRequestClose(bool? result) => RequestClose?.Invoke(this, result);

		public delegate void AddedBusEventHandler(object sender, BO.Bus bus);
		public event AddedBusEventHandler AddedBus;
		protected virtual void OnAddedBus(BO.Bus bus) => AddedBus?.Invoke(this, bus);

		public string this[string columnName]
		{
			get
			{
				switch (columnName)
				{
					case nameof(RegNum):
						return ValidateRegistration().ErrorContent as string;
					case nameof(RegDate):
						return ValidateRegistration().ErrorContent as string;
					default:
						return null;
				}
			}
		}
		public string Error => throw new NotImplementedException();

		
		private ValidationResult ValidateRegistration()
		{
			try
			{
				BlWork(bl => bl.ValidateRegistration(bus.Registration));
				return ValidationResult.ValidResult;
			}
			catch (BO.BadBusRegistrationException ex)
			{
				return new ValidationResult(false, ex.Message);
			}
		}
	}
}
