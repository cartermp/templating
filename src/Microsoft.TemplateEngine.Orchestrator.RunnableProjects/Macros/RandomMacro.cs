using System;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Core.Contracts;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Macros.Config;
using Newtonsoft.Json.Linq;

namespace Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Macros
{
    internal class RandomMacro : IMacro
    {
        public Guid Id => new Guid("011E8DC1-8544-4360-9B40-65FD916049B7");

        public string Type => "random";

        public void EvaluateConfig(IVariableCollection vars, IMacroConfig rawConfig, IParameterSet parameters, ParameterSetter setter)
        {
            RandomMacroConfig config = rawConfig as RandomMacroConfig;

            if (config == null)
            {
                throw new InvalidCastException("Couldn't cast the rawConfig as RandomMacroConfig");
            }

            switch (config.Action)
            {
                case "new":
                    Random rnd = new Random();
                    int value = rnd.Next(config.Low, config.High);
                    Parameter p = new Parameter
                    {
                        IsVariable = true,
                        Name = config.VariableName
                    };

                    setter(p, value.ToString());
                    break;
            }
        }

        public void Evaluate(string variableName, IVariableCollection vars, JObject def, IParameterSet parameters, ParameterSetter setter)
        {
            switch (def["action"].ToString())
            {
                case "new":
                    int low = def.ToInt32("low");
                    int high = def.ToInt32("high", int.MaxValue);
                    Random rnd = new Random();
                    int val = rnd.Next(low, high);
                    string value = val.ToString();
                    Parameter p = new Parameter
                    {
                        IsVariable = true,
                        Name = variableName
                    };

                    setter(p, value);
                    break;
            }
        }
    }
}