// <copyright file="ContentType.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

using Logikfabrik.Umbraco.Jet.Mappings;

namespace Logikfabrik.Umbraco.Jet.Test.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public abstract class ContentType
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [DefaultValue("Default")]
        public string StringProperty { get; set; }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string StringPropertyWithoutSetter { get; }

        public string StringPropertyWithoutGetter
        {
            set => throw new Exception();
        }

        [ScaffoldColumn(false)]
        public string NonScaffoldedStringProperty { get; set; }

        [DefaultValue(1)]
        public int IntegerProperty { get; set; }

        [DefaultValue(1.1f)]
        public float FloatingBinaryPointProperty { get; set; }

        [DefaultValue(typeof(decimal), "1.1")]
        public decimal FloatingDecimalPointProperty { get; set; }

        [DefaultValue(true)]
        public bool BooleanProperty { get; set; }

        [DefaultValue(typeof(DateTime), "2016-01-01T09:30:00Z")]
        public DateTime DateTimeProperty { get; set; }

        // ReSharper disable once UnusedMember.Local
        private string PrivateStringProperty { get; set; }
    }
}
