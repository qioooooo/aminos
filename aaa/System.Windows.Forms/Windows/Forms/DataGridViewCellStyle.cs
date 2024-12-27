using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200031F RID: 799
	[Editor("System.Windows.Forms.Design.DataGridViewCellStyleEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[TypeConverter(typeof(DataGridViewCellStyleConverter))]
	public class DataGridViewCellStyle : ICloneable
	{
		// Token: 0x0600337D RID: 13181 RVA: 0x000B466C File Offset: 0x000B366C
		public DataGridViewCellStyle()
		{
			this.propertyStore = new PropertyStore();
			this.scope = DataGridViewCellStyleScopes.None;
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x000B4688 File Offset: 0x000B3688
		public DataGridViewCellStyle(DataGridViewCellStyle dataGridViewCellStyle)
		{
			if (dataGridViewCellStyle == null)
			{
				throw new ArgumentNullException("dataGridViewCellStyle");
			}
			this.propertyStore = new PropertyStore();
			this.scope = DataGridViewCellStyleScopes.None;
			this.BackColor = dataGridViewCellStyle.BackColor;
			this.ForeColor = dataGridViewCellStyle.ForeColor;
			this.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
			this.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
			this.Font = dataGridViewCellStyle.Font;
			this.NullValue = dataGridViewCellStyle.NullValue;
			this.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
			this.Format = dataGridViewCellStyle.Format;
			if (!dataGridViewCellStyle.IsFormatProviderDefault)
			{
				this.FormatProvider = dataGridViewCellStyle.FormatProvider;
			}
			this.AlignmentInternal = dataGridViewCellStyle.Alignment;
			this.WrapModeInternal = dataGridViewCellStyle.WrapMode;
			this.Tag = dataGridViewCellStyle.Tag;
			this.PaddingInternal = dataGridViewCellStyle.Padding;
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x0600337F RID: 13183 RVA: 0x000B4760 File Offset: 0x000B3760
		// (set) Token: 0x06003380 RID: 13184 RVA: 0x000B4788 File Offset: 0x000B3788
		[DefaultValue(DataGridViewContentAlignment.NotSet)]
		[SRDescription("DataGridViewCellStyleAlignmentDescr")]
		[SRCategory("CatLayout")]
		public DataGridViewContentAlignment Alignment
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(DataGridViewCellStyle.PropAlignment, out flag);
				if (flag)
				{
					return (DataGridViewContentAlignment)integer;
				}
				return DataGridViewContentAlignment.NotSet;
			}
			set
			{
				if (value <= DataGridViewContentAlignment.MiddleCenter)
				{
					switch (value)
					{
					case DataGridViewContentAlignment.NotSet:
					case DataGridViewContentAlignment.TopLeft:
					case DataGridViewContentAlignment.TopCenter:
					case DataGridViewContentAlignment.TopRight:
						goto IL_006A;
					case (DataGridViewContentAlignment)3:
						break;
					default:
						if (value == DataGridViewContentAlignment.MiddleLeft || value == DataGridViewContentAlignment.MiddleCenter)
						{
							goto IL_006A;
						}
						break;
					}
				}
				else if (value <= DataGridViewContentAlignment.BottomLeft)
				{
					if (value == DataGridViewContentAlignment.MiddleRight || value == DataGridViewContentAlignment.BottomLeft)
					{
						goto IL_006A;
					}
				}
				else if (value == DataGridViewContentAlignment.BottomCenter || value == DataGridViewContentAlignment.BottomRight)
				{
					goto IL_006A;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewContentAlignment));
				IL_006A:
				this.AlignmentInternal = value;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (set) Token: 0x06003381 RID: 13185 RVA: 0x000B4806 File Offset: 0x000B3806
		internal DataGridViewContentAlignment AlignmentInternal
		{
			set
			{
				if (this.Alignment != value)
				{
					this.Properties.SetInteger(DataGridViewCellStyle.PropAlignment, (int)value);
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003382 RID: 13186 RVA: 0x000B4829 File Offset: 0x000B3829
		// (set) Token: 0x06003383 RID: 13187 RVA: 0x000B483C File Offset: 0x000B383C
		[SRCategory("CatAppearance")]
		public Color BackColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropBackColor);
			}
			set
			{
				Color backColor = this.BackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropBackColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropBackColor, value);
				}
				if (!backColor.Equals(this.BackColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Color);
				}
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003384 RID: 13188 RVA: 0x000B489D File Offset: 0x000B389D
		// (set) Token: 0x06003385 RID: 13189 RVA: 0x000B48C8 File Offset: 0x000B38C8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public object DataSourceNullValue
		{
			get
			{
				if (this.Properties.ContainsObject(DataGridViewCellStyle.PropDataSourceNullValue))
				{
					return this.Properties.GetObject(DataGridViewCellStyle.PropDataSourceNullValue);
				}
				return DBNull.Value;
			}
			set
			{
				object dataSourceNullValue = this.DataSourceNullValue;
				if (dataSourceNullValue == value || (dataSourceNullValue != null && dataSourceNullValue.Equals(value)))
				{
					return;
				}
				if (value == DBNull.Value && this.Properties.ContainsObject(DataGridViewCellStyle.PropDataSourceNullValue))
				{
					this.Properties.RemoveObject(DataGridViewCellStyle.PropDataSourceNullValue);
				}
				else
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropDataSourceNullValue, value);
				}
				this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003386 RID: 13190 RVA: 0x000B4931 File Offset: 0x000B3931
		// (set) Token: 0x06003387 RID: 13191 RVA: 0x000B4948 File Offset: 0x000B3948
		[SRCategory("CatAppearance")]
		public Font Font
		{
			get
			{
				return (Font)this.Properties.GetObject(DataGridViewCellStyle.PropFont);
			}
			set
			{
				Font font = this.Font;
				if (value != null || this.Properties.ContainsObject(DataGridViewCellStyle.PropFont))
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropFont, value);
				}
				if ((font == null && value != null) || (font != null && value == null) || (font != null && value != null && !font.Equals(this.Font)))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Font);
				}
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003388 RID: 13192 RVA: 0x000B49A9 File Offset: 0x000B39A9
		// (set) Token: 0x06003389 RID: 13193 RVA: 0x000B49BC File Offset: 0x000B39BC
		[SRCategory("CatAppearance")]
		public Color ForeColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropForeColor);
			}
			set
			{
				Color foreColor = this.ForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropForeColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropForeColor, value);
				}
				if (!foreColor.Equals(this.ForeColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.ForeColor);
				}
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600338A RID: 13194 RVA: 0x000B4A20 File Offset: 0x000B3A20
		// (set) Token: 0x0600338B RID: 13195 RVA: 0x000B4A50 File Offset: 0x000B3A50
		[SRCategory("CatBehavior")]
		[Editor("System.Windows.Forms.Design.FormatStringEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[DefaultValue("")]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string Format
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropFormat);
				if (@object == null)
				{
					return string.Empty;
				}
				return (string)@object;
			}
			set
			{
				string format = this.Format;
				if ((value != null && value.Length > 0) || this.Properties.ContainsObject(DataGridViewCellStyle.PropFormat))
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropFormat, value);
				}
				if (!format.Equals(this.Format))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600338C RID: 13196 RVA: 0x000B4AA8 File Offset: 0x000B3AA8
		// (set) Token: 0x0600338D RID: 13197 RVA: 0x000B4AD8 File Offset: 0x000B3AD8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IFormatProvider FormatProvider
		{
			get
			{
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider);
				if (@object == null)
				{
					return CultureInfo.CurrentCulture;
				}
				return (IFormatProvider)@object;
			}
			set
			{
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider);
				this.Properties.SetObject(DataGridViewCellStyle.PropFormatProvider, value);
				if (value != @object)
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x0600338E RID: 13198 RVA: 0x000B4B12 File Offset: 0x000B3B12
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsDataSourceNullValueDefault
		{
			get
			{
				return !this.Properties.ContainsObject(DataGridViewCellStyle.PropDataSourceNullValue) || this.Properties.GetObject(DataGridViewCellStyle.PropDataSourceNullValue) == DBNull.Value;
			}
		}

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x0600338F RID: 13199 RVA: 0x000B4B3F File Offset: 0x000B3B3F
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public bool IsFormatProviderDefault
		{
			get
			{
				return this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider) == null;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06003390 RID: 13200 RVA: 0x000B4B54 File Offset: 0x000B3B54
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[Browsable(false)]
		public bool IsNullValueDefault
		{
			get
			{
				if (!this.Properties.ContainsObject(DataGridViewCellStyle.PropNullValue))
				{
					return true;
				}
				object @object = this.Properties.GetObject(DataGridViewCellStyle.PropNullValue);
				return @object is string && @object.Equals("");
			}
		}

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06003391 RID: 13201 RVA: 0x000B4B9B File Offset: 0x000B3B9B
		// (set) Token: 0x06003392 RID: 13202 RVA: 0x000B4BC8 File Offset: 0x000B3BC8
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue("")]
		[SRCategory("CatData")]
		public object NullValue
		{
			get
			{
				if (this.Properties.ContainsObject(DataGridViewCellStyle.PropNullValue))
				{
					return this.Properties.GetObject(DataGridViewCellStyle.PropNullValue);
				}
				return "";
			}
			set
			{
				object nullValue = this.NullValue;
				if (nullValue == value || (nullValue != null && nullValue.Equals(value)))
				{
					return;
				}
				if (value is string && value.Equals("") && this.Properties.ContainsObject(DataGridViewCellStyle.PropNullValue))
				{
					this.Properties.RemoveObject(DataGridViewCellStyle.PropNullValue);
				}
				else
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropNullValue, value);
				}
				this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
			}
		}

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06003393 RID: 13203 RVA: 0x000B4C3E File Offset: 0x000B3C3E
		// (set) Token: 0x06003394 RID: 13204 RVA: 0x000B4C50 File Offset: 0x000B3C50
		[SRCategory("CatLayout")]
		public Padding Padding
		{
			get
			{
				return this.Properties.GetPadding(DataGridViewCellStyle.PropPadding);
			}
			set
			{
				if (value.Left < 0 || value.Right < 0 || value.Top < 0 || value.Bottom < 0)
				{
					if (value.All != -1)
					{
						value.All = 0;
					}
					else
					{
						value.Left = Math.Max(0, value.Left);
						value.Right = Math.Max(0, value.Right);
						value.Top = Math.Max(0, value.Top);
						value.Bottom = Math.Max(0, value.Bottom);
					}
				}
				this.PaddingInternal = value;
			}
		}

		// Token: 0x17000936 RID: 2358
		// (set) Token: 0x06003395 RID: 13205 RVA: 0x000B4CF0 File Offset: 0x000B3CF0
		internal Padding PaddingInternal
		{
			set
			{
				if (value != this.Padding)
				{
					this.Properties.SetPadding(DataGridViewCellStyle.PropPadding, value);
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06003396 RID: 13206 RVA: 0x000B4D18 File Offset: 0x000B3D18
		internal PropertyStore Properties
		{
			get
			{
				return this.propertyStore;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06003397 RID: 13207 RVA: 0x000B4D20 File Offset: 0x000B3D20
		// (set) Token: 0x06003398 RID: 13208 RVA: 0x000B4D28 File Offset: 0x000B3D28
		internal DataGridViewCellStyleScopes Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				this.scope = value;
			}
		}

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06003399 RID: 13209 RVA: 0x000B4D31 File Offset: 0x000B3D31
		// (set) Token: 0x0600339A RID: 13210 RVA: 0x000B4D44 File Offset: 0x000B3D44
		[SRCategory("CatAppearance")]
		public Color SelectionBackColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropSelectionBackColor);
			}
			set
			{
				Color selectionBackColor = this.SelectionBackColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropSelectionBackColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropSelectionBackColor, value);
				}
				if (!selectionBackColor.Equals(this.SelectionBackColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Color);
				}
			}
		}

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x0600339B RID: 13211 RVA: 0x000B4DA5 File Offset: 0x000B3DA5
		// (set) Token: 0x0600339C RID: 13212 RVA: 0x000B4DB8 File Offset: 0x000B3DB8
		[SRCategory("CatAppearance")]
		public Color SelectionForeColor
		{
			get
			{
				return this.Properties.GetColor(DataGridViewCellStyle.PropSelectionForeColor);
			}
			set
			{
				Color selectionForeColor = this.SelectionForeColor;
				if (!value.IsEmpty || this.Properties.ContainsObject(DataGridViewCellStyle.PropSelectionForeColor))
				{
					this.Properties.SetColor(DataGridViewCellStyle.PropSelectionForeColor, value);
				}
				if (!selectionForeColor.Equals(this.SelectionForeColor))
				{
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Color);
				}
			}
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x0600339D RID: 13213 RVA: 0x000B4E19 File Offset: 0x000B3E19
		// (set) Token: 0x0600339E RID: 13214 RVA: 0x000B4E2B File Offset: 0x000B3E2B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public object Tag
		{
			get
			{
				return this.Properties.GetObject(DataGridViewCellStyle.PropTag);
			}
			set
			{
				if (value != null || this.Properties.ContainsObject(DataGridViewCellStyle.PropTag))
				{
					this.Properties.SetObject(DataGridViewCellStyle.PropTag, value);
				}
			}
		}

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x0600339F RID: 13215 RVA: 0x000B4E54 File Offset: 0x000B3E54
		// (set) Token: 0x060033A0 RID: 13216 RVA: 0x000B4E7A File Offset: 0x000B3E7A
		[SRCategory("CatLayout")]
		[DefaultValue(DataGridViewTriState.NotSet)]
		public DataGridViewTriState WrapMode
		{
			get
			{
				bool flag;
				int integer = this.Properties.GetInteger(DataGridViewCellStyle.PropWrapMode, out flag);
				if (flag)
				{
					return (DataGridViewTriState)integer;
				}
				return DataGridViewTriState.NotSet;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DataGridViewTriState));
				}
				this.WrapModeInternal = value;
			}
		}

		// Token: 0x1700093D RID: 2365
		// (set) Token: 0x060033A1 RID: 13217 RVA: 0x000B4EA9 File Offset: 0x000B3EA9
		internal DataGridViewTriState WrapModeInternal
		{
			set
			{
				if (this.WrapMode != value)
				{
					this.Properties.SetInteger(DataGridViewCellStyle.PropWrapMode, (int)value);
					this.OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal.Other);
				}
			}
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x000B4ECC File Offset: 0x000B3ECC
		internal void AddScope(DataGridView dataGridView, DataGridViewCellStyleScopes scope)
		{
			this.scope |= scope;
			this.dataGridView = dataGridView;
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x000B4EE4 File Offset: 0x000B3EE4
		public virtual void ApplyStyle(DataGridViewCellStyle dataGridViewCellStyle)
		{
			if (dataGridViewCellStyle == null)
			{
				throw new ArgumentNullException("dataGridViewCellStyle");
			}
			if (!dataGridViewCellStyle.BackColor.IsEmpty)
			{
				this.BackColor = dataGridViewCellStyle.BackColor;
			}
			if (!dataGridViewCellStyle.ForeColor.IsEmpty)
			{
				this.ForeColor = dataGridViewCellStyle.ForeColor;
			}
			if (!dataGridViewCellStyle.SelectionBackColor.IsEmpty)
			{
				this.SelectionBackColor = dataGridViewCellStyle.SelectionBackColor;
			}
			if (!dataGridViewCellStyle.SelectionForeColor.IsEmpty)
			{
				this.SelectionForeColor = dataGridViewCellStyle.SelectionForeColor;
			}
			if (dataGridViewCellStyle.Font != null)
			{
				this.Font = dataGridViewCellStyle.Font;
			}
			if (!dataGridViewCellStyle.IsNullValueDefault)
			{
				this.NullValue = dataGridViewCellStyle.NullValue;
			}
			if (!dataGridViewCellStyle.IsDataSourceNullValueDefault)
			{
				this.DataSourceNullValue = dataGridViewCellStyle.DataSourceNullValue;
			}
			if (dataGridViewCellStyle.Format.Length != 0)
			{
				this.Format = dataGridViewCellStyle.Format;
			}
			if (!dataGridViewCellStyle.IsFormatProviderDefault)
			{
				this.FormatProvider = dataGridViewCellStyle.FormatProvider;
			}
			if (dataGridViewCellStyle.Alignment != DataGridViewContentAlignment.NotSet)
			{
				this.AlignmentInternal = dataGridViewCellStyle.Alignment;
			}
			if (dataGridViewCellStyle.WrapMode != DataGridViewTriState.NotSet)
			{
				this.WrapModeInternal = dataGridViewCellStyle.WrapMode;
			}
			if (dataGridViewCellStyle.Tag != null)
			{
				this.Tag = dataGridViewCellStyle.Tag;
			}
			if (dataGridViewCellStyle.Padding != Padding.Empty)
			{
				this.PaddingInternal = dataGridViewCellStyle.Padding;
			}
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x000B5032 File Offset: 0x000B4032
		public virtual DataGridViewCellStyle Clone()
		{
			return new DataGridViewCellStyle(this);
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x000B503C File Offset: 0x000B403C
		public override bool Equals(object o)
		{
			DataGridViewCellStyle dataGridViewCellStyle = o as DataGridViewCellStyle;
			return dataGridViewCellStyle != null && this.GetDifferencesFrom(dataGridViewCellStyle) == DataGridViewCellStyleDifferences.None;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x000B5060 File Offset: 0x000B4060
		internal DataGridViewCellStyleDifferences GetDifferencesFrom(DataGridViewCellStyle dgvcs)
		{
			bool flag = dgvcs.Alignment != this.Alignment || dgvcs.DataSourceNullValue != this.DataSourceNullValue || dgvcs.Font != this.Font || dgvcs.Format != this.Format || dgvcs.FormatProvider != this.FormatProvider || dgvcs.NullValue != this.NullValue || dgvcs.Padding != this.Padding || dgvcs.Tag != this.Tag || dgvcs.WrapMode != this.WrapMode;
			bool flag2 = dgvcs.BackColor != this.BackColor || dgvcs.ForeColor != this.ForeColor || dgvcs.SelectionBackColor != this.SelectionBackColor || dgvcs.SelectionForeColor != this.SelectionForeColor;
			if (flag)
			{
				return DataGridViewCellStyleDifferences.AffectPreferredSize;
			}
			if (flag2)
			{
				return DataGridViewCellStyleDifferences.DoNotAffectPreferredSize;
			}
			return DataGridViewCellStyleDifferences.None;
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x000B5158 File Offset: 0x000B4158
		public override int GetHashCode()
		{
			return WindowsFormsUtils.GetCombinedHashCodes(new int[]
			{
				(int)this.Alignment,
				(int)this.WrapMode,
				this.Padding.GetHashCode(),
				this.Format.GetHashCode(),
				this.BackColor.GetHashCode(),
				this.ForeColor.GetHashCode(),
				this.SelectionBackColor.GetHashCode(),
				this.SelectionForeColor.GetHashCode(),
				(this.Font == null) ? 1 : this.Font.GetHashCode(),
				(this.NullValue == null) ? 1 : this.NullValue.GetHashCode(),
				(this.DataSourceNullValue == null) ? 1 : this.DataSourceNullValue.GetHashCode(),
				(this.Tag == null) ? 1 : this.Tag.GetHashCode()
			});
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x000B526F File Offset: 0x000B426F
		private void OnPropertyChanged(DataGridViewCellStyle.DataGridViewCellStylePropertyInternal property)
		{
			if (this.dataGridView != null && this.scope != DataGridViewCellStyleScopes.None)
			{
				this.dataGridView.OnCellStyleContentChanged(this, property);
			}
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x000B528E File Offset: 0x000B428E
		internal void RemoveScope(DataGridViewCellStyleScopes scope)
		{
			this.scope &= ~scope;
			if (this.scope == DataGridViewCellStyleScopes.None)
			{
				this.dataGridView = null;
			}
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x000B52B0 File Offset: 0x000B42B0
		private bool ShouldSerializeBackColor()
		{
			bool flag;
			this.Properties.GetColor(DataGridViewCellStyle.PropBackColor, out flag);
			return flag;
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x000B52D1 File Offset: 0x000B42D1
		private bool ShouldSerializeFont()
		{
			return this.Properties.GetObject(DataGridViewCellStyle.PropFont) != null;
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x000B52EC File Offset: 0x000B42EC
		private bool ShouldSerializeForeColor()
		{
			bool flag;
			this.Properties.GetColor(DataGridViewCellStyle.PropForeColor, out flag);
			return flag;
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x000B530D File Offset: 0x000B430D
		private bool ShouldSerializeFormatProvider()
		{
			return this.Properties.GetObject(DataGridViewCellStyle.PropFormatProvider) != null;
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x000B5325 File Offset: 0x000B4325
		private bool ShouldSerializePadding()
		{
			return this.Padding != Padding.Empty;
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x000B5338 File Offset: 0x000B4338
		private bool ShouldSerializeSelectionBackColor()
		{
			bool flag;
			this.Properties.GetObject(DataGridViewCellStyle.PropSelectionBackColor, out flag);
			return flag;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x000B535C File Offset: 0x000B435C
		private bool ShouldSerializeSelectionForeColor()
		{
			bool flag;
			this.Properties.GetColor(DataGridViewCellStyle.PropSelectionForeColor, out flag);
			return flag;
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x000B5380 File Offset: 0x000B4380
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			stringBuilder.Append("DataGridViewCellStyle {");
			bool flag = true;
			if (this.BackColor != Color.Empty)
			{
				stringBuilder.Append(" BackColor=" + this.BackColor.ToString());
				flag = false;
			}
			if (this.ForeColor != Color.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" ForeColor=" + this.ForeColor.ToString());
				flag = false;
			}
			if (this.SelectionBackColor != Color.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" SelectionBackColor=" + this.SelectionBackColor.ToString());
				flag = false;
			}
			if (this.SelectionForeColor != Color.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" SelectionForeColor=" + this.SelectionForeColor.ToString());
				flag = false;
			}
			if (this.Font != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Font=" + this.Font.ToString());
				flag = false;
			}
			if (!this.IsNullValueDefault && this.NullValue != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" NullValue=" + this.NullValue.ToString());
				flag = false;
			}
			if (!this.IsDataSourceNullValueDefault && this.DataSourceNullValue != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" DataSourceNullValue=" + this.DataSourceNullValue.ToString());
				flag = false;
			}
			if (!string.IsNullOrEmpty(this.Format))
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Format=" + this.Format);
				flag = false;
			}
			if (this.WrapMode != DataGridViewTriState.NotSet)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" WrapMode=" + this.WrapMode.ToString());
				flag = false;
			}
			if (this.Alignment != DataGridViewContentAlignment.NotSet)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Alignment=" + this.Alignment.ToString());
				flag = false;
			}
			if (this.Padding != Padding.Empty)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Padding=" + this.Padding.ToString());
				flag = false;
			}
			if (this.Tag != null)
			{
				if (!flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append(" Tag=" + this.Tag.ToString());
			}
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x000B56A1 File Offset: 0x000B46A1
		object ICloneable.Clone()
		{
			return this.Clone();
		}

		// Token: 0x04001AD2 RID: 6866
		private const string DATAGRIDVIEWCELLSTYLE_nullText = "";

		// Token: 0x04001AD3 RID: 6867
		private static readonly int PropAlignment = PropertyStore.CreateKey();

		// Token: 0x04001AD4 RID: 6868
		private static readonly int PropBackColor = PropertyStore.CreateKey();

		// Token: 0x04001AD5 RID: 6869
		private static readonly int PropDataSourceNullValue = PropertyStore.CreateKey();

		// Token: 0x04001AD6 RID: 6870
		private static readonly int PropFont = PropertyStore.CreateKey();

		// Token: 0x04001AD7 RID: 6871
		private static readonly int PropForeColor = PropertyStore.CreateKey();

		// Token: 0x04001AD8 RID: 6872
		private static readonly int PropFormat = PropertyStore.CreateKey();

		// Token: 0x04001AD9 RID: 6873
		private static readonly int PropFormatProvider = PropertyStore.CreateKey();

		// Token: 0x04001ADA RID: 6874
		private static readonly int PropNullValue = PropertyStore.CreateKey();

		// Token: 0x04001ADB RID: 6875
		private static readonly int PropPadding = PropertyStore.CreateKey();

		// Token: 0x04001ADC RID: 6876
		private static readonly int PropSelectionBackColor = PropertyStore.CreateKey();

		// Token: 0x04001ADD RID: 6877
		private static readonly int PropSelectionForeColor = PropertyStore.CreateKey();

		// Token: 0x04001ADE RID: 6878
		private static readonly int PropTag = PropertyStore.CreateKey();

		// Token: 0x04001ADF RID: 6879
		private static readonly int PropWrapMode = PropertyStore.CreateKey();

		// Token: 0x04001AE0 RID: 6880
		private DataGridViewCellStyleScopes scope;

		// Token: 0x04001AE1 RID: 6881
		private PropertyStore propertyStore;

		// Token: 0x04001AE2 RID: 6882
		private DataGridView dataGridView;

		// Token: 0x02000320 RID: 800
		internal enum DataGridViewCellStylePropertyInternal
		{
			// Token: 0x04001AE4 RID: 6884
			Color,
			// Token: 0x04001AE5 RID: 6885
			Other,
			// Token: 0x04001AE6 RID: 6886
			Font,
			// Token: 0x04001AE7 RID: 6887
			ForeColor
		}
	}
}
