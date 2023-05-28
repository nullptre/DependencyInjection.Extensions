using System;
using System.Collections.Generic;
using System.Text;

namespace Neleus.DependencyInjection.Extensions
{
    public class TypeLifetime
    {
        public Lifetime Lifetime { get; set; } = Lifetime.Transient;
        public Type Type { get; set; } = typeof(Type);
        public object Instance { get; set; }
    }
}
