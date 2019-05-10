using System;
using System.Runtime.InteropServices;

namespace Standard
{
	[Guid("4e530b0a-e611-4c77-a3ac-9031d022281b")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IApplicationAssociationRegistration
	{
		void ClearUserAssociations();

		bool QueryAppIsDefault(string pszQuery, AT atQueryType, AL alQueryLevel, string pszAppRegistryName);

		bool QueryAppIsDefaultAll(AL alQueryLevel, string pszAppRegistryName);

		string QueryCurrentDefault(string pszQuery, AT atQueryType, AL alQueryLevel);

		void SetAppAsDefault(string pszAppRegistryName, string pszSet, AT atSetType);

		void SetAppAsDefaultAll(string pszAppRegistryName);
	}
}