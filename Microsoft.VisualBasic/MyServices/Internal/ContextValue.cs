using System;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Microsoft.VisualBasic.MyServices.Internal
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class ContextValue<T>
	{
		public ContextValue()
		{
			this.m_ContextKey = Guid.NewGuid().ToString();
		}

		public T Value
		{
			get
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					return (T)((object)httpContext.Items[this.m_ContextKey]);
				}
				return (T)((object)CallContext.GetData(this.m_ContextKey));
			}
			set
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					httpContext.Items[this.m_ContextKey] = value;
				}
				else
				{
					CallContext.SetData(this.m_ContextKey, value);
				}
			}
		}

		private string m_ContextKey;
	}
}
