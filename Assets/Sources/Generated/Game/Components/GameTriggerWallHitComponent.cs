//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public TriggerWallHit triggerWallHit { get { return (TriggerWallHit)GetComponent(GameComponentsLookup.TriggerWallHit); } }
    public bool hasTriggerWallHit { get { return HasComponent(GameComponentsLookup.TriggerWallHit); } }

    public void AddTriggerWallHit(bool newLeftWall) {
        var index = GameComponentsLookup.TriggerWallHit;
        var component = CreateComponent<TriggerWallHit>(index);
        component.leftWall = newLeftWall;
        AddComponent(index, component);
    }

    public void ReplaceTriggerWallHit(bool newLeftWall) {
        var index = GameComponentsLookup.TriggerWallHit;
        var component = CreateComponent<TriggerWallHit>(index);
        component.leftWall = newLeftWall;
        ReplaceComponent(index, component);
    }

    public void RemoveTriggerWallHit() {
        RemoveComponent(GameComponentsLookup.TriggerWallHit);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherTriggerWallHit;

    public static Entitas.IMatcher<GameEntity> TriggerWallHit {
        get {
            if (_matcherTriggerWallHit == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.TriggerWallHit);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherTriggerWallHit = matcher;
            }

            return _matcherTriggerWallHit;
        }
    }
}