using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000A7 RID: 167
	internal sealed class JSExpandoField : JSField
	{
		// Token: 0x060007BA RID: 1978 RVA: 0x00035887 File Offset: 0x00034887
		internal JSExpandoField(string name)
			: this(name, null)
		{
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00035891 File Offset: 0x00034891
		internal JSExpandoField(string name, object value)
		{
			this.value = value;
			this.name = name;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x000358A7 File Offset: 0x000348A7
		public override FieldAttributes Attributes
		{
			get
			{
				return FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static;
			}
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000358AB File Offset: 0x000348AB
		public override object GetValue(object obj)
		{
			return this.value;
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x000358B3 File Offset: 0x000348B3
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x000358BB File Offset: 0x000348BB
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale)
		{
			this.value = value;
		}

		// Token: 0x04000425 RID: 1061
		private object value;

		// Token: 0x04000426 RID: 1062
		private string name;
	}
}
