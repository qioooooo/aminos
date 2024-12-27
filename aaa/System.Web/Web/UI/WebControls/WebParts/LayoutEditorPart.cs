using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006D3 RID: 1747
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class LayoutEditorPart : EditorPart
	{
		// Token: 0x1700161A RID: 5658
		// (get) Token: 0x060055BC RID: 21948 RVA: 0x0015B0CC File Offset: 0x0015A0CC
		private bool CanChangeChromeState
		{
			get
			{
				WebPart webPartToEdit = base.WebPartToEdit;
				return webPartToEdit.Zone.AllowLayoutChange && (webPartToEdit.AllowMinimize || webPartToEdit.ChromeState == PartChromeState.Minimized);
			}
		}

		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x060055BD RID: 21949 RVA: 0x0015B104 File Offset: 0x0015A104
		private bool CanChangeZone
		{
			get
			{
				WebPart webPartToEdit = base.WebPartToEdit;
				WebPartZoneBase zone = webPartToEdit.Zone;
				return zone.AllowLayoutChange && webPartToEdit.AllowZoneChange;
			}
		}

		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x060055BE RID: 21950 RVA: 0x0015B12F File Offset: 0x0015A12F
		private bool CanChangeZoneIndex
		{
			get
			{
				return base.WebPartToEdit.Zone.AllowLayoutChange;
			}
		}

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x060055BF RID: 21951 RVA: 0x0015B141 File Offset: 0x0015A141
		// (set) Token: 0x060055C0 RID: 21952 RVA: 0x0015B149 File Offset: 0x0015A149
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Themeable(false)]
		[Browsable(false)]
		public override string DefaultButton
		{
			get
			{
				return base.DefaultButton;
			}
			set
			{
				base.DefaultButton = value;
			}
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x060055C1 RID: 21953 RVA: 0x0015B152 File Offset: 0x0015A152
		public override bool Display
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x060055C2 RID: 21954 RVA: 0x0015B155 File Offset: 0x0015A155
		private bool HasError
		{
			get
			{
				return this._chromeStateErrorMessage != null || this._zoneIndexErrorMessage != null;
			}
		}

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x060055C3 RID: 21955 RVA: 0x0015B170 File Offset: 0x0015A170
		// (set) Token: 0x060055C4 RID: 21956 RVA: 0x0015B1A2 File Offset: 0x0015A1A2
		[WebSysDefaultValue("LayoutEditorPart_PartTitle")]
		public override string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return SR.GetString("LayoutEditorPart_PartTitle");
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x060055C5 RID: 21957 RVA: 0x0015B1B8 File Offset: 0x0015A1B8
		public override bool ApplyChanges()
		{
			WebPart webPartToEdit = base.WebPartToEdit;
			if (webPartToEdit != null)
			{
				this.EnsureChildControls();
				try
				{
					if (this.CanChangeChromeState)
					{
						TypeConverter converter = TypeDescriptor.GetConverter(typeof(PartChromeState));
						webPartToEdit.ChromeState = (PartChromeState)converter.ConvertFromString(this._chromeState.SelectedValue);
					}
				}
				catch (Exception ex)
				{
					this._chromeStateErrorMessage = base.CreateErrorMessage(ex.Message);
				}
				int zoneIndex = webPartToEdit.ZoneIndex;
				if (this.CanChangeZoneIndex)
				{
					if (int.TryParse(this._zoneIndex.Text, NumberStyles.Integer, CultureInfo.CurrentCulture, out zoneIndex))
					{
						if (zoneIndex < 0)
						{
							this._zoneIndexErrorMessage = SR.GetString("EditorPart_PropertyMinValue", new object[] { 0.ToString(CultureInfo.CurrentCulture) });
						}
					}
					else
					{
						this._zoneIndexErrorMessage = SR.GetString("EditorPart_PropertyMustBeInteger");
					}
				}
				WebPartZoneBase zone = webPartToEdit.Zone;
				WebPartZoneBase webPartZoneBase = zone;
				if (this.CanChangeZone)
				{
					webPartZoneBase = base.WebPartManager.Zones[this._zone.SelectedValue];
				}
				if (this._zoneIndexErrorMessage == null && zone.AllowLayoutChange && webPartZoneBase.AllowLayoutChange)
				{
					if (webPartToEdit.Zone == webPartZoneBase)
					{
						if (webPartToEdit.ZoneIndex == zoneIndex)
						{
							goto IL_0150;
						}
					}
					try
					{
						base.WebPartManager.MoveWebPart(webPartToEdit, webPartZoneBase, zoneIndex);
					}
					catch (Exception ex2)
					{
						this._zoneIndexErrorMessage = base.CreateErrorMessage(ex2.Message);
					}
				}
			}
			IL_0150:
			return !this.HasError;
		}

		// Token: 0x060055C6 RID: 21958 RVA: 0x0015B33C File Offset: 0x0015A33C
		protected internal override void CreateChildControls()
		{
			ControlCollection controls = this.Controls;
			controls.Clear();
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(PartChromeState));
			this._chromeState = new DropDownList();
			this._chromeState.Items.Add(new ListItem(SR.GetString("PartChromeState_Normal"), converter.ConvertToString(PartChromeState.Normal)));
			this._chromeState.Items.Add(new ListItem(SR.GetString("PartChromeState_Minimized"), converter.ConvertToString(PartChromeState.Minimized)));
			controls.Add(this._chromeState);
			this._zone = new DropDownList();
			WebPartManager webPartManager = base.WebPartManager;
			if (webPartManager != null)
			{
				WebPartZoneCollection zones = webPartManager.Zones;
				if (zones != null)
				{
					foreach (object obj in zones)
					{
						WebPartZoneBase webPartZoneBase = (WebPartZoneBase)obj;
						ListItem listItem = new ListItem(webPartZoneBase.DisplayTitle, webPartZoneBase.ID);
						this._zone.Items.Add(listItem);
					}
				}
			}
			controls.Add(this._zone);
			this._zoneIndex = new TextBox();
			this._zoneIndex.Columns = 10;
			controls.Add(this._zoneIndex);
			foreach (object obj2 in controls)
			{
				Control control = (Control)obj2;
				control.EnableViewState = false;
			}
		}

		// Token: 0x060055C7 RID: 21959 RVA: 0x0015B4E0 File Offset: 0x0015A4E0
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Display && this.Visible && !this.HasError)
			{
				this.SyncChanges();
			}
		}

		// Token: 0x060055C8 RID: 21960 RVA: 0x0015B508 File Offset: 0x0015A508
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			this.EnsureChildControls();
			if (base.DesignMode)
			{
				this._zone.Items.Add(SR.GetString("Zone_SampleHeaderText"));
			}
			string[] array = new string[]
			{
				SR.GetString("LayoutEditorPart_ChromeState"),
				SR.GetString("LayoutEditorPart_Zone"),
				SR.GetString("LayoutEditorPart_ZoneIndex")
			};
			WebControl[] array2 = new WebControl[] { this._chromeState, this._zone, this._zoneIndex };
			string[] array3 = new string[] { this._chromeStateErrorMessage, null, this._zoneIndexErrorMessage };
			base.RenderPropertyEditors(writer, array, null, array2, array3);
		}

		// Token: 0x060055C9 RID: 21961 RVA: 0x0015B5D4 File Offset: 0x0015A5D4
		public override void SyncChanges()
		{
			WebPart webPartToEdit = base.WebPartToEdit;
			if (webPartToEdit != null)
			{
				WebPartZoneBase zone = webPartToEdit.Zone;
				bool allowLayoutChange = zone.AllowLayoutChange;
				this.EnsureChildControls();
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(PartChromeState));
				this._chromeState.SelectedValue = converter.ConvertToString(webPartToEdit.ChromeState);
				this._chromeState.Enabled = this.CanChangeChromeState;
				WebPartManager webPartManager = base.WebPartManager;
				if (webPartManager != null)
				{
					WebPartZoneCollection zones = webPartManager.Zones;
					bool allowZoneChange = webPartToEdit.AllowZoneChange;
					this._zone.ClearSelection();
					foreach (object obj in this._zone.Items)
					{
						ListItem listItem = (ListItem)obj;
						string value = listItem.Value;
						WebPartZoneBase webPartZoneBase = zones[value];
						if (webPartZoneBase == zone || (allowZoneChange && webPartZoneBase.AllowLayoutChange))
						{
							listItem.Enabled = true;
						}
						else
						{
							listItem.Enabled = false;
						}
						if (webPartZoneBase == zone)
						{
							listItem.Selected = true;
						}
					}
					this._zone.Enabled = this.CanChangeZone;
				}
				this._zoneIndex.Text = webPartToEdit.ZoneIndex.ToString(CultureInfo.CurrentCulture);
				this._zoneIndex.Enabled = this.CanChangeZoneIndex;
			}
		}

		// Token: 0x04002F2D RID: 12077
		private const int TextBoxColumns = 10;

		// Token: 0x04002F2E RID: 12078
		private const int MinZoneIndex = 0;

		// Token: 0x04002F2F RID: 12079
		private DropDownList _chromeState;

		// Token: 0x04002F30 RID: 12080
		private DropDownList _zone;

		// Token: 0x04002F31 RID: 12081
		private TextBox _zoneIndex;

		// Token: 0x04002F32 RID: 12082
		private string _chromeStateErrorMessage;

		// Token: 0x04002F33 RID: 12083
		private string _zoneIndexErrorMessage;
	}
}
