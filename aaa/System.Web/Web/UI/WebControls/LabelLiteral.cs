using System;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C4 RID: 1476
	internal sealed class LabelLiteral : Literal
	{
		// Token: 0x06004816 RID: 18454 RVA: 0x0012698C File Offset: 0x0012598C
		internal LabelLiteral(Control forControl)
		{
			this._for = forControl;
		}

		// Token: 0x170011C5 RID: 4549
		// (get) Token: 0x06004817 RID: 18455 RVA: 0x0012699B File Offset: 0x0012599B
		// (set) Token: 0x06004818 RID: 18456 RVA: 0x001269A3 File Offset: 0x001259A3
		internal bool RenderAsLabel
		{
			get
			{
				return this._renderAsLabel;
			}
			set
			{
				this._renderAsLabel = value;
			}
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x001269AC File Offset: 0x001259AC
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (this.RenderAsLabel)
			{
				writer.Write("<asp:label runat=\"server\" AssociatedControlID=\"");
				writer.Write(this._for.ID);
				writer.Write("\" ID=\"");
				writer.Write(this._for.ID);
				writer.Write("Label\">");
				writer.Write(base.Text);
				writer.Write("</asp:label>");
				return;
			}
			writer.AddAttribute(HtmlTextWriterAttribute.For, this._for.ClientID);
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			base.Render(writer);
			writer.RenderEndTag();
		}

		// Token: 0x04002AC3 RID: 10947
		internal Control _for;

		// Token: 0x04002AC4 RID: 10948
		internal bool _renderAsLabel;
	}
}
