using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A4 RID: 420
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class DefaultSerializationProviderAttribute : Attribute
	{
		// Token: 0x06000D03 RID: 3331 RVA: 0x0002A525 File Offset: 0x00029525
		public DefaultSerializationProviderAttribute(Type providerType)
		{
			if (providerType == null)
			{
				throw new ArgumentNullException("providerType");
			}
			this._providerTypeName = providerType.AssemblyQualifiedName;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0002A547 File Offset: 0x00029547
		public DefaultSerializationProviderAttribute(string providerTypeName)
		{
			if (providerTypeName == null)
			{
				throw new ArgumentNullException("providerTypeName");
			}
			this._providerTypeName = providerTypeName;
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x0002A564 File Offset: 0x00029564
		public string ProviderTypeName
		{
			get
			{
				return this._providerTypeName;
			}
		}

		// Token: 0x04000EA9 RID: 3753
		private string _providerTypeName;
	}
}
