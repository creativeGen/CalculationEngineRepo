using System;
using CalculationEngineClient.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CalculationEngineClient
{
    class Program
    {
        static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            _serviceProvider = Startup.RegisterServices();

            EvaluateExpressions();

            Startup.DisposeServices();
        }

        private static void EvaluateExpressions()
        {
            IServiceScope scope = _serviceProvider.CreateScope();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("*****                    CALCULATION ENGINE CLIENT                   *******");
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Enter one or more arithmetic expression(s) separated by -:");
            var strExpressions = Console.ReadLine().Trim();
            while (string.IsNullOrEmpty(strExpressions))
            {
                Console.WriteLine("Please enter one or more arithmetic expression(s) separated by -: ");
                strExpressions = Console.ReadLine().Trim();
            }

            Console.WriteLine("Do you want to follow BEDMAS to evaluate expression(s)? - y/n");
            var applyBEDMAS = Console.ReadLine().ToLower() == "y" ? true : false;            

            if (!string.IsNullOrEmpty(strExpressions))
            {

                var strExpressionsArr = strExpressions.Split("-:");
                var calculationEngineService = scope.ServiceProvider.GetRequiredService<ICalculationEngineService>();
                var response = calculationEngineService.CalculateResult(strExpressionsArr, applyBEDMAS);
                if (response != null && response.Result != null && response.Result.result != null)
                {
                    for (int i = 0; i < strExpressionsArr.Length; i++)
                    {
                        Console.WriteLine($"Result for expression {strExpressionsArr[i].Trim()} is {response.Result.result[i]}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error retrieving the results for the expressions - {response.Result.error}");
                }
            }
            Console.ReadLine();
        }
        
    }
}
