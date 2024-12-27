using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000089 RID: 137
	[Guid("36dcda30-dc3b-4d93-be42-90b2d74c64e7")]
	[Serializable]
	public class RegistrationConfig
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000A431 File Offset: 0x00009431
		// (set) Token: 0x06000335 RID: 821 RVA: 0x0000A428 File Offset: 0x00009428
		public string AssemblyFile
		{
			get
			{
				return this._assmfile;
			}
			set
			{
				this._assmfile = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000A442 File Offset: 0x00009442
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0000A439 File Offset: 0x00009439
		public InstallationFlags InstallationFlags
		{
			get
			{
				return this._flags;
			}
			set
			{
				this._flags = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000A453 File Offset: 0x00009453
		// (set) Token: 0x06000339 RID: 825 RVA: 0x0000A44A File Offset: 0x0000944A
		public string Application
		{
			get
			{
				return this._application;
			}
			set
			{
				this._application = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000A464 File Offset: 0x00009464
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000A45B File Offset: 0x0000945B
		public string TypeLibrary
		{
			get
			{
				return this._typelib;
			}
			set
			{
				this._typelib = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000A475 File Offset: 0x00009475
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0000A46C File Offset: 0x0000946C
		public string Partition
		{
			get
			{
				return this._partition;
			}
			set
			{
				this._partition = value;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000A486 File Offset: 0x00009486
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0000A47D File Offset: 0x0000947D
		public string ApplicationRootDirectory
		{
			get
			{
				return this._approotdir;
			}
			set
			{
				this._approotdir = value;
			}
		}

		// Token: 0x04000140 RID: 320
		private string _assmfile;

		// Token: 0x04000141 RID: 321
		private InstallationFlags _flags;

		// Token: 0x04000142 RID: 322
		private string _application;

		// Token: 0x04000143 RID: 323
		private string _typelib;

		// Token: 0x04000144 RID: 324
		private string _partition;

		// Token: 0x04000145 RID: 325
		private string _approotdir;
	}
}
