using System;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x0200001A RID: 26
	internal class DependentAssembly
	{
		// Token: 0x060000DB RID: 219 RVA: 0x00005AE8 File Offset: 0x00004AE8
		public DependentAssembly(ReferenceIdentity refId)
		{
			this._identity = refId;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005B04 File Offset: 0x00004B04
		public DependentAssembly(AssemblyReferenceEntry assemblyReferenceEntry)
		{
			AssemblyReferenceDependentAssemblyEntry dependentAssembly = assemblyReferenceEntry.DependentAssembly;
			this._size = dependentAssembly.Size;
			this._codebase = dependentAssembly.Codebase;
			this._group = dependentAssembly.Group;
			bool flag = false;
			ISection hashElements = dependentAssembly.HashElements;
			uint num = ((hashElements != null) ? hashElements.Count : 0U);
			if (num > 0U)
			{
				uint num2 = 0U;
				IHashElementEntry[] array = new IHashElementEntry[num];
				IEnumUnknown enumUnknown = (IEnumUnknown)hashElements._NewEnum;
				int num3 = enumUnknown.Next(num, array, ref num2);
				Marshal.ThrowExceptionForHR(num3);
				if (num2 != num)
				{
					throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_IsoEnumFetchNotEqualToCount"));
				}
				for (uint num4 = 0U; num4 < num; num4 += 1U)
				{
					HashElementEntry allData = array[(int)((UIntPtr)num4)].AllData;
					if (allData.DigestValueSize > 0U)
					{
						byte[] array2 = new byte[allData.DigestValueSize];
						Marshal.Copy(allData.DigestValue, array2, 0, (int)allData.DigestValueSize);
						this._hashCollection.AddHash(array2, (CMS_HASH_DIGESTMETHOD)allData.DigestMethod, (CMS_HASH_TRANSFORM)allData.Transform);
						flag = true;
					}
				}
			}
			if (!flag && dependentAssembly.HashValueSize > 0U)
			{
				byte[] array3 = new byte[dependentAssembly.HashValueSize];
				Marshal.Copy(dependentAssembly.HashValue, array3, 0, (int)dependentAssembly.HashValueSize);
				this._hashCollection.AddHash(array3, (CMS_HASH_DIGESTMETHOD)dependentAssembly.HashAlgorithm, CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_IDENTITY);
			}
			this._preRequisite = (dependentAssembly.Flags & 4U) != 0U;
			this._optional = (assemblyReferenceEntry.Flags & 1U) != 0U;
			this._visible = (dependentAssembly.Flags & 2U) != 0U;
			this._resourceFallbackCultureInternal = (dependentAssembly.Flags & 8U) != 0U;
			this._resourceFallbackCulture = dependentAssembly.ResourceFallbackCulture;
			this._description = dependentAssembly.Description;
			this._supportUrl = AssemblyManifest.UriFromMetadataEntry(dependentAssembly.SupportUrl, "Ex_DependencySupportUrlNotValid");
			IReferenceIdentity referenceIdentity = assemblyReferenceEntry.ReferenceIdentity;
			this._identity = new ReferenceIdentity(referenceIdentity);
			this._codebaseFS = UriHelper.NormalizePathDirectorySeparators(this._codebase);
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00005CFE File Offset: 0x00004CFE
		public ReferenceIdentity Identity
		{
			get
			{
				return this._identity;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00005D06 File Offset: 0x00004D06
		public string Codebase
		{
			get
			{
				return this._codebase;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00005D0E File Offset: 0x00004D0E
		public ulong Size
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005D16 File Offset: 0x00004D16
		public string Group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00005D1E File Offset: 0x00004D1E
		public string CodebaseFS
		{
			get
			{
				return this._codebaseFS;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005D26 File Offset: 0x00004D26
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00005D2E File Offset: 0x00004D2E
		public Uri SupportUrl
		{
			get
			{
				return this._supportUrl;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005D36 File Offset: 0x00004D36
		public string ResourceFallbackCulture
		{
			get
			{
				return this._resourceFallbackCulture;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005D3E File Offset: 0x00004D3E
		public bool IsPreRequisite
		{
			get
			{
				return this._preRequisite;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005D46 File Offset: 0x00004D46
		public bool IsOptional
		{
			get
			{
				return this._optional;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005D4E File Offset: 0x00004D4E
		public HashCollection HashCollection
		{
			get
			{
				return this._hashCollection;
			}
		}

		// Token: 0x04000082 RID: 130
		private readonly ulong _size;

		// Token: 0x04000083 RID: 131
		private readonly string _codebase;

		// Token: 0x04000084 RID: 132
		private readonly ReferenceIdentity _identity;

		// Token: 0x04000085 RID: 133
		private readonly string _group;

		// Token: 0x04000086 RID: 134
		private readonly string _codebaseFS;

		// Token: 0x04000087 RID: 135
		private readonly string _description;

		// Token: 0x04000088 RID: 136
		private readonly Uri _supportUrl;

		// Token: 0x04000089 RID: 137
		private readonly string _resourceFallbackCulture;

		// Token: 0x0400008A RID: 138
		private readonly bool _resourceFallbackCultureInternal;

		// Token: 0x0400008B RID: 139
		private readonly bool _optional;

		// Token: 0x0400008C RID: 140
		private readonly bool _visible;

		// Token: 0x0400008D RID: 141
		private readonly bool _preRequisite;

		// Token: 0x0400008E RID: 142
		private HashCollection _hashCollection = new HashCollection();
	}
}
