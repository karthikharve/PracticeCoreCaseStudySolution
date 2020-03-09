using System;
using System.Collections.Generic;
using System.Text;

namespace Practice.Core.ORM.Interfaces
{
    public interface ISystemContext : IDisposable
    {
        List<T> ExecuteProcedure<T>(string procedureName, IDictionary<string, object> procedureParameters);

        int CommitChanges();
    }
}
