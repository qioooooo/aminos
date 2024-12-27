using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Web.Util
{
	// Token: 0x0200074F RID: 1871
	internal static class AltSerialization
	{
		// Token: 0x06005ADB RID: 23259 RVA: 0x0016E91C File Offset: 0x0016D91C
		internal static void WriteValueToStream(object value, BinaryWriter writer)
		{
			if (value == null)
			{
				writer.Write(21);
				return;
			}
			if (value is string)
			{
				writer.Write(1);
				writer.Write((string)value);
				return;
			}
			if (value is int)
			{
				writer.Write(2);
				writer.Write((int)value);
				return;
			}
			if (value is bool)
			{
				writer.Write(3);
				writer.Write((bool)value);
				return;
			}
			if (value is DateTime)
			{
				writer.Write(4);
				writer.Write(((DateTime)value).Ticks);
				return;
			}
			if (value is decimal)
			{
				writer.Write(5);
				int[] bits = decimal.GetBits((decimal)value);
				for (int i = 0; i < 4; i++)
				{
					writer.Write(bits[i]);
				}
				return;
			}
			if (value is byte)
			{
				writer.Write(6);
				writer.Write((byte)value);
				return;
			}
			if (value is char)
			{
				writer.Write(7);
				writer.Write((char)value);
				return;
			}
			if (value is float)
			{
				writer.Write(8);
				writer.Write((float)value);
				return;
			}
			if (value is double)
			{
				writer.Write(9);
				writer.Write((double)value);
				return;
			}
			if (value is sbyte)
			{
				writer.Write(10);
				writer.Write((sbyte)value);
				return;
			}
			if (value is short)
			{
				writer.Write(11);
				writer.Write((short)value);
				return;
			}
			if (value is long)
			{
				writer.Write(12);
				writer.Write((long)value);
				return;
			}
			if (value is ushort)
			{
				writer.Write(13);
				writer.Write((ushort)value);
				return;
			}
			if (value is uint)
			{
				writer.Write(14);
				writer.Write((uint)value);
				return;
			}
			if (value is ulong)
			{
				writer.Write(15);
				writer.Write((ulong)value);
				return;
			}
			if (value is TimeSpan)
			{
				writer.Write(16);
				writer.Write(((TimeSpan)value).Ticks);
				return;
			}
			if (value is Guid)
			{
				writer.Write(17);
				byte[] array = ((Guid)value).ToByteArray();
				writer.Write(array);
				return;
			}
			if (value is IntPtr)
			{
				writer.Write(18);
				IntPtr intPtr = (IntPtr)value;
				if (IntPtr.Size == 4)
				{
					writer.Write(intPtr.ToInt32());
					return;
				}
				writer.Write(intPtr.ToInt64());
				return;
			}
			else
			{
				if (!(value is UIntPtr))
				{
					writer.Write(20);
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					try
					{
						binaryFormatter.Serialize(writer.BaseStream, value);
					}
					catch (Exception ex)
					{
						HttpException ex2 = new HttpException(SR.GetString("Cant_serialize_session_state"), ex);
						ex2.SetFormatter(new UseLastUnhandledErrorFormatter(ex2));
						throw ex2;
					}
					return;
				}
				writer.Write(19);
				UIntPtr uintPtr = (UIntPtr)value;
				if (UIntPtr.Size == 4)
				{
					writer.Write(uintPtr.ToUInt32());
					return;
				}
				writer.Write(uintPtr.ToUInt64());
				return;
			}
		}

		// Token: 0x06005ADC RID: 23260 RVA: 0x0016EC18 File Offset: 0x0016DC18
		internal static object ReadValueFromStream(BinaryReader reader)
		{
			object obj = null;
			switch (reader.ReadByte())
			{
			case 1:
				obj = reader.ReadString();
				break;
			case 2:
				obj = reader.ReadInt32();
				break;
			case 3:
				obj = reader.ReadBoolean();
				break;
			case 4:
				obj = new DateTime(reader.ReadInt64());
				break;
			case 5:
			{
				int[] array = new int[4];
				for (int i = 0; i < 4; i++)
				{
					array[i] = reader.ReadInt32();
				}
				obj = new decimal(array);
				break;
			}
			case 6:
				obj = reader.ReadByte();
				break;
			case 7:
				obj = reader.ReadChar();
				break;
			case 8:
				obj = reader.ReadSingle();
				break;
			case 9:
				obj = reader.ReadDouble();
				break;
			case 10:
				obj = reader.ReadSByte();
				break;
			case 11:
				obj = reader.ReadInt16();
				break;
			case 12:
				obj = reader.ReadInt64();
				break;
			case 13:
				obj = reader.ReadUInt16();
				break;
			case 14:
				obj = reader.ReadUInt32();
				break;
			case 15:
				obj = reader.ReadUInt64();
				break;
			case 16:
				obj = new TimeSpan(reader.ReadInt64());
				break;
			case 17:
			{
				byte[] array2 = reader.ReadBytes(16);
				obj = new Guid(array2);
				break;
			}
			case 18:
				if (IntPtr.Size == 4)
				{
					obj = new IntPtr(reader.ReadInt32());
				}
				else
				{
					obj = new IntPtr(reader.ReadInt64());
				}
				break;
			case 19:
				if (UIntPtr.Size == 4)
				{
					obj = new UIntPtr(reader.ReadUInt32());
				}
				else
				{
					obj = new UIntPtr(reader.ReadUInt64());
				}
				break;
			case 20:
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				obj = binaryFormatter.Deserialize(reader.BaseStream);
				break;
			}
			case 21:
				obj = null;
				break;
			}
			return obj;
		}

		// Token: 0x02000750 RID: 1872
		private enum TypeID : byte
		{
			// Token: 0x040030D0 RID: 12496
			String = 1,
			// Token: 0x040030D1 RID: 12497
			Int32,
			// Token: 0x040030D2 RID: 12498
			Boolean,
			// Token: 0x040030D3 RID: 12499
			DateTime,
			// Token: 0x040030D4 RID: 12500
			Decimal,
			// Token: 0x040030D5 RID: 12501
			Byte,
			// Token: 0x040030D6 RID: 12502
			Char,
			// Token: 0x040030D7 RID: 12503
			Single,
			// Token: 0x040030D8 RID: 12504
			Double,
			// Token: 0x040030D9 RID: 12505
			SByte,
			// Token: 0x040030DA RID: 12506
			Int16,
			// Token: 0x040030DB RID: 12507
			Int64,
			// Token: 0x040030DC RID: 12508
			UInt16,
			// Token: 0x040030DD RID: 12509
			UInt32,
			// Token: 0x040030DE RID: 12510
			UInt64,
			// Token: 0x040030DF RID: 12511
			TimeSpan,
			// Token: 0x040030E0 RID: 12512
			Guid,
			// Token: 0x040030E1 RID: 12513
			IntPtr,
			// Token: 0x040030E2 RID: 12514
			UIntPtr,
			// Token: 0x040030E3 RID: 12515
			Object,
			// Token: 0x040030E4 RID: 12516
			Null
		}
	}
}
