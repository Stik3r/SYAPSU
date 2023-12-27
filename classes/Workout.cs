using System;
using System.Collections.Generic;
using System.Linq;

namespace SYAPSU.classes
{
    class Workout
    {
        public bool begin = false;
        string[] startPos = { "С толчка", "С тумбы" };
        string[] noParams = { "Откупка длинная", "Откупка короткая", "Поплавок" };
        string[] withParams = { "Серия", "Отдых", "Звезда"};
        string loop = "Метод круговой тренировки";

        string[] parmDistance = { "Длинная вода", "Короткая вода" };
        string[] nameExercise = { "Вольный стиль", "На спине", "С доской", "Брасс", "Баттерфляй" };

        public bool ContainsLoop { get; set; }

        List<Operator> operators = new List<Operator>();

        public Workout()
        {
            ContainsLoop = false;
        }

        public bool AddOperator(string op)
        {
            if (!begin)
            {
                Operator @operator = CreateOperatorByName(op);
                operators.Insert(0, @operator);
                begin = true;
                Link.funcOff();
                return true;
            }
            else if(!startPos.Contains(op))
            {
                Operator @operator = CreateOperatorByName(op);
                operators.Add(@operator);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void RerangeOperator(int newIndex, int oldIndex)
        {
            Operator @operator = operators[newIndex];
            operators[newIndex] = operators[oldIndex];
            operators[oldIndex] = @operator;
        }

        public Operator CreateOperatorByName(string name)
        {
            if (noParams.Contains(name) || startPos.Contains(name))
            {
                return new Operator(name, 0);
            }
            else if(withParams.Contains(name))
            {
                switch (name)
                {
                    case "Серия":
                        var tmp = new Operator(name, 3, "Расстояние", "Упражнение", "Кол-во повторений");
                        tmp.Param[2].value = 1;
                        return tmp;
                    default: return new Operator(name, 1, "Кол-во секунд");
                }
            }
            else 
            {
                ContainsLoop = true;
                operators.Add(new Operator(name, 1, "Кол-во повторений"));
                return new Operator("Конец", 0);
            }
        }

        public bool CheckLoop(string name)
        {
            return name == loop ? true : false;
        }

        public string[] TextBoxParam()
        {
            return new string[] { withParams[1], withParams[2], loop };
        }

        public Operator GetOperatorById(int id)
        {
            return operators[id];
        }

        public void RemoveOperator(Operator @operator)
        {
            if(startPos.Contains(@operator.Name))
            {
                begin = false;
            }
            operators.Remove(@operator);
        }

        public string[] GetStartPos()
        {
            return startPos;
        }

        public List<Operator> GetOperators() { return operators; }
        public void SetOperators(List<Operator> value) { operators = value; }

        public void ClearAll()
        {
            begin = false;
            ContainsLoop = false;
            operators.Clear();
        }
        
        public void SetOperatorParams(int indx, (string, int)[] param)
        {
            operators[indx].Param = param;
        }

        public void CheckOperators()
        {
            foreach(Operator op in operators)
            {
                if (startPos.Contains(op.Name))
                {
                    begin = true;
                    Link.EnableOperators();
                    Link.funcOff();
                }
                else if(op.Name == loop)
                {
                    ContainsLoop = true;
                }
            }
        }

        public string DistanceById(int id)
        {
            return parmDistance[id];
        }

        public string ExerciseById(int id)
        {
            return nameExercise[id];
        }
    }
}
