using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200073A RID: 1850
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartMenuStyle : TableStyle, ICustomTypeDescriptor
	{
		// Token: 0x060059CD RID: 22989 RVA: 0x0016B018 File Offset: 0x0016A018
		public WebPartMenuStyle()
			: this(null)
		{
		}

		// Token: 0x060059CE RID: 22990 RVA: 0x0016B021 File Offset: 0x0016A021
		public WebPartMenuStyle(StateBag bag)
			: base(bag)
		{
			this.CellPadding = 1;
			this.CellSpacing = 0;
		}

		// Token: 0x17001732 RID: 5938
		// (get) Token: 0x060059CF RID: 22991 RVA: 0x0016B038 File Offset: 0x0016A038
		// (set) Token: 0x060059D0 RID: 22992 RVA: 0x0016B062 File Offset: 0x0016A062
		[WebSysDescription("WebPartMenuStyle_ShadowColor")]
		[DefaultValue(typeof(Color), "")]
		[TypeConverter(typeof(WebColorConverter))]
		[WebCategory("Appearance")]
		public Color ShadowColor
		{
			get
			{
				if (base.IsSet(2097152))
				{
					return (Color)base.ViewState["ShadowColor"];
				}
				return Color.Empty;
			}
			set
			{
				base.ViewState["ShadowColor"] = value;
				this.SetBit(2097152);
			}
		}

		// Token: 0x17001733 RID: 5939
		// (get) Token: 0x060059D1 RID: 22993 RVA: 0x0016B085 File Offset: 0x0016A085
		// (set) Token: 0x060059D2 RID: 22994 RVA: 0x0016B08D File Offset: 0x0016A08D
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override HorizontalAlign HorizontalAlign
		{
			get
			{
				return base.HorizontalAlign;
			}
			set
			{
			}
		}

		// Token: 0x060059D3 RID: 22995 RVA: 0x0016B090 File Offset: 0x0016A090
		protected override void FillStyleAttributes(CssStyleCollection attributes, IUrlResolutionService urlResolver)
		{
			base.FillStyleAttributes(attributes, urlResolver);
			Color shadowColor = this.ShadowColor;
			if (!shadowColor.IsEmpty)
			{
				string text = ColorTranslator.ToHtml(shadowColor);
				string text2 = "progid:DXImageTransform.Microsoft.Shadow(color='" + text + "', Direction=135, Strength=3)";
				attributes.Add(HtmlTextWriterStyle.Filter, text2);
			}
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x0016B0D8 File Offset: 0x0016A0D8
		public override void CopyFrom(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				base.CopyFrom(s);
				if (s is WebPartMenuStyle)
				{
					WebPartMenuStyle webPartMenuStyle = (WebPartMenuStyle)s;
					if (s.RegisteredCssClass.Length != 0)
					{
						if (webPartMenuStyle.IsSet(2097152))
						{
							base.ViewState.Remove("ShadowColor");
							base.ClearBit(2097152);
							return;
						}
					}
					else if (webPartMenuStyle.IsSet(2097152))
					{
						this.ShadowColor = webPartMenuStyle.ShadowColor;
					}
				}
			}
		}

		// Token: 0x060059D5 RID: 22997 RVA: 0x0016B158 File Offset: 0x0016A158
		public override void MergeWith(Style s)
		{
			if (s != null && !s.IsEmpty)
			{
				if (this.IsEmpty)
				{
					this.CopyFrom(s);
					return;
				}
				base.MergeWith(s);
				if (s is WebPartMenuStyle)
				{
					WebPartMenuStyle webPartMenuStyle = (WebPartMenuStyle)s;
					if (s.RegisteredCssClass.Length == 0 && webPartMenuStyle.IsSet(2097152) && !base.IsSet(2097152))
					{
						this.ShadowColor = webPartMenuStyle.ShadowColor;
					}
				}
			}
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x0016B1C9 File Offset: 0x0016A1C9
		public override void Reset()
		{
			if (base.IsSet(2097152))
			{
				base.ViewState.Remove("ShadowColor");
			}
			base.Reset();
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x0016B1EE File Offset: 0x0016A1EE
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes(this, true);
		}

		// Token: 0x060059D8 RID: 23000 RVA: 0x0016B1F7 File Offset: 0x0016A1F7
		string ICustomTypeDescriptor.GetClassName()
		{
			return TypeDescriptor.GetClassName(this, true);
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x0016B200 File Offset: 0x0016A200
		string ICustomTypeDescriptor.GetComponentName()
		{
			return TypeDescriptor.GetComponentName(this, true);
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x0016B209 File Offset: 0x0016A209
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return TypeDescriptor.GetConverter(this, true);
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x0016B212 File Offset: 0x0016A212
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(this, true);
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x0016B21B File Offset: 0x0016A21B
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(this, true);
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x0016B224 File Offset: 0x0016A224
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(this, editorBaseType, true);
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x0016B22E File Offset: 0x0016A22E
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(this, true);
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x0016B237 File Offset: 0x0016A237
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(this, attributes, true);
		}

		// Token: 0x060059E0 RID: 23008 RVA: 0x0016B241 File Offset: 0x0016A241
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x060059E1 RID: 23009 RVA: 0x0016B24C File Offset: 0x0016A24C
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(base.GetType(), attributes);
			PropertyDescriptor[] array = new PropertyDescriptor[properties.Count];
			PropertyDescriptor propertyDescriptor = properties["CellPadding"];
			PropertyDescriptor propertyDescriptor2 = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor, new Attribute[]
			{
				new DefaultValueAttribute(1)
			});
			PropertyDescriptor propertyDescriptor3 = properties["CellSpacing"];
			PropertyDescriptor propertyDescriptor4 = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor3, new Attribute[]
			{
				new DefaultValueAttribute(0)
			});
			PropertyDescriptor propertyDescriptor5 = properties["Font"];
			PropertyDescriptor propertyDescriptor6 = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor5, new Attribute[]
			{
				new BrowsableAttribute(false),
				new ThemeableAttribute(false),
				new EditorBrowsableAttribute(EditorBrowsableState.Never)
			});
			PropertyDescriptor propertyDescriptor7 = properties["ForeColor"];
			PropertyDescriptor propertyDescriptor8 = TypeDescriptor.CreateProperty(base.GetType(), propertyDescriptor7, new Attribute[]
			{
				new BrowsableAttribute(false),
				new ThemeableAttribute(false),
				new EditorBrowsableAttribute(EditorBrowsableState.Never)
			});
			for (int i = 0; i < properties.Count; i++)
			{
				PropertyDescriptor propertyDescriptor9 = properties[i];
				if (propertyDescriptor9 == propertyDescriptor)
				{
					array[i] = propertyDescriptor2;
				}
				else if (propertyDescriptor9 == propertyDescriptor3)
				{
					array[i] = propertyDescriptor4;
				}
				else if (propertyDescriptor9 == propertyDescriptor5)
				{
					array[i] = propertyDescriptor6;
				}
				else if (propertyDescriptor9 == propertyDescriptor7)
				{
					array[i] = propertyDescriptor8;
				}
				else
				{
					array[i] = propertyDescriptor9;
				}
			}
			return new PropertyDescriptorCollection(array, true);
		}

		// Token: 0x060059E2 RID: 23010 RVA: 0x0016B3B9 File Offset: 0x0016A3B9
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x04003066 RID: 12390
		private const int PROP_SHADOWCOLOR = 2097152;
	}
}
