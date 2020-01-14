using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gali
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGali(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GaliMiddleware>();
        }
    }
}
