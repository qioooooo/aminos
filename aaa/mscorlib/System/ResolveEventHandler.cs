using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200004E RID: 78
	// (Invoke) Token: 0x06000428 RID: 1064
	[ComVisible(true)]
	[Serializable]
	public delegate Assembly ResolveEventHandler(object sender, ResolveEventArgs args);
}
