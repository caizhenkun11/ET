namespace ET.Client
{
    /// <summary>
    /// 单元助手
    /// </summary>
    public static class UnitHelper
    {
        public static Unit GetMyUnitFromClientScene(Scene clientScene)
        {
            PlayerComponent playerComponent = clientScene.GetComponent<PlayerComponent>();
            Scene currentScene = clientScene.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }

        public static Unit GetMyUnitFromCurrentScene(Scene currentScene)
        {
            PlayerComponent playerComponent = currentScene.Parent.GetParent<Scene>().GetComponent<PlayerComponent>();
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }
    }
}