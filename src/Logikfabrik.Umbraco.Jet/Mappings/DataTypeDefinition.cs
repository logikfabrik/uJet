// <copyright file="DataTypeDefinition.cs" company="Logikfabrik">
//   Copyright (c) 2016 anton(at)logikfabrik.se. Licensed under the MIT license.
// </copyright>

namespace Logikfabrik.Umbraco.Jet.Mappings
{
    /// <summary>
    /// The <see cref="DataTypeDefinition" /> enumeration for Umbraco built-in data types.
    /// </summary>
    public enum DataTypeDefinition
    {
        /// <summary>
        /// Data type for approved color.
        /// </summary>
        ApprovedColor = -37,

        /// <summary>
        /// Data type for list of check boxes.
        /// </summary>
        CheckboxList = -43,

        /// <summary>
        /// Data type for content picker.
        /// </summary>
        ContentPicker = 1034,

        /// <summary>
        /// Data type for new content picker.
        /// </summary>
        ContentPicker2 = 1046,

        /// <summary>
        /// Data type for short date picker.
        /// </summary>
        DatePicker = -41,

        /// <summary>
        /// Data type for long date picker.
        /// </summary>
        DatePickerWithTime = -36,

        /// <summary>
        /// Data type for drop down.
        /// </summary>
        Dropdown = -42,

        /// <summary>
        /// Data type for drop down multiple.
        /// </summary>
        DropdownMultiple = -39,

        /// <summary>
        /// Data type for folder browser.
        /// </summary>
        FolderBrowser = -38,

        /// <summary>
        /// Data type for label.
        /// </summary>
        Label = -92,

        /// <summary>
        /// Data type for media picker.
        /// </summary>
        MediaPicker = 1035,

        /// <summary>
        /// Data type for new media picker.
        /// </summary>
        MediaPicker2 = 1048,

        /// <summary>
        /// Data type for member picker.
        /// </summary>
        MemberPicker = 1036,

        /// <summary>
        /// Data type for new member picker.
        /// </summary>
        MemberPicker2 = 1047,

        /// <summary>
        /// Data type for multiple media picker.
        /// </summary>
        MultipleMediaPicker = 1045,

        /// <summary>
        /// Data type for number.
        /// </summary>
        Numeric = -51,

        /// <summary>
        /// Data type for list of radio buttons.
        /// </summary>
        Radiobox = -40,

        /// <summary>
        /// Data type for related links.
        /// </summary>
        RelatedLinks = 1040,

        /// <summary>
        /// Data type for new related links.
        /// </summary>
        RelatedLinks2 = 1050,

        /// <summary>
        /// Data type for rich text.
        /// </summary>
        RichtextEditor = -87,

        /// <summary>
        /// Data type for tags.
        /// </summary>
        Tags = 1041,

        /// <summary>
        /// Data type for textbox multiple.
        /// </summary>
        TextboxMultiple = -89,

        /// <summary>
        /// Data type for text string.
        /// </summary>
        Textstring = -88,

        /// <summary>
        /// Data type for true or false.
        /// </summary>
        TrueFalse = -49,

        /// <summary>
        /// Data type for upload.
        /// </summary>
        Upload = -90
    }
}
