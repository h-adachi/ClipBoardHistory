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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace ClipBoradHistory
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		ClipBoard cp;
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			cp = new ClipBoard(this, DataContext as ViewModel);
		}

		private void ButtonCopyText_Click(object sender, RoutedEventArgs e)
		{
			String text = (sender as Button)?.DataContext as String;
			if (String.IsNullOrEmpty(text)) return;

			var vm = DataContext as ViewModel;
			vm.Texts.Remove(text);

			Clipboard.SetText(text);
		}

		private void ButtonCopyImage_Click(object sender, RoutedEventArgs e)
		{
			var img = (sender as Button)?.DataContext as BitmapSource;
			if (img == null) return;

			var vm = DataContext as ViewModel;
			vm.Images.Remove(img);

			Clipboard.SetImage(img);
		}

		private void ButtonSaveImage_Click(object sender, RoutedEventArgs e)
		{
			var img = (sender as Button)?.DataContext as BitmapSource;
			if (img == null) return;

			var dlg = new SaveFileDialog();
			dlg.DefaultExt = "png";
			dlg.Filter = "png|*.png";
			if (dlg.ShowDialog() == false) return;

			using (var fs = new System.IO.FileStream(dlg.FileName, System.IO.FileMode.Create))
			{
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(img));
				encoder.Save(fs);
			}
		}

		private void ButtonRemoveImage_Click(object sender, RoutedEventArgs e)
		{
			var img = (sender as Button)?.DataContext as BitmapSource;
			if (img == null) return;

			var vm = DataContext as ViewModel;
			vm.Images.Remove(img);
		}
	}
}
