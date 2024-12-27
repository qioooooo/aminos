using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000045 RID: 69
	internal class COMGetterMethod : COMMethodInfo
	{
		// Token: 0x060002E5 RID: 741 RVA: 0x000159FB File Offset: 0x000149FB
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this._comObject.GetValue(invokeAttr, binder, (parameters != null) ? parameters : new object[0], culture);
		}
	}
}
