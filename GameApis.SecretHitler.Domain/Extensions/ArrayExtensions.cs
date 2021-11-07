namespace GameApis.SecretHitler.Domain.Extensions;

public static class ArrayExtensions
{
    public static T[] ShuffleRandomly<T>(this T[] array)
    {
        var random = new Random();
        for (var i = 0; i < 100; i++)
        {
            var left = random.Next(array.Length);
            var right = random.Next(array.Length);
            array.SwapIndices(left, right);
        }
        return array;
    }

    public static T[] SwapIndices<T>(this T[] array, int left, int right)
    {
        if (left == right)
        {
            return array;
        }
        var temp = array[left];
        array[left] = array[right];
        array[right] = temp;

        return array;
    }
}
