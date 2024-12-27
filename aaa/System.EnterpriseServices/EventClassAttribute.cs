using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000073 RID: 115
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class EventClassAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000278 RID: 632 RVA: 0x00006D2B File Offset: 0x00005D2B
		public EventClassAttribute()
		{
			this._fireInParallel = false;
			this._allowInprocSubscribers = true;
			this._filter = null;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000279 RID: 633 RVA: 0x00006D48 File Offset: 0x00005D48
		// (set) Token: 0x0600027A RID: 634 RVA: 0x00006D50 File Offset: 0x00005D50
		public bool FireInParallel
		{
			get
			{
				return this._fireInParallel;
			}
			set
			{
				this._fireInParallel = value;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600027B RID: 635 RVA: 0x00006D59 File Offset: 0x00005D59
		// (set) Token: 0x0600027C RID: 636 RVA: 0x00006D61 File Offset: 0x00005D61
		public bool AllowInprocSubscribers
		{
			get
			{
				return this._allowInprocSubscribers;
			}
			set
			{
				this._allowInprocSubscribers = value;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600027D RID: 637 RVA: 0x00006D6A File Offset: 0x00005D6A
		// (set) Token: 0x0600027E RID: 638 RVA: 0x00006D72 File Offset: 0x00005D72
		public string PublisherFilter
		{
			get
			{
				return this._filter;
			}
			set
			{
				this._filter = value;
			}
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00006D7B File Offset: 0x00005D7B
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00006D88 File Offset: 0x00005D88
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "EventClassAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("FireInParallel", this._fireInParallel);
			catalogObject.SetValue("AllowInprocSubscribers", this._allowInprocSubscribers);
			if (this._filter != null)
			{
				catalogObject.SetValue("MultiInterfacePublisherFilterCLSID", this._filter);
			}
			return true;
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00006DFB File Offset: 0x00005DFB
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000FF RID: 255
		private bool _fireInParallel;

		// Token: 0x04000100 RID: 256
		private bool _allowInprocSubscribers;

		// Token: 0x04000101 RID: 257
		private string _filter;
	}
}
