namespace System {
    public static class StringExtensions {
        public static string GetFirstChars(this string value, int length = 1) {
            var words = value.Split(' ');
            string result = "";

            foreach (var word in words)
                result += word.Substring(0, length);


            return result;
        }
    }
}
