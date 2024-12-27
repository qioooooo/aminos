using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000172 RID: 370
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesigntimeLicenseContextSerializer
	{
		// Token: 0x06000C02 RID: 3074 RVA: 0x0002925A File Offset: 0x0002825A
		private DesigntimeLicenseContextSerializer()
		{
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x00029264 File Offset: 0x00028264
		public static void Serialize(Stream o, string cryptoKey, DesigntimeLicenseContext context)
		{
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(o, new object[] { cryptoKey, context.savedLicenseKeys });
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x00029294 File Offset: 0x00028294
		internal static void Deserialize(Stream o, string cryptoKey, RuntimeLicenseContext context)
		{
			IFormatter formatter = new BinaryFormatter();
			object obj = formatter.Deserialize(o);
			if (obj is object[])
			{
				object[] array = (object[])obj;
				if (array[0] is string && (string)array[0] == cryptoKey)
				{
					context.savedLicenseKeys = (Hashtable)array[1];
				}
			}
		}
	}
}
