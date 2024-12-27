using System;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x0200001C RID: 28
	internal class File
	{
		// Token: 0x060000EE RID: 238 RVA: 0x00005DCF File Offset: 0x00004DCF
		protected internal File(string name, ulong size)
		{
			this._name = name;
			this._size = size;
			this._nameFS = UriHelper.NormalizePathDirectorySeparators(this._name);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005E01 File Offset: 0x00004E01
		public File(string name, byte[] hash, ulong size)
		{
			this._name = name;
			this._hashCollection.AddHash(hash, CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA1, CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_IDENTITY);
			this._size = size;
			this._nameFS = UriHelper.NormalizePathDirectorySeparators(this._name);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00005E44 File Offset: 0x00004E44
		public File(FileEntry fileEntry)
		{
			this._name = fileEntry.Name;
			this._loadFrom = fileEntry.LoadFrom;
			this._size = fileEntry.Size;
			this._group = fileEntry.Group;
			this._optional = (fileEntry.Flags & 1U) != 0U;
			this._isData = (fileEntry.WritableType & 2U) != 0U;
			bool flag = false;
			ISection hashElements = fileEntry.HashElements;
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
			if (!flag && fileEntry.HashValueSize > 0U)
			{
				byte[] array3 = new byte[fileEntry.HashValueSize];
				Marshal.Copy(fileEntry.HashValue, array3, 0, (int)fileEntry.HashValueSize);
				this._hashCollection.AddHash(array3, (CMS_HASH_DIGESTMETHOD)fileEntry.HashAlgorithm, CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_IDENTITY);
			}
			this._nameFS = UriHelper.NormalizePathDirectorySeparators(this._name);
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00005FD6 File Offset: 0x00004FD6
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005FDE File Offset: 0x00004FDE
		public ulong Size
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005FE6 File Offset: 0x00004FE6
		public string Group
		{
			get
			{
				return this._group;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005FEE File Offset: 0x00004FEE
		public bool IsOptional
		{
			get
			{
				return this._optional;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00005FF6 File Offset: 0x00004FF6
		public bool IsData
		{
			get
			{
				return this._isData;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005FFE File Offset: 0x00004FFE
		public string NameFS
		{
			get
			{
				return this._nameFS;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00006006 File Offset: 0x00005006
		public HashCollection HashCollection
		{
			get
			{
				return this._hashCollection;
			}
		}

		// Token: 0x04000094 RID: 148
		private readonly string _name;

		// Token: 0x04000095 RID: 149
		private readonly string _loadFrom;

		// Token: 0x04000096 RID: 150
		private readonly ulong _size;

		// Token: 0x04000097 RID: 151
		private readonly string _group;

		// Token: 0x04000098 RID: 152
		private readonly bool _optional;

		// Token: 0x04000099 RID: 153
		private readonly bool _isData;

		// Token: 0x0400009A RID: 154
		private readonly string _nameFS;

		// Token: 0x0400009B RID: 155
		private HashCollection _hashCollection = new HashCollection();
	}
}
