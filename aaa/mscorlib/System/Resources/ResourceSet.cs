using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Resources
{
	// Token: 0x02000424 RID: 1060
	[ComVisible(true)]
	[Serializable]
	public class ResourceSet : IDisposable, IEnumerable
	{
		// Token: 0x06002BE1 RID: 11233 RVA: 0x0009564D File Offset: 0x0009464D
		protected ResourceSet()
		{
			this.Table = new Hashtable(0);
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x00095661 File Offset: 0x00094661
		internal ResourceSet(bool junk)
		{
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x00095669 File Offset: 0x00094669
		public ResourceSet(string fileName)
		{
			this.Reader = new ResourceReader(fileName);
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x0009568E File Offset: 0x0009468E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public ResourceSet(Stream stream)
		{
			this.Reader = new ResourceReader(stream);
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000956B3 File Offset: 0x000946B3
		public ResourceSet(IResourceReader reader)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.Reader = reader;
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000956E1 File Offset: 0x000946E1
		public virtual void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000956EC File Offset: 0x000946EC
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				IResourceReader reader = this.Reader;
				this.Reader = null;
				if (reader != null)
				{
					reader.Close();
				}
			}
			this.Reader = null;
			this._caseInsensitiveTable = null;
			this.Table = null;
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00095728 File Offset: 0x00094728
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x00095731 File Offset: 0x00094731
		public virtual Type GetDefaultReader()
		{
			return typeof(ResourceReader);
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x0009573D File Offset: 0x0009473D
		public virtual Type GetDefaultWriter()
		{
			return typeof(ResourceWriter);
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x00095749 File Offset: 0x00094749
		[ComVisible(false)]
		public virtual IDictionaryEnumerator GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x00095751 File Offset: 0x00094751
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x0009575C File Offset: 0x0009475C
		private IDictionaryEnumerator GetEnumeratorHelper()
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			return table.GetEnumerator();
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x0009578C File Offset: 0x0009478C
		public virtual string GetString(string name)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			string text;
			try
			{
				text = (string)table[name];
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Name", new object[] { name }));
			}
			return text;
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x00095800 File Offset: 0x00094800
		public virtual string GetString(string name, bool ignoreCase)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			string text;
			try
			{
				text = (string)table[name];
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Name", new object[] { name }));
			}
			if (text != null || !ignoreCase)
			{
				return text;
			}
			Hashtable hashtable = this._caseInsensitiveTable;
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
				IDictionaryEnumerator enumerator = table.GetEnumerator();
				while (enumerator.MoveNext())
				{
					hashtable.Add(enumerator.Key, enumerator.Value);
				}
				this._caseInsensitiveTable = hashtable;
			}
			string text2;
			try
			{
				text2 = (string)hashtable[name];
			}
			catch (InvalidCastException)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_ResourceNotString_Name", new object[] { name }));
			}
			return text2;
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000958FC File Offset: 0x000948FC
		public virtual object GetObject(string name)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			return table[name];
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x0009593C File Offset: 0x0009493C
		public virtual object GetObject(string name, bool ignoreCase)
		{
			Hashtable table = this.Table;
			if (table == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			object obj = table[name];
			if (obj != null || !ignoreCase)
			{
				return obj;
			}
			Hashtable hashtable = this._caseInsensitiveTable;
			if (hashtable == null)
			{
				hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
				IDictionaryEnumerator enumerator = table.GetEnumerator();
				while (enumerator.MoveNext())
				{
					hashtable.Add(enumerator.Key, enumerator.Value);
				}
				this._caseInsensitiveTable = hashtable;
			}
			return hashtable[name];
		}

		// Token: 0x06002BF2 RID: 11250 RVA: 0x000959C8 File Offset: 0x000949C8
		protected virtual void ReadResources()
		{
			IDictionaryEnumerator enumerator = this.Reader.GetEnumerator();
			while (enumerator.MoveNext())
			{
				object value = enumerator.Value;
				this.Table.Add(enumerator.Key, value);
			}
		}

		// Token: 0x04001540 RID: 5440
		[NonSerialized]
		protected IResourceReader Reader;

		// Token: 0x04001541 RID: 5441
		protected Hashtable Table;

		// Token: 0x04001542 RID: 5442
		private Hashtable _caseInsensitiveTable;
	}
}
