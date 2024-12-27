using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000094 RID: 148
	[ComVisible(false)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class SecureMethodAttribute : Attribute, IConfigurationAttribute
	{
		// Token: 0x06000388 RID: 904 RVA: 0x0000BABE File Offset: 0x0000AABE
		bool IConfigurationAttribute.IsValidTarget(string s)
		{
			return s == "Method" || s == "Component";
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000BADC File Offset: 0x0000AADC
		bool IConfigurationAttribute.Apply(Hashtable cache)
		{
			string text = (string)cache["CurrentTarget"];
			if (text == "Method")
			{
				cache["SecurityOnMethods"] = true;
			}
			return false;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000BB19 File Offset: 0x0000AB19
		bool IConfigurationAttribute.AfterSaveChanges(Hashtable info)
		{
			return false;
		}
	}
}
