using SYAPSU.User_controls;
using System.Collections.Generic;
using System.Windows.Controls;
using NLog;
using NLog.Config;
using System.IO;
using Microsoft.VisualBasic;

namespace SYAPSU.classes
{
    public enum LogLvl
    {
        info,
        warn,
        error
    }
    public static class Link
    {
        public static ListBox listBoxOperators, listBoxCommands;
        public static listCommands listCommands;
        public static Operators operators;
        public static OperatorConfiguration operatorConfiguration;
        public static ImageOperator imageOperator;

        static Workout workout = new Workout();
        static Serializer serializer = new Serializer();
        static Logger logger = LogManager.GetCurrentClassLogger();
        static LoggingConfiguration config = new NLog.Config.LoggingConfiguration();

        public delegate void FuncOff();
        public static FuncOff funcOff;

        static Link()
        {
            var fileTarget = new NLog.Targets.FileTarget("fileTarget") { FileName = "log " + DateAndTime.Now.ToString() + ".txt" };
            config.AddTarget(fileTarget);
            config.AddRule(LogLevel.Info, LogLevel.Fatal, fileTarget);
            NLog.LogManager.Configuration = config;
        }
        public static void EnableOperators()
        {
            operators.EnableOperators();
        }

        public static void DisableOperators()
        {
            operators.DisableOperators();
        }

        public static bool Add(string op)
        {
            if(!workout.begin)
            {
                EnableOperators();
                workout.AddOperator(op);
                return true;
            }
            else
            {
                return workout.AddOperator(op);
            }
        }

        public static void RerangeOperator(int newIndex, int oldIndex)
        {
            workout.RerangeOperator(newIndex, oldIndex);
        }

        public static void AddElement(string element)
        {
            listCommands.AddElement(element);
        }

        public static bool CheckLoop(string name)
        {
            return workout.CheckLoop(name);
        }

        public static bool ContainsLoop
        {
            get { return workout.ContainsLoop; }
            set { workout.ContainsLoop = value; }
        }

        public static string[] GetTextBoxParam()
        {
            return workout.TextBoxParam();
        }

        public static void ChangeConfiguration(Operator @operator)
        {
            operatorConfiguration.CreateComnfiguration(@operator);
        }

        public static Operator GetOperatorById(int id)
        {
            return workout.GetOperatorById(id);
        }

        public static void SetImage(string name)
        {
            imageOperator.SetImage(name);
        }

        public static void ClearImage()
        {
            imageOperator.ClearImage();
        }

        public static void RemoveOperator(Operator @operator)
        {
            workout.RemoveOperator(@operator);
        }

        public static void CallRemoveOperator()
        {
            listCommands.RemoveCommand();
        }

        public static bool GetBegin()
        {
            return workout.begin;
        }

        public static string[] GetStartPos()
        {
            return workout.GetStartPos();
        }

        public static List<Operator> GetOperators()
        {
            return workout.GetOperators();
        }

        public static void SetOperators(List<Operator> value)
        {
            workout.SetOperators(value);
        }

        public static void SaveData()
        {
            serializer.SerializeData(GetOperators());
        }

        public static List<Operator> LoadData()
        {
            return serializer.DeserializeData();
        }

        public static void ClearAll()
        {
            listCommands.ClearAll();
            workout.ClearAll();
            operatorConfiguration.ClearAll();
            imageOperator.ClearImage();
            DisableOperators();
        }

        public static void SetOperatorParams(int indx, (string, int)[] param)
        {
            workout.SetOperatorParams(indx, param);
        }

        public static void CheckOperators()
        {
            workout.CheckOperators();
        }

        public static void AddCommand(string command)
        {
            listCommands.AddCommand(command);
        }

        public static string DistanceById(int id)
        {
            return workout.DistanceById(id);
        }

        public static string ExerciseById(int id)
        {
            return workout.ExerciseById(id);
        }

        public static void Logging(string info, LogLvl lvl)
        {
            switch (lvl)
            {
                case LogLvl.info:
                    logger.Info(info); break;
                case LogLvl.warn:
                    logger.Warn(info); break;
                case LogLvl.error:
                    logger.Error(info); break;
            }
        }
    }
}
