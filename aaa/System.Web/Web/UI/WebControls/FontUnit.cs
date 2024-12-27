using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200057E RID: 1406
	[TypeConverter(typeof(FontUnitConverter))]
	[Serializable]
	public struct FontUnit
	{
		// Token: 0x060044E8 RID: 17640 RVA: 0x0011B140 File Offset: 0x0011A140
		public FontUnit(FontSize type)
		{
			if (type < FontSize.NotSet || type > FontSize.XXLarge)
			{
				throw new ArgumentOutOfRangeException("type");
			}
			this.type = type;
			if (this.type == FontSize.AsUnit)
			{
				this.value = Unit.Point(10);
				return;
			}
			this.value = Unit.Empty;
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x0011B17F File Offset: 0x0011A17F
		public FontUnit(Unit value)
		{
			this.type = FontSize.NotSet;
			if (!value.IsEmpty)
			{
				this.type = FontSize.AsUnit;
				this.value = value;
				return;
			}
			this.value = Unit.Empty;
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x0011B1AB File Offset: 0x0011A1AB
		public FontUnit(int value)
		{
			this.type = FontSize.AsUnit;
			this.value = Unit.Point(value);
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x0011B1C0 File Offset: 0x0011A1C0
		public FontUnit(double value)
		{
			this = new FontUnit(new Unit(value, UnitType.Point));
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x0011B1CF File Offset: 0x0011A1CF
		public FontUnit(double value, UnitType type)
		{
			this = new FontUnit(new Unit(value, type));
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x0011B1DE File Offset: 0x0011A1DE
		public FontUnit(string value)
		{
			this = new FontUnit(value, CultureInfo.CurrentCulture);
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x0011B1EC File Offset: 0x0011A1EC
		public FontUnit(string value, CultureInfo culture)
		{
			this.type = FontSize.NotSet;
			this.value = Unit.Empty;
			if (!string.IsNullOrEmpty(value))
			{
				char c = char.ToLower(value[0], CultureInfo.InvariantCulture);
				if (c == 'x')
				{
					if (string.Equals(value, "xx-small", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "xxsmall", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.XXSmall;
						return;
					}
					if (string.Equals(value, "x-small", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "xsmall", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.XSmall;
						return;
					}
					if (string.Equals(value, "x-large", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "xlarge", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.XLarge;
						return;
					}
					if (string.Equals(value, "xx-large", StringComparison.OrdinalIgnoreCase) || string.Equals(value, "xxlarge", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.XXLarge;
						return;
					}
				}
				else if (c == 's')
				{
					if (string.Equals(value, "small", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.Small;
						return;
					}
					if (string.Equals(value, "smaller", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.Smaller;
						return;
					}
				}
				else if (c == 'l')
				{
					if (string.Equals(value, "large", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.Large;
						return;
					}
					if (string.Equals(value, "larger", StringComparison.OrdinalIgnoreCase))
					{
						this.type = FontSize.Larger;
						return;
					}
				}
				else if (c == 'm' && string.Equals(value, "medium", StringComparison.OrdinalIgnoreCase))
				{
					this.type = FontSize.Medium;
					return;
				}
				this.value = new Unit(value, culture, UnitType.Point);
				this.type = FontSize.AsUnit;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x060044EF RID: 17647 RVA: 0x0011B357 File Offset: 0x0011A357
		public bool IsEmpty
		{
			get
			{
				return this.type == FontSize.NotSet;
			}
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x060044F0 RID: 17648 RVA: 0x0011B362 File Offset: 0x0011A362
		public FontSize Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x060044F1 RID: 17649 RVA: 0x0011B36A File Offset: 0x0011A36A
		public Unit Unit
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x0011B374 File Offset: 0x0011A374
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this.type.GetHashCode(), this.value.GetHashCode());
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x0011B3AC File Offset: 0x0011A3AC
		public override bool Equals(object obj)
		{
			if (obj == null || !(obj is FontUnit))
			{
				return false;
			}
			FontUnit fontUnit = (FontUnit)obj;
			return fontUnit.type == this.type && fontUnit.value == this.value;
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x0011B3F3 File Offset: 0x0011A3F3
		public static bool operator ==(FontUnit left, FontUnit right)
		{
			return left.type == right.type && left.value == right.value;
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0011B41A File Offset: 0x0011A41A
		public static bool operator !=(FontUnit left, FontUnit right)
		{
			return left.type != right.type || left.value != right.value;
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x0011B441 File Offset: 0x0011A441
		public static FontUnit Parse(string s)
		{
			return new FontUnit(s, CultureInfo.InvariantCulture);
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x0011B44E File Offset: 0x0011A44E
		public static FontUnit Parse(string s, CultureInfo culture)
		{
			return new FontUnit(s, culture);
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x0011B457 File Offset: 0x0011A457
		public static FontUnit Point(int n)
		{
			return new FontUnit(n);
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x0011B45F File Offset: 0x0011A45F
		public override string ToString()
		{
			return this.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x0011B46C File Offset: 0x0011A46C
		public string ToString(CultureInfo culture)
		{
			return this.ToString(culture);
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x0011B478 File Offset: 0x0011A478
		public string ToString(IFormatProvider formatProvider)
		{
			string empty = string.Empty;
			if (this.IsEmpty)
			{
				return empty;
			}
			FontSize fontSize = this.type;
			switch (fontSize)
			{
			case FontSize.AsUnit:
				return this.value.ToString(formatProvider);
			case FontSize.Smaller:
			case FontSize.Larger:
				break;
			case FontSize.XXSmall:
				return "XX-Small";
			case FontSize.XSmall:
				return "X-Small";
			default:
				switch (fontSize)
				{
				case FontSize.XLarge:
					return "X-Large";
				case FontSize.XXLarge:
					return "XX-Large";
				}
				break;
			}
			return PropertyConverter.EnumToString(typeof(FontSize), this.type);
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x0011B519 File Offset: 0x0011A519
		public static implicit operator FontUnit(int n)
		{
			return FontUnit.Point(n);
		}

		// Token: 0x040029CD RID: 10701
		public static readonly FontUnit Empty = default(FontUnit);

		// Token: 0x040029CE RID: 10702
		public static readonly FontUnit Smaller = new FontUnit(FontSize.Smaller);

		// Token: 0x040029CF RID: 10703
		public static readonly FontUnit Larger = new FontUnit(FontSize.Larger);

		// Token: 0x040029D0 RID: 10704
		public static readonly FontUnit XXSmall = new FontUnit(FontSize.XXSmall);

		// Token: 0x040029D1 RID: 10705
		public static readonly FontUnit XSmall = new FontUnit(FontSize.XSmall);

		// Token: 0x040029D2 RID: 10706
		public static readonly FontUnit Small = new FontUnit(FontSize.Small);

		// Token: 0x040029D3 RID: 10707
		public static readonly FontUnit Medium = new FontUnit(FontSize.Medium);

		// Token: 0x040029D4 RID: 10708
		public static readonly FontUnit Large = new FontUnit(FontSize.Large);

		// Token: 0x040029D5 RID: 10709
		public static readonly FontUnit XLarge = new FontUnit(FontSize.XLarge);

		// Token: 0x040029D6 RID: 10710
		public static readonly FontUnit XXLarge = new FontUnit(FontSize.XXLarge);

		// Token: 0x040029D7 RID: 10711
		private readonly FontSize type;

		// Token: 0x040029D8 RID: 10712
		private readonly Unit value;
	}
}
