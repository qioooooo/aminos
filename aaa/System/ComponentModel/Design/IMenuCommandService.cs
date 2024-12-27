using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x02000188 RID: 392
	[ComVisible(true)]
	public interface IMenuCommandService
	{
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000C7E RID: 3198
		DesignerVerbCollection Verbs { get; }

		// Token: 0x06000C7F RID: 3199
		void AddCommand(MenuCommand command);

		// Token: 0x06000C80 RID: 3200
		void AddVerb(DesignerVerb verb);

		// Token: 0x06000C81 RID: 3201
		MenuCommand FindCommand(CommandID commandID);

		// Token: 0x06000C82 RID: 3202
		bool GlobalInvoke(CommandID commandID);

		// Token: 0x06000C83 RID: 3203
		void RemoveCommand(MenuCommand command);

		// Token: 0x06000C84 RID: 3204
		void RemoveVerb(DesignerVerb verb);

		// Token: 0x06000C85 RID: 3205
		void ShowContextMenu(CommandID menuID, int x, int y);
	}
}
