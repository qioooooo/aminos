using System;

namespace Microsoft.JScript
{
	// Token: 0x0200002F RID: 47
	public class BooleanObject : JSObject
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000EEBD File Offset: 0x0000DEBD
		protected BooleanObject(ScriptObject prototype, Type subType)
			: base(prototype, subType)
		{
			this.value = this.value;
			this.noExpando = false;
			this.implicitWrapper = false;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000EEE1 File Offset: 0x0000DEE1
		internal BooleanObject(ScriptObject prototype, bool value, bool implicitWrapper)
			: base(prototype, typeof(BooleanObject))
		{
			this.value = value;
			this.noExpando = implicitWrapper;
			this.implicitWrapper = implicitWrapper;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000EF09 File Offset: 0x0000DF09
		internal override string GetClassName()
		{
			return "Boolean";
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000EF10 File Offset: 0x0000DF10
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			if (base.GetParent() is LenientBooleanPrototype)
			{
				return base.GetDefaultValue(preferred_type);
			}
			if (preferred_type == PreferredType.String)
			{
				if (!this.noExpando)
				{
					object obj = base.NameTable["toString"];
					if (obj != null)
					{
						return base.GetDefaultValue(preferred_type);
					}
				}
				return Convert.ToString(this.value);
			}
			if (preferred_type == PreferredType.LocaleString)
			{
				return base.GetDefaultValue(preferred_type);
			}
			if (!this.noExpando)
			{
				object obj2 = base.NameTable["valueOf"];
				if (obj2 == null && preferred_type == PreferredType.Either)
				{
					obj2 = base.NameTable["toString"];
				}
				if (obj2 != null)
				{
					return base.GetDefaultValue(preferred_type);
				}
			}
			return this.value;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000EFB8 File Offset: 0x0000DFB8
		public new Type GetType()
		{
			if (!this.implicitWrapper)
			{
				return Typeob.BooleanObject;
			}
			return Typeob.Boolean;
		}

		// Token: 0x04000098 RID: 152
		internal bool value;

		// Token: 0x04000099 RID: 153
		private bool implicitWrapper;
	}
}
