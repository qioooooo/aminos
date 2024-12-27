using System;
using System.Reflection;

namespace System.Configuration
{
	// Token: 0x02000016 RID: 22
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class CallbackValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000100 RID: 256 RVA: 0x0000AA14 File Offset: 0x00009A14
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				if (this._callbackMethod == null)
				{
					if (this._type == null)
					{
						throw new ArgumentNullException("Type");
					}
					if (!string.IsNullOrEmpty(this._callbackMethodName))
					{
						MethodInfo method = this._type.GetMethod(this._callbackMethodName, BindingFlags.Static | BindingFlags.Public);
						if (method != null)
						{
							ParameterInfo[] parameters = method.GetParameters();
							if (parameters.Length == 1 && parameters[0].ParameterType == typeof(object))
							{
								this._callbackMethod = (ValidatorCallback)TypeUtil.CreateDelegateRestricted(this._declaringType, typeof(ValidatorCallback), method);
							}
						}
					}
				}
				if (this._callbackMethod == null)
				{
					throw new ArgumentException(SR.GetString("Validator_method_not_found", new object[] { this._callbackMethodName }));
				}
				return new CallbackValidator(this._callbackMethod);
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000102 RID: 258 RVA: 0x0000AAE9 File Offset: 0x00009AE9
		// (set) Token: 0x06000103 RID: 259 RVA: 0x0000AAF1 File Offset: 0x00009AF1
		public Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
				this._callbackMethod = null;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000AB01 File Offset: 0x00009B01
		// (set) Token: 0x06000105 RID: 261 RVA: 0x0000AB09 File Offset: 0x00009B09
		public string CallbackMethodName
		{
			get
			{
				return this._callbackMethodName;
			}
			set
			{
				this._callbackMethodName = value;
				this._callbackMethod = null;
			}
		}

		// Token: 0x040001D2 RID: 466
		private Type _type;

		// Token: 0x040001D3 RID: 467
		private string _callbackMethodName = string.Empty;

		// Token: 0x040001D4 RID: 468
		private ValidatorCallback _callbackMethod;
	}
}
