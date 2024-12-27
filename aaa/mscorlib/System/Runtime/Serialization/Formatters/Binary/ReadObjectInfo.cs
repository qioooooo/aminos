using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007E3 RID: 2019
	internal sealed class ReadObjectInfo
	{
		// Token: 0x060047E8 RID: 18408 RVA: 0x000FA04E File Offset: 0x000F904E
		internal ReadObjectInfo()
		{
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x000FA056 File Offset: 0x000F9056
		internal void ObjectEnd()
		{
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x000FA058 File Offset: 0x000F9058
		internal void PrepareForReuse()
		{
			this.lastPosition = 0;
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x000FA064 File Offset: 0x000F9064
		internal static ReadObjectInfo Create(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.Init(objectType, surrogateSelector, context, objectManager, serObjectInfoInit, converter, bSimpleAssembly);
			return objectInfo;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x000FA08A File Offset: 0x000F908A
		internal void Init(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			this.objectType = objectType;
			this.objectManager = objectManager;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			this.formatterConverter = converter;
			this.bSimpleAssembly = bSimpleAssembly;
			this.InitReadConstructor(objectType, surrogateSelector, context);
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x000FA0C4 File Offset: 0x000F90C4
		internal static ReadObjectInfo Create(Type objectType, string[] memberNames, Type[] memberTypes, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			ReadObjectInfo objectInfo = ReadObjectInfo.GetObjectInfo(serObjectInfoInit);
			objectInfo.Init(objectType, memberNames, memberTypes, surrogateSelector, context, objectManager, serObjectInfoInit, converter, bSimpleAssembly);
			return objectInfo;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x000FA0F0 File Offset: 0x000F90F0
		internal void Init(Type objectType, string[] memberNames, Type[] memberTypes, ISurrogateSelector surrogateSelector, StreamingContext context, ObjectManager objectManager, SerObjectInfoInit serObjectInfoInit, IFormatterConverter converter, bool bSimpleAssembly)
		{
			this.objectType = objectType;
			this.objectManager = objectManager;
			this.wireMemberNames = memberNames;
			this.wireMemberTypes = memberTypes;
			this.context = context;
			this.serObjectInfoInit = serObjectInfoInit;
			this.formatterConverter = converter;
			this.bSimpleAssembly = bSimpleAssembly;
			if (memberNames != null)
			{
				this.isNamed = true;
			}
			if (memberTypes != null)
			{
				this.isTyped = true;
			}
			if (objectType != null)
			{
				this.InitReadConstructor(objectType, surrogateSelector, context);
			}
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x000FA15C File Offset: 0x000F915C
		private void InitReadConstructor(Type objectType, ISurrogateSelector surrogateSelector, StreamingContext context)
		{
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
				this.isSi = true;
			}
			else if (objectType != Converter.typeofObject && Converter.typeofISerializable.IsAssignableFrom(objectType))
			{
				this.isSi = true;
			}
			if (this.isSi)
			{
				this.InitSiRead();
				return;
			}
			this.InitMemberInfo();
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x000FA1CF File Offset: 0x000F91CF
		private void InitSiRead()
		{
			if (this.memberTypesList != null)
			{
				this.memberTypesList = new ArrayList(20);
			}
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x000FA1E6 File Offset: 0x000F91E6
		private void InitNoMembers()
		{
			this.cache = new SerObjectInfoCache();
			this.cache.fullTypeName = this.objectType.FullName;
			this.cache.assemblyString = this.objectType.Assembly.FullName;
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x000FA224 File Offset: 0x000F9224
		private void InitMemberInfo()
		{
			this.cache = new SerObjectInfoCache();
			this.cache.memberInfos = FormatterServices.GetSerializableMembers(this.objectType, this.context);
			this.count = this.cache.memberInfos.Length;
			this.cache.memberNames = new string[this.count];
			this.cache.memberTypes = new Type[this.count];
			for (int i = 0; i < this.count; i++)
			{
				this.cache.memberNames[i] = this.cache.memberInfos[i].Name;
				this.cache.memberTypes[i] = this.GetMemberType(this.cache.memberInfos[i]);
			}
			this.cache.fullTypeName = this.objectType.FullName;
			this.cache.assemblyString = this.objectType.Assembly.FullName;
			this.isTyped = true;
			this.isNamed = true;
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x000FA328 File Offset: 0x000F9328
		internal MemberInfo GetMemberInfo(string name)
		{
			if (this.cache == null)
			{
				return null;
			}
			if (this.isSi)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_MemberInfo"), new object[] { this.objectType + " " + name }));
			}
			if (this.cache.memberInfos == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NoMemberInfo"), new object[] { this.objectType + " " + name }));
			}
			int num = this.Position(name);
			if (num != -1)
			{
				return this.cache.memberInfos[this.Position(name)];
			}
			return null;
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x000FA3E4 File Offset: 0x000F93E4
		internal Type GetType(string name)
		{
			int num = this.Position(name);
			if (num == -1)
			{
				return null;
			}
			Type type;
			if (this.isTyped)
			{
				type = this.cache.memberTypes[num];
			}
			else
			{
				type = (Type)this.memberTypesList[num];
			}
			if (type == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ISerializableTypes"), new object[] { this.objectType + " " + name }));
			}
			return type;
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x000FA468 File Offset: 0x000F9468
		internal void AddValue(string name, object value, ref SerializationInfo si, ref object[] memberData)
		{
			if (this.isSi)
			{
				si.AddValue(name, value);
				return;
			}
			int num = this.Position(name);
			if (num != -1)
			{
				memberData[num] = value;
			}
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x000FA49C File Offset: 0x000F949C
		internal void InitDataStore(ref SerializationInfo si, ref object[] memberData)
		{
			if (this.isSi)
			{
				if (si == null)
				{
					si = new SerializationInfo(this.objectType, this.formatterConverter);
					return;
				}
			}
			else if (memberData == null && this.cache != null)
			{
				memberData = new object[this.cache.memberNames.Length];
			}
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x000FA4EC File Offset: 0x000F94EC
		internal void RecordFixup(long objectId, string name, long idRef)
		{
			if (this.isSi)
			{
				this.objectManager.RecordDelayedFixup(objectId, name, idRef);
				return;
			}
			int num = this.Position(name);
			if (num != -1)
			{
				this.objectManager.RecordFixup(objectId, this.cache.memberInfos[num], idRef);
			}
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x000FA536 File Offset: 0x000F9536
		internal void PopulateObjectMembers(object obj, object[] memberData)
		{
			if (!this.isSi && memberData != null)
			{
				FormatterServices.PopulateObjectMembers(obj, this.cache.memberInfos, memberData);
			}
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x000FA558 File Offset: 0x000F9558
		[Conditional("SER_LOGGING")]
		private void DumpPopulate(MemberInfo[] memberInfos, object[] memberData)
		{
			for (int i = 0; i < memberInfos.Length; i++)
			{
			}
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x000FA573 File Offset: 0x000F9573
		[Conditional("SER_LOGGING")]
		private void DumpPopulateSi()
		{
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x000FA578 File Offset: 0x000F9578
		private int Position(string name)
		{
			if (this.cache == null)
			{
				return -1;
			}
			if (this.cache.memberNames.Length > 0 && this.cache.memberNames[this.lastPosition].Equals(name))
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
			this.lastPosition = 0;
			return -1;
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x000FA644 File Offset: 0x000F9644
		internal Type[] GetMemberTypes(string[] inMemberNames, Type objectType)
		{
			if (this.isSi)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ISerializableTypes"), new object[] { objectType }));
			}
			if (this.cache == null)
			{
				return null;
			}
			if (this.cache.memberTypes == null)
			{
				this.cache.memberTypes = new Type[this.count];
				for (int i = 0; i < this.count; i++)
				{
					this.cache.memberTypes[i] = this.GetMemberType(this.cache.memberInfos[i]);
				}
			}
			bool flag = false;
			if (inMemberNames.Length < this.cache.memberInfos.Length)
			{
				flag = true;
			}
			Type[] array = new Type[this.cache.memberInfos.Length];
			for (int j = 0; j < this.cache.memberInfos.Length; j++)
			{
				if (!flag && inMemberNames[j].Equals(this.cache.memberInfos[j].Name))
				{
					array[j] = this.cache.memberTypes[j];
				}
				else
				{
					bool flag2 = false;
					for (int k = 0; k < inMemberNames.Length; k++)
					{
						if (this.cache.memberInfos[j].Name.Equals(inMemberNames[k]))
						{
							array[j] = this.cache.memberTypes[j];
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						object[] customAttributes = this.cache.memberInfos[j].GetCustomAttributes(typeof(OptionalFieldAttribute), false);
						if ((customAttributes == null || customAttributes.Length == 0) && !this.bSimpleAssembly)
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_MissingMember"), new object[]
							{
								this.cache.memberNames[j],
								objectType,
								typeof(OptionalFieldAttribute).FullName
							}));
						}
					}
				}
			}
			return array;
		}

		// Token: 0x060047FD RID: 18429 RVA: 0x000FA834 File Offset: 0x000F9834
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

		// Token: 0x060047FE RID: 18430 RVA: 0x000FA89C File Offset: 0x000F989C
		private static ReadObjectInfo GetObjectInfo(SerObjectInfoInit serObjectInfoInit)
		{
			return new ReadObjectInfo
			{
				objectInfoId = ReadObjectInfo.readObjectInfoCounter++
			};
		}

		// Token: 0x040024D8 RID: 9432
		internal int objectInfoId;

		// Token: 0x040024D9 RID: 9433
		internal static int readObjectInfoCounter = 1;

		// Token: 0x040024DA RID: 9434
		internal Type objectType;

		// Token: 0x040024DB RID: 9435
		internal ObjectManager objectManager;

		// Token: 0x040024DC RID: 9436
		internal int count;

		// Token: 0x040024DD RID: 9437
		internal bool isSi;

		// Token: 0x040024DE RID: 9438
		internal bool isNamed;

		// Token: 0x040024DF RID: 9439
		internal bool isTyped;

		// Token: 0x040024E0 RID: 9440
		internal bool bSimpleAssembly;

		// Token: 0x040024E1 RID: 9441
		internal SerObjectInfoCache cache;

		// Token: 0x040024E2 RID: 9442
		internal string[] wireMemberNames;

		// Token: 0x040024E3 RID: 9443
		internal Type[] wireMemberTypes;

		// Token: 0x040024E4 RID: 9444
		private int lastPosition;

		// Token: 0x040024E5 RID: 9445
		internal ISurrogateSelector surrogateSelector;

		// Token: 0x040024E6 RID: 9446
		internal ISerializationSurrogate serializationSurrogate;

		// Token: 0x040024E7 RID: 9447
		internal StreamingContext context;

		// Token: 0x040024E8 RID: 9448
		internal ArrayList memberTypesList;

		// Token: 0x040024E9 RID: 9449
		internal SerObjectInfoInit serObjectInfoInit;

		// Token: 0x040024EA RID: 9450
		internal IFormatterConverter formatterConverter;
	}
}
