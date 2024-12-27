using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Remoting.Services
{
	// Token: 0x02000792 RID: 1938
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class TrackingServices
	{
		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x0600455F RID: 17759 RVA: 0x000ECC2C File Offset: 0x000EBC2C
		private static object TrackingServicesSyncObject
		{
			get
			{
				if (TrackingServices.s_TrackingServicesSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref TrackingServices.s_TrackingServicesSyncObject, obj, null);
				}
				return TrackingServices.s_TrackingServicesSyncObject;
			}
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x000ECC58 File Offset: 0x000EBC58
		public static void RegisterTrackingHandler(ITrackingHandler handler)
		{
			lock (TrackingServices.TrackingServicesSyncObject)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				if (-1 != TrackingServices.Match(handler))
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_TrackingHandlerAlreadyRegistered"), new object[] { "handler" }));
				}
				if (TrackingServices._Handlers == null || TrackingServices._Size == TrackingServices._Handlers.Length)
				{
					ITrackingHandler[] array = new ITrackingHandler[TrackingServices._Size * 2 + 4];
					if (TrackingServices._Handlers != null)
					{
						Array.Copy(TrackingServices._Handlers, array, TrackingServices._Size);
					}
					TrackingServices._Handlers = array;
				}
				TrackingServices._Handlers[TrackingServices._Size++] = handler;
			}
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x000ECD24 File Offset: 0x000EBD24
		public static void UnregisterTrackingHandler(ITrackingHandler handler)
		{
			lock (TrackingServices.TrackingServicesSyncObject)
			{
				if (handler == null)
				{
					throw new ArgumentNullException("handler");
				}
				int num = TrackingServices.Match(handler);
				if (-1 == num)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_HandlerNotRegistered"), new object[] { handler }));
				}
				Array.Copy(TrackingServices._Handlers, num + 1, TrackingServices._Handlers, num, TrackingServices._Size - num - 1);
				TrackingServices._Size--;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004562 RID: 17762 RVA: 0x000ECDC0 File Offset: 0x000EBDC0
		public static ITrackingHandler[] RegisteredHandlers
		{
			get
			{
				ITrackingHandler[] array;
				lock (TrackingServices.TrackingServicesSyncObject)
				{
					if (TrackingServices._Size == 0)
					{
						array = new ITrackingHandler[0];
					}
					else
					{
						ITrackingHandler[] array2 = new ITrackingHandler[TrackingServices._Size];
						for (int i = 0; i < TrackingServices._Size; i++)
						{
							array2[i] = TrackingServices._Handlers[i];
						}
						array = array2;
					}
				}
				return array;
			}
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x000ECE2C File Offset: 0x000EBE2C
		internal static void MarshaledObject(object obj, ObjRef or)
		{
			try
			{
				ITrackingHandler[] handlers = TrackingServices._Handlers;
				for (int i = 0; i < TrackingServices._Size; i++)
				{
					handlers[i].MarshaledObject(obj, or);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x000ECE70 File Offset: 0x000EBE70
		internal static void UnmarshaledObject(object obj, ObjRef or)
		{
			try
			{
				ITrackingHandler[] handlers = TrackingServices._Handlers;
				for (int i = 0; i < TrackingServices._Size; i++)
				{
					handlers[i].UnmarshaledObject(obj, or);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x000ECEB4 File Offset: 0x000EBEB4
		internal static void DisconnectedObject(object obj)
		{
			try
			{
				ITrackingHandler[] handlers = TrackingServices._Handlers;
				for (int i = 0; i < TrackingServices._Size; i++)
				{
					handlers[i].DisconnectedObject(obj);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x000ECEF8 File Offset: 0x000EBEF8
		private static int Match(ITrackingHandler handler)
		{
			int num = -1;
			for (int i = 0; i < TrackingServices._Size; i++)
			{
				if (TrackingServices._Handlers[i] == handler)
				{
					num = i;
					break;
				}
			}
			return num;
		}

		// Token: 0x04002251 RID: 8785
		private static ITrackingHandler[] _Handlers = new ITrackingHandler[0];

		// Token: 0x04002252 RID: 8786
		private static int _Size = 0;

		// Token: 0x04002253 RID: 8787
		private static object s_TrackingServicesSyncObject = null;
	}
}
