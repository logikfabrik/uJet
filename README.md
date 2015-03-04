# uJet [![Build status](https://ci.appveyor.com/api/projects/status/2ce4vbe5dexoqey7)](https://ci.appveyor.com/project/logikfabrik/ujet)
Umbraco Jet (uJet) is a Code First approach to building MVC applications in Umbraco 7. Declare your document, media, and data types in code, and have uJet move them into Umbraco for you - simplifying development, deployment and versioning.

uJet is capable of serving you with instances of your document types. With uJet you're no longer bound by the `RenderModel`, or by the constraints set by the built-in `ControllerActionInvoker`. uJet brings you fully typed views without requiring you to roll your own view models.

uJet supports document types, media types, data types, and template synchronization.

### Documentation
Online documentation can be found at [http://ujet.readthedocs.org/](http://ujet.readthedocs.org/).

## How To
uJet is easy to use. Add a reference to uJet. Then create your types and decorate them using the `DocumentType`, `MediaType`, and `DataType` attributes. Fire up your application and all of your types will now be available in the Umbraco back office.

### NuGet
```
PM> Install-Package uJet
```

## Quick Start Example
1. Get, compile, and reference the source code (the latest stable version is 2.3.1.0) - or use the NuGet.
2. Create a model; a document type. Use data annotations for editorial support (validation) in the back office. Properties such as `Id`, `Url` and `Name` will be mapped by convention.
3. Create a view. Name it according to the action method defined in the controller, `Views\MyPage\Index.cshtml`.
4. Create a controller inheriting from `Logikfabrik.Umbraco.Jet.Web.Mvc.JetController`.
5. Run your application and log in to Umbraco. The document type *My page* should now be available. Pages you create using this document type will all be handled by the `MyPageController`. **It's that easy!**

**Model**
```csharp
using System.ComponentModel.DataAnnotations;
using Logikfabrik.Umbraco.Jet;

namespace Example.Mvc.Models
{
    [DocumentType(
        "My page",
        Description = "Document type for my page",
        AllowedAsRoot = true)]
    public class MyPage
    {
        [ScaffoldColumn(false)]
        public string Name { get; set; }

        [Display(
            Name = "Description",
            Description = "A short description of my page",
            GroupName = "My group",
            Order = 100)]
        [Required]
        public string Description { get; set; }
    }
}
```

**View**
```html
@model Example.Mvc.Models.MyPage

<!DOCTYPE html>
<html>
<head>
    <title></title>
</head>
<body>
    <h1>@Model.Name</h1>
    <p>@Model.Description</p>
</body>
</html>
```

**Controller**
```csharp
using Logikfabrik.Umbraco.Jet.Web.Mvc;
using Example.Mvc.Models;
using System.Web.Mvc;

namespace Example.Mvc.Controllers
{
    public class MyPageController : JetController
    {
        public ActionResult Index(MyPage model)
        {
            return View(model);
        }
    }
}
```
## Features

### Supported data annotations
uJet supports the following data annotations.

* `RequiredAttribute`
* `DefaultValueAttribute`
* `RegularExpressionAttribute`
* `UIHintAttribute`
* `DisplayAttribute`
* `ScaffoldColumnAttribute`

##### `RequiredAttribute`
Properties decorated using the `RequiredAttribute` attribute will be mandatory in the Umbraco back office.

##### `DefaultValueAttribute`
Properties decorated using the `DefaultValueAttribute` attribute will have a default value of whatever value was set when setting the attribute.

##### `RegularExpressionAttribute`
Properties decorated using the `RegularExpressionAttribute` attribute will be validated in the Umbraco back office using the regular expression specified.

##### `DisplayAttribute`
Use the `DisplayAttribute` attribute to customize the property name and description in the Umbraco back office.

##### `UIHintAttribute`
Use the `UIHintAttribute` attribute to specify the property editor used in the Umbraco back office. The property editor is inferred by the property type by default, but can be overridden using this attribute.

##### `ScaffoldColumnAttribute`
Properties decorated using the `ScaffoldColumnAttribute` attribute (set to false) will not be available for editors through the Umbraco back office.

### Supported data types
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

To support additional data types, implement interface `Logikfabrik.Umbraco.Jet.MappingsIDataTypeDefinitionMapping` and add the implementation to the list of available mappings (`Logikfabrik.Umbraco.Jet.Mappings.DataTypeDefinitionMappings.Mappings.Add(...)`).