using System;
using System.IO;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x02000377 RID: 887
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public static class SessionStateUtility
	{
		// Token: 0x06002B0C RID: 11020 RVA: 0x000BED36 File Offset: 0x000BDD36
		public static void RaiseSessionEnd(IHttpSessionState session, object eventSource, EventArgs eventArgs)
		{
			HttpApplicationFactory.EndSession(new HttpSessionState(session), eventSource, eventArgs);
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000BED48 File Offset: 0x000BDD48
		public static void AddHttpSessionStateToContext(HttpContext context, IHttpSessionState container)
		{
			HttpSessionState httpSessionState = new HttpSessionState(container);
			try
			{
				context.Items.Add("AspSession", httpSessionState);
			}
			catch (ArgumentException)
			{
				throw new HttpException(SR.GetString("Cant_have_multiple_session_module"));
			}
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000BED90 File Offset: 0x000BDD90
		internal static void AddDelayedHttpSessionStateToContext(HttpContext context, SessionStateModule module)
		{
			context.AddDelayedHttpSessionState(module);
		}

		// Token: 0x06002B0F RID: 11023 RVA: 0x000BED99 File Offset: 0x000BDD99
		internal static void RemoveHttpSessionStateFromContext(HttpContext context, bool delayed)
		{
			if (delayed)
			{
				context.RemoveDelayedHttpSessionState();
				return;
			}
			context.Items.Remove("AspSession");
		}

		// Token: 0x06002B10 RID: 11024 RVA: 0x000BEDB5 File Offset: 0x000BDDB5
		public static void RemoveHttpSessionStateFromContext(HttpContext context)
		{
			SessionStateUtility.RemoveHttpSessionStateFromContext(context, false);
		}

		// Token: 0x06002B11 RID: 11025 RVA: 0x000BEDBE File Offset: 0x000BDDBE
		public static IHttpSessionState GetHttpSessionStateFromContext(HttpContext context)
		{
			return context.Session.Container;
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000BEDCB File Offset: 0x000BDDCB
		public static HttpStaticObjectsCollection GetSessionStaticObjects(HttpContext context)
		{
			return context.Application.SessionStaticObjects.Clone();
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000BEDDD File Offset: 0x000BDDDD
		internal static SessionStateStoreData CreateLegitStoreData(HttpContext context, ISessionStateItemCollection sessionItems, HttpStaticObjectsCollection staticObjects, int timeout)
		{
			if (sessionItems == null)
			{
				sessionItems = new SessionStateItemCollection();
			}
			if (staticObjects == null && context != null)
			{
				staticObjects = SessionStateUtility.GetSessionStaticObjects(context);
			}
			return new SessionStateStoreData(sessionItems, staticObjects, timeout);
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x000BEE00 File Offset: 0x000BDE00
		internal static void Serialize(SessionStateStoreData item, Stream stream)
		{
			bool flag = true;
			bool flag2 = true;
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Write(item.Timeout);
			if (item.Items == null || item.Items.Count == 0)
			{
				flag = false;
			}
			binaryWriter.Write(flag);
			if (item.StaticObjects == null || item.StaticObjects.NeverAccessed)
			{
				flag2 = false;
			}
			binaryWriter.Write(flag2);
			if (flag)
			{
				((SessionStateItemCollection)item.Items).Serialize(binaryWriter);
			}
			if (flag2)
			{
				item.StaticObjects.Serialize(binaryWriter);
			}
			binaryWriter.Write(byte.MaxValue);
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x000BEE90 File Offset: 0x000BDE90
		internal static SessionStateStoreData Deserialize(HttpContext context, Stream stream)
		{
			int num;
			SessionStateItemCollection sessionStateItemCollection;
			HttpStaticObjectsCollection httpStaticObjectsCollection;
			try
			{
				BinaryReader binaryReader = new BinaryReader(stream);
				num = binaryReader.ReadInt32();
				bool flag = binaryReader.ReadBoolean();
				bool flag2 = binaryReader.ReadBoolean();
				if (flag)
				{
					sessionStateItemCollection = SessionStateItemCollection.Deserialize(binaryReader);
				}
				else
				{
					sessionStateItemCollection = new SessionStateItemCollection();
				}
				if (flag2)
				{
					httpStaticObjectsCollection = HttpStaticObjectsCollection.Deserialize(binaryReader);
				}
				else
				{
					httpStaticObjectsCollection = SessionStateUtility.GetSessionStaticObjects(context);
				}
				byte b = binaryReader.ReadByte();
				if (b != 255)
				{
					throw new HttpException(SR.GetString("Invalid_session_state"));
				}
			}
			catch (EndOfStreamException)
			{
				throw new HttpException(SR.GetString("Invalid_session_state"));
			}
			return new SessionStateStoreData(sessionStateItemCollection, httpStaticObjectsCollection, num);
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000BEF34 File Offset: 0x000BDF34
		internal static void SerializeStoreData(SessionStateStoreData item, int initialStreamSize, out byte[] buf, out int length)
		{
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream(initialStreamSize);
				SessionStateUtility.Serialize(item, memoryStream);
				buf = memoryStream.GetBuffer();
				length = (int)memoryStream.Length;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
		}

		// Token: 0x04001FBF RID: 8127
		internal const string SESSION_KEY = "AspSession";
	}
}
