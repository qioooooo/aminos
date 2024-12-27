using System;
using System.Security.Permissions;
using System.Web.UI.Design.WebControls.ListControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000441 RID: 1089
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataListComponentEditor : BaseDataListComponentEditor
	{
		// Token: 0x06002768 RID: 10088 RVA: 0x000D7936 File Offset: 0x000D6936
		public DataListComponentEditor()
			: base(DataListComponentEditor.IDX_GENERAL)
		{
		}

		// Token: 0x06002769 RID: 10089 RVA: 0x000D7943 File Offset: 0x000D6943
		public DataListComponentEditor(int initialPage)
			: base(initialPage)
		{
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000D794C File Offset: 0x000D694C
		protected override Type[] GetComponentEditorPages()
		{
			return DataListComponentEditor.editorPages;
		}

		// Token: 0x04001B32 RID: 6962
		private static Type[] editorPages = new Type[]
		{
			typeof(DataListGeneralPage),
			typeof(FormatPage),
			typeof(BordersPage)
		};

		// Token: 0x04001B33 RID: 6963
		internal static int IDX_GENERAL = 0;

		// Token: 0x04001B34 RID: 6964
		internal static int IDX_FORMAT = 1;

		// Token: 0x04001B35 RID: 6965
		internal static int IDX_BORDERS = 2;
	}
}
