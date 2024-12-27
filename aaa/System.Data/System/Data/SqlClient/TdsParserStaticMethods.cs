using System;
using System.Data.Common;
using System.Globalization;

namespace System.Data.SqlClient
{
	// Token: 0x02000335 RID: 821
	internal sealed class TdsParserStaticMethods
	{
		// Token: 0x06002AF0 RID: 10992 RVA: 0x0029FB54 File Offset: 0x0029EF54
		private TdsParserStaticMethods()
		{
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x0029FB68 File Offset: 0x0029EF68
		internal static void AliasRegistryLookup(ref string host, ref string protocol)
		{
			if (!ADP.IsEmpty(host))
			{
				string text = (string)ADP.LocalMachineRegistryValue("SOFTWARE\\Microsoft\\MSSQLServer\\Client\\ConnectTo", host);
				if (!ADP.IsEmpty(text))
				{
					int num = text.IndexOf(',');
					if (-1 != num)
					{
						string text2 = text.Substring(0, num).ToLower(CultureInfo.InvariantCulture);
						if (num + 1 < text.Length)
						{
							string text3 = text.Substring(num + 1);
							if ("dbnetlib" == text2)
							{
								num = text3.IndexOf(':');
								if (-1 != num && num + 1 < text3.Length)
								{
									text2 = text3.Substring(0, num);
									if (SqlConnectionString.ValidProtocal(text2))
									{
										protocol = text2;
										host = text3.Substring(num + 1);
										return;
									}
								}
							}
							else
							{
								protocol = (string)SqlConnectionString.NetlibMapping()[text2];
								if (protocol != null)
								{
									host = text3;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x0029FC34 File Offset: 0x0029F034
		internal static byte[] EncryptPassword(string password)
		{
			byte[] array = new byte[password.Length << 1];
			for (int i = 0; i < password.Length; i++)
			{
				int num = (int)password[i];
				byte b = (byte)(num & 255);
				byte b2 = (byte)((num >> 8) & 255);
				array[i << 1] = (byte)((((int)(b & 15) << 4) | (b >> 4)) ^ 165);
				array[(i << 1) + 1] = (byte)((((int)(b2 & 15) << 4) | (b2 >> 4)) ^ 165);
			}
			return array;
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x0029FCB0 File Offset: 0x0029F0B0
		internal static int GetCurrentProcessId()
		{
			return SafeNativeMethods.GetCurrentProcessId();
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x0029FCC4 File Offset: 0x0029F0C4
		internal static byte[] GetNIC()
		{
			int num = 0;
			byte[] array = null;
			object obj = ADP.LocalMachineRegistryValue("SOFTWARE\\Description\\Microsoft\\Rpc\\UuidTemporaryData", "NetworkAddressLocal");
			if (obj is int)
			{
				num = (int)obj;
			}
			if (num <= 0)
			{
				obj = ADP.LocalMachineRegistryValue("SOFTWARE\\Description\\Microsoft\\Rpc\\UuidTemporaryData", "NetworkAddress");
				if (obj is byte[])
				{
					array = (byte[])obj;
				}
			}
			if (array == null)
			{
				array = new byte[6];
				Random random = new Random();
				random.NextBytes(array);
			}
			return array;
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x0029FD30 File Offset: 0x0029F130
		internal static int GetTimeoutMilliseconds(long timeoutTime)
		{
			if (9223372036854775807L == timeoutTime)
			{
				return -1;
			}
			long num = ADP.TimerRemainingMilliseconds(timeoutTime);
			if (num < 0L)
			{
				return 0;
			}
			if (num > 2147483647L)
			{
				return int.MaxValue;
			}
			return (int)num;
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x0029FD6C File Offset: 0x0029F16C
		internal static long GetTimeoutSeconds(int timeoutSeconds)
		{
			long num;
			if (timeoutSeconds == 0)
			{
				num = long.MaxValue;
			}
			else
			{
				long num2 = ADP.TimerCurrent();
				num = num2 + ADP.TimerFromSeconds(timeoutSeconds);
			}
			return num;
		}

		// Token: 0x06002AF7 RID: 10999 RVA: 0x0029FD98 File Offset: 0x0029F198
		internal static bool TimeoutHasExpired(long timeoutTime)
		{
			bool flag = false;
			if (0L != timeoutTime && 9223372036854775807L != timeoutTime)
			{
				flag = ADP.TimerHasExpired(timeoutTime);
			}
			return flag;
		}
	}
}
