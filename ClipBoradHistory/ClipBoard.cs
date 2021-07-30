using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ClipBoradHistory
{
	class ClipBoard
	{
		const uint WM_CLIPBOARDUPDATE = 0x031D;

		[DllImport("user32.dll")]
		private static extern bool AddClipboardFormatListener(IntPtr hwnd);
		[DllImport("user32.dll")]
		private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

		private HwndSource mHwnd;
		private ViewModel mVm;
		public ClipBoard(Window window, ViewModel vm)
		{
			mVm = vm;
			mHwnd = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
			mHwnd.AddHook(WndProc);
			AddClipboardFormatListener(mHwnd.Handle);
		}

		~ClipBoard()
		{
			RemoveClipboardFormatListener(mHwnd.Handle);
			mHwnd.RemoveHook(WndProc);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if(msg == WM_CLIPBOARDUPDATE)
			{
				UpdateClipBoard();
				handled = true;
			}
			return IntPtr.Zero;
		}

		private void UpdateClipBoard()
		{
			UpdateText();
			UpdateImage();
		}

		private void UpdateText()
		{
			String text = Clipboard.GetText();

			if (String.IsNullOrEmpty(text)) return;
			if (mVm.Texts.Count > 0 && text == mVm.Texts[0]) return;

			mVm.Texts.Insert(0, text);
		}

		private void UpdateImage()
		{
			var source = Clipboard.GetImage();
			if (source == null) return;
			if (mVm.Images.Count > 0 && IsBitmapEqual(source, mVm.Images[0])) return;

			mVm.Images.Insert(0, source);
		}

		private bool IsBitmapEqual(BitmapSource bmp1, BitmapSource bmp2)
		{
			if (bmp1 == null || bmp2 == null) return false;
			//それぞれのハッシュ値を作成
			var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] h1 = md5.ComputeHash(MakeBitmapByte(bmp1));
			byte[] h2 = md5.ComputeHash(MakeBitmapByte(bmp2));
			md5.Clear();
			//ハッシュ値を比較
			return IsArrayEquals(h1, h2);
		}

		//2つのハッシュ値を比較
		private bool IsArrayEquals(byte[] h1, byte[] h2)
		{
			for (int i = 0; i < h1.Length; i++)
			{
				if (h1[i] != h2[i])
				{
					return false;
				}
			}
			return true;
		}

		//BitmapSourceをbyte配列に変換
		private byte[] MakeBitmapByte(BitmapSource bitmap)
		{
			int w = bitmap.PixelWidth;
			int h = bitmap.PixelHeight;
			int stride = w * bitmap.Format.BitsPerPixel / 8;
			byte[] pixels = new byte[h * stride];
			bitmap.CopyPixels(new Int32Rect(0, 0, w, h), pixels, stride, 0);
			return pixels;
		}
	}
}
