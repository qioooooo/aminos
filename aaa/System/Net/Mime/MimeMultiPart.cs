using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x020006AB RID: 1707
	internal class MimeMultiPart : MimeBasePart
	{
		// Token: 0x060034B3 RID: 13491 RVA: 0x000DFBAD File Offset: 0x000DEBAD
		internal MimeMultiPart(MimeMultiPartType type)
		{
			this.MimeMultiPartType = type;
		}

		// Token: 0x17000C54 RID: 3156
		// (set) Token: 0x060034B4 RID: 13492 RVA: 0x000DFBBC File Offset: 0x000DEBBC
		internal MimeMultiPartType MimeMultiPartType
		{
			set
			{
				if (value > MimeMultiPartType.Related || value < MimeMultiPartType.Mixed)
				{
					throw new NotSupportedException(value.ToString());
				}
				this.SetType(value);
			}
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x000DFBDE File Offset: 0x000DEBDE
		private void SetType(MimeMultiPartType type)
		{
			base.ContentType.MediaType = "multipart/" + type.ToString().ToLower(CultureInfo.InvariantCulture);
			base.ContentType.Boundary = this.GetNextBoundary();
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x060034B6 RID: 13494 RVA: 0x000DFC1B File Offset: 0x000DEC1B
		internal Collection<MimeBasePart> Parts
		{
			get
			{
				if (this.parts == null)
				{
					this.parts = new Collection<MimeBasePart>();
				}
				return this.parts;
			}
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x000DFC38 File Offset: 0x000DEC38
		internal void Complete(IAsyncResult result, Exception e)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			if (mimePartContext.completed)
			{
				throw e;
			}
			try
			{
				mimePartContext.outputStream.Close();
			}
			catch (Exception ex)
			{
				if (e == null)
				{
					e = ex;
				}
			}
			catch
			{
				if (e == null)
				{
					e = new Exception(SR.GetString("net_nonClsCompliantException"));
				}
			}
			mimePartContext.completed = true;
			mimePartContext.result.InvokeCallback(e);
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x000DFCB8 File Offset: 0x000DECB8
		internal void MimeWriterCloseCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.MimeWriterCloseCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x000DFD28 File Offset: 0x000DED28
		private void MimeWriterCloseCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			((MimeWriter)mimePartContext.writer).EndClose(result);
			this.Complete(result, null);
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x000DFD5C File Offset: 0x000DED5C
		internal void MimePartSentCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.MimePartSentCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x000DFDCC File Offset: 0x000DEDCC
		private void MimePartSentCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			MimeBasePart mimeBasePart = mimePartContext.partsEnumerator.Current;
			mimeBasePart.EndSend(result);
			if (mimePartContext.partsEnumerator.MoveNext())
			{
				mimeBasePart = mimePartContext.partsEnumerator.Current;
				IAsyncResult asyncResult = mimeBasePart.BeginSend(mimePartContext.writer, this.mimePartSentCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext.writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x000DFE64 File Offset: 0x000DEE64
		internal void ContentStreamCallback(IAsyncResult result)
		{
			if (result.CompletedSynchronously)
			{
				return;
			}
			((MimeMultiPart.MimePartContext)result.AsyncState).completedSynchronously = false;
			try
			{
				this.ContentStreamCallbackHandler(result);
			}
			catch (Exception ex)
			{
				this.Complete(result, ex);
			}
			catch
			{
				this.Complete(result, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x000DFED4 File Offset: 0x000DEED4
		private void ContentStreamCallbackHandler(IAsyncResult result)
		{
			MimeMultiPart.MimePartContext mimePartContext = (MimeMultiPart.MimePartContext)result.AsyncState;
			mimePartContext.outputStream = mimePartContext.writer.EndGetContentStream(result);
			mimePartContext.writer = new MimeWriter(mimePartContext.outputStream, base.ContentType.Boundary);
			if (mimePartContext.partsEnumerator.MoveNext())
			{
				MimeBasePart mimeBasePart = mimePartContext.partsEnumerator.Current;
				this.mimePartSentCallback = new AsyncCallback(this.MimePartSentCallback);
				IAsyncResult asyncResult = mimeBasePart.BeginSend(mimePartContext.writer, this.mimePartSentCallback, mimePartContext);
				if (asyncResult.CompletedSynchronously)
				{
					this.MimePartSentCallbackHandler(asyncResult);
				}
				return;
			}
			IAsyncResult asyncResult2 = ((MimeWriter)mimePartContext.writer).BeginClose(new AsyncCallback(this.MimeWriterCloseCallback), mimePartContext);
			if (asyncResult2.CompletedSynchronously)
			{
				this.MimeWriterCloseCallbackHandler(asyncResult2);
			}
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x000DFF98 File Offset: 0x000DEF98
		internal override IAsyncResult BeginSend(BaseWriter writer, AsyncCallback callback, object state)
		{
			writer.WriteHeaders(base.Headers);
			MimeBasePart.MimePartAsyncResult mimePartAsyncResult = new MimeBasePart.MimePartAsyncResult(this, state, callback);
			MimeMultiPart.MimePartContext mimePartContext = new MimeMultiPart.MimePartContext(writer, mimePartAsyncResult, this.Parts.GetEnumerator());
			IAsyncResult asyncResult = writer.BeginGetContentStream(new AsyncCallback(this.ContentStreamCallback), mimePartContext);
			if (asyncResult.CompletedSynchronously)
			{
				this.ContentStreamCallbackHandler(asyncResult);
			}
			return mimePartAsyncResult;
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x000DFFF4 File Offset: 0x000DEFF4
		internal override void Send(BaseWriter writer)
		{
			writer.WriteHeaders(base.Headers);
			Stream contentStream = writer.GetContentStream();
			MimeWriter mimeWriter = new MimeWriter(contentStream, base.ContentType.Boundary);
			foreach (MimeBasePart mimeBasePart in this.Parts)
			{
				mimeBasePart.Send(mimeWriter);
			}
			mimeWriter.Close();
			contentStream.Close();
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x000E0074 File Offset: 0x000DF074
		internal string GetNextBoundary()
		{
			string text = "--boundary_" + MimeMultiPart.boundary.ToString(CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString(null, CultureInfo.InvariantCulture);
			MimeMultiPart.boundary++;
			return text;
		}

		// Token: 0x0400306A RID: 12394
		private Collection<MimeBasePart> parts;

		// Token: 0x0400306B RID: 12395
		private static int boundary;

		// Token: 0x0400306C RID: 12396
		private AsyncCallback mimePartSentCallback;

		// Token: 0x020006AC RID: 1708
		internal class MimePartContext
		{
			// Token: 0x060034C1 RID: 13505 RVA: 0x000E00C0 File Offset: 0x000DF0C0
			internal MimePartContext(BaseWriter writer, LazyAsyncResult result, IEnumerator<MimeBasePart> partsEnumerator)
			{
				this.writer = writer;
				this.result = result;
				this.partsEnumerator = partsEnumerator;
			}

			// Token: 0x0400306D RID: 12397
			internal IEnumerator<MimeBasePart> partsEnumerator;

			// Token: 0x0400306E RID: 12398
			internal Stream outputStream;

			// Token: 0x0400306F RID: 12399
			internal LazyAsyncResult result;

			// Token: 0x04003070 RID: 12400
			internal BaseWriter writer;

			// Token: 0x04003071 RID: 12401
			internal bool completed;

			// Token: 0x04003072 RID: 12402
			internal bool completedSynchronously = true;
		}
	}
}
