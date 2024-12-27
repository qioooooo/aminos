using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Principal;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001FB RID: 507
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class TempFileCollection : ICollection, IEnumerable, IDisposable
	{
		// Token: 0x06001140 RID: 4416 RVA: 0x000383DF File Offset: 0x000373DF
		public TempFileCollection()
			: this(null, false)
		{
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x000383E9 File Offset: 0x000373E9
		public TempFileCollection(string tempDir)
			: this(tempDir, false)
		{
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x000383F3 File Offset: 0x000373F3
		public TempFileCollection(string tempDir, bool keepFiles)
		{
			this.keepFiles = keepFiles;
			this.tempDir = tempDir;
			this.files = new Hashtable(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00038419 File Offset: 0x00037419
		void IDisposable.Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00038428 File Offset: 0x00037428
		protected virtual void Dispose(bool disposing)
		{
			this.Delete();
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00038430 File Offset: 0x00037430
		~TempFileCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00038460 File Offset: 0x00037460
		public string AddExtension(string fileExtension)
		{
			return this.AddExtension(fileExtension, this.keepFiles);
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x00038470 File Offset: 0x00037470
		public string AddExtension(string fileExtension, bool keepFile)
		{
			if (fileExtension == null || fileExtension.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "fileExtension" }), "fileExtension");
			}
			string text = this.BasePath + "." + fileExtension;
			this.AddFile(text, keepFile);
			return text;
		}

		// Token: 0x06001148 RID: 4424 RVA: 0x000384C8 File Offset: 0x000374C8
		public void AddFile(string fileName, bool keepFile)
		{
			if (fileName == null || fileName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "fileName" }), "fileName");
			}
			if (this.files[fileName] != null)
			{
				throw new ArgumentException(SR.GetString("DuplicateFileName", new object[] { fileName }), "fileName");
			}
			this.files.Add(fileName, keepFile);
		}

		// Token: 0x06001149 RID: 4425 RVA: 0x00038546 File Offset: 0x00037546
		public IEnumerator GetEnumerator()
		{
			return this.files.Keys.GetEnumerator();
		}

		// Token: 0x0600114A RID: 4426 RVA: 0x00038558 File Offset: 0x00037558
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.files.Keys.GetEnumerator();
		}

		// Token: 0x0600114B RID: 4427 RVA: 0x0003856A File Offset: 0x0003756A
		void ICollection.CopyTo(Array array, int start)
		{
			this.files.Keys.CopyTo(array, start);
		}

		// Token: 0x0600114C RID: 4428 RVA: 0x0003857E File Offset: 0x0003757E
		public void CopyTo(string[] fileNames, int start)
		{
			this.files.Keys.CopyTo(fileNames, start);
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x0600114D RID: 4429 RVA: 0x00038592 File Offset: 0x00037592
		public int Count
		{
			get
			{
				return this.files.Count;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600114E RID: 4430 RVA: 0x0003859F File Offset: 0x0003759F
		int ICollection.Count
		{
			get
			{
				return this.files.Count;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600114F RID: 4431 RVA: 0x000385AC File Offset: 0x000375AC
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06001150 RID: 4432 RVA: 0x000385AF File Offset: 0x000375AF
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001151 RID: 4433 RVA: 0x000385B2 File Offset: 0x000375B2
		public string TempDir
		{
			get
			{
				if (this.tempDir != null)
				{
					return this.tempDir;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x000385C8 File Offset: 0x000375C8
		public string BasePath
		{
			get
			{
				this.EnsureTempNameCreated();
				return this.basePath;
			}
		}

		// Token: 0x06001153 RID: 4435 RVA: 0x000385D8 File Offset: 0x000375D8
		private void EnsureTempNameCreated()
		{
			if (this.basePath == null)
			{
				string text = null;
				bool flag = false;
				int num = 5000;
				do
				{
					try
					{
						this.basePath = TempFileCollection.GetTempFileName(this.TempDir);
						string fullPath = this.basePath;
						new EnvironmentPermission(PermissionState.Unrestricted).Assert();
						try
						{
							fullPath = Path.GetFullPath(this.basePath);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						new FileIOPermission(FileIOPermissionAccess.AllAccess, fullPath).Demand();
						text = this.basePath + ".tmp";
						using (new FileStream(text, FileMode.CreateNew, FileAccess.Write))
						{
						}
						flag = true;
					}
					catch (IOException ex)
					{
						num--;
						if (num == 0 || Marshal.GetHRForException(ex) != 80)
						{
							throw;
						}
						flag = false;
					}
				}
				while (!flag);
				this.files.Add(text, this.keepFiles);
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x000386CC File Offset: 0x000376CC
		// (set) Token: 0x06001155 RID: 4437 RVA: 0x000386D4 File Offset: 0x000376D4
		public bool KeepFiles
		{
			get
			{
				return this.keepFiles;
			}
			set
			{
				this.keepFiles = value;
			}
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x000386E0 File Offset: 0x000376E0
		private bool KeepFile(string fileName)
		{
			object obj = this.files[fileName];
			return obj != null && (bool)obj;
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00038708 File Offset: 0x00037708
		public void Delete()
		{
			if (this.files != null)
			{
				string[] array = new string[this.files.Count];
				this.files.Keys.CopyTo(array, 0);
				foreach (string text in array)
				{
					if (!this.KeepFile(text))
					{
						this.Delete(text);
						this.files.Remove(text);
					}
				}
			}
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00038770 File Offset: 0x00037770
		internal void SafeDelete()
		{
			WindowsImpersonationContext windowsImpersonationContext = Executor.RevertImpersonation();
			try
			{
				this.Delete();
			}
			finally
			{
				Executor.ReImpersonate(windowsImpersonationContext);
			}
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x000387A4 File Offset: 0x000377A4
		private void Delete(string fileName)
		{
			try
			{
				File.Delete(fileName);
			}
			catch
			{
			}
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x000387CC File Offset: 0x000377CC
		private static string GetTempFileName(string tempDir)
		{
			if (tempDir == null || tempDir.Length == 0)
			{
				tempDir = Path.GetTempPath();
			}
			string text = TempFileCollection.GenerateRandomFileName();
			if (tempDir.EndsWith("\\", StringComparison.Ordinal))
			{
				return tempDir + text;
			}
			return tempDir + "\\" + text;
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00038814 File Offset: 0x00037814
		private static string GenerateRandomFileName()
		{
			byte[] array = new byte[6];
			lock (TempFileCollection.rng)
			{
				TempFileCollection.rng.GetBytes(array);
			}
			string text = Convert.ToBase64String(array).ToLower(CultureInfo.InvariantCulture);
			text = text.Replace('/', '-');
			return text.Replace('+', '_');
		}

		// Token: 0x04000FB1 RID: 4017
		private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

		// Token: 0x04000FB2 RID: 4018
		private string basePath;

		// Token: 0x04000FB3 RID: 4019
		private string tempDir;

		// Token: 0x04000FB4 RID: 4020
		private bool keepFiles;

		// Token: 0x04000FB5 RID: 4021
		private Hashtable files;
	}
}
