using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;

namespace System.Resources
{
	// Token: 0x02000421 RID: 1057
	[ComVisible(true)]
	public sealed class ResourceReader : IResourceReader, IEnumerable, IDisposable
	{
		// Token: 0x06002BB6 RID: 11190 RVA: 0x00093D70 File Offset: 0x00092D70
		public ResourceReader(string fileName)
		{
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			this._store = new BinaryReader(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.UTF8);
			try
			{
				this.ReadResources();
			}
			catch
			{
				this._store.Close();
				throw;
			}
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x00093DD4 File Offset: 0x00092DD4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public ResourceReader(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanRead)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotReadable"));
			}
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			this._store = new BinaryReader(stream, Encoding.UTF8);
			this._ums = stream as UnmanagedMemoryStream;
			this.ReadResources();
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x00093E40 File Offset: 0x00092E40
		internal ResourceReader(Stream stream, Dictionary<string, ResourceLocator> resCache)
		{
			this._resCache = resCache;
			this._store = new BinaryReader(stream, Encoding.UTF8);
			this._ums = stream as UnmanagedMemoryStream;
			this.ReadResources();
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x00093E72 File Offset: 0x00092E72
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x00093E7B File Offset: 0x00092E7B
		void IDisposable.Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x00093E84 File Offset: 0x00092E84
		private void Dispose(bool disposing)
		{
			if (this._store != null)
			{
				this._resCache = null;
				if (disposing)
				{
					BinaryReader store = this._store;
					this._store = null;
					if (store != null)
					{
						store.Close();
					}
				}
				this._store = null;
				this._namePositions = null;
				this._nameHashes = null;
				this._ums = null;
				this._namePositionsPtr = null;
				this._nameHashesPtr = null;
			}
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x00093EE8 File Offset: 0x00092EE8
		internal unsafe static int ReadUnalignedI4(int* p)
		{
			return (int)(*(byte*)p) | ((int)((byte*)p)[1] << 8) | ((int)((byte*)p)[2] << 16) | ((int)((byte*)p)[3] << 24);
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x00093F13 File Offset: 0x00092F13
		private void SkipInt32()
		{
			this._store.BaseStream.Seek(4L, SeekOrigin.Current);
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x00093F2C File Offset: 0x00092F2C
		private void SkipString()
		{
			int num = this._store.Read7BitEncodedInt();
			this._store.BaseStream.Seek((long)num, SeekOrigin.Current);
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x00093F59 File Offset: 0x00092F59
		private int GetNameHash(int index)
		{
			if (this._ums == null)
			{
				return this._nameHashes[index];
			}
			return ResourceReader.ReadUnalignedI4(this._nameHashesPtr + index);
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x00093F80 File Offset: 0x00092F80
		private int GetNamePosition(int index)
		{
			int num;
			if (this._ums == null)
			{
				num = this._namePositions[index];
			}
			else
			{
				num = ResourceReader.ReadUnalignedI4(this._namePositionsPtr + index);
			}
			if (num < 0 || (long)num > this._dataSectionOffset - this._nameSectionOffset)
			{
				throw new FormatException(Environment.GetResourceString("BadImageFormat_ResourcesNameOutOfSection", new object[]
				{
					index,
					num.ToString("x", CultureInfo.InvariantCulture)
				}));
			}
			return num;
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00093FFD File Offset: 0x00092FFD
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00094005 File Offset: 0x00093005
		public IDictionaryEnumerator GetEnumerator()
		{
			if (this._resCache == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
			}
			return new ResourceReader.ResourceEnumerator(this);
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x00094025 File Offset: 0x00093025
		internal ResourceReader.ResourceEnumerator GetEnumeratorInternal()
		{
			return new ResourceReader.ResourceEnumerator(this);
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x00094030 File Offset: 0x00093030
		internal int FindPosForResource(string name)
		{
			int num = FastResourceComparer.HashFunction(name);
			int i = 0;
			int num2 = this._numResources - 1;
			int num3 = -1;
			bool flag = false;
			while (i <= num2)
			{
				num3 = i + num2 >> 1;
				int nameHash = this.GetNameHash(num3);
				int num4;
				if (nameHash == num)
				{
					num4 = 0;
				}
				else if (nameHash < num)
				{
					num4 = -1;
				}
				else
				{
					num4 = 1;
				}
				if (num4 == 0)
				{
					flag = true;
					break;
				}
				if (num4 < 0)
				{
					i = num3 + 1;
				}
				else
				{
					num2 = num3 - 1;
				}
			}
			if (!flag)
			{
				return -1;
			}
			if (i != num3)
			{
				i = num3;
				while (i > 0 && this.GetNameHash(i - 1) == num)
				{
					i--;
				}
			}
			if (num2 != num3)
			{
				num2 = num3;
				while (num2 < this._numResources && this.GetNameHash(num2 + 1) == num)
				{
					num2++;
				}
			}
			lock (this)
			{
				for (int j = i; j <= num2; j++)
				{
					this._store.BaseStream.Seek(this._nameSectionOffset + (long)this.GetNamePosition(j), SeekOrigin.Begin);
					if (this.CompareStringEqualsName(name))
					{
						return this._store.ReadInt32();
					}
				}
			}
			return -1;
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x00094150 File Offset: 0x00093150
		private unsafe bool CompareStringEqualsName(string name)
		{
			int num = this._store.Read7BitEncodedInt();
			if (this._ums == null)
			{
				byte[] array = new byte[num];
				int num2;
				for (int i = num; i > 0; i -= num2)
				{
					num2 = this._store.Read(array, num - i, i);
					if (num2 == 0)
					{
						throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourceNameCorrupted"));
					}
				}
				return FastResourceComparer.CompareOrdinal(array, num / 2, name) == 0;
			}
			byte* positionPointer = this._ums.PositionPointer;
			this._ums.Seek((long)num, SeekOrigin.Current);
			if (this._ums.Position > this._ums.Length)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesNameTooLong"));
			}
			return FastResourceComparer.CompareOrdinal(positionPointer, num, name) == 0;
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x00094208 File Offset: 0x00093208
		private unsafe string AllocateStringForNameIndex(int index, out int dataOffset)
		{
			long num = (long)this.GetNamePosition(index);
			int num2;
			byte[] array;
			lock (this)
			{
				this._store.BaseStream.Seek(num + this._nameSectionOffset, SeekOrigin.Begin);
				num2 = this._store.Read7BitEncodedInt();
				if (this._ums != null)
				{
					if (this._ums.Position > this._ums.Length - (long)num2)
					{
						throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesIndexTooLong", new object[] { index }));
					}
					char* positionPointer = (char*)this._ums.PositionPointer;
					string text = new string(positionPointer, 0, num2 / 2);
					this._ums.Position += (long)num2;
					dataOffset = this._store.ReadInt32();
					return text;
				}
				else
				{
					array = new byte[num2];
					int num3;
					for (int i = num2; i > 0; i -= num3)
					{
						num3 = this._store.Read(array, num2 - i, i);
						if (num3 == 0)
						{
							throw new EndOfStreamException(Environment.GetResourceString("BadImageFormat_ResourceNameCorrupted_NameIndex", new object[] { index }));
						}
					}
					dataOffset = this._store.ReadInt32();
				}
			}
			return Encoding.Unicode.GetString(array, 0, num2);
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x0009436C File Offset: 0x0009336C
		private object GetValueForNameIndex(int index)
		{
			long num = (long)this.GetNamePosition(index);
			object obj;
			lock (this)
			{
				this._store.BaseStream.Seek(num + this._nameSectionOffset, SeekOrigin.Begin);
				this.SkipString();
				int num2 = this._store.ReadInt32();
				if (this._version == 1)
				{
					obj = this.LoadObjectV1(num2);
				}
				else
				{
					ResourceTypeCode resourceTypeCode;
					obj = this.LoadObjectV2(num2, out resourceTypeCode);
				}
			}
			return obj;
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000943F0 File Offset: 0x000933F0
		internal string LoadString(int pos)
		{
			this._store.BaseStream.Seek(this._dataSectionOffset + (long)pos, SeekOrigin.Begin);
			string text = null;
			int num = this._store.Read7BitEncodedInt();
			if (this._version == 1)
			{
				if (num == -1)
				{
					return null;
				}
				if (this.FindType(num) != typeof(string))
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Type", new object[] { this.FindType(num).GetType().FullName }));
				}
				text = this._store.ReadString();
			}
			else
			{
				ResourceTypeCode resourceTypeCode = (ResourceTypeCode)num;
				if (resourceTypeCode != ResourceTypeCode.String && resourceTypeCode != ResourceTypeCode.Null)
				{
					string text2;
					if (resourceTypeCode < ResourceTypeCode.StartOfUserTypes)
					{
						text2 = resourceTypeCode.ToString();
					}
					else
					{
						text2 = this.FindType(resourceTypeCode - ResourceTypeCode.StartOfUserTypes).FullName;
					}
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Type", new object[] { text2 }));
				}
				if (resourceTypeCode == ResourceTypeCode.String)
				{
					text = this._store.ReadString();
				}
			}
			return text;
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000944E4 File Offset: 0x000934E4
		internal object LoadObject(int pos)
		{
			if (this._version == 1)
			{
				return this.LoadObjectV1(pos);
			}
			ResourceTypeCode resourceTypeCode;
			return this.LoadObjectV2(pos, out resourceTypeCode);
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x0009450C File Offset: 0x0009350C
		internal object LoadObject(int pos, out ResourceTypeCode typeCode)
		{
			if (this._version == 1)
			{
				object obj = this.LoadObjectV1(pos);
				typeCode = ((obj is string) ? ResourceTypeCode.String : ResourceTypeCode.StartOfUserTypes);
				return obj;
			}
			return this.LoadObjectV2(pos, out typeCode);
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x00094544 File Offset: 0x00093544
		internal object LoadObjectV1(int pos)
		{
			this._store.BaseStream.Seek(this._dataSectionOffset + (long)pos, SeekOrigin.Begin);
			int num = this._store.Read7BitEncodedInt();
			if (num == -1)
			{
				return null;
			}
			Type type = this.FindType(num);
			if (type == typeof(string))
			{
				return this._store.ReadString();
			}
			if (type == typeof(int))
			{
				return this._store.ReadInt32();
			}
			if (type == typeof(byte))
			{
				return this._store.ReadByte();
			}
			if (type == typeof(sbyte))
			{
				return this._store.ReadSByte();
			}
			if (type == typeof(short))
			{
				return this._store.ReadInt16();
			}
			if (type == typeof(long))
			{
				return this._store.ReadInt64();
			}
			if (type == typeof(ushort))
			{
				return this._store.ReadUInt16();
			}
			if (type == typeof(uint))
			{
				return this._store.ReadUInt32();
			}
			if (type == typeof(ulong))
			{
				return this._store.ReadUInt64();
			}
			if (type == typeof(float))
			{
				return this._store.ReadSingle();
			}
			if (type == typeof(double))
			{
				return this._store.ReadDouble();
			}
			if (type == typeof(DateTime))
			{
				return new DateTime(this._store.ReadInt64());
			}
			if (type == typeof(TimeSpan))
			{
				return new TimeSpan(this._store.ReadInt64());
			}
			if (type == typeof(decimal))
			{
				int[] array = new int[4];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = this._store.ReadInt32();
				}
				return new decimal(array);
			}
			return this.DeserializeObject(num);
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x00094754 File Offset: 0x00093754
		internal object LoadObjectV2(int pos, out ResourceTypeCode typeCode)
		{
			this._store.BaseStream.Seek(this._dataSectionOffset + (long)pos, SeekOrigin.Begin);
			typeCode = (ResourceTypeCode)this._store.Read7BitEncodedInt();
			switch (typeCode)
			{
			case ResourceTypeCode.Null:
				return null;
			case ResourceTypeCode.String:
				return this._store.ReadString();
			case ResourceTypeCode.Boolean:
				return this._store.ReadBoolean();
			case ResourceTypeCode.Char:
				return (char)this._store.ReadUInt16();
			case ResourceTypeCode.Byte:
				return this._store.ReadByte();
			case ResourceTypeCode.SByte:
				return this._store.ReadSByte();
			case ResourceTypeCode.Int16:
				return this._store.ReadInt16();
			case ResourceTypeCode.UInt16:
				return this._store.ReadUInt16();
			case ResourceTypeCode.Int32:
				return this._store.ReadInt32();
			case ResourceTypeCode.UInt32:
				return this._store.ReadUInt32();
			case ResourceTypeCode.Int64:
				return this._store.ReadInt64();
			case ResourceTypeCode.UInt64:
				return this._store.ReadUInt64();
			case ResourceTypeCode.Single:
				return this._store.ReadSingle();
			case ResourceTypeCode.Double:
				return this._store.ReadDouble();
			case ResourceTypeCode.Decimal:
				return this._store.ReadDecimal();
			case ResourceTypeCode.DateTime:
			{
				long num = this._store.ReadInt64();
				return DateTime.FromBinary(num);
			}
			case ResourceTypeCode.TimeSpan:
			{
				long num2 = this._store.ReadInt64();
				return new TimeSpan(num2);
			}
			case ResourceTypeCode.ByteArray:
			{
				int num3 = this._store.ReadInt32();
				if (this._ums == null)
				{
					return this._store.ReadBytes(num3);
				}
				if ((long)num3 > this._ums.Length - this._ums.Position)
				{
					throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourceDataTooLong"));
				}
				byte[] array = new byte[num3];
				this._ums.Read(array, 0, num3);
				return array;
			}
			case ResourceTypeCode.Stream:
			{
				int num4 = this._store.ReadInt32();
				if (this._ums == null)
				{
					byte[] array2 = this._store.ReadBytes(num4);
					return new PinnedBufferMemoryStream(array2);
				}
				if ((long)num4 > this._ums.Length - this._ums.Position)
				{
					throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourceDataTooLong"));
				}
				return new UnmanagedMemoryStream(this._ums.PositionPointer, (long)num4, (long)num4, FileAccess.Read, true);
			}
			}
			int num5 = typeCode - ResourceTypeCode.StartOfUserTypes;
			return this.DeserializeObject(num5);
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x00094A20 File Offset: 0x00093A20
		private object DeserializeObject(int typeIndex)
		{
			Type type = this.FindType(typeIndex);
			if (this._safeToDeserialize == null)
			{
				this.InitSafeToDeserializeArray();
			}
			object obj;
			if (this._safeToDeserialize[typeIndex])
			{
				this._objFormatter.Binder = this._typeLimitingBinder;
				this._typeLimitingBinder.ExpectingToDeserialize(type);
				obj = this._objFormatter.UnsafeDeserialize(this._store.BaseStream, null);
			}
			else
			{
				this._objFormatter.Binder = null;
				obj = this._objFormatter.Deserialize(this._store.BaseStream);
			}
			if (obj.GetType() != type)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResType&SerBlobMismatch", new object[]
				{
					type.FullName,
					obj.GetType().FullName
				}));
			}
			return obj;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x00094AE0 File Offset: 0x00093AE0
		private unsafe void ReadResources()
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence));
			this._typeLimitingBinder = new ResourceReader.TypeLimitingDeserializationBinder();
			binaryFormatter.Binder = this._typeLimitingBinder;
			this._objFormatter = binaryFormatter;
			try
			{
				int num = this._store.ReadInt32();
				if (num != ResourceManager.MagicNumber)
				{
					throw new ArgumentException(Environment.GetResourceString("Resources_StreamNotValid"));
				}
				int num2 = this._store.ReadInt32();
				if (num2 > 1)
				{
					int num3 = this._store.ReadInt32();
					this._store.BaseStream.Seek((long)num3, SeekOrigin.Current);
				}
				else
				{
					this.SkipInt32();
					string text = this._store.ReadString();
					AssemblyName assemblyName = new AssemblyName(ResourceManager.MscorlibName);
					if (!ResourceManager.CompareNames(text, ResourceManager.ResReaderTypeName, assemblyName))
					{
						throw new NotSupportedException(Environment.GetResourceString("NotSupported_WrongResourceReader_Type", new object[] { text }));
					}
					this.SkipString();
				}
				int num4 = this._store.ReadInt32();
				if (num4 != 2 && num4 != 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Arg_ResourceFileUnsupportedVersion", new object[] { 2, num4 }));
				}
				this._version = num4;
				this._numResources = this._store.ReadInt32();
				int num5 = this._store.ReadInt32();
				this._typeTable = new Type[num5];
				this._typeNamePositions = new int[num5];
				for (int i = 0; i < num5; i++)
				{
					this._typeNamePositions[i] = (int)this._store.BaseStream.Position;
					this.SkipString();
				}
				long position = this._store.BaseStream.Position;
				int num6 = (int)position & 7;
				if (num6 != 0)
				{
					for (int j = 0; j < 8 - num6; j++)
					{
						this._store.ReadByte();
					}
				}
				if (this._ums == null)
				{
					this._nameHashes = new int[this._numResources];
					for (int k = 0; k < this._numResources; k++)
					{
						this._nameHashes[k] = this._store.ReadInt32();
					}
				}
				else
				{
					this._nameHashesPtr = (int*)this._ums.PositionPointer;
					this._ums.Seek((long)(4 * this._numResources), SeekOrigin.Current);
					byte* positionPointer = this._ums.PositionPointer;
				}
				if (this._ums == null)
				{
					this._namePositions = new int[this._numResources];
					for (int l = 0; l < this._numResources; l++)
					{
						this._namePositions[l] = this._store.ReadInt32();
					}
				}
				else
				{
					this._namePositionsPtr = (int*)this._ums.PositionPointer;
					this._ums.Seek((long)(4 * this._numResources), SeekOrigin.Current);
					byte* positionPointer2 = this._ums.PositionPointer;
				}
				this._dataSectionOffset = (long)this._store.ReadInt32();
				this._nameSectionOffset = this._store.BaseStream.Position;
			}
			catch (EndOfStreamException ex)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesHeaderCorrupted"), ex);
			}
			catch (IndexOutOfRangeException ex2)
			{
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResourcesHeaderCorrupted"), ex2);
			}
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00094E2C File Offset: 0x00093E2C
		private Type FindType(int typeIndex)
		{
			if (this._typeTable[typeIndex] == null)
			{
				long position = this._store.BaseStream.Position;
				try
				{
					this._store.BaseStream.Position = (long)this._typeNamePositions[typeIndex];
					string text = this._store.ReadString();
					this._typeTable[typeIndex] = Type.GetType(text, true);
				}
				finally
				{
					this._store.BaseStream.Position = position;
				}
			}
			return this._typeTable[typeIndex];
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x00094EB4 File Offset: 0x00093EB4
		private void InitSafeToDeserializeArray()
		{
			this._safeToDeserialize = new bool[this._typeTable.Length];
			int i = 0;
			while (i < this._typeTable.Length)
			{
				long position = this._store.BaseStream.Position;
				string text;
				try
				{
					this._store.BaseStream.Position = (long)this._typeNamePositions[i];
					text = this._store.ReadString();
				}
				finally
				{
					this._store.BaseStream.Position = position;
				}
				Type type = Type.GetType(text, false);
				if (type == null)
				{
					AssemblyName assemblyName = null;
					string text2 = text;
					goto IL_00D0;
				}
				if (type.BaseType != typeof(Enum))
				{
					string text2 = type.FullName;
					AssemblyName assemblyName = new AssemblyName();
					Assembly assembly = type.Assembly;
					assemblyName.Init(assembly.nGetSimpleName(), assembly.nGetPublicKey(), null, null, assembly.GetLocale(), AssemblyHashAlgorithm.None, AssemblyVersionCompatibility.SameMachine, null, AssemblyNameFlags.PublicKey, null);
					goto IL_00D0;
				}
				this._safeToDeserialize[i] = true;
				IL_0106:
				i++;
				continue;
				IL_00D0:
				foreach (string text3 in ResourceReader.TypesSafeForDeserialization)
				{
					AssemblyName assemblyName;
					string text2;
					if (ResourceManager.CompareNames(text3, text2, assemblyName))
					{
						this._safeToDeserialize[i] = true;
					}
				}
				goto IL_0106;
			}
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x00094FEC File Offset: 0x00093FEC
		public void GetResourceData(string resourceName, out string resourceType, out byte[] resourceData)
		{
			if (resourceName == null)
			{
				throw new ArgumentNullException("resourceName");
			}
			if (this._resCache == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
			}
			int[] array = new int[this._numResources];
			int num = this.FindPosForResource(resourceName);
			if (num == -1)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_ResourceNameNotExist", new object[] { resourceName }));
			}
			lock (this)
			{
				for (int i = 0; i < this._numResources; i++)
				{
					this._store.BaseStream.Position = this._nameSectionOffset + (long)this.GetNamePosition(i);
					int num2 = this._store.Read7BitEncodedInt();
					this._store.BaseStream.Position += (long)num2;
					array[i] = this._store.ReadInt32();
				}
				Array.Sort<int>(array);
				int num3 = Array.BinarySearch<int>(array, num);
				long num4 = ((num3 < this._numResources - 1) ? ((long)array[num3 + 1] + this._dataSectionOffset) : this._store.BaseStream.Length);
				int num5 = (int)(num4 - ((long)num + this._dataSectionOffset));
				this._store.BaseStream.Position = this._dataSectionOffset + (long)num;
				ResourceTypeCode resourceTypeCode = (ResourceTypeCode)this._store.Read7BitEncodedInt();
				resourceType = this.TypeNameFromTypeCode(resourceTypeCode);
				num5 -= (int)(this._store.BaseStream.Position - (this._dataSectionOffset + (long)num));
				byte[] array2 = this._store.ReadBytes(num5);
				if (array2.Length != num5)
				{
					throw new FormatException(Environment.GetResourceString("BadImageFormat_ResourceNameCorrupted"));
				}
				resourceData = array2;
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x000951B0 File Offset: 0x000941B0
		private string TypeNameFromTypeCode(ResourceTypeCode typeCode)
		{
			if (typeCode < ResourceTypeCode.StartOfUserTypes)
			{
				return "ResourceTypeCode." + typeCode.ToString();
			}
			int num = typeCode - ResourceTypeCode.StartOfUserTypes;
			long position = this._store.BaseStream.Position;
			string text;
			try
			{
				this._store.BaseStream.Position = (long)this._typeNamePositions[num];
				text = this._store.ReadString();
			}
			finally
			{
				this._store.BaseStream.Position = position;
			}
			return text;
		}

		// Token: 0x04001527 RID: 5415
		private BinaryReader _store;

		// Token: 0x04001528 RID: 5416
		internal Dictionary<string, ResourceLocator> _resCache;

		// Token: 0x04001529 RID: 5417
		private long _nameSectionOffset;

		// Token: 0x0400152A RID: 5418
		private long _dataSectionOffset;

		// Token: 0x0400152B RID: 5419
		private int[] _nameHashes;

		// Token: 0x0400152C RID: 5420
		private unsafe int* _nameHashesPtr;

		// Token: 0x0400152D RID: 5421
		private int[] _namePositions;

		// Token: 0x0400152E RID: 5422
		private unsafe int* _namePositionsPtr;

		// Token: 0x0400152F RID: 5423
		private Type[] _typeTable;

		// Token: 0x04001530 RID: 5424
		private int[] _typeNamePositions;

		// Token: 0x04001531 RID: 5425
		private BinaryFormatter _objFormatter;

		// Token: 0x04001532 RID: 5426
		private int _numResources;

		// Token: 0x04001533 RID: 5427
		private UnmanagedMemoryStream _ums;

		// Token: 0x04001534 RID: 5428
		private int _version;

		// Token: 0x04001535 RID: 5429
		private bool[] _safeToDeserialize;

		// Token: 0x04001536 RID: 5430
		private ResourceReader.TypeLimitingDeserializationBinder _typeLimitingBinder;

		// Token: 0x04001537 RID: 5431
		private static readonly string[] TypesSafeForDeserialization = new string[]
		{
			"System.String[], mscorlib, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.DateTime[], mscorlib, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Bitmap, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Imaging.Metafile, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Point, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.PointF, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Size, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.SizeF, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Font, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Icon, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
			"System.Drawing.Color, System.Drawing, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Windows.Forms.Cursor, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Windows.Forms.Padding, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Windows.Forms.LinkArea, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089", "System.Windows.Forms.ImageListStreamer, System.Windows.Forms, Culture=neutral, PublicKeyToken=b77a5c561934e089"
		};

		// Token: 0x02000422 RID: 1058
		internal sealed class TypeLimitingDeserializationBinder : SerializationBinder
		{
			// Token: 0x17000823 RID: 2083
			// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000952D1 File Offset: 0x000942D1
			// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x000952D9 File Offset: 0x000942D9
			internal ObjectReader ObjectReader
			{
				get
				{
					return this._objectReader;
				}
				set
				{
					this._objectReader = value;
				}
			}

			// Token: 0x06002BD6 RID: 11222 RVA: 0x000952E2 File Offset: 0x000942E2
			internal void ExpectingToDeserialize(Type type)
			{
				this._typeToDeserialize = type;
			}

			// Token: 0x06002BD7 RID: 11223 RVA: 0x000952EC File Offset: 0x000942EC
			public override Type BindToType(string assemblyName, string typeName)
			{
				AssemblyName assemblyName2 = new AssemblyName();
				Assembly assembly = this._typeToDeserialize.Assembly;
				assemblyName2.Init(assembly.nGetSimpleName(), assembly.nGetPublicKey(), null, null, assembly.GetLocale(), AssemblyHashAlgorithm.None, AssemblyVersionCompatibility.SameMachine, null, AssemblyNameFlags.PublicKey, null);
				bool flag = false;
				foreach (string text in ResourceReader.TypesSafeForDeserialization)
				{
					if (ResourceManager.CompareNames(text, typeName, assemblyName2))
					{
						flag = true;
						break;
					}
				}
				Type type = this.ObjectReader.FastBindToType(assemblyName, typeName);
				if (type.IsEnum)
				{
					flag = true;
				}
				if (flag)
				{
					return null;
				}
				throw new BadImageFormatException(Environment.GetResourceString("BadImageFormat_ResType&SerBlobMismatch", new object[]
				{
					this._typeToDeserialize.FullName,
					typeName
				}));
			}

			// Token: 0x04001538 RID: 5432
			private Type _typeToDeserialize;

			// Token: 0x04001539 RID: 5433
			private ObjectReader _objectReader;
		}

		// Token: 0x02000423 RID: 1059
		internal sealed class ResourceEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x06002BD9 RID: 11225 RVA: 0x000953AE File Offset: 0x000943AE
			internal ResourceEnumerator(ResourceReader reader)
			{
				this._currentName = -1;
				this._reader = reader;
				this._dataPosition = -2;
			}

			// Token: 0x06002BDA RID: 11226 RVA: 0x000953CC File Offset: 0x000943CC
			public bool MoveNext()
			{
				if (this._currentName == this._reader._numResources - 1 || this._currentName == -2147483648)
				{
					this._currentIsValid = false;
					this._currentName = int.MinValue;
					return false;
				}
				this._currentIsValid = true;
				this._currentName++;
				return true;
			}

			// Token: 0x17000824 RID: 2084
			// (get) Token: 0x06002BDB RID: 11227 RVA: 0x00095428 File Offset: 0x00094428
			public object Key
			{
				get
				{
					if (this._currentName == -2147483648)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					if (!this._currentIsValid)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._reader._resCache == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
					}
					return this._reader.AllocateStringForNameIndex(this._currentName, out this._dataPosition);
				}
			}

			// Token: 0x17000825 RID: 2085
			// (get) Token: 0x06002BDC RID: 11228 RVA: 0x0009549E File Offset: 0x0009449E
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x17000826 RID: 2086
			// (get) Token: 0x06002BDD RID: 11229 RVA: 0x000954AB File Offset: 0x000944AB
			internal int DataPosition
			{
				get
				{
					return this._dataPosition;
				}
			}

			// Token: 0x17000827 RID: 2087
			// (get) Token: 0x06002BDE RID: 11230 RVA: 0x000954B4 File Offset: 0x000944B4
			public DictionaryEntry Entry
			{
				get
				{
					if (this._currentName == -2147483648)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					if (!this._currentIsValid)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._reader._resCache == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
					}
					object obj = null;
					string text;
					lock (this._reader._resCache)
					{
						text = this._reader.AllocateStringForNameIndex(this._currentName, out this._dataPosition);
						ResourceLocator resourceLocator;
						if (this._reader._resCache.TryGetValue(text, out resourceLocator))
						{
							obj = resourceLocator.Value;
						}
						if (obj == null)
						{
							if (this._dataPosition == -1)
							{
								obj = this._reader.GetValueForNameIndex(this._currentName);
							}
							else
							{
								obj = this._reader.LoadObject(this._dataPosition);
							}
						}
					}
					return new DictionaryEntry(text, obj);
				}
			}

			// Token: 0x17000828 RID: 2088
			// (get) Token: 0x06002BDF RID: 11231 RVA: 0x000955B0 File Offset: 0x000945B0
			public object Value
			{
				get
				{
					if (this._currentName == -2147483648)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
					}
					if (!this._currentIsValid)
					{
						throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
					}
					if (this._reader._resCache == null)
					{
						throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
					}
					return this._reader.GetValueForNameIndex(this._currentName);
				}
			}

			// Token: 0x06002BE0 RID: 11232 RVA: 0x00095620 File Offset: 0x00094620
			public void Reset()
			{
				if (this._reader._resCache == null)
				{
					throw new InvalidOperationException(Environment.GetResourceString("ResourceReaderIsClosed"));
				}
				this._currentIsValid = false;
				this._currentName = -1;
			}

			// Token: 0x0400153A RID: 5434
			private const int ENUM_DONE = -2147483648;

			// Token: 0x0400153B RID: 5435
			private const int ENUM_NOT_STARTED = -1;

			// Token: 0x0400153C RID: 5436
			private ResourceReader _reader;

			// Token: 0x0400153D RID: 5437
			private bool _currentIsValid;

			// Token: 0x0400153E RID: 5438
			private int _currentName;

			// Token: 0x0400153F RID: 5439
			private int _dataPosition;
		}
	}
}
