using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;

namespace Gali.AppServer.Services
{
    public class CommandsService : Commands.CommandsBase
    {
        public override Task<ExecuteReply> Execute(ExecuteRequest request, ServerCallContext context)
        {
            return base.Execute(request, context);
        }
    }
}
