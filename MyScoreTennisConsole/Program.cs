using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScoreTennisConsole
{
    class Program
    {
        static public void SupportParserScore()
        {
            var Bot = new MyScoreTennisEntity.Bot.BotParserScore();
            Bot.Start();
        }
        static void Main(string[] args)
        {
            try
            {
                SupportParserScore();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
