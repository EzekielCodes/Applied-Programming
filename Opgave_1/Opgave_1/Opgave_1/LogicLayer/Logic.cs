using Globals.Interfaces;

namespace LogicLayer;
public class Logic : ILogic
{
    private readonly IData _data;

    public Logic(IData data)
    {
        _data = data;
    }
}

