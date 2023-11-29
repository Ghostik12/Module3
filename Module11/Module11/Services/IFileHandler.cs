using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module11.Services
{
    public interface IFileHandler
    {
        Task Dowmload(string fileId, CancellationToken ct);
        string Process(string param);
    }
}
