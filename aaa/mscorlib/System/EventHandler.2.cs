using System;

namespace System
{
	// Token: 0x020000B6 RID: 182
	// (Invoke) Token: 0x06000AAA RID: 2730
	[Serializable]
	public delegate void EventHandler<TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;
}
