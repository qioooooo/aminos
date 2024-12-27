using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000039 RID: 57
	public abstract class UrlEncodedParameterWriter : MimeParameterWriter
	{
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000144 RID: 324 RVA: 0x000054FE File Offset: 0x000044FE
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00005506 File Offset: 0x00004506
		public override Encoding RequestEncoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				this.encoding = value;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000550F File Offset: 0x0000450F
		public override object GetInitializer(LogicalMethodInfo methodInfo)
		{
			if (!ValueCollectionParameterReader.IsSupported(methodInfo))
			{
				return null;
			}
			return methodInfo.InParameters;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00005521 File Offset: 0x00004521
		public override void Initialize(object initializer)
		{
			this.paramInfos = (ParameterInfo[])initializer;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00005530 File Offset: 0x00004530
		protected void Encode(TextWriter writer, object[] values)
		{
			this.numberEncoded = 0;
			for (int i = 0; i < this.paramInfos.Length; i++)
			{
				ParameterInfo parameterInfo = this.paramInfos[i];
				if (parameterInfo.ParameterType.IsArray)
				{
					Array array = (Array)values[i];
					for (int j = 0; j < array.Length; j++)
					{
						this.Encode(writer, parameterInfo.Name, array.GetValue(j));
					}
				}
				else
				{
					this.Encode(writer, parameterInfo.Name, values[i]);
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000055B0 File Offset: 0x000045B0
		protected void Encode(TextWriter writer, string name, object value)
		{
			if (this.numberEncoded > 0)
			{
				writer.Write('&');
			}
			writer.Write(this.UrlEncode(name));
			writer.Write('=');
			writer.Write(this.UrlEncode(ScalarFormatter.ToString(value)));
			this.numberEncoded++;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00005603 File Offset: 0x00004603
		private string UrlEncode(string value)
		{
			if (this.encoding != null)
			{
				return UrlEncoder.UrlEscapeString(value, this.encoding);
			}
			return UrlEncoder.UrlEscapeStringUnicode(value);
		}

		// Token: 0x0400027D RID: 637
		private ParameterInfo[] paramInfos;

		// Token: 0x0400027E RID: 638
		private int numberEncoded;

		// Token: 0x0400027F RID: 639
		private Encoding encoding;
	}
}
