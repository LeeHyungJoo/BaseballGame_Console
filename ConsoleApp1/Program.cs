using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Random rand = new Random();

                int answerLength = 0;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("=====Welcome To SBO Game ! =====");
                    Console.Write("Enter digit number (2 < n < 6) : ");
                    if (int.TryParse(Console.ReadLine(), out answerLength))
                    {
                        if (answerLength < 2 || answerLength > 5)
                        {
                            Console.WriteLine($"### 2 < n < 6 ! ###");
                            Thread.Sleep(2000);
                            continue;
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("### Plz Enter Digits! ###");
                        Thread.Sleep(2000);
                        continue;
                    }
                }

                
                Thread.Sleep(1000);
                for(int loadingBar = 0; loadingBar < 12; loadingBar++)
                {
                    string startmsg = string.Concat(Enumerable.Repeat(">>>", loadingBar + 1));
                    Console.Clear();
                    Console.WriteLine($"{startmsg}");
                    Console.WriteLine($"++ Ok, Starting Game with {answerLength} digits ++");
                    Console.WriteLine($"{startmsg}");
                    Thread.Sleep(rand.Next(50, 600));
                }

                Thread.Sleep(2000);
                List<int> answer = new List<int>(Enumerable.Repeat(0, answerLength));

                //중복 처리 검사
                {
                    HashSet<int> duplCheck = new HashSet<int>();
                    for (int i = 0; i < answer.Count; i++)
                    {
                        int candidate = rand.Next(0, 9);
                        while (duplCheck.AsQueryable().Where(c => c == candidate).Any())
                        {
                            candidate = rand.Next(0, 9);
                        }
                        duplCheck.Add(candidate);
                        answer[i] = candidate;
                    }
                }

                Dictionary<string, string> tryStatements = new Dictionary<string, string>();
                bool bCorrect = false;

                
                TimeSpan timelapse = new TimeSpan();
                DateTime starttime = DateTime.UtcNow;
                Console.Clear();
                Console.WriteLine("### if you want give up, write \"quit\" ###");
                Thread.Sleep(2000);
                while (true)
                {
                    Console.Clear();

                    if (tryStatements.Count != 0)
                    {
                        Console.WriteLine("===History======");
                        tryStatements.ToList().ForEach(map => Console.WriteLine($"{map.Key}\t{map.Value}"));
                        Console.WriteLine("================\n");
                    }

                    Console.Write("--> Try Answer : ");
                    string tryAnswer = Console.ReadLine();
                    if (tryAnswer == "quit")
                    {
                        bCorrect = false;
                        break;
                    }

                    if (tryAnswer.Length != answerLength || !Regex.IsMatch(tryAnswer, @"^[0-9]+$"))
                    {
                        Console.WriteLine($"### Should {answerLength} Digits! ###");
                        Thread.Sleep(2000);
                        continue;
                    }

                    if (tryStatements.ContainsKey(tryAnswer))
                    {
                        Console.WriteLine("### You Already Tried! ###");
                        Thread.Sleep(2000);
                        continue;
                    }

                    if (tryAnswer.AsQueryable().Distinct().Count() != answerLength)
                    {
                        Console.WriteLine("### Digits aren't Duplicated! ###");
                        Thread.Sleep(2000);
                        continue;
                    }

                    int s_num = 0;
                    int b_num = 0;
                    int o_num = 0;

                    for (int i = 0; i < answer.Count; i++)
                    {
                        char word = answer[i].ToString()[0];
                        if (word == tryAnswer[i])
                        {
                            s_num++;
                        }
                        else
                        {
                            if (tryAnswer.AsQueryable().Where(s => s == word).Count() != 0)
                            {
                                b_num++;
                            }
                            else
                            {
                                o_num++;
                            }
                        }
                    }

                    string result = "";
                    if (s_num != 0)
                    {
                        result += $"{s_num}S ";
                    }

                    if (b_num != 0)
                    {
                        result += $"{b_num}B ";
                    }

                    if (o_num != 0)
                    {
                        result += $"{o_num}O ";
                    }

                    tryStatements.Add(tryAnswer, result);
                    Thread.Sleep(1000);

                    if (s_num == answerLength)
                    {
                        timelapse = DateTime.UtcNow - starttime;
                        bCorrect = true;
                        break;
                    }
                }

                Console.Clear();

                if (bCorrect)
                {
                    Console.WriteLine("### Correct !  Congratulations !! ###\n");
                }

                Console.WriteLine("===History======");
                tryStatements.ToList().ForEach(map => Console.WriteLine($"{map.Key}\t{map.Value}"));
                Console.WriteLine("================\n");
                Console.WriteLine($"Total Try Count : {tryStatements.Count}");
                Console.WriteLine($"Timelapse: {timelapse.TotalSeconds} sec");
                Console.WriteLine($"Answer  [{string.Join("", answer)}] \n");

                Console.Write("--> Once a Again ? (Y/N) : ");
                string restartStr = Console.ReadLine();
                if(restartStr == "y" || restartStr == "Y")
                {
                    continue;
                }
                else
                {
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine("BYE !");
            Thread.Sleep(3000);
        }
    }
}
