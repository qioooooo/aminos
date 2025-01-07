using System;
using System.Collections;
using System.ComponentModel;
using System.IO;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal sealed class AssemblyData
	{
		public AssemblyData()
		{
			ArrayList arrayList = new ArrayList(256);
			object obj = null;
			int num = 0;
			checked
			{
				do
				{
					arrayList.Add(obj);
					num++;
				}
				while (num <= 255);
				this.m_Files = arrayList;
			}
		}

		internal VB6File GetChannelObj(int lChannel)
		{
			object obj;
			if (lChannel < this.m_Files.Count)
			{
				obj = this.m_Files[lChannel];
			}
			else
			{
				obj = null;
			}
			return (VB6File)obj;
		}

		internal void SetChannelObj(int lChannel, VB6File oFile)
		{
			if (this.m_Files == null)
			{
				this.m_Files = new ArrayList(256);
			}
			if (oFile == null)
			{
				VB6File vb6File = (VB6File)this.m_Files[lChannel];
				if (vb6File != null)
				{
					vb6File.CloseFile();
				}
				this.m_Files[lChannel] = null;
			}
			else
			{
				this.m_Files[lChannel] = oFile;
			}
		}

		public ArrayList m_Files;

		internal FileSystemInfo[] m_DirFiles;

		internal int m_DirNextFileIndex;

		internal FileAttributes m_DirAttributes;
	}
}
