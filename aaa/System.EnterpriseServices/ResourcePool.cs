using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000BE RID: 190
	public sealed class ResourcePool : IObjPool
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x0000DCD0 File Offset: 0x0000CCD0
		public ResourcePool(ResourcePool.TransactionEndDelegate cb)
		{
			Platform.Assert(Platform.W2K, "ResourcePool");
			this._cb = cb;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000DCEE File Offset: 0x0000CCEE
		private IntPtr GetToken()
		{
			return Marshal.GetComInterfaceForObject(this, typeof(IObjPool));
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000DD00 File Offset: 0x0000CD00
		private void ReleaseToken()
		{
			IntPtr comInterfaceForObject = Marshal.GetComInterfaceForObject(this, typeof(IObjPool));
			Marshal.Release(comInterfaceForObject);
			Marshal.Release(comInterfaceForObject);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000DD2C File Offset: 0x0000CD2C
		public bool PutResource(object resource)
		{
			ITransactionResourcePool transactionResourcePool = null;
			IntPtr intPtr = (IntPtr)0;
			bool flag = false;
			try
			{
				transactionResourcePool = ResourcePool.GetResourcePool();
				if (transactionResourcePool != null)
				{
					intPtr = this.GetToken();
					int num = transactionResourcePool.PutResource(intPtr, resource);
					flag = num >= 0;
				}
			}
			finally
			{
				if (!flag && intPtr != (IntPtr)0)
				{
					Marshal.Release(intPtr);
				}
				if (transactionResourcePool != null)
				{
					Marshal.ReleaseComObject(transactionResourcePool);
				}
			}
			return flag;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000DDA0 File Offset: 0x0000CDA0
		public object GetResource()
		{
			object obj = null;
			ITransactionResourcePool transactionResourcePool = null;
			IntPtr intPtr = (IntPtr)0;
			try
			{
				intPtr = this.GetToken();
				transactionResourcePool = ResourcePool.GetResourcePool();
				if (transactionResourcePool != null)
				{
					int resource = transactionResourcePool.GetResource(intPtr, out obj);
					if (resource >= 0)
					{
						Marshal.Release(intPtr);
					}
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Marshal.Release(intPtr);
				}
				if (transactionResourcePool != null)
				{
					Marshal.ReleaseComObject(transactionResourcePool);
				}
			}
			return obj;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000DE10 File Offset: 0x0000CE10
		private static ITransactionResourcePool GetResourcePool()
		{
			ITransactionResourcePool transactionResourcePool = null;
			object obj = null;
			int num = 0;
			((IContext)ContextUtil.ObjectContext).GetProperty(ResourcePool.GUID_TransactionProperty, out num, out obj);
			int transactionResourcePool2 = ((ITransactionProperty)obj).GetTransactionResourcePool(out transactionResourcePool);
			if (transactionResourcePool2 >= 0)
			{
				return transactionResourcePool;
			}
			return null;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000DE52 File Offset: 0x0000CE52
		void IObjPool.Init(object p)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000DE59 File Offset: 0x0000CE59
		object IObjPool.Get()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000DE60 File Offset: 0x0000CE60
		void IObjPool.SetOption(int o, int dw)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000DE67 File Offset: 0x0000CE67
		void IObjPool.PutNew(object o)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000DE6E File Offset: 0x0000CE6E
		void IObjPool.PutDeactivated(object p)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000DE75 File Offset: 0x0000CE75
		void IObjPool.Shutdown()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000DE7C File Offset: 0x0000CE7C
		void IObjPool.PutEndTx(object p)
		{
			this._cb(p);
			this.ReleaseToken();
		}

		// Token: 0x040001F5 RID: 501
		private static readonly Guid GUID_TransactionProperty = new Guid("ecabaeb1-7f19-11d2-978e-0000f8757e2a");

		// Token: 0x040001F6 RID: 502
		private ResourcePool.TransactionEndDelegate _cb;

		// Token: 0x020000BF RID: 191
		// (Invoke) Token: 0x0600047F RID: 1151
		public delegate void TransactionEndDelegate(object resource);
	}
}
