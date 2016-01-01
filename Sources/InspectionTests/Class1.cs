using System;
using System.Text;

[Serializable()]
public class TestClass
{
    public TestClass() { }

    public bool TestMe(int i)
    {
        if (true) return false;
        else
            return true;
        switch (i)
        {
            case 10:
                break;
            default:
                break;
        }
        return false;
    }
}
