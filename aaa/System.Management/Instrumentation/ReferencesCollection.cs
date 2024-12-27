using System;
using System.Collections.Specialized;

namespace System.Management.Instrumentation
{
	// Token: 0x02000094 RID: 148
	internal class ReferencesCollection
	{
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00021B35 File Offset: 0x00020B35
		public StringCollection Namespaces
		{
			get
			{
				return this.namespaces;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00021B3D File Offset: 0x00020B3D
		public StringCollection Assemblies
		{
			get
			{
				return this.assemblies;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x00021B45 File Offset: 0x00020B45
		public CodeWriter UsingCode
		{
			get
			{
				return this.usingCode;
			}
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00021B50 File Offset: 0x00020B50
		public void Add(Type type)
		{
			if (!this.namespaces.Contains(type.Namespace))
			{
				this.namespaces.Add(type.Namespace);
				this.usingCode.Line(string.Format("using {0};", type.Namespace));
			}
			if (!this.assemblies.Contains(type.Assembly.Location))
			{
				this.assemblies.Add(type.Assembly.Location);
			}
		}

		// Token: 0x04000263 RID: 611
		private StringCollection namespaces = new StringCollection();

		// Token: 0x04000264 RID: 612
		private StringCollection assemblies = new StringCollection();

		// Token: 0x04000265 RID: 613
		private CodeWriter usingCode = new CodeWriter();
	}
}
