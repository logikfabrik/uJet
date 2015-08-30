## 3.2.0.0
* 2015-08-30 Compiled using Umbraco 7.2.8 binaries.
* 2015-08-30 Reverted changes to the `JetViewEngine` made 2015-05-04.

## 3.1.1.0
* 2015-06-01 Bug fix for when using `ContentAttribute` without setting the `Name` property, causing exception to be thrown when editing users through the back office.
* 2015-05-04 Bug fix for later Umbraco versions, 7.2.4 known. `ViewLocationFormats` and `PartialViewLocationFormats` was changed to LINQ queries, causing exceptions in the `JetViewEngine`.

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