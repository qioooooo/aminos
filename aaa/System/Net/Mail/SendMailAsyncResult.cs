using System;
using System.Collections;
using System.IO;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x020006DD RID: 1757
	internal class SendMailAsyncResult : LazyAsyncResult
	{
		// Token: 0x0600362B RID: 13867 RVA: 0x000E72D8 File Offset: 0x000E62D8
		internal SendMailAsyncResult(SmtpConnection connection, string from, MailAddressCollection toCollection, string deliveryNotify, AsyncCallback callback, object state)
			: base(null, state, callback)
		{
			this.toCollection = toCollection;
			this.connection = connection;
			this.from = from;
			this.deliveryNotify = deliveryNotify;
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x000E730D File Offset: 0x000E630D
		internal void Send()
		{
			this.SendMailFrom();
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000E7318 File Offset: 0x000E6318
		internal static MailWriter End(IAsyncResult result)
		{
			SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result;
			object obj = sendMailAsyncResult.InternalWaitForCompletion();
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return new MailWriter(sendMailAsyncResult.stream);
		}

		// Token: 0x0600362E RID: 13870 RVA: 0x000E7350 File Offset: 0x000E6350
		private void SendMailFrom()
		{
			IAsyncResult asyncResult = MailCommand.BeginSend(this.connection, SmtpCommands.Mail, this.from, SendMailAsyncResult.sendMailFromCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			MailCommand.EndSend(asyncResult);
			this.SendTo();
		}

		// Token: 0x0600362F RID: 13871 RVA: 0x000E7390 File Offset: 0x000E6390
		private static void SendMailFromCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					MailCommand.EndSend(result);
					sendMailAsyncResult.SendTo();
				}
				catch (Exception ex)
				{
					sendMailAsyncResult.InvokeCallback(ex);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003630 RID: 13872 RVA: 0x000E7400 File Offset: 0x000E6400
		private void SendTo()
		{
			if (this.to == null)
			{
				if (this.SendToCollection())
				{
					this.SendData();
				}
				return;
			}
			IAsyncResult asyncResult = RecipientCommand.BeginSend(this.connection, (this.deliveryNotify != null) ? (this.to + this.deliveryNotify) : this.to, SendMailAsyncResult.sendToCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			string text;
			if (!RecipientCommand.EndSend(asyncResult, out text))
			{
				throw new SmtpFailedRecipientException(this.connection.Reader.StatusCode, this.to, text);
			}
			this.SendData();
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000E7490 File Offset: 0x000E6490
		private static void SendToCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					string text;
					if (RecipientCommand.EndSend(result, out text))
					{
						sendMailAsyncResult.SendData();
					}
					else
					{
						sendMailAsyncResult.InvokeCallback(new SmtpFailedRecipientException(sendMailAsyncResult.connection.Reader.StatusCode, sendMailAsyncResult.to, text));
					}
				}
				catch (Exception ex)
				{
					sendMailAsyncResult.InvokeCallback(ex);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000E7528 File Offset: 0x000E6528
		private bool SendToCollection()
		{
			while (this.toIndex < this.toCollection.Count)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)RecipientCommand.BeginSend(this.connection, this.toCollection[this.toIndex++].SmtpAddress + this.deliveryNotify, SendMailAsyncResult.sendToCollectionCompleted, this);
				if (!multiAsyncResult.CompletedSynchronously)
				{
					return false;
				}
				string text;
				if (!RecipientCommand.EndSend(multiAsyncResult, out text))
				{
					this.failedRecipientExceptions.Add(new SmtpFailedRecipientException(this.connection.Reader.StatusCode, this.toCollection[this.toIndex - 1].SmtpAddress, text));
				}
			}
			return true;
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000E75E4 File Offset: 0x000E65E4
		private static void SendToCollectionCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					string text;
					if (!RecipientCommand.EndSend(result, out text))
					{
						sendMailAsyncResult.failedRecipientExceptions.Add(new SmtpFailedRecipientException(sendMailAsyncResult.connection.Reader.StatusCode, sendMailAsyncResult.toCollection[sendMailAsyncResult.toIndex - 1].SmtpAddress, text));
						if (sendMailAsyncResult.failedRecipientExceptions.Count == sendMailAsyncResult.toCollection.Count)
						{
							SmtpFailedRecipientException ex;
							if (sendMailAsyncResult.toCollection.Count == 1)
							{
								ex = (SmtpFailedRecipientException)sendMailAsyncResult.failedRecipientExceptions[0];
							}
							else
							{
								ex = new SmtpFailedRecipientsException(sendMailAsyncResult.failedRecipientExceptions, true);
							}
							ex.fatal = true;
							sendMailAsyncResult.InvokeCallback(ex);
							return;
						}
					}
					if (sendMailAsyncResult.SendToCollection())
					{
						sendMailAsyncResult.SendData();
					}
				}
				catch (Exception ex2)
				{
					sendMailAsyncResult.InvokeCallback(ex2);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000E76F8 File Offset: 0x000E66F8
		private void SendData()
		{
			IAsyncResult asyncResult = DataCommand.BeginSend(this.connection, SendMailAsyncResult.sendDataCompleted, this);
			if (!asyncResult.CompletedSynchronously)
			{
				return;
			}
			DataCommand.EndSend(asyncResult);
			this.stream = this.connection.GetClosableStream();
			if (this.failedRecipientExceptions.Count > 1)
			{
				base.InvokeCallback(new SmtpFailedRecipientsException(this.failedRecipientExceptions, this.failedRecipientExceptions.Count == this.toCollection.Count));
				return;
			}
			if (this.failedRecipientExceptions.Count == 1)
			{
				base.InvokeCallback(this.failedRecipientExceptions[0]);
				return;
			}
			base.InvokeCallback();
		}

		// Token: 0x06003635 RID: 13877 RVA: 0x000E7798 File Offset: 0x000E6798
		private static void SendDataCompleted(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				SendMailAsyncResult sendMailAsyncResult = (SendMailAsyncResult)result.AsyncState;
				try
				{
					DataCommand.EndSend(result);
					sendMailAsyncResult.stream = sendMailAsyncResult.connection.GetClosableStream();
					if (sendMailAsyncResult.failedRecipientExceptions.Count > 1)
					{
						sendMailAsyncResult.InvokeCallback(new SmtpFailedRecipientsException(sendMailAsyncResult.failedRecipientExceptions, sendMailAsyncResult.failedRecipientExceptions.Count == sendMailAsyncResult.toCollection.Count));
					}
					else if (sendMailAsyncResult.failedRecipientExceptions.Count == 1)
					{
						sendMailAsyncResult.InvokeCallback(sendMailAsyncResult.failedRecipientExceptions[0]);
					}
					else
					{
						sendMailAsyncResult.InvokeCallback();
					}
				}
				catch (Exception ex)
				{
					sendMailAsyncResult.InvokeCallback(ex);
				}
				catch
				{
					sendMailAsyncResult.InvokeCallback(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x0400315D RID: 12637
		private SmtpConnection connection;

		// Token: 0x0400315E RID: 12638
		private string from;

		// Token: 0x0400315F RID: 12639
		private string deliveryNotify;

		// Token: 0x04003160 RID: 12640
		private static AsyncCallback sendMailFromCompleted = new AsyncCallback(SendMailAsyncResult.SendMailFromCompleted);

		// Token: 0x04003161 RID: 12641
		private static AsyncCallback sendToCompleted = new AsyncCallback(SendMailAsyncResult.SendToCompleted);

		// Token: 0x04003162 RID: 12642
		private static AsyncCallback sendToCollectionCompleted = new AsyncCallback(SendMailAsyncResult.SendToCollectionCompleted);

		// Token: 0x04003163 RID: 12643
		private static AsyncCallback sendDataCompleted = new AsyncCallback(SendMailAsyncResult.SendDataCompleted);

		// Token: 0x04003164 RID: 12644
		private ArrayList failedRecipientExceptions = new ArrayList();

		// Token: 0x04003165 RID: 12645
		private Stream stream;

		// Token: 0x04003166 RID: 12646
		private string to;

		// Token: 0x04003167 RID: 12647
		private MailAddressCollection toCollection;

		// Token: 0x04003168 RID: 12648
		private int toIndex;
	}
}
