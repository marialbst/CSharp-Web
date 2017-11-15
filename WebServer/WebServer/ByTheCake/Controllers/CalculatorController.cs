namespace WebServer.ByTheCake.Controllers
{
    using System;
    using System.Collections.Generic;
    using Server.HTTP.Contracts;
    using Server.HTTP.Response;
    using Server.Enums;
    using Views;

    public class CalculatorController
    {
        private static readonly Dictionary<string, Func<double, double, double>> operations = ParseOperators();

        public IHttpResponse Calculate()
        {
            return new ViewResponse(HttpStatusCode.Ok, new CalculatorView());
        }

        public IHttpResponse Calculate(Dictionary<string, string> formData)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string num1 = string.Empty;
            string num2 = string.Empty;
            string op = string.Empty;

            if (formData.ContainsKey("number1"))
            {
                num1 = formData["number1"];
            }

            if (formData.ContainsKey("number2"))
            {
                num2 = formData["number2"];
            }

            if (formData.ContainsKey("operator"))
            {
                op = formData["operator"];
            }

            double firstNum;
            double secondNum;

            bool parseNum1Result = double.TryParse(num1, out firstNum);
            bool parseNume2Result = double.TryParse(num2, out secondNum);

            if (!parseNum1Result || !parseNume2Result)
            {
                result.Add("Error", "Invalid Number");
                return new ViewResponse(HttpStatusCode.Ok, new CalculatorView(result));
            }

            if (operations.ContainsKey(op))
            {
                result.Add("Result", operations[op].Invoke(firstNum, secondNum).ToString());
            }
            else
            {
                result.Add("Invalid operator", "Operator should be +, -, * or /");
            }

            return new ViewResponse(HttpStatusCode.Ok, new CalculatorView(result));
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
