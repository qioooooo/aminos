using System;
using System.Security.Permissions;
using System.Web.UI.Design.WebControls.ListControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataListComponentEditor : BaseDataListComponentEditor
	{
		public DataListComponentEditor()
			: base(DataListComponentEditor.IDX_GENERAL)
		{
		}

		public DataListComponentEditor(int initialPage)
			: base(initialPage)
		{
		}

		protected override Type[] GetComponentEditorPages()
		{
			return DataListComponentEditor.editorPages;
		}

		private static Type[] editorPages = new Type[]
		{
			typeof(DataListGeneralPage),
			typeof(FormatPage),
			typeof(BordersPage)
		};

		internal static int IDX_GENERAL = 0;

		internal static int IDX_FORMAT = 1;

		internal static int IDX_BORDERS = 2;
	}
}
