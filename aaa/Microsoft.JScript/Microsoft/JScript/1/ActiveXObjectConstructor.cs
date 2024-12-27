using System;

namespace Microsoft.JScript
{
	// Token: 0x0200000A RID: 10
	public sealed class ActiveXObjectConstructor : ScriptFunction
	{
		// Token: 0x06000071 RID: 113 RVA: 0x00004451 File Offset: 0x00003451
		internal ActiveXObjectConstructor()
			: base(FunctionPrototype.ob, "ActiveXObject", 1)
		{
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004464 File Offset: 0x00003464
		internal ActiveXObjectConstructor(LenientFunctionPrototype parent)
			: base(parent, "ActiveXObject", 1)
		{
			this.noExpando = false;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000447A File Offset: 0x0000347A
		internal override object Call(object[] args, object thisob)
		{
			return null;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000447D File Offset: 0x0000347D
		internal override object Construct(object[] args)
		{
			return this.CreateInstance(args);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004488 File Offset: 0x00003488
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		public new object CreateInstance(params object[] args)
		{
			if (args.Length == 0 || args[0].GetType() != typeof(string))
			{
				throw new JScriptException(JSError.TypeMismatch);
			}
			string text = args[0].ToString();
			string text2 = null;
			if (args.Length == 2)
			{
				if (args[1].GetType() != typeof(string))
				{
					throw new JScriptException(JSError.TypeMismatch);
				}
				text2 = args[1].ToString();
			}
			object obj;
			try
			{
				Type type;
				if (text2 == null)
				{
					type = Type.GetTypeFromProgID(text);
				}
				else
				{
					type = Type.GetTypeFromProgID(text, text2);
				}
				if (!type.IsPublic && type.Assembly == typeof(ActiveXObjectConstructor).Assembly)
				{
					throw new JScriptException(JSError.CantCreateObject);
				}
				obj = Activator.CreateInstance(type);
			}
			catch
			{
				throw new JScriptException(JSError.CantCreateObject);
			}
			return obj;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004554 File Offset: 0x00003554
		public object Invoke()
		{
			return null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004557 File Offset: 0x00003557
		internal override bool HasInstance(object ob)
		{
			return !(ob is JSObject);
		}

		// Token: 0x04000021 RID: 33
		internal static readonly ActiveXObjectConstructor ob = new ActiveXObjectConstructor();
	}
}
