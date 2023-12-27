using System;

namespace SYAPSU.classes
{
    [Serializable]
    public class Operator
    {
        int countParams;

        public Operator()
        {
            Name = "";
            Param = new (string name, int value)[0];
        }
        public Operator(string name, int countParams, params string[] pNames)
        {
            Name = name;
            this.countParams = countParams;
            Param = new (string name, int value)[countParams];
            if (countParams > 0)
            {
                for(int  i = 0; i < countParams; i++)
                {
                    Param[i].name = pNames[i];
                }
            }
        }

        public Operator(Operator op)
        {
            this.Name = op.Name;
            this.countParams = op.countParams;
            this.Param = op.Param;
        }

        public string Name 
        {
            get;
            set;
        }

        public (string name, int value)[] Param
        {
            get;
            set;
        }
        public override string ToString()
        {
            return Name;
        }
    }

   
}
