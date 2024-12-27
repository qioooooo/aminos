using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000263 RID: 611
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class VirtualDirectoryMappingCollection : NameObjectCollectionBase
	{
		// Token: 0x06002035 RID: 8245 RVA: 0x0008CCAB File Offset: 0x0008BCAB
		public VirtualDirectoryMappingCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06002036 RID: 8246 RVA: 0x0008CCB8 File Offset: 0x0008BCB8
		public ICollection AllKeys
		{
			get
			{
				return base.BaseGetAllKeys();
			}
		}

		// Token: 0x170006F3 RID: 1779
		public VirtualDirectoryMapping this[string virtualDirectory]
		{
			get
			{
				virtualDirectory = VirtualDirectoryMappingCollection.ValidateVirtualDirectoryParameter(virtualDirectory);
				return this.Get(virtualDirectory);
			}
		}

		// Token: 0x170006F4 RID: 1780
		public VirtualDirectoryMapping this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x0008CCDA File Offset: 0x0008BCDA
		public void Add(string virtualDirectory, VirtualDirectoryMapping mapping)
		{
			virtualDirectory = VirtualDirectoryMappingCollection.ValidateVirtualDirectoryParameter(virtualDirectory);
			if (mapping == null)
			{
				throw new ArgumentNullException("mapping");
			}
			if (this.Get(virtualDirectory) != null)
			{
				throw ExceptionUtil.ParameterInvalid("virtualDirectory");
			}
			mapping.SetVirtualDirectory(VirtualPath.CreateAbsoluteAllowNull(virtualDirectory));
			base.BaseAdd(virtualDirectory, mapping);
		}

		// Token: 0x0600203A RID: 8250 RVA: 0x0008CD1A File Offset: 0x0008BD1A
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x0008CD24 File Offset: 0x0008BD24
		public void CopyTo(VirtualDirectoryMapping[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int count = this.Count;
			if (array.Length < count + index)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int i = 0;
			int num = index;
			while (i < count)
			{
				array[num] = this.Get(i);
				i++;
				num++;
			}
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x0008CD75 File Offset: 0x0008BD75
		public VirtualDirectoryMapping Get(int index)
		{
			return (VirtualDirectoryMapping)base.BaseGet(index);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x0008CD83 File Offset: 0x0008BD83
		public VirtualDirectoryMapping Get(string virtualDirectory)
		{
			virtualDirectory = VirtualDirectoryMappingCollection.ValidateVirtualDirectoryParameter(virtualDirectory);
			return (VirtualDirectoryMapping)base.BaseGet(virtualDirectory);
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x0008CD99 File Offset: 0x0008BD99
		public string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x0600203F RID: 8255 RVA: 0x0008CDA2 File Offset: 0x0008BDA2
		public void Remove(string virtualDirectory)
		{
			virtualDirectory = VirtualDirectoryMappingCollection.ValidateVirtualDirectoryParameter(virtualDirectory);
			base.BaseRemove(virtualDirectory);
		}

		// Token: 0x06002040 RID: 8256 RVA: 0x0008CDB3 File Offset: 0x0008BDB3
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x0008CDBC File Offset: 0x0008BDBC
		internal VirtualDirectoryMappingCollection Clone()
		{
			VirtualDirectoryMappingCollection virtualDirectoryMappingCollection = new VirtualDirectoryMappingCollection();
			for (int i = 0; i < this.Count; i++)
			{
				VirtualDirectoryMapping virtualDirectoryMapping = this[i];
				virtualDirectoryMappingCollection.Add(virtualDirectoryMapping.VirtualDirectory, virtualDirectoryMapping.Clone());
			}
			return virtualDirectoryMappingCollection;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x0008CDFC File Offset: 0x0008BDFC
		private static string ValidateVirtualDirectoryParameter(string virtualDirectory)
		{
			VirtualPath virtualPath = VirtualPath.CreateAbsoluteAllowNull(virtualDirectory);
			return VirtualPath.GetVirtualPathString(virtualPath);
		}
	}
}
