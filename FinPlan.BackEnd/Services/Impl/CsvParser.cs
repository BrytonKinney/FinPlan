using FinPlan.BackEnd.Data;
using FinPlan.BackEnd.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinPlan.BackEnd.Services.Impl
{
    // todo: genericize
    public class CsvParser : ICsvParser
    {
        public async Task<IEnumerable<Transaction>> ParseFileAsync(Stream csvStream)
        {
            using (var sr = new StreamReader(csvStream))
            {
                var line = await sr.ReadLineAsync().ConfigureAwait(false);
                List<CsvColumnMapping> columnMappings = new List<CsvColumnMapping>();
                int columnIterator = 0;
                foreach (var header in line.Split(','))
                {
                    columnMappings.Add(new CsvColumnMapping { Ordinal = columnIterator++, Name = header });
                }
                var resultList = new List<Transaction>();
                while (!sr.EndOfStream)
                {
                    line = await sr.ReadLineAsync().ConfigureAwait(false);
                    var splitLine = line.Split(',', StringSplitOptions.None).ToList();
                    if (splitLine.All(l => string.IsNullOrEmpty(l)))
                        break;
                    while(splitLine.Count < columnMappings.Count)
                    {
                        splitLine.Add("");
                    }
                    for(int i = 0; i < splitLine.Count; i++)
                    {
                        splitLine[i] = splitLine[i].Replace("\"", string.Empty);
                    }
                    int.TryParse(splitLine[8], out var mcc);
                    resultList.Add(new Transaction()
                    {
                        TransactionDate = DateTime.Parse(splitLine[0]),
                        PostDate = DateTime.Parse(splitLine[1]),
                        Reference = splitLine[2],
                        Description = splitLine[3],
                        Amount = decimal.Parse(splitLine[4]),
                        AccountNumber = splitLine[5],
                        CardNumber = splitLine[6],
                        CardholderName = splitLine[7],
                        MCC = mcc,
                        MCCDescription = splitLine[9],
                        MCCGroup = splitLine[10]
                    });
                }
                return resultList;
            }
        }
    }
    internal class CsvColumnMapping
    {
        public int Ordinal { get; set; }
        public string Name { get; set; }
    }
}
