using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcForge.Compiler
{

    [AttributeUsage(AttributeTargets.Field)]
    public class CompliableAttribute : Attribute
    {
        public string Template { get; }
        public CompileNeeds[] Needs { get; }

        public CompliableAttribute(string template, params CompileNeeds[] needs)
        {
            Template = template;
            Needs = needs;
        }
    }


}
