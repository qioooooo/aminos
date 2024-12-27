using System;
using System.IO;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000067 RID: 103
	public abstract class SoapExtension
	{
		// Token: 0x060002B5 RID: 693
		public abstract object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute);

		// Token: 0x060002B6 RID: 694
		public abstract object GetInitializer(Type serviceType);

		// Token: 0x060002B7 RID: 695
		public abstract void Initialize(object initializer);

		// Token: 0x060002B8 RID: 696
		public abstract void ProcessMessage(SoapMessage message);

		// Token: 0x060002B9 RID: 697 RVA: 0x0000D45D File Offset: 0x0000C45D
		public virtual Stream ChainStream(Stream stream)
		{
			return stream;
		}
	}
}
