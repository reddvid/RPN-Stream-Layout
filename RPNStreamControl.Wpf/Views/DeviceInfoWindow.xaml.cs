using Hardware.Info;
using SpeedTest;
using SpeedTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Windows.ApplicationModel.Activation;

namespace RPNStreamControl.Wpf.Views
{
	/// <summary>
	/// Interaction logic for DeviceInfoWindow.xaml
	/// </summary>
	public partial class DeviceInfoWindow : Window
	{
		static readonly IHardwareInfo hardwareInfo = new HardwareInfo();

		public DeviceInfoWindow()
		{
			InitializeComponent();

			Loaded += DeviceInfoWindow_Loaded;

			Closing += DeviceInfoWindow_Closing;
		}

		private void DeviceInfoWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Visibility = Visibility.Hidden;
		}

		private async void DeviceInfoWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await RefreshDetailsAsync();
		}

		private async Task RefreshDetailsAsync()
		{
			var publicIP = new System.Net.WebClient().DownloadString("https://api.ipify.org");
			Title += $" {publicIP}";

			loading.IsActive = true;
			loading.Visibility = Visibility.Visible;
			loadingGrid.Visibility = Visibility.Visible;

			var task = Task.Run(() =>
			{
				hardwareInfo.RefreshCPUList();
				hardwareInfo.RefreshMotherboardList();
				hardwareInfo.RefreshMemoryList();
				hardwareInfo.RefreshBIOSList();
				hardwareInfo.RefreshDriveList();
				hardwareInfo.RefreshOperatingSystem();
				hardwareInfo.RefreshVideoControllerList();
			});

			await task.ContinueWith(t =>
			{
				var details = new List<DeviceDetials>
				{
					new DeviceDetials("Public IP", publicIP),
					new DeviceDetials("CPU", hardwareInfo.CpuList.FirstOrDefault()!.Name),
					new DeviceDetials("Motherboard", hardwareInfo.MotherboardList.FirstOrDefault()!.Product),
					new DeviceDetials("BIOS", GetBios(hardwareInfo.BiosList)),
					new DeviceDetials("Memory", GetMemory(hardwareInfo.MemoryList)),
					new DeviceDetials("Storage", GetStorage(hardwareInfo.DriveList)),
					new DeviceDetials("Video", GetGpu(hardwareInfo.VideoControllerList)),
					new DeviceDetials("OS", $"{hardwareInfo.OperatingSystem.Name} {hardwareInfo.OperatingSystem.Version}"),
					new DeviceDetials($"Speed test {DateTime.Now.ToString("f")}", GetSpeedTest())
				};

				ShowData(details);
			});
		}

		private string GetSpeedTest()
		{
			var speedTestClient = new SpeedTestClient();
			var settings = speedTestClient.GetSettings();
			var server = settings.Servers[0];
			var latency = speedTestClient.TestServerLatency(server);
			var download = speedTestClient.TestDownloadSpeed(server);
			var upload = speedTestClient.TestUploadSpeed(server);

			return $"{server.Host} {server.Name}, {server.Country} {latency}ms {download / 1000,0:N2}Mbps {upload / 1000,0:N2}Mbps";
		}

		private string GetBios(List<BIOS> biosList)
		{
			string bios = string.Empty;

			foreach (BIOS b in biosList)
			{
				bios += $"{b.Manufacturer} {b.Name} {b.Version} {b.SoftwareElementID}\n";
			}

			return bios.TrimEnd('\n');
		}

		private string GetGpu(List<VideoController> videoControllerList)
		{
			string gpu = string.Empty;

			foreach (VideoController vc in videoControllerList)
			{
				gpu += $"{vc.Name} {vc.AdapterRAM / Math.Pow(1024, 3),0:N0}GB {vc.DriverVersion}\n";
			}

			return gpu.TrimEnd('\n');
		}

		private string GetStorage(List<Drive> driveList)
		{
			string drive = string.Empty;

			foreach (Drive dr in driveList)
			{
				drive += $"{dr.Model} {dr.Manufacturer} {dr.Size / Math.Pow(1024, 3),0:N0}GB\n";
			}

			return drive.TrimEnd('\n');
		}

		private string GetMemory(List<Memory> memoryList)
		{
			string memory = string.Empty;

			foreach (Memory mem in memoryList)
			{
				memory += $"{mem.Manufacturer} {mem.PartNumber.Trim()} {mem.Capacity / Math.Pow(1024, 3)}GB {mem.Speed}MHz\n";
			}

			return memory.TrimEnd('\n');
		}

		private void ShowData(List<DeviceDetials> details)
		{
			this.Dispatcher.Invoke(() =>
			{
				main.Text = string.Empty;

				foreach (var d in details)
				{
					main.AppendText($"{d.Type} - {d.Detail}{Environment.NewLine}");
				}

				loading.IsActive = false;
				loading.Visibility = Visibility.Collapsed;
				loadingGrid.Visibility = Visibility.Collapsed;
			});
		}

		private void Copy_Clicked(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(main.Text);
		}

		private async void Refresh_Clicked(object sender, RoutedEventArgs e)
		{
			await RefreshDetailsAsync();
		}

		public record struct DeviceDetials(string Type, string Detail);
	}
}
