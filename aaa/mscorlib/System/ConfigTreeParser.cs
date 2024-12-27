using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000083 RID: 131
	internal class ConfigTreeParser : IConfigHandler
	{
		// Token: 0x06000751 RID: 1873 RVA: 0x00017D57 File Offset: 0x00016D57
		internal ConfigNode Parse(string fileName, string configPath)
		{
			return this.Parse(fileName, configPath, false);
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x00017D64 File Offset: 0x00016D64
		internal ConfigNode Parse(string fileName, string configPath, bool skipSecurityStuff)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			this.fileName = fileName;
			if (configPath[0] == '/')
			{
				this.treeRootPath = configPath.Substring(1).Split(new char[] { '/' });
				this.pathDepth = this.treeRootPath.Length - 1;
				this.bNoSearchPath = false;
			}
			else
			{
				this.treeRootPath = new string[1];
				this.treeRootPath[0] = configPath;
				this.bNoSearchPath = true;
			}
			if (!skipSecurityStuff)
			{
				new FileIOPermission(FileIOPermissionAccess.Read, Path.GetFullPathInternal(fileName)).Demand();
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			try
			{
				ConfigServer.RunParser(this, fileName);
			}
			catch (FileNotFoundException)
			{
				throw;
			}
			catch (DirectoryNotFoundException)
			{
				throw;
			}
			catch (UnauthorizedAccessException)
			{
				throw;
			}
			catch (FileLoadException)
			{
				throw;
			}
			catch (Exception ex)
			{
				throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("XML_Syntax_InvalidSyntaxInFile"), new object[] { fileName, this.lastProcessed }), ex);
			}
			catch
			{
				throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("XML_Syntax_InvalidSyntaxInFile"), new object[] { fileName, this.lastProcessed }));
			}
			return this.rootNode;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x00017ECC File Offset: 0x00016ECC
		public void NotifyEvent(ConfigEvents nEvent)
		{
		}

		// Token: 0x06000754 RID: 1876 RVA: 0x00017ED0 File Offset: 0x00016ED0
		public void BeginChildren(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength)
		{
			if (!this.parsing && !this.bNoSearchPath && this.depth == this.searchDepth + 1 && string.Compare(text, this.treeRootPath[this.searchDepth], StringComparison.Ordinal) == 0)
			{
				this.searchDepth++;
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x00017F24 File Offset: 0x00016F24
		public void EndChildren(int fEmpty, int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength)
		{
			this.lastProcessed = "</" + text + ">";
			if (this.parsing)
			{
				if (this.currentNode == this.rootNode)
				{
					this.parsing = false;
				}
				this.currentNode = this.currentNode.Parent;
				return;
			}
			if (nType == ConfigNodeType.Element)
			{
				if (this.depth == this.searchDepth && string.Compare(text, this.treeRootPath[this.searchDepth - 1], StringComparison.Ordinal) == 0)
				{
					this.searchDepth--;
					this.depth--;
					return;
				}
				this.depth--;
			}
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x00017FCE File Offset: 0x00016FCE
		public void Error(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength)
		{
		}

		// Token: 0x06000757 RID: 1879 RVA: 0x00017FD0 File Offset: 0x00016FD0
		public void CreateNode(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength)
		{
			if (nType != ConfigNodeType.Element)
			{
				if (nType == ConfigNodeType.PCData && this.currentNode != null)
				{
					this.currentNode.Value = text;
				}
				return;
			}
			this.lastProcessed = "<" + text + ">";
			if (!this.parsing && (!this.bNoSearchPath || string.Compare(text, this.treeRootPath[0], StringComparison.OrdinalIgnoreCase) != 0) && (this.depth != this.searchDepth || this.searchDepth != this.pathDepth || string.Compare(text, this.treeRootPath[this.pathDepth], StringComparison.OrdinalIgnoreCase) != 0))
			{
				this.depth++;
				return;
			}
			this.parsing = true;
			ConfigNode configNode = this.currentNode;
			this.currentNode = new ConfigNode(text, configNode);
			if (this.rootNode == null)
			{
				this.rootNode = this.currentNode;
				return;
			}
			configNode.AddChild(this.currentNode);
		}

		// Token: 0x06000758 RID: 1880 RVA: 0x000180B8 File Offset: 0x000170B8
		public void CreateAttribute(int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength)
		{
			if (!this.parsing)
			{
				return;
			}
			if (nType == ConfigNodeType.Attribute)
			{
				this.attributeEntry = this.currentNode.AddAttribute(text, "");
				this.key = text;
				return;
			}
			if (nType == ConfigNodeType.PCData)
			{
				this.currentNode.ReplaceAttribute(this.attributeEntry, this.key, text);
				return;
			}
			throw new ApplicationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("XML_Syntax_InvalidSyntaxInFile"), new object[] { this.fileName, this.lastProcessed }));
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00018145 File Offset: 0x00017145
		[Conditional("_LOGGING")]
		private void Trace(string name, int size, ConfigNodeSubType subType, ConfigNodeType nType, int terminal, [MarshalAs(UnmanagedType.LPWStr)] string text, int textLength, int prefixLength, int fEmpty)
		{
		}

		// Token: 0x04000276 RID: 630
		private ConfigNode rootNode;

		// Token: 0x04000277 RID: 631
		private ConfigNode currentNode;

		// Token: 0x04000278 RID: 632
		private string lastProcessed;

		// Token: 0x04000279 RID: 633
		private string fileName;

		// Token: 0x0400027A RID: 634
		private int attributeEntry;

		// Token: 0x0400027B RID: 635
		private string key;

		// Token: 0x0400027C RID: 636
		private string[] treeRootPath;

		// Token: 0x0400027D RID: 637
		private bool parsing;

		// Token: 0x0400027E RID: 638
		private int depth;

		// Token: 0x0400027F RID: 639
		private int pathDepth;

		// Token: 0x04000280 RID: 640
		private int searchDepth;

		// Token: 0x04000281 RID: 641
		private bool bNoSearchPath;
	}
}
