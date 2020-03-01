using FinPlan.BackEnd.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.BackEnd.Services.Interfaces
{
    public interface ICsvParser
    {
        Task<IEnumerable<Transaction>> ParseFileAsync(Stream csvStream);
    }
}
