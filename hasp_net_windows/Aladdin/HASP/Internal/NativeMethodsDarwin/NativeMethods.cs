using System;
using System.Runtime.InteropServices;

namespace Aladdin.HASP.Internal.NativeMethodsDarwin
{
	internal static class NativeMethods
	{
		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_login(int feature_id, string vendor_code, ref int handle);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_login(int feature_id, byte[] vendor_code, ref int handle);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_login_scope(int feature_id, string scope, string vendor_code, ref int handle);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_login_scope(int feature_id, string scope, byte[] vendor_code, ref int handle);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_logout(int handle);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_encrypt(int handle, [In] [Out] byte[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_encrypt(int handle, [In] [Out] double[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_encrypt(int handle, [In] [Out] short[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_encrypt(int handle, [In] [Out] int[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_encrypt(int handle, [In] [Out] long[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_encrypt(int handle, [In] [Out] float[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_decrypt(int handle, [In] [Out] byte[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_decrypt(int handle, [In] [Out] double[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_decrypt(int handle, [In] [Out] short[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_decrypt(int handle, [In] [Out] int[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_decrypt(int handle, [In] [Out] long[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_decrypt(int handle, [In] [Out] float[] data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, [Out] byte[] buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, [MarshalAs(UnmanagedType.U1)] ref bool buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref byte buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref char buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref double buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref short buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref int buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref long buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref ushort buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref uint buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref ulong buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref float buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref string buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, byte[] buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, [MarshalAs(UnmanagedType.U1)] ref bool buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref byte buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref char buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref double buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref short buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref int buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref long buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref ushort buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref uint buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref ulong buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref float buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref string buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_size(int handle, int fileid, ref int size);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_rtc(int handle, ref long time);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_encrypt(int handle, [In] [Out] byte[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_encrypt(int handle, [In] [Out] double[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_encrypt(int handle, [In] [Out] short[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_encrypt(int handle, [In] [Out] int[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_encrypt(int handle, [In] [Out] long[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_encrypt(int handle, [In] [Out] float[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_decrypt(int handle, [In] [Out] byte[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_decrypt(int handle, [In] [Out] double[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_decrypt(int handle, [In] [Out] short[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_decrypt(int handle, [In] [Out] int[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_decrypt(int handle, [In] [Out] long[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_decrypt(int handle, [In] [Out] float[] buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_set_rtc(int handle, long new_time);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_set_idletime(int handle, short idle_time);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_info(string scope, string format, string vendor_code, ref string info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_info(string scope, string format, byte[] vendor_code, ref string info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_sessioninfo(int handle, string format, ref string info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern void hasp_free(IntPtr info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_update(string update_data, ref string ack_data);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_detach(string detach_action, string scope, string vendor_code, string recipient, ref string info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_detach(string detach_action, string scope, byte[] vendor_code, string recipient, ref string info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_transfer(string action, string scope, string vendor_code, string recipient, ref string info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_transfer(string action, string scope, byte[] vendor_code, string recipient, ref string info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, string vendor_code);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, byte[] vendor_code);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_datetime_to_hasptime(int day, int month, int year, int hour, int minute, int second, ref long time);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_hasptime_to_datetime(long time, ref int day, ref int month, ref int year, ref int hour, ref int minute, ref int second);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_login(int feature_id, IntPtr vendor_code, ref int handle);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_login_scope(int feature_id, IntPtr scope, IntPtr vendor_code, ref int handle);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_encrypt(int handle, [In] [Out] IntPtr data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_decrypt(int handle, [In] [Out] IntPtr data, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, ref IntPtr buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_read(int handle, int fileid, int offset, int length, IntPtr buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, ref IntPtr buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_write(int handle, int fileid, int offset, int length, IntPtr buffer);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_encrypt(int handle, [In] [Out] IntPtr buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_legacy_decrypt(int handle, [In] [Out] IntPtr buffer, int length);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_info(IntPtr scope, IntPtr format, IntPtr vendor_code, ref IntPtr info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_sessioninfo(int handle, IntPtr format, ref IntPtr info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_update(IntPtr update_data, ref IntPtr ack_data);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_detach(IntPtr detach_action, IntPtr scope, IntPtr vendor_code, IntPtr recipient, ref IntPtr info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_transfer(IntPtr action, IntPtr scope, IntPtr vendor_code, IntPtr recipient, ref IntPtr info);

		[DllImport("apidsp_darwin.dylib")]
		public static extern HaspStatus hasp_get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, IntPtr vendor_code);
	}
}
