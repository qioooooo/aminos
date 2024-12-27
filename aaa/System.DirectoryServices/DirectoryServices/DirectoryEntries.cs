using System;
using System.Collections;
using System.DirectoryServices.Interop;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x0200001E RID: 30
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class DirectoryEntries : IEnumerable
	{
		// Token: 0x0600006A RID: 106 RVA: 0x00003015 File Offset: 0x00002015
		internal DirectoryEntries(DirectoryEntry parent)
		{
			this.container = parent;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00003024 File Offset: 0x00002024
		public SchemaNameCollection SchemaFilter
		{
			get
			{
				this.CheckIsContainer();
				SchemaNameCollection.FilterDelegateWrapper filterDelegateWrapper = new SchemaNameCollection.FilterDelegateWrapper(this.container.ContainerObject);
				return new SchemaNameCollection(filterDelegateWrapper.Getter, filterDelegateWrapper.Setter);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000305C File Offset: 0x0000205C
		private void CheckIsContainer()
		{
			if (!this.container.IsContainer)
			{
				throw new InvalidOperationException(Res.GetString("DSNotAContainer", new object[] { this.container.Path }));
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000309C File Offset: 0x0000209C
		public DirectoryEntry Add(string name, string schemaClassName)
		{
			this.CheckIsContainer();
			object obj = this.container.ContainerObject.Create(schemaClassName, name);
			return new DirectoryEntry(obj, this.container.UsePropertyCache, this.container.GetUsername(), this.container.GetPassword(), this.container.AuthenticationType)
			{
				JustCreated = true
			};
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000030FD File Offset: 0x000020FD
		public DirectoryEntry Find(string name)
		{
			return this.Find(name, null);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00003108 File Offset: 0x00002108
		public DirectoryEntry Find(string name, string schemaClassName)
		{
			this.CheckIsContainer();
			object obj = null;
			try
			{
				obj = this.container.ContainerObject.GetObject(schemaClassName, name);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
			return new DirectoryEntry(obj, this.container.UsePropertyCache, this.container.GetUsername(), this.container.GetPassword(), this.container.AuthenticationType);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000317C File Offset: 0x0000217C
		public void Remove(DirectoryEntry entry)
		{
			this.CheckIsContainer();
			try
			{
				this.container.ContainerObject.Delete(entry.SchemaClassName, entry.Name);
			}
			catch (COMException ex)
			{
				throw COMExceptionHelper.CreateFormattedComException(ex);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000031C8 File Offset: 0x000021C8
		public IEnumerator GetEnumerator()
		{
			return new DirectoryEntries.ChildEnumerator(this.container);
		}

		// Token: 0x04000169 RID: 361
		private DirectoryEntry container;

		// Token: 0x0200001F RID: 31
		private class ChildEnumerator : IEnumerator
		{
			// Token: 0x06000072 RID: 114 RVA: 0x000031D5 File Offset: 0x000021D5
			internal ChildEnumerator(DirectoryEntry container)
			{
				this.container = container;
				if (container.IsContainer)
				{
					this.enumVariant = new SafeNativeMethods.EnumVariant((SafeNativeMethods.IEnumVariant)container.ContainerObject._NewEnum);
				}
			}

			// Token: 0x1700000F RID: 15
			// (get) Token: 0x06000073 RID: 115 RVA: 0x00003208 File Offset: 0x00002208
			public DirectoryEntry Current
			{
				get
				{
					if (this.enumVariant == null)
					{
						throw new InvalidOperationException(Res.GetString("DSNoCurrentChild"));
					}
					if (this.currentEntry == null)
					{
						this.currentEntry = new DirectoryEntry(this.enumVariant.GetValue(), this.container.UsePropertyCache, this.container.GetUsername(), this.container.GetPassword(), this.container.AuthenticationType);
					}
					return this.currentEntry;
				}
			}

			// Token: 0x06000074 RID: 116 RVA: 0x0000327D File Offset: 0x0000227D
			public bool MoveNext()
			{
				if (this.enumVariant == null)
				{
					return false;
				}
				this.currentEntry = null;
				return this.enumVariant.GetNext();
			}

			// Token: 0x06000075 RID: 117 RVA: 0x0000329C File Offset: 0x0000229C
			public void Reset()
			{
				if (this.enumVariant != null)
				{
					try
					{
						this.enumVariant.Reset();
					}
					catch (NotImplementedException)
					{
						this.enumVariant = new SafeNativeMethods.EnumVariant((SafeNativeMethods.IEnumVariant)this.container.ContainerObject._NewEnum);
					}
					this.currentEntry = null;
				}
			}

			// Token: 0x17000010 RID: 16
			// (get) Token: 0x06000076 RID: 118 RVA: 0x000032F8 File Offset: 0x000022F8
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0400016A RID: 362
			private DirectoryEntry container;

			// Token: 0x0400016B RID: 363
			private SafeNativeMethods.EnumVariant enumVariant;

			// Token: 0x0400016C RID: 364
			private DirectoryEntry currentEntry;
		}
	}
}
