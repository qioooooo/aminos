using System;

namespace System.Data.Design
{
	internal enum DbObjectType
	{
		Unknown,
		Table,
		View,
		StoredProcedure,
		Function,
		Package,
		PackageBody
	}
}
