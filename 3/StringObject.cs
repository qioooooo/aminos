using System;

namespace Microsoft.JScript
{
	// Token: 0x020000DD RID: 221
	public class StringObject : JSObject
	{
		// Token: 0x060009D1 RID: 2513 RVA: 0x0004AC38 File Offset: 0x00049C38
		protected StringObject(ScriptObject prototype, string value)
			: base(prototype)
		{
			this.value = value;
			this.noExpando = false;
			this.implicitWrapper = false;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0004AC56 File Offset: 0x00049C56
		internal StringObject(ScriptObject prototype, string value, bool implicitWrapper)
			: base(prototype, typeof(StringObject))
		{
			this.value = value;
			this.noExpando = implicitWrapper;
			this.implicitWrapper = implicitWrapper;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0004AC7E File Offset: 0x00049C7E
		internal override string GetClassName()
		{
			return "String";
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0004AC85 File Offset: 0x00049C85
		public int length
		{
			get
			{
				return this.value.Length;
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0004AC92 File Offset: 0x00049C92
		public override bool Equals(object ob)
		{
			if (ob is StringObject)
			{
				ob = ((StringObject)ob).value;
			}
			return this.value.Equals(ob);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0004ACB8 File Offset: 0x00049CB8
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			if (base.GetParent() is LenientStringPrototype)
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
				return this.value;
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

		// Token: 0x060009D7 RID: 2519 RVA: 0x0004AD56 File Offset: 0x00049D56
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0004AD63 File Offset: 0x00049D63
		public new Type GetType()
		{
			if (!this.implicitWrapper)
			{
				return Typeob.StringObject;
			}
			return Typeob.String;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0004AD78 File Offset: 0x00049D78
		internal override object GetValueAtIndex(uint index)
		{
			if (this.implicitWrapper && (ulong)index < (ulong)((long)this.value.Length))
			{
				return this.value[(int)index];
			}
			return base.GetValueAtIndex(index);
		}

		// Token: 0x0400062C RID: 1580
		internal string value;

		// Token: 0x0400062D RID: 1581
		private bool implicitWrapper;
	}
}
