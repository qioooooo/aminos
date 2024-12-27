using System;

namespace Microsoft.JScript
{
	// Token: 0x020000D4 RID: 212
	public class NumberObject : JSObject
	{
		// Token: 0x060009A0 RID: 2464 RVA: 0x0004A028 File Offset: 0x00049028
		protected NumberObject(ScriptObject parent, object value)
			: base(parent)
		{
			this.baseType = Globals.TypeRefs.ToReferenceContext(value.GetType());
			this.value = value;
			this.noExpando = false;
			this.implicitWrapper = false;
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0004A05C File Offset: 0x0004905C
		internal NumberObject(ScriptObject parent, object value, bool implicitWrapper)
			: base(parent, typeof(NumberObject))
		{
			this.baseType = Globals.TypeRefs.ToReferenceContext(value.GetType());
			this.value = value;
			this.noExpando = implicitWrapper;
			this.implicitWrapper = implicitWrapper;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0004A09A File Offset: 0x0004909A
		internal NumberObject(ScriptObject parent, Type baseType)
			: base(parent)
		{
			this.baseType = baseType;
			this.value = 0.0;
			this.noExpando = false;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0004A0C8 File Offset: 0x000490C8
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			if (base.GetParent() is LenientNumberPrototype)
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

		// Token: 0x060009A4 RID: 2468 RVA: 0x0004A16B File Offset: 0x0004916B
		internal override string GetClassName()
		{
			return "Number";
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0004A172 File Offset: 0x00049172
		public new Type GetType()
		{
			if (!this.implicitWrapper)
			{
				return Typeob.NumberObject;
			}
			return this.baseType;
		}

		// Token: 0x0400060D RID: 1549
		internal Type baseType;

		// Token: 0x0400060E RID: 1550
		internal object value;

		// Token: 0x0400060F RID: 1551
		private bool implicitWrapper;
	}
}
