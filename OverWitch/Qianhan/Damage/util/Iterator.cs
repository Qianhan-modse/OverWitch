using Entitying;
using UnityEditor.SearchService;
using Valuitem;

public interface Iterator<E>
{
    bool hasNext();

    E next();

    public void remove()
    {
        throw new UnsupportedOperationException("remove");
    }

    public void forEachRemaining(Consumer<T, E>action)
    {
        Object.RequireNonNull(action);
        while(hasNext())
        {
            action.accept(next());
        }
    }
}
