using System;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x0200047F RID: 1151
	[ComVisible(true)]
	[Serializable]
	public sealed class ApplicationDirectory : IBuiltInEvidence
	{
		// Token: 0x06002E24 RID: 11812 RVA: 0x0009C7B0 File Offset: 0x0009B7B0
		internal ApplicationDirectory()
		{
			this.m_appDirectory = null;
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x0009C7BF File Offset: 0x0009B7BF
		public ApplicationDirectory(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.m_appDirectory = new URLString(name);
		}

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002E26 RID: 11814 RVA: 0x0009C7E1 File Offset: 0x0009B7E1
		public string Directory
		{
			get
			{
				return this.m_appDirectory.ToString();
			}
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x0009C7F0 File Offset: 0x0009B7F0
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			if (!(o is ApplicationDirectory))
			{
				return false;
			}
			ApplicationDirectory applicationDirectory = (ApplicationDirectory)o;
			if (this.m_appDirectory == null)
			{
				return applicationDirectory.m_appDirectory == null;
			}
			return applicationDirectory.m_appDirectory != null && this.m_appDirectory.IsSubsetOf(applicationDirectory.m_appDirectory) && applicationDirectory.m_appDirectory.IsSubsetOf(this.m_appDirectory);
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x0009C855 File Offset: 0x0009B855
		public override int GetHashCode()
		{
			return this.Directory.GetHashCode();
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x0009C864 File Offset: 0x0009B864
		public object Copy()
		{
			return new ApplicationDirectory
			{
				m_appDirectory = this.m_appDirectory
			};
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x0009C884 File Offset: 0x0009B884
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.ApplicationDirectory");
			securityElement.AddAttribute("version", "1");
			if (this.m_appDirectory != null)
			{
				securityElement.AddChild(new SecurityElement("Directory", this.m_appDirectory.ToString()));
			}
			return securityElement;
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x0009C8D0 File Offset: 0x0009B8D0
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position++] = '\0';
			string directory = this.Directory;
			int length = directory.Length;
			if (verbose)
			{
				BuiltInEvidenceHelper.CopyIntToCharArray(length, buffer, position);
				position += 2;
			}
			directory.CopyTo(0, buffer, position, length);
			return length + position;
		}

		// Token: 0x06002E2C RID: 11820 RVA: 0x0009C914 File Offset: 0x0009B914
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			int intFromCharArray = BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			position += 2;
			this.m_appDirectory = new URLString(new string(buffer, position, intFromCharArray));
			return position + intFromCharArray;
		}

		// Token: 0x06002E2D RID: 11821 RVA: 0x0009C944 File Offset: 0x0009B944
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (verbose)
			{
				return this.Directory.Length + 3;
			}
			return this.Directory.Length + 1;
		}

		// Token: 0x06002E2E RID: 11822 RVA: 0x0009C964 File Offset: 0x0009B964
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x0400177D RID: 6013
		private URLString m_appDirectory;
	}
}
