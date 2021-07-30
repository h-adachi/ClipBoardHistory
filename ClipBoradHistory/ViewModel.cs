using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ClipBoradHistory
{
	class ViewModel
	{
		public ObservableCollection<String> Texts { get; set; } = new ObservableCollection<String>();
		public ObservableCollection<BitmapSource> Images { get; set; } = new ObservableCollection<BitmapSource>();
	}
}
