using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.Globalization
{
	// Token: 0x02000372 RID: 882
	internal class JapaneseCalendarTable
	{
		// Token: 0x0600230B RID: 8971 RVA: 0x0005912C File Offset: 0x0005812C
		private JapaneseCalendarTable(JapaneseCalendarTable.ExtendedEraInfo[] eraInfo)
		{
			this._eraInfo = eraInfo;
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x0005913B File Offset: 0x0005813B
		private static JapaneseCalendarTable GetJapaneseCalendarTableInstance()
		{
			if (JapaneseCalendarTable.s_japanese == null)
			{
				JapaneseCalendarTable.s_japanese = new JapaneseCalendarTable(JapaneseCalendarTable.GetAllEras());
			}
			return JapaneseCalendarTable.s_japanese;
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x00059158 File Offset: 0x00058158
		private static JapaneseCalendarTable GetJapaneseLunisolarCalendarTableInstance()
		{
			if (JapaneseCalendarTable.s_japaneseLunisolar == null)
			{
				JapaneseCalendarTable.s_japaneseLunisolar = new JapaneseCalendarTable(JapaneseCalendarTable.TrimErasForLunisolar(JapaneseCalendarTable.GetAllEras()));
			}
			return JapaneseCalendarTable.s_japaneseLunisolar;
		}

		// Token: 0x0600230E RID: 8974 RVA: 0x0005917C File Offset: 0x0005817C
		private static JapaneseCalendarTable GetInstance(int calendarId)
		{
			if (calendarId == 3)
			{
				return JapaneseCalendarTable.GetJapaneseCalendarTableInstance();
			}
			if (calendarId != 14)
			{
				return null;
			}
			return JapaneseCalendarTable.GetJapaneseLunisolarCalendarTableInstance();
		}

		// Token: 0x0600230F RID: 8975 RVA: 0x000591A3 File Offset: 0x000581A3
		internal static bool IsJapaneseCalendar(int calendarId)
		{
			return calendarId == 3 || calendarId == 14;
		}

		// Token: 0x06002310 RID: 8976 RVA: 0x000591B0 File Offset: 0x000581B0
		private static JapaneseCalendarTable.ExtendedEraInfo[] GetAllEras()
		{
			if (JapaneseCalendarTable.s_allEras == null)
			{
				JapaneseCalendarTable.s_allEras = JapaneseCalendarTable.GetErasFromRegistry();
				if (JapaneseCalendarTable.s_allEras == null)
				{
					JapaneseCalendarTable.s_allEras = new JapaneseCalendarTable.ExtendedEraInfo[]
					{
						new JapaneseCalendarTable.ExtendedEraInfo(4, new DateTime(1989, 1, 8).Ticks, 1988, 1, 8011, "平成", "平", "H"),
						new JapaneseCalendarTable.ExtendedEraInfo(3, new DateTime(1926, 12, 25).Ticks, 1925, 1, 64, "昭和", "昭", "S"),
						new JapaneseCalendarTable.ExtendedEraInfo(2, new DateTime(1912, 7, 30).Ticks, 1911, 1, 15, "大正", "大", "T"),
						new JapaneseCalendarTable.ExtendedEraInfo(1, new DateTime(1868, 1, 1).Ticks, 1867, 1, 45, "明治", "明", "M")
					};
				}
			}
			return JapaneseCalendarTable.s_allEras;
		}

		// Token: 0x06002311 RID: 8977 RVA: 0x000592C4 File Offset: 0x000582C4
		[SecuritySafeCritical]
		private static JapaneseCalendarTable.ExtendedEraInfo[] GetErasFromRegistry()
		{
			int num = 0;
			JapaneseCalendarTable.ExtendedEraInfo[] array = null;
			try
			{
				PermissionSet permissionSet = new PermissionSet(PermissionState.None);
				permissionSet.AddPermission(new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras"));
				permissionSet.Assert();
				RegistryKey registryKey = RegistryKey.GetBaseKey(RegistryKey.HKEY_LOCAL_MACHINE).OpenSubKey("System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras", false);
				if (registryKey == null)
				{
					return null;
				}
				string[] valueNames = registryKey.GetValueNames();
				if (valueNames != null && valueNames.Length > 0)
				{
					array = new JapaneseCalendarTable.ExtendedEraInfo[valueNames.Length];
					for (int i = 0; i < valueNames.Length; i++)
					{
						JapaneseCalendarTable.ExtendedEraInfo eraFromValue = JapaneseCalendarTable.GetEraFromValue(valueNames[i], registryKey.GetValue(valueNames[i]).ToString());
						if (eraFromValue != null)
						{
							array[num] = eraFromValue;
							num++;
						}
					}
				}
			}
			catch (SecurityException)
			{
				return null;
			}
			catch (IOException)
			{
				return null;
			}
			catch (UnauthorizedAccessException)
			{
				return null;
			}
			if (num < 4)
			{
				return null;
			}
			Array.Resize<JapaneseCalendarTable.ExtendedEraInfo>(ref array, num);
			Array.Sort<JapaneseCalendarTable.ExtendedEraInfo>(array, new Comparison<JapaneseCalendarTable.ExtendedEraInfo>(JapaneseCalendarTable.CompareEraRanges));
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Era = array.Length - j;
				if (j == 0)
				{
					array[0].MaxEraYear = 9999 - array[0].YearOffset;
				}
				else
				{
					array[j].MaxEraYear = array[j - 1].YearOffset + 1 - array[j].YearOffset;
				}
			}
			return array;
		}

		// Token: 0x06002312 RID: 8978 RVA: 0x00059434 File Offset: 0x00058434
		private static int CompareEraRanges(JapaneseCalendarTable.ExtendedEraInfo a, JapaneseCalendarTable.ExtendedEraInfo b)
		{
			return b.Ticks.CompareTo(a.Ticks);
		}

		// Token: 0x06002313 RID: 8979 RVA: 0x00059458 File Offset: 0x00058458
		private static JapaneseCalendarTable.ExtendedEraInfo GetEraFromValue(string value, string data)
		{
			if (value == null || data == null)
			{
				return null;
			}
			if (value.Length != 10)
			{
				return null;
			}
			int num;
			int num2;
			int num3;
			if (!Number.TryParseInt32(value.Substring(0, 4), NumberStyles.None, NumberFormatInfo.InvariantInfo, out num) || !Number.TryParseInt32(value.Substring(5, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out num2) || !Number.TryParseInt32(value.Substring(8, 2), NumberStyles.None, NumberFormatInfo.InvariantInfo, out num3))
			{
				return null;
			}
			string[] array = data.Split(new char[] { '_' });
			if (array.Length != 4)
			{
				return null;
			}
			if (array[0].Length == 0 || array[1].Length == 0 || array[2].Length == 0 || array[3].Length == 0)
			{
				return null;
			}
			return new JapaneseCalendarTable.ExtendedEraInfo(0, new DateTime(num, num2, num3).Ticks, num - 1, 1, 0, array[0], array[1], array[3]);
		}

		// Token: 0x06002314 RID: 8980 RVA: 0x0005952E File Offset: 0x0005852E
		internal static int CurrentEra(int calendarId)
		{
			return JapaneseCalendarTable.GetAllEras().Length;
		}

		// Token: 0x06002315 RID: 8981 RVA: 0x00059537 File Offset: 0x00058537
		internal static string[] EraNames(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).EraNames();
		}

		// Token: 0x06002316 RID: 8982 RVA: 0x00059544 File Offset: 0x00058544
		private string[] EraNames()
		{
			if (this._eraNames == null)
			{
				this._eraNames = JapaneseCalendarTable.EraNames(this._eraInfo);
			}
			return this._eraNames;
		}

		// Token: 0x06002317 RID: 8983 RVA: 0x00059568 File Offset: 0x00058568
		private static string[] EraNames(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			string[] array = new string[eras.Length];
			for (int i = 0; i < eras.Length; i++)
			{
				array[i] = eras[eras.Length - i - 1].EraName;
			}
			return array;
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x0005959E File Offset: 0x0005859E
		internal static string[] AbbrevEraNames(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).AbbrevEraNames();
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000595AB File Offset: 0x000585AB
		private string[] AbbrevEraNames()
		{
			if (this._abbrevEraNames == null)
			{
				this._abbrevEraNames = JapaneseCalendarTable.AbbrevEraNames(this._eraInfo);
			}
			return this._abbrevEraNames;
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000595CC File Offset: 0x000585CC
		private static string[] AbbrevEraNames(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			string[] array = new string[eras.Length];
			for (int i = 0; i < eras.Length; i++)
			{
				array[i] = eras[eras.Length - i - 1].AbbrevEraName;
			}
			return array;
		}

		// Token: 0x0600231B RID: 8987 RVA: 0x00059602 File Offset: 0x00058602
		internal static string[] EnglishEraNames(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).EnglishEraNames();
		}

		// Token: 0x0600231C RID: 8988 RVA: 0x0005960F File Offset: 0x0005860F
		private string[] EnglishEraNames()
		{
			if (this._englishEraNames == null)
			{
				this._englishEraNames = JapaneseCalendarTable.EnglishEraNames(this._eraInfo);
			}
			return this._englishEraNames;
		}

		// Token: 0x0600231D RID: 8989 RVA: 0x00059630 File Offset: 0x00058630
		private static string[] EnglishEraNames(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			string[] array = new string[eras.Length];
			for (int i = 0; i < eras.Length; i++)
			{
				array[i] = eras[eras.Length - i - 1].EnglishEraName;
			}
			return array;
		}

		// Token: 0x0600231E RID: 8990 RVA: 0x00059666 File Offset: 0x00058666
		internal static int[][] EraRanges(int calendarId)
		{
			return JapaneseCalendarTable.GetInstance(calendarId).EraRanges();
		}

		// Token: 0x0600231F RID: 8991 RVA: 0x00059673 File Offset: 0x00058673
		private int[][] EraRanges()
		{
			if (this._eraRanges == null)
			{
				this._eraRanges = JapaneseCalendarTable.EraRanges(this._eraInfo);
			}
			return this._eraRanges;
		}

		// Token: 0x06002320 RID: 8992 RVA: 0x00059694 File Offset: 0x00058694
		private static int[][] EraRanges(JapaneseCalendarTable.ExtendedEraInfo[] eras)
		{
			int[][] array = new int[eras.Length][];
			for (int i = 0; i < eras.Length; i++)
			{
				JapaneseCalendarTable.ExtendedEraInfo extendedEraInfo = eras[i];
				int[] array2 = (array[i] = new int[6]);
				array2[0] = extendedEraInfo.Era;
				DateTime dateTime = new DateTime(extendedEraInfo.Ticks);
				array2[1] = dateTime.Year;
				array2[2] = dateTime.Month;
				array2[3] = dateTime.Day;
				array2[4] = extendedEraInfo.YearOffset;
				array2[5] = extendedEraInfo.MinEraYear;
			}
			return array;
		}

		// Token: 0x06002321 RID: 8993 RVA: 0x00059714 File Offset: 0x00058714
		private static JapaneseCalendarTable.ExtendedEraInfo[] TrimErasForLunisolar(JapaneseCalendarTable.ExtendedEraInfo[] baseEras)
		{
			JapaneseCalendarTable.ExtendedEraInfo[] array = new JapaneseCalendarTable.ExtendedEraInfo[baseEras.Length];
			int num = 0;
			for (int i = 0; i < baseEras.Length; i++)
			{
				if (baseEras[i].YearOffset + baseEras[i].MinEraYear < 2049)
				{
					if (baseEras[i].YearOffset + baseEras[i].MaxEraYear < 1960)
					{
						break;
					}
					array[num] = baseEras[i];
					num++;
				}
			}
			if (num == 0)
			{
				return baseEras;
			}
			Array.Resize<JapaneseCalendarTable.ExtendedEraInfo>(ref array, num);
			return array;
		}

		// Token: 0x04000EBA RID: 3770
		private const string c_japaneseErasHive = "System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

		// Token: 0x04000EBB RID: 3771
		private const string c_japaneseErasHivePermissionList = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Control\\Nls\\Calendars\\Japanese\\Eras";

		// Token: 0x04000EBC RID: 3772
		private static JapaneseCalendarTable.ExtendedEraInfo[] s_allEras;

		// Token: 0x04000EBD RID: 3773
		private static JapaneseCalendarTable s_japanese;

		// Token: 0x04000EBE RID: 3774
		private static JapaneseCalendarTable s_japaneseLunisolar;

		// Token: 0x04000EBF RID: 3775
		private JapaneseCalendarTable.ExtendedEraInfo[] _eraInfo;

		// Token: 0x04000EC0 RID: 3776
		private string[] _eraNames;

		// Token: 0x04000EC1 RID: 3777
		private string[] _abbrevEraNames;

		// Token: 0x04000EC2 RID: 3778
		private string[] _englishEraNames;

		// Token: 0x04000EC3 RID: 3779
		private int[][] _eraRanges;

		// Token: 0x02000373 RID: 883
		private class ExtendedEraInfo
		{
			// Token: 0x17000634 RID: 1588
			// (get) Token: 0x06002322 RID: 8994 RVA: 0x00059782 File Offset: 0x00058782
			// (set) Token: 0x06002323 RID: 8995 RVA: 0x0005978F File Offset: 0x0005878F
			public int Era
			{
				get
				{
					return this.EraInfo.era;
				}
				set
				{
					this.EraInfo.era = value;
				}
			}

			// Token: 0x17000635 RID: 1589
			// (get) Token: 0x06002324 RID: 8996 RVA: 0x0005979D File Offset: 0x0005879D
			public long Ticks
			{
				get
				{
					return this.EraInfo.ticks;
				}
			}

			// Token: 0x17000636 RID: 1590
			// (get) Token: 0x06002325 RID: 8997 RVA: 0x000597AA File Offset: 0x000587AA
			public int YearOffset
			{
				get
				{
					return this.EraInfo.yearOffset;
				}
			}

			// Token: 0x17000637 RID: 1591
			// (get) Token: 0x06002326 RID: 8998 RVA: 0x000597B7 File Offset: 0x000587B7
			public int MinEraYear
			{
				get
				{
					return this.EraInfo.minEraYear;
				}
			}

			// Token: 0x17000638 RID: 1592
			// (get) Token: 0x06002327 RID: 8999 RVA: 0x000597C4 File Offset: 0x000587C4
			// (set) Token: 0x06002328 RID: 9000 RVA: 0x000597D1 File Offset: 0x000587D1
			public int MaxEraYear
			{
				get
				{
					return this.EraInfo.maxEraYear;
				}
				set
				{
					this.EraInfo.maxEraYear = value;
				}
			}

			// Token: 0x06002329 RID: 9001 RVA: 0x000597DF File Offset: 0x000587DF
			internal ExtendedEraInfo(int era, long ticks, int yearOffset, int minEraYear, int maxEraYear, string eraName, string abbrevEraName, string englishEraName)
			{
				this.EraInfo = new EraInfo(era, ticks, yearOffset, minEraYear, maxEraYear);
				this.EraName = eraName;
				this.AbbrevEraName = abbrevEraName;
				this.EnglishEraName = englishEraName;
			}

			// Token: 0x04000EC4 RID: 3780
			public EraInfo EraInfo;

			// Token: 0x04000EC5 RID: 3781
			public string EraName;

			// Token: 0x04000EC6 RID: 3782
			public string AbbrevEraName;

			// Token: 0x04000EC7 RID: 3783
			public string EnglishEraName;
		}
	}
}
