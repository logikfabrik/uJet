***********************
Working with Data Types
***********************

Data Type Definition Mappings
=============================
When creating a document type, media type, or member type with properties, the Umbraco data types used are inferred by the .NET data types of the properties declared. A property of type `bool` will be mapped to the data type `True/False` by default, a property of type `string` to the data type `Textstring` and so on.

.NET data types are mapped to Umbraco data types using data type definition mappings (DTDM). The Umbraco data type mapped will determine how Umbraco stores the property value in the database, and what property editor to use for editing the property value in the Umbraco back office.

Built-in Data Type Definition Mappings
--------------------------------------
The following data type definition mappings are built into uJet. Class names have been shortened.

* `BooleanDTDM`
* `DateTimeDTDM`
* `FloatingBinaryPointDTDM`
* `FloatingDecimalPointDTDM`
* `IntegerDTDM`
* `StringDTDM`

BooleanDTDM
^^^^^^^^^^^
Will map .NET types `bool`, and `bool?` to the Umbraco data type `TrueFalse`.

DateTimeDTDM
^^^^^^^^^^^^
Will map .NET types `DateTime`, and `DateTime?` to the Umbraco data type `DatePicker`.

FloatingBinaryPointDTDM
^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET types `float`, `float?`, `double`, and `double?` to the Umbraco data type `Textstring`. Converted using property value converter `FloatingBinaryPointPropertyValueConverter`.

FloatingDecimalPointDTDM
^^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET types `decimal`, and `decimal?` to the Umbraco data type `Textstring`. Converted using property value converter `FloatingDecimalPointPropertyValueConverter`.

IntegerDTDM
^^^^^^^^^^^
Will map .NET types `Int16`, `Int16?`, `Int32`, `Int32?`, `UInt16`, `UInt16?`, `UInt32`, and `UInt32?` to the Umbraco data type `Numeric`.

StringDTDM
^^^^^^^^^^
Will map .NET type `string` to the Umbraco data type `Textstring`.

Custom Data Type Definition Mappings
------------------------------------
uJet can easily be extended to support additional .NET types and Umbraco data types. Implement the `IDataTypeDefinitionMapping` interface and add the implementation to the list of data type definition mappings by calling `DataTypeDefinitionMappings.Mappings.Add()`.

Property Value Converters
=========================
The Umbraco database schema has it's limitations; e.g. the Umbraco database schema for Microsoft SQL Server supports property values of types `int`, `datetime`, `nvarchar`, and `ntext`. .NET types without a supported SQL Server counterpart must therefore be stored as `nvarchar` or `ntext`. Property value converters (PVC) are used to convert property values stored as `nvarchar` or `ntext` to .NET types on model binding, e.g. `decimal` and `float`.

Property value converters can also be used to add support for custom complex types.

Built-in Property Value Converters
----------------------------------
The following property value converters are built into uJet. Class names have been shortened.

* `FloatingBinaryPointPVC`
* `FloatingDecimalPointPVC`
* `HtmlStringPVC`

Custom Property Value Converters
--------------------------------
uJet can easily be extended to support additional .NET types. Implement the `IPropertyValueConverter` interface and add the implementation to the list of property value converters by calling `PropertyValueConverters.Converters.Add()`.