using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000026 RID: 38
	public class DirSyncResponseControl : DirectoryControl
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x000046AF File Offset: 0x000036AF
		internal DirSyncResponseControl(byte[] cookie, bool moreData, int resultSize, bool criticality, byte[] controlValue)
			: base("1.2.840.113556.1.4.841", controlValue, criticality, true)
		{
			this.dirsyncCookie = cookie;
			this.moreResult = moreData;
			this.size = resultSize;
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000046D8 File Offset: 0x000036D8
		public byte[] Cookie
		{
			get
			{
				if (this.dirsyncCookie == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.dirsyncCookie.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.dirsyncCookie[i];
				}
				return array;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000471C File Offset: 0x0000371C
		public bool MoreData
		{
			get
			{
				return this.moreResult;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004724 File Offset: 0x00003724
		public int ResultSize
		{
			get
			{
				return this.size;
			}
		}

		// Token: 0x040000EC RID: 236
		private byte[] dirsyncCookie;

		// Token: 0x040000ED RID: 237
		private bool moreResult;

		// Token: 0x040000EE RID: 238
		private int size;
	}
}
