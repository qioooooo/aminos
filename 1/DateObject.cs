using System;

namespace Microsoft.JScript
{
	// Token: 0x02000058 RID: 88
	public class DateObject : JSObject
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x00021CA0 File Offset: 0x00020CA0
		internal DateObject(ScriptObject parent, double value)
			: base(parent)
		{
			this.value = ((value != value || value > 9.223372036854776E+18 || value < -9.223372036854776E+18) ? double.NaN : Math.Round(value));
			this.noExpando = false;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00021CEE File Offset: 0x00020CEE
		internal override string GetClassName()
		{
			return "Date";
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00021CF8 File Offset: 0x00020CF8
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			if (base.GetParent() is LenientDatePrototype)
			{
				return base.GetDefaultValue(preferred_type);
			}
			if (preferred_type == PreferredType.String || preferred_type == PreferredType.Either)
			{
				if (!this.noExpando)
				{
					object obj = base.NameTable["toString"];
					if (obj != null)
					{
						return base.GetDefaultValue(preferred_type);
					}
				}
				return DatePrototype.toString(this);
			}
			if (preferred_type == PreferredType.LocaleString)
			{
				if (!this.noExpando)
				{
					object obj2 = base.NameTable["toLocaleString"];
					if (obj2 != null)
					{
						return base.GetDefaultValue(preferred_type);
					}
				}
				return DatePrototype.toLocaleString(this);
			}
			if (!this.noExpando)
			{
				object obj3 = base.NameTable["valueOf"];
				if (obj3 == null && preferred_type == PreferredType.Either)
				{
					obj3 = base.NameTable["toString"];
				}
				if (obj3 != null)
				{
					return base.GetDefaultValue(preferred_type);
				}
			}
			return this.value;
		}

		// Token: 0x04000205 RID: 517
		internal double value;
	}
}
