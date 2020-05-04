using _3NF.Decomposition.Core.Interfaces;
using _3NF.Decomposition.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace _3NF.Decomposition.Core
{
    public static class DependencyInjection
    {
        public static void AddCore(this IServiceCollection services)
        {
            services.AddScoped<IRelationService, RelationService>();
            services.AddScoped<IDecompositionService, DecompositionService>();
        }
    }
}
