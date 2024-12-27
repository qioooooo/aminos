using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200008C RID: 140
	public class AttributeMetadata
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x00017E98 File Offset: 0x00016E98
		internal AttributeMetadata(IntPtr info, bool advanced, DirectoryServer server, Hashtable table)
		{
			if (advanced)
			{
				DS_REPL_ATTR_META_DATA_2 ds_REPL_ATTR_META_DATA_ = new DS_REPL_ATTR_META_DATA_2();
				Marshal.PtrToStructure(info, ds_REPL_ATTR_META_DATA_);
				this.pszAttributeName = Marshal.PtrToStringUni(ds_REPL_ATTR_META_DATA_.pszAttributeName);
				this.dwVersion = ds_REPL_ATTR_META_DATA_.dwVersion;
				long num = (long)((ulong)ds_REPL_ATTR_META_DATA_.ftimeLastOriginatingChange1 + (ulong)((ulong)((long)ds_REPL_ATTR_META_DATA_.ftimeLastOriginatingChange2) << 32));
				this.ftimeLastOriginatingChange = DateTime.FromFileTime(num);
				this.uuidLastOriginatingDsaInvocationID = ds_REPL_ATTR_META_DATA_.uuidLastOriginatingDsaInvocationID;
				this.usnOriginatingChange = ds_REPL_ATTR_META_DATA_.usnOriginatingChange;
				this.usnLocalChange = ds_REPL_ATTR_META_DATA_.usnLocalChange;
				this.pszLastOriginatingDsaDN = Marshal.PtrToStringUni(ds_REPL_ATTR_META_DATA_.pszLastOriginatingDsaDN);
			}
			else
			{
				DS_REPL_ATTR_META_DATA ds_REPL_ATTR_META_DATA = new DS_REPL_ATTR_META_DATA();
				Marshal.PtrToStructure(info, ds_REPL_ATTR_META_DATA);
				this.pszAttributeName = Marshal.PtrToStringUni(ds_REPL_ATTR_META_DATA.pszAttributeName);
				this.dwVersion = ds_REPL_ATTR_META_DATA.dwVersion;
				long num2 = (long)((ulong)ds_REPL_ATTR_META_DATA.ftimeLastOriginatingChange1 + (ulong)((ulong)((long)ds_REPL_ATTR_META_DATA.ftimeLastOriginatingChange2) << 32));
				this.ftimeLastOriginatingChange = DateTime.FromFileTime(num2);
				this.uuidLastOriginatingDsaInvocationID = ds_REPL_ATTR_META_DATA.uuidLastOriginatingDsaInvocationID;
				this.usnOriginatingChange = ds_REPL_ATTR_META_DATA.usnOriginatingChange;
				this.usnLocalChange = ds_REPL_ATTR_META_DATA.usnLocalChange;
			}
			this.server = server;
			this.nameTable = table;
			this.advanced = advanced;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00017FB4 File Offset: 0x00016FB4
		public string Name
		{
			get
			{
				return this.pszAttributeName;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x00017FBC File Offset: 0x00016FBC
		public int Version
		{
			get
			{
				return this.dwVersion;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x00017FC4 File Offset: 0x00016FC4
		public DateTime LastOriginatingChangeTime
		{
			get
			{
				return this.ftimeLastOriginatingChange;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00017FCC File Offset: 0x00016FCC
		public Guid LastOriginatingInvocationId
		{
			get
			{
				return this.uuidLastOriginatingDsaInvocationID;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x00017FD4 File Offset: 0x00016FD4
		public long OriginatingChangeUsn
		{
			get
			{
				return this.usnOriginatingChange;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x00017FDC File Offset: 0x00016FDC
		public long LocalChangeUsn
		{
			get
			{
				return this.usnLocalChange;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000452 RID: 1106 RVA: 0x00017FE4 File Offset: 0x00016FE4
		public string OriginatingServer
		{
			get
			{
				if (this.originatingServerName == null)
				{
					if (this.nameTable.Contains(this.LastOriginatingInvocationId))
					{
						this.originatingServerName = (string)this.nameTable[this.LastOriginatingInvocationId];
					}
					else if (!this.advanced || (this.advanced && this.pszLastOriginatingDsaDN != null))
					{
						this.originatingServerName = Utils.GetServerNameFromInvocationID(this.pszLastOriginatingDsaDN, this.LastOriginatingInvocationId, this.server);
						this.nameTable.Add(this.LastOriginatingInvocationId, this.originatingServerName);
					}
				}
				return this.originatingServerName;
			}
		}

		// Token: 0x040003C8 RID: 968
		private string pszAttributeName;

		// Token: 0x040003C9 RID: 969
		private int dwVersion;

		// Token: 0x040003CA RID: 970
		private DateTime ftimeLastOriginatingChange;

		// Token: 0x040003CB RID: 971
		private Guid uuidLastOriginatingDsaInvocationID;

		// Token: 0x040003CC RID: 972
		private long usnOriginatingChange;

		// Token: 0x040003CD RID: 973
		private long usnLocalChange;

		// Token: 0x040003CE RID: 974
		private string pszLastOriginatingDsaDN;

		// Token: 0x040003CF RID: 975
		private string originatingServerName;

		// Token: 0x040003D0 RID: 976
		private DirectoryServer server;

		// Token: 0x040003D1 RID: 977
		private Hashtable nameTable;

		// Token: 0x040003D2 RID: 978
		private bool advanced;
	}
}
