// <copyright file="MediaType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Test.Models
{
    using System;

    [MediaType(
        "7bbd6ff5-54ac-4b5a-80fb-4adabe366bcd",
        "MediaType",
        Description = "Description",
        AllowedAsRoot = true,
        AllowedChildNodeTypes = new Type[] { })]
    public class MediaType : ContentType
    {
    }
}