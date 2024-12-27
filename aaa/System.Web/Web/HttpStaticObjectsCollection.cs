using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000092 RID: 146
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpStaticObjectsCollection : ICollection, IEnumerable
	{
		// Token: 0x060007A6 RID: 1958 RVA: 0x00022915 File Offset: 0x00021915
		internal void Add(string name, Type t, bool lateBound)
		{
			this._objects.Add(name, new HttpStaticObjectsEntry(name, t, lateBound));
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0002292B File Offset: 0x0002192B
		internal IDictionary Objects
		{
			get
			{
				return this._objects;
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00022934 File Offset: 0x00021934
		internal HttpStaticObjectsCollection Clone()
		{
			HttpStaticObjectsCollection httpStaticObjectsCollection = new HttpStaticObjectsCollection();
			IDictionaryEnumerator enumerator = this._objects.GetEnumerator();
			while (enumerator.MoveNext())
			{
				HttpStaticObjectsEntry httpStaticObjectsEntry = (HttpStaticObjectsEntry)enumerator.Value;
				httpStaticObjectsCollection.Add(httpStaticObjectsEntry.Name, httpStaticObjectsEntry.ObjectType, httpStaticObjectsEntry.LateBound);
			}
			return httpStaticObjectsCollection;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00022984 File Offset: 0x00021984
		internal int GetInstanceCount()
		{
			int num = 0;
			IDictionaryEnumerator enumerator = this._objects.GetEnumerator();
			while (enumerator.MoveNext())
			{
				HttpStaticObjectsEntry httpStaticObjectsEntry = (HttpStaticObjectsEntry)enumerator.Value;
				if (httpStaticObjectsEntry.HasInstance)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x000229C2 File Offset: 0x000219C2
		public bool NeverAccessed
		{
			get
			{
				return this.GetInstanceCount() == 0;
			}
		}

		// Token: 0x17000296 RID: 662
		public object this[string name]
		{
			get
			{
				HttpStaticObjectsEntry httpStaticObjectsEntry = (HttpStaticObjectsEntry)this._objects[name];
				if (httpStaticObjectsEntry == null)
				{
					return null;
				}
				return httpStaticObjectsEntry.Instance;
			}
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x000229FA File Offset: 0x000219FA
		public object GetObject(string name)
		{
			return this[name];
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x00022A03 File Offset: 0x00021A03
		public int Count
		{
			get
			{
				return this._objects.Count;
			}
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x00022A10 File Offset: 0x00021A10
		public IEnumerator GetEnumerator()
		{
			return new HttpStaticObjectsEnumerator(this._objects.GetEnumerator());
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x00022A24 File Offset: 0x00021A24
		public void CopyTo(Array array, int index)
		{
			foreach (object obj in this)
			{
				array.SetValue(obj, index++);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060007B0 RID: 1968 RVA: 0x00022A54 File Offset: 0x00021A54
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x00022A57 File Offset: 0x00021A57
		public bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00022A5A File Offset: 0x00021A5A
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x00022A60 File Offset: 0x00021A60
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(this.Count);
			IDictionaryEnumerator enumerator = this._objects.GetEnumerator();
			while (enumerator.MoveNext())
			{
				HttpStaticObjectsEntry httpStaticObjectsEntry = (HttpStaticObjectsEntry)enumerator.Value;
				writer.Write(httpStaticObjectsEntry.Name);
				bool hasInstance = httpStaticObjectsEntry.HasInstance;
				writer.Write(hasInstance);
				if (hasInstance)
				{
					AltSerialization.WriteValueToStream(httpStaticObjectsEntry.Instance, writer);
				}
				else
				{
					writer.Write(httpStaticObjectsEntry.ObjectType.FullName);
					writer.Write(httpStaticObjectsEntry.LateBound);
				}
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00022AE4 File Offset: 0x00021AE4
		public static HttpStaticObjectsCollection Deserialize(BinaryReader reader)
		{
			HttpStaticObjectsCollection httpStaticObjectsCollection = new HttpStaticObjectsCollection();
			int num = reader.ReadInt32();
			while (num-- > 0)
			{
				string text = reader.ReadString();
				bool flag = reader.ReadBoolean();
				HttpStaticObjectsEntry httpStaticObjectsEntry;
				if (flag)
				{
					object obj = AltSerialization.ReadValueFromStream(reader);
					httpStaticObjectsEntry = new HttpStaticObjectsEntry(text, obj, 0);
				}
				else
				{
					string text2 = reader.ReadString();
					bool flag2 = reader.ReadBoolean();
					httpStaticObjectsEntry = new HttpStaticObjectsEntry(text, Type.GetType(text2), flag2);
				}
				httpStaticObjectsCollection._objects.Add(text, httpStaticObjectsEntry);
			}
			return httpStaticObjectsCollection;
		}

		// Token: 0x04001160 RID: 4448
		private IDictionary _objects = new Hashtable(StringComparer.OrdinalIgnoreCase);
	}
}
