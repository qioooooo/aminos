using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200002C RID: 44
	internal sealed class WriteObjectInfo
	{
		// Token: 0x0600011E RID: 286 RVA: 0x0000E2D7 File Offset: 0x0000D2D7
		internal WriteObjectInfo()
		{
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000E2DF File Offset: 0x0000D2DF
		internal void ObjectEnd()
		{
			WriteObjectInfo.PutObjectInfo(this.serObjectInfoInit, this);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000E2F0 File Offset: 0x0000D2F0
		private void InternalInit()
		{
			this.obj = null;
			this.objectType = null;
			this.isSi = false;
			this.isNamed = false;
			this.isTyped = false;
			this.si = null;
			this.cache = null;
			this.memberData = null;
			this.isArray = false;
			this.objectId = 0L;
			this.assemId = 0L;
			this.lastPosition = 0;
			this.typeAttributeInfo = null;
			this.parentMemberAttributeInfo = null;
			this.arrayElemObjectInfo = null;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000E368 File Offset: 0x0000D368
		internal static WriteObjectInfo Serialize(object obj, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, SoapAttributeInfo attributeInfo, ObjectWriter objectWriter)
		{
			WriteObjectInfo objectInfo = WriteObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.InitSerialize(obj, surrogateSelector, context, serObjectInfoInit, converter, attributeInfo, objectWriter);
			return objectInfo;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000E390 File Offset: 0x0000D390
		internal void InitSerialize(object obj, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, SoapAttributeInfo attributeInfo, ObjectWriter objectWriter)
		{
			this.context = context;
			this.obj = obj;
			this.serObjectInfoInit = serObjectInfoInit;
			this.parentMemberAttributeInfo = attributeInfo;
			this.surrogateSelector = surrogateSelector;
			this.converter = converter;
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
				this.arrayElemObjectInfo = WriteObjectInfo.Serialize(this.objectType.GetElementType(), surrogateSelector, context, serObjectInfoInit, converter, null);
				this.typeAttributeInfo = this.GetTypeAttributeInfo();
				this.isArray = true;
				this.InitNoMembers();
				return;
			}
			this.typeAttributeInfo = this.GetTypeAttributeInfo();
			objectWriter.ObjectManager.RegisterObject(obj);
			ISurrogateSelector surrogateSelector2;
			if (surrogateSelector != null && (this.serializationSurrogate = surrogateSelector.GetSurrogate(this.objectType, context, out surrogateSelector2)) != null)
			{
				this.si = new SerializationInfo(this.objectType, converter);
				if (!this.objectType.IsPrimitive)
				{
					this.serializationSurrogate.GetObjectData(obj, this.si, context);
				}
				this.InitSiWrite(objectWriter);
				return;
			}
			if (!(obj is ISerializable))
			{
				this.InitMemberInfo();
				return;
			}
			if (!this.objectType.IsSerializable)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_NonSerType"), new object[]
				{
					this.objectType.FullName,
					this.objectType.Module.Assembly.FullName
				}));
			}
			this.si = new SerializationInfo(this.objectType, converter);
			((ISerializable)obj).GetObjectData(this.si, context);
			this.InitSiWrite(objectWriter);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000E538 File Offset: 0x0000D538
		[Conditional("SER_LOGGING")]
		private void DumpMemberInfo()
		{
			for (int i = 0; i < this.cache.memberInfos.Length; i++)
			{
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000E560 File Offset: 0x0000D560
		internal static WriteObjectInfo Serialize(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, SoapAttributeInfo attributeInfo)
		{
			WriteObjectInfo objectInfo = WriteObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.InitSerialize(objectType, surrogateSelector, context, serObjectInfoInit, converter, attributeInfo);
			return objectInfo;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000E584 File Offset: 0x0000D584
		internal void InitSerialize(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, SoapAttributeInfo attributeInfo)
		{
			this.objectType = objectType;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			this.parentMemberAttributeInfo = attributeInfo;
			this.surrogateSelector = surrogateSelector;
			this.converter = converter;
			if (objectType.IsArray)
			{
				this.arrayElemObjectInfo = WriteObjectInfo.Serialize(objectType.GetElementType(), surrogateSelector, context, serObjectInfoInit, converter, null);
				this.typeAttributeInfo = this.GetTypeAttributeInfo();
				this.InitNoMembers();
				return;
			}
			this.typeAttributeInfo = this.GetTypeAttributeInfo();
			ISurrogateSelector surrogateSelector2 = null;
			if (surrogateSelector != null)
			{
				this.serializationSurrogate = surrogateSelector.GetSurrogate(objectType, context, out surrogateSelector2);
			}
			if (this.serializationSurrogate != null)
			{
				this.isSi = true;
			}
			else if (objectType != Converter.typeofObject && Converter.typeofISerializable.IsAssignableFrom(objectType))
			{
				this.isSi = true;
			}
			if (this.isSi)
			{
				this.si = new SerializationInfo(objectType, converter);
				this.cache = new SerObjectInfoCache();
				this.cache.fullTypeName = this.si.FullTypeName;
				this.cache.assemblyString = this.si.AssemblyName;
				return;
			}
			this.InitMemberInfo();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000E694 File Offset: 0x0000D694
		private void InitSiWrite(ObjectWriter objectWriter)
		{
			if (this.si.FullTypeName.Equals("FormatterWrapper"))
			{
				this.obj = this.si.GetValue("__WrappedObject", Converter.typeofObject);
				this.InitSerialize(this.obj, this.surrogateSelector, this.context, this.serObjectInfoInit, this.converter, null, objectWriter);
				return;
			}
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

		// Token: 0x06000127 RID: 295 RVA: 0x0000E7E4 File Offset: 0x0000D7E4
		private void InitNoMembers()
		{
			this.cache = (SerObjectInfoCache)this.serObjectInfoInit.seenBeforeTable[this.objectType];
			if (this.cache == null)
			{
				this.cache = new SerObjectInfoCache();
				this.cache.fullTypeName = this.objectType.FullName;
				this.cache.assemblyString = this.objectType.Module.Assembly.FullName;
				this.serObjectInfoInit.seenBeforeTable.Add(this.objectType, this.cache);
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000E878 File Offset: 0x0000D878
		private void InitMemberInfo()
		{
			this.cache = (SerObjectInfoCache)this.serObjectInfoInit.seenBeforeTable[this.objectType];
			if (this.cache == null)
			{
				this.cache = new SerObjectInfoCache();
				int num = 0;
				if (!this.objectType.IsByRef)
				{
					this.cache.memberInfos = FormatterServices.GetSerializableMembers(this.objectType, this.context);
					num = this.cache.memberInfos.Length;
				}
				this.cache.memberNames = new string[num];
				this.cache.memberTypes = new Type[num];
				this.cache.memberAttributeInfos = new SoapAttributeInfo[num];
				for (int i = 0; i < num; i++)
				{
					this.cache.memberNames[i] = this.cache.memberInfos[i].Name;
					this.cache.memberTypes[i] = this.GetMemberType(this.cache.memberInfos[i]);
					this.cache.memberAttributeInfos[i] = Attr.GetMemberAttributeInfo(this.cache.memberInfos[i], this.cache.memberNames[i], this.cache.memberTypes[i]);
				}
				this.cache.fullTypeName = this.objectType.FullName;
				this.cache.assemblyString = this.objectType.Module.Assembly.FullName;
				this.serObjectInfoInit.seenBeforeTable.Add(this.objectType, this.cache);
			}
			if (this.obj != null)
			{
				this.memberData = FormatterServices.GetObjectData(this.obj, this.cache.memberInfos);
			}
			this.isTyped = true;
			this.isNamed = true;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000EA30 File Offset: 0x0000DA30
		internal string GetTypeFullName()
		{
			return this.cache.fullTypeName;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000EA40 File Offset: 0x0000DA40
		internal string GetAssemblyString()
		{
			string text;
			if (this.arrayElemObjectInfo != null)
			{
				text = this.arrayElemObjectInfo.GetAssemblyString();
			}
			else if (this.IsAttributeNameSpace())
			{
				text = this.typeAttributeInfo.m_nameSpace;
			}
			else
			{
				text = this.cache.assemblyString;
			}
			return text;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000EA88 File Offset: 0x0000DA88
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
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_SerMemberInfo"), new object[] { objMember.GetType() }));
				}
				type = ((PropertyInfo)objMember).PropertyType;
			}
			return type;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000EAF0 File Offset: 0x0000DAF0
		internal void GetMemberInfo(out string[] outMemberNames, out Type[] outMemberTypes, out object[] outMemberData, out SoapAttributeInfo[] outAttributeInfo)
		{
			outMemberNames = this.cache.memberNames;
			outMemberTypes = this.cache.memberTypes;
			outMemberData = this.memberData;
			outAttributeInfo = this.cache.memberAttributeInfos;
			if (this.isSi && !this.isNamed)
			{
				throw new SerializationException(SoapUtil.GetResourceString("Serialization_ISerializableMemberInfo"));
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000EB50 File Offset: 0x0000DB50
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

		// Token: 0x0600012E RID: 302 RVA: 0x0000EBA4 File Offset: 0x0000DBA4
		private int Position(string name)
		{
			if (this.cache.memberNames[this.lastPosition].Equals(name))
			{
				return this.lastPosition;
			}
			if (++this.lastPosition < this.cache.memberNames.Length && this.cache.memberNames[this.lastPosition].Equals(name))
			{
				return this.lastPosition;
			}
			for (int i = 0; i < this.cache.memberNames.Length; i++)
			{
				if (this.cache.memberNames[i].Equals(name))
				{
					this.lastPosition = i;
					return this.lastPosition;
				}
			}
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_Position"), new object[] { this.objectType + " " + name }));
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000EC83 File Offset: 0x0000DC83
		private static void PutObjectInfo(SerObjectInfoInit serObjectInfoInit, WriteObjectInfo objectInfo)
		{
			serObjectInfoInit.oiPool.Push(objectInfo);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000EC91 File Offset: 0x0000DC91
		internal bool IsInteropNameSpace()
		{
			if (this.arrayElemObjectInfo != null)
			{
				return this.arrayElemObjectInfo.IsInteropNameSpace();
			}
			return this.IsAttributeNameSpace() || this.IsCallElement();
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000ECBC File Offset: 0x0000DCBC
		internal bool IsCallElement()
		{
			return (this.objectType != Converter.typeofObject && Converter.typeofIMethodCallMessage.IsAssignableFrom(this.objectType) && !Converter.typeofIConstructionCallMessage.IsAssignableFrom(this.objectType)) || this.objectType == Converter.typeofReturnMessage || this.objectType == Converter.typeofInternalSoapMessage;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000ED17 File Offset: 0x0000DD17
		internal bool IsCustomXmlAttribute()
		{
			if (this.arrayElemObjectInfo != null)
			{
				return this.arrayElemObjectInfo.IsCustomXmlAttribute();
			}
			return this.typeAttributeInfo != null && (this.typeAttributeInfo.m_attributeType & SoapAttributeType.XmlAttribute) != SoapAttributeType.None;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000ED47 File Offset: 0x0000DD47
		internal bool IsCustomXmlElement()
		{
			if (this.arrayElemObjectInfo != null)
			{
				return this.arrayElemObjectInfo.IsCustomXmlElement();
			}
			return this.typeAttributeInfo != null && (this.typeAttributeInfo.m_attributeType & SoapAttributeType.XmlElement) != SoapAttributeType.None;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000ED77 File Offset: 0x0000DD77
		internal bool IsAttributeNameSpace()
		{
			if (this.arrayElemObjectInfo != null)
			{
				return this.arrayElemObjectInfo.IsAttributeNameSpace();
			}
			return this.typeAttributeInfo != null && this.typeAttributeInfo.m_nameSpace != null;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000EDA8 File Offset: 0x0000DDA8
		private SoapAttributeInfo GetTypeAttributeInfo()
		{
			if (this.arrayElemObjectInfo != null)
			{
				return this.arrayElemObjectInfo.GetTypeAttributeInfo();
			}
			SoapAttributeInfo soapAttributeInfo;
			if (this.parentMemberAttributeInfo != null)
			{
				soapAttributeInfo = this.parentMemberAttributeInfo;
			}
			else
			{
				soapAttributeInfo = new SoapAttributeInfo();
			}
			Attr.ProcessTypeAttribute(this.objectType, soapAttributeInfo);
			return soapAttributeInfo;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000EDF0 File Offset: 0x0000DDF0
		internal bool IsEmbeddedAttribute(string name)
		{
			if (this.arrayElemObjectInfo != null)
			{
				return this.arrayElemObjectInfo.IsEmbeddedAttribute(name);
			}
			bool flag = false;
			if (this.cache.memberAttributeInfos != null && this.cache.memberAttributeInfos.Length > 0)
			{
				SoapAttributeInfo soapAttributeInfo = this.cache.memberAttributeInfos[this.Position(name)];
				flag = soapAttributeInfo.IsEmbedded();
			}
			return flag;
		}

		// Token: 0x040001E1 RID: 481
		internal int objectInfoId;

		// Token: 0x040001E2 RID: 482
		internal object obj;

		// Token: 0x040001E3 RID: 483
		internal Type objectType;

		// Token: 0x040001E4 RID: 484
		internal bool isSi;

		// Token: 0x040001E5 RID: 485
		internal bool isNamed;

		// Token: 0x040001E6 RID: 486
		internal bool isTyped;

		// Token: 0x040001E7 RID: 487
		internal SerializationInfo si;

		// Token: 0x040001E8 RID: 488
		internal SerObjectInfoCache cache;

		// Token: 0x040001E9 RID: 489
		internal object[] memberData;

		// Token: 0x040001EA RID: 490
		internal ISerializationSurrogate serializationSurrogate;

		// Token: 0x040001EB RID: 491
		internal ISurrogateSelector surrogateSelector;

		// Token: 0x040001EC RID: 492
		internal IFormatterConverter converter;

		// Token: 0x040001ED RID: 493
		internal StreamingContext context;

		// Token: 0x040001EE RID: 494
		internal SerObjectInfoInit serObjectInfoInit;

		// Token: 0x040001EF RID: 495
		internal long objectId;

		// Token: 0x040001F0 RID: 496
		internal long assemId;

		// Token: 0x040001F1 RID: 497
		private int lastPosition;

		// Token: 0x040001F2 RID: 498
		private SoapAttributeInfo parentMemberAttributeInfo;

		// Token: 0x040001F3 RID: 499
		internal bool isArray;

		// Token: 0x040001F4 RID: 500
		internal SoapAttributeInfo typeAttributeInfo;

		// Token: 0x040001F5 RID: 501
		internal WriteObjectInfo arrayElemObjectInfo;
	}
}
