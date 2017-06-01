using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Log("Start");

            //ParallelPlay();

            //CancellationToken();

            //ConcurentFileAppend();

            MoveData();

            Log("Stop");
            Console.ReadKey();

        }

        private static void MoveData()
        {
            var p = new Person
            {
                Id = 1,
                Name = "Pippo",
                LastName = "Pluto",
                Age = 22
            };

            StampaOggetto(p);



            var anagr = new AnagrDto();
            anagr.Id = p.Id;
            anagr.Name = p.Name;
            anagr.LastName = p.LastName;
            anagr.Age = p.Age;

            ClonaDati(p, anagr);


            var dto = new PersonDto();
            dto.Name = p.Name;
            dto.LastName = p.LastName;
            dto.Age = p.Age;


            
            StampaOggetto(dto);
        }

        private static void ClonaDati(object source, object destination)
        {
            // copia i valore delle proprietà dell'oggetto source sul 
            // quelle con lo stesso nome del destination
        }

        private static void StampaOggetto(object obj)
        {
            // stampi le proprieta e i valori delle proprietà dell'oggetto
        }

        private static void ConcurentFileAppend()
        {
            var file = "out.txt";
            var baseText = "lasdkjflasjdfhlasjhdflaksdjhf";

            var records = Enumerable.Range(1, 100);

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 20
            };

            System.IO.File.Delete(file);

            try
            {
                Parallel.ForEach(records, parallelOptions, rec =>
                {
                    System.IO.File.AppendAllText(file, rec + baseText + Environment.NewLine);
                });
            }catch(AggregateException ex)
            {
                Log(ex.ToString());
            }
        }

        private static void CancellationToken()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Log(" ....");
                    Thread.Sleep(2000);
                    if (token.IsCancellationRequested)
                    {
                        Log("closing...");
                        token.ThrowIfCancellationRequested();
                        return;
                    }
                }

            }, token);

            Log("Waiting for task end");
            task.Wait(5000);

            if (!task.IsCompleted)
            {
                Log("!!!! task still running...");
                tokenSource.Cancel();
                task.Wait();
                Log("task closed");

            }
        }

        private static void ParallelPlay()
        {
            int count = 0;
            var records = Enumerable.Range(1, 100);



            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 20
            };

            //Parallel.ForEach(records, parallelOptions, rec =>
            //{
            //    Interlocked.Increment(ref count);
            //    var id = Thread.CurrentThread.ManagedThreadId;
            //    Log(id + "] processing " + rec + "   threads: " + count);
            //    Thread.Sleep(300 +  new Random().Next(4000));
            //    Log(id + "] completed " + rec);
            //    Interlocked.Decrement(ref count);
            //});


            List<int> numbers = new List<int>();
            for (int i = 0; i < 100000; i++) numbers.Add(5);

            var total = 0;

            foreach (var num in numbers) total += num;

            Log("total foreach: " + total);

            var total1 = 0;
            Parallel.ForEach(numbers, parallelOptions, num =>
            {
                // problema di concorrenza
                total1 += num;
            });

            Log("total parallel (non sync): " + total1);

            var lck = new object();
            var total2 = 0;
            Parallel.ForEach(numbers, parallelOptions, num =>
            {
                lock (lck)
                {
                    total2 += num;
                }
            });

            Log("total parallel (lock): " + total2);

            var total3 = 0;
            Parallel.ForEach(numbers, parallelOptions, num =>
            {
                Interlocked.Add(ref total3, num);
            });

            Log("total parallel (interlocked): " + total3);
        }

        private static void Log(string v)
        {
            Console.WriteLine(v);
        }
    }
}
