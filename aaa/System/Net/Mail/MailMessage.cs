using System;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x020006A0 RID: 1696
	public class MailMessage : IDisposable
	{
		// Token: 0x0600344F RID: 13391 RVA: 0x000DE0CC File Offset: 0x000DD0CC
		public MailMessage()
		{
			this.message = new Message();
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.message);
			}
			string from = SmtpClient.MailConfiguration.Smtp.From;
			if (from != null && from.Length > 0)
			{
				this.message.From = new MailAddress(from);
			}
		}

		// Token: 0x06003450 RID: 13392 RVA: 0x000DE13C File Offset: 0x000DD13C
		public MailMessage(string from, string to)
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
			this.message = new Message(from, to);
			if (Logging.On)
			{
				Logging.Associate(Logging.Web, this, this.message);
			}
		}

		// Token: 0x06003451 RID: 13393 RVA: 0x000DE1FF File Offset: 0x000DD1FF
		public MailMessage(string from, string to, string subject, string body)
			: this(from, to)
		{
			this.Subject = subject;
			this.Body = body;
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x000DE218 File Offset: 0x000DD218
		public MailMessage(MailAddress from, MailAddress to)
		{
			if (from == null)
			{
				throw new ArgumentNullException("from");
			}
			if (to == null)
			{
				throw new ArgumentNullException("to");
			}
			this.message = new Message(from, to);
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x000DE254 File Offset: 0x000DD254
		// (set) Token: 0x06003454 RID: 13396 RVA: 0x000DE261 File Offset: 0x000DD261
		public MailAddress From
		{
			get
			{
				return this.message.From;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.message.From = value;
			}
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x000DE27D File Offset: 0x000DD27D
		// (set) Token: 0x06003456 RID: 13398 RVA: 0x000DE28A File Offset: 0x000DD28A
		public MailAddress Sender
		{
			get
			{
				return this.message.Sender;
			}
			set
			{
				this.message.Sender = value;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06003457 RID: 13399 RVA: 0x000DE298 File Offset: 0x000DD298
		// (set) Token: 0x06003458 RID: 13400 RVA: 0x000DE2A5 File Offset: 0x000DD2A5
		public MailAddress ReplyTo
		{
			get
			{
				return this.message.ReplyTo;
			}
			set
			{
				this.message.ReplyTo = value;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06003459 RID: 13401 RVA: 0x000DE2B3 File Offset: 0x000DD2B3
		public MailAddressCollection To
		{
			get
			{
				return this.message.To;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600345A RID: 13402 RVA: 0x000DE2C0 File Offset: 0x000DD2C0
		public MailAddressCollection Bcc
		{
			get
			{
				return this.message.Bcc;
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x0600345B RID: 13403 RVA: 0x000DE2CD File Offset: 0x000DD2CD
		public MailAddressCollection CC
		{
			get
			{
				return this.message.CC;
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600345C RID: 13404 RVA: 0x000DE2DA File Offset: 0x000DD2DA
		// (set) Token: 0x0600345D RID: 13405 RVA: 0x000DE2E7 File Offset: 0x000DD2E7
		public MailPriority Priority
		{
			get
			{
				return this.message.Priority;
			}
			set
			{
				this.message.Priority = value;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600345E RID: 13406 RVA: 0x000DE2F5 File Offset: 0x000DD2F5
		// (set) Token: 0x0600345F RID: 13407 RVA: 0x000DE2FD File Offset: 0x000DD2FD
		public DeliveryNotificationOptions DeliveryNotificationOptions
		{
			get
			{
				return this.deliveryStatusNotification;
			}
			set
			{
				if ((DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure | DeliveryNotificationOptions.Delay) < value && value != DeliveryNotificationOptions.Never)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.deliveryStatusNotification = value;
			}
		}

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06003460 RID: 13408 RVA: 0x000DE31D File Offset: 0x000DD31D
		// (set) Token: 0x06003461 RID: 13409 RVA: 0x000DE33D File Offset: 0x000DD33D
		public string Subject
		{
			get
			{
				if (this.message.Subject == null)
				{
					return string.Empty;
				}
				return this.message.Subject;
			}
			set
			{
				this.message.Subject = value;
			}
		}

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06003462 RID: 13410 RVA: 0x000DE34B File Offset: 0x000DD34B
		// (set) Token: 0x06003463 RID: 13411 RVA: 0x000DE358 File Offset: 0x000DD358
		public Encoding SubjectEncoding
		{
			get
			{
				return this.message.SubjectEncoding;
			}
			set
			{
				this.message.SubjectEncoding = value;
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x000DE366 File Offset: 0x000DD366
		public NameValueCollection Headers
		{
			get
			{
				return this.message.Headers;
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06003465 RID: 13413 RVA: 0x000DE373 File Offset: 0x000DD373
		// (set) Token: 0x06003466 RID: 13414 RVA: 0x000DE38C File Offset: 0x000DD38C
		public string Body
		{
			get
			{
				if (this.body == null)
				{
					return string.Empty;
				}
				return this.body;
			}
			set
			{
				this.body = value;
				if (this.bodyEncoding == null && this.body != null)
				{
					if (MimeBasePart.IsAscii(this.body, true))
					{
						this.bodyEncoding = Encoding.ASCII;
						return;
					}
					this.bodyEncoding = Encoding.GetEncoding("utf-8");
				}
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x000DE3DA File Offset: 0x000DD3DA
		// (set) Token: 0x06003468 RID: 13416 RVA: 0x000DE3E2 File Offset: 0x000DD3E2
		public Encoding BodyEncoding
		{
			get
			{
				return this.bodyEncoding;
			}
			set
			{
				this.bodyEncoding = value;
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06003469 RID: 13417 RVA: 0x000DE3EB File Offset: 0x000DD3EB
		// (set) Token: 0x0600346A RID: 13418 RVA: 0x000DE3F3 File Offset: 0x000DD3F3
		public bool IsBodyHtml
		{
			get
			{
				return this.isBodyHtml;
			}
			set
			{
				this.isBodyHtml = value;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x0600346B RID: 13419 RVA: 0x000DE3FC File Offset: 0x000DD3FC
		public AttachmentCollection Attachments
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.attachments == null)
				{
					this.attachments = new AttachmentCollection();
				}
				return this.attachments;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x0600346C RID: 13420 RVA: 0x000DE430 File Offset: 0x000DD430
		public AlternateViewCollection AlternateViews
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().FullName);
				}
				if (this.views == null)
				{
					this.views = new AlternateViewCollection();
				}
				return this.views;
			}
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x000DE464 File Offset: 0x000DD464
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x000DE470 File Offset: 0x000DD470
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this.disposed)
			{
				this.disposed = true;
				if (this.views != null)
				{
					this.views.Dispose();
				}
				if (this.attachments != null)
				{
					this.attachments.Dispose();
				}
				if (this.bodyView != null)
				{
					this.bodyView.Dispose();
				}
			}
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000DE4C8 File Offset: 0x000DD4C8
		private void SetContent()
		{
			if (this.bodyView != null)
			{
				this.bodyView.Dispose();
				this.bodyView = null;
			}
			if (this.AlternateViews.Count == 0 && this.Attachments.Count == 0)
			{
				if (this.body != null && this.body != string.Empty)
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
					this.message.Content = this.bodyView.MimePart;
					return;
				}
			}
			else
			{
				if (this.AlternateViews.Count == 0 && this.Attachments.Count > 0)
				{
					MimeMultiPart mimeMultiPart = new MimeMultiPart(MimeMultiPartType.Mixed);
					if (this.body != null && this.body != string.Empty)
					{
						this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, this.isBodyHtml ? "text/html" : null);
					}
					else
					{
						this.bodyView = AlternateView.CreateAlternateViewFromString(string.Empty);
					}
					mimeMultiPart.Parts.Add(this.bodyView.MimePart);
					foreach (Attachment attachment in this.Attachments)
					{
						if (attachment != null)
						{
							attachment.PrepareForSending();
							mimeMultiPart.Parts.Add(attachment.MimePart);
						}
					}
					this.message.Content = mimeMultiPart;
					return;
				}
				MimeMultiPart mimeMultiPart2 = null;
				MimeMultiPart mimeMultiPart3 = new MimeMultiPart(MimeMultiPartType.Alternative);
				if (this.body != null && this.body != string.Empty)
				{
					this.bodyView = AlternateView.CreateAlternateViewFromString(this.body, this.bodyEncoding, null);
					mimeMultiPart3.Parts.Add(this.bodyView.MimePart);
				}
				foreach (AlternateView alternateView in this.AlternateViews)
				{
					if (alternateView != null)
					{
						alternateView.PrepareForSending();
						if (alternateView.LinkedResources.Count > 0)
						{
							MimeMultiPart mimeMultiPart4 = new MimeMultiPart(MimeMultiPartType.Related);
							mimeMultiPart4.ContentType.Parameters["type"] = alternateView.ContentType.MediaType;
							mimeMultiPart4.ContentLocation = alternateView.MimePart.ContentLocation;
							mimeMultiPart4.Parts.Add(alternateView.MimePart);
							foreach (LinkedResource linkedResource in alternateView.LinkedResources)
							{
								linkedResource.PrepareForSending();
								mimeMultiPart4.Parts.Add(linkedResource.MimePart);
							}
							mimeMultiPart3.Parts.Add(mimeMultiPart4);
						}
						else
						{
							mimeMultiPart3.Parts.Add(alternateView.MimePart);
						}
					}
				}
				if (this.Attachments.Count > 0)
				{
					mimeMultiPart2 = new MimeMultiPart(MimeMultiPartType.Mixed);
					mimeMultiPart2.Parts.Add(mimeMultiPart3);
					MimeMultiPart mimeMultiPart5 = new MimeMultiPart(MimeMultiPartType.Mixed);
					foreach (Attachment attachment2 in this.Attachments)
					{
						if (attachment2 != null)
						{
							attachment2.PrepareForSending();
							mimeMultiPart5.Parts.Add(attachment2.MimePart);
						}
					}
					mimeMultiPart2.Parts.Add(mimeMultiPart5);
					this.message.Content = mimeMultiPart2;
					return;
				}
				if (mimeMultiPart3.Parts.Count == 1 && (this.body == null || this.body == string.Empty))
				{
					this.message.Content = mimeMultiPart3.Parts[0];
					return;
				}
				this.message.Content = mimeMultiPart3;
			}
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000DE8C4 File Offset: 0x000DD8C4
		internal void Send(BaseWriter writer, bool sendEnvelope)
		{
			this.SetContent();
			this.message.Send(writer, sendEnvelope);
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000DE8D9 File Offset: 0x000DD8D9
		internal IAsyncResult BeginSend(BaseWriter writer, bool sendEnvelope, AsyncCallback callback, object state)
		{
			this.SetContent();
			return this.message.BeginSend(writer, sendEnvelope, callback, state);
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x000DE8F1 File Offset: 0x000DD8F1
		internal void EndSend(IAsyncResult asyncResult)
		{
			this.message.EndSend(asyncResult);
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000DE900 File Offset: 0x000DD900
		internal string BuildDeliveryStatusNotificationString()
		{
			if (this.deliveryStatusNotification == DeliveryNotificationOptions.None)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(" NOTIFY=");
			bool flag = false;
			if (this.deliveryStatusNotification == DeliveryNotificationOptions.Never)
			{
				stringBuilder.Append("NEVER");
				return stringBuilder.ToString();
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnSuccess) > DeliveryNotificationOptions.None)
			{
				stringBuilder.Append("SUCCESS");
				flag = true;
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.OnFailure) > DeliveryNotificationOptions.None)
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("FAILURE");
				flag = true;
			}
			if ((this.deliveryStatusNotification & DeliveryNotificationOptions.Delay) > DeliveryNotificationOptions.None)
			{
				if (flag)
				{
					stringBuilder.Append(",");
				}
				stringBuilder.Append("DELAY");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04003036 RID: 12342
		private AlternateViewCollection views;

		// Token: 0x04003037 RID: 12343
		private AttachmentCollection attachments;

		// Token: 0x04003038 RID: 12344
		private AlternateView bodyView;

		// Token: 0x04003039 RID: 12345
		private string body = string.Empty;

		// Token: 0x0400303A RID: 12346
		private Encoding bodyEncoding;

		// Token: 0x0400303B RID: 12347
		private bool isBodyHtml;

		// Token: 0x0400303C RID: 12348
		private bool disposed;

		// Token: 0x0400303D RID: 12349
		private Message message;

		// Token: 0x0400303E RID: 12350
		private DeliveryNotificationOptions deliveryStatusNotification;
	}
}
