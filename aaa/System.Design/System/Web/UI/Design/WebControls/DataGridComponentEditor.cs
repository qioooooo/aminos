using System;
using System.Security.Permissions;
using System.Web.UI.Design.WebControls.ListControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200043E RID: 1086
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataGridComponentEditor : BaseDataListComponentEditor
	{
		// Token: 0x0600274A RID: 10058 RVA: 0x000D6BCA File Offset: 0x000D5BCA
		public DataGridComponentEditor()
			: base(DataGridComponentEditor.IDX_GENERAL)
		{
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000D6BD7 File Offset: 0x000D5BD7
		public DataGridComponentEditor(int initialPage)
			: base(initialPage)
		{
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x000D6BE0 File Offset: 0x000D5BE0
		protected override Type[] GetComponentEditorPages()
		{
			return DataGridComponentEditor.editorPages;
		}

		// Token: 0x04001B09 RID: 6921
		private static Type[] editorPages = new Type[]
		{
			typeof(DataGridGeneralPage),
			typeof(DataGridColumnsPage),
			typeof(DataGridPagingPage),
			typeof(FormatPage),
			typeof(BordersPage)
		};

		// Token: 0x04001B0A RID: 6922
		internal static int IDX_GENERAL = 0;

		// Token: 0x04001B0B RID: 6923
		internal static int IDX_COLUMNS = 1;

		// Token: 0x04001B0C RID: 6924
		internal static int IDX_PAGING = 2;

		// Token: 0x04001B0D RID: 6925
		internal static int IDX_FORMAT = 3;

		// Token: 0x04001B0E RID: 6926
		internal static int IDX_BORDERS = 4;
	}
}
