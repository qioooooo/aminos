using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	// Token: 0x0200053D RID: 1341
	internal sealed class EditorZoneAutoFormat : BaseAutoFormat
	{
		// Token: 0x06002F5A RID: 12122 RVA: 0x0010E32E File Offset: 0x0010D32E
		public EditorZoneAutoFormat(DataRow schemeData)
			: base(schemeData)
		{
			base.Style.Height = 275;
			base.Style.Width = 300;
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x0010E364 File Offset: 0x0010D364
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

		// Token: 0x04002040 RID: 8256
		internal const string PreviewControlID = "AutoFormatPreviewControl";

		// Token: 0x0200053E RID: 1342
		private sealed class AutoFormatTemplate : ITemplate
		{
			// Token: 0x06002F5C RID: 12124 RVA: 0x0010E3A8 File Offset: 0x0010D3A8
			public void InstantiateIn(Control container)
			{
				LayoutEditorPart layoutEditorPart = new LayoutEditorPart();
				layoutEditorPart.ID = "LayoutEditorPart";
				container.Controls.Add(layoutEditorPart);
			}
		}
	}
}
