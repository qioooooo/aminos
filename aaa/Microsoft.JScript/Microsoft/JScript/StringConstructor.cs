using System;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x020000DB RID: 219
	public class StringConstructor : ScriptFunction
	{
		// Token: 0x060009C5 RID: 2501 RVA: 0x0004AAD6 File Offset: 0x00049AD6
		internal StringConstructor()
			: base(FunctionPrototype.ob, "String", 1)
		{
			this.originalPrototype = StringPrototype.ob;
			StringPrototype._constructor = this;
			this.proto = StringPrototype.ob;
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0004AB05 File Offset: 0x00049B05
		internal StringConstructor(FunctionPrototype parent, LenientStringPrototype prototypeProp)
			: base(parent, "String", 1)
		{
			this.originalPrototype = prototypeProp;
			prototypeProp.constructor = this;
			this.proto = prototypeProp;
			this.noExpando = false;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0004AB30 File Offset: 0x00049B30
		internal override object Call(object[] args, object thisob)
		{
			if (args.Length == 0)
			{
				return "";
			}
			return Convert.ToString(args[0]);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0004AB45 File Offset: 0x00049B45
		internal StringObject Construct()
		{
			return new StringObject(this.originalPrototype, "", false);
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0004AB58 File Offset: 0x00049B58
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0004AB61 File Offset: 0x00049B61
		internal StringObject ConstructImplicitWrapper(string arg)
		{
			return new StringObject(this.originalPrototype, arg, true);
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0004AB70 File Offset: 0x00049B70
		internal StringObject ConstructWrapper(string arg)
		{
			return new StringObject(this.originalPrototype, arg, false);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0004AB7F File Offset: 0x00049B7F
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new StringObject CreateInstance(params object[] args)
		{
			return new StringObject(this.originalPrototype, (args.Length == 0) ? "" : Convert.ToString(args[0]), false);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0004ABA1 File Offset: 0x00049BA1
		public string Invoke(object arg)
		{
			return Convert.ToString(arg);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0004ABAC File Offset: 0x00049BAC
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs, JSBuiltin.String_fromCharCode)]
		public static string fromCharCode(params object[] args)
		{
			StringBuilder stringBuilder = new StringBuilder(args.Length);
			for (int i = 0; i < args.Length; i++)
			{
				stringBuilder.Append(Convert.ToChar(args[i]));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000629 RID: 1577
		internal static readonly StringConstructor ob = new StringConstructor();

		// Token: 0x0400062A RID: 1578
		private StringPrototype originalPrototype;
	}
}
