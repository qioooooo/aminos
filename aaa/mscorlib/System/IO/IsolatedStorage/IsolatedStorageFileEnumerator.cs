using System;
using System.Collections;
using System.Security.Permissions;
using System.Text;

namespace System.IO.IsolatedStorage
{
	// Token: 0x02000797 RID: 1943
	internal sealed class IsolatedStorageFileEnumerator : IEnumerator
	{
		// Token: 0x060045E3 RID: 17891 RVA: 0x000EF652 File Offset: 0x000EE652
		internal IsolatedStorageFileEnumerator(IsolatedStorageScope scope)
		{
			this.m_Scope = scope;
			this.m_fiop = IsolatedStorageFile.GetGlobalFileIOPerm(scope);
			this.m_rootDir = IsolatedStorageFile.GetRootDir(scope);
			this.m_fileEnum = new TwoLevelFileEnumerator(this.m_rootDir);
			this.Reset();
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x000EF690 File Offset: 0x000EE690
		public bool MoveNext()
		{
			this.m_fiop.Assert();
			this.m_fReset = false;
			while (this.m_fileEnum.MoveNext())
			{
				IsolatedStorageFile isolatedStorageFile = new IsolatedStorageFile();
				TwoPaths twoPaths = (TwoPaths)this.m_fileEnum.Current;
				bool flag = false;
				if (IsolatedStorageFile.NotAssemFilesDir(twoPaths.Path2) && IsolatedStorageFile.NotAppFilesDir(twoPaths.Path2))
				{
					flag = true;
				}
				Stream stream = null;
				Stream stream2 = null;
				Stream stream3 = null;
				IsolatedStorageScope isolatedStorageScope;
				string text;
				string text2;
				string text3;
				if (flag)
				{
					if (!this.GetIDStream(twoPaths.Path1, out stream) || !this.GetIDStream(twoPaths.Path1 + '\\' + twoPaths.Path2, out stream2))
					{
						continue;
					}
					stream.Position = 0L;
					if (IsolatedStorage.IsRoaming(this.m_Scope))
					{
						isolatedStorageScope = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming;
					}
					else if (IsolatedStorage.IsMachine(this.m_Scope))
					{
						isolatedStorageScope = IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;
					}
					else
					{
						isolatedStorageScope = IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly;
					}
					text = twoPaths.Path1;
					text2 = twoPaths.Path2;
					text3 = null;
				}
				else if (IsolatedStorageFile.NotAppFilesDir(twoPaths.Path2))
				{
					if (!this.GetIDStream(twoPaths.Path1, out stream2))
					{
						continue;
					}
					if (IsolatedStorage.IsRoaming(this.m_Scope))
					{
						isolatedStorageScope = IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Roaming;
					}
					else if (IsolatedStorage.IsMachine(this.m_Scope))
					{
						isolatedStorageScope = IsolatedStorageScope.Assembly | IsolatedStorageScope.Machine;
					}
					else
					{
						isolatedStorageScope = IsolatedStorageScope.User | IsolatedStorageScope.Assembly;
					}
					text = null;
					text2 = twoPaths.Path1;
					text3 = null;
					stream2.Position = 0L;
				}
				else
				{
					if (!this.GetIDStream(twoPaths.Path1, out stream3))
					{
						continue;
					}
					if (IsolatedStorage.IsRoaming(this.m_Scope))
					{
						isolatedStorageScope = IsolatedStorageScope.User | IsolatedStorageScope.Roaming | IsolatedStorageScope.Application;
					}
					else if (IsolatedStorage.IsMachine(this.m_Scope))
					{
						isolatedStorageScope = IsolatedStorageScope.Machine | IsolatedStorageScope.Application;
					}
					else
					{
						isolatedStorageScope = IsolatedStorageScope.User | IsolatedStorageScope.Application;
					}
					text = null;
					text2 = null;
					text3 = twoPaths.Path1;
					stream3.Position = 0L;
				}
				if (isolatedStorageFile.InitStore(isolatedStorageScope, stream, stream2, stream3, text, text2, text3) && isolatedStorageFile.InitExistingStore(isolatedStorageScope))
				{
					this.m_Current = isolatedStorageFile;
					return true;
				}
			}
			this.m_fEnd = true;
			return false;
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x060045E5 RID: 17893 RVA: 0x000EF863 File Offset: 0x000EE863
		public object Current
		{
			get
			{
				if (this.m_fReset)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
				}
				if (this.m_fEnd)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
				}
				return this.m_Current;
			}
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x000EF89B File Offset: 0x000EE89B
		public void Reset()
		{
			this.m_Current = null;
			this.m_fReset = true;
			this.m_fEnd = false;
			this.m_fileEnum.Reset();
		}

		// Token: 0x060045E7 RID: 17895 RVA: 0x000EF8C0 File Offset: 0x000EE8C0
		private bool GetIDStream(string path, out Stream s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.m_rootDir);
			stringBuilder.Append(path);
			stringBuilder.Append('\\');
			stringBuilder.Append("identity.dat");
			s = null;
			try
			{
				byte[] array;
				using (FileStream fileStream = new FileStream(stringBuilder.ToString(), FileMode.Open))
				{
					int i = (int)fileStream.Length;
					array = new byte[i];
					int num = 0;
					while (i > 0)
					{
						int num2 = fileStream.Read(array, num, i);
						if (num2 == 0)
						{
							__Error.EndOfFile();
						}
						num += num2;
						i -= num2;
					}
				}
				s = new MemoryStream(array);
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x0400229B RID: 8859
		private const char s_SepExternal = '\\';

		// Token: 0x0400229C RID: 8860
		private IsolatedStorageFile m_Current;

		// Token: 0x0400229D RID: 8861
		private IsolatedStorageScope m_Scope;

		// Token: 0x0400229E RID: 8862
		private FileIOPermission m_fiop;

		// Token: 0x0400229F RID: 8863
		private string m_rootDir;

		// Token: 0x040022A0 RID: 8864
		private TwoLevelFileEnumerator m_fileEnum;

		// Token: 0x040022A1 RID: 8865
		private bool m_fReset;

		// Token: 0x040022A2 RID: 8866
		private bool m_fEnd;
	}
}
