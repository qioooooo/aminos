using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000B3 RID: 179
	public sealed class JSMethodInfo : MethodInfo
	{
		// Token: 0x06000815 RID: 2069 RVA: 0x000389C6 File Offset: 0x000379C6
		internal JSMethodInfo(MethodInfo method)
		{
			this.method = method;
			this.methAttributes = method.Attributes;
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000816 RID: 2070 RVA: 0x000389E1 File Offset: 0x000379E1
		public override MethodAttributes Attributes
		{
			get
			{
				return this.methAttributes;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x000389EC File Offset: 0x000379EC
		public override Type DeclaringType
		{
			get
			{
				Type type = this.declaringType;
				if (type == null)
				{
					type = (this.declaringType = this.method.DeclaringType);
				}
				return type;
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00038A17 File Offset: 0x00037A17
		public override MethodInfo GetBaseDefinition()
		{
			return this.method.GetBaseDefinition();
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00038A24 File Offset: 0x00037A24
		public sealed override object[] GetCustomAttributes(bool inherit)
		{
			object[] array = this.attributes;
			if (array != null)
			{
				return array;
			}
			return this.attributes = this.method.GetCustomAttributes(true);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00038A54 File Offset: 0x00037A54
		public sealed override object[] GetCustomAttributes(Type type, bool inherit)
		{
			if (type != typeof(JSFunctionAttribute))
			{
				return null;
			}
			object[] array = this.attributes;
			if (array != null)
			{
				return array;
			}
			return this.attributes = CustomAttribute.GetCustomAttributes(this.method, type, true);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00038A92 File Offset: 0x00037A92
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.method.GetMethodImplementationFlags();
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00038AA0 File Offset: 0x00037AA0
		public override ParameterInfo[] GetParameters()
		{
			ParameterInfo[] array = this.parameters;
			if (array != null)
			{
				return array;
			}
			array = this.method.GetParameters();
			int i = 0;
			int num = array.Length;
			while (i < num)
			{
				array[i] = new JSParameterInfo(array[i]);
				i++;
			}
			return this.parameters = array;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00038AEC File Offset: 0x00037AEC
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(object obj, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			MethodInfo methodInfo = TypeReferences.ToExecutionContext(this.method);
			if (binder != null)
			{
				try
				{
					return methodInfo.Invoke(obj, options, binder, parameters, culture);
				}
				catch (TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
			}
			MethodInvoker methodInvoker = this.methodInvoker;
			if (methodInvoker == null)
			{
				methodInvoker = (this.methodInvoker = MethodInvoker.GetInvokerFor(methodInfo));
				if (methodInvoker == null)
				{
					try
					{
						return methodInfo.Invoke(obj, options, binder, parameters, culture);
					}
					catch (TargetInvocationException ex2)
					{
						throw ex2.InnerException;
					}
				}
			}
			return methodInvoker.Invoke(obj, parameters);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00038B80 File Offset: 0x00037B80
		public sealed override bool IsDefined(Type type, bool inherit)
		{
			object[] array = this.attributes;
			if (array == null)
			{
				array = (this.attributes = CustomAttribute.GetCustomAttributes(this.method, type, true));
			}
			return array.Length > 0;
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00038BB2 File Offset: 0x00037BB2
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00038BB5 File Offset: 0x00037BB5
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.method.MethodHandle;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00038BC4 File Offset: 0x00037BC4
		public override string Name
		{
			get
			{
				string text = this.name;
				if (text == null)
				{
					text = (this.name = this.method.Name);
				}
				return text;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00038BEF File Offset: 0x00037BEF
		public override Type ReflectedType
		{
			get
			{
				return this.method.ReflectedType;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00038BFC File Offset: 0x00037BFC
		public override Type ReturnType
		{
			get
			{
				return this.method.ReturnType;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00038C09 File Offset: 0x00037C09
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return this.method.ReturnTypeCustomAttributes;
			}
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00038C16 File Offset: 0x00037C16
		public override string ToString()
		{
			return this.method.ToString();
		}

		// Token: 0x04000456 RID: 1110
		internal MethodInfo method;

		// Token: 0x04000457 RID: 1111
		private MethodAttributes methAttributes;

		// Token: 0x04000458 RID: 1112
		private string name;

		// Token: 0x04000459 RID: 1113
		private Type declaringType;

		// Token: 0x0400045A RID: 1114
		private ParameterInfo[] parameters;

		// Token: 0x0400045B RID: 1115
		private object[] attributes;

		// Token: 0x0400045C RID: 1116
		private MethodInvoker methodInvoker;
	}
}
