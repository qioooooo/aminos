using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x02000745 RID: 1861
	internal static class RemotingXmlConfigFileParser
	{
		// Token: 0x06004285 RID: 17029 RVA: 0x000E2AF3 File Offset: 0x000E1AF3
		private static Hashtable CreateSyncCaseInsensitiveHashtable()
		{
			return Hashtable.Synchronized(RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable());
		}

		// Token: 0x06004286 RID: 17030 RVA: 0x000E2AFF File Offset: 0x000E1AFF
		private static Hashtable CreateCaseInsensitiveHashtable()
		{
			return new Hashtable(StringComparer.InvariantCultureIgnoreCase);
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x000E2B0C File Offset: 0x000E1B0C
		public static RemotingXmlConfigFileData ParseDefaultConfiguration()
		{
			ConfigNode configNode = new ConfigNode("system.runtime.remoting", null);
			ConfigNode configNode2 = new ConfigNode("application", configNode);
			configNode.Children.Add(configNode2);
			ConfigNode configNode3 = new ConfigNode("channels", configNode2);
			configNode2.Children.Add(configNode3);
			ConfigNode configNode4 = new ConfigNode("channel", configNode2);
			configNode4.Attributes.Add(new DictionaryEntry("ref", "http client"));
			configNode4.Attributes.Add(new DictionaryEntry("displayName", "http client (delay loaded)"));
			configNode4.Attributes.Add(new DictionaryEntry("delayLoadAsClientChannel", "true"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode2);
			configNode4.Attributes.Add(new DictionaryEntry("ref", "tcp client"));
			configNode4.Attributes.Add(new DictionaryEntry("displayName", "tcp client (delay loaded)"));
			configNode4.Attributes.Add(new DictionaryEntry("delayLoadAsClientChannel", "true"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode2);
			configNode4.Attributes.Add(new DictionaryEntry("ref", "ipc client"));
			configNode4.Attributes.Add(new DictionaryEntry("displayName", "ipc client (delay loaded)"));
			configNode4.Attributes.Add(new DictionaryEntry("delayLoadAsClientChannel", "true"));
			configNode3.Children.Add(configNode4);
			configNode3 = new ConfigNode("channels", configNode);
			configNode.Children.Add(configNode3);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "http"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Http.HttpChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "http client"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Http.HttpClientChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "http server"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Http.HttpServerChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "tcp"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Tcp.TcpChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "tcp client"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Tcp.TcpClientChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "tcp server"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Tcp.TcpServerChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "ipc"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Ipc.IpcChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "ipc client"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Ipc.IpcClientChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			configNode4 = new ConfigNode("channel", configNode3);
			configNode4.Attributes.Add(new DictionaryEntry("id", "ipc server"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.Ipc.IpcServerChannel, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode3.Children.Add(configNode4);
			ConfigNode configNode5 = new ConfigNode("channelSinkProviders", configNode);
			configNode.Children.Add(configNode5);
			ConfigNode configNode6 = new ConfigNode("clientProviders", configNode5);
			configNode5.Children.Add(configNode6);
			configNode4 = new ConfigNode("formatter", configNode6);
			configNode4.Attributes.Add(new DictionaryEntry("id", "soap"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.SoapClientFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode6.Children.Add(configNode4);
			configNode4 = new ConfigNode("formatter", configNode6);
			configNode4.Attributes.Add(new DictionaryEntry("id", "binary"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.BinaryClientFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode6.Children.Add(configNode4);
			ConfigNode configNode7 = new ConfigNode("serverProviders", configNode5);
			configNode5.Children.Add(configNode7);
			configNode4 = new ConfigNode("formatter", configNode7);
			configNode4.Attributes.Add(new DictionaryEntry("id", "soap"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.SoapServerFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode7.Children.Add(configNode4);
			configNode4 = new ConfigNode("formatter", configNode7);
			configNode4.Attributes.Add(new DictionaryEntry("id", "binary"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.Channels.BinaryServerFormatterSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode7.Children.Add(configNode4);
			configNode4 = new ConfigNode("provider", configNode7);
			configNode4.Attributes.Add(new DictionaryEntry("id", "wsdl"));
			configNode4.Attributes.Add(new DictionaryEntry("type", "System.Runtime.Remoting.MetadataServices.SdlChannelSinkProvider, System.Runtime.Remoting, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
			configNode7.Children.Add(configNode4);
			return RemotingXmlConfigFileParser.ParseConfigNode(configNode);
		}

		// Token: 0x06004288 RID: 17032 RVA: 0x000E3220 File Offset: 0x000E2220
		public static RemotingXmlConfigFileData ParseConfigFile(string filename)
		{
			ConfigTreeParser configTreeParser = new ConfigTreeParser();
			ConfigNode configNode = configTreeParser.Parse(filename, "/configuration/system.runtime.remoting");
			return RemotingXmlConfigFileParser.ParseConfigNode(configNode);
		}

		// Token: 0x06004289 RID: 17033 RVA: 0x000E3248 File Offset: 0x000E2248
		private static RemotingXmlConfigFileData ParseConfigNode(ConfigNode rootNode)
		{
			RemotingXmlConfigFileData remotingXmlConfigFileData = new RemotingXmlConfigFileData();
			if (rootNode == null)
			{
				return null;
			}
			foreach (object obj in rootNode.Attributes)
			{
				string text = ((DictionaryEntry)obj).Key.ToString();
				string text2;
				if ((text2 = text) != null)
				{
					text2 == "version";
				}
			}
			ConfigNode configNode = null;
			ConfigNode configNode2 = null;
			ConfigNode configNode3 = null;
			ConfigNode configNode4 = null;
			ConfigNode configNode5 = null;
			foreach (object obj2 in rootNode.Children)
			{
				ConfigNode configNode6 = (ConfigNode)obj2;
				string name;
				if ((name = configNode6.Name) != null)
				{
					if (!(name == "application"))
					{
						if (!(name == "channels"))
						{
							if (!(name == "channelSinkProviders"))
							{
								if (!(name == "debug"))
								{
									if (name == "customErrors")
									{
										if (configNode5 != null)
										{
											RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode5, remotingXmlConfigFileData);
										}
										configNode5 = configNode6;
									}
								}
								else
								{
									if (configNode4 != null)
									{
										RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode4, remotingXmlConfigFileData);
									}
									configNode4 = configNode6;
								}
							}
							else
							{
								if (configNode3 != null)
								{
									RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode3, remotingXmlConfigFileData);
								}
								configNode3 = configNode6;
							}
						}
						else
						{
							if (configNode2 != null)
							{
								RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode2, remotingXmlConfigFileData);
							}
							configNode2 = configNode6;
						}
					}
					else
					{
						if (configNode != null)
						{
							RemotingXmlConfigFileParser.ReportUniqueSectionError(rootNode, configNode, remotingXmlConfigFileData);
						}
						configNode = configNode6;
					}
				}
			}
			if (configNode4 != null)
			{
				RemotingXmlConfigFileParser.ProcessDebugNode(configNode4, remotingXmlConfigFileData);
			}
			if (configNode3 != null)
			{
				RemotingXmlConfigFileParser.ProcessChannelSinkProviderTemplates(configNode3, remotingXmlConfigFileData);
			}
			if (configNode2 != null)
			{
				RemotingXmlConfigFileParser.ProcessChannelTemplates(configNode2, remotingXmlConfigFileData);
			}
			if (configNode != null)
			{
				RemotingXmlConfigFileParser.ProcessApplicationNode(configNode, remotingXmlConfigFileData);
			}
			if (configNode5 != null)
			{
				RemotingXmlConfigFileParser.ProcessCustomErrorsNode(configNode5, remotingXmlConfigFileData);
			}
			return remotingXmlConfigFileData;
		}

		// Token: 0x0600428A RID: 17034 RVA: 0x000E3418 File Offset: 0x000E2418
		private static void ReportError(string errorStr, RemotingXmlConfigFileData configData)
		{
			throw new RemotingException(errorStr);
		}

		// Token: 0x0600428B RID: 17035 RVA: 0x000E3420 File Offset: 0x000E2420
		private static void ReportUniqueSectionError(ConfigNode parent, ConfigNode child, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_NodeMustBeUnique"), new object[] { child.Name, parent.Name }), configData);
		}

		// Token: 0x0600428C RID: 17036 RVA: 0x000E3464 File Offset: 0x000E2464
		private static void ReportUnknownValueError(ConfigNode node, string value, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_UnknownValue"), new object[] { node.Name, value }), configData);
		}

		// Token: 0x0600428D RID: 17037 RVA: 0x000E34A0 File Offset: 0x000E24A0
		private static void ReportMissingAttributeError(ConfigNode node, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportMissingAttributeError(node.Name, attributeName, configData);
		}

		// Token: 0x0600428E RID: 17038 RVA: 0x000E34B0 File Offset: 0x000E24B0
		private static void ReportMissingAttributeError(string nodeDescription, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_RequiredXmlAttribute"), new object[] { nodeDescription, attributeName }), configData);
		}

		// Token: 0x0600428F RID: 17039 RVA: 0x000E34E8 File Offset: 0x000E24E8
		private static void ReportMissingTypeAttributeError(ConfigNode node, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_MissingTypeAttribute"), new object[] { node.Name, attributeName }), configData);
		}

		// Token: 0x06004290 RID: 17040 RVA: 0x000E3524 File Offset: 0x000E2524
		private static void ReportMissingXmlTypeAttributeError(ConfigNode node, string attributeName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_MissingXmlTypeAttribute"), new object[] { node.Name, attributeName }), configData);
		}

		// Token: 0x06004291 RID: 17041 RVA: 0x000E3560 File Offset: 0x000E2560
		private static void ReportInvalidTimeFormatError(string time, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_InvalidTimeFormat"), new object[] { time }), configData);
		}

		// Token: 0x06004292 RID: 17042 RVA: 0x000E3594 File Offset: 0x000E2594
		private static void ReportNonTemplateIdAttributeError(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_NonTemplateIdAttribute"), new object[] { node.Name }), configData);
		}

		// Token: 0x06004293 RID: 17043 RVA: 0x000E35CC File Offset: 0x000E25CC
		private static void ReportTemplateCannotReferenceTemplateError(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_TemplateCannotReferenceTemplate"), new object[] { node.Name }), configData);
		}

		// Token: 0x06004294 RID: 17044 RVA: 0x000E3604 File Offset: 0x000E2604
		private static void ReportUnableToResolveTemplateReferenceError(ConfigNode node, string referenceName, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_UnableToResolveTemplate"), new object[] { node.Name, referenceName }), configData);
		}

		// Token: 0x06004295 RID: 17045 RVA: 0x000E3640 File Offset: 0x000E2640
		private static void ReportAssemblyVersionInfoPresent(string assemName, string entryDescription, RemotingXmlConfigFileData configData)
		{
			RemotingXmlConfigFileParser.ReportError(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_Config_VersionPresent"), new object[] { assemName, entryDescription }), configData);
		}

		// Token: 0x06004296 RID: 17046 RVA: 0x000E3678 File Offset: 0x000E2678
		private static void ProcessDebugNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				string text2;
				if ((text2 = text) != null && text2 == "loadTypes")
				{
					RemotingXmlConfigFileData.LoadTypes = Convert.ToBoolean((string)dictionaryEntry.Value, CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x06004297 RID: 17047 RVA: 0x000E3708 File Offset: 0x000E2708
		private static void ProcessApplicationNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				if (text.Equals("name"))
				{
					configData.ApplicationName = (string)dictionaryEntry.Value;
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "channels"))
					{
						if (!(name == "client"))
						{
							if (!(name == "lifetime"))
							{
								if (!(name == "service"))
								{
									if (name == "soapInterop")
									{
										RemotingXmlConfigFileParser.ProcessSoapInteropNode(configNode, configData);
									}
								}
								else
								{
									RemotingXmlConfigFileParser.ProcessServiceNode(configNode, configData);
								}
							}
							else
							{
								RemotingXmlConfigFileParser.ProcessLifetimeNode(node, configNode, configData);
							}
						}
						else
						{
							RemotingXmlConfigFileParser.ProcessClientNode(configNode, configData);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessChannelsNode(configNode, configData);
					}
				}
			}
		}

		// Token: 0x06004298 RID: 17048 RVA: 0x000E3858 File Offset: 0x000E2858
		private static void ProcessCustomErrorsNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				if (text.Equals("mode"))
				{
					string text2 = (string)dictionaryEntry.Value;
					CustomErrorsModes customErrorsModes = CustomErrorsModes.On;
					if (string.Compare(text2, "on", StringComparison.OrdinalIgnoreCase) == 0)
					{
						customErrorsModes = CustomErrorsModes.On;
					}
					else if (string.Compare(text2, "off", StringComparison.OrdinalIgnoreCase) == 0)
					{
						customErrorsModes = CustomErrorsModes.Off;
					}
					else if (string.Compare(text2, "remoteonly", StringComparison.OrdinalIgnoreCase) == 0)
					{
						customErrorsModes = CustomErrorsModes.RemoteOnly;
					}
					else
					{
						RemotingXmlConfigFileParser.ReportUnknownValueError(node, text2, configData);
					}
					configData.CustomErrors = new RemotingXmlConfigFileData.CustomErrorsEntry(customErrorsModes);
				}
			}
		}

		// Token: 0x06004299 RID: 17049 RVA: 0x000E392C File Offset: 0x000E292C
		private static void ProcessLifetimeNode(ConfigNode parentNode, ConfigNode node, RemotingXmlConfigFileData configData)
		{
			if (configData.Lifetime != null)
			{
				RemotingXmlConfigFileParser.ReportUniqueSectionError(node, parentNode, configData);
			}
			configData.Lifetime = new RemotingXmlConfigFileData.LifetimeEntry();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				string text2;
				if ((text2 = text) != null)
				{
					if (!(text2 == "leaseTime"))
					{
						if (!(text2 == "sponsorshipTimeout"))
						{
							if (!(text2 == "renewOnCallTime"))
							{
								if (text2 == "leaseManagerPollTime")
								{
									configData.Lifetime.LeaseManagerPollTime = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
								}
							}
							else
							{
								configData.Lifetime.RenewOnCallTime = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
							}
						}
						else
						{
							configData.Lifetime.SponsorshipTimeout = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
						}
					}
					else
					{
						configData.Lifetime.LeaseTime = RemotingXmlConfigFileParser.ParseTime((string)dictionaryEntry.Value, configData);
					}
				}
			}
		}

		// Token: 0x0600429A RID: 17050 RVA: 0x000E3A68 File Offset: 0x000E2A68
		private static void ProcessServiceNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "wellknown"))
					{
						if (name == "activated")
						{
							RemotingXmlConfigFileParser.ProcessServiceActivatedNode(configNode, configData);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessServiceWellKnownNode(configNode, configData);
					}
				}
			}
		}

		// Token: 0x0600429B RID: 17051 RVA: 0x000E3AF0 File Offset: 0x000E2AF0
		private static void ProcessClientNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text2 = dictionaryEntry.Key.ToString();
				string text3;
				if ((text3 = text2) != null)
				{
					if (!(text3 == "url"))
					{
						if (!(text3 == "displayName"))
						{
						}
					}
					else
					{
						text = (string)dictionaryEntry.Value;
					}
				}
			}
			RemotingXmlConfigFileData.RemoteAppEntry remoteAppEntry = configData.AddRemoteAppEntry(text);
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "wellknown"))
					{
						if (name == "activated")
						{
							RemotingXmlConfigFileParser.ProcessClientActivatedNode(configNode, configData, remoteAppEntry);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessClientWellKnownNode(configNode, configData, remoteAppEntry);
					}
				}
			}
			if (remoteAppEntry.ActivatedObjects.Count > 0 && text == null)
			{
				RemotingXmlConfigFileParser.ReportMissingAttributeError(node, "url", configData);
			}
		}

		// Token: 0x0600429C RID: 17052 RVA: 0x000E3C38 File Offset: 0x000E2C38
		private static void ProcessSoapInteropNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text = dictionaryEntry.Key.ToString();
				string text2;
				if ((text2 = text) != null && text2 == "urlObjRef")
				{
					configData.UrlObjRefMode = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "preLoad"))
					{
						if (!(name == "interopXmlElement"))
						{
							if (name == "interopXmlType")
							{
								RemotingXmlConfigFileParser.ProcessInteropXmlTypeNode(configNode, configData);
							}
						}
						else
						{
							RemotingXmlConfigFileParser.ProcessInteropXmlElementNode(configNode, configData);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessPreLoadNode(configNode, configData);
					}
				}
			}
		}

		// Token: 0x0600429D RID: 17053 RVA: 0x000E3D60 File Offset: 0x000E2D60
		private static void ProcessChannelsNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				if (configNode.Name.Equals("channel"))
				{
					RemotingXmlConfigFileData.ChannelEntry channelEntry = RemotingXmlConfigFileParser.ProcessChannelsChannelNode(configNode, configData, false);
					configData.ChannelEntries.Add(channelEntry);
				}
			}
		}

		// Token: 0x0600429E RID: 17054 RVA: 0x000E3DDC File Offset: 0x000E2DDC
		private static void ProcessServiceWellKnownNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			ArrayList arrayList = new ArrayList();
			string text3 = null;
			WellKnownObjectMode wellKnownObjectMode = WellKnownObjectMode.Singleton;
			bool flag = false;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text4 = dictionaryEntry.Key.ToString();
				string text5;
				if ((text5 = text4) != null && !(text5 == "displayName"))
				{
					if (!(text5 == "mode"))
					{
						if (!(text5 == "objectUri"))
						{
							if (text5 == "type")
							{
								RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
							}
						}
						else
						{
							text3 = (string)dictionaryEntry.Value;
						}
					}
					else
					{
						string text6 = (string)dictionaryEntry.Value;
						flag = true;
						if (string.CompareOrdinal(text6, "Singleton") == 0)
						{
							wellKnownObjectMode = WellKnownObjectMode.Singleton;
						}
						else if (string.CompareOrdinal(text6, "SingleCall") == 0)
						{
							wellKnownObjectMode = WellKnownObjectMode.SingleCall;
						}
						else
						{
							flag = false;
						}
					}
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "contextAttribute"))
					{
						if (!(name == "lifetime"))
						{
						}
					}
					else
					{
						arrayList.Add(RemotingXmlConfigFileParser.ProcessContextAttributeNode(configNode, configData));
					}
				}
			}
			if (!flag)
			{
				RemotingXmlConfigFileParser.ReportError(Environment.GetResourceString("Remoting_Config_MissingWellKnownModeAttribute"), configData);
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			if (text3 == null)
			{
				text3 = text + ".soap";
			}
			configData.AddServerWellKnownEntry(text, text2, arrayList, text3, wellKnownObjectMode);
		}

		// Token: 0x0600429F RID: 17055 RVA: 0x000E3FC8 File Offset: 0x000E2FC8
		private static void ProcessServiceActivatedNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = dictionaryEntry.Key.ToString();
				string text4;
				if ((text4 = text3) != null && text4 == "type")
				{
					RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "contextAttribute"))
					{
						if (!(name == "lifetime"))
						{
						}
					}
					else
					{
						arrayList.Add(RemotingXmlConfigFileParser.ProcessContextAttributeNode(configNode, configData));
					}
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			if (RemotingXmlConfigFileParser.CheckAssemblyNameForVersionInfo(text2))
			{
				RemotingXmlConfigFileParser.ReportAssemblyVersionInfoPresent(text2, "service activated", configData);
			}
			configData.AddServerActivatedEntry(text, text2, arrayList);
		}

		// Token: 0x060042A0 RID: 17056 RVA: 0x000E4114 File Offset: 0x000E3114
		private static void ProcessClientWellKnownNode(ConfigNode node, RemotingXmlConfigFileData configData, RemotingXmlConfigFileData.RemoteAppEntry remoteApp)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text4 = dictionaryEntry.Key.ToString();
				string text5;
				if ((text5 = text4) != null && !(text5 == "displayName"))
				{
					if (!(text5 == "type"))
					{
						if (text5 == "url")
						{
							text3 = (string)dictionaryEntry.Value;
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
					}
				}
			}
			if (text3 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingAttributeError("WellKnown client", "url", configData);
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			if (RemotingXmlConfigFileParser.CheckAssemblyNameForVersionInfo(text2))
			{
				RemotingXmlConfigFileParser.ReportAssemblyVersionInfoPresent(text2, "client wellknown", configData);
			}
			remoteApp.AddWellKnownEntry(text, text2, text3);
		}

		// Token: 0x060042A1 RID: 17057 RVA: 0x000E421C File Offset: 0x000E321C
		private static void ProcessClientActivatedNode(ConfigNode node, RemotingXmlConfigFileData configData, RemotingXmlConfigFileData.RemoteAppEntry remoteApp)
		{
			string text = null;
			string text2 = null;
			ArrayList arrayList = new ArrayList();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = dictionaryEntry.Key.ToString();
				string text4;
				if ((text4 = text3) != null && text4 == "type")
				{
					RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
				}
			}
			foreach (object obj2 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj2;
				string name;
				if ((name = configNode.Name) != null && name == "contextAttribute")
				{
					arrayList.Add(RemotingXmlConfigFileParser.ProcessContextAttributeNode(configNode, configData));
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			remoteApp.AddActivatedEntry(text, text2, arrayList);
		}

		// Token: 0x060042A2 RID: 17058 RVA: 0x000E4344 File Offset: 0x000E3344
		private static void ProcessInteropXmlElementNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text5 = dictionaryEntry.Key.ToString();
				string text6;
				if ((text6 = text5) != null)
				{
					if (!(text6 == "xml"))
					{
						if (text6 == "clr")
						{
							RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text3, out text4);
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
					}
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingXmlTypeAttributeError(node, "xml", configData);
			}
			if (text3 == null || text4 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "clr", configData);
			}
			configData.AddInteropXmlElementEntry(text, text2, text3, text4);
		}

		// Token: 0x060042A3 RID: 17059 RVA: 0x000E4434 File Offset: 0x000E3434
		private static void ProcessInteropXmlTypeNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text5 = dictionaryEntry.Key.ToString();
				string text6;
				if ((text6 = text5) != null)
				{
					if (!(text6 == "xml"))
					{
						if (text6 == "clr")
						{
							RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text3, out text4);
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
					}
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingXmlTypeAttributeError(node, "xml", configData);
			}
			if (text3 == null || text4 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "clr", configData);
			}
			configData.AddInteropXmlTypeEntry(text, text2, text3, text4);
		}

		// Token: 0x060042A4 RID: 17060 RVA: 0x000E4524 File Offset: 0x000E3524
		private static void ProcessPreLoadNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = dictionaryEntry.Key.ToString();
				string text4;
				if ((text4 = text3) != null)
				{
					if (!(text4 == "type"))
					{
						if (text4 == "assembly")
						{
							text2 = (string)dictionaryEntry.Value;
						}
					}
					else
					{
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
					}
				}
			}
			if (text2 == null)
			{
				RemotingXmlConfigFileParser.ReportError(Environment.GetResourceString("Remoting_Config_PreloadRequiresTypeOrAssembly"), configData);
			}
			configData.AddPreLoadEntry(text, text2);
		}

		// Token: 0x060042A5 RID: 17061 RVA: 0x000E45F4 File Offset: 0x000E35F4
		private static RemotingXmlConfigFileData.ContextAttributeEntry ProcessContextAttributeNode(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			string text = null;
			string text2 = null;
			Hashtable hashtable = RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable();
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text3 = ((string)dictionaryEntry.Key).ToLower(CultureInfo.InvariantCulture);
				string text4;
				if ((text4 = text3) != null && text4 == "type")
				{
					RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text, out text2);
				}
				else
				{
					hashtable[text3] = dictionaryEntry.Value;
				}
			}
			if (text == null || text2 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			return new RemotingXmlConfigFileData.ContextAttributeEntry(text, text2, hashtable);
		}

		// Token: 0x060042A6 RID: 17062 RVA: 0x000E46C8 File Offset: 0x000E36C8
		private static RemotingXmlConfigFileData.ChannelEntry ProcessChannelsChannelNode(ConfigNode node, RemotingXmlConfigFileData configData, bool isTemplate)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			Hashtable hashtable = RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable();
			bool flag = false;
			RemotingXmlConfigFileData.ChannelEntry channelEntry = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text4 = (string)dictionaryEntry.Key;
				string text5;
				if ((text5 = text4) != null)
				{
					if (text5 == "displayName")
					{
						continue;
					}
					if (!(text5 == "id"))
					{
						if (!(text5 == "ref"))
						{
							if (!(text5 == "type"))
							{
								if (!(text5 == "delayLoadAsClientChannel"))
								{
									goto IL_019F;
								}
								flag = Convert.ToBoolean((string)dictionaryEntry.Value, CultureInfo.InvariantCulture);
								continue;
							}
						}
						else
						{
							if (isTemplate)
							{
								RemotingXmlConfigFileParser.ReportTemplateCannotReferenceTemplateError(node, configData);
								continue;
							}
							channelEntry = (RemotingXmlConfigFileData.ChannelEntry)RemotingXmlConfigFileParser._channelTemplates[dictionaryEntry.Value];
							if (channelEntry == null)
							{
								RemotingXmlConfigFileParser.ReportUnableToResolveTemplateReferenceError(node, dictionaryEntry.Value.ToString(), configData);
								continue;
							}
							text2 = channelEntry.TypeName;
							text3 = channelEntry.AssemblyName;
							using (IDictionaryEnumerator enumerator2 = channelEntry.Properties.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
									hashtable[dictionaryEntry2.Key] = dictionaryEntry2.Value;
								}
								continue;
							}
						}
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text2, out text3);
						continue;
					}
					if (!isTemplate)
					{
						RemotingXmlConfigFileParser.ReportNonTemplateIdAttributeError(node, configData);
						continue;
					}
					text = ((string)dictionaryEntry.Value).ToLower(CultureInfo.InvariantCulture);
					continue;
				}
				IL_019F:
				hashtable[text4] = dictionaryEntry.Value;
			}
			if (text2 == null || text3 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			RemotingXmlConfigFileData.ChannelEntry channelEntry2 = new RemotingXmlConfigFileData.ChannelEntry(text2, text3, hashtable);
			channelEntry2.DelayLoad = flag;
			foreach (object obj3 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj3;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "clientProviders"))
					{
						if (name == "serverProviders")
						{
							RemotingXmlConfigFileParser.ProcessSinkProviderNodes(configNode, channelEntry2, configData, true);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessSinkProviderNodes(configNode, channelEntry2, configData, false);
					}
				}
			}
			if (channelEntry != null)
			{
				if (channelEntry2.ClientSinkProviders.Count == 0)
				{
					channelEntry2.ClientSinkProviders = channelEntry.ClientSinkProviders;
				}
				if (channelEntry2.ServerSinkProviders.Count == 0)
				{
					channelEntry2.ServerSinkProviders = channelEntry.ServerSinkProviders;
				}
			}
			if (isTemplate)
			{
				RemotingXmlConfigFileParser._channelTemplates[text] = channelEntry2;
				return null;
			}
			return channelEntry2;
		}

		// Token: 0x060042A7 RID: 17063 RVA: 0x000E49E8 File Offset: 0x000E39E8
		private static void ProcessSinkProviderNodes(ConfigNode node, RemotingXmlConfigFileData.ChannelEntry channelEntry, RemotingXmlConfigFileData configData, bool isServer)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				RemotingXmlConfigFileData.SinkProviderEntry sinkProviderEntry = RemotingXmlConfigFileParser.ProcessSinkProviderNode(configNode, configData, false, isServer);
				if (isServer)
				{
					channelEntry.ServerSinkProviders.Add(sinkProviderEntry);
				}
				else
				{
					channelEntry.ClientSinkProviders.Add(sinkProviderEntry);
				}
			}
		}

		// Token: 0x060042A8 RID: 17064 RVA: 0x000E4A64 File Offset: 0x000E3A64
		private static RemotingXmlConfigFileData.SinkProviderEntry ProcessSinkProviderNode(ConfigNode node, RemotingXmlConfigFileData configData, bool isTemplate, bool isServer)
		{
			bool flag = false;
			string name = node.Name;
			if (name.Equals("formatter"))
			{
				flag = true;
			}
			else if (name.Equals("provider"))
			{
				flag = false;
			}
			else
			{
				RemotingXmlConfigFileParser.ReportError(Environment.GetResourceString("Remoting_Config_ProviderNeedsElementName"), configData);
			}
			string text = null;
			string text2 = null;
			string text3 = null;
			Hashtable hashtable = RemotingXmlConfigFileParser.CreateCaseInsensitiveHashtable();
			RemotingXmlConfigFileData.SinkProviderEntry sinkProviderEntry = null;
			foreach (object obj in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				string text4 = (string)dictionaryEntry.Key;
				string text5;
				if ((text5 = text4) != null)
				{
					if (!(text5 == "id"))
					{
						if (!(text5 == "ref"))
						{
							if (!(text5 == "type"))
							{
								goto IL_01B2;
							}
						}
						else
						{
							if (isTemplate)
							{
								RemotingXmlConfigFileParser.ReportTemplateCannotReferenceTemplateError(node, configData);
								continue;
							}
							if (isServer)
							{
								sinkProviderEntry = (RemotingXmlConfigFileData.SinkProviderEntry)RemotingXmlConfigFileParser._serverChannelSinkTemplates[dictionaryEntry.Value];
							}
							else
							{
								sinkProviderEntry = (RemotingXmlConfigFileData.SinkProviderEntry)RemotingXmlConfigFileParser._clientChannelSinkTemplates[dictionaryEntry.Value];
							}
							if (sinkProviderEntry == null)
							{
								RemotingXmlConfigFileParser.ReportUnableToResolveTemplateReferenceError(node, dictionaryEntry.Value.ToString(), configData);
								continue;
							}
							text2 = sinkProviderEntry.TypeName;
							text3 = sinkProviderEntry.AssemblyName;
							using (IDictionaryEnumerator enumerator2 = sinkProviderEntry.Properties.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
									hashtable[dictionaryEntry2.Key] = dictionaryEntry2.Value;
								}
								continue;
							}
						}
						RemotingConfigHandler.ParseType((string)dictionaryEntry.Value, out text2, out text3);
						continue;
					}
					if (!isTemplate)
					{
						RemotingXmlConfigFileParser.ReportNonTemplateIdAttributeError(node, configData);
						continue;
					}
					text = (string)dictionaryEntry.Value;
					continue;
				}
				IL_01B2:
				hashtable[text4] = dictionaryEntry.Value;
			}
			if (text2 == null || text3 == null)
			{
				RemotingXmlConfigFileParser.ReportMissingTypeAttributeError(node, "type", configData);
			}
			RemotingXmlConfigFileData.SinkProviderEntry sinkProviderEntry2 = new RemotingXmlConfigFileData.SinkProviderEntry(text2, text3, hashtable, flag);
			foreach (object obj3 in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj3;
				SinkProviderData sinkProviderData = RemotingXmlConfigFileParser.ProcessSinkProviderData(configNode, configData);
				sinkProviderEntry2.ProviderData.Add(sinkProviderData);
			}
			if (sinkProviderEntry != null && sinkProviderEntry2.ProviderData.Count == 0)
			{
				sinkProviderEntry2.ProviderData = sinkProviderEntry.ProviderData;
			}
			if (isTemplate)
			{
				if (isServer)
				{
					RemotingXmlConfigFileParser._serverChannelSinkTemplates[text] = sinkProviderEntry2;
				}
				else
				{
					RemotingXmlConfigFileParser._clientChannelSinkTemplates[text] = sinkProviderEntry2;
				}
				return null;
			}
			return sinkProviderEntry2;
		}

		// Token: 0x060042A9 RID: 17065 RVA: 0x000E4D60 File Offset: 0x000E3D60
		private static SinkProviderData ProcessSinkProviderData(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			SinkProviderData sinkProviderData = new SinkProviderData(node.Name);
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				SinkProviderData sinkProviderData2 = RemotingXmlConfigFileParser.ProcessSinkProviderData(configNode, configData);
				sinkProviderData.Children.Add(sinkProviderData2);
			}
			foreach (object obj2 in node.Attributes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				sinkProviderData.Properties[dictionaryEntry.Key] = dictionaryEntry.Value;
			}
			return sinkProviderData;
		}

		// Token: 0x060042AA RID: 17066 RVA: 0x000E4E3C File Offset: 0x000E3E3C
		private static void ProcessChannelTemplates(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				string name;
				if ((name = configNode.Name) != null && name == "channel")
				{
					RemotingXmlConfigFileParser.ProcessChannelsChannelNode(configNode, configData, true);
				}
			}
		}

		// Token: 0x060042AB RID: 17067 RVA: 0x000E4EB0 File Offset: 0x000E3EB0
		private static void ProcessChannelSinkProviderTemplates(ConfigNode node, RemotingXmlConfigFileData configData)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				string name;
				if ((name = configNode.Name) != null)
				{
					if (!(name == "clientProviders"))
					{
						if (name == "serverProviders")
						{
							RemotingXmlConfigFileParser.ProcessChannelProviderTemplates(configNode, configData, true);
						}
					}
					else
					{
						RemotingXmlConfigFileParser.ProcessChannelProviderTemplates(configNode, configData, false);
					}
				}
			}
		}

		// Token: 0x060042AC RID: 17068 RVA: 0x000E4F3C File Offset: 0x000E3F3C
		private static void ProcessChannelProviderTemplates(ConfigNode node, RemotingXmlConfigFileData configData, bool isServer)
		{
			foreach (object obj in node.Children)
			{
				ConfigNode configNode = (ConfigNode)obj;
				RemotingXmlConfigFileParser.ProcessSinkProviderNode(configNode, configData, true, isServer);
			}
		}

		// Token: 0x060042AD RID: 17069 RVA: 0x000E4F98 File Offset: 0x000E3F98
		private static bool CheckAssemblyNameForVersionInfo(string assemName)
		{
			if (assemName == null)
			{
				return false;
			}
			int num = assemName.IndexOf(',');
			return num != -1;
		}

		// Token: 0x060042AE RID: 17070 RVA: 0x000E4FBC File Offset: 0x000E3FBC
		private static TimeSpan ParseTime(string time, RemotingXmlConfigFileData configData)
		{
			string text = time;
			string text2 = "s";
			int num = 0;
			char c = ' ';
			if (time.Length > 0)
			{
				c = time[time.Length - 1];
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds(0.0);
			try
			{
				if (!char.IsDigit(c))
				{
					if (time.Length == 0)
					{
						RemotingXmlConfigFileParser.ReportInvalidTimeFormatError(text, configData);
					}
					time = time.ToLower(CultureInfo.InvariantCulture);
					num = 1;
					if (time.EndsWith("ms", StringComparison.Ordinal))
					{
						num = 2;
					}
					text2 = time.Substring(time.Length - num, num);
				}
				int num2 = int.Parse(time.Substring(0, time.Length - num), CultureInfo.InvariantCulture);
				string text3;
				if ((text3 = text2) != null)
				{
					if (text3 == "d")
					{
						timeSpan = TimeSpan.FromDays((double)num2);
						goto IL_012A;
					}
					if (text3 == "h")
					{
						timeSpan = TimeSpan.FromHours((double)num2);
						goto IL_012A;
					}
					if (text3 == "m")
					{
						timeSpan = TimeSpan.FromMinutes((double)num2);
						goto IL_012A;
					}
					if (text3 == "s")
					{
						timeSpan = TimeSpan.FromSeconds((double)num2);
						goto IL_012A;
					}
					if (text3 == "ms")
					{
						timeSpan = TimeSpan.FromMilliseconds((double)num2);
						goto IL_012A;
					}
				}
				RemotingXmlConfigFileParser.ReportInvalidTimeFormatError(text, configData);
				IL_012A:;
			}
			catch (Exception)
			{
				RemotingXmlConfigFileParser.ReportInvalidTimeFormatError(text, configData);
			}
			return timeSpan;
		}

		// Token: 0x0400216F RID: 8559
		private static Hashtable _channelTemplates = RemotingXmlConfigFileParser.CreateSyncCaseInsensitiveHashtable();

		// Token: 0x04002170 RID: 8560
		private static Hashtable _clientChannelSinkTemplates = RemotingXmlConfigFileParser.CreateSyncCaseInsensitiveHashtable();

		// Token: 0x04002171 RID: 8561
		private static Hashtable _serverChannelSinkTemplates = RemotingXmlConfigFileParser.CreateSyncCaseInsensitiveHashtable();
	}
}
