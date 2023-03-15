using Esprima.Ast;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulControl.Unit
{
    public class BufebleUnit<ValueType>
    {
        private ValueType? stat;
        private ValueType? standart;

        public ValueType Value
        {
            get => stat;
            set => stat = value;
        }
        public ValueType Standart
        {
            get => standart;
            set => standart = value;
        }

        public BufebleUnit() 
        { 
            standart = default(ValueType);
            stat = standart; 
        }
        public BufebleUnit(ValueType basicValue) 
        {
            standart = basicValue;
            stat = basicValue;
        }

        public void Restore() { stat = standart; }
        public void SetBasicValue(ValueType basicValue) { standart = basicValue; }

        public static implicit operator ValueType(BufebleUnit<ValueType> x)
        {
            return x.Value;
        }
    }
}
