using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace System.Web.Compilation
{
	// Token: 0x02000189 RID: 393
	internal class PreservationFileWriter
	{
		// Token: 0x060010E2 RID: 4322 RVA: 0x0004BB84 File Offset: 0x0004AB84
		internal PreservationFileWriter(bool precompilationMode)
		{
			this._precompilationMode = precompilationMode;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0004BB94 File Offset: 0x0004AB94
		internal void SaveBuildResultToFile(string preservationFile, BuildResult result, long hashCode)
		{
			this._writer = new XmlTextWriter(preservationFile, Encoding.UTF8);
			try
			{
				this._writer.Formatting = Formatting.Indented;
				this._writer.Indentation = 4;
				this._writer.WriteStartDocument();
				this._writer.WriteStartElement("preserve");
				this.SetAttribute("resultType", ((int)result.GetCode()).ToString(CultureInfo.InvariantCulture));
				if (result.VirtualPath != null)
				{
					this.SetAttribute("virtualPath", result.VirtualPath.VirtualPathString);
				}
				this.SetAttribute("hash", result.ComputeHashCode(hashCode).ToString("x", CultureInfo.InvariantCulture));
				string virtualPathDependenciesHash = result.VirtualPathDependenciesHash;
				if (virtualPathDependenciesHash != null)
				{
					this.SetAttribute("filehash", virtualPathDependenciesHash);
				}
				result.SetPreservedAttributes(this);
				this.SaveDependencies(result.VirtualPathDependencies);
				this._writer.WriteEndElement();
				this._writer.WriteEndDocument();
				this._writer.Close();
			}
			catch
			{
				this._writer.Close();
				File.Delete(preservationFile);
				throw;
			}
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0004BCBC File Offset: 0x0004ACBC
		private void SaveDependencies(ICollection dependencies)
		{
			if (dependencies != null)
			{
				this._writer.WriteStartElement("filedeps");
				foreach (object obj in dependencies)
				{
					string text = (string)obj;
					this._writer.WriteStartElement("filedep");
					this._writer.WriteAttributeString("name", text);
					this._writer.WriteEndElement();
				}
				this._writer.WriteEndElement();
			}
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0004BD54 File Offset: 0x0004AD54
		internal void SetAttribute(string name, string value)
		{
			this._writer.WriteAttributeString(name, value);
		}

		// Token: 0x04001684 RID: 5764
		internal const string fileDependenciesTagName = "filedeps";

		// Token: 0x04001685 RID: 5765
		internal const string fileDependencyTagName = "filedep";

		// Token: 0x04001686 RID: 5766
		internal const string buildResultDependenciesTagName = "builddeps";

		// Token: 0x04001687 RID: 5767
		internal const string buildResultDependencyTagName = "builddep";

		// Token: 0x04001688 RID: 5768
		private XmlTextWriter _writer;

		// Token: 0x04001689 RID: 5769
		private bool _precompilationMode;
	}
}
