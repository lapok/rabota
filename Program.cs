using System;
using System.Threading;

class BaboonCanyonSimulation
{
    static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    static int crossingBaboons = 0;
    static object consoleLock = new object();

    static void Main()
    {
        int totalBaboons = 10;
        Thread[] baboons = new Thread[totalBaboons];

        for (int i = 0; i < totalBaboons; i++)
        {
            baboons[i] = new Thread(BaboonCrossing);
            baboons[i].Name = $"Baboon-{i + 1}";
            baboons[i].Start();
        }

        for (int i = 0; i < totalBaboons; i++)
        {
            baboons[i].Join();
        }

        Console.WriteLine("Все бабуины успешно пересекли каньон.");
    }

    static void BaboonCrossing()
    {
        Random rand = new Random();
        int baboonId = int.Parse(Thread.CurrentThread.Name.Split('-')[1]);

        lock (consoleLock)
        {
            Console.WriteLine($"Бабуин {baboonId} подошел к канату.");
        }

        semaphore.Wait();

        lock (consoleLock)
        {
            Console.WriteLine($"Бабуин {baboonId} начал пересекать каньон.");
        }

        crossingBaboons++;

        Thread.Sleep(rand.Next(1000, 3000));

        crossingBaboons--;

        semaphore.Release();

        lock (consoleLock)
        {
            Console.WriteLine($"Бабуин {baboonId} успешно пересек каньон.");
        }
    }
}
