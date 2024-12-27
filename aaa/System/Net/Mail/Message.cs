using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020006A7 RID: 1703
	internal class Message
	{
		// Token: 0x06003484 RID: 13444 RVA: 0x000DEE84 File Offset: 0x000DDE84
		internal Message()
		{
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x000DEE94 File Offset: 0x000DDE94
		internal Message(string from, string to)
			: this()
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			if (from == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "from" }), "from");
			}
			if (to == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "to" }), "to");
			}
			this.from = new MailAddress(from);
			this.to = new MailAddressCollection { to };
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x000DEF47 File Offset: 0x000DDF47
		internal Message(MailAddress from, MailAddress to)
			: this()
		{
			this.from = from;
			this.To.Add(to);
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x000DEF62 File Offset: 0x000DDF62
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x000DEF75 File Offset: 0x000DDF75
		public MailPriority Priority
		{
			get
			{
				if (this.priority != (MailPriority)(-1))
				{
					return this.priority;
				}
				return MailPriority.Normal;
			}
			set
			{
				this.priority = value;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06003489 RID: 13449 RVA: 0x000DEF7E File Offset: 0x000DDF7E
		// (set) Token: 0x0600348A RID: 13450 RVA: 0x000DEF86 File Offset: 0x000DDF86
		internal MailAddress From
		{
			get
			{
				return this.from;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.from = value;
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x0600348B RID: 13451 RVA: 0x000DEF9D File Offset: 0x000DDF9D
		// (set) Token: 0x0600348C RID: 13452 RVA: 0x000DEFA5 File Offset: 0x000DDFA5
		internal MailAddress Sender
		{
			get
			{
				return this.sender;
			}
			set
			{
				this.sender = value;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x000DEFAE File Offset: 0x000DDFAE
		// (set) Token: 0x0600348E RID: 13454 RVA: 0x000DEFB6 File Offset: 0x000DDFB6
		internal MailAddress ReplyTo
		{
			get
			{
				return this.replyTo;
			}
			set
			{
				this.replyTo = value;
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x000DEFBF File Offset: 0x000DDFBF
		internal MailAddressCollection To
		{
			get
			{
				if (this.to == null)
				{
					this.to = new MailAddressCollection();
				}
				return this.to;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06003490 RID: 13456 RVA: 0x000DEFDA File Offset: 0x000DDFDA
		internal MailAddressCollection Bcc
		{
			get
			{
				if (this.bcc == null)
				{
					this.bcc = new MailAddressCollection();
				}
				return this.bcc;
			}
		}

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x000DEFF5 File Offset: 0x000DDFF5
		internal MailAddressCollection CC
		{
			get
			{
				if (this.cc == null)
				{
					this.cc = new MailAddressCollection();
				}
				return this.cc;
			}
		}

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06003492 RID: 13458 RVA: 0x000DF010 File Offset: 0x000DE010
		// (set) Token: 0x06003493 RID: 13459 RVA: 0x000DF018 File Offset: 0x000DE018
		internal string Subject
		{
			get
			{
				return this.subject;
			}
			set
			{
				if (value != null && MailBnfHelper.HasCROrLF(value))
				{
					throw new ArgumentException(SR.GetString("MailSubjectInvalidFormat"));
				}
				this.subject = value;
				if (this.subject != null && this.subjectEncoding == null && !MimeBasePart.IsAscii(this.subject, false))
				{
					this.subjectEncoding = Encoding.GetEncoding("utf-8");
				}
			}
		}

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06003494 RID: 13460 RVA: 0x000DF075 File Offset: 0x000DE075
		// (set) Token: 0x06003495 RID: 13461 RVA: 0x000DF07D File Offset: 0x000DE07D
		internal Encoding SubjectEncoding
		{
			get
			{
				return this.subjectEncoding;
			}
			set
			{
				this.subjectEncoding = value;
			}
		}

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06003496 RID: 13462 RVA: 0x000DF086 File Offset: 0x000DE086
		internal NameValueCollection Headers
		{
			get
			{
				if (this.headers == null)
				{
					this.headers = new HeaderCollection();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, this.headers);
					}
				}
				return this.headers;
			}
		}

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06003497 RID: 13463 RVA: 0x000DF0B9 File Offset: 0x000DE0B9
		internal NameValueCollection EnvelopeHeaders
		{
			get
			{
				if (this.envelopeHeaders == null)
				{
					this.envelopeHeaders = new HeaderCollection();
					if (Logging.On)
					{
						Logging.Associate(Logging.Web, this, this.envelopeHeaders);
					}
				}
				return this.envelopeHeaders;
			}
		}

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06003498 RID: 13464 RVA: 0x000DF0EC File Offset: 0x000DE0EC
		// (set) Token: 0x06003499 RID: 13465 RVA: 0x000DF0F4 File Offset: 0x000DE0F4
		internal virtual MimeBasePart Content
		{
			get
			{
				return this.content;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.content = value;
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x000DF10C File Offset: 0x000DE10C
		internal void EmptySendCallback(IAsyncResult result)
		{
			Exception ex = null;
			if (result.CompletedSynchronously)
			{
				return;
			}
			Message.EmptySendContext emptySendContext = (Message.EmptySendContext)result.AsyncState;
			try
			{
				emptySendContext.writer.EndGetContentStream(result).Close();
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			catch
			{
				ex = new Exception(SR.GetString("net_nonClsCompliantException"));
			}
			emptySendContext.result.InvokeCallback(ex);
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x000DF184 File Offset: 0x000DE184
		internal virtual IAsyncResult BeginSend(BaseWriter writer, bool sendEnvelope, AsyncCallback callback, object state)
		{
			this.PrepareHeaders(sendEnvelope);
			writer.WriteHeaders(this.Headers);
			if (this.Content != null)
			{
				return this.Content.BeginSend(writer, callback, state);
			}
			LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(this, state, callback);
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.EmptySendCallback), new Message.EmptySendContext(writer, lazyAsyncResult));
			if (asyncResult.CompletedSynchronously)
			{
				writer.EndGetContentStream(asyncResult).Close();
			}
			return lazyAsyncResult;
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x000DF1F8 File Offset: 0x000DE1F8
		internal virtual void EndSend(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (this.Content != null)
			{
				this.Content.EndSend(asyncResult);
				return;
			}
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			if (lazyAsyncResult == null || lazyAsyncResult.AsyncObject != this)
			{
				throw new ArgumentException(SR.GetString("net_io_invalidasyncresult"));
			}
			if (lazyAsyncResult.EndCalled)
			{
				throw new InvalidOperationException(SR.GetString("net_io_invalidendcall", new object[] { "EndSend" }));
			}
			lazyAsyncResult.InternalWaitForCompletion();
			lazyAsyncResult.EndCalled = true;
			if (lazyAsyncResult.Result is Exception)
			{
				throw (Exception)lazyAsyncResult.Result;
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x000DF29C File Offset: 0x000DE29C
		internal virtual void Send(BaseWriter writer, bool sendEnvelope)
		{
			if (sendEnvelope)
			{
				this.PrepareEnvelopeHeaders(sendEnvelope);
				writer.WriteHeaders(this.EnvelopeHeaders);
			}
			this.PrepareHeaders(sendEnvelope);
			writer.WriteHeaders(this.Headers);
			if (this.Content != null)
			{
				this.Content.Send(writer);
				return;
			}
			writer.GetContentStream().Close();
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x000DF2F4 File Offset: 0x000DE2F4
		internal void PrepareEnvelopeHeaders(bool sendEnvelope)
		{
			this.EnvelopeHeaders[MailHeaderInfo.GetString(MailHeaderID.XSender)] = this.From.ToEncodedString();
			this.EnvelopeHeaders.Remove(MailHeaderInfo.GetString(MailHeaderID.XReceiver));
			foreach (MailAddress mailAddress in this.To)
			{
				this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), mailAddress.ToEncodedString());
			}
			foreach (MailAddress mailAddress2 in this.CC)
			{
				this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), mailAddress2.ToEncodedString());
			}
			foreach (MailAddress mailAddress3 in this.Bcc)
			{
				this.EnvelopeHeaders.Add(MailHeaderInfo.GetString(MailHeaderID.XReceiver), mailAddress3.ToEncodedString());
			}
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x000DF428 File Offset: 0x000DE428
		internal void PrepareHeaders(bool sendEnvelope)
		{
			this.Headers[MailHeaderInfo.GetString(MailHeaderID.MimeVersion)] = "1.0";
			this.Headers[MailHeaderInfo.GetString(MailHeaderID.From)] = this.From.ToEncodedString();
			if (this.Sender != null)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Sender)] = this.Sender.ToEncodedString();
			}
			else
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Sender));
			}
			if (this.To.Count > 0)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.To)] = this.To.ToEncodedString();
			}
			else
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.To));
			}
			if (this.CC.Count > 0)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Cc)] = this.CC.ToEncodedString();
			}
			else
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Cc));
			}
			if (this.replyTo != null)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.ReplyTo)] = this.ReplyTo.ToEncodedString();
			}
			if (this.priority == MailPriority.High)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.XPriority)] = "1";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Priority)] = "urgent";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Importance)] = "high";
			}
			else if (this.priority == MailPriority.Low)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.XPriority)] = "5";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Priority)] = "non-urgent";
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Importance)] = "low";
			}
			else if (this.priority != (MailPriority)(-1))
			{
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.XPriority));
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Priority));
				this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Importance));
			}
			this.Headers[MailHeaderInfo.GetString(MailHeaderID.Date)] = MailBnfHelper.GetDateTimeString(DateTime.Now, null);
			if (this.subject != null && this.subject != string.Empty)
			{
				this.Headers[MailHeaderInfo.GetString(MailHeaderID.Subject)] = MimeBasePart.EncodeHeaderValue(this.subject, this.subjectEncoding, MimeBasePart.ShouldUseBase64Encoding(this.subjectEncoding));
				return;
			}
			this.Headers.Remove(MailHeaderInfo.GetString(MailHeaderID.Subject));
		}

		// Token: 0x04003058 RID: 12376
		private MailAddress from;

		// Token: 0x04003059 RID: 12377
		private MailAddress sender;

		// Token: 0x0400305A RID: 12378
		private MailAddress replyTo;

		// Token: 0x0400305B RID: 12379
		private MailAddressCollection to;

		// Token: 0x0400305C RID: 12380
		private MailAddressCollection cc;

		// Token: 0x0400305D RID: 12381
		private MailAddressCollection bcc;

		// Token: 0x0400305E RID: 12382
		private MimeBasePart content;

		// Token: 0x0400305F RID: 12383
		private HeaderCollection headers;

		// Token: 0x04003060 RID: 12384
		private HeaderCollection envelopeHeaders;

		// Token: 0x04003061 RID: 12385
		private string subject;

		// Token: 0x04003062 RID: 12386
		private Encoding subjectEncoding;

		// Token: 0x04003063 RID: 12387
		private MailPriority priority = (MailPriority)(-1);

		// Token: 0x020006A8 RID: 1704
		internal class EmptySendContext
		{
			// Token: 0x060034A0 RID: 13472 RVA: 0x000DF69D File Offset: 0x000DE69D
			internal EmptySendContext(BaseWriter writer, LazyAsyncResult result)
			{
				this.writer = writer;
				this.result = result;
			}

			// Token: 0x04003064 RID: 12388
			internal LazyAsyncResult result;

			// Token: 0x04003065 RID: 12389
			internal BaseWriter writer;
		}
	}
}
