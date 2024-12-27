using System;
using System.Reflection;
using System.Threading;

namespace System.Xml.Serialization
{
	// Token: 0x02000322 RID: 802
	public abstract class XmlSerializationGeneratedCode
	{
		// Token: 0x06002653 RID: 9811 RVA: 0x000BAE78 File Offset: 0x000B9E78
		internal void Init(TempAssembly tempAssembly)
		{
			this.tempAssembly = tempAssembly;
			if (tempAssembly != null && tempAssembly.NeedAssembyResolve)
			{
				this.threadCode = Thread.CurrentThread.GetHashCode();
				this.assemblyResolver = new ResolveEventHandler(this.OnAssemblyResolve);
				AppDomain.CurrentDomain.AssemblyResolve += this.assemblyResolver;
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x000BAEC9 File Offset: 0x000B9EC9
		internal void Dispose()
		{
			if (this.assemblyResolver != null)
			{
				AppDomain.CurrentDomain.AssemblyResolve -= this.assemblyResolver;
			}
			this.assemblyResolver = null;
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x000BAEEA File Offset: 0x000B9EEA
		internal Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (this.tempAssembly != null && Thread.CurrentThread.GetHashCode() == this.threadCode)
			{
				return this.tempAssembly.GetReferencedAssembly(args.Name);
			}
			return null;
		}

		// Token: 0x040015D0 RID: 5584
		private TempAssembly tempAssembly;

		// Token: 0x040015D1 RID: 5585
		private int threadCode;

		// Token: 0x040015D2 RID: 5586
		private ResolveEventHandler assemblyResolver;
	}
}
