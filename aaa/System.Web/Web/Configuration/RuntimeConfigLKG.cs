using System;
using System.Configuration;
using System.Configuration.Internal;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000244 RID: 580
	internal class RuntimeConfigLKG : RuntimeConfig
	{
		// Token: 0x06001ED1 RID: 7889 RVA: 0x00089B6C File Offset: 0x00088B6C
		internal RuntimeConfigLKG(IInternalConfigRecord configRecord)
			: base(configRecord, true)
		{
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x00089B78 File Offset: 0x00088B78
		[ConfigurationPermission(SecurityAction.Assert, Unrestricted = true)]
		protected override object GetSectionObject(string sectionName)
		{
			if (this._configRecord != null)
			{
				return this._configRecord.GetLkgSection(sectionName);
			}
			object obj;
			try
			{
				obj = ConfigurationManager.GetSection(sectionName);
			}
			catch
			{
				obj = null;
			}
			return obj;
		}
	}
}
