using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UserStorage.Entities;
using UserStorage.Factory;
using UserStorage.Infrastructure;
using UserStorage.Replication;
using UserStorage.Replication.Events;
using UserStorage.Services;
using UserStorage.Storages;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //WitoutDomains();
            //Domains();
            Threads();

            //Console.WriteLine(typeof(XmlUserRepository).Name);
        }

        private static void WitoutDomains()
        {
            var master = new Storage(new XmlUserRepository(@"d:\Projects\EPAM.RD.2016S.Larkovich\Day1\UserStorage\bin\Debug\UserDataBase.xml"), 
                new ValidatorUsers(), new GeneratorIds(), new MessageSender());
            var slave = new SlaveStorage(new MemoryUserRepository());

            var id = master.Add(new User
            {
                FirstName = "Vanja",
                LastName = "Petrov",
                PersonalId = "AB123",
                Visa = new VisaRecord { Country = "Belarus" }
            });

            var findResult = slave.SearchByName("Vanja", "Petrov").FirstOrDefault();

            Console.WriteLine(id);
            Console.WriteLine(findResult);
        }

        private static void Domains()
        {
            var master = UserStorageFactory.GetMaster;
            var slave1 = UserStorageFactory.GetSlave;
            var slave2 = UserStorageFactory.GetSlave;

            var id = master.Add(new User
            {
                FirstName = "Stas",
                LastName = "Petrov",
                PersonalId = "AB123",
                Visa = new VisaRecord { Country = "Belarus" }
            });

            var findResult1 = slave1.SearchByName("Stas", "Petrov").FirstOrDefault();
            var findResult2 = slave2.SearchByName("Stas", "Petrov").FirstOrDefault();

            Console.WriteLine(id);
            Console.WriteLine(findResult1);
            Console.WriteLine(findResult2);
        }

        private static IList<Thread> CreateWorkers(ManualResetEventSlim mres, Action action, int threadsNum, int cycles)
        {
            var threads = new Thread[threadsNum];

            for (int i = 0; i < threadsNum; i++)
            {
                Action d = () =>
                {
                    mres.Wait();

                    for (int j = 0; j < cycles; j++)
                    {
                        action();
                    }
                };

                Thread thread = new Thread(new ThreadStart(d));

                threads[i] = thread;
            }

            return threads;
        }

        private static void Threads()
        {
            IUserStorage master = UserStorageFactory.GetMaster;
            IUserStorage slave = UserStorageFactory.GetSlave;

            var mres = new ManualResetEventSlim();

            var threads = new List<Thread>();

            threads.AddRange(CreateWorkers(mres, () =>
            {
                var firstName = "first" + DateTime.Now.Ticks;
                master.Add(new User
                {
                    FirstName = firstName,
                    LastName = "Ivanovich",
                    PersonalId = "Pass",
                    Visa = new VisaRecord() { Country = "Gondor" }
                });

                Console.WriteLine(firstName);
            }, 10, 100));
            threads.AddRange(CreateWorkers(mres, () =>
            {
                var firstName = "second" + DateTime.Now.Ticks;
                master.Add(new User
                {
                    FirstName = firstName,
                    LastName = "Ivanovich",
                    PersonalId = "Pass",
                    Visa = new VisaRecord() { Country = "Gondor" }
                });

                Console.WriteLine(firstName);
            }, 10, 100));
            threads.AddRange(CreateWorkers(mres, () =>
            {
                var firstOrDefault = master.SearchByVisaCountry("Gondor").FirstOrDefault();
                if (firstOrDefault > 0)
                {
                    var res = master.Delete(firstOrDefault);
                    Console.WriteLine(firstOrDefault + " " + res);
                }
            }, 10, 100));

            foreach (var thread in threads)
            {
                thread.Start();
            }

            Console.WriteLine("Press any key to run unblock working threads.");
            Console.ReadKey();

            mres.Set();

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.WriteLine(slave.SearchByVisaCountry("Gondor")?.Length);

            Console.WriteLine("Press any key.");
            Console.ReadKey();
        }
    }
}
