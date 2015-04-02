**********************
Working with Templates
**********************

Template Synchronization
------------------------
Templates are created through the Umbraco back office. When creating a template the template markup is saved to a `.cshtml` file in the `Views/` folder, and the database.

Copying `.cshtml` files from one Umbraco setup to another is not enough to make the copied templates available in the back office. The database must reflect the contents of the `Views/` folder.

uJet supports template synchronization. When your Umbraco application is started, uJet will scan the `Views/` folder, looking for `.cshtml` files. Found files will be used as blueprints to synchronize your database. When your database has been updated, the copied templates will be available in the back office.

.. note::
   uJet will not synchronize files with file names that starts with an underscore. E.g. files such as `_layout.cshtml` and `_viewstart.cshtml` will be excluded when synchronizing templates.

.. note::
   uJet does not scan subfolders in the `Views/` folder.

Preview Template
----------------