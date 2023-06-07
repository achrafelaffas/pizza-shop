namespace PizzaStore.Utilities
{
    public class RandomStringGenerator
    {
        private static Random random = new Random();
        private const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string GenerateRandomString(int length)
        {
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = characters[random.Next(characters.Length)];
            }

            return new string(result);
        }
    }
}
