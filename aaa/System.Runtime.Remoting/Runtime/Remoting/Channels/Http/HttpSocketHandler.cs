using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x0200002F RID: 47
	internal abstract class HttpSocketHandler : SocketHandler
	{
		// Token: 0x0600018C RID: 396 RVA: 0x00008308 File Offset: 0x00007308
		public HttpSocketHandler(Socket socket, RequestQueue requestQueue, Stream stream)
			: base(socket, requestQueue, stream)
		{
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008314 File Offset: 0x00007314
		protected void ReadToEndOfHeaders(BaseTransportHeaders headers, out bool bChunked, out int contentLength, ref bool bKeepAlive, ref bool bSendContinue)
		{
			bChunked = false;
			contentLength = 0;
			for (;;)
			{
				string text = base.ReadToEndOfLine();
				if (text.Length == 0)
				{
					break;
				}
				int num = text.IndexOf(":");
				string text2 = text.Substring(0, num);
				string text3 = text.Substring(num + 1 + 1);
				if (string.Compare(text2, "Transfer-Encoding", StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (string.Compare(text3, "chunked", StringComparison.OrdinalIgnoreCase) == 0)
					{
						bChunked = true;
					}
				}
				else if (string.Compare(text2, "Connection", StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (string.Compare(text3, "Keep-Alive", StringComparison.OrdinalIgnoreCase) == 0)
					{
						bKeepAlive = true;
					}
					else if (string.Compare(text3, "Close", StringComparison.OrdinalIgnoreCase) == 0)
					{
						bKeepAlive = false;
					}
				}
				else if (string.Compare(text2, "Expect", StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (string.Compare(text3, "100-continue", StringComparison.OrdinalIgnoreCase) == 0)
					{
						bSendContinue = true;
					}
				}
				else if (string.Compare(text2, "Content-Length", StringComparison.OrdinalIgnoreCase) == 0)
				{
					contentLength = int.Parse(text3, CultureInfo.InvariantCulture);
				}
				else
				{
					headers[text2] = text3;
				}
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000840C File Offset: 0x0000740C
		protected void WriteHeaders(ITransportHeaders headers, Stream outputStream)
		{
			/*
An exception occurred when decompiling this method (0600018E)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Runtime.Remoting.Channels.Http.HttpSocketHandler::WriteHeaders(System.Runtime.Remoting.Channels.ITransportHeaders,System.IO.Stream)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.CreateLoopLocalCore(ILWhileLoop block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 753
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.CreateLoopLocal(ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 677
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 373
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000084A0 File Offset: 0x000074A0
		private void WriteHeader(string name, string value, Stream outputStream)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(name);
			byte[] bytes2 = Encoding.ASCII.GetBytes(value);
			outputStream.Write(bytes, 0, bytes.Length);
			outputStream.Write(HttpSocketHandler.s_headerSeparator, 0, HttpSocketHandler.s_headerSeparator.Length);
			outputStream.Write(bytes2, 0, bytes2.Length);
			outputStream.Write(HttpSocketHandler.s_endOfLine, 0, HttpSocketHandler.s_endOfLine.Length);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008504 File Offset: 0x00007504
		protected void WriteResponseFirstLine(string statusCode, string reasonPhrase, Stream outputStream)
		{
			/*
An exception occurred when decompiling this method (06000190)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Runtime.Remoting.Channels.Http.HttpSocketHandler::WriteResponseFirstLine(System.String,System.String,System.IO.Stream)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.VariableSlot.CloneVariableState(VariableSlot[] state) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 78
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 407
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008570 File Offset: 0x00007570
		// Note: this type is marked as 'beforefieldinit'.
		static HttpSocketHandler()
		{
			/*
An exception occurred when decompiling this method (06000191)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Runtime.Remoting.Channels.Http.HttpSocketHandler::.cctor()

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.NRefactory.CSharp.AstType.MakeArrayType(Int32 rank) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\NRefactory\ICSharpCode.NRefactory.CSharp\Ast\AstType.cs:line 200
   at ICSharpCode.Decompiler.Ast.AstBuilder.ConvertType(TypeSig type, IHasCustomAttribute typeAttributes, Int32& typeIndex, ConvertTypeOptions options, Int32 depth, StringBuilder sb) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 699
   at ICSharpCode.Decompiler.Ast.AstBuilder.ConvertType(ITypeDefOrRef type, IHasCustomAttribute typeAttributes, Int32& typeIndex, ConvertTypeOptions options, Int32 depth, StringBuilder sb) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 817
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 484
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformByteCode(ILExpression byteCode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 488
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformExpression(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 407
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformNode(ILNode node) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 268
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.TransformBlock(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 252
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 150
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x04000134 RID: 308
		private static byte[] s_httpVersion;

		// Token: 0x04000135 RID: 309
		private static byte[] s_httpVersionAndSpace;

		// Token: 0x04000136 RID: 310
		private static byte[] s_headerSeparator;

		// Token: 0x04000137 RID: 311
		private static byte[] s_endOfLine;
	}
}
