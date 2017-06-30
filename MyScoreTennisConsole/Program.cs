using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyScoreTennisConsole
{

    public class PriceComparator
    {
        private PriceComparator instance;

        public PriceComparator()
        {
            instance = this;
        }

        public PriceComparator Instance
        {
            get
            {
                return instance;
            }
        }

    }

    public class Shape
    {
        public string GetName() { return "shape"; }
    }

    public class Ball: Shape
    {
        public new string GetName() { return "ball"; }
    }

    public class Pet
    {
        public virtual string GetName() { return "pet"; }
    }

    public class Cat : Pet
    {
        public override string GetName() { return "cat"; }
    }
    class Program
    {
        private static string result;
 
 
  static async Task SaySomething() {
    await Task.Delay(5);
      // Thread.Sleep(5);
    result = "Hello world!";
   // return "Something";
  }

        static String location;
        static DateTime time;
        static public void SupportParserScore()
        {
            var Bot = new MyScoreTennisEntity.Bot.BotParserScore();
            Bot.Start();
        }
        static void Main(string[] args)
        {
            SaySomething();
            Console.WriteLine(result);

            Pet myPet = new Cat();
            Cat jonsCat = new Cat();
            Shape shape = new Ball();

            Console.WriteLine(string.Format("{0} {1} {2}", myPet.GetName(), shape.GetName(), jonsCat.GetName()));
            try
            {
                Console.WriteLine(location == null ? "location is null" : location);
                Console.WriteLine(time == null ? "time is null" : time.ToString());
                //SupportParserScore();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}
