using System;
using System.Globalization;
using Xamarin.Forms;

namespace TestTask.Behavior
{
    public class DateValidation : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                if (!DateTime.TryParseExact(e.NewTextValue, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                {
                    entry.TextColor = Color.Red; 
                }
                else
                {
                    entry.TextColor = Color.Black; 
                }
            }
        }
    }
}

