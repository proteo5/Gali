using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gali
{
    public class GaliMiddleware
    {
        private readonly RequestDelegate _next;

        public GaliMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var x = context;
            await this._next(context);
        }
    }
}
