using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007C2 RID: 1986
	internal sealed class SerializationHeaderRecord : IStreamable
	{
		// Token: 0x060046DD RID: 18141 RVA: 0x000F373B File Offset: 0x000F273B
		internal SerializationHeaderRecord()
		{
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x000F374A File Offset: 0x000F274A
		internal SerializationHeaderRecord(BinaryHeaderEnum binaryHeaderEnum, int topId, int headerId, int majorVersion, int minorVersion)
		{
			this.binaryHeaderEnum = binaryHeaderEnum;
			this.topId = topId;
			this.headerId = headerId;
			this.majorVersion = majorVersion;
			this.minorVersion = minorVersion;
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x000F3780 File Offset: 0x000F2780
		public void Write(__BinaryWriter sout)
		{
			this.majorVersion = this.binaryFormatterMajorVersion;
			this.minorVersion = this.binaryFormatterMinorVersion;
			sout.WriteByte((byte)this.binaryHeaderEnum);
			sout.WriteInt32(this.topId);
			sout.WriteInt32(this.headerId);
			sout.WriteInt32(this.binaryFormatterMajorVersion);
			sout.WriteInt32(this.binaryFormatterMinorVersion);
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x000F37E2 File Offset: 0x000F27E2
		private static int GetInt32(byte[] buffer, int index)
		{
			return (int)buffer[index] | ((int)buffer[index + 1] << 8) | ((int)buffer[index + 2] << 16) | ((int)buffer[index + 3] << 24);
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x000F3804 File Offset: 0x000F2804
		public void Read(__BinaryParser input)
		{
			byte[] array = input.ReadBytes(17);
			if (array.Length < 17)
			{
				__Error.EndOfFile();
			}
			this.majorVersion = SerializationHeaderRecord.GetInt32(array, 9);
			if (this.majorVersion > this.binaryFormatterMajorVersion)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidFormat"), new object[] { BitConverter.ToString(array) }));
			}
			this.binaryHeaderEnum = (BinaryHeaderEnum)array[0];
			this.topId = SerializationHeaderRecord.GetInt32(array, 1);
			this.headerId = SerializationHeaderRecord.GetInt32(array, 5);
			this.minorVersion = SerializationHeaderRecord.GetInt32(array, 13);
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x000F389E File Offset: 0x000F289E
		public void Dump()
		{
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x000F38A0 File Offset: 0x000F28A0
		[Conditional("_LOGGING")]
		private void DumpInternal()
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x040023A0 RID: 9120
		internal int binaryFormatterMajorVersion = 1;

		// Token: 0x040023A1 RID: 9121
		internal int binaryFormatterMinorVersion;

		// Token: 0x040023A2 RID: 9122
		internal BinaryHeaderEnum binaryHeaderEnum;

		// Token: 0x040023A3 RID: 9123
		internal int topId;

		// Token: 0x040023A4 RID: 9124
		internal int headerId;

		// Token: 0x040023A5 RID: 9125
		internal int majorVersion;

		// Token: 0x040023A6 RID: 9126
		internal int minorVersion;
	}
}
