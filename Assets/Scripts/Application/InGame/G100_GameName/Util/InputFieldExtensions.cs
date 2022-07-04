using UnityEngine.UI;

namespace BCPG9 {
    /*
        Input Field Custom Extension
        Check Korean string last character is complete word.
    */
    public static class InputFieldExtensions {
#warning Todo: iOS Korean Encoding Support
        public static bool CheckKoreanInputEnd(this InputField field) {
            return CheckKoreanInputUnicode(field.text);
        }

        private static bool CheckKoreanInputUnicode(string text) {
            if (string.IsNullOrEmpty(text))
                return false;
            var lastCode = (int)text[text.Length - 1];
            bool isNotComplete = (lastCode - 0xAC00) % 28 == 0;
            bool isNotWord = lastCode >= 0xAC00 && lastCode <= 0xD7A3;
            if (isNotComplete || !isNotWord)
                return false;
            return true;
        }
    }
}