using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000504 RID: 1284
	[ControlBuilder(typeof(ContentBuilderInternal))]
	[Designer("System.Web.UI.Design.WebControls.ContentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxItem(false)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class Content : Control, INonBindingContainer, INamingContainer
	{
		// Token: 0x17000ED6 RID: 3798
		// (get) Token: 0x06003EA9 RID: 16041 RVA: 0x001050D8 File Offset: 0x001040D8
		// (set) Token: 0x06003EAA RID: 16042 RVA: 0x001050F0 File Offset: 0x001040F0
		[WebCategory("Behavior")]
		[IDReferenceProperty(typeof(ContentPlaceHolder))]
		[Themeable(false)]
		[WebSysDescription("Content_ContentPlaceHolderID")]
		[DefaultValue("")]
		public string ContentPlaceHolderID
		{
			get
			{
				if (this._contentPlaceHolderID == null)
				{
					return string.Empty;
				}
				return this._contentPlaceHolderID;
			}
			set
			{
				if (!base.DesignMode)
				{
					throw new NotSupportedException(SR.GetString("Property_Set_Not_Supported", new object[]
					{
						"ContentPlaceHolderID",
						base.GetType().ToString()
					}));
				}
				this._contentPlaceHolderID = value;
			}
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x06003EAB RID: 16043 RVA: 0x0010513A File Offset: 0x0010413A
		// (remove) Token: 0x06003EAC RID: 16044 RVA: 0x00105143 File Offset: 0x00104143
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler DataBinding
		{
			add
			{
				base.DataBinding += value;
			}
			remove
			{
				base.DataBinding -= value;
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x06003EAD RID: 16045 RVA: 0x0010514C File Offset: 0x0010414C
		// (remove) Token: 0x06003EAE RID: 16046 RVA: 0x00105155 File Offset: 0x00104155
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler Disposed
		{
			add
			{
				base.Disposed += value;
			}
			remove
			{
				base.Disposed -= value;
			}
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x06003EAF RID: 16047 RVA: 0x0010515E File Offset: 0x0010415E
		// (remove) Token: 0x06003EB0 RID: 16048 RVA: 0x00105167 File Offset: 0x00104167
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler Init
		{
			add
			{
				base.Init += value;
			}
			remove
			{
				base.Init -= value;
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06003EB1 RID: 16049 RVA: 0x00105170 File Offset: 0x00104170
		// (remove) Token: 0x06003EB2 RID: 16050 RVA: 0x00105179 File Offset: 0x00104179
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler Load
		{
			add
			{
				base.Load += value;
			}
			remove
			{
				base.Load -= value;
			}
		}

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06003EB3 RID: 16051 RVA: 0x00105182 File Offset: 0x00104182
		// (remove) Token: 0x06003EB4 RID: 16052 RVA: 0x0010518B File Offset: 0x0010418B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public new event EventHandler PreRender
		{
			add
			{
				base.PreRender += value;
			}
			remove
			{
				base.PreRender -= value;
			}
		}

		// Token: 0x14000081 RID: 129
		// (add) Token: 0x06003EB5 RID: 16053 RVA: 0x00105194 File Offset: 0x00104194
		// (remove) Token: 0x06003EB6 RID: 16054 RVA: 0x0010519D File Offset: 0x0010419D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new event EventHandler Unload
		{
			add
			{
				base.Unload += value;
			}
			remove
			{
				base.Unload -= value;
			}
		}

		// Token: 0x040027A2 RID: 10146
		private string _contentPlaceHolderID;
	}
}
