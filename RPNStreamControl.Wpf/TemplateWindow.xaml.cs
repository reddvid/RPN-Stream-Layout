using System;
using System.Collections.Generic;
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

		public TemplateWindow()
		{
			InitializeComponent();
		}

		private void TextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			string path = System.IO.Path.Combine(FILES_DIRECTORY, FILENAMES[3]);

			LoadText(path, ScrollText);
		}

		private void LoadText(string path, TextBlock textbox)
		{
			if (!File.Exists(path)) return;

			// Read and Load saved text
			textbox.Text = File.ReadAllText(path);
		}

		private void Canvas_Loaded(object sender, RoutedEventArgs e)
		{
			DoubleAnimation doubleAnimation = new DoubleAnimation();
			doubleAnimation.From = -ScrollText.ActualWidth;
			doubleAnimation.To = canMain.ActualWidth;
			doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
			doubleAnimation.Duration = new Duration(TimeSpan.Parse("0:0:10"));
			ScrollText.BeginAnimation(Canvas.RightProperty, doubleAnimation);
		}
	}
}
