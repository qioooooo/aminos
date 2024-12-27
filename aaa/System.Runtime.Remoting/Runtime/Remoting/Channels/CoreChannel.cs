using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web;
using Microsoft.Win32;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200000F RID: 15
	internal static class CoreChannel
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002EE1 File Offset: 0x00001EE1
		internal static IByteBufferPool BufferPool
		{
			get
			{
				return CoreChannel._bufferPool;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002EE8 File Offset: 0x00001EE8
		internal static RequestQueue RequestQueue
		{
			get
			{
				return CoreChannel._requestQueue;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002EF0 File Offset: 0x00001EF0
		internal static bool IsClientSKUInstallation
		{
			get
			{
				if (!CoreChannel.s_isClientSKUInstallationInitialized)
				{
					bool flag = false;
					string text = "SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v2.0.50727";
					new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\" + text).Assert();
					try
					{
						RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text);
						if (registryKey != null)
						{
							object value = registryKey.GetValue("Install");
							if (value is int)
							{
								flag = (int)value == 1;
							}
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					bool flag2 = false;
					if (!flag)
					{
						string text2 = "SOFTWARE\\Microsoft\\NET Framework Setup\\DotNetClient\\v3.5";
						new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\" + text2).Assert();
						try
						{
							RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(text2);
							if (registryKey2 != null)
							{
								object value2 = registryKey2.GetValue("Install");
								if (value2 is int)
								{
									flag2 = (int)value2 == 1;
								}
							}
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					CoreChannel.s_isClientSKUInstallation = flag2;
					CoreChannel.s_isClientSKUInstallationInitialized = true;
				}
				return CoreChannel.s_isClientSKUInstallation;
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002FEC File Offset: 0x00001FEC
		internal static string GetHostName()
		{
			if (CoreChannel.s_hostName == null)
			{
				CoreChannel.s_hostName = Dns.GetHostName();
				if (CoreChannel.s_hostName == null)
				{
					throw new ArgumentNullException("hostName");
				}
			}
			return CoreChannel.s_hostName;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003018 File Offset: 0x00002018
		internal static string GetMachineName()
		{
			if (CoreChannel.s_MachineName == null)
			{
				string hostName = CoreChannel.GetHostName();
				if (hostName != null)
				{
					IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
					if (hostEntry != null)
					{
						CoreChannel.s_MachineName = hostEntry.HostName;
					}
				}
				if (CoreChannel.s_MachineName == null)
				{
					throw new ArgumentNullException("machine");
				}
			}
			return CoreChannel.s_MachineName;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003064 File Offset: 0x00002064
		internal static bool IsLocalIpAddress(IPAddress remoteAddress)
		{
			if (CoreChannel.s_MachineIpAddress == null)
			{
				string machineName = CoreChannel.GetMachineName();
				IPHostEntry hostEntry = Dns.GetHostEntry(machineName);
				if (hostEntry == null || hostEntry.AddressList.Length != 1)
				{
					return CoreChannel.IsLocalIpAddress(hostEntry, remoteAddress.AddressFamily, remoteAddress);
				}
				if (Socket.SupportsIPv4)
				{
					CoreChannel.s_MachineIpAddress = CoreChannel.GetMachineAddress(hostEntry, AddressFamily.InterNetwork);
				}
				else
				{
					CoreChannel.s_MachineIpAddress = CoreChannel.GetMachineAddress(hostEntry, AddressFamily.InterNetworkV6);
				}
			}
			return CoreChannel.s_MachineIpAddress.Equals(remoteAddress);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x000030D0 File Offset: 0x000020D0
		internal static bool IsLocalIpAddress(IPHostEntry host, AddressFamily addressFamily, IPAddress remoteAddress)
		{
			if (host != null)
			{
				IPAddress[] addressList = host.AddressList;
				for (int i = 0; i < addressList.Length; i++)
				{
					if (addressList[i].AddressFamily == addressFamily && addressList[i].Equals(remoteAddress))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000310E File Offset: 0x0000210E
		internal static string DecodeMachineName(string machineName)
		{
			if (machineName.Equals("$hostName"))
			{
				return CoreChannel.GetHostName();
			}
			return machineName;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003124 File Offset: 0x00002124
		internal static string GetMachineIp()
		{
			if (CoreChannel.s_MachineIp == null)
			{
				string machineName = CoreChannel.GetMachineName();
				IPHostEntry hostEntry = Dns.GetHostEntry(machineName);
				AddressFamily addressFamily = (Socket.SupportsIPv4 ? AddressFamily.InterNetwork : AddressFamily.InterNetworkV6);
				IPAddress machineAddress = CoreChannel.GetMachineAddress(hostEntry, addressFamily);
				if (machineAddress != null)
				{
					CoreChannel.s_MachineIp = machineAddress.ToString();
				}
				if (CoreChannel.s_MachineIp == null)
				{
					throw new ArgumentNullException("ip");
				}
			}
			return CoreChannel.s_MachineIp;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003180 File Offset: 0x00002180
		internal static IPAddress GetMachineAddress(IPHostEntry host, AddressFamily addressFamily)
		{
			IPAddress ipaddress = null;
			if (host != null)
			{
				IPAddress[] addressList = host.AddressList;
				for (int i = 0; i < addressList.Length; i++)
				{
					if (addressList[i].AddressFamily == addressFamily)
					{
						ipaddress = addressList[i];
						break;
					}
				}
			}
			return ipaddress;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000031BC File Offset: 0x000021BC
		internal static Header[] GetMessagePropertiesAsSoapHeader(IMessage reqMsg)
		{
			IDictionary properties = reqMsg.Properties;
			if (properties == null)
			{
				return null;
			}
			int count = properties.Count;
			if (count == 0)
			{
				return null;
			}
			IDictionaryEnumerator enumerator = properties.GetEnumerator();
			bool[] array = new bool[count];
			int num = 0;
			int num2 = 0;
			IMethodMessage methodMessage = (IMethodMessage)reqMsg;
			while (enumerator.MoveNext())
			{
				string text = (string)enumerator.Key;
				if (text.Length >= 2 && string.CompareOrdinal(text, 0, "__", 0, 2) == 0 && (text.Equals("__Args") || text.Equals("__OutArgs") || text.Equals("__Return") || text.Equals("__Uri") || text.Equals("__MethodName") || (text.Equals("__MethodSignature") && !RemotingServices.IsMethodOverloaded(methodMessage) && !methodMessage.HasVarArgs) || (text.Equals("__TypeName") || text.Equals("__Fault")) || (text.Equals("__CallContext") && (enumerator.Value == null || !((LogicalCallContext)enumerator.Value).HasInfo))))
				{
					num2++;
				}
				else
				{
					array[num2] = true;
					num2++;
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			Header[] array2 = new Header[num];
			enumerator.Reset();
			int num3 = 0;
			num2 = 0;
			while (enumerator.MoveNext())
			{
				object key = enumerator.Key;
				if (!array[num3])
				{
					num3++;
				}
				else
				{
					Header header = enumerator.Value as Header;
					if (header == null)
					{
						header = new Header((string)key, enumerator.Value, false, "http://schemas.microsoft.com/clr/soap/messageProperties");
					}
					if (num2 == array2.Length)
					{
						Header[] array3 = new Header[num2 + 1];
						Array.Copy(array2, array3, num2);
						array2 = array3;
					}
					array2[num2] = header;
					num2++;
					num3++;
				}
			}
			return array2;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000033AC File Offset: 0x000023AC
		internal static Header[] GetSoapHeaders(IMessage reqMsg)
		{
			return CoreChannel.GetMessagePropertiesAsSoapHeader(reqMsg);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000033C4 File Offset: 0x000023C4
		internal static SoapFormatter CreateSoapFormatter(bool serialize, bool includeVersions)
		{
			SoapFormatter soapFormatter = new SoapFormatter();
			if (serialize)
			{
				RemotingSurrogateSelector remotingSurrogateSelector = new RemotingSurrogateSelector();
				soapFormatter.SurrogateSelector = remotingSurrogateSelector;
				remotingSurrogateSelector.UseSoapFormat();
			}
			else
			{
				soapFormatter.SurrogateSelector = null;
			}
			soapFormatter.Context = new StreamingContext(StreamingContextStates.Other);
			soapFormatter.AssemblyFormat = (includeVersions ? FormatterAssemblyStyle.Full : FormatterAssemblyStyle.Simple);
			return soapFormatter;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003414 File Offset: 0x00002414
		internal static BinaryFormatter CreateBinaryFormatter(bool serialize, bool includeVersionsOrStrictBinding)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			if (serialize)
			{
				RemotingSurrogateSelector remotingSurrogateSelector = new RemotingSurrogateSelector();
				binaryFormatter.SurrogateSelector = remotingSurrogateSelector;
			}
			else
			{
				binaryFormatter.SurrogateSelector = null;
			}
			binaryFormatter.Context = new StreamingContext(StreamingContextStates.Other);
			binaryFormatter.AssemblyFormat = (includeVersionsOrStrictBinding ? FormatterAssemblyStyle.Full : FormatterAssemblyStyle.Simple);
			return binaryFormatter;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000345C File Offset: 0x0000245C
		internal static void SerializeSoapMessage(IMessage msg, Stream outputStream, bool includeVersions)
		{
			SoapFormatter soapFormatter = CoreChannel.CreateSoapFormatter(true, includeVersions);
			IMethodMessage methodMessage = msg as IMethodMessage;
			if (methodMessage != null)
			{
				MethodBase methodBase = methodMessage.MethodBase;
				if (methodBase != null)
				{
					Type declaringType = methodMessage.MethodBase.DeclaringType;
					SoapTypeAttribute soapTypeAttribute = (SoapTypeAttribute)InternalRemotingServices.GetCachedSoapAttribute(declaringType);
					if ((soapTypeAttribute.SoapOptions & SoapOption.AlwaysIncludeTypes) == SoapOption.AlwaysIncludeTypes)
					{
						soapFormatter.TypeFormat |= FormatterTypeStyle.TypesAlways;
					}
					if ((soapTypeAttribute.SoapOptions & SoapOption.XsdString) == SoapOption.XsdString)
					{
						soapFormatter.TypeFormat |= FormatterTypeStyle.XsdString;
					}
				}
			}
			Header[] soapHeaders = CoreChannel.GetSoapHeaders(msg);
			((RemotingSurrogateSelector)soapFormatter.SurrogateSelector).SetRootObject(msg);
			soapFormatter.Serialize(outputStream, msg, soapHeaders);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000034F8 File Offset: 0x000024F8
		internal static Stream SerializeSoapMessage(IMessage msg, bool includeVersions)
		{
			MemoryStream memoryStream = new MemoryStream();
			CoreChannel.SerializeSoapMessage(msg, memoryStream, includeVersions);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000351C File Offset: 0x0000251C
		internal static void SerializeBinaryMessage(IMessage msg, Stream outputStream, bool includeVersions)
		{
			BinaryFormatter binaryFormatter = CoreChannel.CreateBinaryFormatter(true, includeVersions);
			binaryFormatter.Serialize(outputStream, msg, null);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0000353C File Offset: 0x0000253C
		internal static Stream SerializeBinaryMessage(IMessage msg, bool includeVersions)
		{
			MemoryStream memoryStream = new MemoryStream();
			CoreChannel.SerializeBinaryMessage(msg, memoryStream, includeVersions);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003560 File Offset: 0x00002560
		internal static IMessage DeserializeSoapRequestMessage(Stream inputStream, Header[] h, bool bStrictBinding, TypeFilterLevel securityLevel)
		{
			SoapFormatter soapFormatter = CoreChannel.CreateSoapFormatter(false, bStrictBinding);
			soapFormatter.FilterLevel = securityLevel;
			MethodCall methodCall = new MethodCall(h);
			soapFormatter.Deserialize(inputStream, new HeaderHandler(methodCall.HeaderHandler));
			return methodCall;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000359C File Offset: 0x0000259C
		internal static IMessage DeserializeSoapResponseMessage(Stream inputStream, IMessage requestMsg, Header[] h, bool bStrictBinding)
		{
			SoapFormatter soapFormatter = CoreChannel.CreateSoapFormatter(false, bStrictBinding);
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)requestMsg;
			MethodResponse methodResponse = new MethodResponse(h, methodCallMessage);
			soapFormatter.Deserialize(inputStream, new HeaderHandler(methodResponse.HeaderHandler));
			return methodResponse;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000035D8 File Offset: 0x000025D8
		internal static IMessage DeserializeBinaryRequestMessage(string objectUri, Stream inputStream, bool bStrictBinding, TypeFilterLevel securityLevel)
		{
			BinaryFormatter binaryFormatter = CoreChannel.CreateBinaryFormatter(false, bStrictBinding);
			binaryFormatter.FilterLevel = securityLevel;
			CoreChannel.UriHeaderHandler uriHeaderHandler = new CoreChannel.UriHeaderHandler(objectUri);
			return (IMessage)binaryFormatter.UnsafeDeserialize(inputStream, new HeaderHandler(uriHeaderHandler.HeaderHandler));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003618 File Offset: 0x00002618
		internal static IMessage DeserializeBinaryResponseMessage(Stream inputStream, IMethodCallMessage reqMsg, bool bStrictBinding)
		{
			BinaryFormatter binaryFormatter = CoreChannel.CreateBinaryFormatter(false, bStrictBinding);
			return (IMessage)binaryFormatter.UnsafeDeserializeMethodResponse(inputStream, null, reqMsg);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003640 File Offset: 0x00002640
		internal static Stream SerializeMessage(string mimeType, IMessage msg, bool includeVersions)
		{
			Stream stream = new MemoryStream();
			CoreChannel.SerializeMessage(mimeType, msg, stream, includeVersions);
			stream.Position = 0L;
			return stream;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003665 File Offset: 0x00002665
		internal static void SerializeMessage(string mimeType, IMessage msg, Stream outputStream, bool includeVersions)
		{
			if (string.Compare(mimeType, "text/xml", StringComparison.Ordinal) == 0)
			{
				CoreChannel.SerializeSoapMessage(msg, outputStream, includeVersions);
				return;
			}
			if (string.Compare(mimeType, "application/octet-stream", StringComparison.Ordinal) == 0)
			{
				CoreChannel.SerializeBinaryMessage(msg, outputStream, includeVersions);
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003694 File Offset: 0x00002694
		internal static IMessage DeserializeMessage(string mimeType, Stream xstm, bool methodRequest, IMessage msg)
		{
			return CoreChannel.DeserializeMessage(mimeType, xstm, methodRequest, msg, null);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000036A0 File Offset: 0x000026A0
		internal static IMessage DeserializeMessage(string mimeType, Stream xstm, bool methodRequest, IMessage msg, Header[] h)
		{
			bool flag = false;
			bool flag2 = true;
			if (string.Compare(mimeType, "application/octet-stream", StringComparison.Ordinal) == 0)
			{
				flag2 = true;
			}
			if (string.Compare(mimeType, "text/xml", StringComparison.Ordinal) == 0)
			{
				flag2 = false;
			}
			Stream stream;
			if (!flag)
			{
				stream = xstm;
			}
			else
			{
				long position = xstm.Position;
				MemoryStream memoryStream = (MemoryStream)xstm;
				byte[] array = memoryStream.ToArray();
				xstm.Position = position;
				string @string = Encoding.ASCII.GetString(array, 0, array.Length);
				byte[] array2 = Convert.FromBase64String(@string);
				MemoryStream memoryStream2 = new MemoryStream(array2);
				stream = memoryStream2;
			}
			IRemotingFormatter remotingFormatter = CoreChannel.MimeTypeToFormatter(mimeType, false);
			object obj;
			if (flag2)
			{
				obj = ((BinaryFormatter)remotingFormatter).UnsafeDeserializeMethodResponse(stream, null, (IMethodCallMessage)msg);
			}
			else if (methodRequest)
			{
				MethodCall methodCall = new MethodCall(h);
				remotingFormatter.Deserialize(stream, new HeaderHandler(methodCall.HeaderHandler));
				obj = methodCall;
			}
			else
			{
				IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
				MethodResponse methodResponse = new MethodResponse(h, methodCallMessage);
				remotingFormatter.Deserialize(stream, new HeaderHandler(methodResponse.HeaderHandler));
				obj = methodResponse;
			}
			return (IMessage)obj;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000037A7 File Offset: 0x000027A7
		internal static IRemotingFormatter MimeTypeToFormatter(string mimeType, bool serialize)
		{
			if (string.Compare(mimeType, "text/xml", StringComparison.Ordinal) == 0)
			{
				return CoreChannel.CreateSoapFormatter(serialize, true);
			}
			if (string.Compare(mimeType, "application/octet-stream", StringComparison.Ordinal) == 0)
			{
				return CoreChannel.CreateBinaryFormatter(serialize, true);
			}
			return null;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000037D8 File Offset: 0x000027D8
		internal static string RemoveApplicationNameFromUri(string uri)
		{
			if (uri == null)
			{
				return null;
			}
			string applicationName = RemotingConfiguration.ApplicationName;
			if (applicationName == null || applicationName.Length == 0)
			{
				return uri;
			}
			if (uri.Length < applicationName.Length + 2)
			{
				return uri;
			}
			if (string.Compare(applicationName, 0, uri, 0, applicationName.Length, StringComparison.OrdinalIgnoreCase) == 0 && uri[applicationName.Length] == '/')
			{
				uri = uri.Substring(applicationName.Length + 1);
			}
			return uri;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003842 File Offset: 0x00002842
		internal static void AppendProviderToClientProviderChain(IClientChannelSinkProvider providerChain, IClientChannelSinkProvider provider)
		{
			if (providerChain == null)
			{
				throw new ArgumentNullException("providerChain");
			}
			while (providerChain.Next != null)
			{
				providerChain = providerChain.Next;
			}
			providerChain.Next = provider;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003869 File Offset: 0x00002869
		internal static void CollectChannelDataFromServerSinkProviders(ChannelDataStore channelData, IServerChannelSinkProvider provider)
		{
			while (provider != null)
			{
				provider.GetChannelData(channelData);
				provider = provider.Next;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003880 File Offset: 0x00002880
		internal static void VerifyNoProviderData(string providerTypeName, ICollection providerData)
		{
			if (providerData != null && providerData.Count > 0)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Providers_Config_NotExpectingProviderData"), new object[] { providerTypeName }));
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000038C0 File Offset: 0x000028C0
		internal static void ReportUnknownProviderConfigProperty(string providerTypeName, string propertyName)
		{
			throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Providers_Config_UnknownProperty"), new object[] { providerTypeName, propertyName }));
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000038F8 File Offset: 0x000028F8
		internal static SinkChannelProtocol DetermineChannelProtocol(IChannel channel)
		{
			string text2;
			string text = channel.Parse("http://foo.com/foo", out text2);
			if (text != null)
			{
				return SinkChannelProtocol.Http;
			}
			return SinkChannelProtocol.Other;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000391C File Offset: 0x0000291C
		internal static bool SetupUrlBashingForIisSslIfNecessary()
		{
			return !CoreChannel.IsClientSKUInstallation && CoreChannel.SetupUrlBashingForIisSslIfNecessaryWorker();
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000393C File Offset: 0x0000293C
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool SetupUrlBashingForIisSslIfNecessaryWorker()
		{
			HttpContext httpContext = HttpContext.Current;
			bool flag = false;
			if (httpContext != null && httpContext.Request.IsSecureConnection)
			{
				Uri url = httpContext.Request.Url;
				StringBuilder stringBuilder = new StringBuilder(100);
				stringBuilder.Append("https://");
				stringBuilder.Append(url.Host);
				stringBuilder.Append(":");
				stringBuilder.Append(url.Port);
				stringBuilder.Append("/");
				stringBuilder.Append(RemotingConfiguration.ApplicationName);
				CallContext.SetData("__bashChannelUrl", new string[]
				{
					IisHelper.ApplicationUrl,
					stringBuilder.ToString()
				});
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000039EF File Offset: 0x000029EF
		internal static void CleanupUrlBashingForIisSslIfNecessary(bool bBashedUrl)
		{
			if (bBashedUrl)
			{
				CallContext.FreeNamedDataSlot("__bashChannelUrl");
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000039FE File Offset: 0x000029FE
		internal static string GetCurrentSidString()
		{
			return WindowsIdentity.GetCurrent().User.ToString();
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003A10 File Offset: 0x00002A10
		internal static string SidToString(IntPtr sidPointer)
		{
			if (!NativeMethods.IsValidSid(sidPointer))
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_InvalidSid"));
			}
			StringBuilder stringBuilder = new StringBuilder();
			IntPtr sidIdentifierAuthority = NativeMethods.GetSidIdentifierAuthority(sidPointer);
			int num = Marshal.GetLastWin32Error();
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			byte[] array = new byte[6];
			Marshal.Copy(sidIdentifierAuthority, array, 0, 6);
			IntPtr sidSubAuthorityCount = NativeMethods.GetSidSubAuthorityCount(sidPointer);
			num = Marshal.GetLastWin32Error();
			if (num != 0)
			{
				throw new Win32Exception(num);
			}
			uint num2 = (uint)Marshal.ReadByte(sidSubAuthorityCount);
			if (array[0] != 0 && array[1] != 0)
			{
				stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "{0:x2}{1:x2}{2:x2}{3:x2}{4:x2}{5:x2}", new object[]
				{
					array[0],
					array[1],
					array[2],
					array[3],
					array[4],
					array[5]
				}));
			}
			else
			{
				uint num3 = (uint)((int)array[5] + ((int)array[4] << 8) + ((int)array[3] << 16) + ((int)array[2] << 24));
				stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "{0:x12}", new object[] { num3 }));
			}
			int num4 = 0;
			while ((long)num4 < (long)((ulong)num2))
			{
				IntPtr sidSubAuthority = NativeMethods.GetSidSubAuthority(sidPointer, num4);
				num = Marshal.GetLastWin32Error();
				if (num != 0)
				{
					throw new Win32Exception(num);
				}
				uint num5 = (uint)Marshal.ReadInt32(sidSubAuthority);
				stringBuilder.Append(string.Format(CultureInfo.CurrentCulture, "-{0:x12}", new object[] { num5 }));
				num4++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003BA2 File Offset: 0x00002BA2
		private static ResourceManager InitResourceManager()
		{
			if (CoreChannel.SystemResMgr == null)
			{
				CoreChannel.SystemResMgr = new ResourceManager("System.Runtime.Remoting", typeof(CoreChannel).Module.Assembly);
			}
			return CoreChannel.SystemResMgr;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003BD4 File Offset: 0x00002BD4
		internal static string GetResourceString(string key)
		{
			if (CoreChannel.SystemResMgr == null)
			{
				CoreChannel.InitResourceManager();
			}
			return CoreChannel.SystemResMgr.GetString(key, null);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003BFC File Offset: 0x00002BFC
		[Conditional("_DEBUG")]
		internal static void DebugOut(string s)
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003BFE File Offset: 0x00002BFE
		[Conditional("_DEBUG")]
		internal static void DebugOutXMLStream(Stream stm, string tag)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003C00 File Offset: 0x00002C00
		[Conditional("_DEBUG")]
		internal static void DebugMessage(IMessage msg)
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003C02 File Offset: 0x00002C02
		[Conditional("_DEBUG")]
		internal static void DebugException(string name, Exception e)
		{
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003C04 File Offset: 0x00002C04
		[Conditional("_DEBUG")]
		internal static void DebugStream(Stream stm)
		{
		}

		// Token: 0x04000057 RID: 87
		internal const int MaxStringLen = 512;

		// Token: 0x04000058 RID: 88
		internal const string SOAPMimeType = "text/xml";

		// Token: 0x04000059 RID: 89
		internal const string BinaryMimeType = "application/octet-stream";

		// Token: 0x0400005A RID: 90
		internal const string SOAPContentType = "text/xml; charset=\"utf-8\"";

		// Token: 0x0400005B RID: 91
		internal const int CLIENT_MSG_GEN = 1;

		// Token: 0x0400005C RID: 92
		internal const int CLIENT_MSG_SINK_CHAIN = 2;

		// Token: 0x0400005D RID: 93
		internal const int CLIENT_MSG_SER = 3;

		// Token: 0x0400005E RID: 94
		internal const int CLIENT_MSG_SEND = 4;

		// Token: 0x0400005F RID: 95
		internal const int SERVER_MSG_RECEIVE = 5;

		// Token: 0x04000060 RID: 96
		internal const int SERVER_MSG_DESER = 6;

		// Token: 0x04000061 RID: 97
		internal const int SERVER_MSG_SINK_CHAIN = 7;

		// Token: 0x04000062 RID: 98
		internal const int SERVER_MSG_STACK_BUILD = 8;

		// Token: 0x04000063 RID: 99
		internal const int SERVER_DISPATCH = 9;

		// Token: 0x04000064 RID: 100
		internal const int SERVER_RET_STACK_BUILD = 10;

		// Token: 0x04000065 RID: 101
		internal const int SERVER_RET_SINK_CHAIN = 11;

		// Token: 0x04000066 RID: 102
		internal const int SERVER_RET_SER = 12;

		// Token: 0x04000067 RID: 103
		internal const int SERVER_RET_SEND = 13;

		// Token: 0x04000068 RID: 104
		internal const int SERVER_RET_END = 14;

		// Token: 0x04000069 RID: 105
		internal const int CLIENT_RET_RECEIVE = 15;

		// Token: 0x0400006A RID: 106
		internal const int CLIENT_RET_DESER = 16;

		// Token: 0x0400006B RID: 107
		internal const int CLIENT_RET_SINK_CHAIN = 17;

		// Token: 0x0400006C RID: 108
		internal const int CLIENT_RET_PROPAGATION = 18;

		// Token: 0x0400006D RID: 109
		internal const int CLIENT_END_CALL = 19;

		// Token: 0x0400006E RID: 110
		internal const int TIMING_DATA_EOF = 99;

		// Token: 0x0400006F RID: 111
		private static IByteBufferPool _bufferPool = new ByteBufferPool(10, 4096);

		// Token: 0x04000070 RID: 112
		private static RequestQueue _requestQueue = new RequestQueue(8, 4, 250);

		// Token: 0x04000071 RID: 113
		private static string s_hostName = null;

		// Token: 0x04000072 RID: 114
		private static string s_MachineName = null;

		// Token: 0x04000073 RID: 115
		private static string s_MachineIp = null;

		// Token: 0x04000074 RID: 116
		private static IPAddress s_MachineIpAddress = null;

		// Token: 0x04000075 RID: 117
		private static bool s_isClientSKUInstallationInitialized = false;

		// Token: 0x04000076 RID: 118
		private static bool s_isClientSKUInstallation = false;

		// Token: 0x04000077 RID: 119
		internal static ResourceManager SystemResMgr;

		// Token: 0x02000010 RID: 16
		private class UriHeaderHandler
		{
			// Token: 0x06000068 RID: 104 RVA: 0x00003C5B File Offset: 0x00002C5B
			internal UriHeaderHandler(string uri)
			{
				this._uri = uri;
			}

			// Token: 0x06000069 RID: 105 RVA: 0x00003C6A File Offset: 0x00002C6A
			public object HeaderHandler(Header[] Headers)
			{
				return this._uri;
			}

			// Token: 0x04000078 RID: 120
			private string _uri;
		}
	}
}
