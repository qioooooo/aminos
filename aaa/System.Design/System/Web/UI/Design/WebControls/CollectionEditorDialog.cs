using System;
using System.Web.UI.Design.Util;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x0200040A RID: 1034
	internal abstract partial class CollectionEditorDialog : DesignerForm
	{
		// Token: 0x060025D7 RID: 9687 RVA: 0x000CBE74 File Offset: 0x000CAE74
		protected CollectionEditorDialog(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x000CBE80 File Offset: 0x000CAE80
		protected ToolStripButton CreatePushButton(string toolTipText, int imageIndex)
		{
			return new ToolStripButton
			{
				Text = toolTipText,
				AutoToolTip = true,
				DisplayStyle = ToolStripItemDisplayStyle.Image,
				ImageIndex = imageIndex,
				ImageScaling = ToolStripItemImageScaling.SizeToFit
			};
		}
	}
}
