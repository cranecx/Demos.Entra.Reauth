using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.Entra.Reauth.Shared
{
    public class RetrievedMessage
    {
        public Guid Id { get; set; }
        public string? Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
