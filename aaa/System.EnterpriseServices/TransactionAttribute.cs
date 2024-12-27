using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000068 RID: 104
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class TransactionAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000227 RID: 551 RVA: 0x000065A0 File Offset: 0x000055A0
		public TransactionAttribute()
			: this(TransactionOption.Required)
		{
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000065A9 File Offset: 0x000055A9
		public TransactionAttribute(TransactionOption val)
		{
			this._value = val;
			this._isolation = TransactionIsolationLevel.Serializable;
			this._timeout = -1;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000229 RID: 553 RVA: 0x000065C6 File Offset: 0x000055C6
		public TransactionOption Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600022A RID: 554 RVA: 0x000065CE File Offset: 0x000055CE
		// (set) Token: 0x0600022B RID: 555 RVA: 0x000065D6 File Offset: 0x000055D6
		public TransactionIsolationLevel Isolation
		{
			get
			{
				return this._isolation;
			}
			set
			{
				this._isolation = value;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600022C RID: 556 RVA: 0x000065DF File Offset: 0x000055DF
		// (set) Token: 0x0600022D RID: 557 RVA: 0x000065E7 File Offset: 0x000055E7
		public int Timeout
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

		// Token: 0x0600022E RID: 558 RVA: 0x000065F0 File Offset: 0x000055F0
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00006600 File Offset: 0x00005600
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			object obj = this._value;
			Platform.Assert(Platform.MTS, "TransactionAttribute");
			if (Platform.IsLessThan(Platform.W2K))
			{
				switch (this._value)
				{
				case TransactionOption.Disabled:
					obj = "NotSupported";
					break;
				case TransactionOption.NotSupported:
					obj = "NotSupported";
					break;
				case TransactionOption.Supported:
					obj = "Supported";
					break;
				case TransactionOption.Required:
					obj = "Required";
					break;
				case TransactionOption.RequiresNew:
					obj = "Requires New";
					break;
				}
			}
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("Transaction", obj);
			if (this._isolation != TransactionIsolationLevel.Serializable)
			{
				Platform.Assert(Platform.Whistler, "TransactionAttribute.Isolation");
				catalogObject.SetValue("TxIsolationLevel", this._isolation);
			}
			if (this._timeout != -1)
			{
				Platform.Assert(Platform.W2K, "TransactionAttribute.Timeout");
				catalogObject.SetValue("ComponentTransactionTimeout", this._timeout);
				catalogObject.SetValue("ComponentTransactionTimeoutEnabled", true);
			}
			return true;
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00006708 File Offset: 0x00005708
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}

		// Token: 0x040000EE RID: 238
		private TransactionOption _value;

		// Token: 0x040000EF RID: 239
		private TransactionIsolationLevel _isolation;

		// Token: 0x040000F0 RID: 240
		private int _timeout;
	}
}
