using System;
using System.Collections;
using System.Threading;

namespace System.Web.Services.Description
{
	// Token: 0x020000F3 RID: 243
	public abstract class ServiceDescriptionBaseCollection : CollectionBase
	{
		// Token: 0x06000671 RID: 1649 RVA: 0x0001D9EF File Offset: 0x0001C9EF
		internal ServiceDescriptionBaseCollection(object parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x0001D9FE File Offset: 0x0001C9FE
		protected virtual IDictionary Table
		{
			get
			{
				if (this.table == null)
				{
					this.table = new Hashtable();
				}
				return this.table;
			}
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001DA19 File Offset: 0x0001CA19
		protected virtual string GetKey(object value)
		{
			return null;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001DA1C File Offset: 0x0001CA1C
		protected virtual void SetParent(object value, object parent)
		{
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001DA1E File Offset: 0x0001CA1E
		protected override void OnInsertComplete(int index, object value)
		{
			this.AddValue(value);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001DA27 File Offset: 0x0001CA27
		protected override void OnRemove(int index, object value)
		{
			this.RemoveValue(value);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001DA30 File Offset: 0x0001CA30
		protected override void OnClear()
		{
			for (int i = 0; i < base.List.Count; i++)
			{
				this.RemoveValue(base.List[i]);
			}
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001DA65 File Offset: 0x0001CA65
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			this.RemoveValue(oldValue);
			this.AddValue(newValue);
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001DA78 File Offset: 0x0001CA78
		private void AddValue(object value)
		{
			string key = this.GetKey(value);
			if (key != null)
			{
				try
				{
					this.Table.Add(key, value);
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					if (this.Table[key] != null)
					{
						throw new ArgumentException(ServiceDescriptionBaseCollection.GetDuplicateMessage(value.GetType(), key), ex.InnerException);
					}
					throw ex;
				}
				catch
				{
					if (this.Table[key] != null)
					{
						throw new ArgumentException(ServiceDescriptionBaseCollection.GetDuplicateMessage(value.GetType(), key), null);
					}
					throw;
				}
			}
			this.SetParent(value, this.parent);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001DB30 File Offset: 0x0001CB30
		private void RemoveValue(object value)
		{
			string key = this.GetKey(value);
			if (key != null)
			{
				this.Table.Remove(key);
			}
			this.SetParent(value, null);
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001DB5C File Offset: 0x0001CB5C
		private static string GetDuplicateMessage(Type type, string elemName)
		{
			string text;
			if (type == typeof(ServiceDescriptionFormatExtension))
			{
				text = Res.GetString("WebDuplicateFormatExtension", new object[] { elemName });
			}
			else if (type == typeof(OperationMessage))
			{
				text = Res.GetString("WebDuplicateOperationMessage", new object[] { elemName });
			}
			else if (type == typeof(Import))
			{
				text = Res.GetString("WebDuplicateImport", new object[] { elemName });
			}
			else if (type == typeof(Message))
			{
				text = Res.GetString("WebDuplicateMessage", new object[] { elemName });
			}
			else if (type == typeof(Port))
			{
				text = Res.GetString("WebDuplicatePort", new object[] { elemName });
			}
			else if (type == typeof(PortType))
			{
				text = Res.GetString("WebDuplicatePortType", new object[] { elemName });
			}
			else if (type == typeof(Binding))
			{
				text = Res.GetString("WebDuplicateBinding", new object[] { elemName });
			}
			else if (type == typeof(Service))
			{
				text = Res.GetString("WebDuplicateService", new object[] { elemName });
			}
			else if (type == typeof(MessagePart))
			{
				text = Res.GetString("WebDuplicateMessagePart", new object[] { elemName });
			}
			else if (type == typeof(OperationBinding))
			{
				text = Res.GetString("WebDuplicateOperationBinding", new object[] { elemName });
			}
			else if (type == typeof(FaultBinding))
			{
				text = Res.GetString("WebDuplicateFaultBinding", new object[] { elemName });
			}
			else if (type == typeof(Operation))
			{
				text = Res.GetString("WebDuplicateOperation", new object[] { elemName });
			}
			else if (type == typeof(OperationFault))
			{
				text = Res.GetString("WebDuplicateOperationFault", new object[] { elemName });
			}
			else
			{
				text = Res.GetString("WebDuplicateUnknownElement", new object[] { type, elemName });
			}
			return text;
		}

		// Token: 0x0400047E RID: 1150
		private Hashtable table;

		// Token: 0x0400047F RID: 1151
		private object parent;
	}
}
