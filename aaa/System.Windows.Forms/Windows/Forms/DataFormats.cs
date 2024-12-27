using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020002BF RID: 703
	public class DataFormats
	{
		// Token: 0x060026F7 RID: 9975 RVA: 0x0005FE10 File Offset: 0x0005EE10
		private DataFormats()
		{
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x0005FE18 File Offset: 0x0005EE18
		public static DataFormats.Format GetFormat(string format)
		{
			DataFormats.Format format2;
			lock (DataFormats.internalSyncObject)
			{
				DataFormats.EnsurePredefined();
				for (int i = 0; i < DataFormats.formatCount; i++)
				{
					if (DataFormats.formatList[i].Name.Equals(format))
					{
						return DataFormats.formatList[i];
					}
				}
				for (int j = 0; j < DataFormats.formatCount; j++)
				{
					if (string.Equals(DataFormats.formatList[j].Name, format, StringComparison.OrdinalIgnoreCase))
					{
						return DataFormats.formatList[j];
					}
				}
				int num = SafeNativeMethods.RegisterClipboardFormat(format);
				if (num == 0)
				{
					throw new Win32Exception(Marshal.GetLastWin32Error(), SR.GetString("RegisterCFFailed"));
				}
				DataFormats.EnsureFormatSpace(1);
				DataFormats.formatList[DataFormats.formatCount] = new DataFormats.Format(format, num);
				format2 = DataFormats.formatList[DataFormats.formatCount++];
			}
			return format2;
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x0005FF00 File Offset: 0x0005EF00
		public static DataFormats.Format GetFormat(int id)
		{
			return DataFormats.InternalGetFormat(null, id);
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x0005FF0C File Offset: 0x0005EF0C
		private static DataFormats.Format InternalGetFormat(string strName, int id)
		{
			DataFormats.Format format;
			lock (DataFormats.internalSyncObject)
			{
				DataFormats.EnsurePredefined();
				for (int i = 0; i < DataFormats.formatCount; i++)
				{
					if ((int)((short)DataFormats.formatList[i].Id) == id)
					{
						return DataFormats.formatList[i];
					}
				}
				StringBuilder stringBuilder = new StringBuilder(128);
				if (SafeNativeMethods.GetClipboardFormatName(id, stringBuilder, stringBuilder.Capacity) == 0)
				{
					stringBuilder.Length = 0;
					if (strName == null)
					{
						stringBuilder.Append("Format").Append(id);
					}
					else
					{
						stringBuilder.Append(strName);
					}
				}
				DataFormats.EnsureFormatSpace(1);
				DataFormats.formatList[DataFormats.formatCount] = new DataFormats.Format(stringBuilder.ToString(), id);
				format = DataFormats.formatList[DataFormats.formatCount++];
			}
			return format;
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x0005FFE4 File Offset: 0x0005EFE4
		private static void EnsureFormatSpace(int size)
		{
			if (DataFormats.formatList == null || DataFormats.formatList.Length <= DataFormats.formatCount + size)
			{
				int num = DataFormats.formatCount + 20;
				DataFormats.Format[] array = new DataFormats.Format[num];
				for (int i = 0; i < DataFormats.formatCount; i++)
				{
					array[i] = DataFormats.formatList[i];
				}
				DataFormats.formatList = array;
			}
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x00060038 File Offset: 0x0005F038
		private static void EnsurePredefined()
		{
			if (DataFormats.formatCount == 0)
			{
				DataFormats.formatList = new DataFormats.Format[]
				{
					new DataFormats.Format(DataFormats.UnicodeText, 13),
					new DataFormats.Format(DataFormats.Text, 1),
					new DataFormats.Format(DataFormats.Bitmap, 2),
					new DataFormats.Format(DataFormats.MetafilePict, 3),
					new DataFormats.Format(DataFormats.EnhancedMetafile, 14),
					new DataFormats.Format(DataFormats.Dif, 5),
					new DataFormats.Format(DataFormats.Tiff, 6),
					new DataFormats.Format(DataFormats.OemText, 7),
					new DataFormats.Format(DataFormats.Dib, 8),
					new DataFormats.Format(DataFormats.Palette, 9),
					new DataFormats.Format(DataFormats.PenData, 10),
					new DataFormats.Format(DataFormats.Riff, 11),
					new DataFormats.Format(DataFormats.WaveAudio, 12),
					new DataFormats.Format(DataFormats.SymbolicLink, 4),
					new DataFormats.Format(DataFormats.FileDrop, 15),
					new DataFormats.Format(DataFormats.Locale, 16)
				};
				DataFormats.formatCount = DataFormats.formatList.Length;
			}
		}

		// Token: 0x0400167B RID: 5755
		public static readonly string Text = "Text";

		// Token: 0x0400167C RID: 5756
		public static readonly string UnicodeText = "UnicodeText";

		// Token: 0x0400167D RID: 5757
		public static readonly string Dib = "DeviceIndependentBitmap";

		// Token: 0x0400167E RID: 5758
		public static readonly string Bitmap = "Bitmap";

		// Token: 0x0400167F RID: 5759
		public static readonly string EnhancedMetafile = "EnhancedMetafile";

		// Token: 0x04001680 RID: 5760
		public static readonly string MetafilePict = "MetaFilePict";

		// Token: 0x04001681 RID: 5761
		public static readonly string SymbolicLink = "SymbolicLink";

		// Token: 0x04001682 RID: 5762
		public static readonly string Dif = "DataInterchangeFormat";

		// Token: 0x04001683 RID: 5763
		public static readonly string Tiff = "TaggedImageFileFormat";

		// Token: 0x04001684 RID: 5764
		public static readonly string OemText = "OEMText";

		// Token: 0x04001685 RID: 5765
		public static readonly string Palette = "Palette";

		// Token: 0x04001686 RID: 5766
		public static readonly string PenData = "PenData";

		// Token: 0x04001687 RID: 5767
		public static readonly string Riff = "RiffAudio";

		// Token: 0x04001688 RID: 5768
		public static readonly string WaveAudio = "WaveAudio";

		// Token: 0x04001689 RID: 5769
		public static readonly string FileDrop = "FileDrop";

		// Token: 0x0400168A RID: 5770
		public static readonly string Locale = "Locale";

		// Token: 0x0400168B RID: 5771
		public static readonly string Html = "HTML Format";

		// Token: 0x0400168C RID: 5772
		public static readonly string Rtf = "Rich Text Format";

		// Token: 0x0400168D RID: 5773
		public static readonly string CommaSeparatedValue = "Csv";

		// Token: 0x0400168E RID: 5774
		public static readonly string StringFormat = typeof(string).FullName;

		// Token: 0x0400168F RID: 5775
		public static readonly string Serializable = Application.WindowsFormsVersion + "PersistentObject";

		// Token: 0x04001690 RID: 5776
		private static DataFormats.Format[] formatList;

		// Token: 0x04001691 RID: 5777
		private static int formatCount = 0;

		// Token: 0x04001692 RID: 5778
		private static object internalSyncObject = new object();

		// Token: 0x020002C0 RID: 704
		public class Format
		{
			// Token: 0x1700063D RID: 1597
			// (get) Token: 0x060026FE RID: 9982 RVA: 0x0006025B File Offset: 0x0005F25B
			public string Name
			{
				get
				{
					return this.name;
				}
			}

			// Token: 0x1700063E RID: 1598
			// (get) Token: 0x060026FF RID: 9983 RVA: 0x00060263 File Offset: 0x0005F263
			public int Id
			{
				get
				{
					return this.id;
				}
			}

			// Token: 0x06002700 RID: 9984 RVA: 0x0006026B File Offset: 0x0005F26B
			public Format(string name, int id)
			{
				this.name = name;
				this.id = id;
			}

			// Token: 0x04001693 RID: 5779
			private readonly string name;

			// Token: 0x04001694 RID: 5780
			private readonly int id;
		}
	}
}
