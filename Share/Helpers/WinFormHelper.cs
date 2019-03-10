 
using System;
using System.Windows.Forms;
using System.IO;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Share
{
	
	public static class WinFormHelper
	{
		public static bool UnregisterHotKey(NativeMethods.HWND hWnd, int id)
		{
			bool result = UnsafeNativeMethods.UnregisterHotKey(hWnd, id);
			int lastWin32Error = Marshal.GetLastWin32Error();

			if (!result) {
				//ThrowWin32ExceptionsIfError(lastWin32Error);
			}

			return result;
		}
		public static bool RegisterHotKey(NativeMethods.HWND hWnd, int id, int fsModifiers, int vk)
		{
			bool result = UnsafeNativeMethods.RegisterHotKey(hWnd, id, fsModifiers, vk);
			int lastWin32Error = Marshal.GetLastWin32Error();

			if (!result) {
				//ThrowWin32ExceptionsIfError(lastWin32Error);
			}

			return result;
		}
		internal static void ThrowWin32ExceptionsIfError(int errorCode)
		{
			switch (errorCode) {
				case 0:     //    0 ERROR_SUCCESS                   The operation completed successfully.
                    // The error code indicates that there is no error, so do not throw an exception.
					break;

				case 6:     //    6 ERROR_INVALID_HANDLE            The handle is invalid.
				case 1400:  // 1400 ERROR_INVALID_WINDOW_HANDLE     Invalid window handle.
				case 1401:  // 1401 ERROR_INVALID_MENU_HANDLE       Invalid menu handle.
				case 1402:  // 1402 ERROR_INVALID_CURSOR_HANDLE     Invalid cursor handle.
				case 1403:  // 1403 ERROR_INVALID_ACCEL_HANDLE      Invalid accelerator table handle.
				case 1404:  // 1404 ERROR_INVALID_HOOK_HANDLE       Invalid hook handle.
				case 1405:  // 1405 ERROR_INVALID_DWP_HANDLE        Invalid handle to a multiple-window position structure.
				case 1406:  // 1406 ERROR_TLW_WITH_WSCHILD          Cannot create a top-level child window.
				case 1407:  // 1407 ERROR_CANNOT_FIND_WND_CLASS     Cannot find window class.
				case 1408:  // 1408 ERROR_WINDOW_OF_OTHER_THREAD    Invalid window; it belongs to other thread.
					throw new Exception();

			// We're getting this in AMD64 when calling RealGetWindowClass; adding this code
			// to allow the DRTs to pass while we continue investigation.
				case 87:    //   87 ERROR_INVALID_PARAMETER
					throw new Exception();


				case 8:     //    8 ERROR_NOT_ENOUGH_MEMORY         Not enough storage is available to process this command.
				case 14:    //   14 ERROR_OUTOFMEMORY               Not enough storage is available to complete this operation.
					throw new OutOfMemoryException();

				case 998:   //  998 ERROR_NOACCESS                  Invalid access to memory location.
					throw new InvalidOperationException();

				default:
                    // Not sure how to map the reset of the error codes so throw generic Win32Exception.
					throw new Win32Exception(errorCode);
			}
		}
		public static void OnClipboardFileDropList(Action<List<string>> action)
		{
			var files = Clipboard.GetFileDropList();
			if (files.Count > 0) {
				var ls=new List<string>();
				for (int i = 0; i < files.Count; i++) {
					ls.Add(files[i]);
				}
					
				action(ls);
			}
		}
		public static void OnClipboardDirectory(Action<string> action)
		{
			
			var dir = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir)) {
				var files = Clipboard.GetFileDropList();
				if (files.Count > 0)
					dir = files[0];
			}
			if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
				return;
			action(dir);
			
		}
		public static void OnClipboardFile(Action<string> action)
		{
			var dir = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir)) {
				var files = Clipboard.GetFileDropList();
				if (files.Count > 0)
					dir = files[0];
			}
			if (string.IsNullOrWhiteSpace(dir) || !File.Exists(dir))
				return;
			action(dir);
		}
		public static void OnClipboardText(Action<string> action)
		{
			var value = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(value))
				return;
			action(value);
		}
		public static void OnClipboardString(Func<string, string> func)
		{
			var value = Clipboard.GetText();
			if (string.IsNullOrWhiteSpace(value))
				return;
			var result = func(value);
			if (string.IsNullOrWhiteSpace(result))
				return;
			Clipboard.SetText(result);
		}
	}
}
