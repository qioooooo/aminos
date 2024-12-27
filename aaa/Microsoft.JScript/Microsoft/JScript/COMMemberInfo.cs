using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000040 RID: 64
	[Guid("84BCEB62-16EB-4e1c-975C-FCB40D331043")]
	[ComVisible(true)]
	public interface COMMemberInfo
	{
		// Token: 0x0600029B RID: 667
		object Call(BindingFlags invokeAttr, Binder binder, object[] arguments, CultureInfo culture);

		// Token: 0x0600029C RID: 668
		object GetValue(BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);

		// Token: 0x0600029D RID: 669
		void SetValue(object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture);
	}
}
