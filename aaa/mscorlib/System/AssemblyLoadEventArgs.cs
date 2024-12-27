using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200004D RID: 77
	[ComVisible(true)]
	public class AssemblyLoadEventArgs : EventArgs
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00010A0A File Offset: 0x0000FA0A
		public Assembly LoadedAssembly
		{
			get
			{
				return this._LoadedAssembly;
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00010A12 File Offset: 0x0000FA12
		public AssemblyLoadEventArgs(Assembly loadedAssembly)
		{
			this._LoadedAssembly = loadedAssembly;
		}

		// Token: 0x0400018F RID: 399
		private Assembly _LoadedAssembly;
	}
}
