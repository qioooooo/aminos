using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C3 RID: 1475
	[DataBindingHandler("System.Web.UI.Design.TextDataBindingHandler, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DefaultProperty("Text")]
	[Designer("System.Web.UI.Design.WebControls.LiteralDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ControlBuilder(typeof(LiteralControlBuilder))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Literal : Control, ITextControl
	{
		// Token: 0x170011C3 RID: 4547
		// (get) Token: 0x0600480D RID: 18445 RVA: 0x00126820 File Offset: 0x00125820
		// (set) Token: 0x0600480E RID: 18446 RVA: 0x00126849 File Offset: 0x00125849
		[WebSysDescription("Literal_Mode")]
		[DefaultValue(LiteralMode.Transform)]
		[WebCategory("Behavior")]
		public LiteralMode Mode
		{
			get
			{
				object obj = this.ViewState["Mode"];
				if (obj != null)
				{
					return (LiteralMode)obj;
				}
				return LiteralMode.Transform;
			}
			set
			{
				if (value < LiteralMode.Transform || value > LiteralMode.Encode)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["Mode"] = value;
			}
		}

		// Token: 0x170011C4 RID: 4548
		// (get) Token: 0x0600480F RID: 18447 RVA: 0x00126874 File Offset: 0x00125874
		// (set) Token: 0x06004810 RID: 18448 RVA: 0x001268A1 File Offset: 0x001258A1
		[Bindable(true)]
		[Localizable(true)]
		[WebCategory("Appearance")]
		[DefaultValue("")]
		[WebSysDescription("Literal_Text")]
		public string Text
		{
			get
			{
				string text = (string)this.ViewState["Text"];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				this.ViewState["Text"] = value;
			}
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x001268B4 File Offset: 0x001258B4
		protected override void AddParsedSubObject(object obj)
		{
			if (obj is LiteralControl)
			{
				this.Text = ((LiteralControl)obj).Text;
				return;
			}
			throw new HttpException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
			{
				"Literal",
				obj.GetType().Name.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x00126912 File Offset: 0x00125912
		protected override ControlCollection CreateControlCollection()
		{
			return new EmptyControlCollection(this);
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x0012691C File Offset: 0x0012591C
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override void Focus()
		{
			throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x00126950 File Offset: 0x00125950
		protected internal override void Render(HtmlTextWriter writer)
		{
			string text = this.Text;
			if (text.Length != 0)
			{
				if (this.Mode != LiteralMode.Encode)
				{
					writer.Write(text);
					return;
				}
				HttpUtility.HtmlEncode(text, writer);
			}
		}
	}
}
