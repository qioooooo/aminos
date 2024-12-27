using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000079 RID: 121
	[AttributeUsage(AttributeTargets.Assembly, Inherited = true)]
	[ComVisible(false)]
	public sealed class ApplicationQueuingAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600029F RID: 671 RVA: 0x000070DD File Offset: 0x000060DD
		public ApplicationQueuingAttribute()
		{
			this._enabled = true;
			this._listen = false;
			this._maxthreads = 0;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x000070FA File Offset: 0x000060FA
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x00007102 File Offset: 0x00006102
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				this._enabled = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000710B File Offset: 0x0000610B
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x00007113 File Offset: 0x00006113
		public bool QueueListenerEnabled
		{
			get
			{
				return this._listen;
			}
			set
			{
				this._listen = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000711C File Offset: 0x0000611C
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x00007124 File Offset: 0x00006124
		public int MaxListenerThreads
		{
			get
			{
				return this._maxthreads;
			}
			set
			{
				this._maxthreads = value;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000712D File Offset: 0x0000612D
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Application";
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000713C File Offset: 0x0000613C
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "ApplicationQueueingAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Application"];
			catalogObject.SetValue("QueuingEnabled", this._enabled);
			catalogObject.SetValue("QueueListenerEnabled", this._listen);
			if (this._maxthreads != 0)
			{
				Platform.Assert(Platform.Whistler, "ApplicationQueuingAttribute.MaxListenerThreads");
				catalogObject.SetValue("QCListenerMaxThreads", this._maxthreads);
			}
			return true;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x000071C3 File Offset: 0x000061C3
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x04000108 RID: 264
		private bool _enabled;

		// Token: 0x04000109 RID: 265
		private bool _listen;

		// Token: 0x0400010A RID: 266
		private int _maxthreads;
	}
}
