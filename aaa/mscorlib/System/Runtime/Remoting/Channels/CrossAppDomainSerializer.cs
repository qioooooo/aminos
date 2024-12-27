using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006BD RID: 1725
	internal static class CrossAppDomainSerializer
	{
		// Token: 0x06003EA7 RID: 16039 RVA: 0x000D7A34 File Offset: 0x000D6A34
		internal static MemoryStream SerializeMessage(IMessage msg)
		{
			MemoryStream memoryStream = new MemoryStream();
			RemotingSurrogateSelector remotingSurrogateSelector = new RemotingSurrogateSelector();
			new BinaryFormatter
			{
				SurrogateSelector = remotingSurrogateSelector,
				Context = new StreamingContext(StreamingContextStates.CrossAppDomain)
			}.Serialize(memoryStream, msg, null, false);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x000D7A80 File Offset: 0x000D6A80
		internal static MemoryStream SerializeMessageParts(ArrayList argsToSerialize)
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			RemotingSurrogateSelector remotingSurrogateSelector = new RemotingSurrogateSelector();
			binaryFormatter.SurrogateSelector = remotingSurrogateSelector;
			binaryFormatter.Context = new StreamingContext(StreamingContextStates.CrossAppDomain);
			binaryFormatter.Serialize(memoryStream, argsToSerialize, null, false);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x06003EA9 RID: 16041 RVA: 0x000D7ACC File Offset: 0x000D6ACC
		internal static void SerializeObject(object obj, MemoryStream stm)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			RemotingSurrogateSelector remotingSurrogateSelector = new RemotingSurrogateSelector();
			binaryFormatter.SurrogateSelector = remotingSurrogateSelector;
			binaryFormatter.Context = new StreamingContext(StreamingContextStates.CrossAppDomain);
			binaryFormatter.Serialize(stm, obj, null, false);
		}

		// Token: 0x06003EAA RID: 16042 RVA: 0x000D7B08 File Offset: 0x000D6B08
		internal static MemoryStream SerializeObject(object obj)
		{
			MemoryStream memoryStream = new MemoryStream();
			CrossAppDomainSerializer.SerializeObject(obj, memoryStream);
			memoryStream.Position = 0L;
			return memoryStream;
		}

		// Token: 0x06003EAB RID: 16043 RVA: 0x000D7B2B File Offset: 0x000D6B2B
		internal static IMessage DeserializeMessage(MemoryStream stm)
		{
			return CrossAppDomainSerializer.DeserializeMessage(stm, null);
		}

		// Token: 0x06003EAC RID: 16044 RVA: 0x000D7B34 File Offset: 0x000D6B34
		internal static IMessage DeserializeMessage(MemoryStream stm, IMethodCallMessage reqMsg)
		{
			if (stm == null)
			{
				throw new ArgumentNullException("stm");
			}
			stm.Position = 0L;
			return (IMessage)new BinaryFormatter
			{
				SurrogateSelector = null,
				Context = new StreamingContext(StreamingContextStates.CrossAppDomain)
			}.Deserialize(stm, null, false, true, reqMsg);
		}

		// Token: 0x06003EAD RID: 16045 RVA: 0x000D7B84 File Offset: 0x000D6B84
		internal static ArrayList DeserializeMessageParts(MemoryStream stm)
		{
			return (ArrayList)CrossAppDomainSerializer.DeserializeObject(stm);
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x000D7B94 File Offset: 0x000D6B94
		internal static object DeserializeObject(MemoryStream stm)
		{
			stm.Position = 0L;
			return new BinaryFormatter
			{
				Context = new StreamingContext(StreamingContextStates.CrossAppDomain)
			}.Deserialize(stm, null, false, true, null);
		}
	}
}
