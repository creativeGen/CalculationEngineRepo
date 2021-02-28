using CalculationEngine.Expressions;
using CalculationEngine.Services;
using CalculationEngineClient.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CalculationEngineClient
{

    public static class Startup
    {
        private static IServiceProvider _serviceProvider;

        public static IServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IParseExpressions, ParseExpressions>();
            services.AddSingleton<ICallMathsAPIService, CallMathsAPIService>();
            services.AddSingleton<IExpressions, SimpleArithmeticExpression>();
            services.AddSingleton<IExpressions, ComplexArithmeticExpression>();
            services.AddSingleton<IExpressions, ChainOfExpressions>();

            services.AddSingleton<ICalculationEngineService, CalculationEngineService>();
            services.AddSingleton<ICalculateResultService, CalculateResultService>();
            return _serviceProvider = services.BuildServiceProvider(true);
        }
        public static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
