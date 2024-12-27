using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000334 RID: 820
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false)]
	public sealed class XmlSerializerAssemblyAttribute : Attribute
	{
		// Token: 0x06002820 RID: 10272 RVA: 0x000D04B8 File Offset: 0x000CF4B8
		public XmlSerializerAssemblyAttribute()
			: this(null, null)
		{
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x000D04C2 File Offset: 0x000CF4C2
		public XmlSerializerAssemblyAttribute(string assemblyName)
			: this(assemblyName, null)
		{
		}

		// Token: 0x06002822 RID: 10274 RVA: 0x000D04CC File Offset: 0x000CF4CC
		public XmlSerializerAssemblyAttribute(string assemblyName, string codeBase)
		{
			this.assemblyName = assemblyName;
			this.codeBase = codeBase;
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x06002823 RID: 10275 RVA: 0x000D04E2 File Offset: 0x000CF4E2
		// (set) Token: 0x06002824 RID: 10276 RVA: 0x000D04EA File Offset: 0x000CF4EA
		public string CodeBase
		{
			get
			{
				return this.codeBase;
			}
			set
			{
				this.codeBase = value;
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x06002825 RID: 10277 RVA: 0x000D04F3 File Offset: 0x000CF4F3
		// (set) Token: 0x06002826 RID: 10278 RVA: 0x000D04FB File Offset: 0x000CF4FB
		public string AssemblyName
		{
			get
			{
				return this.assemblyName;
			}
			set
			{
				this.assemblyName = value;
			}
		}

		// Token: 0x04001670 RID: 5744
		private string assemblyName;

		// Token: 0x04001671 RID: 5745
		private string codeBase;
	}
}
