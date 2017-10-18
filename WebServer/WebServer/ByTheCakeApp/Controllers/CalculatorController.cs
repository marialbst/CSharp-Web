namespace WebServer.ByTheCakeApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    using Server.HTTP.Contracts;

    public class CalculatorController : Controller
    {
        private static readonly Dictionary<string, Func<double, double, double>> operations = ParseOperators();

        public IHttpResponse CalculateGet()
        {
            return this.FileViewResponse("calculator");
        }

        public IHttpResponse CalculatePost(string number1, string op, string number2)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            double firstNum;
            double secondNum;

            bool parseNum1Result = double.TryParse(number1, out firstNum);
            bool parseNume2Result = double.TryParse(number2, out secondNum);

            if (!parseNum1Result || !parseNume2Result)
            {
                result.Add("Error","Invalid Number");
                return this.FileViewResponse("calculator", result); ;
            }

            if (operations.ContainsKey(op))
            {
                result.Add("Result", operations[op].Invoke(firstNum, secondNum).ToString());
            }
            else
            {
                result.Add("Error", "Invalid Sign. Operator should be +, -, * or /");
            }
            
            return this.FileViewResponse("calculator", result);
        }

        private static Dictionary<string, Func<double, double, double>> ParseOperators()
        {
            Dictionary<string, Func<double, double, double>> operatorsParser = new Dictionary<string, Func<double, double, double>>();

            operatorsParser.Add("+", (a, b) => a + b);
            operatorsParser.Add("-", (a, b) => a - b);
            operatorsParser.Add("*", (a, b) => a * b);
            operatorsParser.Add("/", (a, b) => a / b);

            return operatorsParser;
        }
    }
}
