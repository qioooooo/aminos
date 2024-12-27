using System;

namespace System.Xml.Serialization
{
	// Token: 0x0200031F RID: 799
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public sealed class XmlSchemaProviderAttribute : Attribute
	{
		// Token: 0x06002617 RID: 9751 RVA: 0x000B98A7 File Offset: 0x000B88A7
		public XmlSchemaProviderAttribute(string methodName)
		{
			this.methodName = methodName;
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x000B98B6 File Offset: 0x000B88B6
		public string MethodName
		{
			get
			{
				return this.methodName;
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06002619 RID: 9753 RVA: 0x000B98BE File Offset: 0x000B88BE
		// (set) Token: 0x0600261A RID: 9754 RVA: 0x000B98C6 File Offset: 0x000B88C6
		public bool IsAny
		{
			get
			{
				return this.any;
			}
			set
			{
				this.any = value;
			}
		}

		// Token: 0x040015C1 RID: 5569
		private string methodName;

		// Token: 0x040015C2 RID: 5570
		private bool any;
	}
}
