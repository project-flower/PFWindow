namespace PFWindow
{
    public class EnumAndStringPair<T>
    {
        #region Public Fields

        public readonly T Instance;
        public readonly string Text;

        #endregion

        #region Public Methods

        public EnumAndStringPair(T instance, string text)
        {
            Instance = instance;
            Text = text;
        }

        public override string ToString()
        {
            return Text;
        }

        #endregion
    }
}
