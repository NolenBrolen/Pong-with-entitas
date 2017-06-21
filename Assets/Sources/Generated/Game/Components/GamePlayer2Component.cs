//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity player2Entity { get { return GetGroup(GameMatcher.Player2).GetSingleEntity(); } }

    public bool isPlayer2 {
        get { return player2Entity != null; }
        set {
            var entity = player2Entity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isPlayer2 = true;
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

    static readonly Player2Component player2Component = new Player2Component();

    public bool isPlayer2 {
        get { return HasComponent(GameComponentsLookup.Player2); }
        set {
            if (value != isPlayer2) {
                if (value) {
                    AddComponent(GameComponentsLookup.Player2, player2Component);
                } else {
                    RemoveComponent(GameComponentsLookup.Player2);
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

    static Entitas.IMatcher<GameEntity> _matcherPlayer2;

    public static Entitas.IMatcher<GameEntity> Player2 {
        get {
            if (_matcherPlayer2 == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.Player2);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherPlayer2 = matcher;
            }

            return _matcherPlayer2;
        }
    }
}
