using System;
using System.Collections;
using System.Text;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200002B RID: 43
	public class VlvRequestControl : DirectoryControl
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00004CFB File Offset: 0x00003CFB
		public VlvRequestControl()
			: base("2.16.840.1.113730.3.4.9", null, true, true)
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00004D0B File Offset: 0x00003D0B
		public VlvRequestControl(int beforeCount, int afterCount, int offset)
			: this()
		{
			this.BeforeCount = beforeCount;
			this.AfterCount = afterCount;
			this.Offset = offset;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00004D28 File Offset: 0x00003D28
		public VlvRequestControl(int beforeCount, int afterCount, string target)
			: this()
		{
			this.BeforeCount = beforeCount;
			this.AfterCount = afterCount;
			if (target != null)
			{
				UTF8Encoding utf8Encoding = new UTF8Encoding();
				byte[] bytes = utf8Encoding.GetBytes(target);
				this.target = bytes;
			}
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00004D61 File Offset: 0x00003D61
		public VlvRequestControl(int beforeCount, int afterCount, byte[] target)
			: this()
		{
			this.BeforeCount = beforeCount;
			this.AfterCount = afterCount;
			this.Target = target;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00004D7E File Offset: 0x00003D7E
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00004D86 File Offset: 0x00003D86
		public int BeforeCount
		{
			get
			{
				return this.before;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.before = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00004DA8 File Offset: 0x00003DA8
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00004DB0 File Offset: 0x00003DB0
		public int AfterCount
		{
			get
			{
				return this.after;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.after = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00004DD2 File Offset: 0x00003DD2
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x00004DDA File Offset: 0x00003DDA
		public int Offset
		{
			get
			{
				return this.offset;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.offset = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004DFC File Offset: 0x00003DFC
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004E04 File Offset: 0x00003E04
		public int EstimateCount
		{
			get
			{
				return this.estimateCount;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("ValidValue"), "value");
				}
				this.estimateCount = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004E28 File Offset: 0x00003E28
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004E6C File Offset: 0x00003E6C
		public byte[] Target
		{
			get
			{
				if (this.target == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.target.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.target[i];
				}
				return array;
			}
			set
			{
				this.target = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004E78 File Offset: 0x00003E78
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004EBC File Offset: 0x00003EBC
		public byte[] ContextId
		{
			get
			{
				if (this.context == null)
				{
					return new byte[0];
				}
				byte[] array = new byte[this.context.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this.context[i];
				}
				return array;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004EC8 File Offset: 0x00003EC8
		public override byte[] GetValue()
		{
			StringBuilder stringBuilder = new StringBuilder(10);
			ArrayList arrayList = new ArrayList();
			stringBuilder.Append("{ii");
			arrayList.Add(this.BeforeCount);
			arrayList.Add(this.AfterCount);
			if (this.Target.Length != 0)
			{
				stringBuilder.Append("t");
				arrayList.Add(129);
				stringBuilder.Append("o");
				arrayList.Add(this.Target);
			}
			else
			{
				stringBuilder.Append("t{");
				arrayList.Add(160);
				stringBuilder.Append("ii");
				arrayList.Add(this.Offset);
				arrayList.Add(this.EstimateCount);
				stringBuilder.Append("}");
			}
			if (this.ContextId.Length != 0)
			{
				stringBuilder.Append("o");
				arrayList.Add(this.ContextId);
			}
			stringBuilder.Append("}");
			object[] array = new object[arrayList.Count];
			for (int i = 0; i < arrayList.Count; i++)
			{
				array[i] = arrayList[i];
			}
			this.directoryControlValue = BerConverter.Encode(stringBuilder.ToString(), array);
			return base.GetValue();
		}

		// Token: 0x040000F6 RID: 246
		private int before;

		// Token: 0x040000F7 RID: 247
		private int after;

		// Token: 0x040000F8 RID: 248
		private int offset;

		// Token: 0x040000F9 RID: 249
		private int estimateCount;

		// Token: 0x040000FA RID: 250
		private byte[] target;

		// Token: 0x040000FB RID: 251
		private byte[] context;
	}
}
