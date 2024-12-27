using System;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.IO
{
	// Token: 0x02000002 RID: 2
	internal class EndianBinaryWriter : BinaryWriter
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		internal EndianBinaryWriter(Stream s, bool bigEndian)
			: base(s, new UnicodeEncoding(bigEndian, false))
		{
			this.BigEndian = bigEndian;
		}

		// Token: 0x17000001 RID: 1
		// (set) Token: 0x06000002 RID: 2 RVA: 0x000020E7 File Offset: 0x000010E7
		internal bool BigEndian
		{
			set
			{
				this.bigEndian = value;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020F0 File Offset: 0x000010F0
		public override void Write(ushort value)
		{
			if (this.bigEndian)
			{
				base.Write((byte)(value >> 8));
				base.Write((byte)(value & 255));
				return;
			}
			base.Write(value);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000211C File Offset: 0x0000111C
		public override void Write(uint value)
		{
			if (this.bigEndian)
			{
				base.Write((byte)((value >> 24) & 255U));
				base.Write((byte)((value >> 16) & 255U));
				base.Write((byte)((value >> 8) & 255U));
				base.Write((byte)(value & 255U));
				return;
			}
			base.Write(value);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000217C File Offset: 0x0000117C
		public uint WriteObject(object writeObject)
		{
			uint num = 0U;
			Type type = writeObject.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++)
			{
				object obj = type.InvokeMember(fields[i].Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField, null, writeObject, null, CultureInfo.InvariantCulture);
				if (fields[i].FieldType == typeof(ushort))
				{
					this.Write((ushort)obj);
					num += 2U;
				}
				else
				{
					if (fields[i].FieldType != typeof(uint))
					{
						throw new ArgumentException(CultureAndRegionInfoBuilder.GetResourceString("Argument_UnexpectedObjectFieldType"), fields[i].Name);
					}
					this.Write((uint)obj);
					num += 4U;
				}
			}
			return num;
		}

		// Token: 0x04000001 RID: 1
		private bool bigEndian;
	}
}
