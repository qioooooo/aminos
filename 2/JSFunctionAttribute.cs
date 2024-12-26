using System;

namespace Microsoft.JScript
{
	// Token: 0x020000AB RID: 171
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
	public class JSFunctionAttribute : Attribute
	{
		// Token: 0x060007E5 RID: 2021 RVA: 0x00035E1A File Offset: 0x00034E1A
		public JSFunctionAttribute(JSFunctionAttributeEnum value)
		{
			this.attributeValue = value;
			this.builtinFunction = JSBuiltin.None;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00035E30 File Offset: 0x00034E30
		public JSFunctionAttribute(JSFunctionAttributeEnum value, JSBuiltin builtinFunction)
		{
			this.attributeValue = value;
			this.builtinFunction = builtinFunction;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00035E46 File Offset: 0x00034E46
		public JSFunctionAttributeEnum GetAttributeValue()
		{
			return this.attributeValue;
		}

		// Token: 0x04000434 RID: 1076
		internal JSFunctionAttributeEnum attributeValue;

		// Token: 0x04000435 RID: 1077
		internal JSBuiltin builtinFunction;
	}
}
