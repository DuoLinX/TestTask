using Xamarin.Forms;

namespace TestTask.Behavior
{
    public class QuantityValidation : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            base.OnAttachedTo(entry);
            entry.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            base.OnDetachingFrom(entry);
            entry.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                string newText = e.NewTextValue;

                if (IsValidDecimal(newText))
                {
                    entry.Text = newText;
                }
                else
                {
                    entry.Text = e.OldTextValue; 
                }
            }
        }

        private bool IsValidDecimal(string text, char separator = ',')
        {
            if (string.IsNullOrEmpty(text))
                return true;
                        
            int separatorCount = 0;

            foreach (char c in text)
            {
                if (char.IsDigit(c))
                {
                    continue;
                }
                else if (c == separator)
                {
                    separatorCount++;
                    if (separatorCount > 1)
                        return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}