using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web.UI;

namespace System.Web.Util
{
	// Token: 0x0200076B RID: 1899
	internal class HashCodeCombiner
	{
		// Token: 0x06005C28 RID: 23592 RVA: 0x00171DBA File Offset: 0x00170DBA
		internal HashCodeCombiner()
		{
			this._combinedHash = 5381L;
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x00171DCE File Offset: 0x00170DCE
		internal HashCodeCombiner(long initialCombinedHash)
		{
			this._combinedHash = initialCombinedHash;
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x00171DDD File Offset: 0x00170DDD
		internal static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x00171DE6 File Offset: 0x00170DE6
		internal static int CombineHashCodes(int h1, int h2, int h3)
		{
			return HashCodeCombiner.CombineHashCodes(HashCodeCombiner.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x00171DF5 File Offset: 0x00170DF5
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return HashCodeCombiner.CombineHashCodes(HashCodeCombiner.CombineHashCodes(h1, h2), HashCodeCombiner.CombineHashCodes(h3, h4));
		}

		// Token: 0x06005C2D RID: 23597 RVA: 0x00171E0A File Offset: 0x00170E0A
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return HashCodeCombiner.CombineHashCodes(HashCodeCombiner.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		// Token: 0x06005C2E RID: 23598 RVA: 0x00171E1C File Offset: 0x00170E1C
		internal static string GetDirectoryHash(VirtualPath virtualDir)
		{
			HashCodeCombiner hashCodeCombiner = new HashCodeCombiner();
			hashCodeCombiner.AddDirectory(virtualDir.MapPathInternal());
			return hashCodeCombiner.CombinedHashString;
		}

		// Token: 0x06005C2F RID: 23599 RVA: 0x00171E44 File Offset: 0x00170E44
		internal void AddArray(string[] a)
		{
			if (a != null)
			{
				int num = a.Length;
				for (int i = 0; i < num; i++)
				{
					this.AddObject(a[i]);
				}
			}
		}

		// Token: 0x06005C30 RID: 23600 RVA: 0x00171E6D File Offset: 0x00170E6D
		internal void AddInt(int n)
		{
			this._combinedHash = ((this._combinedHash << 5) + this._combinedHash) ^ (long)n;
		}

		// Token: 0x06005C31 RID: 23601 RVA: 0x00171E87 File Offset: 0x00170E87
		internal void AddObject(int n)
		{
			this.AddInt(n);
		}

		// Token: 0x06005C32 RID: 23602 RVA: 0x00171E90 File Offset: 0x00170E90
		internal void AddObject(byte b)
		{
			this.AddInt(b.GetHashCode());
		}

		// Token: 0x06005C33 RID: 23603 RVA: 0x00171E9F File Offset: 0x00170E9F
		internal void AddObject(long l)
		{
			this.AddInt(l.GetHashCode());
		}

		// Token: 0x06005C34 RID: 23604 RVA: 0x00171EAE File Offset: 0x00170EAE
		internal void AddObject(bool b)
		{
			this.AddInt(b.GetHashCode());
		}

		// Token: 0x06005C35 RID: 23605 RVA: 0x00171EBD File Offset: 0x00170EBD
		internal void AddObject(string s)
		{
			if (s != null)
			{
				this.AddInt(StringUtil.GetStringHashCode(s));
			}
		}

		// Token: 0x06005C36 RID: 23606 RVA: 0x00171ECE File Offset: 0x00170ECE
		internal void AddObject(object o)
		{
			if (o != null)
			{
				this.AddInt(o.GetHashCode());
			}
		}

		// Token: 0x06005C37 RID: 23607 RVA: 0x00171EDF File Offset: 0x00170EDF
		internal void AddCaseInsensitiveString(string s)
		{
			if (s != null)
			{
				this.AddInt(StringComparer.InvariantCultureIgnoreCase.GetHashCode(s));
			}
		}

		// Token: 0x06005C38 RID: 23608 RVA: 0x00171EF5 File Offset: 0x00170EF5
		internal void AddDateTime(DateTime dt)
		{
			this.AddInt(dt.GetHashCode());
		}

		// Token: 0x06005C39 RID: 23609 RVA: 0x00171F0A File Offset: 0x00170F0A
		private void AddFileSize(long fileSize)
		{
			this.AddInt(fileSize.GetHashCode());
		}

		// Token: 0x06005C3A RID: 23610 RVA: 0x00171F19 File Offset: 0x00170F19
		internal void AddFile(string fileName)
		{
			if (!FileUtil.FileExists(fileName))
			{
				if (FileUtil.DirectoryExists(fileName))
				{
					this.AddDirectory(fileName);
				}
				return;
			}
			this.AddExistingFile(fileName);
		}

		// Token: 0x06005C3B RID: 23611 RVA: 0x00171F3C File Offset: 0x00170F3C
		private void AddExistingFile(string fileName)
		{
			this.AddInt(StringUtil.GetStringHashCode(fileName));
			FileInfo fileInfo = new FileInfo(fileName);
			this.AddDateTime(fileInfo.CreationTimeUtc);
			this.AddDateTime(fileInfo.LastWriteTimeUtc);
			this.AddFileSize(fileInfo.Length);
		}

		// Token: 0x06005C3C RID: 23612 RVA: 0x00171F80 File Offset: 0x00170F80
		internal void AddDirectory(string directoryName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
			if (!directoryInfo.Exists)
			{
				return;
			}
			this.AddObject(directoryName);
			foreach (object obj in ((IEnumerable)FileEnumerator.Create(directoryName)))
			{
				FileData fileData = (FileData)obj;
				if (fileData.IsDirectory)
				{
					this.AddDirectory(fileData.FullName);
				}
				else
				{
					this.AddExistingFile(fileData.FullName);
				}
			}
			this.AddDateTime(directoryInfo.CreationTimeUtc);
			this.AddDateTime(directoryInfo.LastWriteTimeUtc);
		}

		// Token: 0x06005C3D RID: 23613 RVA: 0x00172024 File Offset: 0x00171024
		internal void AddResourcesDirectory(string directoryName)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
			if (!directoryInfo.Exists)
			{
				return;
			}
			this.AddObject(directoryName);
			foreach (object obj in ((IEnumerable)FileEnumerator.Create(directoryName)))
			{
				FileData fileData = (FileData)obj;
				if (fileData.IsDirectory)
				{
					this.AddResourcesDirectory(fileData.FullName);
				}
				else
				{
					string fullName = fileData.FullName;
					if (Util.GetCultureName(fullName) == null)
					{
						this.AddExistingFile(fullName);
					}
				}
			}
			this.AddDateTime(directoryInfo.CreationTimeUtc);
		}

		// Token: 0x170017B9 RID: 6073
		// (get) Token: 0x06005C3E RID: 23614 RVA: 0x001720C8 File Offset: 0x001710C8
		internal long CombinedHash
		{
			get
			{
				return this._combinedHash;
			}
		}

		// Token: 0x170017BA RID: 6074
		// (get) Token: 0x06005C3F RID: 23615 RVA: 0x001720D0 File Offset: 0x001710D0
		internal int CombinedHash32
		{
			get
			{
				return this._combinedHash.GetHashCode();
			}
		}

		// Token: 0x170017BB RID: 6075
		// (get) Token: 0x06005C40 RID: 23616 RVA: 0x001720DD File Offset: 0x001710DD
		internal string CombinedHashString
		{
			get
			{
				return this._combinedHash.ToString("x", CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x04003148 RID: 12616
		private long _combinedHash;
	}
}
