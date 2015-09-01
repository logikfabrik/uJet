****************************
Working with MVC Conventions
****************************
In addition to enabling code first development, uJet tries to close the gap between Umbraco and conventional ASP.NET MVC. uJet extends the capabilities of Umbraco; it does not strip Umbraco of any of its out-of-the-box features. It simply makes it possible to develop conventional MVC sites within Umbraco, if you like to.

Controllers and Model Binding
-----------------------------

Views
-----
In Umbraco the concept of templates and the concept of views are interchangeable; templates are Razor views saved as `.cshtml` files in the root of the `Views\` folder.

When developing with uJet, the default ASP.NET MVC naming conventions are supported. Views can be saved in controller subfolders e.g. `Views\Home\`, and/or as shared views in the subfolder `Views\Shared\`. uJet does so by providing its own implementation of the `IViewEngine` interface.

Views following the default ASP.NET MVC naming conventions will not be treated as templates; they will not be available for manual selection in the Umbraco back office. This is by design; views following the default ASP.NET MVC naming conventions are not Umbraco templates.