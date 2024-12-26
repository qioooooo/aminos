using System;

namespace Microsoft.JScript
{
	// Token: 0x020000B0 RID: 176
	internal sealed class JSKeyword
	{
		// Token: 0x060007FD RID: 2045 RVA: 0x000379D3 File Offset: 0x000369D3
		private JSKeyword(JSToken token, string name)
		{
			this.name = name;
			this.next = null;
			this.token = token;
			this.length = this.name.Length;
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00037A01 File Offset: 0x00036A01
		private JSKeyword(JSToken token, string name, JSKeyword next)
		{
			this.name = name;
			this.next = next;
			this.token = token;
			this.length = this.name.Length;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00037A30 File Offset: 0x00036A30
		internal static string CanBeIdentifier(JSToken keyword)
		{
			switch (keyword)
			{
			case JSToken.Package:
				return "package";
			case JSToken.Internal:
				return "internal";
			case JSToken.Abstract:
				return "abstract";
			case JSToken.Public:
				return "public";
			case JSToken.Static:
				return "static";
			case JSToken.Private:
				return "private";
			case JSToken.Protected:
				return "protected";
			case JSToken.Final:
				return "final";
			case JSToken.Event:
				return "event";
			default:
				if (keyword != JSToken.Void)
				{
					switch (keyword)
					{
					case JSToken.Get:
						return "get";
					case JSToken.Implements:
						return "implements";
					case JSToken.Interface:
						return "interface";
					case JSToken.Set:
						return "set";
					case JSToken.Assert:
						return "assert";
					case JSToken.Boolean:
						return "boolean";
					case JSToken.Byte:
						return "byte";
					case JSToken.Char:
						return "char";
					case JSToken.Decimal:
						return "decimal";
					case JSToken.Double:
						return "double";
					case JSToken.Enum:
						return "enum";
					case JSToken.Ensure:
						return "ensure";
					case JSToken.Float:
						return "float";
					case JSToken.Goto:
						return "goto";
					case JSToken.Int:
						return "int";
					case JSToken.Invariant:
						return "invariant";
					case JSToken.Long:
						return "long";
					case JSToken.Namespace:
						return "namespace";
					case JSToken.Native:
						return "native";
					case JSToken.Require:
						return "require";
					case JSToken.Sbyte:
						return "sbyte";
					case JSToken.Short:
						return "short";
					case JSToken.Synchronized:
						return "synchronized";
					case JSToken.Transient:
						return "transient";
					case JSToken.Throws:
						return "throws";
					case JSToken.Volatile:
						return "volatile";
					case JSToken.Ushort:
						return "ushort";
					case JSToken.Uint:
						return "uint";
					case JSToken.Ulong:
						return "ulong";
					case JSToken.Use:
						return "use";
					}
					return null;
				}
				return "void";
			}
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00037C14 File Offset: 0x00036C14
		internal JSToken GetKeyword(Context token, int length)
		{
			JSKeyword jskeyword = this;
			IL_0071:
			while (jskeyword != null)
			{
				if (length == jskeyword.length)
				{
					int i = 1;
					int num = token.startPos + 1;
					while (i < length)
					{
						char c = jskeyword.name[i];
						char c2 = token.source_string[num];
						if (c != c2)
						{
							if (c2 < c)
							{
								return JSToken.Identifier;
							}
							jskeyword = jskeyword.next;
							goto IL_0071;
						}
						else
						{
							i++;
							num++;
						}
					}
					return jskeyword.token;
				}
				if (length < jskeyword.length)
				{
					return JSToken.Identifier;
				}
				jskeyword = jskeyword.next;
			}
			return JSToken.Identifier;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00037C98 File Offset: 0x00036C98
		internal static JSKeyword[] InitKeywords()
		{
			JSKeyword[] array = new JSKeyword[26];
			JSKeyword jskeyword = new JSKeyword(JSToken.Abstract, "abstract");
			jskeyword = new JSKeyword(JSToken.Assert, "assert", jskeyword);
			array[0] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Boolean, "boolean");
			jskeyword = new JSKeyword(JSToken.Break, "break", jskeyword);
			jskeyword = new JSKeyword(JSToken.Byte, "byte", jskeyword);
			array[1] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Continue, "continue");
			jskeyword = new JSKeyword(JSToken.Const, "const", jskeyword);
			jskeyword = new JSKeyword(JSToken.Class, "class", jskeyword);
			jskeyword = new JSKeyword(JSToken.Catch, "catch", jskeyword);
			jskeyword = new JSKeyword(JSToken.Char, "char", jskeyword);
			jskeyword = new JSKeyword(JSToken.Case, "case", jskeyword);
			array[2] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Debugger, "debugger");
			jskeyword = new JSKeyword(JSToken.Default, "default", jskeyword);
			jskeyword = new JSKeyword(JSToken.Double, "double", jskeyword);
			jskeyword = new JSKeyword(JSToken.Delete, "delete", jskeyword);
			jskeyword = new JSKeyword(JSToken.Do, "do", jskeyword);
			array[3] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Extends, "extends");
			jskeyword = new JSKeyword(JSToken.Export, "export", jskeyword);
			jskeyword = new JSKeyword(JSToken.Ensure, "ensure", jskeyword);
			jskeyword = new JSKeyword(JSToken.Event, "event", jskeyword);
			jskeyword = new JSKeyword(JSToken.Enum, "enum", jskeyword);
			jskeyword = new JSKeyword(JSToken.Else, "else", jskeyword);
			array[4] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Function, "function");
			jskeyword = new JSKeyword(JSToken.Finally, "finally", jskeyword);
			jskeyword = new JSKeyword(JSToken.Float, "float", jskeyword);
			jskeyword = new JSKeyword(JSToken.Final, "final", jskeyword);
			jskeyword = new JSKeyword(JSToken.False, "false", jskeyword);
			jskeyword = new JSKeyword(JSToken.For, "for", jskeyword);
			array[5] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Goto, "goto");
			jskeyword = new JSKeyword(JSToken.Get, "get", jskeyword);
			array[6] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Instanceof, "instanceof");
			jskeyword = new JSKeyword(JSToken.Implements, "implements", jskeyword);
			jskeyword = new JSKeyword(JSToken.Invariant, "invariant", jskeyword);
			jskeyword = new JSKeyword(JSToken.Interface, "interface", jskeyword);
			jskeyword = new JSKeyword(JSToken.Internal, "internal", jskeyword);
			jskeyword = new JSKeyword(JSToken.Import, "import", jskeyword);
			jskeyword = new JSKeyword(JSToken.Int, "int", jskeyword);
			jskeyword = new JSKeyword(JSToken.In, "in", jskeyword);
			jskeyword = new JSKeyword(JSToken.If, "if", jskeyword);
			array[8] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Long, "long");
			array[11] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Namespace, "namespace");
			jskeyword = new JSKeyword(JSToken.Native, "native", jskeyword);
			jskeyword = new JSKeyword(JSToken.Null, "null", jskeyword);
			jskeyword = new JSKeyword(JSToken.New, "new", jskeyword);
			array[13] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Protected, "protected");
			jskeyword = new JSKeyword(JSToken.Private, "private", jskeyword);
			jskeyword = new JSKeyword(JSToken.Package, "package", jskeyword);
			jskeyword = new JSKeyword(JSToken.Public, "public", jskeyword);
			array[15] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Require, "require");
			jskeyword = new JSKeyword(JSToken.Return, "return", jskeyword);
			array[17] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Synchronized, "synchronized");
			jskeyword = new JSKeyword(JSToken.Switch, "switch", jskeyword);
			jskeyword = new JSKeyword(JSToken.Static, "static", jskeyword);
			jskeyword = new JSKeyword(JSToken.Super, "super", jskeyword);
			jskeyword = new JSKeyword(JSToken.Short, "short", jskeyword);
			jskeyword = new JSKeyword(JSToken.Set, "set", jskeyword);
			array[18] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Transient, "transient");
			jskeyword = new JSKeyword(JSToken.Typeof, "typeof", jskeyword);
			jskeyword = new JSKeyword(JSToken.Throws, "throws", jskeyword);
			jskeyword = new JSKeyword(JSToken.Throw, "throw", jskeyword);
			jskeyword = new JSKeyword(JSToken.True, "true", jskeyword);
			jskeyword = new JSKeyword(JSToken.This, "this", jskeyword);
			jskeyword = new JSKeyword(JSToken.Try, "try", jskeyword);
			array[19] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Volatile, "volatile");
			jskeyword = new JSKeyword(JSToken.Void, "void", jskeyword);
			jskeyword = new JSKeyword(JSToken.Var, "var", jskeyword);
			array[21] = jskeyword;
			jskeyword = new JSKeyword(JSToken.Use, "use");
			array[20] = jskeyword;
			jskeyword = new JSKeyword(JSToken.While, "while");
			jskeyword = new JSKeyword(JSToken.With, "with", jskeyword);
			array[22] = jskeyword;
			return array;
		}

		// Token: 0x04000449 RID: 1097
		private JSKeyword next;

		// Token: 0x0400044A RID: 1098
		private JSToken token;

		// Token: 0x0400044B RID: 1099
		private string name;

		// Token: 0x0400044C RID: 1100
		private int length;
	}
}
