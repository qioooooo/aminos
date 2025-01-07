using System;
using System.Collections;

namespace System.Web.UI.Design
{
	public abstract class WebFormsReferenceManager
	{
		public abstract Type GetType(string tagPrefix, string tagName);

		public abstract string GetTagPrefix(Type objectType);

		public abstract string RegisterTagPrefix(Type objectType);

		public abstract ICollection GetRegisterDirectives();

		public abstract string GetUserControlPath(string tagPrefix, string tagName);
	}
}
