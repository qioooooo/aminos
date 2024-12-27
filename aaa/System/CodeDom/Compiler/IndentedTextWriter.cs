using System;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F7 RID: 503
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class IndentedTextWriter : TextWriter
	{
		// Token: 0x06001111 RID: 4369 RVA: 0x00037EBA File Offset: 0x00036EBA
		public IndentedTextWriter(TextWriter writer)
			: this(writer, "    ")
		{
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00037EC8 File Offset: 0x00036EC8
		public IndentedTextWriter(TextWriter writer, string tabString)
			: base(CultureInfo.InvariantCulture)
		{
			this.writer = writer;
			this.tabString = tabString;
			this.indentLevel = 0;
			this.tabsPending = false;
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00037EF1 File Offset: 0x00036EF1
		public override Encoding Encoding
		{
			get
			{
				return this.writer.Encoding;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001114 RID: 4372 RVA: 0x00037EFE File Offset: 0x00036EFE
		// (set) Token: 0x06001115 RID: 4373 RVA: 0x00037F0B File Offset: 0x00036F0B
		public override string NewLine
		{
			get
			{
				return this.writer.NewLine;
			}
			set
			{
				this.writer.NewLine = value;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001116 RID: 4374 RVA: 0x00037F19 File Offset: 0x00036F19
		// (set) Token: 0x06001117 RID: 4375 RVA: 0x00037F21 File Offset: 0x00036F21
		public int Indent
		{
			get
			{
				return this.indentLevel;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				this.indentLevel = value;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001118 RID: 4376 RVA: 0x00037F31 File Offset: 0x00036F31
		public TextWriter InnerWriter
		{
			get
			{
				return this.writer;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x00037F39 File Offset: 0x00036F39
		internal string TabString
		{
			get
			{
				return this.tabString;
			}
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x00037F41 File Offset: 0x00036F41
		public override void Close()
		{
			this.writer.Close();
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x00037F4E File Offset: 0x00036F4E
		public override void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x00037F5C File Offset: 0x00036F5C
		protected virtual void OutputTabs()
		{
			if (this.tabsPending)
			{
				for (int i = 0; i < this.indentLevel; i++)
				{
					this.writer.Write(this.tabString);
				}
				this.tabsPending = false;
			}
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x00037F9A File Offset: 0x00036F9A
		public override void Write(string s)
		{
			this.OutputTabs();
			this.writer.Write(s);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x00037FAE File Offset: 0x00036FAE
		public override void Write(bool value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00037FC2 File Offset: 0x00036FC2
		public override void Write(char value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00037FD6 File Offset: 0x00036FD6
		public override void Write(char[] buffer)
		{
			this.OutputTabs();
			this.writer.Write(buffer);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00037FEA File Offset: 0x00036FEA
		public override void Write(char[] buffer, int index, int count)
		{
			this.OutputTabs();
			this.writer.Write(buffer, index, count);
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x00038000 File Offset: 0x00037000
		public override void Write(double value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00038014 File Offset: 0x00037014
		public override void Write(float value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00038028 File Offset: 0x00037028
		public override void Write(int value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06001125 RID: 4389 RVA: 0x0003803C File Offset: 0x0003703C
		public override void Write(long value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06001126 RID: 4390 RVA: 0x00038050 File Offset: 0x00037050
		public override void Write(object value)
		{
			this.OutputTabs();
			this.writer.Write(value);
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00038064 File Offset: 0x00037064
		public override void Write(string format, object arg0)
		{
			this.OutputTabs();
			this.writer.Write(format, arg0);
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00038079 File Offset: 0x00037079
		public override void Write(string format, object arg0, object arg1)
		{
			this.OutputTabs();
			this.writer.Write(format, arg0, arg1);
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x0003808F File Offset: 0x0003708F
		public override void Write(string format, params object[] arg)
		{
			this.OutputTabs();
			this.writer.Write(format, arg);
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x000380A4 File Offset: 0x000370A4
		public void WriteLineNoTabs(string s)
		{
			this.writer.WriteLine(s);
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x000380B2 File Offset: 0x000370B2
		public override void WriteLine(string s)
		{
			this.OutputTabs();
			this.writer.WriteLine(s);
			this.tabsPending = true;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x000380CD File Offset: 0x000370CD
		public override void WriteLine()
		{
			this.OutputTabs();
			this.writer.WriteLine();
			this.tabsPending = true;
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x000380E7 File Offset: 0x000370E7
		public override void WriteLine(bool value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00038102 File Offset: 0x00037102
		public override void WriteLine(char value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x0003811D File Offset: 0x0003711D
		public override void WriteLine(char[] buffer)
		{
			this.OutputTabs();
			this.writer.WriteLine(buffer);
			this.tabsPending = true;
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00038138 File Offset: 0x00037138
		public override void WriteLine(char[] buffer, int index, int count)
		{
			this.OutputTabs();
			this.writer.WriteLine(buffer, index, count);
			this.tabsPending = true;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00038155 File Offset: 0x00037155
		public override void WriteLine(double value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00038170 File Offset: 0x00037170
		public override void WriteLine(float value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x0003818B File Offset: 0x0003718B
		public override void WriteLine(int value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000381A6 File Offset: 0x000371A6
		public override void WriteLine(long value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000381C1 File Offset: 0x000371C1
		public override void WriteLine(object value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x000381DC File Offset: 0x000371DC
		public override void WriteLine(string format, object arg0)
		{
			this.OutputTabs();
			this.writer.WriteLine(format, arg0);
			this.tabsPending = true;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x000381F8 File Offset: 0x000371F8
		public override void WriteLine(string format, object arg0, object arg1)
		{
			this.OutputTabs();
			this.writer.WriteLine(format, arg0, arg1);
			this.tabsPending = true;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00038215 File Offset: 0x00037215
		public override void WriteLine(string format, params object[] arg)
		{
			this.OutputTabs();
			this.writer.WriteLine(format, arg);
			this.tabsPending = true;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00038231 File Offset: 0x00037231
		[CLSCompliant(false)]
		public override void WriteLine(uint value)
		{
			this.OutputTabs();
			this.writer.WriteLine(value);
			this.tabsPending = true;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x0003824C File Offset: 0x0003724C
		internal void InternalOutputTabs()
		{
			for (int i = 0; i < this.indentLevel; i++)
			{
				this.writer.Write(this.tabString);
			}
		}

		// Token: 0x04000FA0 RID: 4000
		public const string DefaultTabString = "    ";

		// Token: 0x04000FA1 RID: 4001
		private TextWriter writer;

		// Token: 0x04000FA2 RID: 4002
		private int indentLevel;

		// Token: 0x04000FA3 RID: 4003
		private bool tabsPending;

		// Token: 0x04000FA4 RID: 4004
		private string tabString;
	}
}
