using System;
using System.Collections;

namespace System.Web.UI.Design
{
	// Token: 0x0200037B RID: 891
	public interface IFolderProjectItem
	{
		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06002123 RID: 8483
		ICollection Children { get; }

		// Token: 0x06002124 RID: 8484
		IDocumentProjectItem AddDocument(string name, byte[] content);

		// Token: 0x06002125 RID: 8485
		IFolderProjectItem AddFolder(string name);
	}
}
