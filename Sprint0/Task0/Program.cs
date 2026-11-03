using System;
using System.Collections.Generic;
using System.Linq;

int[] numbers = { 5, 8, 1, 3, 9, 6, 7, 2 };
int[] sorted = numbers.OrderBy(x => x).ToArray();
int searched = 7;
Console.WriteLine($"Минимум: {numbers.Min()}, максимум: {numbers.Max()}");
Console.WriteLine($"Сумма: {numbers.Sum()}, среднее: {numbers.Average()}");
Console.WriteLine(string.Join(", ", sorted));
Console.WriteLine($"Есть {searched}: {numbers.Contains(searched)}");
Verify.That(numbers.Min() == 1 && numbers.Max() == 9, "Границы массива");
Verify.That(numbers.Sum() == 41 && numbers.Average() == 5.125, "Агрегаты");
Verify.That(sorted.SequenceEqual(new[] {1, 2, 3, 5, 6, 7, 8, 9}), "Сортировка");
Verify.That(numbers.Contains(7) && !numbers.Contains(4), "Поиск");
Console.WriteLine("Проверки выполнены успешно.");



public static class Verify
{
    public static void That(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }

    public static void Throws<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException($"Ожидалось исключение {typeof(T).Name}");
    }
}
