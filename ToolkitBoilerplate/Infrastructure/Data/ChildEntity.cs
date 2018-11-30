using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ToolkitBoilerplate.Infrastructure;

namespace ToolkitBoilerplate.Infrastructure.Data
{
    [DataContract]
    public class ChildEntity<TParent> : ApplicationEntity
        where TParent : ApplicationEntity, new()
    {
        [DataMember]
        public int ParentId { get; set; }
        public TParent Parent { get; set; }
    }
}
