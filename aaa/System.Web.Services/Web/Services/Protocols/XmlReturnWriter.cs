using System;
using System.IO;
using System.Text;
using System.Web.Services.Diagnostics;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000094 RID: 148
	internal class XmlReturnWriter : MimeReturnWriter
	{
		// Token: 0x060003D3 RID: 979 RVA: 0x0001310C File Offset: 0x0001210C
		public override void Initialize(object o)
		{
			this.xmlSerializer = (XmlSerializer)o;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0001311A File Offset: 0x0001211A
		public override object[] GetInitializers(LogicalMethodInfo[] methodInfos)
		{
			return XmlReturn.GetInitializers(methodInfos);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00013122 File Offset: 0x00012122
		public override object GetInitializer(LogicalMethodInfo methodInfo)
		{
			return XmlReturn.GetInitializer(methodInfo);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001312C File Offset: 0x0001212C
		internal override void Write(HttpResponse response, Stream outputStream, object returnValue)
		{
			Encoding encoding = new UTF8Encoding(false);
			response.ContentType = ContentType.Compose("text/xml", encoding);
			StreamWriter streamWriter = new StreamWriter(outputStream, encoding);
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "Write", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceWriteResponse"), traceMethod, new TraceMethod(this.xmlSerializer, "Serialize", new object[] { streamWriter, returnValue }));
			}
			this.xmlSerializer.Serialize(streamWriter, returnValue);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceWriteResponse"), traceMethod);
			}
		}

		// Token: 0x04000397 RID: 919
		private XmlSerializer xmlSerializer;
	}
}
