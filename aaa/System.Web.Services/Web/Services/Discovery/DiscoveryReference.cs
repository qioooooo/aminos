using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Services.Diagnostics;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x02000095 RID: 149
	public abstract class DiscoveryReference
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x000131D8 File Offset: 0x000121D8
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x000131E0 File Offset: 0x000121E0
		[XmlIgnore]
		public DiscoveryClientProtocol ClientProtocol
		{
			get
			{
				return this.clientProtocol;
			}
			set
			{
				this.clientProtocol = value;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060003DA RID: 986 RVA: 0x000131E9 File Offset: 0x000121E9
		[XmlIgnore]
		public virtual string DefaultFilename
		{
			get
			{
				return DiscoveryReference.FilenameFromUrl(this.Url);
			}
		}

		// Token: 0x060003DB RID: 987
		public abstract void WriteDocument(object document, Stream stream);

		// Token: 0x060003DC RID: 988
		public abstract object ReadDocument(Stream stream);

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060003DD RID: 989
		// (set) Token: 0x060003DE RID: 990
		[XmlIgnore]
		public abstract string Url { get; set; }

		// Token: 0x060003DF RID: 991 RVA: 0x000131F6 File Offset: 0x000121F6
		internal virtual void LoadExternals(Hashtable loadedExternals)
		{
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x000131F8 File Offset: 0x000121F8
		public static string FilenameFromUrl(string url)
		{
			int num = url.LastIndexOf('/', url.Length - 1);
			if (num >= 0)
			{
				url = url.Substring(num + 1);
			}
			int num2 = url.IndexOf('.');
			if (num2 >= 0)
			{
				url = url.Substring(0, num2);
			}
			int num3 = url.IndexOf('?');
			if (num3 >= 0)
			{
				url = url.Substring(0, num3);
			}
			if (url == null || url.Length == 0)
			{
				return "item";
			}
			return DiscoveryReference.MakeValidFilename(url);
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0001326C File Offset: 0x0001226C
		private static bool FindChar(char ch, char[] chars)
		{
			for (int i = 0; i < chars.Length; i++)
			{
				if (ch == chars[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00013290 File Offset: 0x00012290
		internal static string MakeValidFilename(string filename)
		{
			if (filename == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(filename.Length);
			foreach (char c in filename)
			{
				if (!DiscoveryReference.FindChar(c, Path.InvalidPathChars))
				{
					stringBuilder.Append(c);
				}
			}
			string text = stringBuilder.ToString();
			if (text.Length == 0)
			{
				text = "item";
			}
			return Path.GetFileName(text);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000132F8 File Offset: 0x000122F8
		public void Resolve()
		{
			if (this.ClientProtocol == null)
			{
				throw new InvalidOperationException(Res.GetString("WebResolveMissingClientProtocol"));
			}
			if (this.ClientProtocol.Documents[this.Url] != null)
			{
				return;
			}
			if (this.ClientProtocol.InlinedSchemas[this.Url] != null)
			{
				return;
			}
			string url = this.Url;
			string url2 = this.Url;
			string text = null;
			Stream stream = this.ClientProtocol.Download(ref url, ref text);
			if (this.ClientProtocol.Documents[url] != null)
			{
				this.Url = url;
				return;
			}
			try
			{
				this.Url = url;
				this.Resolve(text, stream);
			}
			catch
			{
				this.Url = url2;
				throw;
			}
			finally
			{
				stream.Close();
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000133CC File Offset: 0x000123CC
		internal Exception AttemptResolve(string contentType, Stream stream)
		{
			Exception ex;
			try
			{
				this.Resolve(contentType, stream);
				ex = null;
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "AttemptResolve", ex2);
				}
				ex = ex2;
			}
			catch
			{
				ex = new Exception(Res.GetString("NonClsCompliantException"));
			}
			return ex;
		}

		// Token: 0x060003E5 RID: 997
		protected internal abstract void Resolve(string contentType, Stream stream);

		// Token: 0x060003E6 RID: 998 RVA: 0x0001344C File Offset: 0x0001244C
		internal static string UriToString(string baseUrl, string relUrl)
		{
			return new Uri(new Uri(baseUrl), relUrl).GetComponents(UriComponents.AbsoluteUri, UriFormat.SafeUnescaped);
		}

		// Token: 0x04000398 RID: 920
		private DiscoveryClientProtocol clientProtocol;
	}
}
