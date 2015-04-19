//----------------------------------------------------------------------------------
// <copyright file="DatabaseWrapper.cs" company="Logikfabrik">
//     The MIT License (MIT)
//
//     Copyright (c) 2015 anton(at)logikfabrik.se
//
//     Permission is hereby granted, free of charge, to any person obtaining a copy
//     of this software and associated documentation files (the "Software"), to deal
//     in the Software without restriction, including without limitation the rights
//     to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//     copies of the Software, and to permit persons to whom the Software is
//     furnished to do so, subject to the following conditions:
//
//     The above copyright notice and this permission notice shall be included in
//     all copies or substantial portions of the Software.
//
//     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//     IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//     FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//     AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//     LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//     OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//     THE SOFTWARE.
// </copyright>
//----------------------------------------------------------------------------------

namespace Logikfabrik.Umbraco.Jet.Data
{
    using System;
    using System.Reflection;
    using global::Umbraco.Core.Persistence;

    public class DatabaseWrapper : IDatabaseWrapper
    {
        private readonly Database database;

        public DatabaseWrapper(Database database)
        {
            if (database == null)
            {
                throw new ArgumentNullException("database");
            }

            this.database = database;
        }

        public T GetRow<T>(object primaryKey)
        {
            if (primaryKey == null)
            {
                throw new ArgumentNullException("primaryKey");
            }

            return this.database.SingleOrDefault<T>(primaryKey);
        }

        public void InsertRow<T>(T row, object primaryKey) where T : class
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            if (!this.database.Exists<T>(primaryKey))
            {
                this.database.Insert(row);
            }
            else
            {
                this.database.Update(row);
            }
        }

        public bool TableExists<T>()
        {
            var tableName = GetTableName<T>();

            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentException(string.Format("Table name cannot be null or white space for type {0}.", typeof(T)));
            }

            return this.database.TableExist(tableName);
        }

        public void CreateTable<T>() where T : new()
        {
            if (this.TableExists<T>())
            {
                return;
            }

            this.database.CreateTable<T>();
        }

        private static string GetTableName<T>()
        {
            var attribute = typeof(T).GetCustomAttribute<TableNameAttribute>();

            return attribute == null ? null : attribute.Value;
        }
    }
}
