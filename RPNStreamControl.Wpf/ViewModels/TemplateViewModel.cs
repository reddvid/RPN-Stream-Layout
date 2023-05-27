using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RPNStreamControl.Wpf.ViewModels
{
	public class TemplateViewModel : ObservableObject
	{
		public DispatcherTimer _timer;

		public TemplateViewModel()
		{
			InitializeDateAndTime();
		}

		private void InitializeDateAndTime()
		{
			_timer = new DispatcherTimer(DispatcherPriority.Render);
			_timer.Interval = TimeSpan.FromSeconds(1);
			_timer.Tick += (sender, args) =>
			{
				DateTimeTitle = DateTime.Now.Second % 2 == 0
					? DateTime.Now.ToString("MMM dd, yyyy hh:mm tt").ToUpper()
					: DateTime.Now.ToString("MMM dd, yyyy hh mm tt").ToUpper();
			};
			_timer.Start();
		}

		private string _dateTimeTitle;

		public string DateTimeTitle
		{
			get => _dateTimeTitle;
			set => SetProperty(ref _dateTimeTitle, value);
		}


		private string _title;

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		private string _anchor;

		public string Anchor
		{
			get => _anchor;
			set => SetProperty(ref _anchor, value);
		}

		private string _hotlines;

		public string Hotlines
		{
			get => _hotlines;
			set => SetProperty(ref _hotlines, value);
		}
	}
}
