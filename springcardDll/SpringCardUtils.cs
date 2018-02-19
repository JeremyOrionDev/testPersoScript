using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace SpringCardUtils
{	
	public class SystemConsole
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AllocConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FreeConsole();

		[DllImport("kernel32", SetLastError = true)]
		public static extern bool AttachConsole(int dwProcessId);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true)]
		public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
		
		private static bool Visible = false;
		
		public static void Show()
		{
			if (!Visible)
			{
				IntPtr ptr = GetForegroundWindow();
				int  u;
				GetWindowThreadProcessId(ptr, out u);
				Process process = Process.GetProcessById(u);
				if (process.ProcessName.Equals("cmd"))    //Is the uppermost window a cmd process?
				{
					AttachConsole(process.Id);
					Console.WriteLine("\n" + Application.ProductName + " v."+Application.ProductVersion+"\n");
				}
				else
				{
					AllocConsole();
					Console.WriteLine("\n" + Application.ProductName + " v."+Application.ProductVersion+"\n");
				}
				Visible = true;
			}
		}
		
		public static void Hide()
		{
			if (Visible)
				FreeConsole();
			Visible = false;
		}
		
		public static void Verbose(string s)
		{
			if (Visible)
				Console.WriteLine(s);
		}
	}
	
	class ImageConvert
	{
		public static Icon ImageToIcon(Image img, int size, bool keepAspectRatio)
		{
			Bitmap square = new Bitmap(size, size); // create new bitmap
			Graphics g = Graphics.FromImage(square); // allow drawing to it
			
			int x, y, w, h; // dimensions for new image
			
			if(!keepAspectRatio || img.Height == img.Width) {
				// just fill the square
				x = y = 0; // set x and y to 0
				w = h = size; // set width and height to size
			} else {
				// work out the aspect ratio
				float r = (float)img.Width / (float)img.Height;
				
				// set dimensions accordingly to fit inside size^2 square
				if(r > 1) { // w is bigger, so divide h by r
					w = size;
					h = (int)((float)size / r);
					x = 0; y = (size - h) / 2; // center the image
				} else { // h is bigger, so multiply w by r
					w = (int)((float)size * r);
					h = size;
					y = 0; x = (size - w) / 2; // center the image
				}
			}
			
			// make the image shrink nicely by using HighQualityBicubic mode
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.DrawImage(img, x, y, w, h); // draw image with specified dimensions
			g.Flush(); // make sure all drawing operations complete before we get the icon
			
			// following line would work directly on any image, but then
			// it wouldn't look as nice.
			return Icon.FromHandle(square.GetHicon());
		}
	}
}