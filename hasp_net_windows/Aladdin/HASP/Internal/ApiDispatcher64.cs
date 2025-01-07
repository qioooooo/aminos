using System;
using System.Runtime.InteropServices;
using System.Text;
using Aladdin.HASP.Internal.NativeMethods64;

namespace Aladdin.HASP.Internal
{
	internal class ApiDispatcher64 : ApiDispatcher
	{
		public HaspStatus disp_login(int feature_id, string vendor_code, ref int handle)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.StringToHGlobalAnsi(vendor_code);
			HaspStatus haspStatus = NativeMethods.hasp_login(feature_id, intPtr, ref handle);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_login(int feature_id, byte[] vendor_code, ref int handle)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.StringToHGlobalAnsi(Encoding.ASCII.GetString(vendor_code));
			HaspStatus haspStatus = NativeMethods.hasp_login(feature_id, intPtr, ref handle);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_login_scope(int feature_id, string scope, string vendor_code, ref int handle)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			intPtr = Marshal.StringToHGlobalAnsi(vendor_code);
			intPtr2 = ApiDisp.NativeUtf8FromString(scope);
			HaspStatus haspStatus = NativeMethods.hasp_login_scope(feature_id, intPtr2, intPtr, ref handle);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			return haspStatus;
		}

		public HaspStatus disp_login_scope(int feature_id, string scope, byte[] vendor_code, ref int handle)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			intPtr = Marshal.StringToHGlobalAnsi(Encoding.ASCII.GetString(vendor_code));
			intPtr2 = ApiDisp.NativeUtf8FromString(scope);
			HaspStatus haspStatus = NativeMethods.hasp_login_scope(feature_id, intPtr2, intPtr, ref handle);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			return haspStatus;
		}

		public HaspStatus disp_logout(int handle)
		{
			return NativeMethods.hasp_logout(handle);
		}

		public HaspStatus disp_encrypt(int handle, byte[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_encrypt(handle, intPtr, data.Length);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_encrypt(int handle, double[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_encrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_encrypt(int handle, short[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 2);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_encrypt(handle, intPtr, data.Length * 2);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_encrypt(int handle, int[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_encrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_encrypt(int handle, long[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_encrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_encrypt(int handle, float[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_encrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_decrypt(int handle, byte[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_decrypt(handle, intPtr, data.Length);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_decrypt(int handle, double[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_decrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_decrypt(int handle, short[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 2);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_decrypt(handle, intPtr, data.Length * 2);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_decrypt(int handle, int[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_decrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_decrypt(int handle, long[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_decrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_decrypt(int handle, float[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_decrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, int length, byte[] buffer)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(buffer.Length);
			HaspStatus haspStatus = NativeMethods.hasp_read(handle, fileid, offset, length, intPtr);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, buffer, 0, length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref bool buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 1, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref byte buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 1, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref char buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 2, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref double buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 8, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref short buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 2, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref int buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 4, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref long buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 8, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref ushort buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 2, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref uint buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 4, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref ulong buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 8, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref float buffer)
		{
			return NativeMethods.hasp_read(handle, fileid, offset, 4, ref buffer);
		}

		public HaspStatus disp_read(int handle, int fileid, int offset, ref string buffer)
		{
			IntPtr zero = IntPtr.Zero;
			HaspStatus haspStatus = NativeMethods.hasp_read(handle, fileid, offset, 1, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				buffer = Marshal.PtrToStringAuto(zero);
			}
			if (zero != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(zero);
			}
			return haspStatus;
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, byte[] buffer)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(buffer.Length);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(buffer, 0, intPtr, buffer.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_write(handle, fileid, offset, buffer.Length, intPtr);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, bool buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 1, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, byte buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 1, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, char buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 2, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, double buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 8, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, short buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 2, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, int buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 4, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, long buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 8, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, ushort buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 2, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, uint buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 4, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, ulong buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 8, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, float buffer)
		{
			return NativeMethods.hasp_write(handle, fileid, offset, 4, ref buffer);
		}

		public HaspStatus disp_write(int handle, int fileid, int offset, string buffer)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.StringToCoTaskMemAuto(buffer);
			HaspStatus haspStatus = NativeMethods.hasp_write(handle, fileid, offset, 1, ref intPtr);
			if (haspStatus == HaspStatus.StatusOk)
			{
				buffer = Marshal.PtrToStringAnsi(intPtr);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_get_size(int handle, int fileid, ref int size)
		{
			return NativeMethods.hasp_get_size(handle, fileid, ref size);
		}

		public HaspStatus disp_get_rtc(int handle, ref long time)
		{
			return NativeMethods.hasp_get_rtc(handle, ref time);
		}

		public HaspStatus disp_legacy_encrypt(int handle, byte[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_encrypt(handle, intPtr, data.Length);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_encrypt(int handle, double[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_encrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_encrypt(int handle, short[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 2);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_encrypt(handle, intPtr, data.Length * 2);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_encrypt(int handle, int[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_encrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_encrypt(int handle, long[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_encrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_encrypt(int handle, float[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_encrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_decrypt(int handle, byte[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_decrypt(handle, intPtr, data.Length);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_decrypt(int handle, double[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_decrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_decrypt(int handle, short[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 2);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_decrypt(handle, intPtr, data.Length * 2);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_decrypt(int handle, int[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_decrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_decrypt(int handle, long[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 8);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_decrypt(handle, intPtr, data.Length * 8);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_decrypt(int handle, float[] data)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.AllocHGlobal(data.Length * 4);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.Copy(data, 0, intPtr, data.Length);
			}
			HaspStatus haspStatus = NativeMethods.hasp_legacy_decrypt(handle, intPtr, data.Length * 4);
			if (haspStatus == HaspStatus.StatusOk)
			{
				Marshal.Copy(intPtr, data, 0, data.Length);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_legacy_set_rtc(int handle, long new_time)
		{
			return NativeMethods.hasp_legacy_set_rtc(handle, new_time);
		}

		public HaspStatus disp_legacy_set_idletime(int handle, short idle_time)
		{
			return NativeMethods.hasp_legacy_set_idletime(handle, idle_time);
		}

		public HaspStatus disp_get_info(string scope, string format, string vendor_code, ref string info)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			intPtr = ApiDisp.NativeUtf8FromString(scope);
			intPtr2 = Marshal.StringToHGlobalAnsi(format);
			intPtr3 = Marshal.StringToHGlobalAnsi(vendor_code);
			HaspStatus haspStatus = NativeMethods.hasp_get_info(intPtr, intPtr2, intPtr3, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				info = ApiDisp.StringFromNativeUtf8(zero);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			return haspStatus;
		}

		public HaspStatus disp_get_info(string scope, string format, byte[] vendor_code, ref string info)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			intPtr = ApiDisp.NativeUtf8FromString(scope);
			intPtr2 = Marshal.StringToHGlobalAnsi(format);
			intPtr3 = Marshal.StringToHGlobalAnsi(Encoding.ASCII.GetString(vendor_code));
			HaspStatus haspStatus = NativeMethods.hasp_get_info(intPtr, intPtr2, intPtr3, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				info = ApiDisp.StringFromNativeUtf8(zero);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			return haspStatus;
		}

		public HaspStatus disp_get_sessioninfo(int handle, string format, ref string info)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			intPtr = Marshal.StringToHGlobalAnsi(format);
			HaspStatus haspStatus = NativeMethods.hasp_get_sessioninfo(handle, intPtr, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				info = ApiDisp.StringFromNativeUtf8(zero);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			return haspStatus;
		}

		public void disp_free(IntPtr info)
		{
			NativeMethods.hasp_free(info);
		}

		public HaspStatus disp_update(string update_data, ref string ack_data)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			intPtr = ApiDisp.NativeUtf8FromString(update_data);
			HaspStatus haspStatus = NativeMethods.hasp_update(intPtr, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				if (zero != IntPtr.Zero)
				{
					ack_data = ApiDisp.StringFromNativeUtf8(zero);
				}
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			return haspStatus;
		}

		public HaspStatus disp_detach(string detach_action, string scope, string vendor_code, string recipient, ref string info)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			intPtr = ApiDisp.NativeUtf8FromString(scope);
			intPtr2 = Marshal.StringToHGlobalAnsi(detach_action);
			intPtr3 = Marshal.StringToHGlobalAnsi(vendor_code);
			intPtr4 = ApiDisp.NativeUtf8FromString(recipient);
			HaspStatus haspStatus = NativeMethods.hasp_detach(intPtr2, intPtr, intPtr3, intPtr4, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				info = ApiDisp.StringFromNativeUtf8(zero);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (intPtr4 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr4);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			return haspStatus;
		}

		public HaspStatus disp_detach(string detach_action, string scope, byte[] vendor_code, string recipient, ref string info)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			intPtr = ApiDisp.NativeUtf8FromString(scope);
			intPtr2 = Marshal.StringToHGlobalAnsi(detach_action);
			intPtr3 = Marshal.StringToHGlobalAnsi(Encoding.ASCII.GetString(vendor_code));
			intPtr4 = ApiDisp.NativeUtf8FromString(recipient);
			HaspStatus haspStatus = NativeMethods.hasp_detach(intPtr2, intPtr, intPtr3, intPtr4, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				info = ApiDisp.StringFromNativeUtf8(zero);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (intPtr4 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr4);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			return haspStatus;
		}

		public HaspStatus disp_transfer(string action, string scope, string vendor_code, string recipient, ref string info)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			intPtr = ApiDisp.NativeUtf8FromString(scope);
			intPtr2 = Marshal.StringToHGlobalAnsi(action);
			intPtr3 = Marshal.StringToHGlobalAnsi(vendor_code);
			intPtr4 = ApiDisp.NativeUtf8FromString(recipient);
			HaspStatus haspStatus = NativeMethods.hasp_transfer(intPtr2, intPtr, intPtr3, intPtr4, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				info = ApiDisp.StringFromNativeUtf8(zero);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			if (intPtr4 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr4);
			}
			return haspStatus;
		}

		public HaspStatus disp_transfer(string action, string scope, byte[] vendor_code, string recipient, ref string info)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			intPtr = ApiDisp.NativeUtf8FromString(scope);
			intPtr2 = Marshal.StringToHGlobalAnsi(action);
			intPtr3 = Marshal.StringToHGlobalAnsi(Encoding.ASCII.GetString(vendor_code));
			intPtr4 = ApiDisp.NativeUtf8FromString(recipient);
			HaspStatus haspStatus = NativeMethods.hasp_transfer(intPtr2, intPtr, intPtr3, intPtr4, ref zero);
			if (haspStatus == HaspStatus.StatusOk)
			{
				info = ApiDisp.StringFromNativeUtf8(zero);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (zero != IntPtr.Zero)
			{
				this.disp_free(zero);
			}
			if (intPtr4 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr4);
			}
			return haspStatus;
		}

		public HaspStatus disp_get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, string vendor_code)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.StringToHGlobalAnsi(vendor_code);
			HaspStatus haspStatus = NativeMethods.hasp_get_version(ref major_version, ref minor_version, ref build_server, ref build_number, intPtr);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, byte[] vendor_code)
		{
			IntPtr intPtr = IntPtr.Zero;
			intPtr = Marshal.StringToHGlobalAnsi(Encoding.ASCII.GetString(vendor_code));
			HaspStatus haspStatus = NativeMethods.hasp_get_version(ref major_version, ref minor_version, ref build_server, ref build_number, intPtr);
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}

		public HaspStatus disp_datetime_to_hasptime(int day, int month, int year, int hour, int minute, int second, ref long time)
		{
			return NativeMethods.hasp_datetime_to_hasptime(day, month, year, hour, minute, second, ref time);
		}

		public HaspStatus disp_hasptime_to_datetime(long time, ref int day, ref int month, ref int year, ref int hour, ref int minute, ref int second)
		{
			return NativeMethods.hasp_hasptime_to_datetime(time, ref day, ref month, ref year, ref hour, ref minute, ref second);
		}

		public HaspStatus disp_set_lib_path(string path)
		{
			IntPtr intPtr = IntPtr.Zero;
			HaspStatus haspStatus;
			if (path != null)
			{
				intPtr = ApiDisp.NativeUtf8FromString(path);
				haspStatus = NativeMethods.hasp_set_lib_path(intPtr);
			}
			else
			{
				haspStatus = NativeMethods.hasp_set_lib_path(0L);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return haspStatus;
		}
	}
}
