using System;
using System.IO;
using System.Threading;

class Program
{
    static int[] arrayA;
    static int[] arrayC;
    static int result = 0;
    static object lockObject = new object();

    static void Main(string[] args)
    {
        // Обработка входных данных
        int numThreads = 5; // Значение по умолчанию

        if (args.Length > 0)
        {
            if (int.TryParse(args[0], out numThreads) && numThreads >= 2 && numThreads <= 20)
            {
                // Валидное значение числа потоков
            }
            else
            {
                Console.WriteLine("Указано некорректное число потоков");
                return;
            }
        }

        // Считывание массивов
        Console.WriteLine("Введите элементы массива A:");
        arrayA = ReadIntArray();

        Console.WriteLine("Введите элементы массива C:");
        arrayC = ReadIntArray();

        // Создание и запуск потоков
        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int startIdx = i * arrayA.Length / numThreads;
            int endIdx = (i + 1) * arrayA.Length / numThreads;

            threads[i] = new Thread(() => CompareAndCount(startIdx, endIdx));
            threads[i].Start();
        }

        // Ожидание завершения потоков
        foreach (var thread in threads)
        {
            thread.Join();
        }

        // Вывод окончательного результата
        Console.WriteLine($"Количество пар, где элемент ai превосходит ci: {result}");
    }

    static void CompareAndCount(int startIdx, int endIdx)
    {
        for (int i = startIdx; i < endIdx; i++)
        {
            if (arrayA[i] > arrayC[i])
            {
                lock (lockObject)
                {
                    result++;
                }
            }
        }
    }

    static int[] ReadIntArray()
    {
        string input = Console.ReadLine();

        // Можно добавить обработку ошибок при вводе
        return Array.ConvertAll(input.Split(' '), int.Parse);
    }
}
