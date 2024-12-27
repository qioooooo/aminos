using System;
using System.Runtime.CompilerServices;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200050B RID: 1291
	[ComVisible(true)]
	public sealed class ExtensibleClassFactory
	{
		// Token: 0x06003290 RID: 12944 RVA: 0x000AB7E3 File Offset: 0x000AA7E3
		private ExtensibleClassFactory()
		{
		}

		// Token: 0x06003291 RID: 12945
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RegisterObjectCreationCallback(ObjectCreationDelegate callback);
	}
}
