using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000C2 RID: 194
	internal sealed class JSPrototypeField : JSField
	{
		// Token: 0x060008BF RID: 2239 RVA: 0x00041DD0 File Offset: 0x00040DD0
		internal JSPrototypeField(object prototypeObject, FieldInfo prototypeField)
		{
			this.prototypeObject = prototypeObject;
			this.prototypeField = prototypeField;
			this.value = Missing.Value;
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00041DF1 File Offset: 0x00040DF1
		public override FieldAttributes Attributes
		{
			get
			{
				return FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static;
			}
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x00041DF5 File Offset: 0x00040DF5
		public override object GetValue(object obj)
		{
			if (this.value is Missing)
			{
				return this.prototypeField.GetValue(this.prototypeObject);
			}
			return this.value;
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00041E1C File Offset: 0x00040E1C
		public override string Name
		{
			get
			{
				return this.prototypeField.Name;
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00041E29 File Offset: 0x00040E29
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale)
		{
			this.value = value;
		}

		// Token: 0x040004AE RID: 1198
		private object prototypeObject;

		// Token: 0x040004AF RID: 1199
		internal FieldInfo prototypeField;

		// Token: 0x040004B0 RID: 1200
		internal object value;
	}
}
