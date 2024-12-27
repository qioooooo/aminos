using System;

namespace Microsoft.JScript
{
	// Token: 0x02000074 RID: 116
	internal sealed class MetadataEnumValue : EnumWrapper
	{
		// Token: 0x06000579 RID: 1401 RVA: 0x000260F0 File Offset: 0x000250F0
		internal static object GetEnumValue(Type type, object value)
		{
			if (!type.Assembly.ReflectionOnly)
			{
				return Enum.ToObject(type, value);
			}
			return new MetadataEnumValue(type, value);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0002610E File Offset: 0x0002510E
		private MetadataEnumValue(Type type, object value)
		{
			this._type = type;
			this._value = value;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00026124 File Offset: 0x00025124
		internal override object value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0002612C File Offset: 0x0002512C
		internal override Type type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00026134 File Offset: 0x00025134
		internal override string name
		{
			get
			{
				string text = Enum.GetName(this._type, this._value);
				if (text == null)
				{
					text = this._value.ToString();
				}
				return text;
			}
		}

		// Token: 0x04000252 RID: 594
		private Type _type;

		// Token: 0x04000253 RID: 595
		private object _value;
	}
}
