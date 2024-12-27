using System;
using System.Collections;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000298 RID: 664
	internal class SerializationHelperSql9
	{
		// Token: 0x06002270 RID: 8816 RVA: 0x0026D90C File Offset: 0x0026CD0C
		private SerializationHelperSql9()
		{
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0026D920 File Offset: 0x0026CD20
		internal static int SizeInBytes(Type t)
		{
			return SerializationHelperSql9.SizeInBytes(Activator.CreateInstance(t));
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x0026D938 File Offset: 0x0026CD38
		internal static int SizeInBytes(object instance)
		{
			Type type = instance.GetType();
			SerializationHelperSql9.GetFormat(type);
			DummyStream dummyStream = new DummyStream();
			Serializer serializer = SerializationHelperSql9.GetSerializer(instance.GetType());
			serializer.Serialize(dummyStream, instance);
			return (int)dummyStream.Length;
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x0026D974 File Offset: 0x0026CD74
		internal static void Serialize(Stream s, object instance)
		{
			SerializationHelperSql9.GetSerializer(instance.GetType()).Serialize(s, instance);
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x0026D994 File Offset: 0x0026CD94
		internal static object Deserialize(Stream s, Type resultType)
		{
			return SerializationHelperSql9.GetSerializer(resultType).Deserialize(s);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x0026D9B0 File Offset: 0x0026CDB0
		private static Format GetFormat(Type t)
		{
			return SerializationHelperSql9.GetUdtAttribute(t).Format;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0026D9C8 File Offset: 0x0026CDC8
		private static Serializer GetSerializer(Type t)
		{
			if (SerializationHelperSql9.m_types2Serializers == null)
			{
				SerializationHelperSql9.m_types2Serializers = new Hashtable();
			}
			Serializer serializer = (Serializer)SerializationHelperSql9.m_types2Serializers[t];
			if (serializer == null)
			{
				serializer = SerializationHelperSql9.GetNewSerializer(t);
				SerializationHelperSql9.m_types2Serializers[t] = serializer;
			}
			return serializer;
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x0026DA10 File Offset: 0x0026CE10
		internal static int GetUdtMaxLength(Type t)
		{
			SqlUdtInfo fromType = SqlUdtInfo.GetFromType(t);
			if (Format.Native == fromType.SerializationFormat)
			{
				return SerializationHelperSql9.SizeInBytes(t);
			}
			return fromType.MaxByteSize;
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x0026DA3C File Offset: 0x0026CE3C
		private static object[] GetCustomAttributes(Type t)
		{
			return t.GetCustomAttributes(typeof(SqlUserDefinedTypeAttribute), false);
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x0026DA5C File Offset: 0x0026CE5C
		internal static SqlUserDefinedTypeAttribute GetUdtAttribute(Type t)
		{
			object[] customAttributes = SerializationHelperSql9.GetCustomAttributes(t);
			if (customAttributes != null && customAttributes.Length == 1)
			{
				return (SqlUserDefinedTypeAttribute)customAttributes[0];
			}
			throw InvalidUdtException.Create(t, "SqlUdtReason_NoUdtAttribute");
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x0026DA94 File Offset: 0x0026CE94
		private static Serializer GetNewSerializer(Type t)
		{
			SerializationHelperSql9.GetUdtAttribute(t);
			Format format = SerializationHelperSql9.GetFormat(t);
			switch (format)
			{
			case Format.Native:
				return new NormalizedSerializer(t);
			case Format.UserDefined:
				return new BinarySerializeSerializer(t);
			}
			throw ADP.InvalidUserDefinedTypeSerializationFormat(format);
		}

		// Token: 0x04001654 RID: 5716
		[ThreadStatic]
		private static Hashtable m_types2Serializers;
	}
}
