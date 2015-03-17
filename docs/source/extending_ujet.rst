**************
Extending uJet
**************

Data Type Definition Mappings
=============================
.NET data types are mapped to Umbraco data type definitions using data type definition mappings. The Umbraco data type definition mapped will determine how Umbraco stores the property value in the database, and what editor to use for editing the property value in the Umbraco back office.

uJet can easily be extended to support additional types by implementing the `IDataTypeDefinitionMapping` interface and adding the implementation to the list of data type definition mappings. Add implementations to the list of data type definition mappings by calling `DataTypeDefinitionMappings.Mappings.Add()`.

Built-in Data Type Definition Mappings
--------------------------------------
The following data type definition mappings are built-into uJet.

BooleanDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET types `bool`, and `bool?` to the Umbraco data type definition `TrueFalse`.

DateTimeDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET types `DateTime`, and `DateTime?` to the Umbraco data type definition `DatePicker`.

FloatingBinaryPointDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET types `float`, `float?`, `double`, and `double?` to the Umbraco data type definition `Textstring`. Converted using property value converter `FloatingBinaryPointPropertyValueConverter`.

FloatingDecimalPointDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET types `decimal`, and `decimal?` to the Umbraco data type definition `Textstring`. Converted using property value converter `FloatingDecimalPointPropertyValueConverter`.

IntegerDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET types `Int16`, `Int16?`, `Int32`, `Int32?`, `UInt16`, `UInt16?`, `UInt32`, and `UInt32?` to the Umbraco data type definition `Numeric`.

StringDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Will map .NET type `string` to the Umbraco data type definition `Textstring`.

Property Value Converters
=========================

Built-in Property Value Converters
----------------------------------
The following property value converters are built-into uJet.

* `FloatingBinaryPointPropertyValueConverter`
* `FloatingDecimalPointPropertyValueConverter`
* `HtmlStringPropertyValueConverter`