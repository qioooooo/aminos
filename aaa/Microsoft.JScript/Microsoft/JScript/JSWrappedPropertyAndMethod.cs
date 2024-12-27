using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000C8 RID: 200
	internal class JSWrappedPropertyAndMethod : JSWrappedProperty
	{
		// Token: 0x06000919 RID: 2329 RVA: 0x00045DB1 File Offset: 0x00044DB1
		internal JSWrappedPropertyAndMethod(PropertyInfo property, MethodInfo method, object obj)
			: base(property, obj)
		{
			this.method = method;
			this.parameters = method.GetParameters();
		}

		// Token: 0x0600091A RID: 2330 RVA: 0x00045DD0 File Offset: 0x00044DD0
		private object[] CheckArguments(object[] arguments)
		{
			if (arguments == null)
			{
				return arguments;
			}
			int num = arguments.Length;
			int num2 = this.parameters.Length;
			if (num >= num2)
			{
				return arguments;
			}
			object[] array = new object[num2];
			ArrayObject.Copy(arguments, array, num);
			for (int i = num; i < num2; i++)
			{
				array[i] = Type.Missing;
			}
			return array;
		}

		// Token: 0x0600091B RID: 2331 RVA: 0x00045E19 File Offset: 0x00044E19
		internal object Invoke(object obj, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			parameters = this.CheckArguments(parameters);
			if (this.obj != null && !(this.obj is Type))
			{
				obj = this.obj;
			}
			return this.method.Invoke(obj, options, binder, parameters, culture);
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x00045E54 File Offset: 0x00044E54
		public MethodInfo Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x0400055C RID: 1372
		protected MethodInfo method;

		// Token: 0x0400055D RID: 1373
		private ParameterInfo[] parameters;
	}
}
