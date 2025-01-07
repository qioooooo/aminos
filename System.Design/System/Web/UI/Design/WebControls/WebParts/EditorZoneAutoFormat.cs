using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	internal sealed class EditorZoneAutoFormat : BaseAutoFormat
	{
		public EditorZoneAutoFormat(DataRow schemeData)
			: base(schemeData)
		{
			base.Style.Height = 275;
			base.Style.Width = 300;
		}

		public override Control GetPreviewControl(Control runtimeControl)
		{
			EditorZone editorZone = (EditorZone)base.GetPreviewControl(runtimeControl);
			if (editorZone != null && editorZone.EditorParts.Count == 0)
			{
				editorZone.ZoneTemplate = new EditorZoneAutoFormat.AutoFormatTemplate();
			}
			editorZone.ID = "AutoFormatPreviewControl";
			return editorZone;
		}

		internal const string PreviewControlID = "AutoFormatPreviewControl";

		private sealed class AutoFormatTemplate : ITemplate
		{
			public void InstantiateIn(Control container)
			{
				LayoutEditorPart layoutEditorPart = new LayoutEditorPart();
				layoutEditorPart.ID = "LayoutEditorPart";
				container.Controls.Add(layoutEditorPart);
			}
		}
	}
}
