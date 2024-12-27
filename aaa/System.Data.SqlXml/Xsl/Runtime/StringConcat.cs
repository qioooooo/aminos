using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200008A RID: 138
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct StringConcat
	{
		// Token: 0x0600073A RID: 1850 RVA: 0x00025D65 File Offset: 0x00024D65
		public void Clear()
		{
			this.idxStr = 0;
			this.delimiter = null;
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00025D75 File Offset: 0x00024D75
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x00025D7D File Offset: 0x00024D7D
		public string Delimiter
		{
			get
			{
				return this.delimiter;
			}
			set
			{
				this.delimiter = value;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00025D86 File Offset: 0x00024D86
		internal int Count
		{
			get
			{
				return this.idxStr;
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00025D8E File Offset: 0x00024D8E
		public void Concat(string value)
		{
			if (this.delimiter != null && this.idxStr != 0)
			{
				this.ConcatNoDelimiter(this.delimiter);
			}
			this.ConcatNoDelimiter(value);
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00025DB4 File Offset: 0x00024DB4
		public string GetResult()
		{
			switch (this.idxStr)
			{
			case 0:
				return string.Empty;
			case 1:
				return this.s1;
			case 2:
				return this.s1 + this.s2;
			case 3:
				return this.s1 + this.s2 + this.s3;
			case 4:
				return this.s1 + this.s2 + this.s3 + this.s4;
			default:
				return string.Concat(this.strList.ToArray());
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x00025E4C File Offset: 0x00024E4C
		internal void ConcatNoDelimiter(string s)
		{
			switch (this.idxStr)
			{
			case 0:
				this.s1 = s;
				goto IL_00AA;
			case 1:
				this.s2 = s;
				goto IL_00AA;
			case 2:
				this.s3 = s;
				goto IL_00AA;
			case 3:
				this.s4 = s;
				goto IL_00AA;
			case 4:
			{
				int num = ((this.strList == null) ? 8 : this.strList.Count);
				List<string> list = (this.strList = new List<string>(num));
				list.Add(this.s1);
				list.Add(this.s2);
				list.Add(this.s3);
				list.Add(this.s4);
				break;
			}
			}
			this.strList.Add(s);
			IL_00AA:
			this.idxStr++;
		}

		// Token: 0x040004CB RID: 1227
		private string s1;

		// Token: 0x040004CC RID: 1228
		private string s2;

		// Token: 0x040004CD RID: 1229
		private string s3;

		// Token: 0x040004CE RID: 1230
		private string s4;

		// Token: 0x040004CF RID: 1231
		private string delimiter;

		// Token: 0x040004D0 RID: 1232
		private List<string> strList;

		// Token: 0x040004D1 RID: 1233
		private int idxStr;
	}
}
