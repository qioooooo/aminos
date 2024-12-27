using System;
using System.Text;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200009C RID: 156
	internal sealed class HttpSubstBlockResponseElement : IHttpResponseElement
	{
		// Token: 0x060007F3 RID: 2035 RVA: 0x000234E4 File Offset: 0x000224E4
		internal HttpSubstBlockResponseElement(HttpResponseSubstitutionCallback callback, Encoding encoding, Encoder encoder, IIS7WorkerRequest iis7WorkerRequest)
		{
			this._callback = callback;
			if (iis7WorkerRequest == null)
			{
				this._firstSubstitution = this.Substitute(encoding);
				return;
			}
			this._isIIS7WorkerRequest = true;
			string text = this._callback(HttpContext.Current);
			if (text == null)
			{
				throw new ArgumentNullException("substitutionString");
			}
			this.CreateFirstSubstData(text, iis7WorkerRequest, encoder);
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00023540 File Offset: 0x00022540
		private unsafe void CreateFirstSubstData(string s, IIS7WorkerRequest iis7WorkerRequest, Encoder encoder)
		{
			int num = 0;
			int length = s.Length;
			IntPtr intPtr;
			if (length > 0)
			{
				fixed (char* ptr = s)
				{
					int byteCount = encoder.GetByteCount(ptr, length, true);
					intPtr = iis7WorkerRequest.AllocateRequestMemory(byteCount);
					if (intPtr != IntPtr.Zero)
					{
						num = encoder.GetBytes(ptr, length, (byte*)(void*)intPtr, byteCount, true);
					}
				}
			}
			else
			{
				intPtr = iis7WorkerRequest.AllocateRequestMemory(1);
			}
			if (intPtr == IntPtr.Zero)
			{
				throw new OutOfMemoryException();
			}
			this._firstSubstData = intPtr;
			this._firstSubstDataSize = num;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x000235CC File Offset: 0x000225CC
		internal IHttpResponseElement Substitute(Encoding e)
		{
			string text = this._callback(HttpContext.Current);
			byte[] bytes = e.GetBytes(text);
			return new HttpResponseBufferElement(bytes, bytes.Length);
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x000235FB File Offset: 0x000225FB
		internal bool PointerEquals(IntPtr ptr)
		{
			return this._firstSubstData == ptr;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00023609 File Offset: 0x00022609
		long IHttpResponseElement.GetSize()
		{
			if (this._isIIS7WorkerRequest)
			{
				return (long)this._firstSubstDataSize;
			}
			return this._firstSubstitution.GetSize();
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x00023628 File Offset: 0x00022628
		byte[] IHttpResponseElement.GetBytes()
		{
			if (!this._isIIS7WorkerRequest)
			{
				return this._firstSubstitution.GetBytes();
			}
			if (this._firstSubstDataSize > 0)
			{
				byte[] array = new byte[this._firstSubstDataSize];
				Misc.CopyMemory(this._firstSubstData, 0, array, 0, this._firstSubstDataSize);
				return array;
			}
			if (!(this._firstSubstData == IntPtr.Zero))
			{
				return new byte[0];
			}
			return null;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x00023690 File Offset: 0x00022690
		void IHttpResponseElement.Send(HttpWorkerRequest wr)
		{
			if (this._isIIS7WorkerRequest)
			{
				IIS7WorkerRequest iis7WorkerRequest = wr as IIS7WorkerRequest;
				if (iis7WorkerRequest != null)
				{
					iis7WorkerRequest.SendResponseFromIISAllocatedRequestMemory(this._firstSubstData, this._firstSubstDataSize);
					return;
				}
			}
			else
			{
				this._firstSubstitution.Send(wr);
			}
		}

		// Token: 0x0400117D RID: 4477
		private HttpResponseSubstitutionCallback _callback;

		// Token: 0x0400117E RID: 4478
		private IHttpResponseElement _firstSubstitution;

		// Token: 0x0400117F RID: 4479
		private IntPtr _firstSubstData;

		// Token: 0x04001180 RID: 4480
		private int _firstSubstDataSize;

		// Token: 0x04001181 RID: 4481
		private bool _isIIS7WorkerRequest;
	}
}
