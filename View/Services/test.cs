namespace View.Services
{
    public class Test
    {
        LinkedList<int> list = new LinkedList<int>();
        public void Add(int value)
        {
            list.AddLast(value);
        }
        public void Remove(int value)
        {
            var node = list.First;
            while (node != null)
            {
                if (node.Value == value)
                {
                    list.Remove(node);
                    break;
                }
                node = node.Next;
            }
        }
    }
}
