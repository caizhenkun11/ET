﻿namespace ET
{
    /// <summary>
    /// 对象
    /// </summary>
    public abstract class Object
    {
        public override string ToString()
        {
            return JsonHelper.ToJson(this);
        }
    }
}