using System;
using System.Configuration.Provider;
using System.DirectoryServices;

namespace System.Web.Security
{
	// Token: 0x0200031A RID: 794
	internal static class PropertyManager
	{
		// Token: 0x0600274A RID: 10058 RVA: 0x000AC6F8 File Offset: 0x000AB6F8
		public static object GetPropertyValue(DirectoryEntry directoryEntry, string propertyName)
		{
			if (directoryEntry.Properties[propertyName].Count != 0)
			{
				return directoryEntry.Properties[propertyName].Value;
			}
			if (directoryEntry.Properties["distinguishedName"].Count != 0)
			{
				throw new ProviderException(SR.GetString("ADMembership_Property_not_found_on_object", new object[]
				{
					propertyName,
					(string)directoryEntry.Properties["distinguishedName"].Value
				}));
			}
			throw new ProviderException(SR.GetString("ADMembership_Property_not_found", new object[] { propertyName }));
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000AC798 File Offset: 0x000AB798
		public static object GetSearchResultPropertyValue(SearchResult res, string propertyName)
		{
			ResultPropertyValueCollection resultPropertyValueCollection = res.Properties[propertyName];
			if (resultPropertyValueCollection == null || resultPropertyValueCollection.Count < 1)
			{
				throw new ProviderException(SR.GetString("ADMembership_Property_not_found", new object[] { propertyName }));
			}
			return resultPropertyValueCollection[0];
		}
	}
}
