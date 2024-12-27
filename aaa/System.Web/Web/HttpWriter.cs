using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x0200009F RID: 159
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpWriter : TextWriter
	{
		// Token: 0x0600080E RID: 2062 RVA: 0x00023810 File Offset: 0x00022810
		internal HttpWriter(HttpResponse response)
			: base(null)
		{
			this._response = response;
			this._stream = new HttpResponseStream(this);
			this._buffers = new ArrayList();
			this._lastBuffer = null;
			this._charBuffer = (char[])HttpWriter.s_Allocator.GetBuffer();
			this._charBufferLength = this._charBuffer.Length;
			this._charBufferFree = this._charBufferLength;
			this.UpdateResponseBuffering();
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0002387E File Offset: 0x0002287E
		internal ArrayList SubstElements
		{
			get
			{
				if (this._substElements == null)
				{
					this._substElements = new ArrayList();
					this._response.Context.Request.SetDynamicCompression(false);
				}
				return this._substElements;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x000238AF File Offset: 0x000228AF
		internal bool IgnoringFurtherWrites
		{
			get
			{
				return this._ignoringFurtherWrites;
			}
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000238B7 File Offset: 0x000228B7
		internal void IgnoreFurtherWrites()
		{
			this._ignoringFurtherWrites = true;
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x000238C0 File Offset: 0x000228C0
		internal void UpdateResponseBuffering()
		{
			this._responseBufferingOn = this._response.BufferOutput;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x000238D4 File Offset: 0x000228D4
		internal void UpdateResponseEncoding()
		{
			if (this._responseEncodingUpdated && this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._responseEncoding = this._response.ContentEncoding;
			this._responseEncoder = this._response.ContentEncoder;
			this._responseCodePage = this._responseEncoding.CodePage;
			this._responseCodePageIsAsciiCompat = CodePageUtils.IsAsciiCompatibleCodePage(this._responseCodePage);
			this._responseEncodingUpdated = true;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00023949 File Offset: 0x00022949
		public override Encoding Encoding
		{
			get
			{
				if (!this._responseEncodingUpdated)
				{
					this.UpdateResponseEncoding();
				}
				return this._responseEncoding;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0002395F File Offset: 0x0002295F
		internal Encoder Encoder
		{
			get
			{
				if (!this._responseEncodingUpdated)
				{
					this.UpdateResponseEncoding();
				}
				return this._responseEncoder;
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00023975 File Offset: 0x00022975
		private HttpBaseMemoryResponseBufferElement CreateNewMemoryBufferElement()
		{
			return new HttpResponseUnmanagedBufferElement();
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002397C File Offset: 0x0002297C
		internal void DisposeIntegratedBuffers()
		{
			if (this._buffers != null)
			{
				int count = this._buffers.Count;
				for (int i = 0; i < count; i++)
				{
					HttpBaseMemoryResponseBufferElement httpBaseMemoryResponseBufferElement = this._buffers[i] as HttpBaseMemoryResponseBufferElement;
					if (httpBaseMemoryResponseBufferElement != null)
					{
						httpBaseMemoryResponseBufferElement.Recycle();
					}
				}
				this._buffers = null;
			}
			this.ClearBuffers();
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x000239D1 File Offset: 0x000229D1
		internal void RecycleBuffers()
		{
			if (this._charBuffer != null)
			{
				HttpWriter.s_Allocator.ReuseBuffer(this._charBuffer);
				this._charBuffer = null;
			}
			this.RecycleBufferElements();
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x000239F8 File Offset: 0x000229F8
		internal void ClearSubstitutionBlocks()
		{
			this._substElements = null;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00023A04 File Offset: 0x00022A04
		private void RecycleBufferElements()
		{
			if (this._buffers != null)
			{
				int count = this._buffers.Count;
				for (int i = 0; i < count; i++)
				{
					HttpBaseMemoryResponseBufferElement httpBaseMemoryResponseBufferElement = this._buffers[i] as HttpBaseMemoryResponseBufferElement;
					if (httpBaseMemoryResponseBufferElement != null)
					{
						httpBaseMemoryResponseBufferElement.Recycle();
					}
				}
				this._buffers = null;
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00023A53 File Offset: 0x00022A53
		private void ClearCharBuffer()
		{
			this._charBufferFree = this._charBufferLength;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00023A64 File Offset: 0x00022A64
		private void FlushCharBuffer(bool flushEncoder)
		{
			int num = this._charBufferLength - this._charBufferFree;
			if (!this._responseEncodingUpdated)
			{
				this.UpdateResponseEncoding();
			}
			this._responseEncodingUsed = true;
			int maxByteCount = this._responseEncoding.GetMaxByteCount(num);
			if (maxByteCount <= 128 || !this._responseBufferingOn)
			{
				byte[] array = new byte[maxByteCount];
				int bytes = this._responseEncoder.GetBytes(this._charBuffer, 0, num, array, 0, flushEncoder);
				this.BufferData(array, 0, bytes, false);
			}
			else
			{
				int num2 = ((this._lastBuffer != null) ? this._lastBuffer.FreeBytes : 0);
				if (num2 < maxByteCount)
				{
					this._lastBuffer = this.CreateNewMemoryBufferElement();
					this._buffers.Add(this._lastBuffer);
					num2 = this._lastBuffer.FreeBytes;
				}
				this._lastBuffer.AppendEncodedChars(this._charBuffer, 0, num, this._responseEncoder, flushEncoder);
			}
			this._charBufferFree = this._charBufferLength;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00023B4C File Offset: 0x00022B4C
		private void BufferData(byte[] data, int offset, int size, bool needToCopyData)
		{
			if (this._lastBuffer != null)
			{
				int num = this._lastBuffer.Append(data, offset, size);
				size -= num;
				offset += num;
			}
			else if (!needToCopyData && offset == 0 && !this._responseBufferingOn)
			{
				this._buffers.Add(new HttpResponseBufferElement(data, size));
				return;
			}
			while (size > 0)
			{
				this._lastBuffer = this.CreateNewMemoryBufferElement();
				this._buffers.Add(this._lastBuffer);
				int num = this._lastBuffer.Append(data, offset, size);
				offset += num;
				size -= num;
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00023BDC File Offset: 0x00022BDC
		private void BufferResource(IntPtr data, int offset, int size)
		{
			if (size > 4096 || !this._responseBufferingOn)
			{
				this._lastBuffer = null;
				this._buffers.Add(new HttpResourceResponseElement(data, offset, size));
				return;
			}
			if (this._lastBuffer != null)
			{
				int num = this._lastBuffer.Append(data, offset, size);
				size -= num;
				offset += num;
			}
			while (size > 0)
			{
				this._lastBuffer = this.CreateNewMemoryBufferElement();
				this._buffers.Add(this._lastBuffer);
				int num = this._lastBuffer.Append(data, offset, size);
				offset += num;
				size -= num;
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00023C73 File Offset: 0x00022C73
		internal void WriteFromStream(byte[] data, int offset, int size)
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this.BufferData(data, offset, size, true);
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00023CA8 File Offset: 0x00022CA8
		internal void WriteUTF8ResourceString(IntPtr pv, int offset, int size, bool asciiOnly)
		{
			if (!this._responseEncodingUpdated)
			{
				this.UpdateResponseEncoding();
			}
			if (this._responseCodePage == 65001 || (asciiOnly && this._responseCodePageIsAsciiCompat))
			{
				this._responseEncodingUsed = true;
				if (this._charBufferLength != this._charBufferFree)
				{
					this.FlushCharBuffer(true);
				}
				this.BufferResource(pv, offset, size);
				if (!this._responseBufferingOn)
				{
					this._response.Flush();
					return;
				}
			}
			else
			{
				this.Write(StringResourceManager.ResourceToString(pv, offset, size));
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00023D24 File Offset: 0x00022D24
		internal void TransmitFile(string filename, long offset, long size, bool isImpersonating, bool supportsLongTransmitFile)
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._lastBuffer = null;
			this._buffers.Add(new HttpFileResponseElement(filename, offset, size, isImpersonating, supportsLongTransmitFile));
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00023D78 File Offset: 0x00022D78
		internal void WriteFile(string filename, long offset, long size)
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._lastBuffer = null;
			this._buffers.Add(new HttpFileResponseElement(filename, offset, size));
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00023DC8 File Offset: 0x00022DC8
		internal void WriteSubstBlock(HttpResponseSubstitutionCallback callback, IIS7WorkerRequest iis7WorkerRequest)
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._lastBuffer = null;
			IHttpResponseElement httpResponseElement = new HttpSubstBlockResponseElement(callback, this.Encoding, this.Encoder, iis7WorkerRequest);
			this._buffers.Add(httpResponseElement);
			if (iis7WorkerRequest != null)
			{
				this.SubstElements.Add(httpResponseElement);
			}
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00023E35 File Offset: 0x00022E35
		// (set) Token: 0x06000825 RID: 2085 RVA: 0x00023E3D File Offset: 0x00022E3D
		internal bool HasBeenClearedRecently
		{
			get
			{
				return this._hasBeenClearedRecently;
			}
			set
			{
				this._hasBeenClearedRecently = value;
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00023E46 File Offset: 0x00022E46
		internal int GetResponseBufferCountAfterFlush()
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._lastBuffer = null;
			return this._buffers.Count;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00023E70 File Offset: 0x00022E70
		internal void MoveResponseBufferRangeForward(int srcIndex, int srcCount, int dstIndex)
		{
			if (srcCount > 0)
			{
				object[] array = new object[srcIndex - dstIndex];
				this._buffers.CopyTo(dstIndex, array, 0, array.Length);
				for (int i = 0; i < srcCount; i++)
				{
					this._buffers[dstIndex + i] = this._buffers[srcIndex + i];
				}
				for (int j = 0; j < array.Length; j++)
				{
					this._buffers[dstIndex + srcCount + j] = array[j];
				}
			}
			HttpBaseMemoryResponseBufferElement httpBaseMemoryResponseBufferElement = this._buffers[this._buffers.Count - 1] as HttpBaseMemoryResponseBufferElement;
			if (httpBaseMemoryResponseBufferElement != null && httpBaseMemoryResponseBufferElement.FreeBytes > 0)
			{
				this._lastBuffer = httpBaseMemoryResponseBufferElement;
				return;
			}
			this._lastBuffer = null;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00023F20 File Offset: 0x00022F20
		internal void ClearBuffers()
		{
			this.ClearCharBuffer();
			if (this._substElements != null)
			{
				this._response.Context.Request.SetDynamicCompression(true);
			}
			this.RecycleBufferElements();
			this._buffers = new ArrayList();
			this._lastBuffer = null;
			this._hasBeenClearedRecently = true;
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00023F70 File Offset: 0x00022F70
		internal long GetBufferedLength()
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			long num = 0L;
			if (this._buffers != null)
			{
				int count = this._buffers.Count;
				for (int i = 0; i < count; i++)
				{
					num += ((IHttpResponseElement)this._buffers[i]).GetSize();
				}
			}
			return num;
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00023FCF File Offset: 0x00022FCF
		internal bool ResponseEncodingUsed
		{
			get
			{
				return this._responseEncodingUsed;
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00023FD8 File Offset: 0x00022FD8
		internal ArrayList GetIntegratedSnapshot(out bool hasSubstBlocks, IIS7WorkerRequest wr)
		{
			ArrayList snapshot = this.GetSnapshot(out hasSubstBlocks);
			ArrayList bufferedResponseChunks = wr.GetBufferedResponseChunks(true, this._substElements, ref hasSubstBlocks);
			ArrayList arrayList;
			if (bufferedResponseChunks != null)
			{
				for (int i = 0; i < snapshot.Count; i++)
				{
					bufferedResponseChunks.Add(snapshot[i]);
				}
				arrayList = bufferedResponseChunks;
			}
			else
			{
				arrayList = snapshot;
			}
			if (this._substElements != null && this._substElements.Count > 0)
			{
				int num = 0;
				for (int j = 0; j < arrayList.Count; j++)
				{
					if (arrayList[j] is HttpSubstBlockResponseElement)
					{
						num++;
						if (num == this._substElements.Count)
						{
							break;
						}
					}
				}
				if (num != this._substElements.Count)
				{
					throw new InvalidOperationException(SR.GetString("Substitution_blocks_cannot_be_modified"));
				}
				this._response.Context.Request.SetDynamicCompression(true);
			}
			return arrayList;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000240B0 File Offset: 0x000230B0
		internal ArrayList GetSnapshot(out bool hasSubstBlocks)
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._lastBuffer = null;
			hasSubstBlocks = false;
			ArrayList arrayList = new ArrayList();
			int count = this._buffers.Count;
			for (int i = 0; i < count; i++)
			{
				object obj = this._buffers[i];
				HttpBaseMemoryResponseBufferElement httpBaseMemoryResponseBufferElement = obj as HttpBaseMemoryResponseBufferElement;
				if (httpBaseMemoryResponseBufferElement != null)
				{
					if (httpBaseMemoryResponseBufferElement.FreeBytes > 4096)
					{
						obj = httpBaseMemoryResponseBufferElement.Clone();
					}
					else
					{
						httpBaseMemoryResponseBufferElement.DisableRecycling();
					}
				}
				else if (obj is HttpSubstBlockResponseElement)
				{
					hasSubstBlocks = true;
				}
				arrayList.Add(obj);
			}
			return arrayList;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00024148 File Offset: 0x00023148
		internal void UseSnapshot(ArrayList buffers)
		{
			this.ClearBuffers();
			int count = buffers.Count;
			for (int i = 0; i < count; i++)
			{
				object obj = buffers[i];
				HttpSubstBlockResponseElement httpSubstBlockResponseElement = obj as HttpSubstBlockResponseElement;
				if (httpSubstBlockResponseElement != null)
				{
					this._buffers.Add(httpSubstBlockResponseElement.Substitute(this.Encoding));
				}
				else
				{
					this._buffers.Add(obj);
				}
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000241A7 File Offset: 0x000231A7
		internal Stream GetCurrentFilter()
		{
			if (this._installedFilter != null)
			{
				return this._installedFilter;
			}
			if (this._filterSink == null)
			{
				this._filterSink = new HttpResponseStreamFilterSink(this);
			}
			return this._filterSink;
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x000241D2 File Offset: 0x000231D2
		internal bool FilterInstalled
		{
			get
			{
				return this._installedFilter != null;
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000241E0 File Offset: 0x000231E0
		internal void InstallFilter(Stream filter)
		{
			if (this._filterSink == null)
			{
				throw new HttpException(SR.GetString("Invalid_response_filter"));
			}
			this._installedFilter = filter;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00024204 File Offset: 0x00023204
		internal void Filter(bool finalFiltering)
		{
			if (this._installedFilter == null)
			{
				return;
			}
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._lastBuffer = null;
			if (this._buffers.Count == 0)
			{
				return;
			}
			ArrayList buffers = this._buffers;
			this._buffers = new ArrayList();
			this._filterSink.Filtering = true;
			try
			{
				int count = buffers.Count;
				for (int i = 0; i < count; i++)
				{
					IHttpResponseElement httpResponseElement = (IHttpResponseElement)buffers[i];
					long size = httpResponseElement.GetSize();
					if (size > 0L)
					{
						this._installedFilter.Write(httpResponseElement.GetBytes(), 0, Convert.ToInt32(size));
					}
				}
				this._installedFilter.Flush();
			}
			finally
			{
				try
				{
					if (finalFiltering)
					{
						this._installedFilter.Close();
					}
				}
				finally
				{
					this._filterSink.Filtering = false;
				}
			}
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x000242F0 File Offset: 0x000232F0
		internal void FilterIntegrated(bool finalFiltering, IIS7WorkerRequest wr)
		{
			if (this._installedFilter == null)
			{
				return;
			}
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			this._lastBuffer = null;
			ArrayList buffers = this._buffers;
			this._buffers = new ArrayList();
			bool flag = false;
			ArrayList bufferedResponseChunks = wr.GetBufferedResponseChunks(false, null, ref flag);
			this._filterSink.Filtering = true;
			try
			{
				if (bufferedResponseChunks != null)
				{
					for (int i = 0; i < bufferedResponseChunks.Count; i++)
					{
						IHttpResponseElement httpResponseElement = (IHttpResponseElement)bufferedResponseChunks[i];
						long size = httpResponseElement.GetSize();
						if (size > 0L)
						{
							this._installedFilter.Write(httpResponseElement.GetBytes(), 0, Convert.ToInt32(size));
						}
					}
					wr.ClearResponse(true, false);
				}
				if (buffers != null)
				{
					for (int j = 0; j < buffers.Count; j++)
					{
						IHttpResponseElement httpResponseElement2 = (IHttpResponseElement)buffers[j];
						long size2 = httpResponseElement2.GetSize();
						if (size2 > 0L)
						{
							this._installedFilter.Write(httpResponseElement2.GetBytes(), 0, Convert.ToInt32(size2));
						}
					}
				}
				this._installedFilter.Flush();
			}
			finally
			{
				try
				{
					if (finalFiltering)
					{
						this._installedFilter.Close();
					}
				}
				finally
				{
					this._filterSink.Filtering = false;
				}
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002443C File Offset: 0x0002343C
		internal void Send(HttpWorkerRequest wr)
		{
			if (this._charBufferLength != this._charBufferFree)
			{
				this.FlushCharBuffer(true);
			}
			int count = this._buffers.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					((IHttpResponseElement)this._buffers[i]).Send(wr);
				}
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00024491 File Offset: 0x00023491
		public override void Close()
		{
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00024493 File Offset: 0x00023493
		public override void Flush()
		{
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00024498 File Offset: 0x00023498
		public override void Write(char ch)
		{
			if (this._ignoringFurtherWrites)
			{
				return;
			}
			if (this._charBufferFree == 0)
			{
				this.FlushCharBuffer(false);
			}
			this._charBuffer[this._charBufferLength - this._charBufferFree] = ch;
			this._charBufferFree--;
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000244F4 File Offset: 0x000234F4
		public override void Write(char[] buffer, int index, int count)
		{
			if (this._ignoringFurtherWrites)
			{
				return;
			}
			if (buffer == null || index < 0 || count < 0 || buffer.Length - index < count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count == 0)
			{
				return;
			}
			while (count > 0)
			{
				if (this._charBufferFree == 0)
				{
					this.FlushCharBuffer(false);
				}
				int num = ((count < this._charBufferFree) ? count : this._charBufferFree);
				Array.Copy(buffer, index, this._charBuffer, this._charBufferLength - this._charBufferFree, num);
				this._charBufferFree -= num;
				index += num;
				count -= num;
			}
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00024598 File Offset: 0x00023598
		public override void Write(string s)
		{
			if (this._ignoringFurtherWrites)
			{
				return;
			}
			if (s == null)
			{
				return;
			}
			if (s.Length != 0)
			{
				if (s.Length < this._charBufferFree)
				{
					StringUtil.UnsafeStringCopy(s, 0, this._charBuffer, this._charBufferLength - this._charBufferFree, s.Length);
					this._charBufferFree -= s.Length;
				}
				else
				{
					int i = s.Length;
					int num = 0;
					while (i > 0)
					{
						if (this._charBufferFree == 0)
						{
							this.FlushCharBuffer(false);
						}
						int num2 = ((i < this._charBufferFree) ? i : this._charBufferFree);
						StringUtil.UnsafeStringCopy(s, num, this._charBuffer, this._charBufferLength - this._charBufferFree, num2);
						this._charBufferFree -= num2;
						num += num2;
						i -= num2;
					}
				}
			}
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00024678 File Offset: 0x00023678
		public void WriteString(string s, int index, int count)
		{
			if (s == null)
			{
				return;
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index + count > s.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (this._ignoringFurtherWrites)
			{
				return;
			}
			if (count != 0)
			{
				if (count < this._charBufferFree)
				{
					StringUtil.UnsafeStringCopy(s, index, this._charBuffer, this._charBufferLength - this._charBufferFree, count);
					this._charBufferFree -= count;
				}
				else
				{
					while (count > 0)
					{
						if (this._charBufferFree == 0)
						{
							this.FlushCharBuffer(false);
						}
						int num = ((count < this._charBufferFree) ? count : this._charBufferFree);
						StringUtil.UnsafeStringCopy(s, index, this._charBuffer, this._charBufferLength - this._charBufferFree, num);
						this._charBufferFree -= num;
						index += num;
						count -= num;
					}
				}
			}
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0002476F File Offset: 0x0002376F
		public override void Write(object obj)
		{
			if (this._ignoringFurtherWrites)
			{
				return;
			}
			if (obj != null)
			{
				this.Write(obj.ToString());
			}
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00024789 File Offset: 0x00023789
		public void WriteBytes(byte[] buffer, int index, int count)
		{
			if (this._ignoringFurtherWrites)
			{
				return;
			}
			this.WriteFromStream(buffer, index, count);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000247A0 File Offset: 0x000237A0
		public override void WriteLine()
		{
			if (this._ignoringFurtherWrites)
			{
				return;
			}
			if (this._charBufferFree < 2)
			{
				this.FlushCharBuffer(false);
			}
			int num = this._charBufferLength - this._charBufferFree;
			this._charBuffer[num] = '\r';
			this._charBuffer[num + 1] = '\n';
			this._charBufferFree -= 2;
			if (!this._responseBufferingOn)
			{
				this._response.Flush();
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0002480B File Offset: 0x0002380B
		public Stream OutputStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x04001184 RID: 4484
		private HttpResponse _response;

		// Token: 0x04001185 RID: 4485
		private HttpResponseStream _stream;

		// Token: 0x04001186 RID: 4486
		private HttpResponseStreamFilterSink _filterSink;

		// Token: 0x04001187 RID: 4487
		private Stream _installedFilter;

		// Token: 0x04001188 RID: 4488
		private HttpBaseMemoryResponseBufferElement _lastBuffer;

		// Token: 0x04001189 RID: 4489
		private ArrayList _buffers;

		// Token: 0x0400118A RID: 4490
		private char[] _charBuffer;

		// Token: 0x0400118B RID: 4491
		private int _charBufferLength;

		// Token: 0x0400118C RID: 4492
		private int _charBufferFree;

		// Token: 0x0400118D RID: 4493
		private ArrayList _substElements;

		// Token: 0x0400118E RID: 4494
		private static CharBufferAllocator s_Allocator = new CharBufferAllocator(1024, 64);

		// Token: 0x0400118F RID: 4495
		private bool _responseBufferingOn;

		// Token: 0x04001190 RID: 4496
		private Encoding _responseEncoding;

		// Token: 0x04001191 RID: 4497
		private bool _responseEncodingUsed;

		// Token: 0x04001192 RID: 4498
		private bool _responseEncodingUpdated;

		// Token: 0x04001193 RID: 4499
		private Encoder _responseEncoder;

		// Token: 0x04001194 RID: 4500
		private int _responseCodePage;

		// Token: 0x04001195 RID: 4501
		private bool _responseCodePageIsAsciiCompat;

		// Token: 0x04001196 RID: 4502
		private bool _ignoringFurtherWrites;

		// Token: 0x04001197 RID: 4503
		private bool _hasBeenClearedRecently;
	}
}
