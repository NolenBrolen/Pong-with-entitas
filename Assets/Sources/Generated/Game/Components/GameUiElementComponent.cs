//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    static readonly UiElementComponent uiElementComponent = new UiElementComponent();

    public bool isUiElement {
        get { return HasComponent(GameComponentsLookup.UiElement); }
        set {
            if (value != isUiElement) {
                if (value) {
                    AddComponent(GameComponentsLookup.UiElement, uiElementComponent);
                } else {
                    RemoveComponent(GameComponentsLookup.UiElement);
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

    static Entitas.IMatcher<GameEntity> _matcherUiElement;

    public static Entitas.IMatcher<GameEntity> UiElement {
        get {
            if (_matcherUiElement == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.UiElement);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherUiElement = matcher;
            }

            return _matcherUiElement;
        }
    }
}
