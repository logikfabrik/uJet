**************
Extending uJet
**************

Data Type Definition Mappings
=============================

Built-in Data Type Definition Mappings
--------------------------------------
The following data type definition mappings are built-into Umbraco Jet.

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
The following property value converters are built-into Umbraco Jet.

* `FloatingBinaryPointPropertyValueConverter`
* `FloatingDecimalPointPropertyValueConverter`
* `HtmlStringPropertyValueConverter`