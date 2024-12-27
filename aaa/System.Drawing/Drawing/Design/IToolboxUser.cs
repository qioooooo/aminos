using System;

namespace System.Drawing.Design
{
	// Token: 0x020000F7 RID: 247
	public interface IToolboxUser
	{
		// Token: 0x06000DA8 RID: 3496
		bool GetToolSupported(ToolboxItem tool);

		// Token: 0x06000DA9 RID: 3497
		void ToolPicked(ToolboxItem tool);
	}
}
