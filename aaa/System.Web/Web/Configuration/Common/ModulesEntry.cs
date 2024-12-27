using System;
using System.Configuration;

namespace System.Web.Configuration.Common
{
	// Token: 0x020000AC RID: 172
	internal class ModulesEntry
	{
		// Token: 0x06000877 RID: 2167 RVA: 0x00025EC0 File Offset: 0x00024EC0
		internal ModulesEntry(string name, string typeName, string propertyName, ConfigurationElement configElement)
		{
			this._name = ((name != null) ? name : string.Empty);
			this._type = ConfigUtil.GetType(typeName, propertyName, configElement, false);
			if (typeof(IHttpModule).IsAssignableFrom(this._type))
			{
				return;
			}
			if (configElement == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_not_module", new object[] { typeName }));
			}
			throw new ConfigurationErrorsException(SR.GetString("Type_not_module", new object[] { typeName }), configElement.ElementInformation.Properties["type"].Source, configElement.ElementInformation.Properties["type"].LineNumber);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00025F7D File Offset: 0x00024F7D
		internal static bool IsTypeMatch(Type type, string typeName)
		{
			return type.Name.Equals(typeName) || type.FullName.Equals(typeName);
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x00025F9B File Offset: 0x00024F9B
		internal string ModuleName
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00025FA3 File Offset: 0x00024FA3
		internal IHttpModule Create()
		{
			return (IHttpModule)HttpRuntime.CreateNonPublicInstance(this._type);
		}

		// Token: 0x040011AC RID: 4524
		private string _name;

		// Token: 0x040011AD RID: 4525
		private Type _type;
	}
}
