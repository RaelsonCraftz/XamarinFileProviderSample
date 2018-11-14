using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace FileProviderSample
{
    public interface ISave
    {

        Task Save(string filename, string contentType, MemoryStream stream);

    }
}
