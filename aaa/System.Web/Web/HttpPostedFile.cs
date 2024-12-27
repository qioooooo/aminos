using System;
using System.IO;
using System.Security.Permissions;
using System.Web.Configuration;

namespace System.Web
{
	// Token: 0x0200007E RID: 126
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpPostedFile
	{
		// Token: 0x06000540 RID: 1344 RVA: 0x000154E4 File Offset: 0x000144E4
		internal HttpPostedFile(string filename, string contentType, HttpInputStream stream)
		{
			this._filename = filename;
			this._contentType = contentType;
			this._stream = stream;
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00015501 File Offset: 0x00014501
		public string FileName
		{
			get
			{
				return this._filename;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x00015509 File Offset: 0x00014509
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00015511 File Offset: 0x00014511
		public int ContentLength
		{
			get
			{
				return (int)this._stream.Length;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001551F File Offset: 0x0001451F
		public Stream InputStream
		{
			get
			{
				return this._stream;
			}
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00015528 File Offset: 0x00014528
		public void SaveAs(string filename)
		{
			if (!Path.IsPathRooted(filename))
			{
				HttpRuntimeSection httpRuntime = RuntimeConfig.GetConfig().HttpRuntime;
				if (httpRuntime.RequireRootedSaveAsPath)
				{
					throw new HttpException(SR.GetString("SaveAs_requires_rooted_path", new object[] { filename }));
				}
			}
			FileStream fileStream = new FileStream(filename, FileMode.Create);
			try
			{
				this._stream.WriteTo(fileStream);
				fileStream.Flush();
			}
			finally
			{
				fileStream.Close();
			}
		}

		// Token: 0x0400105B RID: 4187
		private string _filename;

		// Token: 0x0400105C RID: 4188
		private string _contentType;

		// Token: 0x0400105D RID: 4189
		private HttpInputStream _stream;
	}
}
