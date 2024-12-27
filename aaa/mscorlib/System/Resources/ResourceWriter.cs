using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace System.Resources
{
	// Token: 0x02000426 RID: 1062
	[ComVisible(true)]
	public sealed class ResourceWriter : IResourceWriter, IDisposable
	{
		// Token: 0x06002BF3 RID: 11251 RVA: 0x00095A04 File Offset: 0x00094A04
		public ResourceWriter(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this._output = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);
			this._resourceList = new Hashtable(1000, FastResourceComparer.Default);
			this._caseInsensitiveDups = new Hashtable(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06002BF4 RID: 11252 RVA: 0x00095A5C File Offset: 0x00094A5C
		public ResourceWriter(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (!stream.CanWrite)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StreamNotWritable"));
			}
			this._output = stream;
			this._resourceList = new Hashtable(1000, FastResourceComparer.Default);
			this._caseInsensitiveDups = new Hashtable(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06002BF5 RID: 11253 RVA: 0x00095AC4 File Offset: 0x00094AC4
		public void AddResource(string name, string value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			this._resourceList.Add(name, value);
		}

		// Token: 0x06002BF6 RID: 11254 RVA: 0x00095B14 File Offset: 0x00094B14
		public void AddResource(string name, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			this._resourceList.Add(name, value);
		}

		// Token: 0x06002BF7 RID: 11255 RVA: 0x00095B64 File Offset: 0x00094B64
		public void AddResource(string name, byte[] value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			this._resourceList.Add(name, value);
		}

		// Token: 0x06002BF8 RID: 11256 RVA: 0x00095BB4 File Offset: 0x00094BB4
		public void AddResourceData(string name, string typeName, byte[] serializedData)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (serializedData == null)
			{
				throw new ArgumentNullException("serializedData");
			}
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			this._caseInsensitiveDups.Add(name, null);
			if (this._preserializedData == null)
			{
				this._preserializedData = new Hashtable(FastResourceComparer.Default);
			}
			this._preserializedData.Add(name, new ResourceWriter.PrecannedResource(typeName, serializedData));
		}

		// Token: 0x06002BF9 RID: 11257 RVA: 0x00095C3B File Offset: 0x00094C3B
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002BFA RID: 11258 RVA: 0x00095C44 File Offset: 0x00094C44
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this._resourceList != null)
				{
					this.Generate();
				}
				if (this._output != null)
				{
					this._output.Close();
				}
			}
			this._output = null;
			this._caseInsensitiveDups = null;
		}

		// Token: 0x06002BFB RID: 11259 RVA: 0x00095C78 File Offset: 0x00094C78
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002BFC RID: 11260 RVA: 0x00095C84 File Offset: 0x00094C84
		public void Generate()
		{
			if (this._resourceList == null)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceWriterSaved"));
			}
			BinaryWriter binaryWriter = new BinaryWriter(this._output, Encoding.UTF8);
			List<string> list = new List<string>();
			binaryWriter.Write(ResourceManager.MagicNumber);
			binaryWriter.Write(ResourceManager.HeaderVersionNumber);
			MemoryStream memoryStream = new MemoryStream(240);
			BinaryWriter binaryWriter2 = new BinaryWriter(memoryStream);
			binaryWriter2.Write(typeof(ResourceReader).AssemblyQualifiedName);
			binaryWriter2.Write(ResourceManager.ResSetTypeName);
			binaryWriter2.Flush();
			binaryWriter.Write((int)memoryStream.Length);
			binaryWriter.Write(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
			binaryWriter.Write(2);
			int num = this._resourceList.Count;
			if (this._preserializedData != null)
			{
				num += this._preserializedData.Count;
			}
			binaryWriter.Write(num);
			int[] array = new int[num];
			int[] array2 = new int[num];
			int num2 = 0;
			MemoryStream memoryStream2 = new MemoryStream(num * 40);
			BinaryWriter binaryWriter3 = new BinaryWriter(memoryStream2, Encoding.Unicode);
			MemoryStream memoryStream3 = new MemoryStream(num * 40);
			BinaryWriter binaryWriter4 = new BinaryWriter(memoryStream3, Encoding.UTF8);
			IFormatter formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File | StreamingContextStates.Persistence));
			SortedList sortedList = new SortedList(this._resourceList, FastResourceComparer.Default);
			if (this._preserializedData != null)
			{
				foreach (object obj in this._preserializedData)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					sortedList.Add(dictionaryEntry.Key, dictionaryEntry.Value);
				}
			}
			IDictionaryEnumerator enumerator2 = sortedList.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				array[num2] = FastResourceComparer.HashFunction((string)enumerator2.Key);
				array2[num2++] = (int)binaryWriter3.Seek(0, SeekOrigin.Current);
				binaryWriter3.Write((string)enumerator2.Key);
				binaryWriter3.Write((int)binaryWriter4.Seek(0, SeekOrigin.Current));
				object value = enumerator2.Value;
				ResourceTypeCode resourceTypeCode = this.FindTypeCode(value, list);
				ResourceWriter.Write7BitEncodedInt(binaryWriter4, (int)resourceTypeCode);
				ResourceWriter.PrecannedResource precannedResource = value as ResourceWriter.PrecannedResource;
				if (precannedResource != null)
				{
					binaryWriter4.Write(precannedResource.Data);
				}
				else
				{
					this.WriteValue(resourceTypeCode, value, binaryWriter4, formatter);
				}
			}
			binaryWriter.Write(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				binaryWriter.Write(list[i]);
			}
			Array.Sort<int, int>(array, array2);
			binaryWriter.Flush();
			int num3 = (int)binaryWriter.BaseStream.Position & 7;
			if (num3 > 0)
			{
				for (int j = 0; j < 8 - num3; j++)
				{
					binaryWriter.Write("PAD"[j % 3]);
				}
			}
			foreach (int num4 in array)
			{
				binaryWriter.Write(num4);
			}
			foreach (int num5 in array2)
			{
				binaryWriter.Write(num5);
			}
			binaryWriter.Flush();
			binaryWriter3.Flush();
			binaryWriter4.Flush();
			int num6 = (int)(binaryWriter.Seek(0, SeekOrigin.Current) + memoryStream2.Length);
			num6 += 4;
			binaryWriter.Write(num6);
			binaryWriter.Write(memoryStream2.GetBuffer(), 0, (int)memoryStream2.Length);
			binaryWriter3.Close();
			binaryWriter.Write(memoryStream3.GetBuffer(), 0, (int)memoryStream3.Length);
			binaryWriter4.Close();
			binaryWriter.Flush();
			this._resourceList = null;
		}

		// Token: 0x06002BFD RID: 11261 RVA: 0x0009602C File Offset: 0x0009502C
		private ResourceTypeCode FindTypeCode(object value, List<string> types)
		{
			if (value == null)
			{
				return ResourceTypeCode.Null;
			}
			Type type = value.GetType();
			if (type == typeof(string))
			{
				return ResourceTypeCode.String;
			}
			if (type == typeof(int))
			{
				return ResourceTypeCode.Int32;
			}
			if (type == typeof(bool))
			{
				return ResourceTypeCode.Boolean;
			}
			if (type == typeof(char))
			{
				return ResourceTypeCode.Char;
			}
			if (type == typeof(byte))
			{
				return ResourceTypeCode.Byte;
			}
			if (type == typeof(sbyte))
			{
				return ResourceTypeCode.SByte;
			}
			if (type == typeof(short))
			{
				return ResourceTypeCode.Int16;
			}
			if (type == typeof(long))
			{
				return ResourceTypeCode.Int64;
			}
			if (type == typeof(ushort))
			{
				return ResourceTypeCode.UInt16;
			}
			if (type == typeof(uint))
			{
				return ResourceTypeCode.UInt32;
			}
			if (type == typeof(ulong))
			{
				return ResourceTypeCode.UInt64;
			}
			if (type == typeof(float))
			{
				return ResourceTypeCode.Single;
			}
			if (type == typeof(double))
			{
				return ResourceTypeCode.Double;
			}
			if (type == typeof(decimal))
			{
				return ResourceTypeCode.Decimal;
			}
			if (type == typeof(DateTime))
			{
				return ResourceTypeCode.DateTime;
			}
			if (type == typeof(TimeSpan))
			{
				return ResourceTypeCode.TimeSpan;
			}
			if (type == typeof(byte[]))
			{
				return ResourceTypeCode.ByteArray;
			}
			if (type == typeof(MemoryStream))
			{
				return ResourceTypeCode.Stream;
			}
			string text;
			if (type == typeof(ResourceWriter.PrecannedResource))
			{
				text = ((ResourceWriter.PrecannedResource)value).TypeName;
				if (text.StartsWith("ResourceTypeCode.", StringComparison.Ordinal))
				{
					text = text.Substring(17);
					return (ResourceTypeCode)Enum.Parse(typeof(ResourceTypeCode), text);
				}
			}
			else
			{
				text = type.AssemblyQualifiedName;
			}
			int num = types.IndexOf(text);
			if (num == -1)
			{
				num = types.Count;
				types.Add(text);
			}
			return num + ResourceTypeCode.StartOfUserTypes;
		}

		// Token: 0x06002BFE RID: 11262 RVA: 0x000961CC File Offset: 0x000951CC
		private void WriteValue(ResourceTypeCode typeCode, object value, BinaryWriter writer, IFormatter objFormatter)
		{
			switch (typeCode)
			{
			case ResourceTypeCode.Null:
				return;
			case ResourceTypeCode.String:
				writer.Write((string)value);
				return;
			case ResourceTypeCode.Boolean:
				writer.Write((bool)value);
				return;
			case ResourceTypeCode.Char:
				writer.Write((ushort)((char)value));
				return;
			case ResourceTypeCode.Byte:
				writer.Write((byte)value);
				return;
			case ResourceTypeCode.SByte:
				writer.Write((sbyte)value);
				return;
			case ResourceTypeCode.Int16:
				writer.Write((short)value);
				return;
			case ResourceTypeCode.UInt16:
				writer.Write((ushort)value);
				return;
			case ResourceTypeCode.Int32:
				writer.Write((int)value);
				return;
			case ResourceTypeCode.UInt32:
				writer.Write((uint)value);
				return;
			case ResourceTypeCode.Int64:
				writer.Write((long)value);
				return;
			case ResourceTypeCode.UInt64:
				writer.Write((ulong)value);
				return;
			case ResourceTypeCode.Single:
				writer.Write((float)value);
				return;
			case ResourceTypeCode.Double:
				writer.Write((double)value);
				return;
			case ResourceTypeCode.Decimal:
				writer.Write((decimal)value);
				return;
			case ResourceTypeCode.DateTime:
			{
				long num = ((DateTime)value).ToBinary();
				writer.Write(num);
				return;
			}
			case ResourceTypeCode.TimeSpan:
				writer.Write(((TimeSpan)value).Ticks);
				return;
			case ResourceTypeCode.ByteArray:
			{
				byte[] array = (byte[])value;
				writer.Write(array.Length);
				writer.Write(array, 0, array.Length);
				return;
			}
			case ResourceTypeCode.Stream:
			{
				MemoryStream memoryStream = (MemoryStream)value;
				if (memoryStream.Length > 2147483647L)
				{
					throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
				}
				int num2;
				int num3;
				memoryStream.InternalGetOriginAndLength(out num2, out num3);
				byte[] array2 = memoryStream.InternalGetBuffer();
				writer.Write(num3);
				writer.Write(array2, num2, num3);
				return;
			}
			}
			objFormatter.Serialize(writer.BaseStream, value);
		}

		// Token: 0x06002BFF RID: 11263 RVA: 0x000963CC File Offset: 0x000953CC
		private static void Write7BitEncodedInt(BinaryWriter store, int value)
		{
			uint num;
			for (num = (uint)value; num >= 128U; num >>= 7)
			{
				store.Write((byte)(num | 128U));
			}
			store.Write((byte)num);
		}

		// Token: 0x04001559 RID: 5465
		private const int _ExpectedNumberOfResources = 1000;

		// Token: 0x0400155A RID: 5466
		private const int AverageNameSize = 40;

		// Token: 0x0400155B RID: 5467
		private const int AverageValueSize = 40;

		// Token: 0x0400155C RID: 5468
		private Hashtable _resourceList;

		// Token: 0x0400155D RID: 5469
		private Stream _output;

		// Token: 0x0400155E RID: 5470
		private Hashtable _caseInsensitiveDups;

		// Token: 0x0400155F RID: 5471
		private Hashtable _preserializedData;

		// Token: 0x02000427 RID: 1063
		private class PrecannedResource
		{
			// Token: 0x06002C00 RID: 11264 RVA: 0x000963FF File Offset: 0x000953FF
			internal PrecannedResource(string typeName, byte[] data)
			{
				this.TypeName = typeName;
				this.Data = data;
			}

			// Token: 0x04001560 RID: 5472
			internal string TypeName;

			// Token: 0x04001561 RID: 5473
			internal byte[] Data;
		}
	}
}
