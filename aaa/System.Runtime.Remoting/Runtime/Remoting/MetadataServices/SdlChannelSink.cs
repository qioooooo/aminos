using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;
using System.Web;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x0200006F RID: 111
	public class SdlChannelSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x0600037B RID: 891 RVA: 0x0001091E File Offset: 0x0000F91E
		public SdlChannelSink(IChannelReceiver receiver, IServerChannelSink nextSink)
		{
			this._receiver = receiver;
			this._nextSink = nextSink;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00010934 File Offset: 0x0000F934
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0001093C File Offset: 0x0000F93C
		internal bool RemoteApplicationMetadataEnabled
		{
			get
			{
				return this._bRemoteApplicationMetadataEnabled;
			}
			set
			{
				this._bRemoteApplicationMetadataEnabled = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00010945 File Offset: 0x0000F945
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0001094D File Offset: 0x0000F94D
		internal bool MetadataEnabled
		{
			get
			{
				return this._bMetadataEnabled;
			}
			set
			{
				this._bMetadataEnabled = value;
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00010958 File Offset: 0x0000F958
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			if (requestMsg != null)
			{
				return this._nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
			}
			SdlType sdlType;
			if (!this.ShouldIntercept(requestHeaders, out sdlType))
			{
				return this._nextSink.ProcessMessage(sinkStack, null, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
			}
			responseHeaders = new TransportHeaders();
			this.GenerateSdl(sdlType, sinkStack, requestHeaders, responseHeaders, out responseStream);
			responseMsg = null;
			return ServerProcessing.Complete;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000109BC File Offset: 0x0000F9BC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000109BE File Offset: 0x0000F9BE
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000383 RID: 899 RVA: 0x000109C5 File Offset: 0x0000F9C5
		public IServerChannelSink NextChannelSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000384 RID: 900 RVA: 0x000109CD File Offset: 0x0000F9CD
		public IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return null;
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000109D0 File Offset: 0x0000F9D0
		private bool ShouldIntercept(ITransportHeaders requestHeaders, out SdlType sdlType)
		{
			sdlType = SdlType.Sdl;
			string text = requestHeaders["__RequestVerb"] as string;
			string text2 = requestHeaders["__RequestUri"] as string;
			if (text2 == null || text == null || !text.Equals("GET"))
			{
				return false;
			}
			int num = text2.LastIndexOf('?');
			if (num == -1)
			{
				return false;
			}
			string text3 = text2.Substring(num).ToLower(CultureInfo.InvariantCulture);
			if (string.CompareOrdinal(text3, "?sdl") == 0 || string.CompareOrdinal(text3, "?sdlx") == 0)
			{
				sdlType = SdlType.Sdl;
				return true;
			}
			if (string.CompareOrdinal(text3, "?wsdl") == 0)
			{
				sdlType = SdlType.Wsdl;
				return true;
			}
			return false;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00010A6C File Offset: 0x0000FA6C
		private void GenerateSdl(SdlType sdlType, IServerResponseChannelSinkStack sinkStack, ITransportHeaders requestHeaders, ITransportHeaders responseHeaders, out Stream outputStream)
		{
			if (!this.MetadataEnabled)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_MetadataNotEnabled"));
			}
			string text = requestHeaders["__RequestUri"] as string;
			string objectUriFromRequestUri = HttpChannelHelper.GetObjectUriFromRequestUri(text);
			if (!this.RemoteApplicationMetadataEnabled && string.Compare(objectUriFromRequestUri, "RemoteApplicationMetadata.rem", StringComparison.OrdinalIgnoreCase) == 0)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_RemoteApplicationMetadataNotEnabled"));
			}
			string text2 = (string)requestHeaders["Host"];
			if (text2 != null)
			{
				int num = text2.IndexOf(':');
				if (num != -1)
				{
					text2 = text2.Substring(0, num);
				}
			}
			string text3 = SdlChannelSink.SetupUrlBashingForIisIfNecessary(text2);
			ServiceType[] array;
			if (string.Compare(objectUriFromRequestUri, "RemoteApplicationMetadata.rem", StringComparison.OrdinalIgnoreCase) == 0)
			{
				ActivatedServiceTypeEntry[] registeredActivatedServiceTypes = RemotingConfiguration.GetRegisteredActivatedServiceTypes();
				WellKnownServiceTypeEntry[] registeredWellKnownServiceTypes = RemotingConfiguration.GetRegisteredWellKnownServiceTypes();
				int num2 = 0;
				if (registeredActivatedServiceTypes != null)
				{
					num2 += registeredActivatedServiceTypes.Length;
				}
				if (registeredWellKnownServiceTypes != null)
				{
					num2 += registeredWellKnownServiceTypes.Length;
				}
				array = new ServiceType[num2];
				int num3 = 0;
				if (registeredActivatedServiceTypes != null)
				{
					foreach (ActivatedServiceTypeEntry activatedServiceTypeEntry in registeredActivatedServiceTypes)
					{
						array[num3++] = new ServiceType(activatedServiceTypeEntry.ObjectType, null);
					}
				}
				if (registeredWellKnownServiceTypes != null)
				{
					foreach (WellKnownServiceTypeEntry wellKnownServiceTypeEntry in registeredWellKnownServiceTypes)
					{
						string[] urlsForUri = this._receiver.GetUrlsForUri(wellKnownServiceTypeEntry.ObjectUri);
						string text4 = urlsForUri[0];
						if (text3 != null)
						{
							text4 = HttpChannelHelper.ReplaceChannelUriWithThisString(text4, text3);
						}
						else if (text2 != null)
						{
							text4 = HttpChannelHelper.ReplaceMachineNameWithThisString(text4, text2);
						}
						array[num3++] = new ServiceType(wellKnownServiceTypeEntry.ObjectType, text4);
					}
				}
			}
			else
			{
				Type serverTypeForUri = RemotingServices.GetServerTypeForUri(objectUriFromRequestUri);
				if (serverTypeForUri == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, "Object with uri '{0}' does not exist at server.", new object[] { objectUriFromRequestUri }));
				}
				string[] urlsForUri2 = this._receiver.GetUrlsForUri(objectUriFromRequestUri);
				string text5 = urlsForUri2[0];
				if (text3 != null)
				{
					text5 = HttpChannelHelper.ReplaceChannelUriWithThisString(text5, text3);
				}
				else if (text2 != null)
				{
					text5 = HttpChannelHelper.ReplaceMachineNameWithThisString(text5, text2);
				}
				array = new ServiceType[]
				{
					new ServiceType(serverTypeForUri, text5)
				};
			}
			responseHeaders["Content-Type"] = "text/xml";
			bool flag = false;
			outputStream = sinkStack.GetResponseStream(null, responseHeaders);
			if (outputStream == null)
			{
				outputStream = new MemoryStream(1024);
				flag = true;
			}
			MetaData.ConvertTypesToSchemaToStream(array, sdlType, outputStream);
			if (flag)
			{
				outputStream.Position = 0L;
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00010CC4 File Offset: 0x0000FCC4
		internal static string SetupUrlBashingForIisIfNecessary(string hostName)
		{
			string text = null;
			if (!CoreChannel.IsClientSKUInstallation)
			{
				text = SdlChannelSink.SetupUrlBashingForIisIfNecessaryWorker(hostName);
			}
			return text;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00010CE4 File Offset: 0x0000FCE4
		[MethodImpl(MethodImplOptions.NoInlining)]
		private static string SetupUrlBashingForIisIfNecessaryWorker(string hostName)
		{
			string text = null;
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				HttpRequest request = httpContext.Request;
				string text2;
				if (request.IsSecureConnection)
				{
					text2 = "https";
				}
				else
				{
					text2 = "http";
				}
				int port = httpContext.Request.Url.Port;
				StringBuilder stringBuilder = new StringBuilder(100);
				stringBuilder.Append(text2);
				stringBuilder.Append("://");
				if (hostName != null)
				{
					stringBuilder.Append(hostName);
				}
				else
				{
					stringBuilder.Append(CoreChannel.GetMachineName());
				}
				stringBuilder.Append(":");
				stringBuilder.Append(port.ToString(CultureInfo.InvariantCulture));
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x04000281 RID: 641
		private IChannelReceiver _receiver;

		// Token: 0x04000282 RID: 642
		private IServerChannelSink _nextSink;

		// Token: 0x04000283 RID: 643
		private bool _bRemoteApplicationMetadataEnabled;

		// Token: 0x04000284 RID: 644
		private bool _bMetadataEnabled;
	}
}
