using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.OverWitch.QianHan.Log.io;

namespace Assets.OverWitch.QianHan.Log.lang
{
    public class StackTraceElement:Serializable
    {
        private string declaringClass;
        private string methodName;
        private string fileName;
        private int lineNumber;

        public StackTraceElement(String declaringClass, String methodName,
                             String fileName, int lineNumber)
        {
            this.declaringClass = declaringClass;
            this.methodName = methodName;
            this.fileName = fileName;
            this.lineNumber = lineNumber;
        }
        public string getFileName() { return fileName; }
        public int getLineNumber() { return lineNumber; }
        public string getClassName() { return declaringClass; }
        public string getMethodName() { return methodName; }
        public bool isNativeMethod() { return lineNumber == -2; }
        public string toString()
        {
            return getClassName() + "." + methodName + (isNativeMethod() ? "(Native Method)" : (fileName != null && lineNumber >= 0
                ? "(" + fileName + "." + lineNumber + ")" : (fileName != null ? "(" + fileName + ")" : "(Unknown Source)")));
        }

    }
}
