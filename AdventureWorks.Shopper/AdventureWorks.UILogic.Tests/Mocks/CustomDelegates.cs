

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public static class CustomDelegates
    {
        public delegate void InOutOutAction<T, TOut1, TOut2>(T in1, out TOut1 out1, out TOut2 out2);
    }
}
