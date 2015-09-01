**************
Extending uJet
**************

Custom Data Type Definition Mappings
====================================
uJet can easily be extended to support additional .NET types and Umbraco data types. By implementing the `IDataTypeDefinitionMapping` interface and adding the implementation to the list of data type definition mappings, uJet will play nice.

Custom mappings are added to the list of data type definition mappings by calling `DataTypeDefinitionMappings.Mappings.Add()`.