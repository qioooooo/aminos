using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System
{
	// Token: 0x02000088 RID: 136
	internal class __ComObject : MarshalByRefObject
	{
		// Token: 0x06000770 RID: 1904 RVA: 0x000182C8 File Offset: 0x000172C8
		private __ComObject()
		{
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x000182D0 File Offset: 0x000172D0
		internal IntPtr GetIUnknown(out bool fIsURTAggregated)
		{
			fIsURTAggregated = !base.GetType().IsDefined(typeof(ComImportAttribute), false);
			return Marshal.GetIUnknownForObject(this);
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x000182F4 File Offset: 0x000172F4
		internal object GetData(object key)
		{
			object obj = null;
			lock (this)
			{
				if (this.m_ObjectToDataMap != null)
				{
					obj = this.m_ObjectToDataMap[key];
				}
			}
			return obj;
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0001833C File Offset: 0x0001733C
		internal bool SetData(object key, object data)
		{
			bool flag = false;
			lock (this)
			{
				if (this.m_ObjectToDataMap == null)
				{
					this.m_ObjectToDataMap = new Hashtable();
				}
				if (this.m_ObjectToDataMap[key] == null)
				{
					this.m_ObjectToDataMap[key] = data;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x000183A0 File Offset: 0x000173A0
		internal void ReleaseAllData()
		{
			lock (this)
			{
				if (this.m_ObjectToDataMap != null)
				{
					foreach (object obj in this.m_ObjectToDataMap.Values)
					{
						IDisposable disposable = obj as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
						__ComObject _ComObject = obj as __ComObject;
						if (_ComObject != null)
						{
							Marshal.ReleaseComObject(_ComObject);
						}
					}
					this.m_ObjectToDataMap = null;
				}
			}
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x00018448 File Offset: 0x00017448
		internal object GetEventProvider(Type t)
		{
			object obj = this.GetData(t);
			if (obj == null)
			{
				obj = this.CreateEventProvider(t);
			}
			return obj;
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x00018469 File Offset: 0x00017469
		internal int ReleaseSelf()
		{
			return Marshal.InternalReleaseComObject(this);
		}

		// Token: 0x06000777 RID: 1911 RVA: 0x00018471 File Offset: 0x00017471
		internal void FinalReleaseSelf()
		{
			Marshal.InternalFinalReleaseComObject(this);
		}

		// Token: 0x06000778 RID: 1912 RVA: 0x0001847C File Offset: 0x0001747C
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private object CreateEventProvider(Type t)
		{
			object obj = Activator.CreateInstance(t, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, new object[] { this }, null);
			if (!this.SetData(t, obj))
			{
				IDisposable disposable = obj as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				obj = this.GetData(t);
			}
			return obj;
		}

		// Token: 0x04000288 RID: 648
		private Hashtable m_ObjectToDataMap;
	}
}
