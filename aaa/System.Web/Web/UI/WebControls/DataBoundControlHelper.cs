using System;
using System.Collections.Specialized;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000529 RID: 1321
	internal static class DataBoundControlHelper
	{
		// Token: 0x06004128 RID: 16680 RVA: 0x0010EAC0 File Offset: 0x0010DAC0
		public static Control FindControl(Control control, string controlID)
		{
			Control control2 = control;
			Control control3 = null;
			if (control == control.Page)
			{
				return control.FindControl(controlID);
			}
			while (control3 == null && control2 != control.Page)
			{
				control2 = control2.NamingContainer;
				if (control2 == null)
				{
					throw new HttpException(SR.GetString("DataBoundControlHelper_NoNamingContainer", new object[]
					{
						control.GetType().Name,
						control.ID
					}));
				}
				control3 = control2.FindControl(controlID);
			}
			return control3;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x0010EB30 File Offset: 0x0010DB30
		public static bool CompareStringArrays(string[] stringA, string[] stringB)
		{
			if (stringA == null && stringB == null)
			{
				return true;
			}
			if (stringA == null || stringB == null)
			{
				return false;
			}
			if (stringA.Length != stringB.Length)
			{
				return false;
			}
			for (int i = 0; i < stringA.Length; i++)
			{
				if (!string.Equals(stringA[i], stringB[i], StringComparison.Ordinal))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0010EB78 File Offset: 0x0010DB78
		public static bool IsBindableType(Type type)
		{
			if (type == null)
			{
				return false;
			}
			Type underlyingType = Nullable.GetUnderlyingType(type);
			if (underlyingType != null)
			{
				type = underlyingType;
			}
			return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal) || type == typeof(Guid) || type == typeof(DateTimeOffset) || type == typeof(TimeSpan);
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x0010EBF0 File Offset: 0x0010DBF0
		internal static void ExtractValuesFromBindableControls(IOrderedDictionary dictionary, Control container)
		{
			IBindableControl bindableControl = container as IBindableControl;
			if (bindableControl != null)
			{
				bindableControl.ExtractValues(dictionary);
			}
			foreach (object obj in container.Controls)
			{
				Control control = (Control)obj;
				DataBoundControlHelper.ExtractValuesFromBindableControls(dictionary, control);
			}
		}
	}
}
