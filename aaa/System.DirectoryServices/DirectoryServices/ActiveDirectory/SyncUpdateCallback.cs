using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200009A RID: 154
	// (Invoke) Token: 0x060004F1 RID: 1265
	public delegate bool SyncUpdateCallback(SyncFromAllServersEvent eventType, string targetServer, string sourceServer, SyncFromAllServersOperationException exception);
}
