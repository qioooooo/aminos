using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x0200036F RID: 879
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SessionStateItemCollection : NameObjectCollectionBase, ISessionStateItemCollection, ICollection, IEnumerable
	{
		// Token: 0x06002AAC RID: 10924 RVA: 0x000BCBBE File Offset: 0x000BBBBE
		public SessionStateItemCollection()
			: base(Misc.CaseInsensitiveInvariantKeyComparer)
		{
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000BCBD8 File Offset: 0x000BBBD8
		static SessionStateItemCollection()
		{
			Type type = typeof(string);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(int);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(bool);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(DateTime);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(decimal);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(byte);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(char);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(float);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(double);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(sbyte);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(short);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(long);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(ushort);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(uint);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(ulong);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(TimeSpan);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(Guid);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(IntPtr);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
			type = typeof(UIntPtr);
			SessionStateItemCollection.s_immutableTypes.Add(type, type);
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x000BCDA6 File Offset: 0x000BBDA6
		internal static bool IsImmutable(object o)
		{
			return SessionStateItemCollection.s_immutableTypes[o.GetType()] != null;
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x000BCDC0 File Offset: 0x000BBDC0
		internal void DeserializeAllItems()
		{
			if (this._serializedItems == null)
			{
				return;
			}
			lock (this._serializedItemsLock)
			{
				for (int i = 0; i < this._serializedItems.Count; i++)
				{
					this.DeserializeItem(this._serializedItems.GetKey(i), false);
				}
			}
		}

		// Token: 0x06002AB0 RID: 10928 RVA: 0x000BCE28 File Offset: 0x000BBE28
		private void DeserializeItem(int index)
		{
			if (this._serializedItems == null)
			{
				return;
			}
			lock (this._serializedItemsLock)
			{
				if (index < this._serializedItems.Count)
				{
					this.DeserializeItem(this._serializedItems.GetKey(index), false);
				}
			}
		}

		// Token: 0x06002AB1 RID: 10929 RVA: 0x000BCE88 File Offset: 0x000BBE88
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
		private object ReadValueFromStreamWithAssert()
		{
			return AltSerialization.ReadValueFromStream(new BinaryReader(this._stream));
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x000BCE9C File Offset: 0x000BBE9C
		private void DeserializeItem(string name, bool check)
		{
			lock (this._serializedItemsLock)
			{
				if (check)
				{
					if (this._serializedItems == null)
					{
						return;
					}
					if (!this._serializedItems.ContainsKey(name))
					{
						return;
					}
				}
				SessionStateItemCollection.SerializedItemPosition serializedItemPosition = (SessionStateItemCollection.SerializedItemPosition)this._serializedItems[name];
				if (!serializedItemPosition.IsDeserialized)
				{
					this._stream.Seek((long)serializedItemPosition.Offset, SeekOrigin.Begin);
					if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
					{
						HttpRuntime.NamedPermissionSet.PermitOnly();
					}
					object obj = this.ReadValueFromStreamWithAssert();
					base.BaseSet(name, obj);
					serializedItemPosition.MarkDeserializedOffsetAndCheck();
				}
			}
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x000BCF4C File Offset: 0x000BBF4C
		private void MarkItemDeserialized(string name)
		{
			if (this._serializedItems == null)
			{
				return;
			}
			lock (this._serializedItemsLock)
			{
				if (this._serializedItems.ContainsKey(name))
				{
					((SessionStateItemCollection.SerializedItemPosition)this._serializedItems[name]).MarkDeserializedOffset();
				}
			}
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x000BCFAC File Offset: 0x000BBFAC
		private void MarkItemDeserialized(int index)
		{
			if (this._serializedItems == null)
			{
				return;
			}
			lock (this._serializedItemsLock)
			{
				if (index < this._serializedItems.Count)
				{
					((SessionStateItemCollection.SerializedItemPosition)this._serializedItems[index]).MarkDeserializedOffset();
				}
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06002AB5 RID: 10933 RVA: 0x000BD010 File Offset: 0x000BC010
		// (set) Token: 0x06002AB6 RID: 10934 RVA: 0x000BD018 File Offset: 0x000BC018
		public bool Dirty
		{
			get
			{
				return this._dirty;
			}
			set
			{
				this._dirty = value;
			}
		}

		// Token: 0x17000928 RID: 2344
		public object this[string name]
		{
			get
			{
				this.DeserializeItem(name, true);
				object obj = base.BaseGet(name);
				if (obj != null && !SessionStateItemCollection.IsImmutable(obj))
				{
					this._dirty = true;
				}
				return obj;
			}
			set
			{
				this.MarkItemDeserialized(name);
				base.BaseSet(name, value);
				this._dirty = true;
			}
		}

		// Token: 0x17000929 RID: 2345
		public object this[int index]
		{
			get
			{
				this.DeserializeItem(index);
				object obj = base.BaseGet(index);
				if (obj != null && !SessionStateItemCollection.IsImmutable(obj))
				{
					this._dirty = true;
				}
				return obj;
			}
			set
			{
				this.MarkItemDeserialized(index);
				base.BaseSet(index, value);
				this._dirty = true;
			}
		}

		// Token: 0x06002ABB RID: 10939 RVA: 0x000BD0B4 File Offset: 0x000BC0B4
		public void Remove(string name)
		{
			lock (this._serializedItemsLock)
			{
				if (this._serializedItems != null)
				{
					this._serializedItems.Remove(name);
				}
				base.BaseRemove(name);
				this._dirty = true;
			}
		}

		// Token: 0x06002ABC RID: 10940 RVA: 0x000BD10C File Offset: 0x000BC10C
		public void RemoveAt(int index)
		{
			lock (this._serializedItemsLock)
			{
				if (this._serializedItems != null && index < this._serializedItems.Count)
				{
					this._serializedItems.RemoveAt(index);
				}
				base.BaseRemoveAt(index);
				this._dirty = true;
			}
		}

		// Token: 0x06002ABD RID: 10941 RVA: 0x000BD170 File Offset: 0x000BC170
		public void Clear()
		{
			lock (this._serializedItemsLock)
			{
				if (this._serializedItems != null)
				{
					this._serializedItems.Clear();
				}
				base.BaseClear();
				this._dirty = true;
			}
		}

		// Token: 0x06002ABE RID: 10942 RVA: 0x000BD1C4 File Offset: 0x000BC1C4
		public override IEnumerator GetEnumerator()
		{
			this.DeserializeAllItems();
			return base.GetEnumerator();
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06002ABF RID: 10943 RVA: 0x000BD1D2 File Offset: 0x000BC1D2
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				this.DeserializeAllItems();
				return base.Keys;
			}
		}

		// Token: 0x06002AC0 RID: 10944 RVA: 0x000BD1E0 File Offset: 0x000BC1E0
		[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private void WriteValueToStreamWithAssert(object value, BinaryWriter writer)
		{
			AltSerialization.WriteValueToStream(value, writer);
		}

		// Token: 0x06002AC1 RID: 10945 RVA: 0x000BD1EC File Offset: 0x000BC1EC
		public void Serialize(BinaryWriter writer)
		{
			byte[] array = null;
			Stream baseStream = writer.BaseStream;
			if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
			{
				HttpRuntime.NamedPermissionSet.PermitOnly();
			}
			lock (this._serializedItemsLock)
			{
				int count = this.Count;
				writer.Write(count);
				if (count > 0)
				{
					if (base.BaseGet(null) != null)
					{
						for (int i = 0; i < count; i++)
						{
							if (base.BaseGetKey(i) == null)
							{
								writer.Write(i);
								break;
							}
						}
					}
					else
					{
						writer.Write(-1);
					}
					for (int i = 0; i < count; i++)
					{
						string text = base.BaseGetKey(i);
						if (text != null)
						{
							writer.Write(text);
						}
					}
					long position = baseStream.Position;
					baseStream.Seek((long)(4 * count), SeekOrigin.Current);
					long position2 = baseStream.Position;
					for (int i = 0; i < count; i++)
					{
						if (this._serializedItems != null && i < this._serializedItems.Count && !((SessionStateItemCollection.SerializedItemPosition)this._serializedItems[i]).IsDeserialized)
						{
							SessionStateItemCollection.SerializedItemPosition serializedItemPosition = (SessionStateItemCollection.SerializedItemPosition)this._serializedItems[i];
							this._stream.Seek((long)serializedItemPosition.Offset, SeekOrigin.Begin);
							if (array == null || array.Length < serializedItemPosition.DataLength)
							{
								array = new byte[serializedItemPosition.DataLength];
							}
							this._stream.Read(array, 0, serializedItemPosition.DataLength);
							baseStream.Write(array, 0, serializedItemPosition.DataLength);
						}
						else
						{
							object obj = base.BaseGet(i);
							this.WriteValueToStreamWithAssert(obj, writer);
						}
						long position3 = baseStream.Position;
						baseStream.Seek((long)(i * 4) + position, SeekOrigin.Begin);
						writer.Write((int)(position3 - position2));
						baseStream.Seek(position3, SeekOrigin.Begin);
					}
				}
			}
		}

		// Token: 0x06002AC2 RID: 10946 RVA: 0x000BD3D0 File Offset: 0x000BC3D0
		public static SessionStateItemCollection Deserialize(BinaryReader reader)
		{
			SessionStateItemCollection sessionStateItemCollection = new SessionStateItemCollection();
			int num = reader.ReadInt32();
			if (num > 0)
			{
				int num2 = reader.ReadInt32();
				sessionStateItemCollection._serializedItems = new SessionStateItemCollection.KeyedCollection(num);
				for (int i = 0; i < num; i++)
				{
					string text;
					if (i == num2)
					{
						text = null;
					}
					else
					{
						text = reader.ReadString();
					}
					sessionStateItemCollection.BaseSet(text, null);
				}
				int num3 = reader.ReadInt32();
				sessionStateItemCollection._serializedItems[sessionStateItemCollection.BaseGetKey(0)] = new SessionStateItemCollection.SerializedItemPosition(0, num3);
				for (int i = 1; i < num; i++)
				{
					int num4 = reader.ReadInt32();
					sessionStateItemCollection._serializedItems[sessionStateItemCollection.BaseGetKey(i)] = new SessionStateItemCollection.SerializedItemPosition(num3, num4 - num3);
					num3 = num4;
				}
				sessionStateItemCollection._iLastOffset = num3;
				byte[] array = new byte[sessionStateItemCollection._iLastOffset];
				int num5 = reader.BaseStream.Read(array, 0, sessionStateItemCollection._iLastOffset);
				if (num5 != sessionStateItemCollection._iLastOffset)
				{
					throw new HttpException(SR.GetString("Invalid_session_state"));
				}
				sessionStateItemCollection._stream = new MemoryStream(array);
			}
			sessionStateItemCollection._dirty = false;
			return sessionStateItemCollection;
		}

		// Token: 0x04001F74 RID: 8052
		private const int NO_NULL_KEY = -1;

		// Token: 0x04001F75 RID: 8053
		private const int SIZE_OF_INT32 = 4;

		// Token: 0x04001F76 RID: 8054
		private static Hashtable s_immutableTypes = new Hashtable(19);

		// Token: 0x04001F77 RID: 8055
		private bool _dirty;

		// Token: 0x04001F78 RID: 8056
		private SessionStateItemCollection.KeyedCollection _serializedItems;

		// Token: 0x04001F79 RID: 8057
		private Stream _stream;

		// Token: 0x04001F7A RID: 8058
		private int _iLastOffset;

		// Token: 0x04001F7B RID: 8059
		private object _serializedItemsLock = new object();

		// Token: 0x02000370 RID: 880
		private class KeyedCollection : NameObjectCollectionBase
		{
			// Token: 0x06002AC3 RID: 10947 RVA: 0x000BD4E6 File Offset: 0x000BC4E6
			internal KeyedCollection(int count)
				: base(count, Misc.CaseInsensitiveInvariantKeyComparer)
			{
			}

			// Token: 0x1700092B RID: 2347
			internal object this[string name]
			{
				get
				{
					return base.BaseGet(name);
				}
				set
				{
					if (base.BaseGet(name) == null && value == null)
					{
						return;
					}
					base.BaseSet(name, value);
				}
			}

			// Token: 0x1700092C RID: 2348
			internal object this[int index]
			{
				get
				{
					return base.BaseGet(index);
				}
			}

			// Token: 0x06002AC7 RID: 10951 RVA: 0x000BD52D File Offset: 0x000BC52D
			internal void Remove(string name)
			{
				base.BaseRemove(name);
			}

			// Token: 0x06002AC8 RID: 10952 RVA: 0x000BD536 File Offset: 0x000BC536
			internal void RemoveAt(int index)
			{
				base.BaseRemoveAt(index);
			}

			// Token: 0x06002AC9 RID: 10953 RVA: 0x000BD53F File Offset: 0x000BC53F
			internal void Clear()
			{
				base.BaseClear();
			}

			// Token: 0x06002ACA RID: 10954 RVA: 0x000BD547 File Offset: 0x000BC547
			internal string GetKey(int index)
			{
				return base.BaseGetKey(index);
			}

			// Token: 0x06002ACB RID: 10955 RVA: 0x000BD550 File Offset: 0x000BC550
			internal bool ContainsKey(string name)
			{
				return base.BaseGet(name) != null;
			}
		}

		// Token: 0x02000371 RID: 881
		private class SerializedItemPosition
		{
			// Token: 0x06002ACC RID: 10956 RVA: 0x000BD55F File Offset: 0x000BC55F
			internal SerializedItemPosition(int offset, int dataLength)
			{
				this._offset = offset;
				this._dataLength = dataLength;
			}

			// Token: 0x1700092D RID: 2349
			// (get) Token: 0x06002ACD RID: 10957 RVA: 0x000BD575 File Offset: 0x000BC575
			internal int Offset
			{
				get
				{
					return this._offset;
				}
			}

			// Token: 0x1700092E RID: 2350
			// (get) Token: 0x06002ACE RID: 10958 RVA: 0x000BD57D File Offset: 0x000BC57D
			internal int DataLength
			{
				get
				{
					return this._dataLength;
				}
			}

			// Token: 0x06002ACF RID: 10959 RVA: 0x000BD585 File Offset: 0x000BC585
			internal void MarkDeserializedOffset()
			{
				this._offset = -1;
			}

			// Token: 0x06002AD0 RID: 10960 RVA: 0x000BD58E File Offset: 0x000BC58E
			internal void MarkDeserializedOffsetAndCheck()
			{
				if (this._offset >= 0)
				{
					this.MarkDeserializedOffset();
				}
			}

			// Token: 0x1700092F RID: 2351
			// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000BD59F File Offset: 0x000BC59F
			internal bool IsDeserialized
			{
				get
				{
					return this._offset < 0;
				}
			}

			// Token: 0x04001F7C RID: 8060
			private int _offset;

			// Token: 0x04001F7D RID: 8061
			private int _dataLength;
		}
	}
}
