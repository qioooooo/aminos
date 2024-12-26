using System;

namespace Microsoft.JScript
{
	// Token: 0x02000079 RID: 121
	public class ErrorPrototype : JSObject
	{
		// Token: 0x06000599 RID: 1433 RVA: 0x00027275 File Offset: 0x00026275
		internal ErrorPrototype(ScriptObject parent, string name)
			: base(parent)
		{
			this.name = name;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00027285 File Offset: 0x00026285
		public ErrorConstructor constructor
		{
			get
			{
				return this._constructor;
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00027290 File Offset: 0x00026290
		[JSFunction(JSFunctionAttributeEnum.HasThisObject, JSBuiltin.Error_toString)]
		public static string toString(object thisob)
		{
			if (!(thisob is ErrorObject))
			{
				return thisob.ToString();
			}
			string message = ((ErrorObject)thisob).Message;
			if (message.Length == 0)
			{
				return LateBinding.GetMemberValue(thisob, "name").ToString();
			}
			return LateBinding.GetMemberValue(thisob, "name").ToString() + ": " + message;
		}

		// Token: 0x0400026B RID: 619
		public readonly string name;

		// Token: 0x0400026C RID: 620
		internal static readonly ErrorPrototype ob = new ErrorPrototype(ObjectPrototype.ob, "Error");

		// Token: 0x0400026D RID: 621
		internal ErrorConstructor _constructor;
	}
}
