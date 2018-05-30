namespace CodeRecordHelpers
{
    public interface IMessageDispatcher
    {
        void DispatchMessage(RedisMessage msg);
    }
}