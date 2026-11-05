using System;
using System.Diagnostics;

namespace Kolmas_kodutöö
{
    public abstract class BaseLogger
    {
        public abstract void Info(string message);
        public abstract void Error(string message);
        public abstract void Warning(string message);
        public abstract void DEBUG(string message);
    }

    public class DebugLogger : BaseLogger
    {
        public override void Info(string message)
        {
            Debug.WriteLine("INFO: " + message);
        }

        public override void Error(string message)
        {
            Debug.WriteLine("ERROR: " + message);
        }

        public override void Warning(string message)
        {
            Debug.WriteLine("WARNING: " + message);
        }

        public override void DEBUG(string message)
        {
            Debug.WriteLine("DEBUG: " + message);
        }
    }

    public class ConsoleLogger : BaseLogger
    {
        public override void Info(string message)
        {
            Console.WriteLine("INFO: " + message);
        }

        public override void Error(string message)
        {
            Console.WriteLine("ERROR: " + message);
        }

        public override void Warning(string message)
        {
            Console.WriteLine("WARNING: " + message);
        }

        public override void DEBUG(string message)
        {
            Console.WriteLine("DEBUG: " + message);
        }

    }

    public class NullLogger
    {
        public void WriteDebug(string message) { }
        public void WriteInfo(string message) { }
        public void WriteWarning(string message) { }
        public void WriteError(string message) { }
    }

    public class NullLoggerAdapter : BaseLogger
    {
        private readonly NullLogger _nullLogger;

        public NullLoggerAdapter(NullLogger nullLogger)
        {
            _nullLogger = nullLogger;
        }

        public override void DEBUG(string message)
        {
            _nullLogger.WriteDebug(message);
        }

        public override void Info(string message)
        {
            _nullLogger.WriteInfo(message);
        }

        public override void Warning(string message)
        {
            _nullLogger.WriteWarning(message);
        }

        public override void Error(string message)
        {
            _nullLogger.WriteError(message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var Loggerid = new BaseLogger[]
            {
                new NullLoggerAdapter(new NullLogger()),
                new ConsoleLogger(),
                new DebugLogger(),
            };

            foreach (BaseLogger logger in Loggerid)
            {
                Console.WriteLine(logger.ToString());

                Console.WriteLine();

                logger.Info("See on infoteade");
                logger.DEBUG("See on silumisteade");
                logger.Warning("See on hoiatus");
                logger.Error("See on veateade");

                Console.WriteLine();
            }

        }
    }


}