***********************
Working with Properties
***********************
Document and media type properties are created by adding properties with public getters and setters to classes decorated using the `DocumentTypeAttribute` or `MediaTypeAttribute` attributes.

When your Umbraco application is started, uJet will scan your document and media type classes, looking for properties. Found properties will be used as blueprints to synchronize your database.

Data Annotations
================
Data annotations can be used to customize the user experience in the Umbraco back office, e.g. to set property name and description.

Supported Data Annotations
--------------------------
uJet supports the following data annotations.

* `RequiredAttribute`
* `DefaultValueAttribute`
* `RegularExpressionAttribute`
* `UIHintAttribute`
* `DisplayAttribute`
* `ScaffoldColumnAttribute`

RequiredAttribute
^^^^^^^^^^^^^^^^^
Properties decorated using the `RequiredAttribute` attribute will be mandatory in the Umbraco back office.

DefaultValueAttribute
^^^^^^^^^^^^^^^^^^^^^
Properties decorated using the `DefaultValueAttribute` attribute will have a default value of whatever value was set when setting the attribute.

RegularExpressionAttribute
^^^^^^^^^^^^^^^^^^^^^^^^^^
Properties decorated using the `RegularExpressionAttribute` attribute will be validated in the Umbraco back office using the regular expression specified.

DisplayAttribute
^^^^^^^^^^^^^^^^
Use the `DisplayAttribute` attribute to customize the property name and description in the Umbraco back office.

UIHintAttribute
^^^^^^^^^^^^^^^
Use the `UIHintAttribute` attribute to specify the property editor used in the Umbraco back office. The property editor is inferred by the property type by default, but can be overridden using this attribute.

ScaffoldColumnAttribute
^^^^^^^^^^^^^^^^^^^^^^^
Properties decorated using the `ScaffoldColumnAttribute` attribute (set to false) will not be available for editors through the Umbraco back office.

Data Types
==========
Property data types are mapped to Umbraco data type definitions using data type definition mappings.

The data type property definition mapped will tell Umbraco how the property value should be stored in the database and what editor to use in the back office.

Supported Data Types
--------------------

uJet supports the following data types out-of-the-box and can be extended.

* `Int16`
* `Int32`
* `UInt16`
* `UInt32`
* `string`
* `decimal`
* `float`
* `double`
* `DateTime`