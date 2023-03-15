using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Analysis
{
    public class TypeAnalyser
    {
        public static string GetTypeAnalysis(object obj)
        {
            if (obj == null)
                return "ERROR: Can not analyze empty object";
            StringBuilder b = new StringBuilder();
            try
            {
                Type type = obj.GetType();
                b.AppendLine("================= TYPE ANALYSIS ====================");
                b.AppendLine("Name:      " + type.Name);
                b.AppendLine("Full name: " + type.FullName);
                b.AppendLine("[GENERIC ARGUMENTS]");
                b.AppendLine("Generic args count: " + type.GenericTypeArguments.Length);
                foreach (var arg in type.GenericTypeArguments)
                    b.AppendLine("\t" + arg.Name.PadRight(20) + " : " + arg.ToString().PadRight(20));
                b.AppendLine("[FIELDS]");
                b.AppendLine("Fileds count: " + type.GetFields().Length);
                foreach (var arg in type.GetFields())
                    b.AppendLine("\t" + arg.Name.PadRight(20) + " : " + arg.ToString().PadRight(20));
                b.AppendLine("[MEMBERS]");
                b.AppendLine("Members count: " + type.GetMembers().Length);
                foreach (var arg in type.GetMembers())
                    b.AppendLine("\t" + arg.Name.PadRight(20) + " : " + arg.ToString().PadRight(20));
                b.AppendLine("[PROPERTIES]");
                b.AppendLine("Properties count: " + type.GetProperties().Length);
                foreach (var arg in type.GetProperties())
                    b.AppendLine("\t" + arg.Name.PadRight(20) + " : " + arg.ToString().PadRight(20));
                b.AppendLine("[METHODS]");
                b.AppendLine("Methods count: " + type.GetMethods().Length);
                foreach (var arg in type.GetMethods())
                {
                    b.AppendLine("\t" + arg.Name.PadRight(20) + " : " + arg.ToString().PadRight(20));
                    b.Append("\t\t( ");
                    foreach (var p in arg.GetParameters())
                        b.Append(p.ParameterType.Name + " " + p.Name + ", ");
                    b.AppendLine(")");
                }
                b.AppendLine("======================= END ========================");
            }
            catch (Exception ex)
            {
                b.AppendLine("ERROR: Initial error: " + ex.Message);
            }
            return b.ToString();
        }
    }
}
