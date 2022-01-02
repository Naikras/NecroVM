//==========AsukaSoftware2022==========//
//Документацию читай в папке дистрибутива
//Может пригодится

using System;
using System.Threading;
using System.IO;
using System.Linq;

namespace NecroVM
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.SetCursorPosition(0, 9); 
            Program program = new Program();
            Processor proc = new Processor();
            proc.tabPrinter("");
            Thread processorCoreThread = new Thread(new ThreadStart(proc.Core));
            processorCoreThread.Start();
        }


        public class Processor
        {
            //====Регистры====//
            public object DRON;
            public object DRTW;
            public object DRTH;
            public object DRFO;
            public object fuckThisRegister;
            public uint IR = 0;
            public uint RRP;
            //====Флаги====//
            public bool RF;

            private int holding = 501; //Задержка, лучше убрать в 0, но так неинтересно

            public void Core()
            {
                Console.WriteLine("Kernel initialized");
                while(true) //Цикл ядра процессора
                {
                    string instruction = read(); //Получаем инструкцию из метода read
                    if (instruction == ".") { break; } //Если точка, то программа закончилась
                    else if (instruction == "mov") mov(); //Если mov, то в метод mov
                    else if (instruction == "jmp") jump(); //Если jmp, то в метод jump
                    else //Елси команда неизвестна, выкидываем ошибку и останавливаем ядро 
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine("Unknown instruction");
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                    }
                    
                }
            }

            string read() //Считываем строку по IR регистру 
            {
                string stringFromFile = File.ReadLines("opcode.txt").Skip(Convert.ToInt32(IR)).First();
                IR++; 
                tabPrinter(stringFromFile); //Печатаем в табличку всю инфу 
                Thread.Sleep(holding); //Задержка 
                #region Отчет
                using (StreamWriter sw = new StreamWriter("traceback.txt", true, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine($"DRON - {DRON}");
                    sw.WriteLine($"DRTW - {DRTW}");
                    sw.WriteLine($"DRTH - {DRTH}");
                    sw.WriteLine($"DRFO - {DRFO}");
                    sw.WriteLine($" IR  -  {IR} ");
                    sw.WriteLine("==============");
                }
                #endregion 
                return stringFromFile; 
            }

            void jump()
            {
                //Устанавливаем значение регистра инструкции в чило, указанное после инструкции в программе 
                IR = Convert.ToUInt32(read());
            }
            void mov()
            {
                string register = read();
                if(register == "DRON") { DRON = read(); }
                else if(register == "DRTW") { DRTW = read(); }
                else if (register == "DRTH") { DRTH = read(); }
                else if (register == "DRFO") { DRFO = read(); }

            }

            //======Выводим данные в табличку============
            public void tabPrinter(string instruction)
            {
                int oldLeftCursorPos = Console.CursorLeft;
                int oldTopCursorPos = Console.CursorTop;
                Console.CursorVisible = false;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("=======necro-VM=======");
                Console.WriteLine("=DRON -              =");
                Console.WriteLine("=DRTW -              =");
                Console.WriteLine("=DRTH -              =");
                Console.WriteLine("=DRFO -              =");
                Console.WriteLine("= IR  -              =");
                Console.WriteLine("=                    =");
                Console.WriteLine("======================");
                Console.WriteLine("");
                Console.SetCursorPosition(8, 1);
                Console.Write(DRON);
                Console.SetCursorPosition(8, 2);
                Console.Write(DRTW);
                Console.SetCursorPosition(8, 3);
                Console.Write(DRTH);
                Console.SetCursorPosition(8, 4);
                Console.Write(DRFO);
                Console.SetCursorPosition(8, 5);
                Console.Write(IR);
                Console.SetCursorPosition(0, 8);
                Console.Write(instruction + "                             ");
                Console.SetCursorPosition(oldLeftCursorPos, oldTopCursorPos);
                Console.CursorVisible = true;
            }

        }

        
    } 
}
