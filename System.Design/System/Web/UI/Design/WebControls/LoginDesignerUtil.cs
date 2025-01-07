using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal static class LoginDesignerUtil
	{
		internal abstract class GenericConvertToTemplateHelper<ControlType, ControlDesignerType> where ControlType : WebControl, IControlDesignerAccessor where ControlDesignerType : ControlDesigner
		{
			public GenericConvertToTemplateHelper(ControlDesignerType designer, IDesignerHost designerHost)
			{
				this._designer = designer;
				this._designerHost = designerHost;
			}

			protected ControlDesignerType Designer
			{
				get
				{
					return this._designer;
				}
			}

			private ControlType ViewControl
			{
				get
				{
					ControlDesignerType designer = this.Designer;
					return (ControlType)((object)designer.ViewControl);
				}
			}

			protected abstract string[] PersistedControlIDs { get; }

			protected abstract string[] PersistedIfNotVisibleControlIDs { get; }

			protected abstract Control GetDefaultTemplateContents();

			protected abstract Style GetFailureTextStyle(ControlType control);

			protected abstract ITemplate GetTemplate(ControlType control);

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

			private const string _failureTextID = "FailureText";

			private ControlDesignerType _designer;

			private IDesignerHost _designerHost;
		}
	}
}
