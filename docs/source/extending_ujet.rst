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
Supports types `bool`, and `bool?`. Maps to Umbraco data type `TrueFalse`.

DateTimeDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Supports types `DateTime`, and `DateTime?`. Maps to Umbraco data type `DatePicker`.

FloatingBinaryPointDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Supports types `float`, `float?`, `double`, and `double?`. Maps to Umbraco data type `Textstring`. Converted using property value converter `FloatingBinaryPointPropertyValueConverter`.

FloatingDecimalPointDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Supports types `decimal`, and `decimal?`. Maps to Umbraco data type `Textstring`. Converted using property value converter `FloatingDecimalPointPropertyValueConverter`.

IntegerDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Supports types `Int16`, `Int16?`, `Int32`, `Int32?`, `UInt16`, `UInt16?`, `UInt32`, and `UInt32?`. Maps to Umbraco data type `Numeric`.

StringDataTypeDefinitionMapping
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
Supports type `string`. Maps to Umbraco data type `Textstring`.

Property Value Converters
=========================

Built-in Property Value Converters
----------------------------------
The following property value converters are built-into Umbraco Jet.

* `FloatingBinaryPointPropertyValueConverter`
* `FloatingDecimalPointPropertyValueConverter`
* `HtmlStringPropertyValueConverter`