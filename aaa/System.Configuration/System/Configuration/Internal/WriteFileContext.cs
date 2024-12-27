using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using Microsoft.Win32;

namespace System.Configuration.Internal
{
	// Token: 0x020000C6 RID: 198
	internal class WriteFileContext
	{
		// Token: 0x06000776 RID: 1910 RVA: 0x0002028C File Offset: 0x0001F28C
		internal WriteFileContext(string filename, string templateFilename)
		{
			string directoryOrRootName = UrlPath.GetDirectoryOrRootName(filename);
			this._templateFilename = templateFilename;
			this._tempFiles = new TempFileCollection(directoryOrRootName);
			try
			{
				this._tempNewFilename = this._tempFiles.AddExtension("newcfg");
			}
			catch
			{
				((IDisposable)this._tempFiles).Dispose();
				this._tempFiles = null;
				throw;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x00020300 File Offset: 0x0001F300
		internal string TempNewFilename
		{
			get
			{
				return this._tempNewFilename;
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x00020308 File Offset: 0x0001F308
		internal void Complete(string filename, bool success)
		{
			try
			{
				if (success)
				{
					if (File.Exists(filename))
					{
						this.ValidateWriteAccess(filename);
						this.DuplicateFileAttributes(filename, this._tempNewFilename);
					}
					else if (this._templateFilename != null)
					{
						this.DuplicateTemplateAttributes(this._templateFilename, this._tempNewFilename);
					}
					this.ReplaceFile(this._tempNewFilename, filename);
					this._tempFiles.KeepFiles = true;
				}
			}
			finally
			{
				((IDisposable)this._tempFiles).Dispose();
				this._tempFiles = null;
			}
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x00020390 File Offset: 0x0001F390
		private void DuplicateFileAttributes(string source, string destination)
		{
			FileAttributes attributes = File.GetAttributes(source);
			File.SetAttributes(destination, attributes);
			DateTime creationTimeUtc = File.GetCreationTimeUtc(source);
			File.SetCreationTimeUtc(destination, creationTimeUtc);
			this.DuplicateTemplateAttributes(source, destination);
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x000203C4 File Offset: 0x0001F3C4
		private void DuplicateTemplateAttributes(string source, string destination)
		{
			if (this.IsWinNT)
			{
				FileSecurity accessControl = File.GetAccessControl(source, AccessControlSections.Access);
				accessControl.SetAccessRuleProtection(accessControl.AreAccessRulesProtected, true);
				File.SetAccessControl(destination, accessControl);
				return;
			}
			FileAttributes attributes = File.GetAttributes(source);
			File.SetAttributes(destination, attributes);
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00020404 File Offset: 0x0001F404
		private void ValidateWriteAccess(string filename)
		{
			FileStream fileStream = null;
			try
			{
				fileStream = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
			}
			catch (UnauthorizedAccessException)
			{
				throw;
			}
			catch (IOException)
			{
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00020468 File Offset: 0x0001F468
		private void ReplaceFile(string Source, string Target)
		{
			int num = 0;
			bool flag = this.AttemptMove(Source, Target);
			while (!flag && num < 10000 && File.Exists(Target) && !this.FileIsWriteLocked(Target))
			{
				Thread.Sleep(100);
				num += 100;
				flag = this.AttemptMove(Source, Target);
			}
			if (!flag)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_write_failed", new object[] { Target }));
			}
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000204D4 File Offset: 0x0001F4D4
		private bool AttemptMove(string Source, string Target)
		{
			bool flag = false;
			if (this.IsWinNT)
			{
				flag = UnsafeNativeMethods.MoveFileEx(Source, Target, 1);
			}
			else
			{
				try
				{
					File.Copy(Source, Target, true);
					flag = true;
				}
				catch
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00020518 File Offset: 0x0001F518
		private bool FileIsWriteLocked(string FileName)
		{
			Stream stream = null;
			bool flag = true;
			if (!FileUtil.FileExists(FileName, true))
			{
				return false;
			}
			try
			{
				FileShare fileShare = FileShare.Read;
				if (this.IsWinNT)
				{
					fileShare |= FileShare.Delete;
				}
				stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, fileShare);
				flag = false;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
					stream = null;
				}
			}
			return flag;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x00020570 File Offset: 0x0001F570
		private bool IsWinNT
		{
			get
			{
				if (!WriteFileContext._osPlatformDetermined)
				{
					WriteFileContext._osPlatform = Environment.OSVersion.Platform;
					WriteFileContext._osPlatformDetermined = true;
				}
				return WriteFileContext._osPlatform == PlatformID.Win32NT;
			}
		}

		// Token: 0x04000429 RID: 1065
		private const int SAVING_TIMEOUT = 10000;

		// Token: 0x0400042A RID: 1066
		private const int SAVING_RETRY_INTERVAL = 100;

		// Token: 0x0400042B RID: 1067
		private static bool _osPlatformDetermined = false;

		// Token: 0x0400042C RID: 1068
		private static PlatformID _osPlatform;

		// Token: 0x0400042D RID: 1069
		private TempFileCollection _tempFiles;

		// Token: 0x0400042E RID: 1070
		private string _tempNewFilename;

		// Token: 0x0400042F RID: 1071
		private string _templateFilename;
	}
}
