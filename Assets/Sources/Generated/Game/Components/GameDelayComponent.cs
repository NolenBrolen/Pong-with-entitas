//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity delayEntity { get { return GetGroup(GameMatcher.Delay).GetSingleEntity(); } }

    public bool isDelay {
        get { return delayEntity != null; }
        set {
            var entity = delayEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isDelay = true;
                } else {
                    entity.Destroy();
                }
            }
        }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly DelayComponent delayComponent = new DelayComponent();

    public bool isDelay {
        get { return HasComponent(GameComponentsLookup.Delay); }
        set {
            if (value != isDelay) {
                if (value) {
                    AddComponent(GameComponentsLookup.Delay, delayComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.Delay);
                }
            }
        }
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

    static Entitas.IMatcher<GameEntity> _matcherDelay;

    public static Entitas.IMatcher<GameEntity> Delay {
        get {
            if (_matcherDelay == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Delay);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherDelay = matcher;
            }

            return _matcherDelay;
        }
    }
}