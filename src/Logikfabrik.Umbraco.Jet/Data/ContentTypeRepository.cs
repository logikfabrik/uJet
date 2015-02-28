// The MIT License (MIT)

// Copyright (c) 2015 anton(at)logikfabrik.se

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace Logikfabrik.Umbraco.Jet.Data
{
    public class ContentTypeRepository : IContentTypeRepository
    {
        private readonly IDatabaseWrapper _databaseWrapper;

        public ContentTypeRepository(IDatabaseWrapper databaseWrapper)
        {
            if (databaseWrapper == null)
                throw new ArgumentNullException("databaseWrapper");

            _databaseWrapper = databaseWrapper;
        }

        public int? GetContentTypeId(Guid id)
        {
            if (!_databaseWrapper.TableExists<ContentTypeRow>())
                return null;

            var row = _databaseWrapper.GetRow<ContentTypeRow>(id);

            if (row == null)
                return null;

            return row.ContentTypeId;
        }

        public int? GetPropertyTypeId(Guid id)
        {
            if (!_databaseWrapper.TableExists<PropertyTypeRow>())
                return null;

            var row = _databaseWrapper.GetRow<PropertyTypeRow>(id);

            if (row == null)
                return null;

            return row.PropertyTypeId;
        }

        public void SetContentTypeId(Guid id, int contentTypeId)
        {
            var row = new ContentTypeRow { Id = id, ContentTypeId = contentTypeId };

            _databaseWrapper.CreateTable<ContentTypeRow>();
            _databaseWrapper.InsertRow(row);
        }

        public void SetPropertyTypeId(Guid id, int propertyTypeId)
        {
            var row = new PropertyTypeRow { Id = id, PropertyTypeId = propertyTypeId };

            _databaseWrapper.CreateTable<PropertyTypeRow>();
            _databaseWrapper.InsertRow(row);
        }
    }
}
