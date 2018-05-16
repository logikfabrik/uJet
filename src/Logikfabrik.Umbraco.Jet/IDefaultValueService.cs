using System.Collections.Generic;
using Umbraco.Core.Models;

namespace Logikfabrik.Umbraco.Jet
{
    public interface IDefaultValueService
    {
        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IEnumerable<IContent> content);

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IContent content);

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IEnumerable<IMedia> content);

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IMedia content);

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IEnumerable<IMember> content);

        /// <summary>
        /// Sets the default values.
        /// </summary>
        /// <param name="content">The content to set default values for.</param>
        void SetDefaultValues(IMember content);
    }
}