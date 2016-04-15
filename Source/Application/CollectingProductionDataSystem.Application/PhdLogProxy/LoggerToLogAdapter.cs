using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Infrastructure.Contracts;
using log4net;
using log4net.Core;

namespace CollectingProductionDataSystem.Application.PhdLogProxy
{
    public class LoggerToLogAdapter : ILog
    {

        private readonly CollectingProductionDataSystem.Infrastructure.Contracts.ILogger logger;
        private readonly IProgressRegistrator progressRegistrator;

        public LoggerToLogAdapter(CollectingProductionDataSystem.Infrastructure.Contracts.ILogger loggerParam, IProgressRegistrator progressRegistratorParam)
        {
            this.logger = loggerParam;
            this.progressRegistrator = progressRegistratorParam;
        }

        #region Debug
        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Debug" /> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Debug" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>DEBUG</c>
        /// enabled by comparing the level of this logger with the
        /// <see cref="F:log4net.Core.Level.Debug" /> level. If this logger is
        /// <c>DEBUG</c> enabled, then it converts the message object
        /// (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        /// proceeds to call all the registered appenders in this logger
        /// and also higher in the hierarchy depending on the value of
        /// the additivity flag.
        /// </para>
        /// <para><b>WARNING</b> Note that passing an <see cref="T:System.Exception" />
        /// to this method will print the name of the <see cref="T:System.Exception" />
        /// but no stack trace. To print a stack trace use the
        /// <see cref="M:Debug(object,Exception)" /> form instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        public void Debug(object message)
        {

        }

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Debug" /> level
        /// including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// See the &lt;see cref="M:Debug(object)" /&gt; form for more detailed information.
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        public void Debug(object message, Exception exception)
        {

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted string with the <see cref="F:log4net.Core.Level.Debug" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        public void DebugFormat(string format, params object[] args)
        {

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        public void DebugFormat(string format, object arg0)
        {

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        public void DebugFormat(string format, object arg0, object arg1)
        {
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Debug" />
        /// level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies
        /// culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Debug(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
        }
        #endregion

        #region Info
        /// <summary>
        /// Logs a message object with the <see cref="F:log4net.Core.Level.Info" /> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Info" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>INFO</c>
        /// enabled by comparing the level of this logger with the
        /// <see cref="F:log4net.Core.Level.Info" /> level. If this logger is
        /// <c>INFO</c> enabled, then it converts the message object
        /// (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        /// proceeds to call all the registered appenders in this logger
        /// and also higher in the hierarchy depending on the value of the
        /// additivity flag.
        /// </para>
        /// <para><b>WARNING</b> Note that passing an <see cref="T:System.Exception" />
        /// to this method will print the name of the <see cref="T:System.Exception" />
        /// but no stack trace. To print a stack trace use the
        /// <see cref="M:Info(object,Exception)" /> form instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        public void Info(object message)
        {
            if (message is IProgressMessage)
            {
                this.progressRegistrator.RegisterProgress((message as IProgressMessage).ProgressValue);
            }
            this.logger.Info(message.ToString(), this);

        }

        /// <summary>
        /// Logs a message object with the <c>INFO</c> level including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// See the &lt;see cref="M:Info(object)" /&gt; form for more detailed information.
        /// </remarks>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        public void Info(object message, Exception exception)
        {
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Info" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        public void InfoFormat(string format, params object[] args)
        {
            var arguments = new List<object>();

            foreach (var param in args)
            {
                if (param is IProgressMessage)
                {
                    this.progressRegistrator.RegisterProgress((param as IProgressMessage).ProgressValue);
                }
                else
                {
                    arguments.Add(param);
                }

            }

            this.Info(string.Format(format,arguments.ToArray()));
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        public void InfoFormat(string format, object arg0)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        public void InfoFormat(string format, object arg0, object arg1)
        {
            var args = new object[] { arg0, arg1 };
            InfoFormat(format, args);

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Info" />
        /// level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies
        /// culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Info(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Info(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsInfoEnabled" />
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
        #endregion

        #region Warning

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Warn" /> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Warn" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>WARN</c>
        /// enabled by comparing the level of this logger with the
        /// <see cref="F:log4net.Core.Level.Warn" /> level. If this logger is
        /// <c>WARN</c> enabled, then it converts the message object
        /// (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        /// proceeds to call all the registered appenders in this logger
        /// and also higher in the hierarchy depending on the value of the
        /// additivity flag.
        /// </para>
        /// <para><b>WARNING</b> Note that passing an <see cref="T:System.Exception" />
        /// to this method will print the name of the <see cref="T:System.Exception" />
        /// but no stack trace. To print a stack trace use the
        /// <see cref="M:Warn(object,Exception)" /> form instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        public void Warn(object message)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Warn" /> level including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// See the &lt;see cref="M:Warn(object)" /&gt; form for more detailed information.
        /// </remarks>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        public void Warn(object message, Exception exception)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Warn" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        public void WarnFormat(string format, params object[] args)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        public void WarnFormat(string format, object arg0)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        public void WarnFormat(string format, object arg0, object arg1)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Warn" />
        /// level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies
        /// culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Warn(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Warn(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsWarnEnabled" />
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }
        #endregion

        #region Error

        /// <summary>
        /// Logs a message object with the <see cref="F:log4net.Core.Level.Error" /> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Error" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>ERROR</c>
        /// enabled by comparing the level of this logger with the
        /// <see cref="F:log4net.Core.Level.Error" /> level. If this logger is
        /// <c>ERROR</c> enabled, then it converts the message object
        /// (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        /// proceeds to call all the registered appenders in this logger
        /// and also higher in the hierarchy depending on the value of the
        /// additivity flag.
        /// </para>
        /// <para><b>WARNING</b> Note that passing an <see cref="T:System.Exception" />
        /// to this method will print the name of the <see cref="T:System.Exception" />
        /// but no stack trace. To print a stack trace use the
        /// <see cref="M:Error(object,Exception)" /> form instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        public void Error(object message)
        {
            this.logger.Error(message.ToString(), this, new Exception());
        }

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Error" /> level
        /// including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// See the &lt;see cref="M:Error(object)" /&gt; form for more detailed information.
        /// </remarks>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        public void Error(object message, Exception exception)
        {
            this.logger.Error(message.ToString(), exception.Source, exception);
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Error" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        public void ErrorFormat(string format, params object[] args)
        {

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        public void ErrorFormat(string format, object arg0)
        {

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        public void ErrorFormat(string format, object arg0, object arg1)
        {

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {

        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Error" />
        /// level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies
        /// culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Error(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Error(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsErrorEnabled" />
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {

        }

        #endregion

        #region FatalError

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Fatal" /> level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <overloads>Log a message object with the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// This method first checks if this logger is <c>FATAL</c>
        /// enabled by comparing the level of this logger with the
        /// <see cref="F:log4net.Core.Level.Fatal" /> level. If this logger is
        /// <c>FATAL</c> enabled, then it converts the message object
        /// (passed as parameter) to a string by invoking the appropriate
        /// <see cref="T:log4net.ObjectRenderer.IObjectRenderer" />. It then
        /// proceeds to call all the registered appenders in this logger
        /// and also higher in the hierarchy depending on the value of the
        /// additivity flag.
        /// </para>
        /// <para><b>WARNING</b> Note that passing an <see cref="T:System.Exception" />
        /// to this method will print the name of the <see cref="T:System.Exception" />
        /// but no stack trace. To print a stack trace use the
        /// <see cref="M:Fatal(object,Exception)" /> form instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void Fatal(object message)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Log a message object with the <see cref="F:log4net.Core.Level.Fatal" /> level
        /// including
        /// the stack trace of the <see cref="T:System.Exception" /> passed
        /// as a parameter.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        /// <param name="exception">The exception to log, including its stack trace.</param>
        /// <remarks>
        /// See the &lt;see cref="M:Fatal(object)" /&gt; form for more detailed information.
        /// </remarks>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void Fatal(object message, Exception exception)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <overloads>Log a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.</overloads>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void FatalFormat(string format, params object[] args)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void FatalFormat(string format, object arg0)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void FatalFormat(string format, object arg0, object arg1)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.
        /// </summary>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="arg0">An Object to format</param>
        /// <param name="arg1">An Object to format</param>
        /// <param name="arg2">An Object to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object,Exception)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logs a formatted message string with the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.
        /// </summary>
        /// <param name="provider">An <see cref="T:System.IFormatProvider" /> that supplies
        /// culture-specific formatting information</param>
        /// <param name="format">A String containing zero or more format items</param>
        /// <param name="args">An Object array containing zero or more objects to format</param>
        /// <remarks>
        /// <para>
        /// The message is formatted using the <c>String.Format</c> method. See
        /// <see cref="M:String.Format(string, object[])" /> for details of the syntax of
        /// the format string and the behavior
        /// of the formatting.
        /// </para>
        /// <para>
        /// This method does not take an <see cref="T:System.Exception" /> object to include
        /// in the
        /// log event. To pass an <see cref="T:System.Exception" /> use one of the <see cref="M:Fatal(object)" />
        /// methods instead.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Fatal(object,Exception)" />
        /// <seealso cref="P:log4net.ILog.IsFatalEnabled" />
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        #endregion


        /// <summary>
        /// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Debug" />
        /// level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This function is intended to lessen the computational cost of
        /// disabled log debug statements.
        /// </para>
        /// <para> For some ILog interface <c>log</c>, when you write:</para>
        /// <code lang="C#">
        /// log.Debug("This is entry number: " + i );
        /// </code>
        /// <para>
        /// You incur the cost constructing the message, string construction and concatenation
        /// in
        /// this case, regardless of whether the message is logged or not.
        /// </para>
        /// <para>
        /// If you are worried about speed (who isn't), then you should write:
        /// </para>
        /// <code lang="C#">
        /// if (log.IsDebugEnabled)
        /// {
        /// log.Debug("This is entry number: " + i );
        /// }
        /// </code>
        /// <para>
        /// This way you will not incur the cost of parameter
        /// construction if debugging is disabled for <c>log</c>. On
        /// the other hand, if the <c>log</c> is debug enabled, you
        /// will incur the cost of evaluating whether the logger is debug
        /// enabled twice. Once in <see cref="P:log4net.ILog.IsDebugEnabled" /> and once in
        /// the <see cref="M:Debug(object)" />.  This is an insignificant overhead
        /// since evaluating a logger takes about 1% of the time it
        /// takes to actually log. This is the preferred style of logging.
        /// </para>
        /// <para>Alternatively if your logger is available statically then the is debug
        /// enabled state can be stored in a static variable like this:
        /// </para>
        /// <code lang="C#">
        /// private static readonly bool isDebugEnabled = log.IsDebugEnabled;
        /// </code>
        /// <para>
        /// Then when you come to log you can write:
        /// </para>
        /// <code lang="C#">
        /// if (isDebugEnabled)
        /// {
        /// log.Debug("This is entry number: " + i );
        /// }
        /// </code>
        /// <para>
        /// This way the debug enabled state is only queried once
        /// when the class is loaded. Using a <c>private static readonly</c>
        /// variable is the most efficient because it is a run time constant
        /// and can be heavily optimized by the JIT compiler.
        /// </para>
        /// <para>
        /// Of course if you use a static readonly variable to
        /// hold the enabled state of the logger then you cannot
        /// change the enabled state at runtime to vary the logging
        /// that is produced. You have to decide if you need absolute
        /// speed or runtime flexibility.
        /// </para>
        /// </remarks>
        /// <seealso cref="M:Debug(object)" />
        /// <seealso cref="M:DebugFormat(IFormatProvider, string, object[])" />
        /// <value>
        /// <c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Debug" />
        /// events, <c>false</c> otherwise.
        /// </value>
        public bool IsDebugEnabled { get; private set; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Fatal" />
        /// level.
        /// </summary>
        /// <remarks>
        /// For more information see <see cref="P:log4net.ILog.IsDebugEnabled" />.
        /// </remarks>
        /// <seealso cref="M:Fatal(object)" />
        /// <seealso cref="M:FatalFormat(IFormatProvider, string, object[])" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <value>
        /// <c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Fatal" />
        /// events, <c>false</c> otherwise.
        /// </value>
        public bool IsFatalEnabled { get; private set; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Info" />
        /// level.
        /// </summary>
        /// <remarks>
        /// For more information see <see cref="P:log4net.ILog.IsDebugEnabled" />.
        /// </remarks>
        /// <seealso cref="M:Info(object)" />
        /// <seealso cref="M:InfoFormat(IFormatProvider, string, object[])" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <value>
        /// <c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Info" />
        /// events, <c>false</c> otherwise.
        /// </value>
        public bool IsInfoEnabled { get; private set; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Warn" />
        /// level.
        /// </summary>
        /// <remarks>
        /// For more information see <see cref="P:log4net.ILog.IsDebugEnabled" />.
        /// </remarks>
        /// <seealso cref="M:Warn(object)" />
        /// <seealso cref="M:WarnFormat(IFormatProvider, string, object[])" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <value>
        /// <c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Warn" />
        /// events, <c>false</c> otherwise.
        /// </value>
        public bool IsWarnEnabled { get; private set; }

        /// <summary>
        /// Checks if this logger is enabled for the <see cref="F:log4net.Core.Level.Error" />
        /// level.
        /// </summary>
        /// <remarks>
        /// For more information see <see cref="P:log4net.ILog.IsDebugEnabled" />.
        /// </remarks>
        /// <seealso cref="M:Error(object)" />
        /// <seealso cref="M:ErrorFormat(IFormatProvider, string, object[])" />
        /// <seealso cref="P:log4net.ILog.IsDebugEnabled" />
        /// <value>
        /// <c>true</c> if this logger is enabled for <see cref="F:log4net.Core.Level.Error" />
        /// events, <c>false</c> otherwise.
        /// </value>
        public bool IsErrorEnabled { get; private set; }

        /// <summary>
        /// Get the implementation behind this wrapper object.
        /// </summary>
        /// <remarks>
        /// The &lt;see cref="T:log4net.Core.ILogger" /&gt; object that in implementing this
        /// object. The &lt;c&gt;Logger&lt;/c&gt; object may not
        /// be the same object as this object because of logger decorators.
        /// This gets the actual underlying objects that is used to process
        /// the log events.
        /// </remarks>
        /// <value>
        /// The <see cref="T:log4net.Core.ILogger" /> object that in implementing this object.
        /// </value>
        public log4net.Core.ILogger Logger { get; private set; }
    }
}
