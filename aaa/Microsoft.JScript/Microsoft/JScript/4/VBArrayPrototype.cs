using System;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000E0 RID: 224
	public class VBArrayPrototype : JSObject
	{
		// Token: 0x06000A03 RID: 2563 RVA: 0x0004BD57 File Offset: 0x0004AD57
		internal VBArrayPrototype(FunctionPrototype funcprot, ObjectPrototype parent)
			: base(parent)
		{
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0004BD60 File Offset: 0x0004AD60
		public static VBArrayConstructor constructor
		{
			get
			{
				return VBArrayPrototype._constructor;
			}
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0004BD67 File Offset: 0x0004AD67
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.VBArray_dimensions)]
		public static int dimensions(object thisob)
		{
			if (thisob is VBArrayObject)
			{
				return ((VBArrayObject)thisob).dimensions();
			}
			throw new JScriptException(JSError.VBArrayExpected);
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0004BD87 File Offset: 0x0004AD87
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.VBArray_getItem)]
		public static object getItem(object thisob, params object[] args)
		{
			if (thisob is VBArrayObject)
			{
				return ((VBArrayObject)thisob).getItem(args);
			}
			throw new JScriptException(JSError.VBArrayExpected);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0004BDA8 File Offset: 0x0004ADA8
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.VBArray_lbound)]
		public static int lbound(object thisob, object dimension)
		{
			if (thisob is VBArrayObject)
			{
				return ((VBArrayObject)thisob).lbound(dimension);
			}
			throw new JScriptException(JSError.VBArrayExpected);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0004BDC9 File Offset: 0x0004ADC9
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasEngine, JSBuiltin.VBArray_toArray)]
		public static ArrayObject toArray(object thisob, VsaEngine engine)
		{
			if (thisob is VBArrayObject)
			{
				return ((VBArrayObject)thisob).toArray(engine);
			}
			throw new JScriptException(JSError.VBArrayExpected);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0004BDEA File Offset: 0x0004ADEA
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.VBArray_ubound)]
		public static int ubound(object thisob, object dimension)
		{
			if (thisob is VBArrayObject)
			{
				return ((VBArrayObject)thisob).ubound(dimension);
			}
			throw new JScriptException(JSError.VBArrayExpected);
		}

		// Token: 0x04000651 RID: 1617
		internal static readonly VBArrayPrototype ob = new VBArrayPrototype(FunctionPrototype.ob, ObjectPrototype.ob);

		// Token: 0x04000652 RID: 1618
		internal static VBArrayConstructor _constructor;
	}
}
