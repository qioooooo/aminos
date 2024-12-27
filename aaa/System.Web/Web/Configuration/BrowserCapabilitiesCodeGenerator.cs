using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.ServiceProcess;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.Util;
using System.Xml;
using System.Xml.Schema;
using Microsoft.CSharp;

namespace System.Web.Configuration
{
	// Token: 0x0200012F RID: 303
	[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
	public class BrowserCapabilitiesCodeGenerator
	{
		// Token: 0x06000DD7 RID: 3543 RVA: 0x0003C6EC File Offset: 0x0003B6EC
		public BrowserCapabilitiesCodeGenerator()
		{
			this._headers = new CaseInsensitiveStringSet();
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000DD8 RID: 3544 RVA: 0x0003C74A File Offset: 0x0003B74A
		internal BrowserTree BrowserTree
		{
			get
			{
				return this._browserTree;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000DD9 RID: 3545 RVA: 0x0003C752 File Offset: 0x0003B752
		internal BrowserTree DefaultTree
		{
			get
			{
				return this._defaultTree;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000DDA RID: 3546 RVA: 0x0003C75A File Offset: 0x0003B75A
		internal ArrayList CustomTreeList
		{
			get
			{
				return this._customTreeList;
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06000DDB RID: 3547 RVA: 0x0003C762 File Offset: 0x0003B762
		internal ArrayList CustomTreeNames
		{
			get
			{
				return this._customTreeNames;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000DDC RID: 3548 RVA: 0x0003C76C File Offset: 0x0003B76C
		internal static string BrowserCapAssemblyPublicKeyToken
		{
			get
			{
				if (BrowserCapabilitiesCodeGenerator._publicKeyTokenLoaded)
				{
					return BrowserCapabilitiesCodeGenerator._publicKeyToken;
				}
				string text;
				lock (BrowserCapabilitiesCodeGenerator._staticLock)
				{
					if (BrowserCapabilitiesCodeGenerator._publicKeyTokenLoaded)
					{
						text = BrowserCapabilitiesCodeGenerator._publicKeyToken;
					}
					else
					{
						BrowserCapabilitiesCodeGenerator._publicKeyToken = BrowserCapabilitiesCodeGenerator.LoadPublicKeyTokenFromFile(BrowserCapabilitiesCodeGenerator._publicKeyTokenFile);
						BrowserCapabilitiesCodeGenerator._publicKeyTokenLoaded = true;
						text = BrowserCapabilitiesCodeGenerator._publicKeyToken;
					}
				}
				return text;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000DDD RID: 3549 RVA: 0x0003C7D8 File Offset: 0x0003B7D8
		internal virtual bool GenerateOverrides
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000DDE RID: 3550 RVA: 0x0003C7DB File Offset: 0x0003B7DB
		internal virtual string TypeName
		{
			get
			{
				return "BrowserCapabilitiesFactory";
			}
		}

		// Token: 0x06000DDF RID: 3551 RVA: 0x0003C7E2 File Offset: 0x0003B7E2
		internal void AddFile(string filePath)
		{
			if (this._browserFileList == null)
			{
				this._browserFileList = new ArrayList();
			}
			this._browserFileList.Add(filePath);
		}

		// Token: 0x06000DE0 RID: 3552 RVA: 0x0003C804 File Offset: 0x0003B804
		internal void AddCustomFile(string filePath)
		{
			if (this._customBrowserFileLists == null)
			{
				this._customBrowserFileLists = new ArrayList();
			}
			this._customBrowserFileLists.Add(filePath);
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0003C828 File Offset: 0x0003B828
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public virtual void Create()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(BrowserCapabilitiesCodeGenerator._browsersDirectory);
			FileInfo[] files = directoryInfo.GetFiles("*.browser");
			if (files == null || files.Length == 0)
			{
				return;
			}
			foreach (FileInfo fileInfo in files)
			{
				this.AddFile(fileInfo.FullName);
			}
			this.ProcessBrowserFiles();
			this.ProcessCustomBrowserFiles();
			this.Uninstall();
			this.GenerateAssembly();
			this.RestartW3SVCIfNecessary();
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0003C89C File Offset: 0x0003B89C
		internal bool UninstallInternal()
		{
			if (File.Exists(BrowserCapabilitiesCodeGenerator._publicKeyTokenFile))
			{
				File.Delete(BrowserCapabilitiesCodeGenerator._publicKeyTokenFile);
			}
			GacUtil gacUtil = new GacUtil();
			return gacUtil.GacUnInstall("ASP.BrowserCapsFactory, Version=2.0.0.0, Culture=neutral");
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0003C8D7 File Offset: 0x0003B8D7
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		public bool Uninstall()
		{
			this.RestartW3SVCIfNecessary();
			if (!this.UninstallInternal())
			{
				return false;
			}
			this.RestartW3SVCIfNecessary();
			return true;
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x0003C8F0 File Offset: 0x0003B8F0
		private void RestartW3SVCIfNecessary()
		{
			try
			{
				ServiceController serviceController = new ServiceController("W3SVC");
				ServiceControllerStatus status = serviceController.Status;
				if (!status.Equals(ServiceControllerStatus.Stopped) && !status.Equals(ServiceControllerStatus.StopPending) && !status.Equals(ServiceControllerStatus.StartPending))
				{
					serviceController.Stop();
					serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 5, 0));
					serviceController.Start();
					if (status.Equals(ServiceControllerStatus.Paused) || status.Equals(ServiceControllerStatus.PausePending))
					{
						serviceController.Pause();
					}
				}
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException(SR.GetString("Browser_W3SVC_Failure_Helper_Text", new object[] { ex }));
			}
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0003C9C0 File Offset: 0x0003B9C0
		internal void ProcessBrowserFiles()
		{
			this.ProcessBrowserFiles(false, string.Empty);
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x0003C9D0 File Offset: 0x0003B9D0
		private string NoPathFileName(string fullPath)
		{
			int num = fullPath.LastIndexOf("\\", StringComparison.Ordinal);
			if (num > -1)
			{
				return fullPath.Substring(num + 1);
			}
			return fullPath;
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x0003C9FC File Offset: 0x0003B9FC
		internal virtual void ProcessBrowserNode(XmlNode node, BrowserTree browserTree)
		{
			BrowserDefinition browserDefinition;
			if (node.Name == "gateway")
			{
				browserDefinition = new GatewayDefinition(node);
			}
			else if (node.Name == "browser")
			{
				browserDefinition = new BrowserDefinition(node);
			}
			else
			{
				browserDefinition = new BrowserDefinition(node, true);
			}
			BrowserDefinition browserDefinition2 = (BrowserDefinition)browserTree[browserDefinition.Name];
			if (browserDefinition2 == null)
			{
				browserTree[browserDefinition.Name] = browserDefinition;
				return;
			}
			if (browserDefinition.IsRefID)
			{
				browserDefinition2.MergeWithDefinition(browserDefinition);
				return;
			}
			throw new ConfigurationErrorsException(SR.GetString("Duplicate_browser_id", new object[] { browserDefinition.ID }), node);
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x0003CA9D File Offset: 0x0003BA9D
		private void NormalizeAndValidateTree(BrowserTree browserTree, bool isDefaultBrowser)
		{
			this.NormalizeAndValidateTree(browserTree, isDefaultBrowser, false);
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x0003CAA8 File Offset: 0x0003BAA8
		private void NormalizeAndValidateTree(BrowserTree browserTree, bool isDefaultBrowser, bool isCustomBrowser)
		{
			foreach (object obj in browserTree)
			{
				BrowserDefinition browserDefinition = (BrowserDefinition)((DictionaryEntry)obj).Value;
				string parentName = browserDefinition.ParentName;
				BrowserDefinition browserDefinition2 = null;
				if (!this.IsRootNode(browserDefinition.Name))
				{
					if (parentName.Length > 0)
					{
						browserDefinition2 = (BrowserDefinition)browserTree[parentName];
					}
					if (browserDefinition2 != null)
					{
						if (browserDefinition.IsRefID)
						{
							if (browserDefinition is GatewayDefinition)
							{
								browserDefinition2.RefGateways.Add(browserDefinition);
							}
							else
							{
								browserDefinition2.RefBrowsers.Add(browserDefinition);
							}
						}
						else if (browserDefinition is GatewayDefinition)
						{
							browserDefinition2.Gateways.Add(browserDefinition);
						}
						else
						{
							browserDefinition2.Browsers.Add(browserDefinition);
						}
					}
					else
					{
						if (isCustomBrowser)
						{
							throw new ConfigurationErrorsException(SR.GetString("Browser_parentID_Not_Found", new object[] { browserDefinition.ParentID }), browserDefinition.XmlNode);
						}
						this.HandleUnRecognizedParentElement(browserDefinition, isDefaultBrowser);
					}
				}
			}
			foreach (object obj2 in browserTree)
			{
				BrowserDefinition browserDefinition3 = (BrowserDefinition)((DictionaryEntry)obj2).Value;
				Hashtable hashtable = new Hashtable();
				BrowserDefinition browserDefinition4 = browserDefinition3;
				string text = browserDefinition4.Name;
				while (!this.IsRootNode(text))
				{
					if (hashtable[text] != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Browser_Circular_Reference", new object[] { text }), browserDefinition4.XmlNode);
					}
					hashtable[text] = text;
					browserDefinition4 = (BrowserDefinition)browserTree[browserDefinition4.ParentName];
					if (browserDefinition4 == null)
					{
						break;
					}
					text = browserDefinition4.Name;
				}
			}
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x0003CCA4 File Offset: 0x0003BCA4
		private void SetCustomTreeRoots(BrowserTree browserTree, int index)
		{
			foreach (object obj in browserTree)
			{
				BrowserDefinition browserDefinition = (BrowserDefinition)((DictionaryEntry)obj).Value;
				if (browserDefinition.ParentName == null)
				{
					this._customTreeNames[index] = browserDefinition.Name;
					break;
				}
			}
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x0003CD1C File Offset: 0x0003BD1C
		private bool IsRootNode(string nodeName)
		{
			if (string.Compare(nodeName, "Default", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			foreach (object obj in this._customTreeNames)
			{
				string text = (string)obj;
				if (string.Compare(nodeName, text, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x0003CD90 File Offset: 0x0003BD90
		protected void ProcessBrowserFiles(bool useVirtualPath, string virtualDir)
		{
			this._browserTree = new BrowserTree();
			this._defaultTree = new BrowserTree();
			this._customTreeNames = new ArrayList();
			if (this._browserFileList == null)
			{
				this._browserFileList = new ArrayList();
			}
			this._browserFileList.Sort();
			string text = null;
			string text2 = null;
			string text3 = null;
			foreach (object obj in this._browserFileList)
			{
				string text4 = (string)obj;
				if (text4.EndsWith("ie.browser", StringComparison.OrdinalIgnoreCase))
				{
					text2 = text4;
				}
				else if (text4.EndsWith("mozilla.browser", StringComparison.OrdinalIgnoreCase))
				{
					text = text4;
				}
				else if (text4.EndsWith("opera.browser", StringComparison.OrdinalIgnoreCase))
				{
					text3 = text4;
					break;
				}
			}
			if (text2 != null)
			{
				this._browserFileList.Remove(text2);
				this._browserFileList.Insert(0, text2);
			}
			if (text != null)
			{
				this._browserFileList.Remove(text);
				this._browserFileList.Insert(1, text);
			}
			if (text3 != null)
			{
				this._browserFileList.Remove(text3);
				this._browserFileList.Insert(2, text3);
			}
			foreach (object obj2 in this._browserFileList)
			{
				string text5 = (string)obj2;
				XmlDocument xmlDocument = new ConfigXmlDocument();
				try
				{
					xmlDocument.Load(text5);
					XmlNode documentElement = xmlDocument.DocumentElement;
					if (documentElement.Name != "browsers")
					{
						if (useVirtualPath)
						{
							throw new HttpParseException(SR.GetString("Invalid_browser_root"), null, virtualDir + "/" + this.NoPathFileName(text5), null, 1);
						}
						throw new HttpParseException(SR.GetString("Invalid_browser_root"), null, text5, null, 1);
					}
					else
					{
						foreach (object obj3 in documentElement.ChildNodes)
						{
							XmlNode xmlNode = (XmlNode)obj3;
							if (xmlNode.NodeType == XmlNodeType.Element)
							{
								if (xmlNode.Name == "browser" || xmlNode.Name == "gateway")
								{
									this.ProcessBrowserNode(xmlNode, this._browserTree);
								}
								else if (xmlNode.Name == "defaultBrowser")
								{
									this.ProcessBrowserNode(xmlNode, this._defaultTree);
								}
								else
								{
									HandlerBase.ThrowUnrecognizedElement(xmlNode);
								}
							}
						}
					}
				}
				catch (XmlException ex)
				{
					if (useVirtualPath)
					{
						throw new HttpParseException(ex.Message, null, virtualDir + "/" + this.NoPathFileName(text5), null, ex.LineNumber);
					}
					throw new HttpParseException(ex.Message, null, text5, null, ex.LineNumber);
				}
				catch (XmlSchemaException ex2)
				{
					if (useVirtualPath)
					{
						string empty = string.Empty;
						int num = text5.LastIndexOf("\\", StringComparison.Ordinal);
						if (num > -1)
						{
							text5.Substring(num);
						}
						throw new HttpParseException(ex2.Message, null, virtualDir + "/" + this.NoPathFileName(text5), null, ex2.LineNumber);
					}
					throw new HttpParseException(ex2.Message, null, text5, null, ex2.LineNumber);
				}
			}
			this.NormalizeAndValidateTree(this._browserTree, false);
			this.NormalizeAndValidateTree(this._defaultTree, true);
			BrowserDefinition browserDefinition = (BrowserDefinition)this._browserTree["Default"];
			if (browserDefinition != null)
			{
				this.AddBrowserToCollectionRecursive(browserDefinition, 0);
			}
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x0003D170 File Offset: 0x0003C170
		internal void ProcessCustomBrowserFiles()
		{
			this.ProcessCustomBrowserFiles(false, string.Empty);
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0003D180 File Offset: 0x0003C180
		internal void ProcessCustomBrowserFiles(bool useVirtualPath, string virtualDir)
		{
			DirectoryInfo[] array = null;
			this._customTreeList = new ArrayList();
			this._customBrowserFileLists = new ArrayList();
			this._customBrowserDefinitionCollections = new ArrayList();
			DirectoryInfo directoryInfo;
			if (!useVirtualPath)
			{
				directoryInfo = new DirectoryInfo(BrowserCapabilitiesCodeGenerator._browsersDirectory);
			}
			else
			{
				directoryInfo = new DirectoryInfo(HostingEnvironment.MapPathInternal(virtualDir));
			}
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			int num = 0;
			int num2 = directories.Length;
			array = new DirectoryInfo[num2];
			for (int i = 0; i < num2; i++)
			{
				if ((directories[i].Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
				{
					array[num] = directories[i];
					num++;
				}
			}
			Array.Resize<DirectoryInfo>(ref array, num);
			for (int j = 0; j < array.Length; j++)
			{
				FileInfo[] filesNotHidden = BrowserCapabilitiesCodeGenerator.GetFilesNotHidden(array[j], directoryInfo);
				if (filesNotHidden != null && filesNotHidden.Length != 0)
				{
					BrowserTree browserTree = new BrowserTree();
					this._customTreeList.Add(browserTree);
					this._customTreeNames.Add(array[j].Name);
					ArrayList arrayList = new ArrayList();
					foreach (FileInfo fileInfo in filesNotHidden)
					{
						arrayList.Add(fileInfo.FullName);
					}
					this._customBrowserFileLists.Add(arrayList);
				}
			}
			for (int l = 0; l < this._customBrowserFileLists.Count; l++)
			{
				ArrayList arrayList2 = (ArrayList)this._customBrowserFileLists[l];
				foreach (object obj in arrayList2)
				{
					string text = (string)obj;
					XmlDocument xmlDocument = new ConfigXmlDocument();
					try
					{
						xmlDocument.Load(text);
						XmlNode documentElement = xmlDocument.DocumentElement;
						if (documentElement.Name != "browsers")
						{
							if (useVirtualPath)
							{
								throw new HttpParseException(SR.GetString("Invalid_browser_root"), null, virtualDir + "/" + this.NoPathFileName(text), null, 1);
							}
							throw new HttpParseException(SR.GetString("Invalid_browser_root"), null, text, null, 1);
						}
						else
						{
							foreach (object obj2 in documentElement.ChildNodes)
							{
								XmlNode xmlNode = (XmlNode)obj2;
								if (xmlNode.NodeType == XmlNodeType.Element)
								{
									if (xmlNode.Name == "browser" || xmlNode.Name == "gateway")
									{
										this.ProcessBrowserNode(xmlNode, (BrowserTree)this._customTreeList[l]);
									}
									else
									{
										HandlerBase.ThrowUnrecognizedElement(xmlNode);
									}
								}
							}
						}
					}
					catch (XmlException ex)
					{
						if (useVirtualPath)
						{
							throw new HttpParseException(ex.Message, null, virtualDir + "/" + this.NoPathFileName(text), null, ex.LineNumber);
						}
						throw new HttpParseException(ex.Message, null, text, null, ex.LineNumber);
					}
					catch (XmlSchemaException ex2)
					{
						if (useVirtualPath)
						{
							string empty = string.Empty;
							int num3 = text.LastIndexOf("\\", StringComparison.Ordinal);
							if (num3 > -1)
							{
								text.Substring(num3);
							}
							throw new HttpParseException(ex2.Message, null, virtualDir + "/" + this.NoPathFileName(text), null, ex2.LineNumber);
						}
						throw new HttpParseException(ex2.Message, null, text, null, ex2.LineNumber);
					}
				}
				this.SetCustomTreeRoots((BrowserTree)this._customTreeList[l], l);
				this.NormalizeAndValidateTree((BrowserTree)this._customTreeList[l], false, true);
				this._customBrowserDefinitionCollections.Add(new BrowserDefinitionCollection());
				this.AddCustomBrowserToCollectionRecursive((BrowserDefinition)((BrowserTree)this._customTreeList[l])[this._customTreeNames[l]], 0, l);
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0003D5B4 File Offset: 0x0003C5B4
		internal void AddCustomBrowserToCollectionRecursive(BrowserDefinition bd, int depth, int index)
		{
			if (this._customBrowserDefinitionCollections[index] == null)
			{
				this._customBrowserDefinitionCollections[index] = new BrowserDefinitionCollection();
			}
			bd.Depth = depth;
			bd.IsDeviceNode = true;
			((BrowserDefinitionCollection)this._customBrowserDefinitionCollections[index]).Add(bd);
			foreach (object obj in bd.Browsers)
			{
				BrowserDefinition browserDefinition = (BrowserDefinition)obj;
				this.AddCustomBrowserToCollectionRecursive(browserDefinition, depth + 1, index);
			}
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x0003D658 File Offset: 0x0003C658
		internal void AddBrowserToCollectionRecursive(BrowserDefinition bd, int depth)
		{
			if (this._browserDefinitionCollection == null)
			{
				this._browserDefinitionCollection = new BrowserDefinitionCollection();
			}
			bd.Depth = depth;
			bd.IsDeviceNode = true;
			this._browserDefinitionCollection.Add(bd);
			foreach (object obj in bd.Browsers)
			{
				BrowserDefinition browserDefinition = (BrowserDefinition)obj;
				this.AddBrowserToCollectionRecursive(browserDefinition, depth + 1);
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x0003D6E4 File Offset: 0x0003C6E4
		internal virtual void HandleUnRecognizedParentElement(BrowserDefinition bd, bool isDefault)
		{
			throw new ConfigurationErrorsException(SR.GetString("Browser_parentID_Not_Found", new object[] { bd.ParentID }), bd.XmlNode);
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x0003D718 File Offset: 0x0003C718
		private static FileInfo[] GetFilesNotHidden(DirectoryInfo rootDirectory, DirectoryInfo browserDirInfo)
		{
			ArrayList arrayList = new ArrayList();
			DirectoryInfo[] directories = rootDirectory.GetDirectories("*", SearchOption.AllDirectories);
			FileInfo[] array = rootDirectory.GetFiles("*.browser", SearchOption.TopDirectoryOnly);
			arrayList.AddRange(array);
			for (int i = 0; i < directories.Length; i++)
			{
				if (!BrowserCapabilitiesCodeGenerator.HasHiddenParent(directories[i], browserDirInfo))
				{
					array = directories[i].GetFiles("*.browser", SearchOption.TopDirectoryOnly);
					arrayList.AddRange(array);
				}
			}
			return (FileInfo[])arrayList.ToArray(typeof(FileInfo));
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x0003D790 File Offset: 0x0003C790
		private static bool HasHiddenParent(DirectoryInfo directory, DirectoryInfo browserDirInfo)
		{
			while (!string.Equals(directory.Parent.Name, browserDirInfo.Name))
			{
				if ((directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
				{
					return true;
				}
				directory = directory.Parent;
			}
			return false;
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x0003D7C4 File Offset: 0x0003C7C4
		private void GenerateAssembly()
		{
			BrowserDefinition browserDefinition = (BrowserDefinition)this._browserTree["Default"];
			BrowserDefinition browserDefinition2 = (BrowserDefinition)this._defaultTree["Default"];
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < this._customTreeNames.Count; i++)
			{
				arrayList.Add((BrowserDefinition)((BrowserTree)this._customTreeList[i])[this._customTreeNames[i]]);
			}
			CSharpCodeProvider csharpCodeProvider = new CSharpCodeProvider();
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Reflection.AssemblyKeyFile", new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodePrimitiveExpression(BrowserCapabilitiesCodeGenerator._strongNameKeyFileName))
			});
			CodeAttributeDeclaration codeAttributeDeclaration2 = new CodeAttributeDeclaration("System.Security.AllowPartiallyTrustedCallers");
			codeCompileUnit.AssemblyCustomAttributes.Add(codeAttributeDeclaration2);
			codeCompileUnit.AssemblyCustomAttributes.Add(codeAttributeDeclaration);
			codeAttributeDeclaration = new CodeAttributeDeclaration("System.Reflection.AssemblyVersion", new CodeAttributeArgument[]
			{
				new CodeAttributeArgument(new CodePrimitiveExpression("2.0.0.0"))
			});
			codeCompileUnit.AssemblyCustomAttributes.Add(codeAttributeDeclaration);
			CodeNamespace codeNamespace = new CodeNamespace("ASP");
			codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Web"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Web.Configuration"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Reflection"));
			codeCompileUnit.Namespaces.Add(codeNamespace);
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration("BrowserCapabilitiesFactory");
			codeTypeDeclaration.Attributes = MemberAttributes.Private;
			codeTypeDeclaration.IsClass = true;
			codeTypeDeclaration.Name = this.TypeName;
			codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference("System.Web.Configuration.BrowserCapabilitiesFactoryBase"));
			codeNamespace.Types.Add(codeTypeDeclaration);
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Attributes = (MemberAttributes)24580;
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
			codeMemberMethod.Name = "ConfigureBrowserCapabilities";
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(NameValueCollection), "headers");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(HttpBrowserCapabilities), "browserCaps");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeTypeDeclaration.Members.Add(codeMemberMethod);
			this.GenerateSingleProcessCall(browserDefinition, codeMemberMethod);
			for (int j = 0; j < arrayList.Count; j++)
			{
				this.GenerateSingleProcessCall((BrowserDefinition)arrayList[j], codeMemberMethod);
			}
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression(new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "IsBrowserUnknown", new CodeExpression[0])
			{
				Parameters = { this._browserCapsRefExpr }
			}, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
			codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement());
			codeMemberMethod.Statements.Add(codeConditionStatement);
			if (browserDefinition2 != null)
			{
				this.GenerateSingleProcessCall(browserDefinition2, codeMemberMethod, "Default");
			}
			for (int k = 0; k < arrayList.Count; k++)
			{
				foreach (object obj in ((BrowserTree)this._customTreeList[k]))
				{
					BrowserDefinition browserDefinition3 = ((DictionaryEntry)obj).Value as BrowserDefinition;
					this.GenerateProcessMethod(browserDefinition3, codeTypeDeclaration);
				}
			}
			foreach (object obj2 in this._browserTree)
			{
				BrowserDefinition browserDefinition4 = ((DictionaryEntry)obj2).Value as BrowserDefinition;
				this.GenerateProcessMethod(browserDefinition4, codeTypeDeclaration);
			}
			foreach (object obj3 in this._defaultTree)
			{
				BrowserDefinition browserDefinition5 = ((DictionaryEntry)obj3).Value as BrowserDefinition;
				this.GenerateProcessMethod(browserDefinition5, codeTypeDeclaration, "Default");
			}
			this.GenerateOverrideMatchedHeaders(codeTypeDeclaration);
			this.GenerateOverrideBrowserElements(codeTypeDeclaration);
			TextWriter textWriter = new StreamWriter(new FileStream(BrowserCapabilitiesCodeGenerator._browsersDirectory + "\\BrowserCapsFactory.cs", FileMode.Create));
			try
			{
				csharpCodeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, textWriter, null);
			}
			finally
			{
				if (textWriter != null)
				{
					textWriter.Close();
				}
			}
			CompilationSection compilation = RuntimeConfig.GetAppConfig().Compilation;
			bool debug = compilation.Debug;
			string text = BrowserCapabilitiesCodeGenerator._browsersDirectory + "\\" + BrowserCapabilitiesCodeGenerator._strongNameKeyFileName;
			StrongNameUtility.GenerateStrongNameFile(text);
			string[] array = new string[] { "System.dll", "System.Web.dll" };
			CompilerParameters compilerParameters = new CompilerParameters(array, "ASP.BrowserCapsFactory", debug);
			compilerParameters.GenerateInMemory = false;
			compilerParameters.OutputAssembly = BrowserCapabilitiesCodeGenerator._browsersDirectory + "\\ASP.BrowserCapsFactory.dll";
			CompilerResults compilerResults = null;
			try
			{
				compilerResults = csharpCodeProvider.CompileAssemblyFromFile(compilerParameters, new string[] { BrowserCapabilitiesCodeGenerator._browsersDirectory + "\\BrowserCapsFactory.cs" });
			}
			finally
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			if (compilerResults.NativeCompilerReturnValue != 0 || compilerResults.Errors.HasErrors)
			{
				foreach (object obj4 in compilerResults.Errors)
				{
					CompilerError compilerError = (CompilerError)obj4;
					if (!compilerError.IsWarning)
					{
						throw new HttpCompileException(compilerError.ErrorText);
					}
				}
				throw new HttpCompileException(SR.GetString("Browser_compile_error"));
			}
			Assembly compiledAssembly = compilerResults.CompiledAssembly;
			GacUtil gacUtil = new GacUtil();
			gacUtil.GacInstall(compiledAssembly.Location);
			this.SavePublicKeyTokenFile(BrowserCapabilitiesCodeGenerator._publicKeyTokenFile, compiledAssembly.GetName().GetPublicKeyToken());
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x0003DE08 File Offset: 0x0003CE08
		private void SavePublicKeyTokenFile(string filename, byte[] publicKeyToken)
		{
			using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					foreach (byte b in publicKeyToken)
					{
						streamWriter.Write("{0:X2}", b);
					}
				}
			}
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x0003DE84 File Offset: 0x0003CE84
		private static string LoadPublicKeyTokenFromFile(string filename)
		{
			IStackWalk stackWalk = InternalSecurityPermissions.FileReadAccess(filename);
			stackWalk.Assert();
			if (!File.Exists(filename))
			{
				return null;
			}
			string text;
			try
			{
				using (FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					using (StreamReader streamReader = new StreamReader(fileStream))
					{
						text = streamReader.ReadLine();
					}
				}
			}
			catch (IOException)
			{
				if (HttpRuntime.HasFilePermission(filename))
				{
					throw;
				}
				text = null;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return text;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x0003DF20 File Offset: 0x0003CF20
		internal void GenerateOverrideBrowserElements(CodeTypeDeclaration typeDeclaration)
		{
			if (this._browserDefinitionCollection == null)
			{
				return;
			}
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = "PopulateBrowserElements";
			codeMemberMethod.Attributes = (MemberAttributes)12292;
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(IDictionary)), "dictionary");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			typeDeclaration.Members.Add(codeMemberMethod);
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(new CodeBaseReferenceExpression(), "PopulateBrowserElements");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeMethodReferenceExpression, new CodeExpression[] { this._dictionaryRefExpr });
			codeMemberMethod.Statements.Add(codeMethodInvokeExpression);
			foreach (object obj in this._browserDefinitionCollection)
			{
				BrowserDefinition browserDefinition = (BrowserDefinition)obj;
				if (browserDefinition.IsDeviceNode)
				{
					CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
					codeAssignStatement.Left = new CodeIndexerExpression(this._dictionaryRefExpr, new CodeExpression[]
					{
						new CodePrimitiveExpression(browserDefinition.ID)
					});
					codeAssignStatement.Right = new CodeObjectCreateExpression(typeof(Triplet), new CodeExpression[]
					{
						new CodePrimitiveExpression(browserDefinition.ParentName),
						new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(string)), "Empty"),
						new CodePrimitiveExpression(browserDefinition.Depth)
					});
					codeMemberMethod.Statements.Add(codeAssignStatement);
				}
			}
			for (int i = 0; i < this._customTreeNames.Count; i++)
			{
				foreach (object obj2 in ((BrowserDefinitionCollection)this._customBrowserDefinitionCollections[i]))
				{
					BrowserDefinition browserDefinition2 = (BrowserDefinition)obj2;
					if (browserDefinition2.IsDeviceNode)
					{
						CodeAssignStatement codeAssignStatement2 = new CodeAssignStatement();
						codeAssignStatement2.Left = new CodeIndexerExpression(this._dictionaryRefExpr, new CodeExpression[]
						{
							new CodePrimitiveExpression(browserDefinition2.ID)
						});
						codeAssignStatement2.Right = new CodeObjectCreateExpression(typeof(Triplet), new CodeExpression[]
						{
							new CodePrimitiveExpression(browserDefinition2.ParentName),
							new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(string)), "Empty"),
							new CodePrimitiveExpression(browserDefinition2.Depth)
						});
						codeMemberMethod.Statements.Add(codeAssignStatement2);
					}
				}
			}
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x0003E200 File Offset: 0x0003D200
		internal void GenerateOverrideMatchedHeaders(CodeTypeDeclaration typeDeclaration)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = "PopulateMatchedHeaders";
			codeMemberMethod.Attributes = (MemberAttributes)12292;
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(IDictionary)), "dictionary");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			typeDeclaration.Members.Add(codeMemberMethod);
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(new CodeBaseReferenceExpression(), "PopulateMatchedHeaders");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeMethodReferenceExpression, new CodeExpression[] { this._dictionaryRefExpr });
			codeMemberMethod.Statements.Add(codeMethodInvokeExpression);
			foreach (object obj in ((IEnumerable)this._headers))
			{
				string text = (string)obj;
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = new CodeIndexerExpression(this._dictionaryRefExpr, new CodeExpression[]
				{
					new CodePrimitiveExpression(text)
				});
				codeAssignStatement.Right = new CodePrimitiveExpression(null);
				codeMemberMethod.Statements.Add(codeAssignStatement);
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x0003E340 File Offset: 0x0003D340
		internal void GenerateProcessMethod(BrowserDefinition bd, CodeTypeDeclaration ctd)
		{
			this.GenerateProcessMethod(bd, ctd, string.Empty);
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x0003E350 File Offset: 0x0003D350
		internal void GenerateProcessMethod(BrowserDefinition bd, CodeTypeDeclaration ctd, string prefix)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = prefix + bd.Name + "Process";
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(bool));
			codeMemberMethod.Attributes = MemberAttributes.Private;
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(NameValueCollection), "headers");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(HttpBrowserCapabilities), "browserCaps");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			bool flag = false;
			this.GenerateIdentificationCode(bd, codeMemberMethod, ref flag);
			this.GenerateCapturesCode(bd, codeMemberMethod, ref flag);
			this.GenerateSetCapabilitiesCode(bd, codeMemberMethod, ref flag);
			this.GenerateSetAdaptersCode(bd, codeMemberMethod);
			if (bd.IsDeviceNode)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeVariableReferenceExpression("browserCaps"), "AddBrowser", new CodeExpression[0]);
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(bd.ID));
				codeMemberMethod.Statements.Add(codeMethodInvokeExpression);
			}
			foreach (object obj in bd.RefGateways)
			{
				BrowserDefinition browserDefinition = (BrowserDefinition)obj;
				this.AddComment("ref gateways, parent=" + bd.ID, codeMemberMethod);
				this.GenerateSingleProcessCall(browserDefinition, codeMemberMethod);
			}
			if (this.GenerateOverrides && prefix.Length == 0)
			{
				string text = prefix + bd.Name + "ProcessGateways";
				this.GenerateChildProcessMethod(text, ctd, false);
				this.GenerateChildProcessInvokeExpression(text, codeMemberMethod, false);
			}
			foreach (object obj2 in bd.Gateways)
			{
				BrowserDefinition browserDefinition2 = (BrowserDefinition)obj2;
				this.AddComment("gateway, parent=" + bd.ID, codeMemberMethod);
				this.GenerateSingleProcessCall(browserDefinition2, codeMemberMethod);
			}
			if (this.GenerateOverrides)
			{
				CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(typeof(bool), "ignoreApplicationBrowsers", new CodePrimitiveExpression(bd.Browsers.Count != 0));
				codeMemberMethod.Statements.Add(codeVariableDeclarationStatement);
			}
			if (bd.Browsers.Count > 0)
			{
				CodeStatementCollection codeStatementCollection = codeMemberMethod.Statements;
				this.AddComment("browser, parent=" + bd.ID, codeMemberMethod);
				foreach (object obj3 in bd.Browsers)
				{
					BrowserDefinition browserDefinition3 = (BrowserDefinition)obj3;
					codeStatementCollection = this.GenerateTrackedSingleProcessCall(codeStatementCollection, browserDefinition3, codeMemberMethod, prefix);
				}
				if (this.GenerateOverrides)
				{
					codeStatementCollection.Add(new CodeAssignStatement
					{
						Left = new CodeVariableReferenceExpression("ignoreApplicationBrowsers"),
						Right = new CodePrimitiveExpression(false)
					});
				}
			}
			foreach (object obj4 in bd.RefBrowsers)
			{
				BrowserDefinition browserDefinition4 = (BrowserDefinition)obj4;
				this.AddComment("ref browsers, parent=" + bd.ID, codeMemberMethod);
				if (browserDefinition4.IsDefaultBrowser)
				{
					this.GenerateSingleProcessCall(browserDefinition4, codeMemberMethod, "Default");
				}
				else
				{
					this.GenerateSingleProcessCall(browserDefinition4, codeMemberMethod);
				}
			}
			if (this.GenerateOverrides)
			{
				string text2 = prefix + bd.Name + "ProcessBrowsers";
				this.GenerateChildProcessMethod(text2, ctd, true);
				this.GenerateChildProcessInvokeExpression(text2, codeMemberMethod, true);
			}
			CodeMethodReturnStatement codeMethodReturnStatement = new CodeMethodReturnStatement(new CodePrimitiveExpression(true));
			codeMemberMethod.Statements.Add(codeMethodReturnStatement);
			ctd.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x0003E744 File Offset: 0x0003D744
		private void GenerateChildProcessInvokeExpression(string methodName, CodeMemberMethod cmm, bool generateTracker)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), methodName, new CodeExpression[0]);
			if (generateTracker)
			{
				codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression("ignoreApplicationBrowsers"));
			}
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression("headers"));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression("browserCaps"));
			cmm.Statements.Add(codeMethodInvokeExpression);
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x0003E7B8 File Offset: 0x0003D7B8
		private void GenerateChildProcessMethod(string methodName, CodeTypeDeclaration ctd, bool generateTracker)
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Name = methodName;
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
			codeMemberMethod.Attributes = MemberAttributes.Family;
			CodeParameterDeclarationExpression codeParameterDeclarationExpression;
			if (generateTracker)
			{
				codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(bool), "ignoreApplicationBrowsers");
				codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			}
			codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(NameValueCollection), "headers");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(HttpBrowserCapabilities), "browserCaps");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			ctd.Members.Add(codeMemberMethod);
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x0003E86C File Offset: 0x0003D86C
		private void GenerateRegexWorkerIfNecessary(CodeMemberMethod cmm, ref bool regexWorkerGenerated)
		{
			if (regexWorkerGenerated)
			{
				return;
			}
			regexWorkerGenerated = true;
			cmm.Statements.Add(new CodeVariableDeclarationStatement("RegexWorker", "regexWorker"));
			cmm.Statements.Add(new CodeAssignStatement(this._regexWorkerRefExpr, new CodeObjectCreateExpression("RegexWorker", new CodeExpression[] { this._browserCapsRefExpr })));
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x0003E8D0 File Offset: 0x0003D8D0
		private void ReturnIfHeaderValueEmpty(CodeMemberMethod cmm, CodeVariableReferenceExpression varExpr)
		{
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			CodeMethodReferenceExpression codeMethodReferenceExpression = new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(typeof(string)), "IsNullOrEmpty");
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codeMethodReferenceExpression, new CodeExpression[] { varExpr });
			codeConditionStatement.Condition = codeMethodInvokeExpression;
			codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
			cmm.Statements.Add(codeConditionStatement);
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x0003E940 File Offset: 0x0003D940
		private void GenerateIdentificationCode(BrowserDefinition bd, CodeMemberMethod cmm, ref bool regexWorkerGenerated)
		{
			cmm.Statements.Add(new CodeVariableDeclarationStatement(typeof(IDictionary), "dictionary"));
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement(this._dictionaryRefExpr, new CodePropertyReferenceExpression(this._browserCapsRefExpr, "Capabilities"));
			cmm.Statements.Add(codeAssignStatement);
			bool flag = false;
			CodeVariableReferenceExpression codeVariableReferenceExpression = null;
			CodeVariableReferenceExpression codeVariableReferenceExpression2 = null;
			if (bd.IdHeaderChecks.Count > 0)
			{
				this.AddComment("Identification: check header matches", cmm);
				for (int i = 0; i < bd.IdHeaderChecks.Count; i++)
				{
					string matchString = ((CheckPair)bd.IdHeaderChecks[i]).MatchString;
					if (!matchString.Equals(".*"))
					{
						if (codeVariableReferenceExpression2 == null)
						{
							codeVariableReferenceExpression2 = this.GenerateVarReference(cmm, typeof(string), "headerValue");
						}
						CodeAssignStatement codeAssignStatement2 = new CodeAssignStatement();
						cmm.Statements.Add(codeAssignStatement2);
						codeAssignStatement2.Left = codeVariableReferenceExpression2;
						if (((CheckPair)bd.IdHeaderChecks[i]).Header.Equals("User-Agent"))
						{
							this._headers.Add(string.Empty);
							codeAssignStatement2.Right = new CodeCastExpression(typeof(string), new CodeIndexerExpression(new CodeVariableReferenceExpression("browserCaps"), new CodeExpression[]
							{
								new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(string)), "Empty")
							}));
						}
						else
						{
							string header = ((CheckPair)bd.IdHeaderChecks[i]).Header;
							this._headers.Add(header);
							codeAssignStatement2.Right = new CodeCastExpression(typeof(string), new CodeIndexerExpression(this._headersRefExpr, new CodeExpression[]
							{
								new CodePrimitiveExpression(header)
							}));
							flag = true;
						}
						if (matchString.Equals("."))
						{
							this.ReturnIfHeaderValueEmpty(cmm, codeVariableReferenceExpression2);
						}
						else
						{
							if (codeVariableReferenceExpression == null)
							{
								codeVariableReferenceExpression = this.GenerateVarReference(cmm, typeof(bool), "result");
							}
							this.GenerateRegexWorkerIfNecessary(cmm, ref regexWorkerGenerated);
							CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(this._regexWorkerRefExpr, "ProcessRegex", new CodeExpression[0]);
							codeMethodInvokeExpression.Parameters.Add(codeVariableReferenceExpression2);
							codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(matchString));
							codeAssignStatement = new CodeAssignStatement();
							codeAssignStatement.Left = codeVariableReferenceExpression;
							codeAssignStatement.Right = codeMethodInvokeExpression;
							cmm.Statements.Add(codeAssignStatement);
							CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
							if (((CheckPair)bd.IdHeaderChecks[i]).NonMatch)
							{
								codeConditionStatement.Condition = new CodeBinaryOperatorExpression(codeVariableReferenceExpression, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(true));
							}
							else
							{
								codeConditionStatement.Condition = new CodeBinaryOperatorExpression(codeVariableReferenceExpression, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
							}
							codeConditionStatement.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
							cmm.Statements.Add(codeConditionStatement);
						}
					}
				}
			}
			if (bd.IdCapabilityChecks.Count > 0)
			{
				this.AddComment("Identification: check capability matches", cmm);
				for (int j = 0; j < bd.IdCapabilityChecks.Count; j++)
				{
					string matchString2 = ((CheckPair)bd.IdCapabilityChecks[j]).MatchString;
					if (!matchString2.Equals(".*"))
					{
						if (codeVariableReferenceExpression2 == null)
						{
							codeVariableReferenceExpression2 = this.GenerateVarReference(cmm, typeof(string), "headerValue");
						}
						CodeAssignStatement codeAssignStatement3 = new CodeAssignStatement();
						cmm.Statements.Add(codeAssignStatement3);
						codeAssignStatement3.Left = codeVariableReferenceExpression2;
						codeAssignStatement3.Right = new CodeCastExpression(typeof(string), new CodeIndexerExpression(this._dictionaryRefExpr, new CodeExpression[]
						{
							new CodePrimitiveExpression(((CheckPair)bd.IdCapabilityChecks[j]).Header)
						}));
						if (!matchString2.Equals("."))
						{
							if (codeVariableReferenceExpression == null)
							{
								codeVariableReferenceExpression = this.GenerateVarReference(cmm, typeof(bool), "result");
							}
							this.GenerateRegexWorkerIfNecessary(cmm, ref regexWorkerGenerated);
							CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression(this._regexWorkerRefExpr, "ProcessRegex", new CodeExpression[0]);
							codeMethodInvokeExpression2.Parameters.Add(codeVariableReferenceExpression2);
							codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(matchString2));
							codeAssignStatement = new CodeAssignStatement();
							codeAssignStatement.Left = codeVariableReferenceExpression;
							codeAssignStatement.Right = codeMethodInvokeExpression2;
							cmm.Statements.Add(codeAssignStatement);
							CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
							if (((CheckPair)bd.IdCapabilityChecks[j]).NonMatch)
							{
								codeConditionStatement2.Condition = new CodeBinaryOperatorExpression(codeVariableReferenceExpression, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(true));
							}
							else
							{
								codeConditionStatement2.Condition = new CodeBinaryOperatorExpression(codeVariableReferenceExpression, CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
							}
							codeConditionStatement2.TrueStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
							cmm.Statements.Add(codeConditionStatement2);
						}
					}
				}
			}
			if (flag)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression3 = new CodeMethodInvokeExpression(this._browserCapsRefExpr, "DisableOptimizedCacheKey", new CodeExpression[0]);
				cmm.Statements.Add(codeMethodInvokeExpression3);
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x0003EE51 File Offset: 0x0003DE51
		private CodeVariableReferenceExpression GenerateVarReference(CodeMemberMethod cmm, Type varType, string varName)
		{
			cmm.Statements.Add(new CodeVariableDeclarationStatement(varType, varName));
			return new CodeVariableReferenceExpression(varName);
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0003EE6C File Offset: 0x0003DE6C
		private void GenerateCapturesCode(BrowserDefinition bd, CodeMemberMethod cmm, ref bool regexWorkerGenerated)
		{
			if (bd.CaptureHeaderChecks.Count == 0 && bd.CaptureCapabilityChecks.Count == 0)
			{
				return;
			}
			if (bd.CaptureHeaderChecks.Count > 0)
			{
				this.AddComment("Capture: header values", cmm);
				for (int i = 0; i < bd.CaptureHeaderChecks.Count; i++)
				{
					string matchString = ((CheckPair)bd.CaptureHeaderChecks[i]).MatchString;
					if (!matchString.Equals(".*"))
					{
						this.GenerateRegexWorkerIfNecessary(cmm, ref regexWorkerGenerated);
						CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(this._regexWorkerRefExpr, "ProcessRegex", new CodeExpression[0]);
						if (((CheckPair)bd.CaptureHeaderChecks[i]).Header.Equals("User-Agent"))
						{
							this._headers.Add(string.Empty);
							codeMethodInvokeExpression.Parameters.Add(new CodeCastExpression(typeof(string), new CodeIndexerExpression(new CodeVariableReferenceExpression("browserCaps"), new CodeExpression[]
							{
								new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(typeof(string)), "Empty")
							})));
						}
						else
						{
							string header = ((CheckPair)bd.CaptureHeaderChecks[i]).Header;
							this._headers.Add(header);
							codeMethodInvokeExpression.Parameters.Add(new CodeCastExpression(typeof(string), new CodeIndexerExpression(this._headersRefExpr, new CodeExpression[]
							{
								new CodePrimitiveExpression(header)
							})));
						}
						codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(matchString));
						cmm.Statements.Add(codeMethodInvokeExpression);
					}
				}
			}
			if (bd.CaptureCapabilityChecks.Count > 0)
			{
				this.AddComment("Capture: capability values", cmm);
				for (int j = 0; j < bd.CaptureCapabilityChecks.Count; j++)
				{
					string matchString2 = ((CheckPair)bd.CaptureCapabilityChecks[j]).MatchString;
					if (!matchString2.Equals(".*"))
					{
						this.GenerateRegexWorkerIfNecessary(cmm, ref regexWorkerGenerated);
						CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression(this._regexWorkerRefExpr, "ProcessRegex", new CodeExpression[0]);
						codeMethodInvokeExpression2.Parameters.Add(new CodeCastExpression(typeof(string), new CodeIndexerExpression(this._dictionaryRefExpr, new CodeExpression[]
						{
							new CodePrimitiveExpression(((CheckPair)bd.CaptureCapabilityChecks[j]).Header)
						})));
						codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(matchString2));
						cmm.Statements.Add(codeMethodInvokeExpression2);
					}
				}
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x0003F110 File Offset: 0x0003E110
		private void GenerateSetCapabilitiesCode(BrowserDefinition bd, CodeMemberMethod cmm, ref bool regexWorkerGenerated)
		{
			NameValueCollection capabilities = bd.Capabilities;
			this.AddComment("Capabilities: set capabilities", cmm);
			foreach (object obj in capabilities.Keys)
			{
				string text = (string)obj;
				string text2 = capabilities[text];
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = new CodeIndexerExpression(this._dictionaryRefExpr, new CodeExpression[]
				{
					new CodePrimitiveExpression(text)
				});
				CodePrimitiveExpression codePrimitiveExpression = new CodePrimitiveExpression(text2);
				if (RegexWorker.RefPat.Match(text2).Success)
				{
					this.GenerateRegexWorkerIfNecessary(cmm, ref regexWorkerGenerated);
					codeAssignStatement.Right = new CodeIndexerExpression(this._regexWorkerRefExpr, new CodeExpression[] { codePrimitiveExpression });
				}
				else
				{
					codeAssignStatement.Right = codePrimitiveExpression;
				}
				cmm.Statements.Add(codeAssignStatement);
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x0003F210 File Offset: 0x0003E210
		internal void GenerateSetAdaptersCode(BrowserDefinition bd, CodeMemberMethod cmm)
		{
			foreach (object obj in bd.Adapters)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = (string)dictionaryEntry.Key;
				string text2 = (string)dictionaryEntry.Value;
				CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(this._browserCapsRefExpr, "Adapters");
				CodeIndexerExpression codeIndexerExpression = new CodeIndexerExpression(codePropertyReferenceExpression, new CodeExpression[]
				{
					new CodePrimitiveExpression(text)
				});
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = codeIndexerExpression;
				codeAssignStatement.Right = new CodePrimitiveExpression(text2);
				cmm.Statements.Add(codeAssignStatement);
			}
			if (bd.HtmlTextWriterString != null)
			{
				CodeAssignStatement codeAssignStatement2 = new CodeAssignStatement();
				codeAssignStatement2.Left = new CodePropertyReferenceExpression(this._browserCapsRefExpr, "HtmlTextWriter");
				codeAssignStatement2.Right = new CodePrimitiveExpression(bd.HtmlTextWriterString);
				cmm.Statements.Add(codeAssignStatement2);
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x0003F324 File Offset: 0x0003E324
		internal void AddComment(string comment, CodeMemberMethod cmm)
		{
			cmm.Statements.Add(new CodeCommentStatement(comment));
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0003F338 File Offset: 0x0003E338
		internal CodeStatementCollection GenerateTrackedSingleProcessCall(CodeStatementCollection stmts, BrowserDefinition bd, CodeMemberMethod cmm)
		{
			return this.GenerateTrackedSingleProcessCall(stmts, bd, cmm, string.Empty);
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x0003F348 File Offset: 0x0003E348
		internal CodeStatementCollection GenerateTrackedSingleProcessCall(CodeStatementCollection stmts, BrowserDefinition bd, CodeMemberMethod cmm, string prefix)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), prefix + bd.Name + "Process", new CodeExpression[0]);
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression("headers"));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression("browserCaps"));
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = codeMethodInvokeExpression;
			stmts.Add(codeConditionStatement);
			return codeConditionStatement.FalseStatements;
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0003F3BF File Offset: 0x0003E3BF
		internal void GenerateSingleProcessCall(BrowserDefinition bd, CodeMemberMethod cmm)
		{
			this.GenerateSingleProcessCall(bd, cmm, string.Empty);
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x0003F3D0 File Offset: 0x0003E3D0
		internal void GenerateSingleProcessCall(BrowserDefinition bd, CodeMemberMethod cmm, string prefix)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), prefix + bd.Name + "Process", new CodeExpression[0]);
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression("headers"));
			codeMethodInvokeExpression.Parameters.Add(new CodeVariableReferenceExpression("browserCaps"));
			cmm.Statements.Add(codeMethodInvokeExpression);
		}

		// Token: 0x04001537 RID: 5431
		internal const string browserCapsVariable = "browserCaps";

		// Token: 0x04001538 RID: 5432
		internal const string IgnoreApplicationBrowserVariableName = "ignoreApplicationBrowsers";

		// Token: 0x04001539 RID: 5433
		private const string _factoryTypeName = "BrowserCapabilitiesFactory";

		// Token: 0x0400153A RID: 5434
		private const string _headerDictionaryVarName = "_headerDictionary";

		// Token: 0x0400153B RID: 5435
		private const string _disableOptimizedCacheKeyMethodName = "DisableOptimizedCacheKey";

		// Token: 0x0400153C RID: 5436
		private const string _matchedHeadersMethodName = "PopulateMatchedHeaders";

		// Token: 0x0400153D RID: 5437
		private const string _browserElementsMethodName = "PopulateBrowserElements";

		// Token: 0x0400153E RID: 5438
		private const string _dictionaryRefName = "dictionary";

		// Token: 0x0400153F RID: 5439
		private const string _regexWorkerRefName = "regexWorker";

		// Token: 0x04001540 RID: 5440
		private const string _headersRefName = "headers";

		// Token: 0x04001541 RID: 5441
		private const string _resultVarName = "result";

		// Token: 0x04001542 RID: 5442
		private const string _processRegexMethod = "ProcessRegex";

		// Token: 0x04001543 RID: 5443
		private static readonly string _browsersDirectory = HttpRuntime.ClrInstallDirectoryInternal + "\\config\\browsers";

		// Token: 0x04001544 RID: 5444
		private static readonly string _publicKeyTokenFile = BrowserCapabilitiesCodeGenerator._browsersDirectory + "\\" + BrowserCapabilitiesCodeGenerator._publicKeyTokenFileName;

		// Token: 0x04001545 RID: 5445
		private static object _staticLock = new object();

		// Token: 0x04001546 RID: 5446
		private BrowserTree _browserTree;

		// Token: 0x04001547 RID: 5447
		private BrowserTree _defaultTree;

		// Token: 0x04001548 RID: 5448
		private BrowserDefinitionCollection _browserDefinitionCollection;

		// Token: 0x04001549 RID: 5449
		private static readonly string _strongNameKeyFileName = "browserCaps.snk";

		// Token: 0x0400154A RID: 5450
		private static readonly string _publicKeyTokenFileName = "browserCaps.token";

		// Token: 0x0400154B RID: 5451
		private static bool _publicKeyTokenLoaded;

		// Token: 0x0400154C RID: 5452
		private static string _publicKeyToken;

		// Token: 0x0400154D RID: 5453
		private CodeVariableReferenceExpression _dictionaryRefExpr = new CodeVariableReferenceExpression("dictionary");

		// Token: 0x0400154E RID: 5454
		private CodeVariableReferenceExpression _regexWorkerRefExpr = new CodeVariableReferenceExpression("regexWorker");

		// Token: 0x0400154F RID: 5455
		private CodeVariableReferenceExpression _headersRefExpr = new CodeVariableReferenceExpression("headers");

		// Token: 0x04001550 RID: 5456
		private CodeVariableReferenceExpression _browserCapsRefExpr = new CodeVariableReferenceExpression("browserCaps");

		// Token: 0x04001551 RID: 5457
		private ArrayList _browserFileList;

		// Token: 0x04001552 RID: 5458
		private ArrayList _customBrowserFileLists;

		// Token: 0x04001553 RID: 5459
		private ArrayList _customTreeList;

		// Token: 0x04001554 RID: 5460
		private ArrayList _customTreeNames;

		// Token: 0x04001555 RID: 5461
		private ArrayList _customBrowserDefinitionCollections;

		// Token: 0x04001556 RID: 5462
		private CaseInsensitiveStringSet _headers;
	}
}
