using System;

namespace ET.Server
{
    /// <summary>
    /// 信箱组件激活系统
    /// </summary>
    [ObjectSystem]
    public class MailBoxComponentAwakeSystem : AwakeSystem<MailBoxComponent>
    {
        protected override void Awake(MailBoxComponent self)
        {
            self.MailboxType = MailboxType.MessageDispatcher;
        }
    }
    /// <summary>
    /// 信箱组件激活系统1
    /// </summary>
    [ObjectSystem]
    public class MailBoxComponentAwake1System : AwakeSystem<MailBoxComponent, MailboxType>
    {
        protected override void Awake(MailBoxComponent self, MailboxType mailboxType)
        {
            self.MailboxType = mailboxType;
        }
    }
}