//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentContextGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameContext {

    public GameEntity scoreLeftEntity { get { return GetGroup(GameMatcher.ScoreLeft).GetSingleEntity(); } }

    public bool isScoreLeft {
        get { return scoreLeftEntity != null; }
        set {
            var entity = scoreLeftEntity;
            if (value != (entity != null)) {
                if (value) {
                    CreateEntity().isScoreLeft = true;
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

    static readonly ScoreLeft scoreLeftComponent = new ScoreLeft();

    public bool isScoreLeft {
        get { return HasComponent(GameComponentsLookup.ScoreLeft); }
        set {
            if (value != isScoreLeft) {
                if (value) {
                    AddComponent(GameComponentsLookup.ScoreLeft, scoreLeftComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.ScoreLeft);
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

    static Entitas.IMatcher<GameEntity> _matcherScoreLeft;

    public static Entitas.IMatcher<GameEntity> ScoreLeft {
        get {
            if (_matcherScoreLeft == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.ScoreLeft);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherScoreLeft = matcher;
            }

            return _matcherScoreLeft;
        }
    }
}
