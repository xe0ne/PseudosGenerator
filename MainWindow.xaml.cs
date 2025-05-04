using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace pseudo_generator
{
    public partial class MainWindow : Window
    {
        private static Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            PseudoLengthValue.Text = ((int)PseudoLengthSlider.Value).ToString();
            PseudoLengthSlider.ValueChanged += (s, e) =>
            {
                PseudoLengthValue.Text = ((int)PseudoLengthSlider.Value).ToString();
            };

            NumericTextBox.PreviewTextInput += (s, e) =>
            {
                e.Handled = !int.TryParse(e.Text, out _);
            };

        }
        private void CopyPseudo_Click(object sender, RoutedEventArgs e)
        {
            if (PseudoListBox.SelectedItem != null)
            {
                Clipboard.SetText(PseudoListBox.SelectedItem.ToString());
            }
        }

        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumericTextBox.Text, out int currentValue) && currentValue > 1)
                NumericTextBox.Text = (currentValue - 1).ToString();
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumericTextBox.Text, out int currentValue) && currentValue < 100)
                NumericTextBox.Text = (currentValue + 1).ToString();
        }

        private void GeneratePseudo_Click(object sender, RoutedEventArgs e)
        {
            int minLength = 3;
            int maxLength = (int)PseudoLengthSlider.Value;

            if (!int.TryParse(NumericTextBox.Text, out int numberOfPseudos))
                numberOfPseudos = 1;
            if (numberOfPseudos < 1) numberOfPseudos = 1;
            if (numberOfPseudos > 100) numberOfPseudos = 100;

            NumericTextBox.Text = numberOfPseudos.ToString();

            var pseudos = GeneratePseudos(minLength, maxLength, numberOfPseudos);
            PseudoListBox.Items.Clear();
            foreach (var pseudo in pseudos)
                PseudoListBox.Items.Add(pseudo);
        }

        private List<string> GeneratePseudos(int minLength, int maxLength, int number)
        {
            var vowels = new List<string> { "a", "e", "i", "o", "u" };
            var consonants = new List<string> { "z", "r", "t", "p", "q", "s", "d", "f", "g", "h", "j", "k", "l", "m", "w", "x", "c", "v", "b", "n", "y" };
            var extensions = new List<string> { ".py", ".js", ".fr", ".com", ".xyz", ".uwu", ".io", ".exe", ".org", ".net", ".dev" };
            var suffixes = new List<string> { ".", "_", random.Next(1950, 2025).ToString(), "314" };
            var prefixes = new List<string> { ".", "_" };

            var pseudos = new List<string>();

            for (int i = 0; i < number; i++)
            {
                int length = random.Next(minLength, maxLength + 1);
                List<string> pseudo;

                while (true)
                {
                    pseudo = new List<string>();
                    for (int j = 0; j < length; j++)
                        pseudo.Add(random.Next(0, 2) == 0 ? vowels[random.Next(vowels.Count)] : consonants[random.Next(consonants.Count)]);

                    bool valid = true;
                    for (int j = 0; j < pseudo.Count - 1; j++)
                    {
                        if (pseudo[j] == pseudo[j + 1] ||
                            (vowels.Contains(pseudo[j]) && vowels.Contains(pseudo[j + 1])) ||
                            (consonants.Contains(pseudo[j]) && consonants.Contains(pseudo[j + 1]) && random.Next(0, 64) > random.Next(1, 10)))
                        {
                            valid = false;
                            break;
                        }
                    }
                    if (valid) break;
                }

                string pseudoStr = string.Join("", pseudo).ToLower();
                bool character = true;
                string numberTag = random.Next(1, 9).ToString().PadLeft(random.Next(1, 5), '0');

                if (random.Next(1, 9) == 1)
                {
                    if (random.Next(1, 5) == 1)
                    {
                        pseudoStr += random.Next(0, 2) == 0 ? "." : "_";
                        character = false;
                    }
                    pseudoStr += suffixes[random.Next(suffixes.Count)];
                }
                else if (random.Next(1, 8) == 1)
                {
                    pseudoStr += numberTag;
                }
                else if (random.Next(1, 12) == 1 && character)
                {
                    pseudoStr += extensions[random.Next(extensions.Count)];
                }
                else if (random.Next(1, 8) == 1 && character)
                {
                    pseudoStr = prefixes[random.Next(prefixes.Count)] + pseudoStr;
                }
                else if (random.Next(1, 16) == 1)
                {
                    int insert = random.Next(1, pseudoStr.Length);
                    pseudoStr = pseudoStr.Substring(0, insert) + "_" + pseudoStr.Substring(insert);
                }
                else if (random.Next(1, 24) == 1)
                {
                    var replace = new Dictionary<char, char> { { 'a', '4' }, { 'e', '3' }, { 'i', '1' }, { 'o', '0' } };
                    pseudoStr = string.Concat(pseudoStr.Select(c => replace.ContainsKey(c) ? replace[c] : c));
                }

                pseudos.Add(pseudoStr);
            }

            return pseudos;
        }
    }
}
