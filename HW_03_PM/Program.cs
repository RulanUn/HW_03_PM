using System;
using System.Threading;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Exam_SP
{
    class Program
    {
        private static string message;
        private static int amountSimbols = 0;
        private static int countWords = 0;
        private static int amountSentence = 0;
        private static int amountQuestionSentence = 0;
        private static int amountExclamationSentence = 0;
        private static double sum = 0.0;


        static void Main(string[] args)
        {
            Thread getText = new Thread(GetText);
            getText.Start();
            getText.Join();

            Thread countSimbols = new Thread(CountSimbols);
            countSimbols.Start();

            Thread countWords = new Thread(CountWords);
            countWords.Start();

            Thread countSentence = new Thread(CountSentence);
            countSentence.Start();

            Thread countQuestionSentence = new Thread(CountQuestionSentence);
            countQuestionSentence.Start();

            Thread countExclamationSentence = new Thread(CountExclamationSentence);
            countExclamationSentence.Start();

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-us");
            Thread countNumbersInSentence = new Thread(CountNumbersInSentence);
            countNumbersInSentence.Start();
            countNumbersInSentence.Join();

            Console.WriteLine("\n1. вывети отчет на экран" +
                "\n2. сохранить отчет в файл\n");
            int userChoice = int.Parse(Console.ReadLine());
            if (userChoice == 1)
            {
                Thread printData = new Thread(PrintData);
                printData.Start();
            }
            else if (userChoice == 2)
            {
                Thread writeDataInFile = new Thread(WriteDataInFile);
                writeDataInFile.Start();
                writeDataInFile.Join();

                Thread openFile = new Thread(OpenFile);
                openFile.Start();
            }
        }

        private static void GetText()
        {
            Console.WriteLine("Введит текст:");
            message = Console.ReadLine();
        }

        private static void CountSimbols()
        {
            foreach (char ch in message)
            {
                if (ch != ' ')
                {
                    ++amountSimbols;
                }
            }
        }

        private static void CountWords()
        {
            bool prev = true;
            foreach (char ch in message)
            {
                bool cur = Char.IsWhiteSpace(ch);
                if (prev && !cur) ++countWords;
                prev = cur;
            }
        }

        private static void CountSentence()
        {
            foreach (char ch in message)
            {
                if (ch == '.' || ch == '?' || ch == '!')
                {
                    ++amountSentence;
                }
            }
        }

        private static void CountQuestionSentence()
        {
            foreach (char ch in message)
            {
                if (ch == '?')
                {
                    ++amountQuestionSentence;
                }
            }
        }

        private static void CountExclamationSentence()
        {
            foreach (char ch in message)
            {
                if (ch == '!')
                {
                    ++amountExclamationSentence;
                }
            }
        }

        private static void CountNumbersInSentence()
        {
            var matches = Regex.Matches(message, @"[-+]?\d+(?:\.\d+)?(?:[eE][-+]?\d+)?");
            foreach (Match item in matches)
            {
                sum += double.Parse(item.Value);
            }
        }

        private static void WriteDataInFile()
        {
            string pathFile = @"C:\EXAM_SP\Text.txt";
            string result = "Количество символов в тексте: "+amountSimbols +
                                    "\nКоличество слов в тексте: " + countWords +
                                    "\nКоличество предложений в тексте: "+ amountSentence +
                                    "\nКоличество вопросительных предложений в тексте: " + amountQuestionSentence +
                                    "\nКоличество восклицательных предложений в тексте: " + amountExclamationSentence +
                                    "\nСумма чисел в тексте: " + sum;
            using (StreamWriter sw = new StreamWriter(pathFile, false))
            {
                for (int i = 0; i < message.Length; i++)
                {
                    sw.Write(message[i]);
                }
                sw.WriteLine("\n"+result);
            }
        }

        private static void PrintData()
        {
            Console.WriteLine($"Количество символов в тексте: {amountSimbols}");
            Console.WriteLine($"Количество слов в тексте: {countWords}");
            Console.WriteLine($"Количество предложений в тексте: {amountSentence}");
            Console.WriteLine($"Количество вопросительных предложений в тексте: {amountQuestionSentence}");
            Console.WriteLine($"Количество восклицательных предложений в тексте: {amountExclamationSentence}");
            Console.WriteLine($"Сумма чисел в тексте: {sum}");
        }

        private static void OpenFile()
        {
            Process.Start("notepad.exe", @"C:\EXAM_SP\Text.txt");
        }
    }
}