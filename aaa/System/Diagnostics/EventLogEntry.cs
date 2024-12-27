using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000750 RID: 1872
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	[Serializable]
	public sealed class EventLogEntry : Component, ISerializable
	{
		// Token: 0x06003957 RID: 14679 RVA: 0x000F3792 File Offset: 0x000F2792
		internal EventLogEntry(byte[] buf, int offset, EventLog log)
		{
			this.dataBuf = buf;
			this.bufOffset = offset;
			this.owner = log;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x000F37B8 File Offset: 0x000F27B8
		private EventLogEntry(SerializationInfo info, StreamingContext context)
		{
			this.dataBuf = (byte[])info.GetValue("DataBuffer", typeof(byte[]));
			string @string = info.GetString("LogName");
			string string2 = info.GetString("MachineName");
			this.owner = new EventLog(@string, string2, "");
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06003959 RID: 14681 RVA: 0x000F381C File Offset: 0x000F281C
		[MonitoringDescription("LogEntryMachineName")]
		public string MachineName
		{
			get
			{
				int num = this.bufOffset + 56;
				while (this.CharFrom(this.dataBuf, num) != '\0')
				{
					num += 2;
				}
				num += 2;
				char c = this.CharFrom(this.dataBuf, num);
				StringBuilder stringBuilder = new StringBuilder();
				while (c != '\0')
				{
					stringBuilder.Append(c);
					num += 2;
					c = this.CharFrom(this.dataBuf, num);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x000F3888 File Offset: 0x000F2888
		[MonitoringDescription("LogEntryData")]
		public byte[] Data
		{
			get
			{
				int num = this.IntFrom(this.dataBuf, this.bufOffset + 48);
				byte[] array = new byte[num];
				Array.Copy(this.dataBuf, this.bufOffset + this.IntFrom(this.dataBuf, this.bufOffset + 52), array, 0, num);
				return array;
			}
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x000F38DD File Offset: 0x000F28DD
		[MonitoringDescription("LogEntryIndex")]
		public int Index
		{
			get
			{
				return this.IntFrom(this.dataBuf, this.bufOffset + 8);
			}
		}

		// Token: 0x17000D45 RID: 3397
		// (get) Token: 0x0600395C RID: 14684 RVA: 0x000F38F4 File Offset: 0x000F28F4
		[MonitoringDescription("LogEntryCategory")]
		public string Category
		{
			get
			{
				if (this.category == null)
				{
					string messageLibraryNames = this.GetMessageLibraryNames("CategoryMessageFile");
					string text = this.owner.FormatMessageWrapper(messageLibraryNames, (uint)this.CategoryNumber, null);
					if (text == null)
					{
						this.category = "(" + this.CategoryNumber.ToString(CultureInfo.CurrentCulture) + ")";
					}
					else
					{
						this.category = text;
					}
				}
				return this.category;
			}
		}

		// Token: 0x17000D46 RID: 3398
		// (get) Token: 0x0600395D RID: 14685 RVA: 0x000F3963 File Offset: 0x000F2963
		[MonitoringDescription("LogEntryCategoryNumber")]
		public short CategoryNumber
		{
			get
			{
				return this.ShortFrom(this.dataBuf, this.bufOffset + 28);
			}
		}

		// Token: 0x17000D47 RID: 3399
		// (get) Token: 0x0600395E RID: 14686 RVA: 0x000F397A File Offset: 0x000F297A
		[Obsolete("This property has been deprecated.  Please use System.Diagnostics.EventLogEntry.InstanceId instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		[MonitoringDescription("LogEntryEventID")]
		public int EventID
		{
			get
			{
				return this.IntFrom(this.dataBuf, this.bufOffset + 20) & 1073741823;
			}
		}

		// Token: 0x17000D48 RID: 3400
		// (get) Token: 0x0600395F RID: 14687 RVA: 0x000F3997 File Offset: 0x000F2997
		[MonitoringDescription("LogEntryEntryType")]
		public EventLogEntryType EntryType
		{
			get
			{
				return (EventLogEntryType)this.ShortFrom(this.dataBuf, this.bufOffset + 24);
			}
		}

		// Token: 0x17000D49 RID: 3401
		// (get) Token: 0x06003960 RID: 14688 RVA: 0x000F39B0 File Offset: 0x000F29B0
		[MonitoringDescription("LogEntryMessage")]
		[Editor("System.ComponentModel.Design.BinaryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Message
		{
			get
			{
				if (this.message == null)
				{
					string messageLibraryNames = this.GetMessageLibraryNames("EventMessageFile");
					int num = this.IntFrom(this.dataBuf, this.bufOffset + 20);
					string text = this.owner.FormatMessageWrapper(messageLibraryNames, (uint)num, this.ReplacementStrings);
					if (text == null)
					{
						StringBuilder stringBuilder = new StringBuilder(SR.GetString("MessageNotFormatted", new object[] { num, this.Source }));
						string[] replacementStrings = this.ReplacementStrings;
						for (int i = 0; i < replacementStrings.Length; i++)
						{
							if (i != 0)
							{
								stringBuilder.Append(", ");
							}
							stringBuilder.Append("'");
							stringBuilder.Append(replacementStrings[i]);
							stringBuilder.Append("'");
						}
						text = stringBuilder.ToString();
					}
					else
					{
						text = this.ReplaceMessageParameters(text, this.ReplacementStrings);
					}
					this.message = text;
				}
				return this.message;
			}
		}

		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06003961 RID: 14689 RVA: 0x000F3AA8 File Offset: 0x000F2AA8
		[MonitoringDescription("LogEntrySource")]
		public string Source
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = this.bufOffset + 56;
				for (char c = this.CharFrom(this.dataBuf, num); c != '\0'; c = this.CharFrom(this.dataBuf, num))
				{
					stringBuilder.Append(c);
					num += 2;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06003962 RID: 14690 RVA: 0x000F3AF8 File Offset: 0x000F2AF8
		[MonitoringDescription("LogEntryReplacementStrings")]
		public string[] ReplacementStrings
		{
			get
			{
				string[] array = new string[(int)this.ShortFrom(this.dataBuf, this.bufOffset + 26)];
				int i = 0;
				int num = this.bufOffset + this.IntFrom(this.dataBuf, this.bufOffset + 36);
				StringBuilder stringBuilder = new StringBuilder();
				while (i < array.Length)
				{
					char c = this.CharFrom(this.dataBuf, num);
					if (c != '\0')
					{
						stringBuilder.Append(c);
					}
					else
					{
						array[i] = stringBuilder.ToString();
						i++;
						stringBuilder = new StringBuilder();
					}
					num += 2;
				}
				return array;
			}
		}

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06003963 RID: 14691 RVA: 0x000F3B83 File Offset: 0x000F2B83
		[MonitoringDescription("LogEntryResourceId")]
		[ComVisible(false)]
		public long InstanceId
		{
			get
			{
				return (long)((ulong)this.IntFrom(this.dataBuf, this.bufOffset + 20));
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06003964 RID: 14692 RVA: 0x000F3B9C File Offset: 0x000F2B9C
		[MonitoringDescription("LogEntryTimeGenerated")]
		public DateTime TimeGenerated
		{
			get
			{
				return EventLogEntry.beginningOfTime.AddSeconds((double)this.IntFrom(this.dataBuf, this.bufOffset + 12)).ToLocalTime();
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x06003965 RID: 14693 RVA: 0x000F3BD4 File Offset: 0x000F2BD4
		[MonitoringDescription("LogEntryTimeWritten")]
		public DateTime TimeWritten
		{
			get
			{
				return EventLogEntry.beginningOfTime.AddSeconds((double)this.IntFrom(this.dataBuf, this.bufOffset + 16)).ToLocalTime();
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x06003966 RID: 14694 RVA: 0x000F3C0C File Offset: 0x000F2C0C
		[MonitoringDescription("LogEntryUserName")]
		public string UserName
		{
			get
			{
				int num = this.IntFrom(this.dataBuf, this.bufOffset + 40);
				if (num == 0)
				{
					return null;
				}
				byte[] array = new byte[num];
				Array.Copy(this.dataBuf, this.bufOffset + this.IntFrom(this.dataBuf, this.bufOffset + 44), array, 0, array.Length);
				int[] array2 = new int[1];
				char[] array3 = new char[1024];
				char[] array4 = new char[1024];
				int[] array5 = new int[] { 1024 };
				int[] array6 = new int[] { 1024 };
				if (!UnsafeNativeMethods.LookupAccountSid(this.MachineName, array, array3, array5, array4, array6, array2))
				{
					return "";
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(array4, 0, array6[0]);
				stringBuilder.Append("\\");
				stringBuilder.Append(array3, 0, array5[0]);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x000F3D07 File Offset: 0x000F2D07
		private char CharFrom(byte[] buf, int offset)
		{
			return (char)this.ShortFrom(buf, offset);
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x000F3D14 File Offset: 0x000F2D14
		public bool Equals(EventLogEntry otherEntry)
		{
			if (otherEntry == null)
			{
				return false;
			}
			int num = this.IntFrom(this.dataBuf, this.bufOffset);
			int num2 = this.IntFrom(otherEntry.dataBuf, otherEntry.bufOffset);
			if (num != num2)
			{
				return false;
			}
			int num3 = this.bufOffset;
			int num4 = this.bufOffset + num;
			int num5 = otherEntry.bufOffset;
			int i = num3;
			while (i < num4)
			{
				if (this.dataBuf[i] != otherEntry.dataBuf[num5])
				{
					return false;
				}
				i++;
				num5++;
			}
			return true;
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x000F3D97 File Offset: 0x000F2D97
		private int IntFrom(byte[] buf, int offset)
		{
			return (-16777216 & ((int)buf[offset + 3] << 24)) | (16711680 & ((int)buf[offset + 2] << 16)) | (65280 & ((int)buf[offset + 1] << 8)) | (int)(byte.MaxValue & buf[offset]);
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x000F3DD0 File Offset: 0x000F2DD0
		internal string ReplaceMessageParameters(string msg, string[] insertionStrings)
		{
			int i = msg.IndexOf('%');
			if (i < 0)
			{
				return msg;
			}
			int num = 0;
			int length = msg.Length;
			StringBuilder stringBuilder = new StringBuilder();
			string messageLibraryNames = this.GetMessageLibraryNames("ParameterMessageFile");
			while (i >= 0)
			{
				string text = null;
				int num2 = i + 1;
				while (num2 < length && char.IsDigit(msg, num2))
				{
					num2++;
				}
				uint num3 = 0U;
				if (num2 != i + 1)
				{
					uint.TryParse(msg.Substring(i + 1, num2 - i - 1), out num3);
				}
				if (num3 != 0U)
				{
					text = this.owner.FormatMessageWrapper(messageLibraryNames, num3, insertionStrings);
				}
				if (text != null)
				{
					if (i > num)
					{
						stringBuilder.Append(msg, num, i - num);
					}
					stringBuilder.Append(text);
					num = num2;
				}
				i = msg.IndexOf('%', i + 1);
			}
			if (length - num > 0)
			{
				stringBuilder.Append(msg, num, length - num);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x000F3EB0 File Offset: 0x000F2EB0
		private static RegistryKey GetSourceRegKey(string logName, string source, string machineName)
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryKey registryKey3;
			try
			{
				registryKey = EventLog.GetEventLogRegKey(machineName, false);
				if (registryKey == null)
				{
					registryKey3 = null;
				}
				else
				{
					if (logName == null)
					{
						registryKey2 = registryKey.OpenSubKey("Application", false);
					}
					else
					{
						registryKey2 = registryKey.OpenSubKey(logName, false);
					}
					if (registryKey2 == null)
					{
						registryKey3 = null;
					}
					else
					{
						registryKey3 = registryKey2.OpenSubKey(source, false);
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
			}
			return registryKey3;
		}

		// Token: 0x0600396C RID: 14700 RVA: 0x000F3F24 File Offset: 0x000F2F24
		private string GetMessageLibraryNames(string libRegKey)
		{
			string text = null;
			RegistryKey registryKey = null;
			try
			{
				registryKey = EventLogEntry.GetSourceRegKey(this.owner.Log, this.Source, this.owner.MachineName);
				if (registryKey != null)
				{
					if (this.owner.MachineName == ".")
					{
						text = (string)registryKey.GetValue(libRegKey);
					}
					else
					{
						text = (string)registryKey.GetValue(libRegKey, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			if (text == null)
			{
				return null;
			}
			if (!(this.owner.MachineName != "."))
			{
				return text;
			}
			if (text.EndsWith("EventLogMessages.dll", StringComparison.Ordinal))
			{
				return EventLog.GetDllPath(".");
			}
			if (string.Compare(text, 0, "%systemroot%", 0, 12, StringComparison.OrdinalIgnoreCase) == 0)
			{
				StringBuilder stringBuilder = new StringBuilder(text.Length + this.owner.MachineName.Length - 3);
				stringBuilder.Append("\\\\");
				stringBuilder.Append(this.owner.MachineName);
				stringBuilder.Append("\\admin$");
				stringBuilder.Append(text, 12, text.Length - 12);
				return stringBuilder.ToString();
			}
			if (text[1] == ':')
			{
				StringBuilder stringBuilder2 = new StringBuilder(text.Length + this.owner.MachineName.Length + 3);
				stringBuilder2.Append("\\\\");
				stringBuilder2.Append(this.owner.MachineName);
				stringBuilder2.Append("\\");
				stringBuilder2.Append(text[0]);
				stringBuilder2.Append("$");
				stringBuilder2.Append(text, 2, text.Length - 2);
				return stringBuilder2.ToString();
			}
			return null;
		}

		// Token: 0x0600396D RID: 14701 RVA: 0x000F40E4 File Offset: 0x000F30E4
		private short ShortFrom(byte[] buf, int offset)
		{
			return (short)((65280 & ((int)buf[offset + 1] << 8)) | (int)(byte.MaxValue & buf[offset]));
		}

		// Token: 0x0600396E RID: 14702 RVA: 0x000F4100 File Offset: 0x000F3100
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			int num = this.IntFrom(this.dataBuf, this.bufOffset);
			byte[] array = new byte[num];
			Array.Copy(this.dataBuf, this.bufOffset, array, 0, num);
			info.AddValue("DataBuffer", array, typeof(byte[]));
			info.AddValue("LogName", this.owner.Log);
			info.AddValue("MachineName", this.owner.MachineName);
		}

		// Token: 0x04003295 RID: 12949
		private const int OFFSETFIXUP = 56;

		// Token: 0x04003296 RID: 12950
		internal byte[] dataBuf;

		// Token: 0x04003297 RID: 12951
		internal int bufOffset;

		// Token: 0x04003298 RID: 12952
		private EventLog owner;

		// Token: 0x04003299 RID: 12953
		private string category;

		// Token: 0x0400329A RID: 12954
		private string message;

		// Token: 0x0400329B RID: 12955
		private static readonly DateTime beginningOfTime = new DateTime(1970, 1, 1, 0, 0, 0);

		// Token: 0x02000751 RID: 1873
		private static class FieldOffsets
		{
			// Token: 0x0400329C RID: 12956
			internal const int LENGTH = 0;

			// Token: 0x0400329D RID: 12957
			internal const int RESERVED = 4;

			// Token: 0x0400329E RID: 12958
			internal const int RECORDNUMBER = 8;

			// Token: 0x0400329F RID: 12959
			internal const int TIMEGENERATED = 12;

			// Token: 0x040032A0 RID: 12960
			internal const int TIMEWRITTEN = 16;

			// Token: 0x040032A1 RID: 12961
			internal const int EVENTID = 20;

			// Token: 0x040032A2 RID: 12962
			internal const int EVENTTYPE = 24;

			// Token: 0x040032A3 RID: 12963
			internal const int NUMSTRINGS = 26;

			// Token: 0x040032A4 RID: 12964
			internal const int EVENTCATEGORY = 28;

			// Token: 0x040032A5 RID: 12965
			internal const int RESERVEDFLAGS = 30;

			// Token: 0x040032A6 RID: 12966
			internal const int CLOSINGRECORDNUMBER = 32;

			// Token: 0x040032A7 RID: 12967
			internal const int STRINGOFFSET = 36;

			// Token: 0x040032A8 RID: 12968
			internal const int USERSIDLENGTH = 40;

			// Token: 0x040032A9 RID: 12969
			internal const int USERSIDOFFSET = 44;

			// Token: 0x040032AA RID: 12970
			internal const int DATALENGTH = 48;

			// Token: 0x040032AB RID: 12971
			internal const int DATAOFFSET = 52;

			// Token: 0x040032AC RID: 12972
			internal const int RAWDATA = 56;
		}
	}
}
