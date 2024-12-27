using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000008 RID: 8
	internal sealed class AttributeList
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00004D75 File Offset: 0x00003D75
		internal int Count
		{
			get
			{
				return this.nameA.Count();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00004D82 File Offset: 0x00003D82
		internal void Clear()
		{
			this.nameA.Clear();
			this.valueA.Clear();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004D9A File Offset: 0x00003D9A
		internal void Put(string name, string value)
		{
			this.nameA.Push(name);
			this.valueA.Push(value);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004DB4 File Offset: 0x00003DB4
		internal void Get(int index, out string name, out string value)
		{
			name = (string)this.nameA.Next();
			value = (string)this.valueA.Next();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004DDA File Offset: 0x00003DDA
		[Conditional("SER_LOGGING")]
		internal void Dump()
		{
		}

		// Token: 0x04000038 RID: 56
		private SerStack nameA = new SerStack("AttributeName");

		// Token: 0x04000039 RID: 57
		private SerStack valueA = new SerStack("AttributeValue");
	}
}
