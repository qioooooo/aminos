using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005CA RID: 1482
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[ParseChildren(true, "Text")]
	[ControlBuilder(typeof(ListItemControlBuilder))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ListItem : IStateManager, IParserAccessor, IAttributeAccessor
	{
		// Token: 0x06004837 RID: 18487 RVA: 0x00126E0C File Offset: 0x00125E0C
		public ListItem()
			: this(null, null)
		{
		}

		// Token: 0x06004838 RID: 18488 RVA: 0x00126E16 File Offset: 0x00125E16
		public ListItem(string text)
			: this(text, null)
		{
		}

		// Token: 0x06004839 RID: 18489 RVA: 0x00126E20 File Offset: 0x00125E20
		public ListItem(string text, string value)
			: this(text, value, true)
		{
		}

		// Token: 0x0600483A RID: 18490 RVA: 0x00126E2B File Offset: 0x00125E2B
		public ListItem(string text, string value, bool enabled)
		{
			this.text = text;
			this.value = value;
			this.enabled = enabled;
		}

		// Token: 0x170011CD RID: 4557
		// (get) Token: 0x0600483B RID: 18491 RVA: 0x00126E48 File Offset: 0x00125E48
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public AttributeCollection Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					this._attributes = new AttributeCollection(new StateBag(true));
				}
				return this._attributes;
			}
		}

		// Token: 0x170011CE RID: 4558
		// (get) Token: 0x0600483C RID: 18492 RVA: 0x00126E69 File Offset: 0x00125E69
		// (set) Token: 0x0600483D RID: 18493 RVA: 0x00126E83 File Offset: 0x00125E83
		internal bool Dirty
		{
			get
			{
				return this.textisdirty || this.valueisdirty || this.enabledisdirty;
			}
			set
			{
				this.textisdirty = value;
				this.valueisdirty = value;
				this.enabledisdirty = value;
			}
		}

		// Token: 0x170011CF RID: 4559
		// (get) Token: 0x0600483E RID: 18494 RVA: 0x00126E9A File Offset: 0x00125E9A
		// (set) Token: 0x0600483F RID: 18495 RVA: 0x00126EA2 File Offset: 0x00125EA2
		[DefaultValue(true)]
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				this.enabled = value;
				if (((IStateManager)this).IsTrackingViewState)
				{
					this.enabledisdirty = true;
				}
			}
		}

		// Token: 0x170011D0 RID: 4560
		// (get) Token: 0x06004840 RID: 18496 RVA: 0x00126EBA File Offset: 0x00125EBA
		internal bool HasAttributes
		{
			get
			{
				return this._attributes != null && this._attributes.Count > 0;
			}
		}

		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x06004841 RID: 18497 RVA: 0x00126ED4 File Offset: 0x00125ED4
		// (set) Token: 0x06004842 RID: 18498 RVA: 0x00126EDC File Offset: 0x00125EDC
		[DefaultValue(false)]
		[TypeConverter(typeof(MinimizableAttributeTypeConverter))]
		public bool Selected
		{
			get
			{
				return this.selected;
			}
			set
			{
				this.selected = value;
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x06004843 RID: 18499 RVA: 0x00126EE5 File Offset: 0x00125EE5
		// (set) Token: 0x06004844 RID: 18500 RVA: 0x00126F0A File Offset: 0x00125F0A
		[PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)]
		[Localizable(true)]
		[DefaultValue("")]
		public string Text
		{
			get
			{
				if (this.text != null)
				{
					return this.text;
				}
				if (this.value != null)
				{
					return this.value;
				}
				return string.Empty;
			}
			set
			{
				this.text = value;
				if (((IStateManager)this).IsTrackingViewState)
				{
					this.textisdirty = true;
				}
			}
		}

		// Token: 0x170011D3 RID: 4563
		// (get) Token: 0x06004845 RID: 18501 RVA: 0x00126F22 File Offset: 0x00125F22
		// (set) Token: 0x06004846 RID: 18502 RVA: 0x00126F47 File Offset: 0x00125F47
		[Localizable(true)]
		[DefaultValue("")]
		public string Value
		{
			get
			{
				if (this.value != null)
				{
					return this.value;
				}
				if (this.text != null)
				{
					return this.text;
				}
				return string.Empty;
			}
			set
			{
				this.value = value;
				if (((IStateManager)this).IsTrackingViewState)
				{
					this.valueisdirty = true;
				}
			}
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x00126F5F File Offset: 0x00125F5F
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this.Value.GetHashCode(), this.Text.GetHashCode());
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x00126F7C File Offset: 0x00125F7C
		public override bool Equals(object o)
		{
			ListItem listItem = o as ListItem;
			return listItem != null && this.Value.Equals(listItem.Value) && this.Text.Equals(listItem.Text);
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x00126FBB File Offset: 0x00125FBB
		public static ListItem FromString(string s)
		{
			return new ListItem(s);
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x00126FC3 File Offset: 0x00125FC3
		public override string ToString()
		{
			return this.Text;
		}

		// Token: 0x170011D4 RID: 4564
		// (get) Token: 0x0600484B RID: 18507 RVA: 0x00126FCB File Offset: 0x00125FCB
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this.marked;
			}
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x00126FD3 File Offset: 0x00125FD3
		void IStateManager.LoadViewState(object state)
		{
			this.LoadViewState(state);
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x00126FDC File Offset: 0x00125FDC
		internal void LoadViewState(object state)
		{
			if (state != null)
			{
				if (state is Triplet)
				{
					Triplet triplet = (Triplet)state;
					if (triplet.First != null)
					{
						this.Text = (string)triplet.First;
					}
					if (triplet.Second != null)
					{
						this.Value = (string)triplet.Second;
					}
					if (triplet.Third == null)
					{
						return;
					}
					try
					{
						this.Enabled = (bool)triplet.Third;
						return;
					}
					catch
					{
						return;
					}
				}
				if (state is Pair)
				{
					Pair pair = (Pair)state;
					if (pair.First != null)
					{
						this.Text = (string)pair.First;
					}
					this.Value = (string)pair.Second;
					return;
				}
				this.Text = (string)state;
			}
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x001270A4 File Offset: 0x001260A4
		void IStateManager.TrackViewState()
		{
			this.TrackViewState();
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x001270AC File Offset: 0x001260AC
		internal void TrackViewState()
		{
			this.marked = true;
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x001270B5 File Offset: 0x001260B5
		internal void RenderAttributes(HtmlTextWriter writer)
		{
			if (this._attributes != null)
			{
				this._attributes.AddAttributes(writer);
			}
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x001270CB File Offset: 0x001260CB
		object IStateManager.SaveViewState()
		{
			return this.SaveViewState();
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x001270D4 File Offset: 0x001260D4
		internal object SaveViewState()
		{
			string text = null;
			string text2 = null;
			if (this.textisdirty)
			{
				text = this.Text;
			}
			if (this.valueisdirty)
			{
				text2 = this.Value;
			}
			if (this.enabledisdirty)
			{
				return new Triplet(text, text2, this.Enabled);
			}
			if (this.valueisdirty)
			{
				return new Pair(text, text2);
			}
			if (this.textisdirty)
			{
				return text;
			}
			return null;
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x00127139 File Offset: 0x00126139
		string IAttributeAccessor.GetAttribute(string name)
		{
			return this.Attributes[name];
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x00127147 File Offset: 0x00126147
		void IAttributeAccessor.SetAttribute(string name, string value)
		{
			this.Attributes[name] = value;
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x00127158 File Offset: 0x00126158
		void IParserAccessor.AddParsedSubObject(object obj)
		{
			if (obj is LiteralControl)
			{
				this.Text = ((LiteralControl)obj).Text;
				return;
			}
			if (obj is DataBoundLiteralControl)
			{
				throw new HttpException(SR.GetString("Control_Cannot_Databind", new object[] { "ListItem" }));
			}
			throw new HttpException(SR.GetString("Cannot_Have_Children_Of_Type", new object[]
			{
				"ListItem",
				obj.GetType().Name.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x001271DE File Offset: 0x001261DE
		private void ResetText()
		{
			this.Text = null;
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x001271E7 File Offset: 0x001261E7
		private void ResetValue()
		{
			this.Value = null;
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x001271F0 File Offset: 0x001261F0
		private bool ShouldSerializeText()
		{
			return this.text != null && this.text.Length != 0;
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0012720D File Offset: 0x0012620D
		private bool ShouldSerializeValue()
		{
			return this.value != null && this.value.Length != 0;
		}

		// Token: 0x04002AC5 RID: 10949
		private bool selected;

		// Token: 0x04002AC6 RID: 10950
		private bool marked;

		// Token: 0x04002AC7 RID: 10951
		private bool textisdirty;

		// Token: 0x04002AC8 RID: 10952
		private bool valueisdirty;

		// Token: 0x04002AC9 RID: 10953
		private bool enabled;

		// Token: 0x04002ACA RID: 10954
		private bool enabledisdirty;

		// Token: 0x04002ACB RID: 10955
		private string text;

		// Token: 0x04002ACC RID: 10956
		private string value;

		// Token: 0x04002ACD RID: 10957
		private AttributeCollection _attributes;
	}
}
