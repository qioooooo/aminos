using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security.Permissions;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E2 RID: 2018
	internal sealed class WriteObjectInfo
	{
		// Token: 0x060047D6 RID: 18390 RVA: 0x000F98D7 File Offset: 0x000F88D7
		internal WriteObjectInfo()
		{
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x000F98DF File Offset: 0x000F88DF
		internal void ObjectEnd()
		{
			WriteObjectInfo.PutObjectInfo(this.serObjectInfoInit, this);
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x000F98F0 File Offset: 0x000F88F0
		private void InternalInit()
		{
			this.obj = null;
			this.objectType = null;
			this.isSi = false;
			this.isNamed = false;
			this.isTyped = false;
			this.isArray = false;
			this.si = null;
			this.cache = null;
			this.memberData = null;
			this.objectId = 0L;
			this.assemId = 0L;
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x000F994C File Offset: 0x000F894C
		internal static WriteObjectInfo Serialize(object obj, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, ObjectWriter objectWriter)
		{
			WriteObjectInfo objectInfo = WriteObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.InitSerialize(obj, surrogateSelector, context, serObjectInfoInit, converter, objectWriter);
			return objectInfo;
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x000F9970 File Offset: 0x000F8970
		internal void InitSerialize(object obj, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, ObjectWriter objectWriter)
		{
			this.context = context;
			this.obj = obj;
			this.serObjectInfoInit = serObjectInfoInit;
			if (RemotingServices.IsTransparentProxy(obj))
			{
				this.objectType = Converter.typeofMarshalByRefObject;
			}
			else
			{
				this.objectType = obj.GetType();
			}
			if (this.objectType.IsArray)
			{
				this.isArray = true;
				this.InitNoMembers();
				return;
			}
			objectWriter.ObjectManager.RegisterObject(obj);
			ISurrogateSelector surrogateSelector2;
			if (surrogateSelector != null && (this.serializationSurrogate = surrogateSelector.GetSurrogate(this.objectType, context, out surrogateSelector2)) != null)
			{
				this.si = new SerializationInfo(this.objectType, converter);
				if (!this.objectType.IsPrimitive)
				{
					this.serializationSurrogate.GetObjectData(obj, this.si, context);
				}
				this.InitSiWrite();
				return;
			}
			if (!(obj is ISerializable))
			{
				this.InitMemberInfo();
				return;
			}
			if (!this.objectType.IsSerializable)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NonSerType"), new object[]
				{
					this.objectType.FullName,
					this.objectType.Assembly.FullName
				}));
			}
			this.si = new SerializationInfo(this.objectType, converter, !FormatterServices.UnsafeTypeForwardersIsEnabled());
			((ISerializable)obj).GetObjectData(this.si, context);
			this.InitSiWrite();
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x000F9AC8 File Offset: 0x000F8AC8
		[Conditional("SER_LOGGING")]
		private void DumpMemberInfo()
		{
			for (int i = 0; i < this.cache.memberInfos.Length; i++)
			{
			}
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x000F9AF0 File Offset: 0x000F8AF0
		internal static WriteObjectInfo Serialize(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter)
		{
			WriteObjectInfo objectInfo = WriteObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.InitSerialize(objectType, surrogateSelector, context, serObjectInfoInit, converter);
			return objectInfo;
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x000F9B14 File Offset: 0x000F8B14
		internal void InitSerialize(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter)
		{
			this.objectType = objectType;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			if (objectType.IsArray)
			{
				this.InitNoMembers();
				return;
			}
			ISurrogateSelector surrogateSelector2 = null;
			if (surrogateSelector != null)
			{
				this.serializationSurrogate = surrogateSelector.GetSurrogate(objectType, context, out surrogateSelector2);
			}
			if (this.serializationSurrogate != null)
			{
				this.si = new SerializationInfo(objectType, converter);
				this.cache = new SerObjectInfoCache();
				this.cache.fullTypeName = this.si.FullTypeName;
				this.cache.assemblyString = this.si.AssemblyName;
				this.isSi = true;
			}
			else if (objectType != Converter.typeofObject && Converter.typeofISerializable.IsAssignableFrom(objectType))
			{
				this.si = new SerializationInfo(objectType, converter, !FormatterServices.UnsafeTypeForwardersIsEnabled());
				this.cache = new SerObjectInfoCache();
				this.cache.fullTypeName = this.si.FullTypeName;
				this.cache.assemblyString = this.si.AssemblyName;
				this.isSi = true;
			}
			if (!this.isSi)
			{
				this.InitMemberInfo();
			}
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x000F9C28 File Offset: 0x000F8C28
		private void InitSiWrite()
		{
			this.isSi = true;
			SerializationInfoEnumerator serializationInfoEnumerator = this.si.GetEnumerator();
			int memberCount = this.si.MemberCount;
			int num = memberCount;
			this.cache = new SerObjectInfoCache();
			this.cache.memberNames = new string[num];
			this.cache.memberTypes = new Type[num];
			this.memberData = new object[num];
			this.cache.fullTypeName = this.si.FullTypeName;
			this.cache.assemblyString = this.si.AssemblyName;
			serializationInfoEnumerator = this.si.GetEnumerator();
			int num2 = 0;
			while (serializationInfoEnumerator.MoveNext())
			{
				this.cache.memberNames[num2] = serializationInfoEnumerator.Name;
				this.cache.memberTypes[num2] = serializationInfoEnumerator.ObjectType;
				this.memberData[num2] = serializationInfoEnumerator.Value;
				num2++;
			}
			this.isNamed = true;
			this.isTyped = false;
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x000F9D20 File Offset: 0x000F8D20
		private void InitNoMembers()
		{
			this.cache = (SerObjectInfoCache)this.serObjectInfoInit.seenBeforeTable[this.objectType];
			if (this.cache == null)
			{
				this.cache = new SerObjectInfoCache();
				this.cache.fullTypeName = this.objectType.FullName;
				this.cache.assemblyString = this.objectType.Assembly.FullName;
				this.serObjectInfoInit.seenBeforeTable.Add(this.objectType, this.cache);
			}
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x000F9DB0 File Offset: 0x000F8DB0
		private void InitMemberInfo()
		{
			this.cache = (SerObjectInfoCache)this.serObjectInfoInit.seenBeforeTable[this.objectType];
			if (this.cache == null)
			{
				this.cache = new SerObjectInfoCache();
				this.cache.memberInfos = FormatterServices.GetSerializableMembers(this.objectType, this.context);
				int num = this.cache.memberInfos.Length;
				this.cache.memberNames = new string[num];
				this.cache.memberTypes = new Type[num];
				for (int i = 0; i < num; i++)
				{
					this.cache.memberNames[i] = this.cache.memberInfos[i].Name;
					this.cache.memberTypes[i] = this.GetMemberType(this.cache.memberInfos[i]);
				}
				this.cache.fullTypeName = this.objectType.FullName;
				this.cache.assemblyString = this.objectType.Assembly.FullName;
				this.serObjectInfoInit.seenBeforeTable.Add(this.objectType, this.cache);
			}
			if (this.obj != null)
			{
				this.memberData = FormatterServices.GetObjectData(this.obj, this.cache.memberInfos);
			}
			this.isTyped = true;
			this.isNamed = true;
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x000F9F0A File Offset: 0x000F8F0A
		internal string GetTypeFullName()
		{
			return this.cache.fullTypeName;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x000F9F17 File Offset: 0x000F8F17
		internal string GetAssemblyString()
		{
			return this.cache.assemblyString;
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x000F9F24 File Offset: 0x000F8F24
		internal Type GetMemberType(MemberInfo objMember)
		{
			Type type;
			if (objMember is FieldInfo)
			{
				type = ((FieldInfo)objMember).FieldType;
			}
			else
			{
				if (!(objMember is PropertyInfo))
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_SerMemberInfo"), new object[] { objMember.GetType() }));
				}
				type = ((PropertyInfo)objMember).PropertyType;
			}
			return type;
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x000F9F8C File Offset: 0x000F8F8C
		internal void GetMemberInfo(out string[] outMemberNames, out Type[] outMemberTypes, out object[] outMemberData)
		{
			outMemberNames = this.cache.memberNames;
			outMemberTypes = this.cache.memberTypes;
			outMemberData = this.memberData;
			if (this.isSi && !this.isNamed)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_ISerializableMemberInfo"));
			}
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x000F9FDC File Offset: 0x000F8FDC
		private static WriteObjectInfo GetObjectInfo(SerObjectInfoInit serObjectInfoInit)
		{
			WriteObjectInfo writeObjectInfo;
			if (!serObjectInfoInit.oiPool.IsEmpty())
			{
				writeObjectInfo = (WriteObjectInfo)serObjectInfoInit.oiPool.Pop();
				writeObjectInfo.InternalInit();
			}
			else
			{
				writeObjectInfo = new WriteObjectInfo();
				writeObjectInfo.objectInfoId = serObjectInfoInit.objectInfoIdCount++;
			}
			return writeObjectInfo;
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x000FA02F File Offset: 0x000F902F
		private static void PutObjectInfo(SerObjectInfoInit serObjectInfoInit, WriteObjectInfo objectInfo)
		{
			serObjectInfoInit.oiPool.Push(objectInfo);
		}

		// Token: 0x040024C8 RID: 9416
		private static SecurityPermission serializationPermission = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);

		// Token: 0x040024C9 RID: 9417
		internal int objectInfoId;

		// Token: 0x040024CA RID: 9418
		internal object obj;

		// Token: 0x040024CB RID: 9419
		internal Type objectType;

		// Token: 0x040024CC RID: 9420
		internal bool isSi;

		// Token: 0x040024CD RID: 9421
		internal bool isNamed;

		// Token: 0x040024CE RID: 9422
		internal bool isTyped;

		// Token: 0x040024CF RID: 9423
		internal bool isArray;

		// Token: 0x040024D0 RID: 9424
		internal SerializationInfo si;

		// Token: 0x040024D1 RID: 9425
		internal SerObjectInfoCache cache;

		// Token: 0x040024D2 RID: 9426
		internal object[] memberData;

		// Token: 0x040024D3 RID: 9427
		internal ISerializationSurrogate serializationSurrogate;

		// Token: 0x040024D4 RID: 9428
		internal StreamingContext context;

		// Token: 0x040024D5 RID: 9429
		internal SerObjectInfoInit serObjectInfoInit;

		// Token: 0x040024D6 RID: 9430
		internal long objectId;

		// Token: 0x040024D7 RID: 9431
		internal long assemId;
	}
}
