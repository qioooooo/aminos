using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000B4 RID: 180
	internal sealed class JSNativeMethod : JSMethod
	{
		// Token: 0x06000826 RID: 2086 RVA: 0x00038C24 File Offset: 0x00037C24
		internal JSNativeMethod(MethodInfo method, object obj, VsaEngine engine)
			: base(obj)
		{
			this.method = method;
			this.formalParams = method.GetParameters();
			object[] customAttributes = CustomAttribute.GetCustomAttributes(method, typeof(JSFunctionAttribute), false);
			JSFunctionAttribute jsfunctionAttribute = ((customAttributes.Length > 0) ? ((JSFunctionAttribute)customAttributes[0]) : new JSFunctionAttribute(JSFunctionAttributeEnum.None));
			JSFunctionAttributeEnum attributeValue = jsfunctionAttribute.attributeValue;
			if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
			{
				this.hasThis = true;
			}
			if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
			{
				this.hasEngine = true;
			}
			if ((attributeValue & JSFunctionAttributeEnum.HasVarArgs) != JSFunctionAttributeEnum.None)
			{
				this.hasVarargs = true;
			}
			this.engine = engine;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00038CA8 File Offset: 0x00037CA8
		internal override object Construct(object[] args)
		{
			throw new JScriptException(JSError.NoConstructor);
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00038CB4 File Offset: 0x00037CB4
		public override MethodAttributes Attributes
		{
			get
			{
				return this.method.Attributes;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00038CC1 File Offset: 0x00037CC1
		public override Type DeclaringType
		{
			get
			{
				return this.method.DeclaringType;
			}
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00038CCE File Offset: 0x00037CCE
		public override ParameterInfo[] GetParameters()
		{
			return this.formalParams;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00038CD6 File Offset: 0x00037CD6
		internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
		{
			return this.method;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00038CE0 File Offset: 0x00037CE0
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			int num = this.formalParams.Length;
			int num2 = ((parameters != null) ? parameters.Length : 0);
			if (!this.hasThis && !this.hasVarargs && num == num2)
			{
				if (binder != null)
				{
					return TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, BindingFlags.SuppressChangeType, null, this.ConvertParams(0, parameters, binder, culture), null);
				}
				return TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, options, binder, parameters, culture);
			}
			else
			{
				int num3 = (this.hasThis ? 1 : 0) + (this.hasEngine ? 1 : 0);
				object[] array = new object[num];
				if (this.hasThis)
				{
					array[0] = thisob;
					if (this.hasEngine)
					{
						array[1] = this.engine;
					}
				}
				else if (this.hasEngine)
				{
					array[0] = this.engine;
				}
				if (this.hasVarargs)
				{
					if (num == num3 + 1)
					{
						array[num3] = parameters;
					}
					else
					{
						int num4 = num - 1 - num3;
						if (num2 > num4)
						{
							ArrayObject.Copy(parameters, 0, array, num3, num4);
							int num5 = num2 - num4;
							object[] array2 = new object[num5];
							ArrayObject.Copy(parameters, num4, array2, 0, num5);
							array[num - 1] = array2;
						}
						else
						{
							ArrayObject.Copy(parameters, 0, array, num3, num2);
							for (int i = num2; i < num4; i++)
							{
								array[i + num3] = Missing.Value;
							}
							array[num - 1] = new object[0];
						}
					}
				}
				else
				{
					if (parameters != null)
					{
						if (num - num3 < num2)
						{
							ArrayObject.Copy(parameters, 0, array, num3, num - num3);
						}
						else
						{
							ArrayObject.Copy(parameters, 0, array, num3, num2);
						}
					}
					if (num - num3 > num2)
					{
						for (int j = num2 + num3; j < num; j++)
						{
							if (j == num - 1 && this.formalParams[j].ParameterType.IsArray && CustomAttribute.IsDefined(this.formalParams[j], typeof(ParamArrayAttribute), true))
							{
								array[j] = Array.CreateInstance(this.formalParams[j].ParameterType.GetElementType(), 0);
							}
							else
							{
								array[j] = Missing.Value;
							}
						}
					}
				}
				if (binder != null)
				{
					return TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, BindingFlags.SuppressChangeType, null, this.ConvertParams(num3, array, binder, culture), null);
				}
				return TypeReferences.ToExecutionContext(this.method).Invoke(this.obj, options, binder, array, culture);
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00038F2C File Offset: 0x00037F2C
		private object[] ConvertParams(int offset, object[] parameters, Binder binder, CultureInfo culture)
		{
			int num = this.formalParams.Length;
			if (this.hasVarargs)
			{
				num--;
			}
			for (int i = offset; i < num; i++)
			{
				Type parameterType = this.formalParams[i].ParameterType;
				if (parameterType != Typeob.Object)
				{
					parameters[i] = binder.ChangeType(parameters[i], parameterType, culture);
				}
			}
			return parameters;
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x00038F80 File Offset: 0x00037F80
		public override string Name
		{
			get
			{
				return this.method.Name;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x00038F8D File Offset: 0x00037F8D
		public override Type ReturnType
		{
			get
			{
				return this.method.ReturnType;
			}
		}

		// Token: 0x0400045D RID: 1117
		private MethodInfo method;

		// Token: 0x0400045E RID: 1118
		private ParameterInfo[] formalParams;

		// Token: 0x0400045F RID: 1119
		private bool hasThis;

		// Token: 0x04000460 RID: 1120
		private bool hasVarargs;

		// Token: 0x04000461 RID: 1121
		private bool hasEngine;

		// Token: 0x04000462 RID: 1122
		private VsaEngine engine;
	}
}
