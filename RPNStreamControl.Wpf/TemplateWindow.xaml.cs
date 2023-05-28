using RPNStreamControl.Wpf.Utils;
using RPNStreamControl.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RPNStreamControl.Wpf
{
	/// <summary>
	/// Interaction logic for TemplateWindow.xaml
	/// </summary>
	public partial class TemplateWindow : Window
	{
		readonly string FILES_DIRECTORY = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), "StreamFiles");
		readonly string[] FILENAMES = new string[4] { "datetime.txt", "title.txt", "subtitle.txt", "scroll.txt" };

		public TemplateViewModel ViewModel { get; set; }

		public TemplateWindow(TemplateViewModel viewModel) : this()
		{
			InitializeComponent();

			ViewModel = viewModel;

			DataContext = ViewModel;

			Closing += TemplateWindow_Closing;
		}

		private void TemplateWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Visibility = Visibility.Hidden;
		}

		public TemplateWindow()
		{
			InitializeComponent();
		}

		private void TextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[3]);

			LoadText(path, ScrollText);

			CreateAnimation();
		}

		private void LoadText(string path, TextBlock textbox)
		{
			if (!File.Exists(path)) return;

			// Read and Load saved text
			textbox.Text = File.ReadAllText(path);
		}

		private void Canvas_Loaded(object sender, RoutedEventArgs e)
		{
			CreateAnimation();
		}

		private void CreateAnimation()
		{
			Debug.WriteLine("Start debugging Ticker canvass: ");
			Debug.WriteLine("ScrollText Actual Width: " + StackTicker.ActualWidth);
			Debug.WriteLine("Canvas Actual Width: " + canMain.ActualWidth);
			var newSpan = StackTicker.ActualWidth / 60;
			var newTimeSpan = TimeSpan.FromSeconds(newSpan);
			Debug.WriteLine("Timespan Seconds " + newTimeSpan);
			DoubleAnimation doubleAnimation = new DoubleAnimation();
			doubleAnimation.From = -StackTicker.ActualWidth;
			doubleAnimation.To = canMain.ActualWidth;
			doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
			doubleAnimation.Duration = new Duration(newTimeSpan);
			StackTicker.BeginAnimation(Canvas.RightProperty, doubleAnimation);
		}

		private void ScrollText_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			CreateAnimation();
		}

		private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			
		}
	}
}
