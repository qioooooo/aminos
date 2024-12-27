using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200002D RID: 45
	internal sealed class ReadObjectInfo
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000EE4D File Offset: 0x0000DE4D
		internal ReadObjectInfo()
		{
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000EE55 File Offset: 0x0000DE55
		internal void ObjectEnd()
		{
			ReadObjectInfo.PutObjectInfo(this.serObjectInfoInit, this);
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000EE64 File Offset: 0x0000DE64
		private void InternalInit()
		{
			this.obj = null;
			this.objectType = null;
			this.count = 0;
			this.isSi = false;
			this.isNamed = false;
			this.isTyped = false;
			this.si = null;
			this.wireMemberNames = null;
			this.wireMemberTypes = null;
			this.cache = null;
			this.lastPosition = 0;
			this.numberMembersSeen = 0;
			this.bfake = false;
			this.bSoapFault = false;
			this.majorVersion = 0;
			this.minorVersion = 0;
			this.typeAttributeInfo = null;
			this.arrayElemObjectInfo = null;
			if (this.memberTypesList != null)
			{
				this.memberTypesList.Clear();
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000EF04 File Offset: 0x0000DF04
		internal static ReadObjectInfo Create(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, string assemblyName)
		{
			ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.Init(objectType, surrogateSelector, context, objectManager, serObjectInfoInit, converter, assemblyName);
			return objectInfo;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000EF2A File Offset: 0x0000DF2A
		internal void Init(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, string assemblyName)
		{
			this.objectType = objectType;
			this.objectManager = objectManager;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			this.formatterConverter = converter;
			this.InitReadConstructor(objectType, surrogateSelector, context, assemblyName);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000EF60 File Offset: 0x0000DF60
		internal static ReadObjectInfo Create(Type objectType, string[] memberNames, Type[] memberTypes, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, string assemblyName)
		{
			ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.Init(objectType, memberNames, memberTypes, surrogateSelector, context, objectManager, serObjectInfoInit, converter, assemblyName);
			return objectInfo;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000EF8C File Offset: 0x0000DF8C
		internal void Init(Type objectType, string[] memberNames, Type[] memberTypes, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, string assemblyName)
		{
			this.objectType = objectType;
			this.objectManager = objectManager;
			this.wireMemberNames = memberNames;
			this.wireMemberTypes = memberTypes;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			this.formatterConverter = converter;
			if (memberNames != null)
			{
				this.isNamed = true;
			}
			if (memberTypes != null)
			{
				this.isTyped = true;
			}
			this.InitReadConstructor(objectType, surrogateSelector, context, assemblyName);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000EFF0 File Offset: 0x0000DFF0
		private void InitReadConstructor(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, string assemblyName)
		{
			if (objectType.IsArray)
			{
				this.arrayElemObjectInfo = ReadObjectInfo.Create(objectType.GetElementType(), surrogateSelector, context, this.objectManager, this.serObjectInfoInit, this.formatterConverter, assemblyName);
				this.typeAttributeInfo = this.GetTypeAttributeInfo();
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
				this.isSi = true;
			}
			else if (objectType != Converter.typeofObject && Converter.typeofISerializable.IsAssignableFrom(objectType))
			{
				this.isSi = true;
			}
			if (this.isSi)
			{
				this.si = new SerializationInfo(objectType, this.formatterConverter);
				this.InitSiRead(assemblyName);
				return;
			}
			this.InitMemberInfo();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000F0AC File Offset: 0x0000E0AC
		private void InitSiRead(string assemblyName)
		{
			if (assemblyName != null)
			{
				this.si.AssemblyName = assemblyName;
			}
			this.cache = new SerObjectInfoCache();
			this.cache.fullTypeName = this.si.FullTypeName;
			this.cache.assemblyString = this.si.AssemblyName;
			this.cache.memberNames = this.wireMemberNames;
			this.cache.memberTypes = this.wireMemberTypes;
			if (this.memberTypesList != null)
			{
				this.memberTypesList = new ArrayList(20);
			}
			if (this.wireMemberNames != null && this.wireMemberTypes != null)
			{
				this.isTyped = true;
			}
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000F150 File Offset: 0x0000E150
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

		// Token: 0x06000141 RID: 321 RVA: 0x0000F1E4 File Offset: 0x0000E1E4
		private void InitMemberInfo()
		{
			this.cache = (SerObjectInfoCache)this.serObjectInfoInit.seenBeforeTable[this.objectType];
			if (this.cache == null)
			{
				this.cache = new SerObjectInfoCache();
				this.cache.memberInfos = FormatterServices.GetSerializableMembers(this.objectType, this.context);
				this.count = this.cache.memberInfos.Length;
				this.cache.memberNames = new string[this.count];
				this.cache.memberTypes = new Type[this.count];
				this.cache.memberAttributeInfos = new SoapAttributeInfo[this.count];
				for (int i = 0; i < this.count; i++)
				{
					this.cache.memberNames[i] = this.cache.memberInfos[i].Name;
					this.cache.memberTypes[i] = this.GetMemberType(this.cache.memberInfos[i]);
					this.cache.memberAttributeInfos[i] = Attr.GetMemberAttributeInfo(this.cache.memberInfos[i], this.cache.memberNames[i], this.cache.memberTypes[i]);
				}
				this.cache.fullTypeName = this.objectType.FullName;
				this.cache.assemblyString = this.objectType.Module.Assembly.FullName;
				this.serObjectInfoInit.seenBeforeTable.Add(this.objectType, this.cache);
			}
			this.memberData = new object[this.cache.memberNames.Length];
			this.memberNames = new string[this.cache.memberNames.Length];
			this.isTyped = true;
			this.isNamed = true;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000F3B8 File Offset: 0x0000E3B8
		internal MemberInfo GetMemberInfo(string name)
		{
			if (this.isSi)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_MemberInfo"), new object[] { this.objectType + " " + name }));
			}
			if (this.cache.memberInfos == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_NoMemberInfo"), new object[] { this.objectType + " " + name }));
			}
			return this.cache.memberInfos[this.Position(name)];
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000F45C File Offset: 0x0000E45C
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

		// Token: 0x06000144 RID: 324 RVA: 0x0000F4C4 File Offset: 0x0000E4C4
		internal Type GetType(string name)
		{
			Type type;
			if (this.isTyped)
			{
				type = this.cache.memberTypes[this.Position(name)];
			}
			else
			{
				type = (Type)this.memberTypesList[this.Position(name)];
			}
			if (type == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ISerializableTypes"), new object[] { this.objectType + " " + name }));
			}
			return type;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000F544 File Offset: 0x0000E544
		internal Type GetType(int position)
		{
			Type type = null;
			if (this.isTyped)
			{
				if (position >= this.cache.memberTypes.Length)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, SoapUtil.GetResourceString("Serialization_ISerializableTypes"), new object[] { this.objectType + " " + position }));
				}
				type = this.cache.memberTypes[position];
			}
			return type;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000F5B8 File Offset: 0x0000E5B8
		internal void AddParamName(string name)
		{
			if (!this.bfake)
			{
				return;
			}
			if (name[0] == '_' && name[1] == '_')
			{
				if (name == "__fault")
				{
					this.bSoapFault = true;
					return;
				}
				if (name == "__methodName" || name == "__keyToNamespaceTable" || name == "__paramNameList" || name == "__xmlNameSpace")
				{
					return;
				}
			}
			this.paramNameList.Add(name);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000F63C File Offset: 0x0000E63C
		internal void AddValue(string name, object value)
		{
			if (this.isSi)
			{
				if (this.bfake)
				{
					this.AddParamName(name);
				}
				this.si.AddValue(name, value);
				return;
			}
			int num = this.Position(name);
			this.memberData[num] = value;
			this.memberNames[num] = name;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000F688 File Offset: 0x0000E688
		internal void AddMemberSeen()
		{
			this.numberMembersSeen++;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000F698 File Offset: 0x0000E698
		internal ArrayList SetFakeObject()
		{
			this.bfake = true;
			this.paramNameList = new ArrayList(10);
			return this.paramNameList;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0000F6B4 File Offset: 0x0000E6B4
		internal void SetVersion(int major, int minor)
		{
			this.majorVersion = major;
			this.minorVersion = minor;
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000F6C4 File Offset: 0x0000E6C4
		internal void RecordFixup(long objectId, string name, long idRef)
		{
			if (this.isSi)
			{
				this.objectManager.RecordDelayedFixup(objectId, name, idRef);
				return;
			}
			this.objectManager.RecordFixup(objectId, this.cache.memberInfos[this.Position(name)], idRef);
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000F700 File Offset: 0x0000E700
		internal void PopulateObjectMembers()
		{
			if (!this.isSi)
			{
				int num = 0;
				MemberInfo[] array;
				object[] array2;
				if (this.numberMembersSeen < this.memberNames.Length)
				{
					array = new MemberInfo[this.numberMembersSeen];
					array2 = new object[this.numberMembersSeen];
					for (int i = 0; i < this.memberNames.Length; i++)
					{
						if (this.memberNames[i] == null)
						{
							object[] customAttributes = this.cache.memberInfos[i].GetCustomAttributes(typeof(OptionalFieldAttribute), false);
							if ((customAttributes == null || customAttributes.Length == 0) && this.majorVersion >= 1 && this.minorVersion >= 0)
							{
								throw new SerializationException(SoapUtil.GetResourceString("Serialization_WrongNumberOfMembers", new object[]
								{
									this.objectType,
									this.cache.memberInfos.Length,
									this.numberMembersSeen
								}));
							}
						}
						else
						{
							if (this.memberNames[i] != this.cache.memberInfos[i].Name)
							{
								throw new SerializationException(SoapUtil.GetResourceString("Serialization_WrongNumberOfMembers", new object[]
								{
									this.objectType,
									this.cache.memberInfos.Length,
									this.numberMembersSeen
								}));
							}
							array[num] = this.cache.memberInfos[i];
							array2[num] = this.memberData[i];
							num++;
						}
					}
				}
				else
				{
					array = this.cache.memberInfos;
					array2 = this.memberData;
				}
				FormatterServices.PopulateObjectMembers(this.obj, array, array2);
				this.numberMembersSeen = 0;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000F8B0 File Offset: 0x0000E8B0
		[Conditional("SER_LOGGING")]
		private void DumpPopulate(MemberInfo[] memberInfos, object[] memberData)
		{
			for (int i = 0; i < memberInfos.Length; i++)
			{
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000F8CC File Offset: 0x0000E8CC
		[Conditional("SER_LOGGING")]
		private void DumpPopulateSi()
		{
			SerializationInfoEnumerator enumerator = this.si.GetEnumerator();
			int num = 0;
			while (enumerator.MoveNext())
			{
				num++;
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000F8F8 File Offset: 0x0000E8F8
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

		// Token: 0x06000150 RID: 336 RVA: 0x0000F9D8 File Offset: 0x0000E9D8
		private static ReadObjectInfo GetObjectInfo(SerObjectInfoInit serObjectInfoInit)
		{
			ReadObjectInfo readObjectInfo;
			if (!serObjectInfoInit.oiPool.IsEmpty())
			{
				readObjectInfo = (ReadObjectInfo)serObjectInfoInit.oiPool.Pop();
				readObjectInfo.InternalInit();
			}
			else
			{
				readObjectInfo = new ReadObjectInfo();
				readObjectInfo.objectInfoId = serObjectInfoInit.objectInfoIdCount++;
			}
			return readObjectInfo;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000FA2B File Offset: 0x0000EA2B
		private static void PutObjectInfo(SerObjectInfoInit serObjectInfoInit, ReadObjectInfo objectInfo)
		{
			serObjectInfoInit.oiPool.Push(objectInfo);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000FA3C File Offset: 0x0000EA3C
		private SoapAttributeInfo GetTypeAttributeInfo()
		{
			if (this.arrayElemObjectInfo != null)
			{
				return this.arrayElemObjectInfo.GetTypeAttributeInfo();
			}
			SoapAttributeInfo soapAttributeInfo = new SoapAttributeInfo();
			Attr.ProcessTypeAttribute(this.objectType, soapAttributeInfo);
			return soapAttributeInfo;
		}

		// Token: 0x040001F6 RID: 502
		internal int objectInfoId;

		// Token: 0x040001F7 RID: 503
		internal object obj;

		// Token: 0x040001F8 RID: 504
		internal Type objectType;

		// Token: 0x040001F9 RID: 505
		internal ObjectManager objectManager;

		// Token: 0x040001FA RID: 506
		internal int count;

		// Token: 0x040001FB RID: 507
		internal bool isSi;

		// Token: 0x040001FC RID: 508
		internal bool isNamed;

		// Token: 0x040001FD RID: 509
		internal bool isTyped;

		// Token: 0x040001FE RID: 510
		internal SerializationInfo si;

		// Token: 0x040001FF RID: 511
		internal SerObjectInfoCache cache;

		// Token: 0x04000200 RID: 512
		internal string[] wireMemberNames;

		// Token: 0x04000201 RID: 513
		internal Type[] wireMemberTypes;

		// Token: 0x04000202 RID: 514
		internal object[] memberData;

		// Token: 0x04000203 RID: 515
		internal string[] memberNames;

		// Token: 0x04000204 RID: 516
		private int lastPosition;

		// Token: 0x04000205 RID: 517
		internal ISurrogateSelector surrogateSelector;

		// Token: 0x04000206 RID: 518
		internal ISerializationSurrogate serializationSurrogate;

		// Token: 0x04000207 RID: 519
		internal StreamingContext context;

		// Token: 0x04000208 RID: 520
		internal ArrayList memberTypesList;

		// Token: 0x04000209 RID: 521
		internal SerObjectInfoInit serObjectInfoInit;

		// Token: 0x0400020A RID: 522
		internal IFormatterConverter formatterConverter;

		// Token: 0x0400020B RID: 523
		internal bool bfake;

		// Token: 0x0400020C RID: 524
		internal bool bSoapFault;

		// Token: 0x0400020D RID: 525
		internal ArrayList paramNameList;

		// Token: 0x0400020E RID: 526
		private int majorVersion;

		// Token: 0x0400020F RID: 527
		private int minorVersion;

		// Token: 0x04000210 RID: 528
		internal SoapAttributeInfo typeAttributeInfo;

		// Token: 0x04000211 RID: 529
		private ReadObjectInfo arrayElemObjectInfo;

		// Token: 0x04000212 RID: 530
		private int numberMembersSeen;
	}
}
