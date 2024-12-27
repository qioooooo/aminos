using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Security;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Util;
using System.Xml;

namespace System.Web.Compilation
{
	// Token: 0x02000188 RID: 392
	internal class PreservationFileReader
	{
		// Token: 0x060010DB RID: 4315 RVA: 0x0004B8F0 File Offset: 0x0004A8F0
		internal PreservationFileReader(DiskBuildResultCache diskCache, bool precompilationMode)
		{
			this._diskCache = diskCache;
			this._precompilationMode = precompilationMode;
		}

		// Token: 0x060010DC RID: 4316 RVA: 0x0004B908 File Offset: 0x0004A908
		internal BuildResult ReadBuildResultFromFile(VirtualPath virtualPath, string preservationFile, long hashCode)
		{
			if (!FileUtil.FileExists(preservationFile))
			{
				return null;
			}
			BuildResult buildResult = null;
			try
			{
				buildResult = this.ReadFileInternal(virtualPath, preservationFile, hashCode);
			}
			catch (SecurityException)
			{
				throw;
			}
			catch
			{
				if (!this._precompilationMode)
				{
					Util.RemoveOrRenameFile(preservationFile);
				}
			}
			return buildResult;
		}

		// Token: 0x060010DD RID: 4317 RVA: 0x0004B960 File Offset: 0x0004A960
		private BuildResult ReadFileInternal(VirtualPath virtualPath, string preservationFile, long hashCode)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(preservationFile);
			this._root = xmlDocument.DocumentElement;
			if (this._root == null || this._root.Name != "preserve")
			{
				return null;
			}
			string attribute = this.GetAttribute("resultType");
			BuildResultTypeCode buildResultTypeCode = (BuildResultTypeCode)int.Parse(attribute, CultureInfo.InvariantCulture);
			if (virtualPath == null || AppSettings.VerifyVirtualPathFromDiskCache)
			{
				virtualPath = VirtualPath.Create(this.GetAttribute("virtualPath"));
			}
			long num = 0L;
			string text = null;
			if (!this._precompilationMode)
			{
				string attribute2 = this.GetAttribute("hash");
				if (attribute2 == null)
				{
					return null;
				}
				num = long.Parse(attribute2, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
				text = this.GetAttribute("filehash");
			}
			BuildResult buildResult = BuildResult.CreateBuildResultFromCode(buildResultTypeCode, virtualPath);
			if (!this._precompilationMode)
			{
				this.ReadDependencies();
				if (this._sourceDependencies != null)
				{
					buildResult.SetVirtualPathDependencies(this._sourceDependencies);
				}
				buildResult.VirtualPathDependenciesHash = text;
				bool flag = false;
				if (!buildResult.IsUpToDate(virtualPath))
				{
					flag = true;
				}
				else
				{
					long num2 = buildResult.ComputeHashCode(hashCode);
					if (num2 == 0L || num2 != num)
					{
						flag = true;
					}
				}
				if (flag)
				{
					buildResult.RemoveOutOfDateResources(this);
					File.Delete(preservationFile);
					return null;
				}
			}
			buildResult.GetPreservedAttributes(this);
			return buildResult;
		}

		// Token: 0x060010DE RID: 4318 RVA: 0x0004BAA0 File Offset: 0x0004AAA0
		private void ReadDependencies()
		{
			foreach (object obj in this._root.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				string name;
				if (xmlNode.NodeType == XmlNodeType.Element && (name = xmlNode.Name) != null && name == "filedeps")
				{
					this._sourceDependencies = this.ReadDependencies(xmlNode, "filedep");
				}
			}
		}

		// Token: 0x060010DF RID: 4319 RVA: 0x0004BB08 File Offset: 0x0004AB08
		private ArrayList ReadDependencies(XmlNode parent, string tagName)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in parent.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (!xmlNode.Name.Equals(tagName))
					{
						break;
					}
					string text = HandlerBase.RemoveAttribute(xmlNode, "name");
					if (text == null)
					{
						return null;
					}
					arrayList.Add(text);
				}
			}
			return arrayList;
		}

		// Token: 0x060010E0 RID: 4320 RVA: 0x0004BB6E File Offset: 0x0004AB6E
		internal string GetAttribute(string name)
		{
			return HandlerBase.RemoveAttribute(this._root, name);
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x0004BB7C File Offset: 0x0004AB7C
		internal DiskBuildResultCache DiskCache
		{
			get
			{
				return this._diskCache;
			}
		}

		// Token: 0x04001680 RID: 5760
		private XmlNode _root;

		// Token: 0x04001681 RID: 5761
		private bool _precompilationMode;

		// Token: 0x04001682 RID: 5762
		private DiskBuildResultCache _diskCache;

		// Token: 0x04001683 RID: 5763
		private ArrayList _sourceDependencies;
	}
}
