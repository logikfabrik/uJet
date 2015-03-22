# uJet [![Build status](https://ci.appveyor.com/api/projects/status/2ce4vbe5dexoqey7?svg=true)](https://ci.appveyor.com/project/logikfabrik/ujet) [![Docs status](https://readthedocs.org/projects/pip/badge/)](http://ujet.readthedocs.org/)
Umbraco Jet (uJet) is a Code First approach to building MVC applications in Umbraco 7. Declare your document, media, and data types in code, and have uJet move them into Umbraco for you - simplifying development, deployment and versioning.

uJet is capable of serving you with instances of your document types. With uJet you're no longer bound by the `RenderModel`, or by the constraints set by the built-in `ControllerActionInvoker`. uJet brings you strongly typed views without requiring you to roll your own view models.

uJet is a developer tool for Umbraco 7, released as Open Source (MIT). It supports document types, media types, data types, and template synchronization.

### Documentation
Online documentation can be found at [http://ujet.readthedocs.org/](http://ujet.readthedocs.org/).

### NuGet
```
PM> Install-Package uJet
```

## How To
uJet is easy to use. Add a reference to uJet. Then create your types and decorate them using the `DocumentType`, `MediaType`, and `DataType` attributes. Fire up your application and all of your types will now be available in the Umbraco back office.

Do you want strongly typed views? Of course you do! See the Quick Start Example; strongly typed views is what Jet is all about.

## Quick Start Example
1. Get, compile, and reference the source code - or use the NuGet.
2. Create a model; a document type. Use data annotations for editorial support (validation) in the back office. Properties such as `Id`, `Url` and `Name` will be mapped by convention.
3. Create a view. Name it according to the action method defined in the controller, `Views\MyPage\Index.cshtml`.
4. Create a controller inheriting from `Logikfabrik.Umbraco.Jet.Web.Mvc.JetController`.
5. Run your application and log in to Umbraco. The document type *My page* should now be available. Pages you create using this document type will all be handled by the `MyPageController`. **It's that easy!**

**Model**
```csharp
using Logikfabrik.Umbraco.Jet;
using System.ComponentModel.DataAnnotations;

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
using Example.Mvc.Models;
using Logikfabrik.Umbraco.Jet.Web.Mvc;
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

##### Convention Over Configuration
One attribute per model, that's all uJet needs to get Code First up and running. And, for the full experience, having your controllers inherit from `JetController`. That's it! And the controller part is optional, but without it you're missing out.

##### Extendable
Found a property data type uJet can't handle? Extend it. uJet is built to be easily extended.

##### Data Annotations
uJet supports data annotations. Using data annotations uJet allows you to customize the back office, making life easier for the editors.

## Contributions
uJet is Open Source (MIT), and you’re welcome to contribute!

If you have a bug report, feature request, or suggestion, please open a new issue. To submit a patch, please send a pull request.