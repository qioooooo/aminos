﻿using System;
using System.Runtime.Serialization;

namespace <CrtImplementationDetails>
{
	// Token: 0x020000AD RID: 173
	[Serializable]
	internal class ModuleLoadExceptionHandlerException : ModuleLoadException
	{
		// Token: 0x06000116 RID: 278 RVA: 0x00006298 File Offset: 0x00005698
		protected ModuleLoadExceptionHandlerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			this.NestedException = (Exception)info.GetValue("NestedException", typeof(Exception));
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00006884 File Offset: 0x00005C84
		public ModuleLoadExceptionHandlerException(string message, Exception innerException, Exception nestedException)
			: base(message, innerException)
		{
			this.NestedException = nestedException;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000619C File Offset: 0x0000559C
		// (set) Token: 0x06000119 RID: 281 RVA: 0x000061B0 File Offset: 0x000055B0
		public Exception NestedException
		{
			get
			{
				return this.<backing_store>NestedException;
			}
			set
			{
				this.<backing_store>NestedException = value;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x000061C4 File Offset: 0x000055C4
		public override string ToString()
		{
			string text;
			if (this.InnerException != null)
			{
				text = this.InnerException.ToString();
			}
			else
			{
				text = string.Empty;
			}
			string text2;
			if (this.NestedException != null)
			{
				text2 = this.NestedException.ToString();
			}
			else
			{
				text2 = string.Empty;
			}
			object[] array = new object[4];
			array[0] = this.GetType();
			string text3;
			if (this.Message != null)
			{
				text3 = this.Message;
			}
			else
			{
				text3 = string.Empty;
			}
			array[1] = text3;
			string text4;
			if (text != null)
			{
				text4 = text;
			}
			else
			{
				text4 = string.Empty;
			}
			array[2] = text4;
			string text5;
			if (text2 != null)
			{
				text5 = text2;
			}
			else
			{
				text5 = string.Empty;
			}
			array[3] = text5;
			return string.Format("\n{0}: {1}\n--- Start of primary exception ---\n{2}\n--- End of primary exception ---\n\n--- Start of nested exception ---\n{3}\n--- End of nested exception ---\n", array);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00006268 File Offset: 0x00005668
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("NestedException", this.NestedException, typeof(Exception));
		}

		// Token: 0x040000BE RID: 190
		private const string formatString = "\n{0}: {1}\n--- Start of primary exception ---\n{2}\n--- End of primary exception ---\n\n--- Start of nested exception ---\n{3}\n--- End of nested exception ---\n";

		// Token: 0x040000BF RID: 191
		private Exception <backing_store>NestedException;
	}
}
