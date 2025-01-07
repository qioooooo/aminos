using System;
using System.Runtime.InteropServices;
using System.Text;
using Aladdin.HASP.Internal.NativeMethods32;

namespace Aladdin.HASP.Internal
{
	internal static class ApiDisp
	{
		public static int IsRunningOnMono()
		{
			int num = 1;
			if (Environment.OSVersion.ToString().IndexOf("Windows") >= 0)
			{
				num = 0;
			}
			else
			{
				string text = Environment.OSVersion.ToString();
				int num2 = text.IndexOf('.');
				int num3 = text.IndexOf(' ');
				if (num2 >= 0 && num3 >= 0)
				{
					string text2 = text.Substring(num3, num2 - num3);
					int num4 = int.Parse(text2);
					if (num4 >= 4)
					{
						num = 2;
					}
				}
			}
			return num;
		}

		public static IntPtr NativeUtf8FromString(string managedString)
		{
			int byteCount = Encoding.UTF8.GetByteCount(managedString);
			byte[] array = new byte[byteCount + 1];
			Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, array, 0);
			IntPtr intPtr = Marshal.AllocHGlobal(array.Length);
			Marshal.Copy(array, 0, intPtr, array.Length);
			return intPtr;
		}

		public static string StringFromNativeUtf8(IntPtr nativeUtf8)
		{
			int num = 0;
			while (Marshal.ReadByte(nativeUtf8, num) != 0)
			{
				num++;
			}
			string text;
			if (num == 0)
			{
				text = string.Empty;
			}
			else
			{
				byte[] array = new byte[num];
				Marshal.Copy(nativeUtf8, array, 0, array.Length);
				text = Encoding.UTF8.GetString(array);
			}
			return text;
		}

		public static HaspStatus login(int feature_id, string vendor_code, ref int handle)
		{
			return ApiDisp.apidsp.disp_login(feature_id, vendor_code, ref handle);
		}

		public static HaspStatus login(int feature_id, byte[] vendor_code, ref int handle)
		{
			return ApiDisp.apidsp.disp_login(feature_id, vendor_code, ref handle);
		}

		public static HaspStatus login_scope(int feature_id, string scope, string vendor_code, ref int handle)
		{
			return ApiDisp.apidsp.disp_login_scope(feature_id, scope, vendor_code, ref handle);
		}

		public static HaspStatus login_scope(int feature_id, string scope, byte[] vendor_code, ref int handle)
		{
			return ApiDisp.apidsp.disp_login_scope(feature_id, scope, vendor_code, ref handle);
		}

		public static HaspStatus logout(int handle)
		{
			return ApiDisp.apidsp.disp_logout(handle);
		}

		public static HaspStatus encrypt(int handle, byte[] data)
		{
			return ApiDisp.apidsp.disp_encrypt(handle, data);
		}

		public static HaspStatus encrypt(int handle, char[] data)
		{
			short[] array = new short[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = (short)data[i];
			}
			HaspStatus haspStatus = ApiDisp.apidsp.disp_encrypt(handle, array);
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (char)array[i];
			}
			return haspStatus;
		}

		public static HaspStatus encrypt(int handle, double[] data)
		{
			return ApiDisp.apidsp.disp_encrypt(handle, data);
		}

		public static HaspStatus encrypt(int handle, short[] data)
		{
			return ApiDisp.apidsp.disp_encrypt(handle, data);
		}

		public static HaspStatus encrypt(int handle, int[] data)
		{
			return ApiDisp.apidsp.disp_encrypt(handle, data);
		}

		public static HaspStatus encrypt(int handle, long[] data)
		{
			return ApiDisp.apidsp.disp_encrypt(handle, data);
		}

		public static HaspStatus encrypt(int handle, float[] data)
		{
			return ApiDisp.apidsp.disp_encrypt(handle, data);
		}

		public static HaspStatus encrypt(int handle, ref string data)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			HaspStatus haspStatus = ApiDisp.apidsp.disp_encrypt(handle, bytes);
			data = Convert.ToBase64String(bytes);
			return haspStatus;
		}

		public static HaspStatus decrypt(int handle, byte[] data)
		{
			return ApiDisp.apidsp.disp_decrypt(handle, data);
		}

		public static HaspStatus decrypt(int handle, char[] data)
		{
			short[] array = new short[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = (short)data[i];
			}
			HaspStatus haspStatus = ApiDisp.apidsp.disp_decrypt(handle, array);
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (char)array[i];
			}
			return haspStatus;
		}

		public static HaspStatus decrypt(int handle, double[] data)
		{
			return ApiDisp.apidsp.disp_decrypt(handle, data);
		}

		public static HaspStatus decrypt(int handle, short[] data)
		{
			return ApiDisp.apidsp.disp_decrypt(handle, data);
		}

		public static HaspStatus decrypt(int handle, int[] data)
		{
			return ApiDisp.apidsp.disp_decrypt(handle, data);
		}

		public static HaspStatus decrypt(int handle, long[] data)
		{
			return ApiDisp.apidsp.disp_decrypt(handle, data);
		}

		public static HaspStatus decrypt(int handle, float[] data)
		{
			return ApiDisp.apidsp.disp_decrypt(handle, data);
		}

		public static HaspStatus decrypt(int handle, ref string data)
		{
			byte[] array = Convert.FromBase64String(data);
			HaspStatus haspStatus = ApiDisp.apidsp.disp_decrypt(handle, array);
			data = Encoding.UTF8.GetString(array);
			return haspStatus;
		}

		public static HaspStatus read(int handle, int fileid, int offset, byte[] buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, buffer.Length, buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref bool buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref byte buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref char buffer)
		{
			short num = 0;
			HaspStatus haspStatus = ApiDisp.apidsp.disp_read(handle, fileid, offset, ref num);
			buffer = (char)num;
			return haspStatus;
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref double buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref short buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref int buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref long buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref ushort buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref uint buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref ulong buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref float buffer)
		{
			return ApiDisp.apidsp.disp_read(handle, fileid, offset, ref buffer);
		}

		public static HaspStatus read(int handle, int fileid, int offset, ref string buffer)
		{
			byte b = 0;
			HaspStatus haspStatus = ApiDisp.read(handle, fileid, offset, ref b);
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				buffer = "";
				haspStatus2 = haspStatus;
			}
			else
			{
				byte[] array = new byte[(int)b];
				haspStatus = ApiDisp.read(handle, fileid, offset + 1, array);
				if (haspStatus != HaspStatus.StatusOk)
				{
					buffer = "";
					haspStatus2 = haspStatus;
				}
				else
				{
					buffer = Encoding.UTF8.GetString(array);
					haspStatus2 = HaspStatus.StatusOk;
				}
			}
			return haspStatus2;
		}

		public static HaspStatus write(int handle, int fileid, int offset, byte[] buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, bool buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, byte buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, char buffer)
		{
			short num = (short)buffer;
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, num);
		}

		public static HaspStatus write(int handle, int fileid, int offset, double buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, short buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, int buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, long buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, ushort buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, uint buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, ulong buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, float buffer)
		{
			return ApiDisp.apidsp.disp_write(handle, fileid, offset, buffer);
		}

		public static HaspStatus write(int handle, int fileid, int offset, string buffer)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(buffer);
			byte[] array = new byte[bytes.Length + 1];
			HaspStatus haspStatus;
			if (bytes.Length > 255)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				array[0] = (byte)bytes.Length;
				bytes.CopyTo(array, 1);
				haspStatus = ApiDisp.apidsp.disp_write(handle, fileid, offset, array);
			}
			return haspStatus;
		}

		public static HaspStatus get_size(int handle, int fileid, ref int size)
		{
			return ApiDisp.apidsp.disp_get_size(handle, fileid, ref size);
		}

		public static HaspStatus get_rtc(int handle, ref long time)
		{
			return ApiDisp.apidsp.disp_get_rtc(handle, ref time);
		}

		public static HaspStatus legacy_encrypt(int handle, byte[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_encrypt(handle, buffer);
		}

		public static HaspStatus legacy_encrypt(int handle, char[] data)
		{
			short[] array = new short[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = (short)data[i];
			}
			HaspStatus haspStatus = ApiDisp.apidsp.disp_legacy_encrypt(handle, array);
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (char)array[i];
			}
			return haspStatus;
		}

		public static HaspStatus legacy_encrypt(int handle, double[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_encrypt(handle, buffer);
		}

		public static HaspStatus legacy_encrypt(int handle, short[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_encrypt(handle, buffer);
		}

		public static HaspStatus legacy_encrypt(int handle, int[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_encrypt(handle, buffer);
		}

		public static HaspStatus legacy_encrypt(int handle, long[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_encrypt(handle, buffer);
		}

		public static HaspStatus legacy_encrypt(int handle, float[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_encrypt(handle, buffer);
		}

		public static HaspStatus legacy_encrypt(int handle, ref string data)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			HaspStatus haspStatus = ApiDisp.apidsp.disp_legacy_encrypt(handle, bytes);
			data = Convert.ToBase64String(bytes);
			return haspStatus;
		}

		public static HaspStatus legacy_decrypt(int handle, byte[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_decrypt(handle, buffer);
		}

		public static HaspStatus legacy_decrypt(int handle, char[] data)
		{
			short[] array = new short[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				array[i] = (short)data[i];
			}
			HaspStatus haspStatus = ApiDisp.apidsp.disp_legacy_decrypt(handle, array);
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (char)array[i];
			}
			return haspStatus;
		}

		public static HaspStatus legacy_decrypt(int handle, double[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_decrypt(handle, buffer);
		}

		public static HaspStatus legacy_decrypt(int handle, short[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_decrypt(handle, buffer);
		}

		public static HaspStatus legacy_decrypt(int handle, int[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_decrypt(handle, buffer);
		}

		public static HaspStatus legacy_decrypt(int handle, long[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_decrypt(handle, buffer);
		}

		public static HaspStatus legacy_decrypt(int handle, float[] buffer)
		{
			return ApiDisp.apidsp.disp_legacy_decrypt(handle, buffer);
		}

		public static HaspStatus legacy_decrypt(int handle, ref string data)
		{
			byte[] array = Convert.FromBase64String(data);
			HaspStatus haspStatus = ApiDisp.apidsp.disp_legacy_decrypt(handle, array);
			data = Encoding.UTF8.GetString(array);
			return haspStatus;
		}

		public static HaspStatus legacy_set_rtc(int handle, long new_time)
		{
			return ApiDisp.apidsp.disp_legacy_set_rtc(handle, new_time);
		}

		public static HaspStatus legacy_set_idletime(int handle, short idle_time)
		{
			return ApiDisp.apidsp.disp_legacy_set_idletime(handle, idle_time);
		}

		public static HaspStatus get_info(string scope, string format, string vendor_code, ref string info)
		{
			return ApiDisp.apidsp.disp_get_info(scope, format, vendor_code, ref info);
		}

		public static HaspStatus get_info(string scope, string format, byte[] vendor_code, ref string info)
		{
			return ApiDisp.apidsp.disp_get_info(scope, format, vendor_code, ref info);
		}

		public static HaspStatus get_sessioninfo(int handle, string format, ref string info)
		{
			return ApiDisp.apidsp.disp_get_sessioninfo(handle, format, ref info);
		}

		public static HaspStatus update(string update_data, ref string ack_data)
		{
			return ApiDisp.apidsp.disp_update(update_data, ref ack_data);
		}

		public static HaspStatus get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, string vendor_code)
		{
			return ApiDisp.apidsp.disp_get_version(ref major_version, ref minor_version, ref build_server, ref build_number, vendor_code);
		}

		public static HaspStatus get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, byte[] vendor_code)
		{
			return ApiDisp.apidsp.disp_get_version(ref major_version, ref minor_version, ref build_server, ref build_number, vendor_code);
		}

		public static HaspStatus detach(string detach_action, string scope, string vendor_code, string recipient, ref string info)
		{
			return ApiDisp.apidsp.disp_detach(detach_action, scope, vendor_code, recipient, ref info);
		}

		public static HaspStatus detach(string detach_action, string scope, byte[] vendor_code, string recipient, ref string info)
		{
			return ApiDisp.apidsp.disp_detach(detach_action, scope, vendor_code, recipient, ref info);
		}

		public static HaspStatus set_lib_path(string path)
		{
			if (ApiDisp.IsRunningOnMono() == 1)
			{
				NativeMethods.SetDllDirectory(path);
			}
			return ApiDisp.apidsp.disp_set_lib_path(path);
		}

		public static HaspStatus transfer(string action, string scope, string vendor_code, string recipient, ref string info)
		{
			return ApiDisp.apidsp.disp_transfer(action, scope, vendor_code, recipient, ref info);
		}

		public static HaspStatus transfer(string action, string scope, byte[] vendor_code, string recipient, ref string info)
		{
			return ApiDisp.apidsp.disp_transfer(action, scope, vendor_code, recipient, ref info);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static ApiDisp()
		{
			ApiDispatcher apiDispatcher2;
			if (ApiDisp.IsRunningOnMono() <= 0)
			{
				if (IntPtr.Size != 4)
				{
					ApiDispatcher apiDispatcher = new ApiDispatcher64();
					apiDispatcher2 = apiDispatcher;
				}
				else
				{
					apiDispatcher2 = new ApiDispatcher32();
				}
			}
			else if (ApiDisp.IsRunningOnMono() != 1)
			{
				apiDispatcher2 = new ApiDispatcherDarwin();
			}
			else if (IntPtr.Size != 4)
			{
				ApiDispatcher apiDispatcher = new ApiDispatcherLinux64();
				apiDispatcher2 = apiDispatcher;
			}
			else
			{
				apiDispatcher2 = new ApiDispatcherLinux32();
			}
			ApiDisp.apidsp = apiDispatcher2;
		}

		private static ApiDispatcher apidsp;
	}
}
