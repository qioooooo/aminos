using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007BA RID: 1978
	internal sealed class __BinaryParser
	{
		// Token: 0x06004668 RID: 18024 RVA: 0x000F0990 File Offset: 0x000EF990
		internal __BinaryParser(Stream stream, ObjectReader objectReader)
		{
			this.input = stream;
			this.objectReader = objectReader;
			this.dataReader = new BinaryReader(this.input, __BinaryParser.encoding);
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x06004669 RID: 18025 RVA: 0x000F09DE File Offset: 0x000EF9DE
		internal BinaryAssemblyInfo SystemAssemblyInfo
		{
			get
			{
				if (this.systemAssemblyInfo == null)
				{
					this.systemAssemblyInfo = new BinaryAssemblyInfo(Converter.urtAssemblyString, Converter.urtAssembly);
				}
				return this.systemAssemblyInfo;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x0600466A RID: 18026 RVA: 0x000F0A03 File Offset: 0x000EFA03
		internal SizedArray ObjectMapIdTable
		{
			get
			{
				if (this.objectMapIdTable == null)
				{
					this.objectMapIdTable = new SizedArray();
				}
				return this.objectMapIdTable;
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x0600466B RID: 18027 RVA: 0x000F0A1E File Offset: 0x000EFA1E
		internal SizedArray AssemIdToAssemblyTable
		{
			get
			{
				if (this.assemIdToAssemblyTable == null)
				{
					this.assemIdToAssemblyTable = new SizedArray(2);
				}
				return this.assemIdToAssemblyTable;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x0600466C RID: 18028 RVA: 0x000F0A3A File Offset: 0x000EFA3A
		internal ParseRecord prs
		{
			get
			{
				if (this.PRS == null)
				{
					this.PRS = new ParseRecord();
				}
				return this.PRS;
			}
		}

		// Token: 0x0600466D RID: 18029 RVA: 0x000F0A58 File Offset: 0x000EFA58
		internal void Run()
		{
			try
			{
				bool flag = true;
				this.ReadBegin();
				this.ReadSerializationHeaderRecord();
				while (flag)
				{
					BinaryHeaderEnum binaryHeaderEnum = BinaryHeaderEnum.Object;
					switch (this.expectedType)
					{
					case BinaryTypeEnum.Primitive:
						this.ReadMemberPrimitiveUnTyped();
						break;
					case BinaryTypeEnum.String:
					case BinaryTypeEnum.Object:
					case BinaryTypeEnum.ObjectUrt:
					case BinaryTypeEnum.ObjectUser:
					case BinaryTypeEnum.ObjectArray:
					case BinaryTypeEnum.StringArray:
					case BinaryTypeEnum.PrimitiveArray:
					{
						byte b = this.dataReader.ReadByte();
						binaryHeaderEnum = (BinaryHeaderEnum)b;
						switch (binaryHeaderEnum)
						{
						case BinaryHeaderEnum.Object:
							this.ReadObject();
							break;
						case BinaryHeaderEnum.ObjectWithMap:
						case BinaryHeaderEnum.ObjectWithMapAssemId:
							this.ReadObjectWithMap(binaryHeaderEnum);
							break;
						case BinaryHeaderEnum.ObjectWithMapTyped:
						case BinaryHeaderEnum.ObjectWithMapTypedAssemId:
							this.ReadObjectWithMapTyped(binaryHeaderEnum);
							break;
						case BinaryHeaderEnum.ObjectString:
						case BinaryHeaderEnum.CrossAppDomainString:
							this.ReadObjectString(binaryHeaderEnum);
							break;
						case BinaryHeaderEnum.Array:
						case BinaryHeaderEnum.ArraySinglePrimitive:
						case BinaryHeaderEnum.ArraySingleObject:
						case BinaryHeaderEnum.ArraySingleString:
							this.ReadArray(binaryHeaderEnum);
							break;
						case BinaryHeaderEnum.MemberPrimitiveTyped:
							this.ReadMemberPrimitiveTyped();
							break;
						case BinaryHeaderEnum.MemberReference:
							this.ReadMemberReference();
							break;
						case BinaryHeaderEnum.ObjectNull:
						case BinaryHeaderEnum.ObjectNullMultiple256:
						case BinaryHeaderEnum.ObjectNullMultiple:
							this.ReadObjectNull(binaryHeaderEnum);
							break;
						case BinaryHeaderEnum.MessageEnd:
							flag = false;
							this.ReadMessageEnd();
							this.ReadEnd();
							break;
						case BinaryHeaderEnum.Assembly:
						case BinaryHeaderEnum.CrossAppDomainAssembly:
							this.ReadAssembly(binaryHeaderEnum);
							break;
						case BinaryHeaderEnum.CrossAppDomainMap:
							this.ReadCrossAppDomainMap();
							break;
						case BinaryHeaderEnum.MethodCall:
						case BinaryHeaderEnum.MethodReturn:
							this.ReadMethodObject(binaryHeaderEnum);
							break;
						default:
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_BinaryHeader"), new object[] { b }));
						}
						break;
					}
					default:
						throw new SerializationException(Environment.GetResourceString("Serialization_TypeExpected"));
					}
					if (binaryHeaderEnum != BinaryHeaderEnum.Assembly)
					{
						bool flag2 = false;
						while (!flag2)
						{
							ObjectProgress objectProgress = (ObjectProgress)this.stack.Peek();
							if (objectProgress == null)
							{
								this.expectedType = BinaryTypeEnum.ObjectUrt;
								this.expectedTypeInformation = null;
								flag2 = true;
							}
							else
							{
								flag2 = objectProgress.GetNext(out objectProgress.expectedType, out objectProgress.expectedTypeInformation);
								this.expectedType = objectProgress.expectedType;
								this.expectedTypeInformation = objectProgress.expectedTypeInformation;
								if (!flag2)
								{
									this.prs.Init();
									if (objectProgress.memberValueEnum == InternalMemberValueE.Nested)
									{
										this.prs.PRparseTypeEnum = InternalParseTypeE.MemberEnd;
										this.prs.PRmemberTypeEnum = objectProgress.memberTypeEnum;
										this.prs.PRmemberValueEnum = objectProgress.memberValueEnum;
										this.objectReader.Parse(this.prs);
									}
									else
									{
										this.prs.PRparseTypeEnum = InternalParseTypeE.ObjectEnd;
										this.prs.PRmemberTypeEnum = objectProgress.memberTypeEnum;
										this.prs.PRmemberValueEnum = objectProgress.memberValueEnum;
										this.objectReader.Parse(this.prs);
									}
									this.stack.Pop();
									this.PutOp(objectProgress);
								}
							}
						}
					}
				}
			}
			catch (EndOfStreamException)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_StreamEnd"));
			}
		}

		// Token: 0x0600466E RID: 18030 RVA: 0x000F0D44 File Offset: 0x000EFD44
		internal void ReadBegin()
		{
		}

		// Token: 0x0600466F RID: 18031 RVA: 0x000F0D46 File Offset: 0x000EFD46
		internal void ReadEnd()
		{
		}

		// Token: 0x06004670 RID: 18032 RVA: 0x000F0D48 File Offset: 0x000EFD48
		internal bool ReadBoolean()
		{
			return this.dataReader.ReadBoolean();
		}

		// Token: 0x06004671 RID: 18033 RVA: 0x000F0D55 File Offset: 0x000EFD55
		internal byte ReadByte()
		{
			return this.dataReader.ReadByte();
		}

		// Token: 0x06004672 RID: 18034 RVA: 0x000F0D62 File Offset: 0x000EFD62
		internal byte[] ReadBytes(int length)
		{
			return this.dataReader.ReadBytes(length);
		}

		// Token: 0x06004673 RID: 18035 RVA: 0x000F0D70 File Offset: 0x000EFD70
		internal void ReadBytes(byte[] byteA, int offset, int size)
		{
			while (size > 0)
			{
				int num = this.dataReader.Read(byteA, offset, size);
				if (num == 0)
				{
					__Error.EndOfFile();
				}
				offset += num;
				size -= num;
			}
		}

		// Token: 0x06004674 RID: 18036 RVA: 0x000F0DA4 File Offset: 0x000EFDA4
		internal char ReadChar()
		{
			return this.dataReader.ReadChar();
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x000F0DB1 File Offset: 0x000EFDB1
		internal char[] ReadChars(int length)
		{
			return this.dataReader.ReadChars(length);
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x000F0DBF File Offset: 0x000EFDBF
		internal decimal ReadDecimal()
		{
			return decimal.Parse(this.dataReader.ReadString(), CultureInfo.InvariantCulture);
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x000F0DD6 File Offset: 0x000EFDD6
		internal float ReadSingle()
		{
			return this.dataReader.ReadSingle();
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x000F0DE3 File Offset: 0x000EFDE3
		internal double ReadDouble()
		{
			return this.dataReader.ReadDouble();
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x000F0DF0 File Offset: 0x000EFDF0
		internal short ReadInt16()
		{
			return this.dataReader.ReadInt16();
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x000F0DFD File Offset: 0x000EFDFD
		internal int ReadInt32()
		{
			return this.dataReader.ReadInt32();
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x000F0E0A File Offset: 0x000EFE0A
		internal long ReadInt64()
		{
			return this.dataReader.ReadInt64();
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x000F0E17 File Offset: 0x000EFE17
		internal sbyte ReadSByte()
		{
			return (sbyte)this.ReadByte();
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x000F0E20 File Offset: 0x000EFE20
		internal string ReadString()
		{
			return this.dataReader.ReadString();
		}

		// Token: 0x0600467E RID: 18046 RVA: 0x000F0E2D File Offset: 0x000EFE2D
		internal TimeSpan ReadTimeSpan()
		{
			return new TimeSpan(this.ReadInt64());
		}

		// Token: 0x0600467F RID: 18047 RVA: 0x000F0E3A File Offset: 0x000EFE3A
		internal DateTime ReadDateTime()
		{
			return DateTime.FromBinaryRaw(this.ReadInt64());
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x000F0E47 File Offset: 0x000EFE47
		internal ushort ReadUInt16()
		{
			return this.dataReader.ReadUInt16();
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x000F0E54 File Offset: 0x000EFE54
		internal uint ReadUInt32()
		{
			return this.dataReader.ReadUInt32();
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x000F0E61 File Offset: 0x000EFE61
		internal ulong ReadUInt64()
		{
			return this.dataReader.ReadUInt64();
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x000F0E70 File Offset: 0x000EFE70
		internal void ReadSerializationHeaderRecord()
		{
			SerializationHeaderRecord serializationHeaderRecord = new SerializationHeaderRecord();
			serializationHeaderRecord.Read(this);
			serializationHeaderRecord.Dump();
			this.topId = ((serializationHeaderRecord.topId > 0) ? this.objectReader.GetId((long)serializationHeaderRecord.topId) : ((long)serializationHeaderRecord.topId));
			this.headerId = ((serializationHeaderRecord.headerId > 0) ? this.objectReader.GetId((long)serializationHeaderRecord.headerId) : ((long)serializationHeaderRecord.headerId));
		}

		// Token: 0x06004684 RID: 18052 RVA: 0x000F0EE4 File Offset: 0x000EFEE4
		internal void ReadAssembly(BinaryHeaderEnum binaryHeaderEnum)
		{
			BinaryAssembly binaryAssembly = new BinaryAssembly();
			if (binaryHeaderEnum == BinaryHeaderEnum.CrossAppDomainAssembly)
			{
				BinaryCrossAppDomainAssembly binaryCrossAppDomainAssembly = new BinaryCrossAppDomainAssembly();
				binaryCrossAppDomainAssembly.Read(this);
				binaryCrossAppDomainAssembly.Dump();
				binaryAssembly.assemId = binaryCrossAppDomainAssembly.assemId;
				binaryAssembly.assemblyString = this.objectReader.CrossAppDomainArray(binaryCrossAppDomainAssembly.assemblyIndex) as string;
				if (binaryAssembly.assemblyString == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_CrossAppDomainError"), new object[] { "String", binaryCrossAppDomainAssembly.assemblyIndex }));
				}
			}
			else
			{
				binaryAssembly.Read(this);
				binaryAssembly.Dump();
			}
			this.AssemIdToAssemblyTable[binaryAssembly.assemId] = new BinaryAssemblyInfo(binaryAssembly.assemblyString);
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x000F0FA0 File Offset: 0x000EFFA0
		internal void ReadMethodObject(BinaryHeaderEnum binaryHeaderEnum)
		{
			if (binaryHeaderEnum == BinaryHeaderEnum.MethodCall)
			{
				BinaryMethodCall binaryMethodCall = new BinaryMethodCall();
				binaryMethodCall.Read(this);
				binaryMethodCall.Dump();
				this.objectReader.SetMethodCall(binaryMethodCall);
				return;
			}
			BinaryMethodReturn binaryMethodReturn = new BinaryMethodReturn();
			binaryMethodReturn.Read(this);
			binaryMethodReturn.Dump();
			this.objectReader.SetMethodReturn(binaryMethodReturn);
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x000F0FF4 File Offset: 0x000EFFF4
		private void ReadObject()
		{
			if (this.binaryObject == null)
			{
				this.binaryObject = new BinaryObject();
			}
			this.binaryObject.Read(this);
			this.binaryObject.Dump();
			ObjectMap objectMap = (ObjectMap)this.ObjectMapIdTable[this.binaryObject.mapId];
			if (objectMap == null)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Map"), new object[] { this.binaryObject.mapId }));
			}
			ObjectProgress op = this.GetOp();
			ParseRecord pr = op.pr;
			this.stack.Push(op);
			op.objectTypeEnum = InternalObjectTypeE.Object;
			op.binaryTypeEnumA = objectMap.binaryTypeEnumA;
			op.memberNames = objectMap.memberNames;
			op.memberTypes = objectMap.memberTypes;
			op.typeInformationA = objectMap.typeInformationA;
			op.memberLength = op.binaryTypeEnumA.Length;
			ObjectProgress objectProgress = (ObjectProgress)this.stack.PeekPeek();
			if (objectProgress == null || objectProgress.isInitial)
			{
				op.name = objectMap.objectName;
				pr.PRparseTypeEnum = InternalParseTypeE.Object;
				op.memberValueEnum = InternalMemberValueE.Empty;
			}
			else
			{
				pr.PRparseTypeEnum = InternalParseTypeE.Member;
				pr.PRmemberValueEnum = InternalMemberValueE.Nested;
				op.memberValueEnum = InternalMemberValueE.Nested;
				switch (objectProgress.objectTypeEnum)
				{
				case InternalObjectTypeE.Object:
					pr.PRname = objectProgress.name;
					pr.PRmemberTypeEnum = InternalMemberTypeE.Field;
					op.memberTypeEnum = InternalMemberTypeE.Field;
					break;
				case InternalObjectTypeE.Array:
					pr.PRmemberTypeEnum = InternalMemberTypeE.Item;
					op.memberTypeEnum = InternalMemberTypeE.Item;
					break;
				default:
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Map"), new object[] { objectProgress.objectTypeEnum.ToString() }));
				}
			}
			pr.PRobjectId = this.objectReader.GetId((long)this.binaryObject.objectId);
			pr.PRobjectInfo = objectMap.CreateObjectInfo(ref pr.PRsi, ref pr.PRmemberData);
			if (pr.PRobjectId == this.topId)
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Top;
			}
			pr.PRobjectTypeEnum = InternalObjectTypeE.Object;
			pr.PRkeyDt = objectMap.objectName;
			pr.PRdtType = objectMap.objectType;
			pr.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.objectReader.Parse(pr);
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x000F122C File Offset: 0x000F022C
		internal void ReadCrossAppDomainMap()
		{
			BinaryCrossAppDomainMap binaryCrossAppDomainMap = new BinaryCrossAppDomainMap();
			binaryCrossAppDomainMap.Read(this);
			binaryCrossAppDomainMap.Dump();
			object obj = this.objectReader.CrossAppDomainArray(binaryCrossAppDomainMap.crossAppDomainArrayIndex);
			BinaryObjectWithMap binaryObjectWithMap = obj as BinaryObjectWithMap;
			if (binaryObjectWithMap != null)
			{
				binaryObjectWithMap.Dump();
				this.ReadObjectWithMap(binaryObjectWithMap);
				return;
			}
			BinaryObjectWithMapTyped binaryObjectWithMapTyped = obj as BinaryObjectWithMapTyped;
			if (binaryObjectWithMapTyped != null)
			{
				this.ReadObjectWithMapTyped(binaryObjectWithMapTyped);
				return;
			}
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_CrossAppDomainError"), new object[] { "BinaryObjectMap", obj }));
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x000F12BC File Offset: 0x000F02BC
		internal void ReadObjectWithMap(BinaryHeaderEnum binaryHeaderEnum)
		{
			if (this.bowm == null)
			{
				this.bowm = new BinaryObjectWithMap(binaryHeaderEnum);
			}
			else
			{
				this.bowm.binaryHeaderEnum = binaryHeaderEnum;
			}
			this.bowm.Read(this);
			this.bowm.Dump();
			this.ReadObjectWithMap(this.bowm);
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x000F1310 File Offset: 0x000F0310
		private void ReadObjectWithMap(BinaryObjectWithMap record)
		{
			BinaryAssemblyInfo binaryAssemblyInfo = null;
			ObjectProgress op = this.GetOp();
			ParseRecord pr = op.pr;
			this.stack.Push(op);
			if (record.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapAssemId)
			{
				if (record.assemId < 1)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Assembly"), new object[] { record.name }));
				}
				binaryAssemblyInfo = (BinaryAssemblyInfo)this.AssemIdToAssemblyTable[record.assemId];
				if (binaryAssemblyInfo == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Assembly"), new object[] { record.assemId + " " + record.name }));
				}
			}
			else if (record.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMap)
			{
				binaryAssemblyInfo = this.SystemAssemblyInfo;
			}
			Type type = this.objectReader.GetType(binaryAssemblyInfo, record.name);
			ObjectMap objectMap = ObjectMap.Create(record.name, type, record.memberNames, this.objectReader, record.objectId, binaryAssemblyInfo);
			this.ObjectMapIdTable[record.objectId] = objectMap;
			op.objectTypeEnum = InternalObjectTypeE.Object;
			op.binaryTypeEnumA = objectMap.binaryTypeEnumA;
			op.typeInformationA = objectMap.typeInformationA;
			op.memberLength = op.binaryTypeEnumA.Length;
			op.memberNames = objectMap.memberNames;
			op.memberTypes = objectMap.memberTypes;
			ObjectProgress objectProgress = (ObjectProgress)this.stack.PeekPeek();
			if (objectProgress == null || objectProgress.isInitial)
			{
				op.name = record.name;
				pr.PRparseTypeEnum = InternalParseTypeE.Object;
				op.memberValueEnum = InternalMemberValueE.Empty;
			}
			else
			{
				pr.PRparseTypeEnum = InternalParseTypeE.Member;
				pr.PRmemberValueEnum = InternalMemberValueE.Nested;
				op.memberValueEnum = InternalMemberValueE.Nested;
				switch (objectProgress.objectTypeEnum)
				{
				case InternalObjectTypeE.Object:
					pr.PRname = objectProgress.name;
					pr.PRmemberTypeEnum = InternalMemberTypeE.Field;
					op.memberTypeEnum = InternalMemberTypeE.Field;
					break;
				case InternalObjectTypeE.Array:
					pr.PRmemberTypeEnum = InternalMemberTypeE.Item;
					op.memberTypeEnum = InternalMemberTypeE.Field;
					break;
				default:
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjectTypeEnum"), new object[] { objectProgress.objectTypeEnum.ToString() }));
				}
			}
			pr.PRobjectTypeEnum = InternalObjectTypeE.Object;
			pr.PRobjectId = this.objectReader.GetId((long)record.objectId);
			pr.PRobjectInfo = objectMap.CreateObjectInfo(ref pr.PRsi, ref pr.PRmemberData);
			if (pr.PRobjectId == this.topId)
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Top;
			}
			pr.PRkeyDt = record.name;
			pr.PRdtType = objectMap.objectType;
			pr.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.objectReader.Parse(pr);
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x000F15C7 File Offset: 0x000F05C7
		internal void ReadObjectWithMapTyped(BinaryHeaderEnum binaryHeaderEnum)
		{
			if (this.bowmt == null)
			{
				this.bowmt = new BinaryObjectWithMapTyped(binaryHeaderEnum);
			}
			else
			{
				this.bowmt.binaryHeaderEnum = binaryHeaderEnum;
			}
			this.bowmt.Read(this);
			this.ReadObjectWithMapTyped(this.bowmt);
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x000F1604 File Offset: 0x000F0604
		private void ReadObjectWithMapTyped(BinaryObjectWithMapTyped record)
		{
			BinaryAssemblyInfo binaryAssemblyInfo = null;
			ObjectProgress op = this.GetOp();
			ParseRecord pr = op.pr;
			this.stack.Push(op);
			if (record.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapTypedAssemId)
			{
				if (record.assemId < 1)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_AssemblyId"), new object[] { record.name }));
				}
				binaryAssemblyInfo = (BinaryAssemblyInfo)this.AssemIdToAssemblyTable[record.assemId];
				if (binaryAssemblyInfo == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_AssemblyId"), new object[] { record.assemId + " " + record.name }));
				}
			}
			else if (record.binaryHeaderEnum == BinaryHeaderEnum.ObjectWithMapTyped)
			{
				binaryAssemblyInfo = this.SystemAssemblyInfo;
			}
			ObjectMap objectMap = ObjectMap.Create(record.name, record.memberNames, record.binaryTypeEnumA, record.typeInformationA, record.memberAssemIds, this.objectReader, record.objectId, binaryAssemblyInfo, this.AssemIdToAssemblyTable);
			this.ObjectMapIdTable[record.objectId] = objectMap;
			op.objectTypeEnum = InternalObjectTypeE.Object;
			op.binaryTypeEnumA = objectMap.binaryTypeEnumA;
			op.typeInformationA = objectMap.typeInformationA;
			op.memberLength = op.binaryTypeEnumA.Length;
			op.memberNames = objectMap.memberNames;
			op.memberTypes = objectMap.memberTypes;
			ObjectProgress objectProgress = (ObjectProgress)this.stack.PeekPeek();
			if (objectProgress == null || objectProgress.isInitial)
			{
				op.name = record.name;
				pr.PRparseTypeEnum = InternalParseTypeE.Object;
				op.memberValueEnum = InternalMemberValueE.Empty;
			}
			else
			{
				pr.PRparseTypeEnum = InternalParseTypeE.Member;
				pr.PRmemberValueEnum = InternalMemberValueE.Nested;
				op.memberValueEnum = InternalMemberValueE.Nested;
				switch (objectProgress.objectTypeEnum)
				{
				case InternalObjectTypeE.Object:
					pr.PRname = objectProgress.name;
					pr.PRmemberTypeEnum = InternalMemberTypeE.Field;
					op.memberTypeEnum = InternalMemberTypeE.Field;
					break;
				case InternalObjectTypeE.Array:
					pr.PRmemberTypeEnum = InternalMemberTypeE.Item;
					op.memberTypeEnum = InternalMemberTypeE.Item;
					break;
				default:
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjectTypeEnum"), new object[] { objectProgress.objectTypeEnum.ToString() }));
				}
			}
			pr.PRobjectTypeEnum = InternalObjectTypeE.Object;
			pr.PRobjectInfo = objectMap.CreateObjectInfo(ref pr.PRsi, ref pr.PRmemberData);
			pr.PRobjectId = this.objectReader.GetId((long)record.objectId);
			if (pr.PRobjectId == this.topId)
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Top;
			}
			pr.PRkeyDt = record.name;
			pr.PRdtType = objectMap.objectType;
			pr.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.objectReader.Parse(pr);
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x000F18B8 File Offset: 0x000F08B8
		private void ReadObjectString(BinaryHeaderEnum binaryHeaderEnum)
		{
			if (this.objectString == null)
			{
				this.objectString = new BinaryObjectString();
			}
			if (binaryHeaderEnum == BinaryHeaderEnum.ObjectString)
			{
				this.objectString.Read(this);
				this.objectString.Dump();
			}
			else
			{
				if (this.crossAppDomainString == null)
				{
					this.crossAppDomainString = new BinaryCrossAppDomainString();
				}
				this.crossAppDomainString.Read(this);
				this.crossAppDomainString.Dump();
				this.objectString.value = this.objectReader.CrossAppDomainArray(this.crossAppDomainString.value) as string;
				if (this.objectString.value == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_CrossAppDomainError"), new object[]
					{
						"String",
						this.crossAppDomainString.value
					}));
				}
				this.objectString.objectId = this.crossAppDomainString.objectId;
			}
			this.prs.Init();
			this.prs.PRparseTypeEnum = InternalParseTypeE.Object;
			this.prs.PRobjectId = this.objectReader.GetId((long)this.objectString.objectId);
			if (this.prs.PRobjectId == this.topId)
			{
				this.prs.PRobjectPositionEnum = InternalObjectPositionE.Top;
			}
			this.prs.PRobjectTypeEnum = InternalObjectTypeE.Object;
			ObjectProgress objectProgress = (ObjectProgress)this.stack.Peek();
			this.prs.PRvalue = this.objectString.value;
			this.prs.PRkeyDt = "System.String";
			this.prs.PRdtType = Converter.typeofString;
			this.prs.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			this.prs.PRvarValue = this.objectString.value;
			if (objectProgress == null)
			{
				this.prs.PRparseTypeEnum = InternalParseTypeE.Object;
				this.prs.PRname = "System.String";
			}
			else
			{
				this.prs.PRparseTypeEnum = InternalParseTypeE.Member;
				this.prs.PRmemberValueEnum = InternalMemberValueE.InlineValue;
				switch (objectProgress.objectTypeEnum)
				{
				case InternalObjectTypeE.Object:
					this.prs.PRname = objectProgress.name;
					this.prs.PRmemberTypeEnum = InternalMemberTypeE.Field;
					break;
				case InternalObjectTypeE.Array:
					this.prs.PRmemberTypeEnum = InternalMemberTypeE.Item;
					break;
				default:
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjectTypeEnum"), new object[] { objectProgress.objectTypeEnum.ToString() }));
				}
			}
			this.objectReader.Parse(this.prs);
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x000F1B40 File Offset: 0x000F0B40
		private void ReadMemberPrimitiveTyped()
		{
			if (this.memberPrimitiveTyped == null)
			{
				this.memberPrimitiveTyped = new MemberPrimitiveTyped();
			}
			this.memberPrimitiveTyped.Read(this);
			this.memberPrimitiveTyped.Dump();
			this.prs.PRobjectTypeEnum = InternalObjectTypeE.Object;
			ObjectProgress objectProgress = (ObjectProgress)this.stack.Peek();
			this.prs.Init();
			this.prs.PRvarValue = this.memberPrimitiveTyped.value;
			this.prs.PRkeyDt = Converter.ToComType(this.memberPrimitiveTyped.primitiveTypeEnum);
			this.prs.PRdtType = Converter.ToType(this.memberPrimitiveTyped.primitiveTypeEnum);
			this.prs.PRdtTypeCode = this.memberPrimitiveTyped.primitiveTypeEnum;
			if (objectProgress == null)
			{
				this.prs.PRparseTypeEnum = InternalParseTypeE.Object;
				this.prs.PRname = "System.Variant";
			}
			else
			{
				this.prs.PRparseTypeEnum = InternalParseTypeE.Member;
				this.prs.PRmemberValueEnum = InternalMemberValueE.InlineValue;
				switch (objectProgress.objectTypeEnum)
				{
				case InternalObjectTypeE.Object:
					this.prs.PRname = objectProgress.name;
					this.prs.PRmemberTypeEnum = InternalMemberTypeE.Field;
					break;
				case InternalObjectTypeE.Array:
					this.prs.PRmemberTypeEnum = InternalMemberTypeE.Item;
					break;
				default:
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjectTypeEnum"), new object[] { objectProgress.objectTypeEnum.ToString() }));
				}
			}
			this.objectReader.Parse(this.prs);
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x000F1CCC File Offset: 0x000F0CCC
		private void ReadArray(BinaryHeaderEnum binaryHeaderEnum)
		{
			BinaryArray binaryArray = new BinaryArray(binaryHeaderEnum);
			binaryArray.Read(this);
			BinaryAssemblyInfo binaryAssemblyInfo;
			if (binaryArray.binaryTypeEnum == BinaryTypeEnum.ObjectUser)
			{
				if (binaryArray.assemId < 1)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_AssemblyId"), new object[] { binaryArray.typeInformation }));
				}
				binaryAssemblyInfo = (BinaryAssemblyInfo)this.AssemIdToAssemblyTable[binaryArray.assemId];
			}
			else
			{
				binaryAssemblyInfo = this.SystemAssemblyInfo;
			}
			ObjectProgress op = this.GetOp();
			ParseRecord pr = op.pr;
			op.objectTypeEnum = InternalObjectTypeE.Array;
			op.binaryTypeEnum = binaryArray.binaryTypeEnum;
			op.typeInformation = binaryArray.typeInformation;
			ObjectProgress objectProgress = (ObjectProgress)this.stack.PeekPeek();
			if (objectProgress == null || binaryArray.objectId > 0)
			{
				op.name = "System.Array";
				pr.PRparseTypeEnum = InternalParseTypeE.Object;
				op.memberValueEnum = InternalMemberValueE.Empty;
			}
			else
			{
				pr.PRparseTypeEnum = InternalParseTypeE.Member;
				pr.PRmemberValueEnum = InternalMemberValueE.Nested;
				op.memberValueEnum = InternalMemberValueE.Nested;
				switch (objectProgress.objectTypeEnum)
				{
				case InternalObjectTypeE.Object:
					pr.PRname = objectProgress.name;
					pr.PRmemberTypeEnum = InternalMemberTypeE.Field;
					op.memberTypeEnum = InternalMemberTypeE.Field;
					pr.PRkeyDt = objectProgress.name;
					pr.PRdtType = objectProgress.dtType;
					break;
				case InternalObjectTypeE.Array:
					pr.PRmemberTypeEnum = InternalMemberTypeE.Item;
					op.memberTypeEnum = InternalMemberTypeE.Item;
					break;
				default:
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ObjectTypeEnum"), new object[] { objectProgress.objectTypeEnum.ToString() }));
				}
			}
			pr.PRobjectId = this.objectReader.GetId((long)binaryArray.objectId);
			if (pr.PRobjectId == this.topId)
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Top;
			}
			else if (this.headerId > 0L && pr.PRobjectId == this.headerId)
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Headers;
			}
			else
			{
				pr.PRobjectPositionEnum = InternalObjectPositionE.Child;
			}
			pr.PRobjectTypeEnum = InternalObjectTypeE.Array;
			BinaryConverter.TypeFromInfo(binaryArray.binaryTypeEnum, binaryArray.typeInformation, this.objectReader, binaryAssemblyInfo, out pr.PRarrayElementTypeCode, out pr.PRarrayElementTypeString, out pr.PRarrayElementType, out pr.PRisArrayVariant);
			pr.PRdtTypeCode = InternalPrimitiveTypeE.Invalid;
			pr.PRrank = binaryArray.rank;
			pr.PRlengthA = binaryArray.lengthA;
			pr.PRlowerBoundA = binaryArray.lowerBoundA;
			bool flag = false;
			switch (binaryArray.binaryArrayTypeEnum)
			{
			case BinaryArrayTypeEnum.Single:
			case BinaryArrayTypeEnum.SingleOffset:
				op.numItems = binaryArray.lengthA[0];
				pr.PRarrayTypeEnum = InternalArrayTypeE.Single;
				if (Converter.IsWriteAsByteArray(pr.PRarrayElementTypeCode) && binaryArray.lowerBoundA[0] == 0)
				{
					flag = true;
					this.ReadArrayAsBytes(pr);
				}
				break;
			case BinaryArrayTypeEnum.Jagged:
			case BinaryArrayTypeEnum.JaggedOffset:
				op.numItems = binaryArray.lengthA[0];
				pr.PRarrayTypeEnum = InternalArrayTypeE.Jagged;
				break;
			case BinaryArrayTypeEnum.Rectangular:
			case BinaryArrayTypeEnum.RectangularOffset:
			{
				int num = 1;
				for (int i = 0; i < binaryArray.rank; i++)
				{
					num *= binaryArray.lengthA[i];
				}
				op.numItems = num;
				pr.PRarrayTypeEnum = InternalArrayTypeE.Rectangular;
				break;
			}
			default:
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_ArrayType"), new object[] { binaryArray.binaryArrayTypeEnum.ToString() }));
			}
			if (!flag)
			{
				this.stack.Push(op);
			}
			else
			{
				this.PutOp(op);
			}
			this.objectReader.Parse(pr);
			if (flag)
			{
				pr.PRparseTypeEnum = InternalParseTypeE.ObjectEnd;
				this.objectReader.Parse(pr);
			}
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x000F2050 File Offset: 0x000F1050
		private void ReadArrayAsBytes(ParseRecord pr)
		{
			if (pr.PRarrayElementTypeCode == InternalPrimitiveTypeE.Byte)
			{
				pr.PRnewObj = this.ReadBytes(pr.PRlengthA[0]);
				return;
			}
			if (pr.PRarrayElementTypeCode == InternalPrimitiveTypeE.Char)
			{
				pr.PRnewObj = this.ReadChars(pr.PRlengthA[0]);
				return;
			}
			int num = Converter.TypeLength(pr.PRarrayElementTypeCode);
			pr.PRnewObj = Converter.CreatePrimitiveArray(pr.PRarrayElementTypeCode, pr.PRlengthA[0]);
			Array array = (Array)pr.PRnewObj;
			int i = 0;
			if (this.byteBuffer == null)
			{
				this.byteBuffer = new byte[4096];
			}
			while (i < array.Length)
			{
				int num2 = Math.Min(4096 / num, array.Length - i);
				int num3 = num2 * num;
				this.ReadBytes(this.byteBuffer, 0, num3);
				Buffer.InternalBlockCopy(this.byteBuffer, 0, array, i * num, num3);
				i += num2;
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x000F2130 File Offset: 0x000F1130
		private void ReadMemberPrimitiveUnTyped()
		{
			ObjectProgress objectProgress = (ObjectProgress)this.stack.Peek();
			if (this.memberPrimitiveUnTyped == null)
			{
				this.memberPrimitiveUnTyped = new MemberPrimitiveUnTyped();
			}
			this.memberPrimitiveUnTyped.Set((InternalPrimitiveTypeE)this.expectedTypeInformation);
			this.memberPrimitiveUnTyped.Read(this);
			this.memberPrimitiveUnTyped.Dump();
			this.prs.Init();
			this.prs.PRvarValue = this.memberPrimitiveUnTyped.value;
			this.prs.PRdtTypeCode = (InternalPrimitiveTypeE)this.expectedTypeInformation;
			this.prs.PRdtType = Converter.ToType(this.prs.PRdtTypeCode);
			this.prs.PRparseTypeEnum = InternalParseTypeE.Member;
			this.prs.PRmemberValueEnum = InternalMemberValueE.InlineValue;
			if (objectProgress.objectTypeEnum == InternalObjectTypeE.Object)
			{
				this.prs.PRmemberTypeEnum = InternalMemberTypeE.Field;
				this.prs.PRname = objectProgress.name;
			}
			else
			{
				this.prs.PRmemberTypeEnum = InternalMemberTypeE.Item;
			}
			this.objectReader.Parse(this.prs);
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x000F2240 File Offset: 0x000F1240
		private void ReadMemberReference()
		{
			if (this.memberReference == null)
			{
				this.memberReference = new MemberReference();
			}
			this.memberReference.Read(this);
			this.memberReference.Dump();
			ObjectProgress objectProgress = (ObjectProgress)this.stack.Peek();
			this.prs.Init();
			this.prs.PRidRef = this.objectReader.GetId((long)this.memberReference.idRef);
			this.prs.PRparseTypeEnum = InternalParseTypeE.Member;
			this.prs.PRmemberValueEnum = InternalMemberValueE.Reference;
			if (objectProgress.objectTypeEnum == InternalObjectTypeE.Object)
			{
				this.prs.PRmemberTypeEnum = InternalMemberTypeE.Field;
				this.prs.PRname = objectProgress.name;
				this.prs.PRdtType = objectProgress.dtType;
			}
			else
			{
				this.prs.PRmemberTypeEnum = InternalMemberTypeE.Item;
			}
			this.objectReader.Parse(this.prs);
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x000F2324 File Offset: 0x000F1324
		private void ReadObjectNull(BinaryHeaderEnum binaryHeaderEnum)
		{
			if (this.objectNull == null)
			{
				this.objectNull = new ObjectNull();
			}
			this.objectNull.Read(this, binaryHeaderEnum);
			this.objectNull.Dump();
			ObjectProgress objectProgress = (ObjectProgress)this.stack.Peek();
			this.prs.Init();
			this.prs.PRparseTypeEnum = InternalParseTypeE.Member;
			this.prs.PRmemberValueEnum = InternalMemberValueE.Null;
			if (objectProgress.objectTypeEnum == InternalObjectTypeE.Object)
			{
				this.prs.PRmemberTypeEnum = InternalMemberTypeE.Field;
				this.prs.PRname = objectProgress.name;
				this.prs.PRdtType = objectProgress.dtType;
			}
			else
			{
				this.prs.PRmemberTypeEnum = InternalMemberTypeE.Item;
				this.prs.PRnullCount = this.objectNull.nullCount;
				objectProgress.ArrayCountIncrement(this.objectNull.nullCount - 1);
			}
			this.objectReader.Parse(this.prs);
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x000F2410 File Offset: 0x000F1410
		private void ReadMessageEnd()
		{
			if (__BinaryParser.messageEnd == null)
			{
				__BinaryParser.messageEnd = new MessageEnd();
			}
			__BinaryParser.messageEnd.Read(this);
			__BinaryParser.messageEnd.Dump();
			if (!this.stack.IsEmpty())
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_StreamEnd"));
			}
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x000F2460 File Offset: 0x000F1460
		internal object ReadValue(InternalPrimitiveTypeE code)
		{
			switch (code)
			{
			case InternalPrimitiveTypeE.Boolean:
				return this.ReadBoolean();
			case InternalPrimitiveTypeE.Byte:
				return this.ReadByte();
			case InternalPrimitiveTypeE.Char:
				return this.ReadChar();
			case InternalPrimitiveTypeE.Decimal:
				return this.ReadDecimal();
			case InternalPrimitiveTypeE.Double:
				return this.ReadDouble();
			case InternalPrimitiveTypeE.Int16:
				return this.ReadInt16();
			case InternalPrimitiveTypeE.Int32:
				return this.ReadInt32();
			case InternalPrimitiveTypeE.Int64:
				return this.ReadInt64();
			case InternalPrimitiveTypeE.SByte:
				return this.ReadSByte();
			case InternalPrimitiveTypeE.Single:
				return this.ReadSingle();
			case InternalPrimitiveTypeE.TimeSpan:
				return this.ReadTimeSpan();
			case InternalPrimitiveTypeE.DateTime:
				return this.ReadDateTime();
			case InternalPrimitiveTypeE.UInt16:
				return this.ReadUInt16();
			case InternalPrimitiveTypeE.UInt32:
				return this.ReadUInt32();
			case InternalPrimitiveTypeE.UInt64:
				return this.ReadUInt64();
			}
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_TypeCode"), new object[] { code.ToString() }));
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x000F25DC File Offset: 0x000F15DC
		private ObjectProgress GetOp()
		{
			ObjectProgress objectProgress;
			if (this.opPool != null && !this.opPool.IsEmpty())
			{
				objectProgress = (ObjectProgress)this.opPool.Pop();
				objectProgress.Init();
			}
			else
			{
				objectProgress = new ObjectProgress();
			}
			return objectProgress;
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x000F2620 File Offset: 0x000F1620
		private void PutOp(ObjectProgress op)
		{
			if (this.opPool == null)
			{
				this.opPool = new SerStack("opPool");
			}
			this.opPool.Push(op);
		}

		// Token: 0x04002369 RID: 9065
		private const int chunkSize = 4096;

		// Token: 0x0400236A RID: 9066
		internal ObjectReader objectReader;

		// Token: 0x0400236B RID: 9067
		internal Stream input;

		// Token: 0x0400236C RID: 9068
		internal long topId;

		// Token: 0x0400236D RID: 9069
		internal long headerId;

		// Token: 0x0400236E RID: 9070
		internal SizedArray objectMapIdTable;

		// Token: 0x0400236F RID: 9071
		internal SizedArray assemIdToAssemblyTable;

		// Token: 0x04002370 RID: 9072
		internal SerStack stack = new SerStack("ObjectProgressStack");

		// Token: 0x04002371 RID: 9073
		internal BinaryTypeEnum expectedType = BinaryTypeEnum.ObjectUrt;

		// Token: 0x04002372 RID: 9074
		internal object expectedTypeInformation;

		// Token: 0x04002373 RID: 9075
		internal ParseRecord PRS;

		// Token: 0x04002374 RID: 9076
		private BinaryAssemblyInfo systemAssemblyInfo;

		// Token: 0x04002375 RID: 9077
		private BinaryReader dataReader;

		// Token: 0x04002376 RID: 9078
		private static Encoding encoding = new UTF8Encoding(false, true);

		// Token: 0x04002377 RID: 9079
		private SerStack opPool;

		// Token: 0x04002378 RID: 9080
		private BinaryObject binaryObject;

		// Token: 0x04002379 RID: 9081
		private BinaryObjectWithMap bowm;

		// Token: 0x0400237A RID: 9082
		private BinaryObjectWithMapTyped bowmt;

		// Token: 0x0400237B RID: 9083
		internal BinaryObjectString objectString;

		// Token: 0x0400237C RID: 9084
		internal BinaryCrossAppDomainString crossAppDomainString;

		// Token: 0x0400237D RID: 9085
		internal MemberPrimitiveTyped memberPrimitiveTyped;

		// Token: 0x0400237E RID: 9086
		private byte[] byteBuffer;

		// Token: 0x0400237F RID: 9087
		internal MemberPrimitiveUnTyped memberPrimitiveUnTyped;

		// Token: 0x04002380 RID: 9088
		internal MemberReference memberReference;

		// Token: 0x04002381 RID: 9089
		internal ObjectNull objectNull;

		// Token: 0x04002382 RID: 9090
		internal static MessageEnd messageEnd;
	}
}
