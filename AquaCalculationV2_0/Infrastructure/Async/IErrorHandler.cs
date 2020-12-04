using System;

namespace AquaCalculationV2_0.Infrastructure.Async
{
    interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
