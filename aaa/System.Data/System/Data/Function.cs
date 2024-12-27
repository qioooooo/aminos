using System;

namespace System.Data
{
	// Token: 0x020001B4 RID: 436
	internal sealed class Function
	{
		// Token: 0x06001919 RID: 6425 RVA: 0x0023D8EC File Offset: 0x0023CCEC
		internal Function()
		{
			Type[] array = new Type[3];
			this.parameters = array;
			base..ctor();
			this.name = null;
			this.id = FunctionId.none;
			this.result = null;
			this.IsValidateArguments = false;
			this.argumentCount = 0;
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0023D930 File Offset: 0x0023CD30
		internal Function(string name, FunctionId id, Type result, bool IsValidateArguments, bool IsVariantArgumentList, int argumentCount, Type a1, Type a2, Type a3)
		{
			Type[] array = new Type[3];
			this.parameters = array;
			base..ctor();
			this.name = name;
			this.id = id;
			this.result = result;
			this.IsValidateArguments = IsValidateArguments;
			this.IsVariantArgumentList = IsVariantArgumentList;
			this.argumentCount = argumentCount;
			if (a1 != null)
			{
				this.parameters[0] = a1;
			}
			if (a2 != null)
			{
				this.parameters[1] = a2;
			}
			if (a3 != null)
			{
				this.parameters[2] = a3;
			}
		}

		// Token: 0x04000DD0 RID: 3536
		internal readonly string name;

		// Token: 0x04000DD1 RID: 3537
		internal readonly FunctionId id;

		// Token: 0x04000DD2 RID: 3538
		internal readonly Type result;

		// Token: 0x04000DD3 RID: 3539
		internal readonly bool IsValidateArguments;

		// Token: 0x04000DD4 RID: 3540
		internal readonly bool IsVariantArgumentList;

		// Token: 0x04000DD5 RID: 3541
		internal readonly int argumentCount;

		// Token: 0x04000DD6 RID: 3542
		internal readonly Type[] parameters;

		// Token: 0x04000DD7 RID: 3543
		internal static string[] FunctionName = new string[]
		{
			"Unknown", "Ascii", "Char", "CharIndex", "Difference", "Len", "Lower", "LTrim", "Patindex", "Replicate",
			"Reverse", "Right", "RTrim", "Soundex", "Space", "Str", "Stuff", "Substring", "Upper", "IsNull",
			"Iif", "Convert", "cInt", "cBool", "cDate", "cDbl", "cStr", "Abs", "Acos", "In",
			"Trim", "Sum", "Avg", "Min", "Max", "Count", "StDev", "Var", "DateTimeOffset"
		};
	}
}
