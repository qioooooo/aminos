using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Services.Diagnostics;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000093 RID: 147
	public class XmlReturnReader : MimeReturnReader
	{
		// Token: 0x060003CE RID: 974 RVA: 0x00012FFE File Offset: 0x00011FFE
		public override void Initialize(object o)
		{
			this.xmlSerializer = (XmlSerializer)o;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001300C File Offset: 0x0001200C
		public override object[] GetInitializers(LogicalMethodInfo[] methodInfos)
		{
			return XmlReturn.GetInitializers(methodInfos);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00013014 File Offset: 0x00012014
		public override object GetInitializer(LogicalMethodInfo methodInfo)
		{
			return XmlReturn.GetInitializer(methodInfo);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001301C File Offset: 0x0001201C
		public override object Read(WebResponse response, Stream responseStream)
		{
			object obj2;
			try
			{
				if (response == null)
				{
					throw new ArgumentNullException("response");
				}
				if (!ContentType.MatchesBase(response.ContentType, "text/xml"))
				{
					throw new InvalidOperationException(Res.GetString("WebResultNotXml"));
				}
				Encoding encoding = RequestResponseUtils.GetEncoding(response.ContentType);
				StreamReader streamReader = new StreamReader(responseStream, encoding, true);
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "Read", new object[0]) : null);
				if (Tracing.On)
				{
					Tracing.Enter(Tracing.TraceId("TraceReadResponse"), traceMethod, new TraceMethod(this.xmlSerializer, "Deserialize", new object[] { streamReader }));
				}
				object obj = this.xmlSerializer.Deserialize(streamReader);
				if (Tracing.On)
				{
					Tracing.Exit(Tracing.TraceId("TraceReadResponse"), traceMethod);
				}
				obj2 = obj;
			}
			finally
			{
				response.Close();
			}
			return obj2;
		}

		// Token: 0x04000396 RID: 918
		private XmlSerializer xmlSerializer;
	}
}
