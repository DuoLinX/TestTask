using Xamarin.Forms;

namespace TestTask.Behavior
{
    public class ProductNameValidation : Behavior<Entry>
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
                if (e.NewTextValue.Length < 5)
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