## 3.1.0.0

* 2015-04-05 Fixed an issue in the type service. TypeLoadException and ReflectionTypeLoadException are now ignored.

## 3.0.0.0

* 2015-03-02 Added type constraints in the type service.
* 2015-02-28 Fixed an issue with uJet when setting up Umbraco for the first time.
* 2015-02-28 Added tracking. Setting the ID (`Guid`) property of `DocumentTypeAttribute`, `MediaTypeAttribute`, and `DataTypeAttribute` will enable tracking. Properties can be tracked by using the `IdAttribute`. Tracking allows you to rename types and properties in your code - uJet will update existing types and properties in the Umbraco DB.
* 2015-02-19 Improved the performance of the type service.
* 2015-02-17 Removed the use of the data type definition mappings when querying property value converters. Using the `UIHint` attribute in combination with a property value converter will no longer trigger queries to the data type definition mappings.
* 2015-02-17 Fixed an issue in the property value converter for `decimal` and `decimal?` values.
* 2015-02-17 Added a property value converter for `float`, `float?`, `double`, and `double?` values.