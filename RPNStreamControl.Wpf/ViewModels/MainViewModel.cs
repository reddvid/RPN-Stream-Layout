using CommunityToolkit.Mvvm.ComponentModel;
using RPNStreamControl.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPNStreamControl.Wpf.ViewModels
{
	public class MainViewModel : ObservableObject
	{
		public MainViewModel()
		{
			// Load Selected Station
			if (string.IsNullOrWhiteSpace(Properties.Settings.Default.SelectedStation))
			{
				SelectedStation = StationSelectors[0];
				OnPropertyChanged(nameof(SelectedStation));
			} else
			{
				SelectedStation = StationSelectors.FirstOrDefault(x => x.Name.Equals(Properties.Settings.Default.SelectedStation));
				OnPropertyChanged(nameof(SelectedStation));
			}
		}

		public ObservableCollection<StationSelector> StationSelectors { get; set; } = new ObservableCollection<StationSelector>()
		{
			new StationSelector("DZRL Batac", "batac"),
			new StationSelector("DZBS Baguio", "baguio"),
			new StationSelector("DZKI Iriga", "iriga"),
			new StationSelector("DYKB Bacolod", "bacolod"),
			new StationSelector("DYKC Cebu", "cebu"),
			new StationSelector("DXKS Surigao", "surigao"),
			new StationSelector("DXKO Cagayan de Oro", "cdo"),
			new StationSelector("DXKD Dipolog", "dipolog"),
			new StationSelector("DXKT Davao", "davao"),
			new StationSelector("DXDX General Santos", "gensan"),
			new StationSelector("DXKP Pagadian", "pagadian"),
			new StationSelector("DXXX Zamboanga", "zamboanga"),
		};

		private StationSelector _selectedStation;

		public StationSelector SelectedStation
		{
			get => _selectedStation;
			set
			{
				SetProperty(ref _selectedStation, value);
				if (_selectedStation != null)
				{
					// Set Image Path


					// Save Settings
					Properties.Settings.Default.SelectedStation = SelectedStation.Name;
					Properties.Settings.Default.Save();
				}
			}
		}

		



	}
}
