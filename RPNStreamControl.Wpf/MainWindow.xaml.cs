using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Globalization;
using RPNStreamControl.Wpf.ViewModels;

namespace RPNStreamControl.Wpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		readonly string FILES_DIRECTORY = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), "StreamFiles");
		readonly string[] FILENAMES = new string[4] { "datetime.txt", "title.txt", "subtitle.txt", "scroll.txt" };

		public MainViewModel ViewModel { get; } = new MainViewModel();
		public TemplateViewModel TemplateViewModel { get; } = new TemplateViewModel();

		public MainWindow()
		{
			InitializeComponent();

			Loaded += MainWindow_Loaded;

			DataContext = ViewModel;
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			// Create .txt files to be read by XSplit/OBS
			CreateTxtFilesAsync();

			InitTimer();

			var preview = new TemplateWindow(TemplateViewModel);
			preview.Show();
		}

		private void InitTimer()
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[0]);
			if (!File.Exists(path)) return;

			// Initialize Timer
			var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
			dispatcherTimer.Tick += new EventHandler((s, e) => WriteToFile(path, DateTime.Now.ToString("MMM d, yyy hh:mm tt").ToUpperInvariant()));
			dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
			dispatcherTimer.Start();
		}

		private void WriteToFile(string path, string content)
		{
			using (FileStream fs = File.Create(path))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(content);
				fs.Write(info, 0, info.Length);
			}
		}

		/// <summary>
		/// Create datetime.txt, title.txt, subtitle.txt, scroll.txt
		/// </summary>
		/// <returns></returns>
		private void CreateTxtFilesAsync()
		{
			if (!Directory.Exists(FILES_DIRECTORY))
			{
				Directory.CreateDirectory(FILES_DIRECTORY);
			}

			foreach (var filename in FILENAMES)
			{
				string path = System.IO.Path.Combine(FILES_DIRECTORY, filename);
				if (!File.Exists(path))
				{
					File.Create(path);
				}
			}
		}

		private void ScrollTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[3]);

			LoadText(path, tbxScroll);
		}

		private void LoadText(string path, TextBox textbox)
		{
			if (!File.Exists(path)) return;

			// Read and Load saved text
			textbox.Text = File.ReadAllText(path);
		}

		private void UpdateFiles_Click(object sender, RoutedEventArgs e)
		{
			for (int i = 1; i < FILENAMES.Length; i++)
			{
				string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[i]);

				if (i == 1)
				{
					WriteToFile(path, tbxTitle.Text);
				}

				if (i == 2)
				{
					WriteToFile(path, tbxSubTitle.Text);
				}

				if (i == 3)
				{
					WriteToFile(path, tbxScroll.Text);
				}
			}
		}

		private void TitleTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[1]);

			LoadText(path, tbxTitle);
		}

		private void SubtitleTextBox_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[2]);

			LoadText(path, tbxSubTitle);
		}

		private async void GetForex_Click(object sender, RoutedEventArgs e)
		{
			string url = "https://api.exchangerate.host/latest?base=PHP&symbols=USD,EUR,JPY,GBP,HKD,CAD,SGD,AUD,SAR,AED,KRW,CNY";

			using HttpClient client = new();
			client.DefaultRequestHeaders.Accept.Clear();

			var json = await client.GetStringAsync(url);
			var data = JsonConvert.DeserializeObject<Forex>(json);

			StringBuilder sb = new();

			if (data == null) return;

			sb.Append("EXCHANGE RATES: 1 USD = ");
			sb.Append((1 / data.Rates.USD).ToString("C2", CultureInfo.CurrentCulture) + " | 1 GBP = ");
			sb.Append((1 / data.Rates.GBP).ToString("C2", CultureInfo.CurrentCulture) + " | 1 EUR = ");
			sb.Append((1 / data.Rates.EUR).ToString("C2", CultureInfo.CurrentCulture) + " | 1 SAR = ");
			sb.Append((1 / data.Rates.SAR).ToString("C2", CultureInfo.CurrentCulture) + " | 1 AED = ");
			sb.Append((1 / data.Rates.AED).ToString("C2", CultureInfo.CurrentCulture) + " | 1 CAD = ");
			sb.Append((1 / data.Rates.CAD).ToString("C2", CultureInfo.CurrentCulture) + " | 1 SGD = ");
			sb.Append((1 / data.Rates.SGD).ToString("C2", CultureInfo.CurrentCulture) + " | 1 KRW = ");
			sb.Append((1 / data.Rates.KRW).ToString("C2", CultureInfo.CurrentCulture) + " | 1 HKD = ");
			sb.Append((1 / data.Rates.HKD).ToString("C2", CultureInfo.CurrentCulture) + " | 1 CNY = ");
			sb.Append((1 / data.Rates.CNY).ToString("C2", CultureInfo.CurrentCulture) + " | 1 JPY = ");
			sb.Append((1 / data.Rates.JPY).ToString("C2", CultureInfo.CurrentCulture) + " | 1 AUD = ");
			sb.Append((1 / data.Rates.AUD).ToString("C2", CultureInfo.CurrentCulture) + " *** ");

			tbxForex.Text = sb.ToString();
		}

		// 
		public class Motd
		{
			[JsonProperty("msg")]
			public string Msg { get; set; }

			[JsonProperty("url")]
			public string Url { get; set; }
		}

		public class Rates
		{
			public List<double> RateList { get; private set; }

			public Rates()
			{
				RateList = new List<double>()
				{
					AED, AUD, CAD, CNY, EUR, GBP, HKD, JPY, KRW, SAR, SGD, USD
				};
			}

			[JsonProperty("AED")]
			public double AED { get; set; }

			[JsonProperty("AUD")]
			public double AUD { get; set; }

			[JsonProperty("CAD")]
			public double CAD { get; set; }

			[JsonProperty("CNY")]
			public double CNY { get; set; }

			[JsonProperty("EUR")]
			public double EUR { get; set; }

			[JsonProperty("GBP")]
			public double GBP { get; set; }

			[JsonProperty("HKD")]
			public double HKD { get; set; }

			[JsonProperty("JPY")]
			public double JPY { get; set; }

			[JsonProperty("KRW")]
			public double KRW { get; set; }

			[JsonProperty("SAR")]
			public double SAR { get; set; }

			[JsonProperty("SGD")]
			public double SGD { get; set; }

			[JsonProperty("USD")]
			public double USD { get; set; }
		}

		public class Forex
		{
			[JsonProperty("motd")]
			public Motd Motd { get; set; }

			[JsonProperty("success")]
			public bool Success { get; set; }

			[JsonProperty("base")]
			public string Base { get; set; }

			[JsonProperty("date")]
			public string Date { get; set; }

			[JsonProperty("rates")]
			public Rates Rates { get; set; }
		}

		private void Preview_Click(object sender, RoutedEventArgs e)
		{
			TemplateViewModel.Title = tbxTitle.Text;
			TemplateViewModel.Anchor = tbxSubTitle.Text;
		}
	}
}
