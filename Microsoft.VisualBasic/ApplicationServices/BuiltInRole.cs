using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[TypeConverter(typeof(BuiltInRoleConverter))]
	public enum BuiltInRole
	{
		AccountOperator = 548,
		Administrator = 544,
		BackupOperator = 551,
		Guest = 546,
		PowerUser,
		PrintOperator = 550,
		Replicator = 552,
		SystemOperator = 549,
		User = 545
	}
}
