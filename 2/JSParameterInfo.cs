using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000B5 RID: 181
	internal sealed class JSParameterInfo : ParameterInfo
	{
		// Token: 0x06000830 RID: 2096 RVA: 0x00038F9A File Offset: 0x00037F9A
		internal JSParameterInfo(ParameterInfo parameter)
		{
			this.parameter = parameter;
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00038FA9 File Offset: 0x00037FA9
		public override object DefaultValue
		{
			get
			{
				return TypeReferences.GetDefaultParameterValue(this.parameter);
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00038FB8 File Offset: 0x00037FB8
		public sealed override object[] GetCustomAttributes(bool inherit)
		{
			object[] array = this.attributes;
			if (array != null)
			{
				return array;
			}
			return this.attributes = this.parameter.GetCustomAttributes(true);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00038FE8 File Offset: 0x00037FE8
		public sealed override object[] GetCustomAttributes(Type type, bool inherit)
		{
			object[] array = this.attributes;
			if (array != null)
			{
				return array;
			}
			return this.attributes = CustomAttribute.GetCustomAttributes(this.parameter, type, true);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00039018 File Offset: 0x00038018
		public sealed override bool IsDefined(Type type, bool inherit)
		{
			object[] array = this.attributes;
			if (array == null)
			{
				array = (this.attributes = CustomAttribute.GetCustomAttributes(this.parameter, type, true));
			}
			return array.Length > 0;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0003904A File Offset: 0x0003804A
		public override string Name
		{
			get
			{
				return this.parameter.Name;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00039058 File Offset: 0x00038058
		public override Type ParameterType
		{
			get
			{
				Type type = this.parameterType;
				if (type != null)
				{
					return type;
				}
				return this.parameterType = this.parameter.ParameterType;
			}
		}

		// Token: 0x04000463 RID: 1123
		private ParameterInfo parameter;

		// Token: 0x04000464 RID: 1124
		private Type parameterType;

		// Token: 0x04000465 RID: 1125
		private object[] attributes;
	}
}
