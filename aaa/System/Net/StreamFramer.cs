using System;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200054C RID: 1356
	internal class StreamFramer
	{
		// Token: 0x06002921 RID: 10529 RVA: 0x000ABC80 File Offset: 0x000AAC80
		public StreamFramer(Stream Transport)
		{
			if (Transport == null || Transport == Stream.Null)
			{
				throw new ArgumentNullException("Transport");
			}
			this.m_Transport = Transport;
			if (this.m_Transport.GetType() == typeof(NetworkStream))
			{
				this.m_NetworkStream = Transport as NetworkStream;
			}
			this.m_ReadHeaderBuffer = new byte[this.m_CurReadHeader.Size];
			this.m_WriteHeaderBuffer = new byte[this.m_WriteHeader.Size];
			this.m_ReadFrameCallback = new AsyncCallback(this.ReadFrameCallback);
			this.m_BeginWriteCallback = new AsyncCallback(this.BeginWriteCallback);
		}

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x000ABD47 File Offset: 0x000AAD47
		public FrameHeader ReadHeader
		{
			get
			{
				return this.m_CurReadHeader;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002923 RID: 10531 RVA: 0x000ABD4F File Offset: 0x000AAD4F
		public FrameHeader WriteHeader
		{
			get
			{
				return this.m_WriteHeader;
			}
		}

		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002924 RID: 10532 RVA: 0x000ABD57 File Offset: 0x000AAD57
		public Stream Transport
		{
			get
			{
				return this.m_Transport;
			}
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x000ABD60 File Offset: 0x000AAD60
		public byte[] ReadMessage()
		{
			if (this.m_Eof)
			{
				return null;
			}
			int i = 0;
			byte[] array = this.m_ReadHeaderBuffer;
			int num;
			while (i < array.Length)
			{
				num = this.Transport.Read(array, i, array.Length - i);
				if (num == 0)
				{
					if (i == 0)
					{
						this.m_Eof = true;
						return null;
					}
					throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
				}
				else
				{
					i += num;
				}
			}
			this.m_CurReadHeader.CopyFrom(array, 0, this.m_ReadVerifier);
			if (this.m_CurReadHeader.PayloadSize > this.m_CurReadHeader.MaxMessageSize)
			{
				throw new InvalidOperationException(SR.GetString("net_frame_size", new object[]
				{
					this.m_CurReadHeader.MaxMessageSize.ToString(NumberFormatInfo.InvariantInfo),
					this.m_CurReadHeader.PayloadSize.ToString(NumberFormatInfo.InvariantInfo)
				}));
			}
			array = new byte[this.m_CurReadHeader.PayloadSize];
			for (i = 0; i < array.Length; i += num)
			{
				num = this.Transport.Read(array, i, array.Length - i);
				if (num == 0)
				{
					throw new IOException(SR.GetString("net_io_readfailure", new object[] { SR.GetString("net_io_connectionclosed") }));
				}
			}
			return array;
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x000ABEB0 File Offset: 0x000AAEB0
		public IAsyncResult BeginReadMessage(AsyncCallback asyncCallback, object stateObject)
		{
			WorkerAsyncResult workerAsyncResult;
			if (this.m_Eof)
			{
				workerAsyncResult = new WorkerAsyncResult(this, stateObject, asyncCallback, null, 0, 0);
				workerAsyncResult.InvokeCallback(-1);
				return workerAsyncResult;
			}
			workerAsyncResult = new WorkerAsyncResult(this, stateObject, asyncCallback, this.m_ReadHeaderBuffer, 0, this.m_ReadHeaderBuffer.Length);
			IAsyncResult asyncResult = this.Transport.BeginRead(this.m_ReadHeaderBuffer, 0, this.m_ReadHeaderBuffer.Length, this.m_ReadFrameCallback, workerAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				this.ReadFrameComplete(asyncResult);
			}
			return workerAsyncResult;
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000ABF2C File Offset: 0x000AAF2C
		private void ReadFrameCallback(IAsyncResult transportResult)
		{
			/*
An exception occurred when decompiling this method (06002927)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Net.StreamFramer::ReadFrameCallback(System.IAsyncResult)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.set_Capacity(Int32 value)
   at System.Collections.Generic.List`1.AddWithResize(T item)
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(HashSet`1 scope, ControlFlowNode entryNode) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 245
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindConditions(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 69
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 351
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x000AC000 File Offset: 0x000AB000
		private void ReadFrameComplete(IAsyncResult transportResult)
		{
			WorkerAsyncResult workerAsyncResult;
			int payloadSize;
			for (;;)
			{
				workerAsyncResult = (WorkerAsyncResult)transportResult.AsyncState;
				int num = this.Transport.EndRead(transportResult);
				workerAsyncResult.Offset += num;
				if (num <= 0)
				{
					break;
				}
				if (workerAsyncResult.Offset >= workerAsyncResult.End)
				{
					if (workerAsyncResult.HeaderDone)
					{
						goto IL_0146;
					}
					workerAsyncResult.HeaderDone = true;
					this.m_CurReadHeader.CopyFrom(workerAsyncResult.Buffer, 0, this.m_ReadVerifier);
					payloadSize = this.m_CurReadHeader.PayloadSize;
					if (payloadSize < 0)
					{
						workerAsyncResult.InvokeCallback(new IOException(SR.GetString("net_frame_read_size")));
					}
					if (payloadSize == 0)
					{
						goto Block_6;
					}
					if (payloadSize > this.m_CurReadHeader.MaxMessageSize)
					{
						goto Block_7;
					}
					byte[] array = new byte[payloadSize];
					workerAsyncResult.Buffer = array;
					workerAsyncResult.End = array.Length;
					workerAsyncResult.Offset = 0;
				}
				transportResult = this.Transport.BeginRead(workerAsyncResult.Buffer, workerAsyncResult.Offset, workerAsyncResult.End - workerAsyncResult.Offset, this.m_ReadFrameCallback, workerAsyncResult);
				if (!transportResult.CompletedSynchronously)
				{
					return;
				}
			}
			object obj;
			if (!workerAsyncResult.HeaderDone && workerAsyncResult.Offset == 0)
			{
				obj = -1;
			}
			else
			{
				obj = new IOException(SR.GetString("net_frame_read_io"));
			}
			workerAsyncResult.InvokeCallback(obj);
			return;
			Block_6:
			workerAsyncResult.InvokeCallback(0);
			return;
			Block_7:
			throw new InvalidOperationException(SR.GetString("net_frame_size", new object[]
			{
				this.m_CurReadHeader.MaxMessageSize.ToString(NumberFormatInfo.InvariantInfo),
				payloadSize.ToString(NumberFormatInfo.InvariantInfo)
			}));
			IL_0146:
			workerAsyncResult.HeaderDone = false;
			workerAsyncResult.InvokeCallback(workerAsyncResult.End);
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000AC1A4 File Offset: 0x000AB1A4
		public byte[] EndReadMessage(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WorkerAsyncResult workerAsyncResult = asyncResult as WorkerAsyncResult;
			if (workerAsyncResult == null)
			{
				throw new ArgumentException(SR.GetString("net_io_async_result", new object[] { typeof(WorkerAsyncResult).FullName }), "asyncResult");
			}
			if (!workerAsyncResult.InternalPeekCompleted)
			{
				workerAsyncResult.InternalWaitForCompletion();
			}
			if (workerAsyncResult.Result is Exception)
			{
				throw (Exception)workerAsyncResult.Result;
			}
			int num = (int)workerAsyncResult.Result;
			if (num == -1)
			{
				this.m_Eof = true;
				return null;
			}
			if (num == 0)
			{
				return new byte[0];
			}
			return workerAsyncResult.Buffer;
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000AC24C File Offset: 0x000AB24C
		public void WriteMessage(byte[] message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			this.m_WriteHeader.PayloadSize = message.Length;
			this.m_WriteHeader.CopyTo(this.m_WriteHeaderBuffer, 0);
			if (this.m_NetworkStream != null && message.Length != 0)
			{
				BufferOffsetSize[] array = new BufferOffsetSize[]
				{
					new BufferOffsetSize(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, false),
					new BufferOffsetSize(message, 0, message.Length, false)
				};
				this.m_NetworkStream.MultipleWrite(array);
				return;
			}
			this.Transport.Write(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length);
			if (message.Length == 0)
			{
				return;
			}
			this.Transport.Write(message, 0, message.Length);
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000AC300 File Offset: 0x000AB300
		public IAsyncResult BeginWriteMessage(byte[] message, AsyncCallback asyncCallback, object stateObject)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}
			this.m_WriteHeader.PayloadSize = message.Length;
			this.m_WriteHeader.CopyTo(this.m_WriteHeaderBuffer, 0);
			if (this.m_NetworkStream != null && message.Length != 0)
			{
				BufferOffsetSize[] array = new BufferOffsetSize[]
				{
					new BufferOffsetSize(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, false),
					new BufferOffsetSize(message, 0, message.Length, false)
				};
				return this.m_NetworkStream.BeginMultipleWrite(array, asyncCallback, stateObject);
			}
			if (message.Length == 0)
			{
				return this.Transport.BeginWrite(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, asyncCallback, stateObject);
			}
			WorkerAsyncResult workerAsyncResult = new WorkerAsyncResult(this, stateObject, asyncCallback, message, 0, message.Length);
			IAsyncResult asyncResult = this.Transport.BeginWrite(this.m_WriteHeaderBuffer, 0, this.m_WriteHeaderBuffer.Length, this.m_BeginWriteCallback, workerAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				this.BeginWriteComplete(asyncResult);
			}
			return workerAsyncResult;
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000AC3E8 File Offset: 0x000AB3E8
		private void BeginWriteCallback(IAsyncResult transportResult)
		{
			if (transportResult.CompletedSynchronously)
			{
				return;
			}
			WorkerAsyncResult workerAsyncResult = (WorkerAsyncResult)transportResult.AsyncState;
			try
			{
				this.BeginWriteComplete(transportResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				workerAsyncResult.InvokeCallback(ex);
			}
			catch
			{
				workerAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000AC46C File Offset: 0x000AB46C
		private void BeginWriteComplete(IAsyncResult transportResult)
		{
			WorkerAsyncResult workerAsyncResult;
			for (;;)
			{
				workerAsyncResult = (WorkerAsyncResult)transportResult.AsyncState;
				this.Transport.EndWrite(transportResult);
				if (workerAsyncResult.Offset == workerAsyncResult.End)
				{
					break;
				}
				workerAsyncResult.Offset = workerAsyncResult.End;
				transportResult = this.Transport.BeginWrite(workerAsyncResult.Buffer, 0, workerAsyncResult.End, this.m_BeginWriteCallback, workerAsyncResult);
				if (!transportResult.CompletedSynchronously)
				{
					return;
				}
			}
			workerAsyncResult.InvokeCallback();
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000AC4DC File Offset: 0x000AB4DC
		public void EndWriteMessage(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			WorkerAsyncResult workerAsyncResult = asyncResult as WorkerAsyncResult;
			if (workerAsyncResult != null)
			{
				if (!workerAsyncResult.InternalPeekCompleted)
				{
					workerAsyncResult.InternalWaitForCompletion();
				}
				if (workerAsyncResult.Result is Exception)
				{
					throw (Exception)workerAsyncResult.Result;
				}
			}
			else
			{
				this.Transport.EndWrite(asyncResult);
			}
		}

		// Token: 0x04002832 RID: 10290
		private Stream m_Transport;

		// Token: 0x04002833 RID: 10291
		private bool m_Eof;

		// Token: 0x04002834 RID: 10292
		private FrameHeader m_WriteHeader = new FrameHeader();

		// Token: 0x04002835 RID: 10293
		private FrameHeader m_CurReadHeader = new FrameHeader();

		// Token: 0x04002836 RID: 10294
		private FrameHeader m_ReadVerifier = new FrameHeader(-1, -1, -1);

		// Token: 0x04002837 RID: 10295
		private byte[] m_ReadHeaderBuffer;

		// Token: 0x04002838 RID: 10296
		private byte[] m_WriteHeaderBuffer;

		// Token: 0x04002839 RID: 10297
		private readonly AsyncCallback m_ReadFrameCallback;

		// Token: 0x0400283A RID: 10298
		private readonly AsyncCallback m_BeginWriteCallback;

		// Token: 0x0400283B RID: 10299
		private NetworkStream m_NetworkStream;
	}
}
