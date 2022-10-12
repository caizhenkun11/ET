namespace ET
{
    /// <summary>
    /// 控制台处理
    /// </summary>
    public interface IConsoleHandler
    {
        ETTask Run(ModeContex contex, string content);
    }
}