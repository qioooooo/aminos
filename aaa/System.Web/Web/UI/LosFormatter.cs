using System;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.Web.UI
{
	// Token: 0x02000422 RID: 1058
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class LosFormatter
	{
		// Token: 0x060032F0 RID: 13040 RVA: 0x000DDA30 File Offset: 0x000DCA30
		public LosFormatter()
			: this(false, null)
		{
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x000DDA3A File Offset: 0x000DCA3A
		public LosFormatter(bool enableMac, string macKeyModifier)
			: this(enableMac, LosFormatter.GetBytes(macKeyModifier))
		{
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x000DDA49 File Offset: 0x000DCA49
		public LosFormatter(bool enableMac, byte[] macKeyModifier)
		{
			this._enableMac = enableMac;
			if (enableMac)
			{
				this._formatter = new ObjectStateFormatter(macKeyModifier);
				return;
			}
			this._formatter = new ObjectStateFormatter();
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x000DDA73 File Offset: 0x000DCA73
		private static byte[] GetBytes(string s)
		{
			if (s != null && s.Length != 0)
			{
				return Encoding.Unicode.GetBytes(s);
			}
			return null;
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x000DDA90 File Offset: 0x000DCA90
		public object Deserialize(Stream stream)
		{
			TextReader textReader = new StreamReader(stream);
			return this.Deserialize(textReader);
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x000DDAB0 File Offset: 0x000DCAB0
		public object Deserialize(TextReader input)
		{
			char[] array = new char[128];
			int num = 0;
			int num2 = 24;
			int num3;
			do
			{
				num3 = input.Read(array, num, num2);
				num += num3;
				if (num > array.Length - num2)
				{
					char[] array2 = new char[array.Length * 2];
					Array.Copy(array, array2, array.Length);
					array = array2;
				}
			}
			while (num3 == num2);
			return this.Deserialize(new string(array, 0, num));
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x000DDB11 File Offset: 0x000DCB11
		public object Deserialize(string input)
		{
			return this._formatter.Deserialize(input);
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x000DDB20 File Offset: 0x000DCB20
		public void Serialize(Stream stream, object value)
		{
			TextWriter textWriter = new StreamWriter(stream);
			this.SerializeInternal(textWriter, value);
			textWriter.Flush();
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x000DDB42 File Offset: 0x000DCB42
		public void Serialize(TextWriter output, object value)
		{
			this.SerializeInternal(output, value);
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x000DDB4C File Offset: 0x000DCB4C
		private void SerializeInternal(TextWriter output, object value)
		{
			string text = this._formatter.Serialize(value);
			output.Write(text);
		}

		// Token: 0x040023DA RID: 9178
		private const int InitialBufferSize = 24;

		// Token: 0x040023DB RID: 9179
		private ObjectStateFormatter _formatter;

		// Token: 0x040023DC RID: 9180
		private bool _enableMac;
	}
}
