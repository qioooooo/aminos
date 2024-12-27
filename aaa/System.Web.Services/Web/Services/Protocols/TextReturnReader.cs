using System;
using System.IO;
using System.Net;
using System.Security.Permissions;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000086 RID: 134
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class TextReturnReader : MimeReturnReader
	{
		// Token: 0x06000390 RID: 912 RVA: 0x00011D6C File Offset: 0x00010D6C
		public override void Initialize(object o)
		{
			this.matcher = (PatternMatcher)o;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00011D7A File Offset: 0x00010D7A
		public override object GetInitializer(LogicalMethodInfo methodInfo)
		{
			return new PatternMatcher(methodInfo.ReturnType);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00011D88 File Offset: 0x00010D88
		public override object Read(WebResponse response, Stream responseStream)
		{
			object obj;
			try
			{
				string text = RequestResponseUtils.ReadResponse(response);
				obj = this.matcher.Match(text);
			}
			finally
			{
				response.Close();
			}
			return obj;
		}

		// Token: 0x0400038B RID: 907
		private PatternMatcher matcher;
	}
}
