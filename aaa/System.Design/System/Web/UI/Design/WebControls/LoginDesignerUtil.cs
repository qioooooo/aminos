using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000406 RID: 1030
	internal static class LoginDesignerUtil
	{
		// Token: 0x02000407 RID: 1031
		internal abstract class GenericConvertToTemplateHelper<ControlType, ControlDesignerType> where ControlType : WebControl, IControlDesignerAccessor where ControlDesignerType : ControlDesigner
		{
			// Token: 0x060025C3 RID: 9667 RVA: 0x000CBA36 File Offset: 0x000CAA36
			public GenericConvertToTemplateHelper(ControlDesignerType designer, IDesignerHost designerHost)
			{
				this._designer = designer;
				this._designerHost = designerHost;
			}

			// Token: 0x17000706 RID: 1798
			// (get) Token: 0x060025C4 RID: 9668 RVA: 0x000CBA4C File Offset: 0x000CAA4C
			protected ControlDesignerType Designer
			{
				get
				{
					return this._designer;
				}
			}

			// Token: 0x17000707 RID: 1799
			// (get) Token: 0x060025C5 RID: 9669 RVA: 0x000CBA54 File Offset: 0x000CAA54
			private ControlType ViewControl
			{
				get
				{
					ControlDesignerType designer = this.Designer;
					return (ControlType)((object)designer.ViewControl);
				}
			}

			// Token: 0x17000708 RID: 1800
			// (get) Token: 0x060025C6 RID: 9670
			protected abstract string[] PersistedControlIDs { get; }

			// Token: 0x17000709 RID: 1801
			// (get) Token: 0x060025C7 RID: 9671
			protected abstract string[] PersistedIfNotVisibleControlIDs { get; }

			// Token: 0x060025C8 RID: 9672
			protected abstract Control GetDefaultTemplateContents();

			// Token: 0x060025C9 RID: 9673
			protected abstract Style GetFailureTextStyle(ControlType control);

			// Token: 0x060025CA RID: 9674
			protected abstract ITemplate GetTemplate(ControlType control);

			// Token: 0x060025CB RID: 9675 RVA: 0x000CBA7C File Offset: 0x000CAA7C
			private void ConvertPersistedControlsToLiteralControls(Control defaultTemplateContents)
			{
				foreach (string text in this.PersistedControlIDs)
				{
					Control control = defaultTemplateContents.FindControl(text);
					if (control != null)
					{
						if (Array.IndexOf<string>(this.PersistedIfNotVisibleControlIDs, text) >= 0)
						{
							control.Visible = true;
							control.Parent.Visible = true;
							control.Parent.Parent.Visible = true;
						}
						if (control.Visible)
						{
							string text2 = ControlPersister.PersistControl(control, this._designerHost);
							LiteralControl literalControl = new LiteralControl(text2);
							ControlCollection controls = control.Parent.Controls;
							int num = controls.IndexOf(control);
							controls.Remove(control);
							controls.AddAt(num, literalControl);
						}
					}
				}
			}

			// Token: 0x060025CC RID: 9676 RVA: 0x000CBB34 File Offset: 0x000CAB34
			public ITemplate ConvertToTemplate()
			{
				ITemplate template = this.GetTemplate(this.ViewControl);
				ITemplate template2;
				if (template != null)
				{
					template2 = template;
				}
				else
				{
					this._designer.ViewControlCreated = false;
					Hashtable hashtable = new Hashtable(1);
					hashtable.Add("ConvertToTemplate", true);
					this.ViewControl.SetDesignModeState(hashtable);
					this._designer.GetDesignTimeHtml();
					Control defaultTemplateContents = this.GetDefaultTemplateContents();
					this.SetFailureTextStyle(defaultTemplateContents);
					this.ConvertPersistedControlsToLiteralControls(defaultTemplateContents);
					StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
					HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
					defaultTemplateContents.RenderControl(htmlTextWriter);
					template2 = ControlParser.ParseTemplate(this._designerHost, stringWriter.ToString());
					Hashtable hashtable2 = new Hashtable(1);
					hashtable2.Add("ConvertToTemplate", false);
					this.ViewControl.SetDesignModeState(hashtable2);
				}
				return template2;
			}

			// Token: 0x060025CD RID: 9677 RVA: 0x000CBC1C File Offset: 0x000CAC1C
			private void SetFailureTextStyle(Control defaultTemplateContents)
			{
				Control control = defaultTemplateContents.FindControl("FailureText");
				if (control != null)
				{
					TableCell tableCell = (TableCell)control.Parent;
					tableCell.ForeColor = Color.Red;
					tableCell.ApplyStyle(this.GetFailureTextStyle(this.ViewControl));
					control.EnableViewState = false;
				}
			}

			// Token: 0x040019ED RID: 6637
			private const string _failureTextID = "FailureText";

			// Token: 0x040019EE RID: 6638
			private ControlDesignerType _designer;

			// Token: 0x040019EF RID: 6639
			private IDesignerHost _designerHost;
		}
	}
}
