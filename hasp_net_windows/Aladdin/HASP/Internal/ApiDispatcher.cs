using System;

namespace Aladdin.HASP.Internal
{
	internal interface ApiDispatcher
	{
		HaspStatus disp_login(int feature_id, string vendor_code, ref int handle);

		HaspStatus disp_login(int feature_id, byte[] vendor_code, ref int handle);

		HaspStatus disp_login_scope(int feature_id, string scope, string vendor_code, ref int handle);

		HaspStatus disp_login_scope(int feature_id, string scope, byte[] vendor_code, ref int handle);

		HaspStatus disp_logout(int handle);

		HaspStatus disp_encrypt(int handle, byte[] data);

		HaspStatus disp_encrypt(int handle, double[] data);

		HaspStatus disp_encrypt(int handle, short[] data);

		HaspStatus disp_encrypt(int handle, int[] data);

		HaspStatus disp_encrypt(int handle, long[] data);

		HaspStatus disp_encrypt(int handle, float[] data);

		HaspStatus disp_decrypt(int handle, byte[] data);

		HaspStatus disp_decrypt(int handle, double[] data);

		HaspStatus disp_decrypt(int handle, short[] data);

		HaspStatus disp_decrypt(int handle, int[] data);

		HaspStatus disp_decrypt(int handle, long[] data);

		HaspStatus disp_decrypt(int handle, float[] data);

		HaspStatus disp_read(int handle, int fileid, int offset, int length, byte[] buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref bool buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref byte buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref char buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref double buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref short buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref int buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref long buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref ushort buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref uint buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref ulong buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref float buffer);

		HaspStatus disp_read(int handle, int fileid, int offset, ref string buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, byte[] buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, bool buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, byte buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, char buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, double buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, short buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, int buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, long buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, ushort buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, uint buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, ulong buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, float buffer);

		HaspStatus disp_write(int handle, int fileid, int offset, string buffer);

		HaspStatus disp_get_size(int handle, int fileid, ref int size);

		HaspStatus disp_get_rtc(int handle, ref long time);

		HaspStatus disp_legacy_encrypt(int handle, byte[] buffer);

		HaspStatus disp_legacy_encrypt(int handle, double[] buffer);

		HaspStatus disp_legacy_encrypt(int handle, short[] buffer);

		HaspStatus disp_legacy_encrypt(int handle, int[] buffer);

		HaspStatus disp_legacy_encrypt(int handle, long[] buffer);

		HaspStatus disp_legacy_encrypt(int handle, float[] buffer);

		HaspStatus disp_legacy_decrypt(int handle, byte[] buffer);

		HaspStatus disp_legacy_decrypt(int handle, double[] buffer);

		HaspStatus disp_legacy_decrypt(int handle, short[] buffer);

		HaspStatus disp_legacy_decrypt(int handle, int[] buffer);

		HaspStatus disp_legacy_decrypt(int handle, long[] buffer);

		HaspStatus disp_legacy_decrypt(int handle, float[] buffer);

		HaspStatus disp_legacy_set_rtc(int handle, long new_time);

		HaspStatus disp_legacy_set_idletime(int handle, short idle_time);

		HaspStatus disp_get_info(string scope, string format, string vendor_code, ref string info);

		HaspStatus disp_get_info(string scope, string format, byte[] vendor_code, ref string info);

		HaspStatus disp_get_sessioninfo(int handle, string format, ref string info);

		void disp_free(IntPtr info);

		HaspStatus disp_update(string update_data, ref string ack_data);

		HaspStatus disp_detach(string detach_action, string scope, string vendor_code, string recipient, ref string info);

		HaspStatus disp_detach(string detach_action, string scope, byte[] vendor_code, string recipient, ref string info);

		HaspStatus disp_transfer(string action, string scope, string vendor_code, string recipient, ref string info);

		HaspStatus disp_transfer(string action, string scope, byte[] vendor_code, string recipient, ref string info);

		HaspStatus disp_get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, string vendor_code);

		HaspStatus disp_get_version(ref int major_version, ref int minor_version, ref int build_server, ref int build_number, byte[] vendor_code);

		HaspStatus disp_datetime_to_hasptime(int day, int month, int year, int hour, int minute, int second, ref long time);

		HaspStatus disp_hasptime_to_datetime(long time, ref int day, ref int month, ref int year, ref int hour, ref int minute, ref int second);

		HaspStatus disp_set_lib_path(string path);
	}
}
