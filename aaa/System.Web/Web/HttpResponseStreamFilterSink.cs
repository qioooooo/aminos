using System;

namespace System.Web
{
	// Token: 0x0200009E RID: 158
	internal sealed class HttpResponseStreamFilterSink : HttpResponseStream
	{
		// Token: 0x06000807 RID: 2055 RVA: 0x000237BE File Offset: 0x000227BE
		internal HttpResponseStreamFilterSink(HttpWriter writer)
			: base(writer)
		{
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x000237C7 File Offset: 0x000227C7
		private void VerifyState()
		{
			if (!this._filtering)
			{
				throw new HttpException(SR.GetString("Invalid_use_of_response_filter"));
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x000237E1 File Offset: 0x000227E1
		// (set) Token: 0x0600080A RID: 2058 RVA: 0x000237E9 File Offset: 0x000227E9
		internal bool Filtering
		{
			get
			{
				return this._filtering;
			}
			set
			{
				this._filtering = value;
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000237F2 File Offset: 0x000227F2
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000237FB File Offset: 0x000227FB
		public override void Flush()
		{
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000237FD File Offset: 0x000227FD
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.VerifyState();
			base.Write(buffer, offset, count);
		}

		// Token: 0x04001183 RID: 4483
		private bool _filtering;
	}
}
