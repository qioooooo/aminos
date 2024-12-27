using System;

namespace Microsoft.JScript
{
	// Token: 0x02000071 RID: 113
	public class EnumeratorPrototype : JSObject
	{
		// Token: 0x06000554 RID: 1364 RVA: 0x00025DC0 File Offset: 0x00024DC0
		internal EnumeratorPrototype(ObjectPrototype parent)
			: base(parent)
		{
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00025DC9 File Offset: 0x00024DC9
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_atEnd)]
		public static bool atEnd(object thisob)
		{
			if (thisob is EnumeratorObject)
			{
				return ((EnumeratorObject)thisob).atEnd();
			}
			throw new JScriptException(JSError.EnumeratorExpected);
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00025DE9 File Offset: 0x00024DE9
		public static EnumeratorConstructor constructor
		{
			get
			{
				return EnumeratorPrototype._constructor;
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00025DF0 File Offset: 0x00024DF0
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_item)]
		public static object item(object thisob)
		{
			if (thisob is EnumeratorObject)
			{
				return ((EnumeratorObject)thisob).item();
			}
			throw new JScriptException(JSError.EnumeratorExpected);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x00025E10 File Offset: 0x00024E10
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_moveFirst)]
		public static void moveFirst(object thisob)
		{
			if (thisob is EnumeratorObject)
			{
				((EnumeratorObject)thisob).moveFirst();
				return;
			}
			throw new JScriptException(JSError.EnumeratorExpected);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x00025E30 File Offset: 0x00024E30
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Enumerator_moveNext)]
		public static void moveNext(object thisob)
		{
			if (thisob is EnumeratorObject)
			{
				((EnumeratorObject)thisob).moveNext();
				return;
			}
			throw new JScriptException(JSError.EnumeratorExpected);
		}

		// Token: 0x0400024D RID: 589
		internal static readonly EnumeratorPrototype ob = new EnumeratorPrototype(ObjectPrototype.ob);

		// Token: 0x0400024E RID: 590
		internal static EnumeratorConstructor _constructor;
	}
}
