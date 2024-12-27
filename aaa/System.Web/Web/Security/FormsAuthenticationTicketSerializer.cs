using System;
using System.IO;

namespace System.Web.Security
{
	// Token: 0x02000339 RID: 825
	internal static class FormsAuthenticationTicketSerializer
	{
		// Token: 0x06002860 RID: 10336 RVA: 0x000B1A34 File Offset: 0x000B0A34
		public static FormsAuthenticationTicket Deserialize(byte[] serializedTicket, int serializedTicketLength)
		{
			FormsAuthenticationTicket formsAuthenticationTicket;
			try
			{
				using (MemoryStream memoryStream = new MemoryStream(serializedTicket))
				{
					using (FormsAuthenticationTicketSerializer.SerializingBinaryReader serializingBinaryReader = new FormsAuthenticationTicketSerializer.SerializingBinaryReader(memoryStream))
					{
						byte b = serializingBinaryReader.ReadByte();
						if (b != 1)
						{
							formsAuthenticationTicket = null;
						}
						else
						{
							int num = (int)serializingBinaryReader.ReadByte();
							long num2 = serializingBinaryReader.ReadInt64();
							DateTime dateTime = new DateTime(num2, DateTimeKind.Utc);
							dateTime.ToLocalTime();
							byte b2 = serializingBinaryReader.ReadByte();
							if (b2 != 254)
							{
								formsAuthenticationTicket = null;
							}
							else
							{
								long num3 = serializingBinaryReader.ReadInt64();
								DateTime dateTime2 = new DateTime(num3, DateTimeKind.Utc);
								dateTime2.ToLocalTime();
								bool flag;
								switch (serializingBinaryReader.ReadByte())
								{
								case 0:
									flag = false;
									break;
								case 1:
									flag = true;
									break;
								default:
									return null;
								}
								string text = serializingBinaryReader.ReadBinaryString();
								string text2 = serializingBinaryReader.ReadBinaryString();
								string text3 = serializingBinaryReader.ReadBinaryString();
								byte b3 = serializingBinaryReader.ReadByte();
								if (b3 != 255)
								{
									formsAuthenticationTicket = null;
								}
								else if (memoryStream.Position != (long)serializedTicketLength)
								{
									formsAuthenticationTicket = null;
								}
								else
								{
									formsAuthenticationTicket = FormsAuthenticationTicket.FromUtc(num, text, dateTime, dateTime2, flag, text2, text3);
								}
							}
						}
					}
				}
			}
			catch
			{
				formsAuthenticationTicket = null;
			}
			return formsAuthenticationTicket;
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x000B1BA0 File Offset: 0x000B0BA0
		public static byte[] Serialize(FormsAuthenticationTicket ticket)
		{
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (FormsAuthenticationTicketSerializer.SerializingBinaryWriter serializingBinaryWriter = new FormsAuthenticationTicketSerializer.SerializingBinaryWriter(memoryStream))
				{
					serializingBinaryWriter.Write(1);
					serializingBinaryWriter.Write((byte)ticket.Version);
					serializingBinaryWriter.Write(ticket.IssueDateUtc.Ticks);
					serializingBinaryWriter.Write(254);
					serializingBinaryWriter.Write(ticket.ExpirationUtc.Ticks);
					serializingBinaryWriter.Write(ticket.IsPersistent);
					serializingBinaryWriter.WriteBinaryString(ticket.Name);
					serializingBinaryWriter.WriteBinaryString(ticket.UserData);
					serializingBinaryWriter.WriteBinaryString(ticket.CookiePath);
					serializingBinaryWriter.Write(byte.MaxValue);
					array = memoryStream.ToArray();
				}
			}
			return array;
		}

		// Token: 0x04001EAF RID: 7855
		private const byte CURRENT_TICKET_SERIALIZED_VERSION = 1;

		// Token: 0x0200033A RID: 826
		private sealed class SerializingBinaryReader : BinaryReader
		{
			// Token: 0x06002862 RID: 10338 RVA: 0x000B1C78 File Offset: 0x000B0C78
			public SerializingBinaryReader(Stream input)
				: base(input)
			{
			}

			// Token: 0x06002863 RID: 10339 RVA: 0x000B1C84 File Offset: 0x000B0C84
			public string ReadBinaryString()
			{
				int num = base.Read7BitEncodedInt();
				byte[] array = this.ReadBytes(num * 2);
				char[] array2 = new char[num];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = (char)((int)array[2 * i] | ((int)array[2 * i + 1] << 8));
				}
				return new string(array2);
			}

			// Token: 0x06002864 RID: 10340 RVA: 0x000B1CD0 File Offset: 0x000B0CD0
			public override string ReadString()
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0200033B RID: 827
		private sealed class SerializingBinaryWriter : BinaryWriter
		{
			// Token: 0x06002865 RID: 10341 RVA: 0x000B1CD7 File Offset: 0x000B0CD7
			public SerializingBinaryWriter(Stream output)
				: base(output)
			{
			}

			// Token: 0x06002866 RID: 10342 RVA: 0x000B1CE0 File Offset: 0x000B0CE0
			public override void Write(string value)
			{
				throw new NotImplementedException();
			}

			// Token: 0x06002867 RID: 10343 RVA: 0x000B1CE8 File Offset: 0x000B0CE8
			public void WriteBinaryString(string value)
			{
				byte[] array = new byte[value.Length * 2];
				for (int i = 0; i < value.Length; i++)
				{
					char c = value[i];
					array[2 * i] = (byte)c;
					array[2 * i + 1] = (byte)(c >> 8);
				}
				base.Write7BitEncodedInt(value.Length);
				this.Write(array);
			}
		}
	}
}
