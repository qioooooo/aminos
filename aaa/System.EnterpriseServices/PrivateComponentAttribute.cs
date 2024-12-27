using System;
using System.Collections;
using System.EnterpriseServices.Admin;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000074 RID: 116
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	[ComVisible(false)]
	public sealed class PrivateComponentAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000283 RID: 643 RVA: 0x00006E06 File Offset: 0x00005E06
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Component";
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00006E14 File Offset: 0x00005E14
		bool IConfigurationAttribute.Apply(Hashtable info)
		{
			Platform.Assert(Platform.Whistler, "PrivateComponentAttribute");
			ICatalogObject catalogObject = (ICatalogObject)info["Component"];
			catalogObject.SetValue("IsPrivateComponent", true);
			return true;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00006E53 File Offset: 0x00005E53
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}
	}
}
