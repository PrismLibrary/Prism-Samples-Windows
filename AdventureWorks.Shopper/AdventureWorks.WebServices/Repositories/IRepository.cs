// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace AdventureWorks.WebServices.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetItem(int id);
        T Create(T item);
        bool Update(T item);
        bool Delete(int id);
        void Reset();
    }
}