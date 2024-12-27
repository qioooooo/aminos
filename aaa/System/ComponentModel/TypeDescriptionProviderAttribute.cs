using System;

namespace System.ComponentModel
{
	// Token: 0x02000143 RID: 323
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class TypeDescriptionProviderAttribute : Attribute
	{
		// Token: 0x06000A71 RID: 2673 RVA: 0x00024263 File Offset: 0x00023263
		public TypeDescriptionProviderAttribute(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this._typeName = typeName;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00024280 File Offset: 0x00023280
		public TypeDescriptionProviderAttribute(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x000242A2 File Offset: 0x000232A2
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x04000A76 RID: 2678
		private string _typeName;
	}
}
