using System;
using System.Collections;

namespace Microsoft.JScript
{
	// Token: 0x0200006F RID: 111
	public sealed class EnumeratorConstructor : ScriptFunction
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x00025C2F File Offset: 0x00024C2F
		internal EnumeratorConstructor()
			: base(FunctionPrototype.ob, "Enumerator", 1)
		{
			this.originalPrototype = EnumeratorPrototype.ob;
			EnumeratorPrototype._constructor = this;
			this.proto = EnumeratorPrototype.ob;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x00025C5E File Offset: 0x00024C5E
		internal EnumeratorConstructor(LenientFunctionPrototype parent, LenientEnumeratorPrototype prototypeProp)
			: base(parent, "Enumerator", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x00025C89 File Offset: 0x00024C89
		internal override object Call(object[] args, object thisob)
		{
			return null;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00025C8C File Offset: 0x00024C8C
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00025C98 File Offset: 0x00024C98
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new EnumeratorObject CreateInstance(params object[] args)
		{
			if (args.Length == 0)
			{
				return new EnumeratorObject(this.originalPrototype, null);
			}
			object obj = args[0];
			if (obj is IEnumerable)
			{
				return new EnumeratorObject(this.originalPrototype, (IEnumerable)obj);
			}
			throw new JScriptException(JSError.NotCollection);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00025CDF File Offset: 0x00024CDF
		public object Invoke()
		{
			return null;
		}

		// Token: 0x04000248 RID: 584
		internal static readonly EnumeratorConstructor ob = new EnumeratorConstructor();

		// Token: 0x04000249 RID: 585
		private EnumeratorPrototype originalPrototype;
	}
}
