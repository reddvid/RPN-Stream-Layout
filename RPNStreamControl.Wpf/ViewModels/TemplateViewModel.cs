using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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


		private Visibility _isLive;

		public Visibility IsLive
		{
			get => _isLive;
			set => SetProperty(ref _isLive, value);
		}


		private string _title;

		public string Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		private string _scroll;

		public string Scroll
		{
			get => _scroll;
			set => SetProperty(ref _scroll, value);
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

		private string _imagePath;

		public string ImagePath
		{
			get => _imagePath;
			set => SetProperty(ref _imagePath, value);
		}

	}
}
