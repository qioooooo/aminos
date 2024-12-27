using System;

namespace System.Windows.Forms
{
	// Token: 0x020004C4 RID: 1220
	internal static class MessageDecoder
	{
		// Token: 0x060048BA RID: 18618 RVA: 0x001081EC File Offset: 0x001071EC
		private static string MsgToString(int msg)
		{
			/*
An exception occurred when decompiling this method (060048BA)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String System.Windows.Forms.MessageDecoder::MsgToString(System.Int32)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Union(ByteCode[] a, ByteCode[] b) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 658
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 492
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x00109554 File Offset: 0x00108554
		private static string Parenthesize(string input)
		{
			if (input == null)
			{
				return "";
			}
			return " (" + input + ")";
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0010956F File Offset: 0x0010856F
		public static string ToString(Message message)
		{
			return MessageDecoder.ToString(message.HWnd, message.Msg, message.WParam, message.LParam, message.Result);
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x0010959C File Offset: 0x0010859C
		public static string ToString(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam, IntPtr result)
		{
			string text = MessageDecoder.Parenthesize(MessageDecoder.MsgToString(msg));
			string text2 = "";
			if (msg == 528)
			{
				text2 = MessageDecoder.Parenthesize(MessageDecoder.MsgToString(NativeMethods.Util.LOWORD(wparam)));
			}
			return string.Concat(new string[]
			{
				"msg=0x",
				Convert.ToString(msg, 16),
				text,
				" hwnd=0x",
				Convert.ToString((long)hWnd, 16),
				" wparam=0x",
				Convert.ToString((long)wparam, 16),
				" lparam=0x",
				Convert.ToString((long)lparam, 16),
				text2,
				" result=0x",
				Convert.ToString((long)result, 16)
			});
		}
	}
}
