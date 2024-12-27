using System;
using System.IO;
using System.Text;

namespace System.Net
{
	// Token: 0x020004BA RID: 1210
	internal class CommandStream : PooledStream
	{
		// Token: 0x06002592 RID: 9618 RVA: 0x000957E8 File Offset: 0x000947E8
		internal CommandStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
			: base(connectionPool, lifetime, checkLifetime)
		{
			this.m_Decoder = this.m_Encoding.GetDecoder();
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x0009581C File Offset: 0x0009481C
		internal virtual void Abort(Exception e)
		{
			lock (this)
			{
				if (this.m_Aborted)
				{
					return;
				}
				this.m_Aborted = true;
				base.CanBePooled = false;
			}
			try
			{
				base.Close(0);
			}
			finally
			{
				if (e != null)
				{
					this.InvokeRequestCallback(e);
				}
				else
				{
					this.InvokeRequestCallback(null);
				}
			}
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x0009588C File Offset: 0x0009488C
		protected override void Dispose(bool disposing)
		{
			this.InvokeRequestCallback(null);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00095898 File Offset: 0x00094898
		protected void InvokeRequestCallback(object obj)
		{
			WebRequest request = this.m_Request;
			if (request != null)
			{
				request.RequestCallback(obj);
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06002596 RID: 9622 RVA: 0x000958B6 File Offset: 0x000948B6
		internal bool RecoverableFailure
		{
			get
			{
				return this.m_RecoverableFailure;
			}
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000958BE File Offset: 0x000948BE
		protected void MarkAsRecoverableFailure()
		{
			if (this.m_Index <= 1)
			{
				this.m_RecoverableFailure = true;
			}
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000958D0 File Offset: 0x000948D0
		internal Stream SubmitRequest(WebRequest request, bool async, bool readInitalResponseOnConnect)
		{
			this.ClearState();
			base.UpdateLifetime();
			CommandStream.PipelineEntry[] array = this.BuildCommandsList(request);
			this.InitCommandPipeline(request, array, async);
			if (readInitalResponseOnConnect && base.JustConnected)
			{
				this.m_DoSend = false;
				this.m_Index = -1;
			}
			return this.ContinueCommandPipeline();
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x00095919 File Offset: 0x00094919
		protected virtual void ClearState()
		{
			this.InitCommandPipeline(null, null, false);
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x00095924 File Offset: 0x00094924
		protected virtual CommandStream.PipelineEntry[] BuildCommandsList(WebRequest request)
		{
			return null;
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00095927 File Offset: 0x00094927
		protected Exception GenerateException(WebExceptionStatus status, Exception innerException)
		{
			return new WebException(NetRes.GetWebStatusString("net_connclosed", status), innerException, status, null);
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x0009593C File Offset: 0x0009493C
		protected Exception GenerateException(FtpStatusCode code, string statusDescription, Exception innerException)
		{
			return new WebException(SR.GetString("net_servererror", new object[] { NetRes.GetWebStatusCodeString(code, statusDescription) }), innerException, WebExceptionStatus.ProtocolError, null);
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x00095970 File Offset: 0x00094970
		protected void InitCommandPipeline(WebRequest request, CommandStream.PipelineEntry[] commands, bool async)
		{
			this.m_Commands = commands;
			this.m_Index = 0;
			this.m_Request = request;
			this.m_Aborted = false;
			this.m_DoRead = true;
			this.m_DoSend = true;
			this.m_CurrentResponseDescription = null;
			this.m_Async = async;
			this.m_RecoverableFailure = false;
			this.m_AbortReason = string.Empty;
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x000959C8 File Offset: 0x000949C8
		internal void CheckContinuePipeline()
		{
			if (this.m_Async)
			{
				return;
			}
			try
			{
				this.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				this.Abort(ex);
			}
			catch
			{
				this.Abort(new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x00095A28 File Offset: 0x00094A28
		protected Stream ContinueCommandPipeline()
		{
			bool async = this.m_Async;
			while (this.m_Index < this.m_Commands.Length)
			{
				if (this.m_DoSend)
				{
					if (this.m_Index < 0)
					{
						throw new InternalException();
					}
					byte[] bytes = this.Encoding.GetBytes(this.m_Commands[this.m_Index].Command);
					if (Logging.On)
					{
						string text = this.m_Commands[this.m_Index].Command.Substring(0, this.m_Commands[this.m_Index].Command.Length - 2);
						if (this.m_Commands[this.m_Index].HasFlag(CommandStream.PipelineEntryFlags.DontLogParameter))
						{
							int num = text.IndexOf(' ');
							if (num != -1)
							{
								text = text.Substring(0, num) + " ********";
							}
						}
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_sending_command", new object[] { text }));
					}
					try
					{
						if (async)
						{
							this.BeginWrite(bytes, 0, bytes.Length, CommandStream.m_WriteCallbackDelegate, this);
						}
						else
						{
							this.Write(bytes, 0, bytes.Length);
						}
					}
					catch (IOException)
					{
						this.MarkAsRecoverableFailure();
						throw;
					}
					catch
					{
						throw;
					}
					if (async)
					{
						return null;
					}
				}
				Stream stream = null;
				bool flag = this.PostSendCommandProcessing(ref stream);
				if (flag)
				{
					return stream;
				}
			}
			lock (this)
			{
				this.Close();
			}
			return null;
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x00095BAC File Offset: 0x00094BAC
		private bool PostSendCommandProcessing(ref Stream stream)
		{
			if (this.m_DoRead)
			{
				bool async = this.m_Async;
				int index = this.m_Index;
				CommandStream.PipelineEntry[] commands = this.m_Commands;
				try
				{
					ResponseDescription responseDescription = this.ReceiveCommandResponse();
					if (async)
					{
						return true;
					}
					this.m_CurrentResponseDescription = responseDescription;
				}
				catch
				{
					if (index < 0 || index >= commands.Length || commands[index].Command != "QUIT\r\n")
					{
						throw;
					}
				}
			}
			return this.PostReadCommandProcessing(ref stream);
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x00095C2C File Offset: 0x00094C2C
		private bool PostReadCommandProcessing(ref Stream stream)
		{
			if (this.m_Index >= this.m_Commands.Length)
			{
				return false;
			}
			this.m_DoSend = false;
			this.m_DoRead = false;
			CommandStream.PipelineEntry pipelineEntry;
			if (this.m_Index == -1)
			{
				pipelineEntry = null;
			}
			else
			{
				pipelineEntry = this.m_Commands[this.m_Index];
			}
			CommandStream.PipelineInstruction pipelineInstruction;
			if (this.m_CurrentResponseDescription == null && pipelineEntry.Command == "QUIT\r\n")
			{
				pipelineInstruction = CommandStream.PipelineInstruction.Advance;
			}
			else
			{
				pipelineInstruction = this.PipelineCallback(pipelineEntry, this.m_CurrentResponseDescription, false, ref stream);
			}
			if (pipelineInstruction == CommandStream.PipelineInstruction.Abort)
			{
				Exception ex;
				if (this.m_AbortReason != string.Empty)
				{
					ex = new WebException(this.m_AbortReason);
				}
				else
				{
					ex = this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
				}
				this.Abort(ex);
				throw ex;
			}
			if (pipelineInstruction == CommandStream.PipelineInstruction.Advance)
			{
				this.m_CurrentResponseDescription = null;
				this.m_DoSend = true;
				this.m_DoRead = true;
				this.m_Index++;
			}
			else
			{
				if (pipelineInstruction == CommandStream.PipelineInstruction.Pause)
				{
					return true;
				}
				if (pipelineInstruction == CommandStream.PipelineInstruction.GiveStream)
				{
					this.m_CurrentResponseDescription = null;
					this.m_DoRead = true;
					if (this.m_Async)
					{
						this.ContinueCommandPipeline();
						this.InvokeRequestCallback(stream);
					}
					return true;
				}
				if (pipelineInstruction == CommandStream.PipelineInstruction.Reread)
				{
					this.m_CurrentResponseDescription = null;
					this.m_DoRead = true;
				}
			}
			return false;
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x00095D45 File Offset: 0x00094D45
		protected virtual CommandStream.PipelineInstruction PipelineCallback(CommandStream.PipelineEntry entry, ResponseDescription response, bool timeout, ref Stream stream)
		{
			return CommandStream.PipelineInstruction.Abort;
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x00095D48 File Offset: 0x00094D48
		private static void ReadCallback(IAsyncResult asyncResult)
		{
			ReceiveState receiveState = (ReceiveState)asyncResult.AsyncState;
			try
			{
				Stream connection = receiveState.Connection;
				int num = 0;
				try
				{
					num = connection.EndRead(asyncResult);
					if (num == 0)
					{
						receiveState.Connection.CloseSocket();
					}
				}
				catch (IOException)
				{
					receiveState.Connection.MarkAsRecoverableFailure();
					throw;
				}
				catch
				{
					throw;
				}
				receiveState.Connection.ReceiveCommandResponseCallback(receiveState, num);
			}
			catch (Exception ex)
			{
				receiveState.Connection.Abort(ex);
			}
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x00095DDC File Offset: 0x00094DDC
		private static void WriteCallback(IAsyncResult asyncResult)
		{
			CommandStream commandStream = (CommandStream)asyncResult.AsyncState;
			try
			{
				try
				{
					commandStream.EndWrite(asyncResult);
				}
				catch (IOException)
				{
					commandStream.MarkAsRecoverableFailure();
					throw;
				}
				catch
				{
					throw;
				}
				Stream stream = null;
				if (!commandStream.PostSendCommandProcessing(ref stream))
				{
					commandStream.ContinueCommandPipeline();
				}
			}
			catch (Exception ex)
			{
				commandStream.Abort(ex);
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x00095E54 File Offset: 0x00094E54
		// (set) Token: 0x060025A6 RID: 9638 RVA: 0x00095E5C File Offset: 0x00094E5C
		protected Encoding Encoding
		{
			get
			{
				return this.m_Encoding;
			}
			set
			{
				this.m_Encoding = value;
				this.m_Decoder = this.m_Encoding.GetDecoder();
			}
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x00095E76 File Offset: 0x00094E76
		protected virtual bool CheckValid(ResponseDescription response, ref int validThrough, ref int completeLength)
		{
			return false;
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x00095E7C File Offset: 0x00094E7C
		private ResponseDescription ReceiveCommandResponse()
		{
			ReceiveState receiveState = new ReceiveState(this);
			try
			{
				if (this.m_Buffer.Length > 0)
				{
					this.ReceiveCommandResponseCallback(receiveState, -1);
				}
				else
				{
					try
					{
						if (this.m_Async)
						{
							this.BeginRead(receiveState.Buffer, 0, receiveState.Buffer.Length, CommandStream.m_ReadCallbackDelegate, receiveState);
							return null;
						}
						int num = this.Read(receiveState.Buffer, 0, receiveState.Buffer.Length);
						if (num == 0)
						{
							base.CloseSocket();
						}
						this.ReceiveCommandResponseCallback(receiveState, num);
					}
					catch (IOException)
					{
						this.MarkAsRecoverableFailure();
						throw;
					}
					catch
					{
						throw;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is WebException)
				{
					throw;
				}
				throw this.GenerateException(WebExceptionStatus.ReceiveFailure, ex);
			}
			return receiveState.Resp;
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00095F4C File Offset: 0x00094F4C
		private void ReceiveCommandResponseCallback(ReceiveState state, int bytesRead)
		{
			int num = -1;
			for (;;)
			{
				int validThrough = state.ValidThrough;
				if (this.m_Buffer.Length > 0)
				{
					state.Resp.StatusBuffer.Append(this.m_Buffer);
					this.m_Buffer = string.Empty;
					if (!this.CheckValid(state.Resp, ref validThrough, ref num))
					{
						break;
					}
				}
				else
				{
					if (bytesRead <= 0)
					{
						goto Block_3;
					}
					char[] array = new char[this.m_Decoder.GetCharCount(state.Buffer, 0, bytesRead)];
					int chars = this.m_Decoder.GetChars(state.Buffer, 0, bytesRead, array, 0, false);
					string text = new string(array, 0, chars);
					state.Resp.StatusBuffer.Append(text);
					if (!this.CheckValid(state.Resp, ref validThrough, ref num))
					{
						goto Block_4;
					}
					if (num >= 0)
					{
						int num2 = state.Resp.StatusBuffer.Length - num;
						if (num2 > 0)
						{
							this.m_Buffer = text.Substring(text.Length - num2, num2);
						}
					}
				}
				if (num < 0)
				{
					state.ValidThrough = validThrough;
					try
					{
						if (this.m_Async)
						{
							this.BeginRead(state.Buffer, 0, state.Buffer.Length, CommandStream.m_ReadCallbackDelegate, state);
							return;
						}
						bytesRead = this.Read(state.Buffer, 0, state.Buffer.Length);
						if (bytesRead == 0)
						{
							base.CloseSocket();
						}
						continue;
					}
					catch (IOException)
					{
						this.MarkAsRecoverableFailure();
						throw;
					}
					catch
					{
						throw;
					}
					goto IL_016A;
				}
				goto IL_016A;
			}
			throw this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
			Block_3:
			throw this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
			Block_4:
			throw this.GenerateException(WebExceptionStatus.ServerProtocolViolation, null);
			IL_016A:
			string text2 = state.Resp.StatusBuffer.ToString();
			state.Resp.StatusDescription = text2.Substring(0, num);
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_received_response", new object[] { text2.Substring(0, num - 2) }));
			}
			if (this.m_Async)
			{
				if (state.Resp != null)
				{
					this.m_CurrentResponseDescription = state.Resp;
				}
				Stream stream = null;
				if (this.PostReadCommandProcessing(ref stream))
				{
					return;
				}
				this.ContinueCommandPipeline();
			}
		}

		// Token: 0x04002529 RID: 9513
		private const int _WaitingForPipeline = 1;

		// Token: 0x0400252A RID: 9514
		private const int _CompletedPipeline = 2;

		// Token: 0x0400252B RID: 9515
		private static readonly AsyncCallback m_WriteCallbackDelegate = new AsyncCallback(CommandStream.WriteCallback);

		// Token: 0x0400252C RID: 9516
		private static readonly AsyncCallback m_ReadCallbackDelegate = new AsyncCallback(CommandStream.ReadCallback);

		// Token: 0x0400252D RID: 9517
		private bool m_RecoverableFailure;

		// Token: 0x0400252E RID: 9518
		protected WebRequest m_Request;

		// Token: 0x0400252F RID: 9519
		protected bool m_Async;

		// Token: 0x04002530 RID: 9520
		private bool m_Aborted;

		// Token: 0x04002531 RID: 9521
		protected CommandStream.PipelineEntry[] m_Commands;

		// Token: 0x04002532 RID: 9522
		protected int m_Index;

		// Token: 0x04002533 RID: 9523
		private bool m_DoRead;

		// Token: 0x04002534 RID: 9524
		private bool m_DoSend;

		// Token: 0x04002535 RID: 9525
		private ResponseDescription m_CurrentResponseDescription;

		// Token: 0x04002536 RID: 9526
		protected string m_AbortReason;

		// Token: 0x04002537 RID: 9527
		private string m_Buffer = string.Empty;

		// Token: 0x04002538 RID: 9528
		private Encoding m_Encoding = Encoding.UTF8;

		// Token: 0x04002539 RID: 9529
		private Decoder m_Decoder;

		// Token: 0x020004BB RID: 1211
		internal enum PipelineInstruction
		{
			// Token: 0x0400253B RID: 9531
			Abort,
			// Token: 0x0400253C RID: 9532
			Advance,
			// Token: 0x0400253D RID: 9533
			Pause,
			// Token: 0x0400253E RID: 9534
			Reread,
			// Token: 0x0400253F RID: 9535
			GiveStream
		}

		// Token: 0x020004BC RID: 1212
		[Flags]
		internal enum PipelineEntryFlags
		{
			// Token: 0x04002541 RID: 9537
			UserCommand = 1,
			// Token: 0x04002542 RID: 9538
			GiveDataStream = 2,
			// Token: 0x04002543 RID: 9539
			CreateDataConnection = 4,
			// Token: 0x04002544 RID: 9540
			DontLogParameter = 8
		}

		// Token: 0x020004BD RID: 1213
		internal class PipelineEntry
		{
			// Token: 0x060025AB RID: 9643 RVA: 0x00096190 File Offset: 0x00095190
			internal PipelineEntry(string command)
			{
				this.Command = command;
			}

			// Token: 0x060025AC RID: 9644 RVA: 0x0009619F File Offset: 0x0009519F
			internal PipelineEntry(string command, CommandStream.PipelineEntryFlags flags)
			{
				this.Command = command;
				this.Flags = flags;
			}

			// Token: 0x060025AD RID: 9645 RVA: 0x000961B5 File Offset: 0x000951B5
			internal bool HasFlag(CommandStream.PipelineEntryFlags flags)
			{
				return (this.Flags & flags) != (CommandStream.PipelineEntryFlags)0;
			}

			// Token: 0x04002545 RID: 9541
			internal string Command;

			// Token: 0x04002546 RID: 9542
			internal CommandStream.PipelineEntryFlags Flags;
		}
	}
}
