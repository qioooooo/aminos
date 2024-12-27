using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200006D RID: 109
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class ObjectPoolingAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600024C RID: 588 RVA: 0x0000696A File Offset: 0x0000596A
		public ObjectPoolingAttribute()
		{
			this._enable = true;
			this._maxSize = -1;
			this._minSize = -1;
			this._timeout = -1;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000698E File Offset: 0x0000598E
		public ObjectPoolingAttribute(int minPoolSize, int maxPoolSize)
		{
			this._enable = true;
			this._maxSize = maxPoolSize;
			this._minSize = minPoolSize;
			this._timeout = -1;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x000069B2 File Offset: 0x000059B2
		public ObjectPoolingAttribute(bool enable)
		{
			this._enable = enable;
			this._maxSize = -1;
			this._minSize = -1;
			this._timeout = -1;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x000069D6 File Offset: 0x000059D6
		public ObjectPoolingAttribute(bool enable, int minPoolSize, int maxPoolSize)
		{
			this._enable = enable;
			this._maxSize = maxPoolSize;
			this._minSize = minPoolSize;
			this._timeout = -1;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000250 RID: 592 RVA: 0x000069FA File Offset: 0x000059FA
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00006A02 File Offset: 0x00005A02
		public bool Enabled
		{
			get
			{
				return this._enable;
			}
			set
			{
				this._enable = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00006A0B File Offset: 0x00005A0B
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00006A13 File Offset: 0x00005A13
		public int MaxPoolSize
		{
			get
			{
				return this._maxSize;
			}
			set
			{
				this._maxSize = value;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00006A1C File Offset: 0x00005A1C
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00006A24 File Offset: 0x00005A24
		public int MinPoolSize
		{
			get
			{
				return this._minSize;
			}
			set
			{
				this._minSize = value;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00006A2D File Offset: 0x00005A2D
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00006A35 File Offset: 0x00005A35
		public int CreationTimeout
		{
			get
			{
				return this._timeout;
			}
			set
			{
				this._timeout = value;
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00006A3E File Offset: 0x00005A3E
		public bool IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00006A4C File Offset: 0x00005A4C
		public bool Apply(Hashtable info)
		{
			Platform.Assert(Platform.W2K, "ObjectPoolingAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("ObjectPoolingEnabled", this._enable);
			if (this._minSize >= 0)
			{
				catalogObject.SetValue("MinPoolSize", this._minSize);
			}
			if (this._maxSize >= 0)
			{
				catalogObject.SetValue("MaxPoolSize", this._maxSize);
			}
			if (this._timeout >= 0)
			{
				catalogObject.SetValue("CreationTimeout", this._timeout);
			}
			return true;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00006AED File Offset: 0x00005AED
		public bool AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000F6 RID: 246
		private bool _enable;

		// Token: 0x040000F7 RID: 247
		private int _maxSize;

		// Token: 0x040000F8 RID: 248
		private int _minSize;

		// Token: 0x040000F9 RID: 249
		private int _timeout;
	}
}
