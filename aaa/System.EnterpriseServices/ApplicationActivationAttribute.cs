using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000076 RID: 118
	[AttributeUsage(AttributeTargets.Assembly, Inherited = true)]
	[ComVisible(false)]
	public sealed class ApplicationActivationAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x0600028C RID: 652 RVA: 0x00006ECB File Offset: 0x00005ECB
		public ApplicationActivationAttribute(ActivationOption opt)
		{
			this._value = opt;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600028D RID: 653 RVA: 0x00006EDA File Offset: 0x00005EDA
		public ActivationOption Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00006EE2 File Offset: 0x00005EE2
		// (set) Token: 0x0600028F RID: 655 RVA: 0x00006EEA File Offset: 0x00005EEA
		public string SoapVRoot
		{
			get
			{
				return this._SoapVRoot;
			}
			set
			{
				this._SoapVRoot = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00006EF3 File Offset: 0x00005EF3
		// (set) Token: 0x06000291 RID: 657 RVA: 0x00006EFB File Offset: 0x00005EFB
		public string SoapMailbox
		{
			get
			{
				return this._SoapMailbox;
			}
			set
			{
				this._SoapMailbox = value;
			}
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00006F04 File Offset: 0x00005F04
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Application";
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00006F14 File Offset: 0x00005F14
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.MTS, "ApplicationActivationAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Application"];
			if (Platform.IsLessThan(Platform.W2K))
			{
				switch (this._value)
				{
				case ActivationOption.Library:
					catalogObject.SetValue("Activation", "Inproc");
					break;
				case ActivationOption.Server:
					catalogObject.SetValue("Activation", "Local");
					break;
				}
			}
			else
			{
				catalogObject.SetValue("Activation", this._value);
			}
			return true;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00006FA0 File Offset: 0x00005FA0
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			bool flag = false;
			if (this._SoapVRoot != null)
			{
				ICatalogObject catalogObject = (ICatalogObject)info["Application"];
				Platform.Assert(Platform.Whistler, "ApplicationActivationAttribute.SoapVRoot");
				catalogObject.SetValue("SoapActivated", true);
				catalogObject.SetValue("SoapVRoot", this._SoapVRoot);
				flag = true;
			}
			if (this._SoapMailbox != null)
			{
				ICatalogObject catalogObject2 = (ICatalogObject)info["Application"];
				Platform.Assert(Platform.Whistler, "ApplicationActivationAttribute.SoapMailbox");
				catalogObject2.SetValue("SoapActivated", true);
				catalogObject2.SetValue("SoapMailTo", this._SoapMailbox);
				flag = true;
			}
			return flag;
		}

		// Token: 0x04000103 RID: 259
		private ActivationOption _value;

		// Token: 0x04000104 RID: 260
		private string _SoapVRoot;

		// Token: 0x04000105 RID: 261
		private string _SoapMailbox;
	}
}
