using System;
using System.IO;

namespace Microsoft.JScript.Vsa
{
	// Token: 0x0200010C RID: 268
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public class ResInfo
	{
		// Token: 0x06000B4C RID: 2892 RVA: 0x000567A1 File Offset: 0x000557A1
		public ResInfo(string filename, string name, bool isPublic, bool isLinked)
		{
			this.filename = filename;
			this.fullpath = Path.GetFullPath(filename);
			this.name = name;
			this.isPublic = isPublic;
			this.isLinked = isLinked;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x000567D4 File Offset: 0x000557D4
		public ResInfo(string resinfo, bool isLinked)
		{
			string[] array = resinfo.Split(new char[] { ',' });
			int num = array.Length;
			this.filename = array[0];
			this.name = Path.GetFileName(this.filename);
			this.isPublic = true;
			this.isLinked = isLinked;
			if (num == 2)
			{
				this.name = array[1];
			}
			else if (num > 2)
			{
				bool flag = false;
				if (string.Compare(array[num - 1], "public", StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
				}
				else if (string.Compare(array[num - 1], "private", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.isPublic = false;
					flag = true;
				}
				this.name = array[num - (flag ? 2 : 1)];
				this.filename = string.Join(",", array, 0, num - (flag ? 2 : 1));
			}
			this.fullpath = Path.GetFullPath(this.filename);
		}

		// Token: 0x040006CF RID: 1743
		public string filename;

		// Token: 0x040006D0 RID: 1744
		public string fullpath;

		// Token: 0x040006D1 RID: 1745
		public string name;

		// Token: 0x040006D2 RID: 1746
		public bool isPublic;

		// Token: 0x040006D3 RID: 1747
		public bool isLinked;
	}
}
