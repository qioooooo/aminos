using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000046 RID: 70
	internal class COMSetterMethod : COMMethodInfo
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x00015A24 File Offset: 0x00014A24
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			int num = parameters.Length - 1;
			object obj2 = parameters[num];
			object[] array;
			if (num > 0)
			{
				array = new object[num];
				ArrayObject.Copy(parameters, 0, array, 0, num);
			}
			else
			{
				array = new object[0];
			}
			this._comObject.SetValue(obj2, invokeAttr, binder, array, culture);
			return null;
		}
	}
}
